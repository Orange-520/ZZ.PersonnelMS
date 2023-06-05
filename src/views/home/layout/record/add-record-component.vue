<template>
  <div class="add-record-component">
    <!-- 顶部搜索 -->
    <div class="top-box">
      <span>新增</span>
      <span
        class="pointer s_close"
        @click="
          $router.push({ path: '/home/record/recordList/recordListComponent' })
        "
      >
        <i class="el-icon-close"></i>
      </span>
    </div>

    <!-- 登记表 -->
    <div class="content">
      <div class="title">
        <div>入职信息登记</div>
      </div>

      <div class="content-box">
        <div class="min-title">
          <div></div>
          <div>基本信息</div>
          <div></div>
        </div>

        <!-- 表单 -->
        <el-form
          class="ruleForm"
          ref="ruleForm"
          :model="ruleForm"
          :rules="rules"
          label-width="140px"
          size="small"
        >
          <el-row class="row1">
            <div
              class="card"
              v-for="(item, index) in formConfig1"
              :key="index"
            >
              <el-form-item
                :key="item.prop + index"
                :prop="item.prop"
                :label="item.label"
              >
                <template v-if="item.type === 'input'">
                  <el-input v-model="ruleForm[item.prop]" />
                </template>

                <template v-if="item.type === 'select'">
                  <el-select
                    v-model="ruleForm[item.prop]"
                    style="width: 100%"
                    @change="clearValidate('ruleForm', item.prop)"
                  >
                    <el-option
                      v-for="option in item.optionsList"
                      :key="option.value"
                      :label="option.label"
                      :value="option.value"
                    >
                    </el-option>
                  </el-select>
                </template>

                <template v-if="item.type === 'date-picker'">
                  <el-date-picker
                    v-model="ruleForm[item.prop]"
                    type="date"
                    placeholder="选择日期"
                    value-format="yyyy-MM-dd"
                    style="width: 100%"
                  >
                  </el-date-picker>
                </template>

                <template v-if="item.type === 'cascader'">
                  <el-cascader
                    style="width: 100%"
                    ref="elCascader"
                    v-model="ruleForm[item.prop]"
                    :options="item.optionsList"
                    :props="item.cascaderProps"
                    @change="changeDepartment"
                  >
                  </el-cascader>
                </template>

                <template v-if="item.type === 'image'">
                  <el-upload
                    class="avatar-uploader"
                    :action="baseURL + fileUploadURL"
                    :show-file-list="false"
                    :on-success="handleAvatarSuccess" 
                    :before-upload="beforeAvatarUpload"
                    :on-error="handleAvatarError"
                    :on-progress="handleAvatarProgress"
                  >
                    <el-image
                      v-if="ruleForm.avatar"
                      :src="baseURL +'/image/'+ruleForm.avatar"
                      fit="fit"
                      class="avatar"
                    >
                    </el-image>
                    <i v-else class="el-icon-plus avatar-uploader-icon"></i>
                  </el-upload>
                </template>
              </el-form-item>
            </div>
          </el-row>

          <el-row>
            <template v-for="(item, index) in formConfig2">
              <el-col :span="item.span" :key="index">
                <el-form-item
                  :key="item.prop + index"
                  :prop="item.prop"
                  :label="item.label"
                >
                  <template v-if="item.type === 'input'">
                    <el-input v-model="ruleForm[item.prop]" />
                  </template>

                  <template v-if="item.type === 'textarea'">
                    <el-input
                      v-model="ruleForm[item.prop]"
                      type="textarea"
                      :rows="2"
                    />
                  </template>

                  <template v-if="item.type === 'select'">
                    <el-select
                      v-model="ruleForm[item.prop]"
                      style="width: 100%"
                      @change="clearValidate('ruleForm', item.prop)"
                    >
                      <el-option
                        v-for="option in item.optionsList"
                        :key="option.value"
                        :label="option.label"
                        :value="option.value"
                      >
                      </el-option>
                    </el-select>
                  </template>

                  <template v-if="item.type === 'date-picker'">
                    <el-date-picker
                      v-model="ruleForm[item.prop]"
                      type="date"
                      placeholder="选择日期"
                      value-format="yyyy-MM-dd"
                      style="width: 100%"
                    >
                    </el-date-picker>
                  </template>

                  <template v-if="item.type === 'cascader'">
                    <el-cascader
                      style="width: 100%"
                      ref="elCascader"
                      v-model="ruleForm[item.prop]"
                      :options="item.optionsList"
                      :props="item.cascaderProps"
                      @change="changeDepartment"
                    >
                    </el-cascader>
                  </template>

                </el-form-item>
              </el-col>
            </template>
          </el-row>
        </el-form>

        <div class="min-title" style="margin-top: 20px">
          <div></div>
          <div>
            工作经历
            <el-button
              type="primary"
              size="mini"
              @click="workHistoryDialogVisible = true"
              >新增工作经历</el-button
            >
          </div>
          <div></div>
        </div>

        <!-- 新增工作经历对话框 -->
        <el-dialog
          title="添加工作经历"
          :visible.sync="workHistoryDialogVisible"
          center
          width="400px"
          @close="closeDialog"
          :modal-append-to-body="false"
          top="6vh"
        >
          <div class="dialog_content">
            <el-form
              :model="workHistoryRuleForm"
              :rules="workHistoryRules"
              :hide-required-asterisk="false"
              ref="workHistoryRuleForm"
              label-width="auto"
              label-position="left"
            >
              <el-form-item label="开始日期" prop="startTime">
                <el-date-picker
                  v-model="workHistoryRuleForm.startTime"
                  type="date"
                  value-format="yyyy-MM-dd"
                  :editable="false"
                  placeholder="选择日期"
                  style="width: 100%"
                >
                </el-date-picker>
              </el-form-item>

              <el-form-item label="结束日期" prop="endTime">
                <el-date-picker
                  v-model="workHistoryRuleForm.endTime"
                  type="date"
                  value-format="yyyy-MM-dd"
                  :editable="false"
                  placeholder="选择日期"
                  style="width: 100%"
                >
                </el-date-picker>
              </el-form-item>

              <el-form-item label="公司名称" prop="companyName">
                <el-input v-model="workHistoryRuleForm.companyName"></el-input>
              </el-form-item>

              <el-form-item label="所在地址" prop="companyAddress">
                <el-input
                  v-model="workHistoryRuleForm.companyAddress"
                ></el-input>
              </el-form-item>

              <el-form-item label="曾任职位" prop="position">
                <el-input v-model="workHistoryRuleForm.position"></el-input>
              </el-form-item>

              <el-form-item label="离职原因" prop="dimissionCause">
                <el-input
                  v-model="workHistoryRuleForm.dimissionCause"
                  type="textarea"
                  :rows="2"
                />
              </el-form-item>

              <el-form-item label="最大收获" prop="biggestGain">
                <el-input
                  v-model="workHistoryRuleForm.biggestGain"
                  type="textarea"
                  :rows="2"
                />
              </el-form-item>
            </el-form>
          </div>

          <div slot="footer" class="dialog-footer">
            <el-button @click="workHistoryDialogVisible = false"
              >取 消</el-button
            >
            <el-button @click="reset('workHistoryRuleForm')">重 置</el-button>
            <el-button
              type="primary"
              @click="addWorkHistory('workHistoryRuleForm')"
              >添 加</el-button
            >
          </div>
        </el-dialog>

        <!-- 工作经历表格 -->
        <div class="workHistory table-box">
          <el-table
            class=""
            :data="ruleForm.minWorkHistory"
            border
            style="width: 100%"
            :fit="true"
            :cell-style="{ 'text-align': 'center', width: 'auto' }"
            :header-cell-style="{ 'text-align': 'center' }"
          >
            <el-table-column label="" width="70" align="left">
              <template slot-scope="scope">
                {{ scope.$index + 1 }}
              </template>
            </el-table-column>

            <el-table-column
              prop="startTime"
              width="120"
              label="开始日期"
            ></el-table-column>

            <el-table-column
              prop="endTime"
              width="120"
              label="结束日期"
            ></el-table-column>

            <el-table-column
              prop="companyName"
              label="公司名称"
            ></el-table-column>

            <el-table-column
              prop="companyAddress"
              label="所在地址"
            ></el-table-column>

            <el-table-column prop="position" label="职位"></el-table-column>

            <el-table-column
              prop="dimissionCause"
              label="离职原因"
            ></el-table-column>

            <el-table-column
              prop="biggestGain"
              label="最大收获"
            ></el-table-column>

            <el-table-column fixed="right" label="操作" width="200px">
              <template slot-scope="scope">
                <el-button
                  type="primary"
                  size="small"
                  @click="showDialogRepertory(scope.row)"
                  >修改</el-button
                >
                <el-button
                  type="danger"
                  size="small"
                  @click="showDialogRepertory(scope.row)"
                  >删除</el-button
                >
              </template>
            </el-table-column>
          </el-table>
        </div>

        <div class="min-title" style="margin-top: 20px">
          <div></div>
          <div>
            教育经历
            <el-button
              type="primary"
              size="mini"
              @click="educationHistoryDialogVisible = true"
              >新增教育经历</el-button
            >
          </div>
          <div></div>
        </div>

        <!-- 新增教育经历对话框 -->
        <el-dialog
          title="添加教育经历"
          :visible.sync="educationHistoryDialogVisible"
          center
          width="400px"
          @close="closeDialog"
          :modal-append-to-body="false"
          top="2vh"
          class="educationHistoryDialog"
        >
          <div class="dialog_content">
            <el-form
              :model="educationHistoryRuleForm"
              :rules="educationHistoryRules"
              :hide-required-asterisk="false"
              ref="educationHistoryRuleForm"
              label-width="auto"
              label-position="left"
            >
              <template v-for="(item, index) in educationHistoryRuleFormConfig">
                <el-form-item
                  :label="item.label"
                  :prop="item.prop"
                  :key="index"
                >
                  <template v-if="item.type == 'input'">
                    <el-input
                      v-model="educationHistoryRuleForm[item.prop]"
                    ></el-input>
                  </template>

                  <template v-if="item.type === 'textarea'">
                    <el-input
                      v-model="educationHistoryRuleForm[item.prop]"
                      type="textarea"
                      :rows="2"
                    />
                  </template>

                  <template v-if="item.type === 'select'">
                    <el-select
                      v-model="educationHistoryRuleForm[item.prop]"
                      style="width: 100%"
                      @change="
                        clearValidate('educationHistoryRuleForm', item.prop)
                      "
                    >
                      <el-option
                        v-for="option in item.optionsList"
                        :key="option.value"
                        :label="option.label"
                        :value="option.value"
                      >
                      </el-option>
                    </el-select>
                  </template>

                  <template v-if="item.type === 'date-picker'">
                    <el-date-picker
                      v-model="educationHistoryRuleForm[item.prop]"
                      type="date"
                      placeholder="选择日期"
                      value-format="yyyy-MM-dd"
                      style="width: 100%"
                    >
                    </el-date-picker>
                  </template>

                  <template v-if="item.type === 'input-number'">
                    <el-input-number
                      style="width: 100%"
                      v-model="educationHistoryRuleForm[item.prop]"
                      controls-position="right"
                      :precision="2"
                      :min="0"
                    ></el-input-number>
                  </template>
                </el-form-item>
              </template>
            </el-form>
          </div>

          <div slot="footer" class="dialog-footer">
            <el-button @click="educationHistoryDialogVisible = false"
              >取 消</el-button
            >
            <el-button @click="reset('educationHistoryRuleForm')"
              >重 置</el-button
            >
            <el-button
              type="primary"
              @click="addEducationHistory('educationHistoryRuleForm')"
              >添 加</el-button
            >
          </div>
        </el-dialog>

        <!-- 教育经历表格 -->
        <div class="educationHistory table-box">
          <el-table
            class=""
            :data="ruleForm.minEducationHistory"
            border
            style="width: 100%"
            :fit="true"
            :cell-style="{ 'text-align': 'center', width: 'auto' }"
            :header-cell-style="{ 'text-align': 'center' }"
          >
            <el-table-column label="" width="70" align="left">
              <template slot-scope="scope">
                {{ scope.$index + 1 }}
              </template>
            </el-table-column>

            <el-table-column
              prop="schoolName"
              label="学校名称"
            ></el-table-column>

            <el-table-column
              prop="currentEducation"
              label="学历"
              :formatter="educationHistoryTableFormatter"
            ></el-table-column>

            <el-table-column
              width="120"
              prop="startTime"
              label="入学时间"
            ></el-table-column>

            <el-table-column
              width="120"
              prop="endTime"
              label="毕业时间"
            ></el-table-column>

            <el-table-column
              prop="learningStyle"
              label="学习方式"
              :formatter="educationHistoryTableFormatter"
            ></el-table-column>

            <el-table-column prop="score" label="学习成绩"></el-table-column>

            <el-table-column prop="major" label="专业"></el-table-column>

            <el-table-column
              prop="degree"
              label="学位"
              :formatter="educationHistoryTableFormatter"
            ></el-table-column>

            <el-table-column
              prop="degreeCreateTime"
              label="学位授予时间"
              width="120"
            ></el-table-column>

            <el-table-column prop="remark" label="备注"></el-table-column>

            <el-table-column fixed="right" label="操作" width="200px">
              <template slot-scope="scope">
                <el-button
                  type="primary"
                  size="small"
                  @click="showDialogRepertory(scope.row)"
                  >修改</el-button
                >
                <el-button
                  type="danger"
                  size="small"
                  @click="showDialogRepertory(scope.row)"
                  >删除</el-button
                >
              </template>
            </el-table-column>
          </el-table>
        </div>

        <div class="min-title" style="margin-top: 20px">
          <div></div>
          <div>
            获奖经历
            <el-button
              type="primary"
              size="mini"
              @click="certificateDialogVisible = true"
              >新增获奖经历</el-button
            >
          </div>
          <div></div>
        </div>

        <!-- 新增获奖经历对话框 -->
        <el-dialog
          title="添加获奖经历"
          :visible.sync="certificateDialogVisible"
          center
          width="400px"
          @close="closeDialog"
          :modal-append-to-body="false"
          top="10vh"
        >
          <div class="dialog_content">
            <el-form
              :model="certificateRuleForm"
              :rules="certificateRules"
              :hide-required-asterisk="false"
              ref="certificateRuleForm"
              label-width="auto"
              label-position="left"
            >
              <template v-for="(item, index) in certificateRuleFormConfig">
                <el-form-item
                  :label="item.label"
                  :prop="item.prop"
                  :key="index"
                >
                  <template v-if="item.type == 'input'">
                    <el-input
                      v-model="certificateRuleForm[item.prop]"
                    ></el-input>
                  </template>

                  <template v-if="item.type === 'textarea'">
                    <el-input
                      v-model="certificateRuleForm[item.prop]"
                      type="textarea"
                      :rows="2"
                    />
                  </template>

                  <template v-if="item.type === 'select'">
                    <el-select
                      v-model="certificateRuleForm[item.prop]"
                      style="width: 100%"
                      @change="clearValidate('certificateRuleForm', item.prop)"
                    >
                      <el-option
                        v-for="option in item.optionsList"
                        :key="option.value"
                        :label="option.label"
                        :value="option.value"
                      >
                      </el-option>
                    </el-select>
                  </template>

                  <template v-if="item.type === 'date-picker'">
                    <el-date-picker
                      v-model="certificateRuleForm[item.prop]"
                      type="date"
                      placeholder="选择日期"
                      value-format="yyyy-MM-dd"
                      style="width: 100%"
                    >
                    </el-date-picker>
                  </template>
                </el-form-item>
              </template>
            </el-form>
          </div>

          <div slot="footer" class="dialog-footer">
            <el-button @click="certificateDialogVisible = false"
              >取 消</el-button
            >
            <el-button @click="reset('certificateRuleForm')">重 置</el-button>
            <el-button
              type="primary"
              @click="addCertificate('certificateRuleForm')"
              >添 加</el-button
            >
          </div>
        </el-dialog>

        <!-- 获奖经历表格 -->
        <div class="certificate table-box">
          <el-table
            class=""
            :data="ruleForm.minCertificate"
            border
            style="width: 100%"
            :fit="true"
            :cell-style="{ 'text-align': 'center', width: 'auto' }"
            :header-cell-style="{ 'text-align': 'center' }"
          >
            <el-table-column label="" width="70" align="left">
              <template slot-scope="scope">
                {{ scope.$index + 1 }}
              </template>
            </el-table-column>

            <el-table-column prop="name" label="证书名称"></el-table-column>

            <el-table-column
              prop="getTime"
              width="120"
              label="获取时间"
            ></el-table-column>

            <el-table-column prop="level" label="等级"></el-table-column>

            <el-table-column
              prop="certificateNumber"
              label="证书编号"
            ></el-table-column>

            <el-table-column
              prop="organization"
              label="颁发机构"
            ></el-table-column>

            <el-table-column prop="remark" label="备注"></el-table-column>

            <el-table-column fixed="right" label="操作" width="200px">
              <template slot-scope="scope">
                <el-button
                  type="primary"
                  size="small"
                  @click="showDialogRepertory(scope.row)"
                  >修改</el-button
                >
                <el-button
                  type="danger"
                  size="small"
                  @click="showDialogRepertory(scope.row)"
                  >删除</el-button
                >
              </template>
            </el-table-column>
          </el-table>
        </div>

        <div class="result">
          <el-button
            type="primary"
            class="result-btn"
            @click="addRecord('ruleForm')"
            >登记入职档案信息</el-button
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { AddRecord } from "@/https/join-us/add.js";
import { GetAllDepartment, FindPositionByDepartment } from "@/https/commons.js";
import {isJPGAndIsLt2M} from '@/assets/js/commonFunc.js';
import { mapState } from 'vuex';
export default {
  data() {
    const formConfig1 = [
      { label: "姓名", prop: "name", type: "input", span: 8 },
      { label: "账号", prop: "userName", type: "input", span: 8 },
      {
        label: "部门",
        prop: "departmentId",
        type: "cascader",
        span: 8,
        optionsList: [],
        // 部门级联选择器配置项
        cascaderProps: {
          expandTrigger: "click",
          value: "id",
          label: "name",
          children: "childrenDepartment",
          checkStrictly: true,
        },
      },
      { label: "入职时间", prop: "entryTime", type: "date-picker", span: 8 },
      { label: "密码", prop: "password", type: "input", span: 8 },

      {
        label: "职位",
        prop: "positionId",
        type: "select",
        span: 8,
        optionsList: [],
      },
      { label: "相片", prop: "avatar", type: "image", span: 8 },
    ];
    const formConfig2 = [
      { label: "手机号码", prop: "phoneNumber", type: "input", span: 8 },
      { label: "电子邮件", prop: "email", type: "input", span: 8 },
      { label: "身份证号码", prop: "idCard", type: "input", span: 8 },
      {
        label: "性别",
        prop: "gender",
        type: "select",
        span: 8,
        optionsList: [
          {
            value: 1,
            label: "男",
          },
          {
            value: 2,
            label: "女",
          },
        ],
      },
      { label: "出生日期", prop: "birthday", type: "date-picker", span: 8 },
      {
        label: "兴趣特长",
        prop: "interestsAndTalents",
        type: "input",
        span: 8,
      },
      { label: "籍贯", prop: "nativePlace", type: "input", span: 8 },
      {
        label: "政治面貌",
        prop: "politicsStatus",
        type: "select",
        span: 8,
        optionsList: [
          {
            value: 1,
            label: "中共党员",
          },
          {
            value: 2,
            label: "中共预备党员",
          },
          {
            value: 3,
            label: "共青团员",
          },
          {
            value: 4,
            label: "民革党员",
          },
          {
            value: 5,
            label: "民盟盟员",
          },
          {
            value: 6,
            label: "民建会员",
          },
          {
            value: 7,
            label: "民进会员",
          },
          {
            value: 8,
            label: "农工党党员",
          },
          {
            value: 9,
            label: "致公党党员",
          },
          {
            value: 10,
            label: "九三学社社员",
          },
          {
            value: 11,
            label: "台盟盟员",
          },
          {
            value: 12,
            label: "无党派人士",
          },
          {
            value: 13,
            label: "群众",
          },
        ],
      },
      {
        label: "求职方式",
        prop: "jobHuntingMode",
        type: "select",
        span: 8,
        optionsList: [
          {
            value: 1,
            label: "网上招聘",
          },
          {
            value: 2,
            label: "校园招聘",
          },
          {
            value: 3,
            label: "专场招聘",
          },
          {
            value: 4,
            label: "亲戚朋友推荐",
          },
        ],
      },
      { label: "毕业院校", prop: "schoolOfGraduation", type: "input", span: 8 },
      {
        label: "学历",
        prop: "currentEducation",
        type: "select",
        span: 8,
        optionsList: [
          {
            value: 1,
            label: "小学",
          },
          {
            value: 2,
            label: "初中",
          },
          {
            value: 3,
            label: "高中",
          },
          {
            value: 4,
            label: "大专",
          },
          {
            value: 5,
            label: "大学本科",
          },
          {
            value: 6,
            label: "研究生",
          },
        ],
      },
      { label: "专业", prop: "major", type: "input", span: 8 },
      {
        label: "婚姻状况",
        prop: "maritalStatus",
        type: "select",
        span: 8,
        optionsList: [
          {
            value: 1,
            label: "已婚",
          },
          {
            value: 2,
            label: "未婚",
          },
        ],
      },

      { label: "健康状况", prop: "healthCondition", type: "input", span: 8 },
      { label: "语言能力", prop: "languageCompetence", type: "input", span: 8 },
      {
        label: "掌握技能",
        prop: "professionalSkill",
        type: "textarea",
        span: 24,
      },
      { label: "现居住地", prop: "currentAddress", type: "textarea", span: 24 },
      { label: "紧急联系人", prop: "emergencyContact", type: "input", span: 8 },
      { label: "与其关系", prop: "relationshipWith", type: "input", span: 8 },
      {
        label: "紧急联系人号码",
        prop: "emergencyContactPhoneNumber",
        type: "input",
        span: 8,
      },
      {
        label: "用工性质",
        prop: "natureOfEmployment",
        type: "select",
        span: 8,
        optionsList: [
          {
            label: "正式员工",
            value: 1,
          },
          {
            label: "临时工",
            value: 2,
          },
          {
            label: "实习生",
            value: 3,
          },
        ],
      },
      {
        label: "身份证复印件",
        prop: "idCardPicture",
        type: "input",
        span: 16,
      },
      {
        label: "转正日期",
        prop: "dateOfConfirmationAfterProbation",
        type: "date-picker",
        span: 8,
      },
      {
        label: "购买社保类型",
        prop: "typeOfSocialSecurity",
        type: "input",
        span: 8,
      },
      {
        label: "社保卡号",
        prop: "socialSecurityCardNumber",
        type: "input",
        span: 8,
      },
    ];
    const educationHistoryRuleFormConfig = [
      { label: "学校名称", prop: "schoolName", type: "input", span: 8 },
      {
        label: "当前学历",
        prop: "currentEducation",
        type: "select",
        span: 8,
        optionsList: [
          {
            label: "小学",
            value: 1,
          },
          {
            label: "初中",
            value: 2,
          },
          {
            label: "高中",
            value: 3,
          },
          {
            label: "大专",
            value: 4,
          },
          {
            label: "大学本科",
            value: 5,
          },
          {
            label: "研究生",
            value: 6,
          },
        ],
      },
      { label: "入学时间", prop: "startTime", type: "date-picker", span: 8 },
      { label: "毕业时间", prop: "endTime", type: "date-picker", span: 8 },
      {
        label: "学习方式",
        prop: "learningStyle",
        type: "select",
        span: 8,
        optionsList: [
          {
            label: "全日制",
            value: 1,
          },
          {
            label: "在职",
            value: 2,
          },
          {
            label: "自考",
            value: 3,
          },
        ],
      },
      { label: "学习成绩", prop: "score", type: "input-number", span: 8 },
      { label: "专业", prop: "major", type: "input", span: 8 },
      {
        label: "学位",
        prop: "degree",
        type: "select",
        span: 8,
        optionsList: [
          {
            label: "无",
            value: 0,
          },
          {
            label: "学士",
            value: 1,
          },
          {
            label: "硕士",
            value: 2,
          },
          {
            label: "博士",
            value: 3,
          },
        ],
      },

      {
        label: "学位授予时间",
        prop: "degreeCreateTime",
        type: "date-picker",
        span: 8,
      },
      { label: "备注", prop: "remark", type: "textarea", span: 8 },
    ];
    const certificateRuleFormConfig = [
      { label: "证书名称", prop: "name", type: "input", span: 8 },
      { label: "获得时间", prop: "getTime", type: "date-picker", span: 8 },
      { label: "级别", prop: "level", type: "input", span: 8 },
      { label: "证书编号", prop: "certificateNumber", type: "input", span: 8 },
      { label: "颁发机构", prop: "organization", type: "input", span: 8 },
      { label: "备注", prop: "remark", type: "textarea", span: 8 },
    ];
    const initRuleForm = {
        userName: "",
        password: "",
        departmentId: null,
        positionId: null,
        entryTime: "",
        avatar: "",
        natureOfEmployment: null,
        idCardPicture: "",
        dateOfConfirmationAfterProbation: "",
        typeOfSocialSecurity: "",
        socialSecurityCardNumber: "",
        minWorkHistory: [],
        minEducationHistory: [],
        minCertificate: [],
        name: "",
        gender: null,
        phoneNumber: "",
        nativePlace: "",
        idCard: "",
        maritalStatus: null,
        email: "",
        birthday: "",
        politicsStatus: null,
        schoolOfGraduation: "",
        currentEducation: null,
        major: "",
        healthCondition: "",
        languageCompetence: "",
        jobHuntingMode: null,
        interestsAndTalents: "",
        professionalSkill: "",
        currentAddress: "",
        emergencyContact: "",
        relationshipWith: "",
        emergencyContactPhoneNumber: "",
      };
    const initWorkHistoryRuleForm = {
      startTime: "",
      endTime: "",
      companyName: "",
      companyAddress: "",
      position: "",
      dimissionCause: "",
      biggestGain: "",
    };
    const initEducationHistoryRuleForm = {
      schoolName: "",
      currentEducation: "",
      major: "",
      startTime: "",
      endTime: "",
      degree: 0,
      degreeCreateTime: "",
      learningStyle: 1,
      score: 0,
      remark: "",
    };
    const initCertificateRuleForm = {
      name: "",
      getTime: "",
      level: "",
      certificateNumber: "",
      organization: "",
      remark: "",
    };
    return {
      // 页面渲染配置项
      formConfig1,
      formConfig2,
      educationHistoryRuleFormConfig,
      certificateRuleFormConfig,

      // 对话框是否显示
      workHistoryDialogVisible: false,
      educationHistoryDialogVisible: false,
      certificateDialogVisible: false,

      // 初始化表单项，用以将表单项恢复至初始状态
      initRuleForm,
      initWorkHistoryRuleForm,
      initEducationHistoryRuleForm,
      initCertificateRuleForm,

      // 表单配置项
      ruleForm: {
        userName: "",
        password: "",
        departmentId: null,
        positionId: null,
        entryTime: "",
        avatar: "",
        natureOfEmployment: null,
        idCardPicture: "",
        dateOfConfirmationAfterProbation: "",
        typeOfSocialSecurity: "",
        socialSecurityCardNumber: "",
        minWorkHistory: [],
        minEducationHistory: [],
        minCertificate: [],
        name: "",
        gender: null,
        phoneNumber: "",
        nativePlace: "",
        idCard: "",
        maritalStatus: null,
        email: "",
        birthday: "",
        politicsStatus: null,
        schoolOfGraduation: "",
        currentEducation: null,
        major: "",
        healthCondition: "",
        languageCompetence: "",
        jobHuntingMode: null,
        interestsAndTalents: "",
        professionalSkill: "",
        currentAddress: "",
        emergencyContact: "",
        relationshipWith: "",
        emergencyContactPhoneNumber: "",
      },
      rules: {
        name: [{ required: true, message: "请输入姓名", trigger: "blur" }],
        entryTime: [
          { required: true, message: "请选择入职时间", trigger: "blur" },
        ],
        userName: [{ required: true, message: "请输入账号", trigger: "blur" }],
        password: [{ required: true, message: "请输入密码", trigger: "blur" }],

        departmentId: [
          { required: true, message: "请选择部门", trigger: "blur" },
        ],
        positionId: [
          { required: true, message: "请选择职位", trigger: "blur" },
        ],
        phoneNumber: [
          { required: true, message: "请输入电话号码", trigger: "blur" },
        ],
        email: [{ required: true, message: "请输入电子邮件", trigger: "blur" }],
        idCard: [
          { required: true, message: "请输入身份证号码", trigger: "blur" },
        ],
        gender: [{ required: true, message: "请选择性别", trigger: "blur" }],
        jobHuntingMode: [
          { required: true, message: "请选择求职方式", trigger: "blur" },
        ],
        currentEducation: [
          { required: true, message: "请选择目前学历", trigger: "blur" },
        ],
        maritalStatus: [
          { required: true, message: "请选择婚姻状况", trigger: "blur" },
        ],
        professionalSkill: [
          { required: true, message: "请输入掌握技能", trigger: "blur" },
        ],
        currentAddress: [
          { required: true, message: "请输入现居住地", trigger: "blur" },
        ],
        emergencyContact: [
          { required: true, message: "请输入紧急联系人", trigger: "blur" },
        ],
        relationshipWith: [
          {
            required: true,
            message: "请输入与紧急联系人的关系",
            trigger: "blur",
          },
        ],
        emergencyContactPhoneNumber: [
          {
            required: true,
            message: "请输入紧急联系人的手机号码",
            trigger: "blur",
          },
        ],
        natureOfEmployment: [
          { required: true, message: "请选择用工性质", trigger: "blur" },
        ],
      },

      // 对话框表单
      workHistoryRuleForm: {
        startTime: "",
        endTime: "",
        companyName: "",
        companyAddress: "",
        position: "",
        dimissionCause: "",
        biggestGain: "",
      },
      workHistoryRules: {
        startTime: [
          { required: true, message: "请选择开始时间", trigger: "blur" },
        ],
        endTime: [
          { required: true, message: "请选择结束时间", trigger: "blur" },
        ],
        companyName: [
          { required: true, message: "请输入公司名称", trigger: "blur" },
        ],
        companyAddress: [
          { required: true, message: "请输入公司地址", trigger: "blur" },
        ],
        position: [
          { required: true, message: "请输入曾任职位", trigger: "blur" },
        ],
      },
      educationHistoryRuleForm: {
        schoolName: "",
        currentEducation: "",
        major: "",
        startTime: "",
        endTime: "",
        degree: 0,
        degreeCreateTime: "",
        learningStyle: 1,
        score: 0,
        remark: "",
      },
      educationHistoryRules: {
        startTime: [
          { required: true, message: "请选择入学日期", trigger: "blur" },
        ],
        endTime: [
          { required: true, message: "请选择毕业日期", trigger: "blur" },
        ],
        schoolName: [
          { required: true, message: "请输入学校名称", trigger: "blur" },
        ],
        learningStyle: [
          { required: true, message: "请选择学习方式", trigger: "blur" },
        ],
        score: [{ required: true, message: "请输入成绩", trigger: "blur" }],
        currentEducation: [
          { required: true, message: "请输入此时学历", trigger: "blur" },
        ],
      },
      certificateRuleForm: {
        name: "",
        getTime: "",
        level: "",
        certificateNumber: "",
        organization: "",
        remark: "",
      },
      certificateRules: {
        name: [{ required: true, message: "请输入证书名称", trigger: "blur" }],
        getTime: [
          { required: true, message: "请选择证书获取时间", trigger: "blur" },
        ],
        level: [{ required: true, message: "请输入证书级别", trigger: "blur" }],
        certificateNumber: [
          { required: true, message: "请输入证书编号", trigger: "blur" },
        ],
        organization: [
          { required: true, message: "请输入证书颁发机构", trigger: "blur" },
        ],
      },
    };
  },
  computed:{
    ...mapState(["baseURL", "fileUploadURL"])
  },
  created() {
    // console.log(this.$route);
    // if(this.$route.query.HiringNeedApplyId !== undefined){
    //   console.log(this.$route.query.HiringNeedApplyId);
    //   this.ruleForm.hiringNeedApplyId = this.$route.query.HiringNeedApplyId;
    //   this.ruleForm.departmentId = this.$route.query.minDepartment.id;
    //   this.getPosition(this.ruleForm.departmentId);
    // }
    // 获取部门列表
    GetAllDepartment()
      .then((res) => {
        console.log("部门列表", res);
        for (const item of this.formConfig1) {
          if (item.label === "部门") {
            item.optionsList.push(res.data);
            break;
          }
        }
      })
      .catch(() => {});
  },
  methods: {
    // 当部门级联选择器发生改变
    changeDepartment(value) {
      console.log(value, "部门级联选择器");
      this.ruleForm.departmentId = value[value.length - 1];
      this.clearValidate("ruleForm", "departmentId");
      this.getPosition(value[value.length - 1]);
    },
    // 获取职位列表
    getPosition(id) {
      FindPositionByDepartment(id)
        .then((res) => {
          console.log(res, "职位");
          this.ruleForm.positionId = "";
          for (const item of this.formConfig1) {
            if (item.label === "职位") {
              item.optionsList = [];
              break;
            }
          }
          res.data.forEach((e) => {
            for (const item of this.formConfig1) {
              if (item.label === "职位") {
                item.optionsList.push({
                  label: e.name,
                  value: e.id,
                });
                break;
              }
            }
          });
        })
        .catch(() => {});
    },
    closeDialog() {},
    // 添加简历
    addRecord(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          AddRecord(this.ruleForm)
            .then((res) => {
              this.$message.success(res.msg);
              // 将表单初始化
              Object.assign(this.ruleForm, this.initRuleForm);
            })
            .catch(() => {});
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    addWorkHistory(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          console.log(this.workHistoryRuleForm, "workRuleForm");
          this.ruleForm.minWorkHistory.push(this.workHistoryRuleForm);
          this.$message.success("添加成功");
          this.workHistoryRuleForm = { ...this.initWorkHistoryRuleForm };
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    addEducationHistory(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          this.ruleForm.minEducationHistory.push(this.educationHistoryRuleForm);
          this.$message.success("添加成功");
          this.educationHistoryRuleForm = {
            ...this.initEducationHistoryRuleForm,
          };
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    addCertificate(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          this.ruleForm.minCertificate.push(this.certificateRuleForm);
          this.$message.success("添加成功");
          this.certificateRuleForm = { ...this.initCertificateRuleForm };
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    reset(formName) {
      this.$refs[formName].resetFields();
    },
    // 清除某一项表单的验证，主要用于select表单控件
    clearValidate(formName, prop) {
      if (this[formName][prop] != "" && this[formName][prop] != null) {
        this.$refs[formName].clearValidate([prop]);
      }
    },
    // 格式化教育经历表格项内容
    educationHistoryTableFormatter(row, column, cellValue, index) {
      for (const x of this.educationHistoryRuleFormConfig) {
        if (x.prop === column.property) {
          for (const y of x.optionsList) {
            if (y.value === cellValue) {
              return y.label;
            }
          }
        }
      }
    },
    // 修改头像
      handleAvatarSuccess(res, file) {
        // 服务器返回的图片地址
        console.log("服务器返回的图片地址", res);
        // 本地获取的文件对象
        // console.log("file,本地获取的文件对象", file);
        // 图片地址赋值
        this.ruleForm.avatar = res.filePath;
        // 暂时绑定图片对象
        // this.ruleForm.avatar = URL.createObjectURL(file.raw);
      },
      handleAvatarError(err, file, fileList){
        console.log(err,'上传文件失败');
        if (file.status == 'fail') {
          this.$message.error(JSON.parse(err.message).message);
        }
      },
      handleAvatarProgress(event, file, fileList){
        console.log(event,'文件上传时');
        console.log(file,'文件上传时,file');
        console.log(fileList,'文件上传时,fileList');
      },
      // 判断图片格式
      beforeAvatarUpload(file) {
        let objResult = isJPGAndIsLt2M(file);
        if(objResult.code===200){
          return objResult.data
        }else{
          this.$message.error(objResult.msg);
          return objResult.data
        }
      },
  },
};
</script>

<style lang='less' scoped>
.add-record-component {
  // border: 1px solid red;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;

  // 顶部搜索区
  .top-box {
    min-height: 40px;
    background: rgb(242, 242, 242);
    display: flex;
    justify-content: space-between;
    align-items: center;

    & > span:first-child {
      font-size: 0.92rem;
      color: rgba(0, 0, 0, 0.8);
      padding-left: 20px;
    }

    .s_close {
      width: 40px;
      height: 40px;
      text-align: center;
      line-height: 40px;
    }
  }

  .content {
    position: absolute;
    top: 40px;
    bottom: 0;
    left: 0;
    right: 0;
    // 设置滚动
    overflow-y: auto;
    padding: 25px 0;

    // 标题
    .title {
      padding: 0px 0 10px 0;
      text-align: center;
      // border: 1px solid red;

      & > div {
        font-weight: 500;
        font-size: 1.4rem;
      }
    }

    .content-box {
      padding: 0 30px;

      // 小标题
      .min-title {
        // border: 1px solid green;
        height: 30px;
        display: flex;
        align-items: center;

        & > div:nth-child(1) {
          width: 40px;
          border-bottom: 2px solid rgb(242, 242, 242);
        }
        & > div:nth-child(2) {
          font-weight: 340;
          font-size: 1.2rem;
          padding: 0 10px;
        }
        & > div:nth-child(3) {
          flex: 1;
          border-bottom: 2px solid rgb(242, 242, 242);
        }
      }

      .ruleForm {
        padding: 0 50px 0 20px;
      }

      .educationHistoryDialog {
        /deep/.el-dialog__body {
          padding: 0 25px;
        }
      }

      .table-box {
        // border: 1px solid red;
        padding: 10px 0;
      }

      .result {
        .result-btn {
          width: 100%;
        }
      }
    }
  }
}

// 罪魁祸首，解决upload添加图片后，有间隙问题
    /deep/.el-upload {
      display: block;
    }
.avatar-uploader {
  width: 110px;
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  text-align: center;
}
.avatar-uploader:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 110px;
  height: 134.4px;
  line-height: 134.4px;
  text-align: center;
  // border: 1px solid red;
}
.avatar {
  width: 110px;
  height: 134.4px;
  display: block;
  // border: 1px solid green;
}

// ai
.card {
  break-inside: avoid;
}

@media (min-width: 768px) {
  .row1 {
    // 分成几列
    column-count: 2;
    // 列间的间距
    // column-gap: 20px;
  }
}
@media (min-width: 1200px) {
  .row1 {
    // 列间的边框
      // column-rule-style: solid;
    column-count: 3;
    // 设置列间的间距为0，好像有默认间距。
    column-gap: 0px;
  }
}
</style>