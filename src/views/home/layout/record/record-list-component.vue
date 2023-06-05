<template>
  <div class="record-list-component">
    <!-- 顶部搜索区 -->
    <div class="top-box">
      <span>
        <el-button size="small" type="primary" @click="$router.push({path:'/home/record/recordList/addRecordComponent'})">新增用户</el-button>
      </span>
      <span>
        <span>
          <el-input size="small" v-model="ruleForm.nameKey" placeholder="请输入姓名、登录名"></el-input>
        </span>
        <span>
          <el-cascader
            size="small"
            placeholder="请选择部门"
            ref="elCascader"
            v-model="ruleForm.departmentId"
            :options="departmentList"
            :props= "cascaderProps"
            @change="changeDepartment">
          </el-cascader>
        </span>
        <span>
          <el-select size="small" v-model="ruleForm.positionId" placeholder="请选择职位">
            <el-option v-for="(item,index) in positions" :key="index" :label="item.name" :value="item.id"></el-option>
          </el-select>
        </span>
        <span>
          <el-button size="small" @click="GetData">查询</el-button>
        </span>
        <span>
          <el-button size="small" @click="searchReset">重置</el-button>
        </span>
      </span>
    </div>

    <!-- 表格 -->
    <div class="content-box">
      <el-table
        class="table-box"
        :data="tableData"
        border
        style="width: 100%"
        :fit="true"
        :cell-style="{ 'text-align': 'center', width: 'auto' }"
        :header-cell-style="{ 'text-align': 'center' }"
      >
        <el-table-column label="" width="70" align="left">
          <template slot-scope="scope">
            {{
              scope.$index + 1 + (ruleForm.pageIndex - 1) * ruleForm.pageSize
            }}
          </template>
        </el-table-column>

        <el-table-column prop="name" label="姓名"></el-table-column>

        <el-table-column
          prop="user.userName"
          label="登录名"
        ></el-table-column>

        <el-table-column
          prop="department.name"
          label="部门"
        ></el-table-column>

        <el-table-column prop="position.name" label="职务"></el-table-column>

        <el-table-column
          prop="natureOfEmployment"
          label="用工性质"
          :formatter="natureOfEmploymentColumnFormatter"
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

        <el-table-column fixed="right" label="操作" width="320px">
          <template slot-scope="scope">
            <el-button
              type="primary"
              size="mini"
              >关联角色</el-button
            >
            <el-button
              type="success"
              size="mini"
              @click="console.log(scope.row,'编辑')"
              >编辑</el-button
            >
            <el-button
              type="danger"
              size="mini"
              >删除</el-button
            >
            <el-button
              size="mini"
              >详情</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!-- 分页 -->
    <div class="footer-box">
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
    </div>
  </div>
</template>

<script>
import { GetAllDepartment, FindPositionByDepartment } from "@/https/commons.js";
import { GetRecordList } from '@/https/join-us/recordList.js'
export default {
  data() {
    const initRuleForm = {
        pageIndex: 1,
        pageSize: 10,
        nameKey: null,
        departmentId:null,
        positionId:null
      };
    return {
      initRuleForm,

      // 部门级联选择器配置项
      cascaderProps: {
        expandTrigger: "click",
        value: "id",
        label: "name",
        children: "childrenDepartment",
        checkStrictly: true,
      },
      // 部门列表
      departmentList:[],
      // 职位数组
      positions: [],

      tableData: [],
      ruleForm: {
        pageIndex: 1,
        pageSize: 10,
        nameKey: null,
        departmentId:null,
        positionId:null
      },
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,
    };
  },
  created() {
    // 获取部门列表
     GetAllDepartment().then(res=>{
      console.log('部门列表',res);
      this.departmentList.push(res.data);
    }).catch(()=>{});

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
      this.ruleForm.currentPage = val;
      this.GetData();
    },
    // 获取人才库
    GetData() {
      GetRecordList(this.ruleForm)
        .then(res => {
          console.log("档案列表", res);
          this.total = res.total;
          this.tableData = res.data;
        })
        .catch(() => {});
    },
    // 搜索重置
    searchReset(){
      this.ruleForm = {...this.initRuleForm};
      this.GetData();
    },
    // 处理性别列
    natureOfEmploymentColumnFormatter(row, column, cellValue, index) {
      const natureOfEmploymentConfig = [
        { label: "", value: 0 },
        { label: "正式员工", value: 1 },
        { label: "临时工", value: 2 },
        { label: "实习生", value: 3 },
      ];
      for (const item of natureOfEmploymentConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    closeDialog() {},
    // 清除某一项表单的验证，主要用于select表单控件
    clearValidate(formName, prop) {
      if (this[formName][prop] != "" && this[formName][prop] != null) {
        this.$refs[formName].clearValidate([prop]);
      }
    },
    changeDepartment(value) {
      console.log(value);
      this.ruleForm.departmentId = value[value.length-1];
      this.getPosition(value[value.length-1]);
    },
    // 按部门获取职位
    getPosition(id){
      FindPositionByDepartment(id).then(res=>{
        console.log(res);
        this.ruleForm.positionId = '';
        this.positions = res.data;
      }).catch(()=>{});
    },
  },
};
</script>

<style lang='less' scoped>
.record-list-component {
  // border: 1px solid red;
  display: flex;
  flex-direction: column;
  height: 100%;

  // 顶部搜索区
  .top-box {
    min-height: 40px;
    background: rgb(242, 242, 242);
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 10px;

    & > span:nth-child(2){
      display: flex;

      &>span{
        padding-left: 10px;
      }
    }
  }

  .content-box {
    flex: 1;

    .table-box {
      height: 100%;
    }
  }

  // 底部分页
  .footer-box {
    z-index: 1;
    height: 8%;
    display: flex;
    justify-content: flex-end;
    align-items: center;
    padding: 0 20px;
    box-shadow: 0 -2px 2px 0 rgb(0 0 0 / 10%);
  }
}
</style>