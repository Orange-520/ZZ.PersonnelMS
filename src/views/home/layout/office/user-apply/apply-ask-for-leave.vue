<template>
  <div class="apply-ask-for-leave">
    <Bar>
      <!-- 详情 -->
      <div class="detail">
        <span>招聘需求</span>
        <span class="pointer s_close" @click="closeDetail">
          <i class="el-icon-close"></i>
        </span>
      </div>
    </Bar>

    <div class="content">
      <el-form
        :model="ruleForm"
        :rules="rules"
        ref="ruleForm"
        label-width="100px"
        class="demo-ruleForm"
      >
        <el-form-item label="请假类型" prop="askForLeaveType">
          <el-select
            v-model="ruleForm.askForLeaveType"
            placeholder="请选择请假类型"
            @change="clearValidate('ruleForm','askForLeaveType')"
          >
            <el-option
              v-for="(item, index) in options"
              :key="index"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="开始时间" prop="startTime">
          <el-col :span="11">
            <el-date-picker
              type="date"
              placeholder="选择日期"
              v-model="ruleForm.startTime"
              style="width: 100%"
            ></el-date-picker>
          </el-col>
          <el-col class="line" style="text-align: center" :span="2">-</el-col>
          <el-col :span="11">
            <el-time-picker
              placeholder="选择时间"
              v-model="startTime"
              style="width: 100%"
            ></el-time-picker>
          </el-col>
        </el-form-item>

        <el-form-item label="结束时间" prop="endTime">
          <el-col :span="11">
            <el-date-picker
              type="date"
              placeholder="选择日期"
              v-model="ruleForm.endTime"
              style="width: 100%"
            ></el-date-picker>
          </el-col>
          <el-col class="line" style="text-align: center" :span="2">-</el-col>
          <el-col :span="11">
            <el-time-picker
              placeholder="选择时间"
              v-model="endTime"
              style="width: 100%"
            ></el-time-picker>
          </el-col>
        </el-form-item>

        <el-form-item label="请假时长" prop="howLong">
          <el-input v-model="ruleForm.howLong"></el-input>
        </el-form-item>

        <el-form-item label="请假事由" prop="content">
          <el-input type="textarea" v-model="ruleForm.content"></el-input>
        </el-form-item>

        <el-form-item label="审批人员" prop="checkUser">
          <el-select
            v-model="ruleForm.checkUser"
            placeholder="请选择审批人员"
            @change="clearValidate('ruleForm','checkUser')"
          >
            <el-option
              v-for="(item, index) in checkUserOptions"
              :key="index"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item>
          <el-button type="primary" @click="submitForm('ruleForm')"
            >立即创建</el-button
          >
          <el-button @click="resetForm('ruleForm')">重置</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script>
import Bar from "@/components/bar.vue";
import { AddAskForLeaveApply } from "@/https/office/userApply.js";
export default {
  components: {
    Bar,
  },
  data() {
    return {
      ruleForm: {
        askForLeaveType: "",
        startTime: "",
        endTime: "",
        howLong: "",
        content: "",
        checkUser: "",
      },
      startTime: "",
      endTime: "",
      rules: {
        askForLeaveType: [
          { required: true, message: "请选择请假类型", trigger: "blur" },
        ],
        startTime: [
          { required: true, message: "请选择开始时间", trigger: "blur" },
        ],
        endTime: [
          { required: true, message: "请输入结束时间", trigger: "blur" },
        ],
        howLong: [
          { required: true, message: "请输入请假时长", trigger: "blur" },
        ],
        content: [
          { required: true, message: "请输入请假事由", trigger: "blur" },
        ],
        checkUser: [
          { required: true, message: "请选择审核人", trigger: "blur" },
        ],
      },
      options: [
        {
          value: "1",
          label: "事假",
        },
        {
          value: "2",
          label: "病假",
        },
      ],
      checkUserOptions: [],
    };
  },
  methods: {
    closeDetail() {
      this.$router.push({ name: "applyList" });
    },
    // 提交招聘需求申请
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          AddAskForLeaveApply({
            applyType: 3,
            askForLeaveType: this.ruleForm.askForLeaveType,
            startTime: this.ruleForm.startTime + this.startTime,
            endTime: this.ruleForm.endTime + this.endTime,
            howLong: this.ruleForm.howLong,
            content: this.ruleForm.content,
            checkUser: this.ruleForm.checkUser,
          })
            .then((res) => {
              this.$message.success(res.msg);
              this.ruleForm.content = "";
              this.ruleForm.needPersonCount = 1;
              this.ruleForm.departmentId = "";
              this.ruleForm.positionId = "";
            })
            .catch(() => {});
        } else {
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
    // 清除某一项表单的验证，主要用于select表单控件
    clearValidate(formName, prop) {
      if (this[formName][prop] != "" && this[formName][prop] != null) {
        this.$refs[formName].clearValidate([prop]);
      }
    },
  },
};
</script>

<style lang='less' scoped>
.apply-ask-for-leave {
  .detail {
    display: flex;
    justify-content: space-between;
    align-items: center;

    & > span:first-child {
      font-size: 0.92rem;
      color: rgba(0, 0, 0, 0.8);
    }

    .s_close {
      width: 40px;
      height: 40px;
      display: flex;
      justify-content: center;
      align-items: center;
    }
  }
  .content {
    width: 60%;
    padding: 10px;
  }
}
</style>