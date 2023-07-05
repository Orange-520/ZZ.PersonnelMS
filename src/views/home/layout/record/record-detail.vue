<template>
  <div class="record-detail">
    <!-- 顶部 -->
    <div class="top-box">
      <span>档案详情</span>
      <span
        class="pointer s_close"
        @click="
          $router.push({ path: '/home/record/recordList' })
        "
      >
        <i class="el-icon-close"></i>
      </span>
    </div>

    <!-- 档案信息 -->
    <div class="content">
      <div class="title">
        <div>档案信息</div>
      </div>

      <div class="content-box">
        <div class="min-title">
          <div></div>
          <div>基本信息</div>
          <div></div>
        </div>

        <!-- 信息 -->
        <div>
          <el-row class="row1">
            <div class="card">
              <span>姓名：</span><span>{{ record.name }}</span>
            </div>
            <div class="card">
              <span>账号：</span><span>{{ record.user.userName }}</span>
            </div>
            <div class="card">
              <span>部门：</span><span>{{ record.department.name }}</span>
            </div>
            <div class="card">
              <span>入职时间：</span><span>{{ record.entryTime }}</span>
            </div>
            <div class="card">
              <span>职位：</span><span>{{ record.position.name }}</span>
            </div>
            <div class="card">
              <span>相片：</span>
              <el-image
                v-if="record.avatar"
                :src="baseURL + '/image/' + record.avatar"
                fit="fit"
                class="avatar"
              >
              </el-image>
            </div>
          </el-row>

          <el-row>
            <el-col :span="8">
              <span>手机号码：</span><span>{{ record.phoneNumber }}</span>
            </el-col>
            <el-col :span="8">
              <span>电子邮件</span><span>{{ record.email }}</span>
            </el-col>
            <el-col :span="8">
              <span>身份证号码</span><span>{{ record.idCard }}</span>
            </el-col>
            <el-col :span="8">
              <span>性别</span><span>{{ record.gender }}</span>
            </el-col>
            <el-col :span="8">
              <span>出生日期</span><span>{{ record.birthday }}</span>
            </el-col>
            <el-col :span="8">
              <span>兴趣特长</span><span>{{ record.interestsAndTalents }}</span>
            </el-col>
            <el-col :span="8">
              <span>籍贯</span><span>{{ record.nativePlace }}</span>
            </el-col>
            <el-col :span="8">
              <span>政治面貌</span><span>{{ record.politicsStatus }}</span>
            </el-col>
            <el-col :span="8">
              <span>求职方式</span><span>{{ record.jobHuntingMode }}</span>
            </el-col>
            <el-col :span="8">
              <span>毕业院校</span><span>{{ record.schoolOfGraduation }}</span>
            </el-col>
            <el-col :span="8">
              <span>学历</span><span>{{ record.currentEducation }}</span>
            </el-col>
            <el-col :span="8">
              <span>专业</span><span>{{ record.major }}</span>
            </el-col>
            <el-col :span="8">
              <span>婚姻状况</span><span>{{ record.maritalStatus }}</span>
            </el-col>
            <el-col :span="8">
              <span>健康状况</span><span>{{ record.healthCondition }}</span>
            </el-col>
            <el-col :span="8">
              <span>语言能力</span><span>{{ record.languageCompetence }}</span>
            </el-col>
            <el-col :span="24">
              <span>掌握技能</span><span>{{ record.professionalSkill }}</span>
            </el-col>
            <el-col :span="24">
              <span>现居住地</span><span>{{ record.currentAddress }}</span>
            </el-col>
            <el-col :span="8">
              <span>紧急联系人</span><span>{{ record.emergencyContact }}</span>
            </el-col>
            <el-col :span="8">
              <span>与其关系</span><span>{{ record.relationshipWith }}</span>
            </el-col>
            <el-col :span="8">
              <span>紧急联系人号码</span
              ><span>{{ record.emergencyContactPhoneNumber }}</span>
            </el-col>
            <el-col :span="8">
              <span>用工性质</span><span>{{ record.natureOfEmployment }}</span>
            </el-col>
            <el-col :span="16">
              <span>身份证复印件</span><span>{{ record.idCardPicture }}</span>
            </el-col>
            <el-col :span="8">
              <span>转正日期</span
              ><span>{{ record.dateOfConfirmationAfterProbation }}</span>
            </el-col>
            <el-col :span="8">
              <span>购买社保类型</span
              ><span>{{ record.typeOfSocialSecurity }}</span>
            </el-col>
            <el-col :span="8">
              <span>社保卡号</span
              ><span>{{ record.socialSecurityCardNumber }}</span>
            </el-col>
          </el-row>
        </div>

        <div class="min-title" style="margin-top: 20px">
          <div></div>
          <div>工作经历</div>
          <div></div>
        </div>

        <!-- 工作经历表格 -->
        <div class="workHistory table-box">
          <el-table
            class=""
            :data="record.workHistory"
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
          </el-table>
        </div>

        <div class="min-title" style="margin-top: 20px">
          <div></div>
          <div>教育经历</div>
          <div></div>
        </div>

        <!-- 教育经历表格 -->
        <div class="educationHistory table-box">
          <el-table
            class=""
            :data="record.educationHistory"
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
          </el-table>
        </div>

        <div class="min-title" style="margin-top: 20px">
          <div></div>
          <div>获奖经历</div>
          <div></div>
        </div>

        <!-- 获奖经历表格 -->
        <div class="certificate table-box">
          <el-table
            class=""
            :data="record.certificate"
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
          </el-table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";
export default {
  data() {
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
    return {
      educationHistoryRuleFormConfig,
    };
  },
  computed: {
    ...mapState(["baseURL"]),
    ...mapState('record',["record"]),
  },
  // //进入守卫：通过路由规则，进入该组件时被调用
  // beforeRouteEnter(to, from, next) {
  //   console.log(to, "to");
  //   console.log(from, "from");
  //   console.log(to.query === undefined);
  //   if (to.query.name !== undefined) {
  //     next(vm => {
  //       setTimeout(() => {
  //       console.log(vm);
          
  //       }, 1000);

  //       setTimeout(() => {
  //       vm.record = { ...to.query };
          
  //       }, 5000);
  //     });
  //   }else{
  //     next({ path: "/home/record/recordList/recordListComponent" });
  //   }
  //   console.log('object');
  // },
  methods: {
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
  },
};
</script>

<style lang='less' scoped>
.record-detail {
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