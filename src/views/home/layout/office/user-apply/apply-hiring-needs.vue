<template>
  <div class="apply-hiring-needs">
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
        <el-form-item label="需求部门" prop="departmentId">
          <el-cascader
            ref="elCascader"
            v-model="ruleForm.departmentId"
            :options="departmentList"
            :props="cascaderProps"
            @change="changeDepartment"
          >
          </el-cascader>
        </el-form-item>

        <el-form-item label="需求职位" prop="positionId">
          <el-select placeholder="请选择需求职位" v-model="ruleForm.positionId" @change="clearValidate('ruleForm', 'positionId')">
            <el-option
              v-for="(item, index) in positionList"
              :key="index"
              :label="item.name"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="需求人数" prop="needPersonCount">
          <el-input-number
            v-model="ruleForm.needPersonCount"
            @change="changeNeedCount"
            :min="1"
            :precision="0"
          ></el-input-number>
        </el-form-item>

        <el-form-item label="需求缘由" prop="content">
          <el-input type="textarea" v-model="ruleForm.content"></el-input>
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
import Bar from '@/components/bar.vue';
import { AddHiringNeedApply } from "@/https/office/userApply.js";
import { mapActions, mapState } from 'vuex';
export default {
  components: {
    Bar,
  },
  data() {
    return {
      ruleForm: {
        departmentId: "",
        positionId: "",
        content: "",
        needPersonCount: 1,
        applyType: 1,
      },
      rules: {
        departmentId: [
          { required: true, message: "请选择需求部门", trigger: "blur" },
        ],
        positionId: [
          { required: true, message: "请选择需求职位", trigger: "blur" },
        ],
        needPersonCount: [
          { required: true, message: "请输入需求人数", trigger: "blur" },
        ],
        content: [
          { required: true, message: "请输入需求原因", trigger: "blur" },
        ],
      },
      cascaderProps: {
        expandTrigger: "click",
        value: "id",
        label: "name",
        children: "childrenDepartment",
        checkStrictly: true,
      },
    };
  },
  computed:{
    ...mapState('department',['departmentList']),
    ...mapState('position',['positionList']),
  },
  created() {
    this.getAllDepartment();
  },
  beforeDestroy(){
    // 页面销毁前重置positionList
    this.$store.commit('position/resetState');
  },
  methods: {
    ...mapActions('department',['getAllDepartment']),
    ...mapActions('position',['getPositionByDepartment']),
    closeDetail() {
      this.$router.push({ name: "applyList" });
    },
    changeDepartment(value) {
      console.log(value,'改变部门级联选择器');
      this.ruleForm.departmentId = value[value.length - 1];
      // 清除验证信息
      this.clearValidate('ruleForm', 'departmentId');
      // 先清除上一次选择的职位
      this.ruleForm.positionId = "";
    // 按部门获取职位
      this.getPositionByDepartment(value[value.length - 1]);
      // 关闭级联选择器
      // this.$refs.elCascader.dropDownVisible = false;
    },
    // 计数器改变
    changeNeedCount(value) {
      this.ruleForm.needPersonCount = value;
    },
    // 提交招聘需求申请
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          AddHiringNeedApply(this.ruleForm)
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
.apply-hiring-needs{
  
  .detail {
    display: flex;
    justify-content: space-between;
    align-items: center;
    
    & > span:first-child {
      font-size: 0.92rem;
      color: rgba(0,0,0,0.8);
    }

    .s_close{
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