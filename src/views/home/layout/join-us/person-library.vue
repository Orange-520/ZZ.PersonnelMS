<template>
  <el-container class="person-library">
    <el-header>
      <span>
        <el-input
          placeholder="请输入姓名"
          size="small"
          v-model="ruleForm.nameKey"
        >
          <el-button
            slot="append"
            icon="el-icon-search"
            @click="GetData"
          ></el-button>
        </el-input>
      </span>
    </el-header>

    <el-main>
      <el-table
        class="table-box"
        :data="tableData"
        border
        height="100%"
        style="width: 100%"
        :fit="true"
        :cell-style="{ 'text-align': 'center', width: 'auto' }"
        :header-cell-style="{ 'text-align': 'center' }"
      >
        <el-table-column label="" minWidth="70" align="left">
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

        <el-table-column
          prop="position.name"
          label="应聘职位"
        ></el-table-column>

        <el-table-column
          prop="phoneNumber"
          label="手机号码"
          minWidth="120"
        ></el-table-column>

        <el-table-column
          prop="email"
          label="邮箱"
          minWidth="120"
        ></el-table-column>

        <el-table-column fixed="right" label="操作" minWidth="200px">
          <template slot-scope="scope">
            <el-button
              type="primary"
              size="small"
              @click="console.log(scope.row, '信息')"
              >编辑</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-main>

    <el-footer>
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
    </el-footer>
  </el-container>
</template>

<script>
import { GetResumeList } from "@/https/join-us/resume.js";
import config from "@/assets/js/config.js";
export default {
  data() {
    return {
      tableData: [],
      ruleForm: {
        pageIndex: 1,
        pageSize: 10,
        nameKey: "",
        // 获取简历结果为 【放入人才库】 的
        joinUsResult: 2,
      },
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,
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
      this.ruleForm.currentPage = val;
      this.GetData();
    },
    // 获取人才库
    GetData() {
      GetResumeList(this.ruleForm)
        .then((res) => {
          console.log("人才库", res);
          this.total = res.total;
          this.tableData = res.data;
        })
        .catch(() => {});
    },
    // 处理性别列
    genderColumnFormatter(row, column, cellValue, index) {
      for (const item of config.genderConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
  },
};
</script>

<style lang='less' scoped>
.person-library {
  height: 100%;

  .el-header {
    height: 40px !important;
    line-height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;

    // 设置顶部搜索组合表单的宽度
    .el-input-group,
    .el-input {
      width: auto;
    }
  }

  .el-main {
    // 清除默认填充
    padding: 0;
  }

  .el-footer {
    z-index: 3;
    height: 60px;
    display: flex;
    justify-content: flex-end;
    align-items: center;
    box-shadow: 0 -2px 2px 0 rgb(0 0 0 / 10%);
    overflow: hidden;
  }
}
</style>