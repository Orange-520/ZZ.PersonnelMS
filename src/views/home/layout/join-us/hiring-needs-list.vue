<template>
  <el-container class="hiring-needs-list">
    <el-header>
      <!-- 顶部搜索区 -->
      <el-input
        placeholder="请输入增补职位关键字"
        size="small"
        v-model="ruleForm.positionWordKey"
      >
        <el-button
          slot="append"
          icon="el-icon-search"
          @click="GetData"
        ></el-button>
      </el-input>
    </el-header>

    <el-main>
      <!-- 外层招聘需求表格 -->
      <el-table
        class="table-box"
        :data="tableData"
        height="100%"
        style="width: 100%"
        :fit="true"
        :cell-style="{ 'text-align': 'center', width: 'auto' }"
        :header-cell-style="{ 'text-align': 'center' }"
      >
        <el-table-column type="expand">
          <template slot-scope="props">
            <!-- 嵌套应聘登记表格 -->
            <el-table
              :data="props.row.resumeList"
              border
              stripe
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

              <el-table-column prop="name" label="姓名"></el-table-column>

              <el-table-column
                prop="gender"
                label="性别"
                :formatter="genderColumnFormatter"
              ></el-table-column>

              <el-table-column
                prop="department.name"
                label="应聘部门"
              ></el-table-column>

              <el-table-column
                prop="position.name"
                label="应聘职位"
              ></el-table-column>

              <el-table-column
                prop="phoneNumber"
                label="手机号码"
                width="120"
              ></el-table-column>

              <el-table-column
                prop="email"
                label="邮箱"
                width="120"
              ></el-table-column>

              <el-table-column
                prop="joinUsStep"
                label="当前环节"
                :formatter="joinUsStepColumnFormatter"
              ></el-table-column>

              <el-table-column
                prop="checkUser.userName"
                label="当前处理人"
              ></el-table-column>

              <el-table-column fixed="right" label="操作" width="260px">
                <template slot-scope="scope">
                  <el-button type="primary" size="small">详情</el-button>
                  <el-button
                    type="success"
                    size="small"
                    @click="
                      joinUsStepRuleForm.resumeId = scope.row.id;
                      joinUsStepRuleForm.joinUsStep = scope.row.joinUsStep;
                      joinUsStepDialogVisible = true;
                    "
                    >环节处理</el-button
                  >
                  <el-button
                    type="danger"
                    size="small"
                    @click="
                      joinUsResultRuleForm.resumeId = scope.row.id;
                      resume = scope.row;
                      joinUsResultDialogVisible = true;
                    "
                    >结果处理</el-button
                  >
                </template>
              </el-table-column>
            </el-table>
          </template>
        </el-table-column>

        <el-table-column label="" width="70" align="left">
          <template slot-scope="scope">
            {{
              scope.$index + 1 + (ruleForm.pageIndex - 1) * ruleForm.pageSize
            }}
          </template>
        </el-table-column>

        <el-table-column prop="applyUserName" label="申请人"></el-table-column>

        <el-table-column
          prop="minDepartment.departmentName"
          label="增补部门"
          :show-overflow-tooltip="true"
        ></el-table-column>

        <el-table-column
          prop="minPosition.positionName"
          label="增补职位"
        ></el-table-column>

        <el-table-column prop="content" label="招聘理由"></el-table-column>

        <el-table-column prop="needPersonCount" label="需求人数">
        </el-table-column>

        <el-table-column
          prop="hasPersonCount"
          label="已招聘人数"
        ></el-table-column>

        <el-table-column label="应聘人数">
          <template slot-scope="props">
            <span>{{ props.row.resumeList.length }}</span>
          </template>
        </el-table-column>

        <!-- 点击应聘登记时，跳转页面并将当前的招聘需求Id传递 -->
        <el-table-column fixed="right" label="操作">
          <template slot-scope="scope">
            <el-button
              type="primary"
              size="small"
              @click="
                $router.push({
                  name: 'addResume',
                  query: {
                    hiringNeedApplyId: scope.row.id,
                    departmentId: scope.row.minDepartment.id,
                    positionId: scope.row.minPosition.id,
                  },
                })
              "
              >应聘登记</el-button
            >
          </template>
        </el-table-column>
      </el-table>

      <!-- 环节处理对话框 -->
      <OneColumnDialog
        title="应聘环节处理"
        :visible.sync="joinUsStepDialogVisible"
        :ruleForm="joinUsStepRuleForm"
        :rules="joinUsStepRules"
        :ruleFormConfig="joinUsStepRuleFormConfig"
        :btnFont="'改 变 环 节'"
        @submitData="changeJoinUsStep"
      ></OneColumnDialog>

      <!-- 应聘结果对话框 -->
      <OneColumnDialog
        title="应聘结果处理"
        :visible.sync="joinUsResultDialogVisible"
        :ruleForm="joinUsResultRuleForm"
        :rules="joinUsResultRules"
        :ruleFormConfig="joinUsResultRuleFormConfig"
        :btnFont="'应 聘 结 果'"
        @submitData="changeJoinUsResult"
      ></OneColumnDialog>
    </el-main>

    <el-footer>
      <el-pagination
        class="pagination_box"
        background
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page.sync="ruleForm.pageIndex"
        :page-sizes="pageSizes"
        :page-size="ruleForm.pageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-footer>
  </el-container>
