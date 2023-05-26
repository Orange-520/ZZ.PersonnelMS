<template>
  <div class="add-resume">
    <div class="title">
      <div>应聘信息登记表</div>
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
        label-width="120px"
        size="small"
      >
        <el-row>
          <template v-for="(item, index) in formConfig">
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
                  <el-select v-model="ruleForm[item.prop]" style="width: 100%">
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
              </el-form-item>
            </el-col>
          </template>
        </el-row>
      </el-form>

      <div class="min-title">
        <div></div>
        <div>
          工作经历
          <el-button type="primary" size="small">新增教育经历</el-button>
        </div>
        <div></div>
      </div>

      <div class="min-title">
        <div></div>
        <div>
          教育经历
          <el-button type="primary" size="small">新增教育经历</el-button>
        </div>
        <div></div>
      </div>

      

      <div class="min-title">
        <div></div>
        <div>
          获奖经历
          <el-button type="primary" size="small">新增获奖经历</el-button>
        </div>
        <div></div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    const formConfig = [
      { label: "姓名", prop: "name", type: "input", span: 8 },
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
      { label: "手机号码", prop: "phoneNumber", type: "input", span: 8 },
      { label: "籍贯", prop: "nativePlace", type: "input", span: 8 },
      { label: "身份证号码", prop: "idCard", type: "input", span: 8 },
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
      { label: "电子邮件", prop: "email", type: "input", span: 8 },
      { label: "出生日期", prop: "birthday", type: "date-picker", span: 8 },
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
      { label: "健康状况", prop: "healthCondition", type: "input", span: 8 },
      { label: "语言能力", prop: "languageCompetence", type: "input", span: 8 },
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
      {
        label: "兴趣特长",
        prop: "interestsAndTalents",
        type: "textarea",
        span: 8,
      },
      {
        label: "专业技能",
        prop: "professionalSkill",
        type: "textarea",
        span: 8,
      },
      { label: "现居住地", prop: "currentAddress", type: "textarea", span: 8 },
      { label: "紧急联系人", prop: "emergencyContact", type: "input", span: 8 },
      { label: "与其关系", prop: "relationshipWith", type: "input", span: 8 },
      {
        label: "紧急联系人号码",
        prop: "emergencyContactPhoneNumber",
        type: "input",
        span: 8,
      },
      {
        label: "应聘部门",
        prop: "departmentId",
        type: "select",
        span: 8,
        optionsList: [
          {
            value: 1,
            label: "",
          },
        ],
      },
      {
        label: "应聘职位",
        prop: "positionId",
        type: "select",
        span: 8,
        optionsList: [
          {
            value: 1,
            label: "",
          },
        ],
      },
      { label: "期望薪资", prop: "salaryExpectation", type: "input", span: 8 },
    ];
    return {
      formConfig,
      // 表单配置项
      ruleForm: {
        name: "",
        gender: null,
        phoneNumber: "",
        nativePlace: "",
        idCard: "",
        maritalStatus: "",
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
        departmentId: null,
        positionId: null,
        salaryExpectation: 6000,
        minWorkHistory: [
          {
            startTime: "",
            endTime: "",
            companyName: "",
            companyAddress: "",
            position: "",
            dimissionCause: "",
            biggestGain: ""
          }
        ],
        minEducationHistory: [
          {
            schoolName: "",
            currentEducation: 2,
            major: "",
            startTime: "",
            endTime: "",
            degree: 0,
            degreeCreateTime: "",
            learningStyle: 1,
            score: 350,
            remark: "",
          },
        ],
        minCertificate: [
          {
            name: "",
            getTime: "",
            level: "",
            certificateNumber: "",
            organization: "",
            remark: "",
          },
        ],
      },
      rules: {
        name: [{ required: true, message: "请输入姓名", trigger: "blur" }],
        gender: [{ required: true, message: "请选择性别", trigger: "blur" }],
        phoneNumber: [
          { required: true, message: "请输入电话号码", trigger: "blur" },
        ],
        jobHuntingMode: [
          { required: true, message: "请选择求职方式", trigger: "blur" },
        ],
        departmentId: [
          { required: true, message: "请选择应聘部门", trigger: "blur" },
        ],
        positionId: [
          { required: true, message: "请选择应聘职位", trigger: "blur" },
        ],
      },
    };
  },
};
</script>

<style lang='less' scoped>
.add-resume {
  // border: 1px solid red;

  // 标题
  .title {
    padding: 25px 0 10px 0;
    text-align: center;
    // border: 1px solid red;

    & > div {
      font-weight: 500;
      font-size: 1.4rem;
    }
  }

  .content-box{
    overflow-x: hidden;
  }

  // 小标题
  .min-title {
    // border: 1px solid green;
    height: 30px;
    display: flex;
    align-items: center;
    padding: 0 20px 0 20px;

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
}
</style>