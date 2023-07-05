<template>
  <div class="add-record">
    <!-- 顶部搜索 -->
    <div class="top-box">
      <span>新增</span>
      <span class="pointer s_close" @click="$router.go(-1)">
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
        <ColumnLayoutForm
          class="ruleForm"
          ref="ruleForm"
          :formObj.sync="ruleForm"
          :rules="rules"
          :ruleFormOneConfig="formRowOneConfig"
          :ruleFormTwoConfig="formRowTwoConfig"
          :action="baseURL + fileUploadURL"
          :imageSrc="baseURL + '/image/'"
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
        <!-- .sync 修饰符的作用：
          背景：父组件通过 props 传递给子组件的值，子组件直接修改 props 上的值，违反了规定（子组件不能随意更改父组件的值）。
          如果想这样做（类似一个双向绑定的效果），则使用组件的自定义事件来实现（类似于事件总线）。
          而 .sync 修饰符则简化了组件自定义事件的实现。-->
        <!-- @submitData="addWorkHistory" 就是一个自定义事件，在子组件内调用，父组件中实现，达到传递值的效果 -->
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

        <!-- 添加获奖经历对话框 -->
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
            @click="addRecord('ruleForm')"
            >登记入职档案信息</el-button
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { obj1MapObj2 } from "@/assets/js/commonFunc.js";
import { AddRecord } from "@/https/join-us/record.js";
import { mapActions, mapState } from "vuex";
import OneColumnDialog from "@/components/one-column-dialog.vue";
import CertificateTable from "@/components/certificate-table.vue";
import WorkHistoryTable from "@/components/work-history-table.vue";
import EducationHistoryTable from "@/components/education-history-table.vue";
import ColumnLayoutForm from "@/components/column-layout-form.vue";
import recordConfig from "./js/recordConfig.js";
export default {
  components: {
    OneColumnDialog,
    CertificateTable,
    WorkHistoryTable,
    EducationHistoryTable,
    ColumnLayoutForm,
  },
  data() {
    return {
      // 页面渲染配置项
      formRowOneConfig: recordConfig.formRowOneConfig,
      formRowTwoConfig: recordConfig.formRowTwoConfig,
      workHistoryRuleFormConfig: recordConfig.workHistoryRuleFormConfig,
      educationHistoryRuleFormConfig:
        recordConfig.educationHistoryRuleFormConfig,
      certificateRuleFormConfig: recordConfig.certificateRuleFormConfig,

      // 对话框是否显示
      workHistoryDialogVisible: false,
      educationHistoryDialogVisible: false,
      certificateDialogVisible: false,

      // 初始化表单项，用以将表单项恢复至初始状态
      initRuleForm: recordConfig.initRuleForm,
      initWorkHistoryRuleForm: recordConfig.initWorkHistoryRuleForm,
      initEducationHistoryRuleForm: recordConfig.initEducationHistoryRuleForm,
      initCertificateRuleForm: recordConfig.initCertificateRuleForm,

      // 表单配置项
      ruleForm: {
        userName: "",
        password: "",
        departmentId: null,
        positionId: null,
        entryTime: "",
        avatar: "",
        natureOfEmployment: 1,
        idCardPicture: "",
        dateOfConfirmationAfterProbation: "",
        typeOfSocialSecurity: "",
        socialSecurityCardNumber: "",
        minWorkHistory: [],
        minEducationHistory: [],
        minCertificate: [],
        name: "张三",
        gender: 1,
        phoneNumber: "17899990000",
        nativePlace: "湖南郴州",
        idCard: "431021000000000001",
        maritalStatus: 2,
        email: "abc@163.com",
        birthday: "",
        politicsStatus: 13,
        schoolOfGraduation: "湖南工程职业技术学院",
        currentEducation: 4,
        major: "软件技术",
        healthCondition: "健康",
        languageCompetence: "三级甲等",
        jobHuntingMode: 1,
        interestsAndTalents: "运动和健身、学习新知识",
        professionalSkill:
          "使用 ASP.NET Core WebAPI 完成后端接口的开发，使用 Vue2 完成前端页面的开发",
        currentAddress: "湖南长沙",
        emergencyContact: "朱爸爸",
        relationshipWith: "父子",
        emergencyContactPhoneNumber: "15900001111",
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
  computed: {
    ...mapState(["baseURL", "fileUploadURL"]),
    ...mapState("department", ["departmentList"]),
    ...mapState("position", ["positionList"]),
    ...mapState("resume", ["resume"]),
  },
  created() {
    // 获取部门列表
    this.getAllDepartment().then(() => {
      // 获取的部门数据放入到配置对象的部门属性中
      for (const item of this.formRowOneConfig) {
        if (item.label === "部门") {
          item.optionsList = this.departmentList;
          break;
        }
      }
    });
    // 如果是从
    if (this.$route.query.hasOwnProperty("from") && this.resume !== null) {
      this.getPositionList(this.resume.department.id);
    }
  },
  mounted() {
    if (this.$route.query.hasOwnProperty("from") && this.resume !== null) {
      let resume = this.resume;
      console.log(resume, "resume");
      if (resume.currentEducation === 0) {
        resume.currentEducation = null;
      }
      if (resume.jobHuntingMode === 0) {
        resume.jobHuntingMode = null;
      }
      if (resume.joinUsResult === 0) {
        resume.joinUsResult = null;
      }
      if (resume.joinUsStep === 0) {
        resume.joinUsStep = null;
      }
      if (resume.maritalStatus === 0) {
        resume.maritalStatus = null;
      }
      if (resume.politicsStatus === 0) {
        resume.politicsStatus = null;
      }
      console.log(resume, "resume");
      this.ruleForm = obj1MapObj2(resume, this.ruleForm);
      this.ruleForm.departmentId = resume.department.id;
      this.ruleForm.positionId = resume.position.id;
      this.ruleForm.minWorkHistory = resume.workHistory;
      this.ruleForm.minEducationHistory = resume.educationHistory;
      this.ruleForm.minCertificate = resume.certificate;
    }
  },
  beforeDestroy() {
    // 页面销毁前重置positionList
    this.$store.commit("position/resetState");
    this.resetPositionList();
    // 清除页面间传递的值
    if (this.$route.query.hasOwnProperty("from")) {
      this.$store.commit("resume/RESUME", null);
    }
  },
  methods: {
    ...mapActions("department", ["getAllDepartment"]),
    ...mapActions("position", ["getPositionByDepartment"]),
    ...mapActions("resume", ["changeJoinUsResult"]),
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
          for (const item of this.formRowOneConfig) {
            if (item.label === "职位") {
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
    addRecord(formName) {
      let result = this.$refs[formName].formValidate();
      if (result) {
        AddRecord(this.ruleForm)
          .then((res) => {
            // 提交成功后清理数据
            // 如果是办理入职，并且第一次添加档案信息
            if (
              this.$route.query.hasOwnProperty("from") &&
              this.resume != null
            ) {
              this.$store.commit("resume/RESUME", null);
              // 添加档案信息成功后，改变简历的应聘结果为入职
              this.changeJoinUsResult().then(() => {
                this.$message.success("办理入职成功");
                // 将表单初始化
                return;
              });
            } else {
              this.$message.success(res.msg);
            }
            // 将表单初始化
            Object.assign(this.ruleForm, this.initRuleForm);
          })
          .catch(() => {});
      }
    },
    addWorkHistory(workHistoryObj) {
      this.ruleForm.minWorkHistory.push(workHistoryObj);
      this.$message.success("添加成功");
      // 父传子，重置对话框中的表单填写的值。
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
      for (const item of this.formRowOneConfig) {
        if (item.label === "职位") {
          item.optionsList = [];
          break;
        }
      }
    },
  },
};
</script>

<style lang='less' scoped>
.add-record {
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
</style>