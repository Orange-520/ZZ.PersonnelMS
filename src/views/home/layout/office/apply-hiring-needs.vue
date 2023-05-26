<template>
  <div style="width:60%;">
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
          :props= "cascaderProps"
          @change="changeDepartment">
        </el-cascader>
      </el-form-item>

      <el-form-item label="需求职位" prop="positionId">
        <el-select v-model="ruleForm.positionId" placeholder="请选择需求职位">
          <el-option v-for="(item,index) in positions" :key="index" :label="item.name" :value="item.id"></el-option>
        </el-select>
      </el-form-item>
      
      <el-form-item label="需求人数">
        <el-input-number v-model="ruleForm.needPersonCount" @change="changeNeedCount" :min="1" :precision='0'></el-input-number>
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
</template>

<script>
import { GetAllDepartment,FindPositionByDepartment } from '@/https/commons.js';
import { AddHiringNeedApply } from '@/https/office/userApply.js';
export default {
  data() {
    return {
      ruleForm: {
        departmentId: '',
        positionId: '',
        content: "",
        needPersonCount: 1,
        applyType: 1,
      },
      rules: {
        departmentId: [
          { required: true, message: "请选择需求部门", trigger: "blur" }
        ],
        positionId: [
          { required: true, message: "请选择需求职位", trigger: "blur" },
        ],
        content: [
          { required: true, message: "请输入需求原因", trigger: "blur" },
        ]
      },
      cascaderProps:{ expandTrigger: 'click',value:'id',label:'name',children:'childrenDepartment',checkStrictly: true, },
      // 部门列表
      departmentList:[],
      // 职位数组
      positions: [],
    };
  },
  created(){
    GetAllDepartment().then(res=>{
      console.log('部门列表',res);
      this.departmentList.push(res.data);
    }).catch(()=>{});
  },
  methods:{
    changeDepartment(value) {
      console.log(value);
      this.ruleForm.departmentId = value[value.length-1];
      this.getPosition(value[value.length-1]);
      // 关闭级联选择器
      // this.$refs.elCascader.dropDownVisible = false
    },
    // 按部门获取职位
    getPosition(id){
      FindPositionByDepartment(id).then(res=>{
        console.log(res);
        this.ruleForm.positionId = '';
        this.positions = res.data;
      }).catch(()=>{});
    },
    // 计数器改变
    changeNeedCount(value){
      this.ruleForm.needPersonCount = value;
    },
    // 提交招聘需求申请
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          AddHiringNeedApply(this.ruleForm).then(res=>{
            this.$message.success(res.msg);
            this.ruleForm.content = '';
            this.ruleForm.needPersonCount = 1;
            this.ruleForm.departmentId = '';
            this.ruleForm.positionId = '';
          }).catch(()=>{});
        } else {
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    }
  }
};
</script>

<style lang='less' scoped>

</style>