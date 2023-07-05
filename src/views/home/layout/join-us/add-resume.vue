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
      <ColumnLayoutForm
        class="ruleForm"
        ref="ruleForm"
        :formObj.sync="ruleForm"
        :rules="rules"
        :ruleFormTwoConfig="formConfig"
        @changeCascader="changeDepartment"
      ></ColumnLayoutForm>

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
      <OneColumnDialog
        title="添加工作经历"
        top="6vh"
        :visible.sync="workHistoryDialogVisible"
        :ruleForm="workHistoryRuleForm"
        :rules="workHistoryRules"
        :ruleFormConfig="workHistoryRuleFormConfig"
        @submitData="addWorkHistory"
      ></OneColumnDialog>

      <!-- 工作经历表格 -->
      <div class="workHistory table-box">
        <WorkHistoryTable
          :tableData="ruleForm.minWorkHistory"
        ></WorkHistoryTable>
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
      <OneColumnDialog
        title="添加教育经历"
        top="2vh"
        :visible.sync="educationHistoryDialogVisible"
        :ruleForm="educationHistoryRuleForm"
        :rules="educationHistoryRules"
        :ruleFormConfig="educationHistoryRuleFormConfig"
        @submitData="addEducationHistory"
      ></OneColumnDialog>

      <!-- 教育经历表格 -->
      <div class="educationHistory table-box">
        <EducationHistoryTable
          :tableData="ruleForm.minEducationHistory"
        ></EducationHistoryTable>
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
      <OneColumnDialog
        title="添加获奖经历"
        :visible.sync="certificateDialogVisible"
        :ruleForm="certificateRuleForm"
        :rules="certificateRules"
        :ruleFormConfig="certificateRuleFormConfig"
        @submitData="addCertificate"
      ></OneColumnDialog>

      <!-- 获奖经历表格 -->
      <div class="certificate table-box">
        <CertificateTable
          :tableData="ruleForm.minCertificate"
        ></CertificateTable>
      </div>

      <div class="result">
        <el-button
          type="primary"
          class="result-btn"
          @click="addResume('ruleForm')"
          >添加应聘信息</el-button
        >
      </div>
    </div>
  </div>
</template>