</template>

<script>
import OneColumnDialog from "@/components/one-column-dialog.vue";
import { GetHiringNeedApply } from "@/https/join-us/hiringNeedApply.js";
import {
  ChangeJoinUsStep,
  ChangeJoinUsResult,
} from "@/https/join-us/resume.js";
import config from "@/assets/js/config.js";
import resumeConfig from "./js/resumeConfig.js";
export default {
  components: {
    OneColumnDialog,
  },
  data() {
    return {
      // 应聘环节
      joinUsStepRuleFormConfig: resumeConfig.joinUsStepRuleFormConfig,
      // 应聘结果
      joinUsResultRuleFormConfig: resumeConfig.joinUsResultRuleFormConfig,

      joinUsStepDialogVisible: false,
      joinUsResultDialogVisible: false,

      tableData: [],
      total: 0,
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      ruleForm: {
        pageIndex: 1,
        pageSize: 10,
        positionWordKey: "",
      },

      resume:{},

      joinUsStepRuleForm: {
        resumeId: null,
        joinUsStep: null,
      },
      joinUsStepRules: {
        joinUsStep: [
          { required: true, message: "请选择应聘环节", trigger: "blur" },
        ],
      },

      joinUsResultRuleForm: {
        resumeId: null,
        joinUsResult: null,
      },
      joinUsResultRules: {
        joinUsResult: [
          { required: true, message: "请选择处理结果", trigger: "blur" },
        ],
      },
    };
  },
  created() {
    this.GetData();
  },
  methods: {
    // pageSize改变时调用
    handleSizeChange(val) {
      // 改变每页显示条目个数
      this.ruleForm.pageSize = val;
      this.GetData();
    },
    // 当前页触发调用
    handleCurrentChange(val) {
      this.ruleForm.pageIndex = val;
      this.GetData();
    },
    GetData() {
      GetHiringNeedApply(this.ruleForm)
        .then((res) => {
          console.log("所有招聘需求", res);
          this.total = res.total;
          this.tableData = res.data;
        })
        .catch(() => {});
    },
    // 格式化表格性别列
    genderColumnFormatter(row, column, cellValue, index) {
      for (const item of config.genderConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    // 格式化表格招聘环节列
    joinUsStepColumnFormatter(row, column, cellValue, index) {
      for (const item of config.joinUsStepConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    // 处理应聘环节
    changeJoinUsStep() {
      ChangeJoinUsStep(this.joinUsStepRuleForm)
        .then((res) => {
          this.$message.success(res.msg);
          this.GetData();
          this.joinUsStepDialogVisible = false;
        })
        .catch(() => {});
    },
    // 处理应聘结果
    changeJoinUsResult() {
      // 如果选择不合格，则弹出提示
      if (this.joinUsResultRuleForm.joinUsResult === 4) {
        this.$confirm("此操作将移除此应聘简历！", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        })
          .then(() => {
            // 改变应聘结果
            ChangeJoinUsResult(this.joinUsResultRuleForm)
              .then((res) => {
                this.$message.success(res.msg);
                this.GetData();
                this.joinUsResultDialogVisible = false;
                // 关闭对话框后重置提交表单对象中的内容
                this.joinUsResultRuleForm.joinUsResult = null;
              })
              .catch(() => {});
          })
          .catch(() => {
            return;
          });
      } else {
        // 如果为入职，则跳转到添加档案页面
        if (this.joinUsResultRuleForm.joinUsResult === 1) {
          this.$store.commit("resume/RESUME", this.resume);
          this.$store.commit(
            "resume/JOINUSRESULTRULEFORM",
            this.joinUsResultRuleForm
          );
          this.$router.push({
            name: "addRecord",
            query: {
              from: "fromResumeList",
            },
          });
        } else {
          // 改变应聘结果
          ChangeJoinUsResult(this.joinUsResultRuleForm)
            .then((res) => {
              this.$message.success(res.msg);
              this.GetData();
              this.joinUsResultDialogVisible = false;
              // 关闭对话框后重置提交表单对象中的内容
              this.joinUsResultRuleForm.joinUsResult = null;
            })
            .catch(() => {});
        }
      }
    },
  },
};
</script>

<style lang='less' scoped>
.hiring-needs-list {
  height: 100%;

  .el-header {
    height: 40px !important;
    line-height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;

    // 设置顶部搜索组合表单的宽度
    .el-input-group,
    .el-input {
      width: auto;
    }
  }

  .el-main {
    // 清除默认填充
    padding: 0;
  }

  .el-footer {
    z-index: 3;
    height: 60px;
    display: flex;
    justify-content: flex-end;
    align-items: center;
    box-shadow: 0 -2px 2px 0 rgb(0 0 0 / 10%);
    overflow: hidden;
  }
}
</style>