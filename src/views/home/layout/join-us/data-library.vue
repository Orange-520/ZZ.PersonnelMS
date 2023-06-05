<template>
  <div class="data-library">
    <!-- 顶部搜索区 -->
    <div class="top-box">
      <span>
        <el-input
          placeholder="请输入姓名"
          size="small"
          v-model="ruleForm.nameKey"
        >
          <el-button slot="append" icon="el-icon-search" @click="GetData"></el-button>
        </el-input>
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
          prop="gender"
          label="性别"
          :formatter="genderColumnFormatter"
        ></el-table-column>

        <el-table-column
          prop="department.name"
          label="应聘部门"
        ></el-table-column>

        <el-table-column prop="position.name" label="应聘职位"></el-table-column>

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

        <el-table-column fixed="right" label="操作" width="200px">
          <template slot-scope="scope">
            <el-button
              type="primary"
              size="small"
              @click="console.log(scope.row,'信息')"
              >编辑</el-button
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
import {GetResumeList} from '@/https/join-us/resumeList.js'
export default {
  data() {
    return {
      tableData: [],
      ruleForm:{
        pageIndex: 1,
        pageSize: 10,
        nameKey: "",
        // 获取简历结果为 【放入人才库】 的
        "joinUsResult": 3
      },
       // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,
    }
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
      this.ruleForm.currentPage = val;
      this.GetData();
    },
    // 获取资料库
    GetData(){
      GetResumeList(this.ruleForm).then(res=>{
        console.log('资料库',res);
        this.total = res.total;
        this.tableData = res.data;
      }).catch(()=>{});
    },
    // 处理性别列
    genderColumnFormatter(row, column, cellValue, index){
      const genderConfig = [
        {label:'',value:0},
        {label:'男',value:1},
        {label:'女',value:2}
      ];
      for (const item of genderConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    closeDialog(){},
    // 清除某一项表单的验证，主要用于select表单控件
    clearValidate(formName, prop) {
      if (this[formName][prop] != "" && this[formName][prop] != null) {
        this.$refs[formName].clearValidate([prop]);
      }
    },
  },
}
</script>

<style lang='less' scoped>
.data-library {
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

    & > span {
      padding-left: 10px;
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