<script>
import { AddResume } from "@/https/join-us/resume.js";
import CertificateTable from "@/components/certificate-table.vue";
import WorkHistoryTable from "@/components/work-history-table.vue";
import EducationHistoryTable from "@/components/education-history-table.vue";
import OneColumnDialog from "@/components/one-column-dialog.vue";
import ColumnLayoutForm from "@/components/column-layout-form.vue";
import resumeConfig from "./js/resumeConfig.js";
import { mapActions, mapState } from "vuex";
export default {
  components: {
    CertificateTable,
    WorkHistoryTable,
    EducationHistoryTable,
    OneColumnDialog,
    ColumnLayoutForm,
  },
  data() {
    return {
      // 页面渲染配置项
      formConfig: resumeConfig.formConfig,
      workHistoryRuleFormConfig: resumeConfig.workHistoryRuleFormConfig,
      educationHistoryRuleFormConfig:
        resumeConfig.educationHistoryRuleFormConfig,
      certificateRuleFormConfig: resumeConfig.certificateRuleFormConfig,

      // 对话框是否显示
      workHistoryDialogVisible: false,
      educationHistoryDialogVisible: false,
      certificateDialogVisible: false,

      // 初始化表单项，用以将表单项恢复至初始状态
      initRuleForm: resumeConfig.initRuleForm,
      initWorkHistoryRuleForm: resumeConfig.initWorkHistoryRuleForm,
      initEducationHistoryRuleForm: resumeConfig.initEducationHistoryRuleForm,
      initCertificateRuleForm: resumeConfig.certificateRuleFormConfig,

      // 表单配置项
      ruleForm: {
        hiringNeedApplyId: null,
        name: "李四",
        gender: 2,
        phoneNumber: "12345678911",
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
        jobHuntingMode: 1,
        interestsAndTalents: "",
        professionalSkill: "",
        currentAddress: "",
        emergencyContact: "",
        relationshipWith: "",
        emergencyContactPhoneNumber: "",
        departmentId: null,
        positionId: null,
        salaryExpectation: 6000,
        minWorkHistory: [],
        minEducationHistory: [],
        minCertificate: [],
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
  computed: {
    ...mapState("department", ["departmentList"]),
    ...mapState("position", ["positionList"]),
  },
  created() {
    console.log(this.$route);
    // 获取部门列表
    this.getAllDepartment().then(() => {
      // 获取的部门数据放入到配置对象的部门属性中
      for (const item of this.formConfig) {
        if (item.label === "应聘部门") {
          item.optionsList = this.departmentList;
          break;
        }
      }
    });
    if (this.$route.query.hasOwnProperty("departmentId")) {
      this.getPositionList(this.$route.query.departmentId);
    }
  },
  mounted(){
    // 判断一个对象上是否存在一个属性
    let result = this.$route.query.hasOwnProperty("hiringNeedApplyId");
    if (result) {
      this.ruleForm.hiringNeedApplyId = this.$route.query.hiringNeedApplyId;
      this.ruleForm.departmentId = this.$route.query.departmentId;
      // 怎么让从需求列表跳转过来的需求职位也显示表单中啊。
      this.ruleForm.positionId = this.$route.query.positionId;
    }
  },
  beforeDestroy() {
    // 页面销毁前重置positionList
    this.$store.commit("position/resetState");
    this.resetPositionList();
  },
  methods: {
    ...mapActions("department", ["getAllDepartment"]),
    ...mapActions("position", ["getPositionByDepartment"]),
    // 当部门级联选择器发生改变
    changeDepartment(value) { 
      console.log(value, "部门级联选择器");
      this.ruleForm.departmentId = value[value.length - 1];
      // 清空提交表单中的职位Id
      this.ruleForm.positionId = "";
      this.$refs["ruleForm"].clearValidate("ruleForm", "departmentId");
      this.getPositionList(value[value.length - 1]);
    },
    // 获取职位列表
    getPositionList(id) {
      this.getPositionByDepartment(id).then(() => {
        // 找到配置数组中为职位的属性，先清空职位属性中数组的原有数据
        this.resetPositionList();
        // 找到配置数组中为职位的属性，加工返回的职位数据，装填至职位属性中的数组
        this.positionList.forEach((e) => {
          for (const item of this.formConfig) {
            if (item.label === "应聘职位") {
              item.optionsList.push({
                label: e.name,
                value: e.id,
              });
              break;
            }
          }
        });
      });
    },
    // 添加简历
    addResume(formName) {
      // 调用组件内的表单验证方法，判断输入的数据是否通过验证
      let result = this.$refs[formName].formValidate();
      if (result) {
        AddResume(this.ruleForm)
          .then((res) => {
            this.$message.success(res.msg);
            // 将表单初始化
            Object.assign(this.ruleForm, this.initRuleForm);
          })
          .catch(() => {});
      }
    },
    addWorkHistory(workHistoryObj) {
      this.ruleForm.minWorkHistory.push(workHistoryObj);
      this.$message.success("添加成功");
      this.workHistoryRuleForm = { ...this.initWorkHistoryRuleForm };
    },
    addEducationHistory(educationHistoryObj) {
      this.ruleForm.minEducationHistory.push(educationHistoryObj);
      this.$message.success("添加成功");
      this.educationHistoryRuleForm = { ...this.initEducationHistoryRuleForm };
    },
    addCertificate(certificateObj) {
      console.log(certificateObj, "对话框回调的值");
      this.ruleForm.minCertificate.push(certificateObj);
      this.$message.success("添加成功");
      this.certificateRuleForm = { ...this.initCertificateRuleForm };
    },
    reset(formName) {
      this.$refs[formName].resetFields();
    },
    // 重置配置文件中的职位列表
    resetPositionList() {
      for (const item of this.formConfig) {
        if (item.label === "应聘职位") {
          item.optionsList = [];
          break;
        }
      }
    },
  },
};
</script>

<style lang='less' scoped>
.add-resume {
  // border: 1px solid red;
  // 由于父级是使用了固定定位，子级元素的内容想要滚动，则需要设置 position: absolute; 并且设置top、bottom、left、right使内容填满父元素
  // 思考：如何让 title 固定，content-box 中的内容滚动。
  position: absolute;
  top: 0;
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
      padding: 10px 0;
    }

    .result {
      .result-btn {
        width: 100%;
      }
    }
  }
}
</style>