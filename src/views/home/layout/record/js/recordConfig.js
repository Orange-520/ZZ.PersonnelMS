import config from '@/assets/js/config.js';

export default {
  formRowOneConfig: [
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
  ],
  formRowTwoConfig: [
    { label: "手机号码", prop: "phoneNumber", type: "input", span: 8 },
    { label: "电子邮件", prop: "email", type: "input", span: 8 },
    { label: "身份证号码", prop: "idCard", type: "input", span: 8 },
    {
      label: "性别",
      prop: "gender",
      type: "select",
      span: 8,
      optionsList: config.genderConfig
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
      optionsList: config.politicsStatusConfig
    },
    {
      label: "求职方式",
      prop: "jobHuntingMode",
      type: "select",
      span: 8,
      optionsList: config.jobHuntingModeConfig
    },
    { label: "毕业院校", prop: "schoolOfGraduation", type: "input", span: 8 },
    {
      label: "学历",
      prop: "currentEducation",
      type: "select",
      span: 8,
      optionsList:config.currentEducationConfig
    },
    { label: "专业", prop: "major", type: "input", span: 8 },
    {
      label: "婚姻状况",
      prop: "maritalStatus",
      type: "select",
      span: 8,
      optionsList: config.maritalStatusConfig
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
      optionsList:config.natureOfEmploymentConfig
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
  ],
  workHistoryRuleFormConfig: [
    { label: "开始日期", prop: "startTime", type: "date-picker", span: 8 },
    { label: "结束日期", prop: "endTime", type: "date-picker", span: 8 },
    { label: "公司名称", prop: "companyName", type: "input", span: 8 },
    { label: "所在地址", prop: "companyAddress", type: "input", span: 8 },
    { label: "曾任职位", prop: "position", type: "input", span: 8 },
    { label: "离职原因", prop: "dimissionCause", type: "textarea", span: 8 },
    { label: "最大收获", prop: "biggestGain", type: "textarea", span: 8 },
  ],
  educationHistoryRuleFormConfig: [
    { label: "学校名称", prop: "schoolName", type: "input", span: 8 },
    {
      label: "当前学历",
      prop: "currentEducation",
      type: "select",
      span: 8,
      optionsList: config.currentEducationConfig
    },
    { label: "入学时间", prop: "startTime", type: "date-picker", span: 8 },
    { label: "毕业时间", prop: "endTime", type: "date-picker", span: 8 },
    {
      label: "学习方式",
      prop: "learningStyle",
      type: "select",
      span: 8,
      optionsList: config.learningStyleConfig
    },
    { label: "学习成绩", prop: "score", type: "input-number", span: 8 },
    { label: "专业", prop: "major", type: "input", span: 8 },
    {
      label: "学位",
      prop: "degree",
      type: "select",
      span: 8,
      optionsList: config.degreeConfig
    },

    {
      label: "学位授予时间",
      prop: "degreeCreateTime",
      type: "date-picker",
      span: 8,
    },
    { label: "备注", prop: "remark", type: "textarea", span: 8 },
  ],
  certificateRuleFormConfig: [
    { label: "证书名称", prop: "name", type: "input", span: 8 },
    { label: "获得时间", prop: "getTime", type: "date-picker", span: 8 },
    { label: "级别", prop: "level", type: "input", span: 8 },
    { label: "证书编号", prop: "certificateNumber", type: "input", span: 8 },
    { label: "颁发机构", prop: "organization", type: "input", span: 8 },
    { label: "备注", prop: "remark", type: "textarea", span: 8 },
  ],
  initRuleForm: {
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
  initWorkHistoryRuleForm: {
    startTime: "",
    endTime: "",
    companyName: "",
    companyAddress: "",
    position: "",
    dimissionCause: "",
    biggestGain: "",
  },
  initEducationHistoryRuleForm: {
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
  initCertificateRuleForm: {
    name: "",
    getTime: "",
    level: "",
    certificateNumber: "",
    organization: "",
    remark: "",
  },
}