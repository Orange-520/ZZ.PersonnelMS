<template>
  <div class="hiring-needs-list">
    <!-- 顶部搜索区 -->
    <div class="top-box">
      <span>
        <el-input
          placeholder="请输入增补职位关键字"
          size="small"
          v-model="ruleForm.positionWordKey"
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

        <el-table-column prop="applyUserName" label="申请人"></el-table-column>

        <el-table-column
          prop="minDepartment.departmentName"
          label="增补部门"
          :show-overflow-tooltip="true"
        ></el-table-column>

        <el-table-column
          prop="minPosition.positionName"
          label="增补职位"
        ></el-table-column>

        <el-table-column prop="content" label="招聘理由"></el-table-column>

        <el-table-column
          prop="needPersonCount"
          label="需求人数"
        ></el-table-column>

        <el-table-column
          prop="hasPersonCount"
          label="已招聘人数"
        ></el-table-column>

        <el-table-column fixed="right" label="操作" width="200px">
          <template slot-scope="scope">
            <el-button
              type="primary"
              size="small"
              @click="showDialogRepertory(scope.row)"
              >应聘登记</el-button
            >
            <el-button
              type="danger"
              size="small"
              @click="showDialogRepertory(scope.row)"
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
import { GetHiringNeedApply } from "@/https/join-us/hiringNeedApply.js";
export default {
  data() {
    return {
      tableData:[],
      total:0,
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      ruleForm: {
        pageIndex: 1,
        pageSize: 10,
        positionWordKey: "",
      },
    };
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
      this.ruleForm.pageIndex = val;
      this.GetData();
    },
    GetData() {
      GetHiringNeedApply(this.ruleForm)
        .then(res => {
          console.log("所有招聘需求", res);
          this.total = res.total;
          this.tableData = res.data;
        })
        .catch(() => {});
    }
  },
};
</script>

<style lang='less' scoped>
.hiring-needs-list {
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