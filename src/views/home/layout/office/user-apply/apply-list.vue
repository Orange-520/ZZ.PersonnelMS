<template>
  <el-container class="apply-list">
    <el-header>
      <!-- 顶部分类 -->
      <el-tag
        class="category-item pointer"
        v-for="(item, index) in tags"
        :key="index"
        :type="item.type"
        @click="showDetail(item)"
        >{{ item.name }}</el-tag
      >
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
        <el-table-column label="序号" width="70" align="left">
          <template slot-scope="scope">
            {{
              scope.$index + 1 + (ruleForm.currentPage - 1) * ruleForm.pageSize
            }}
          </template>
        </el-table-column>

        <el-table-column
          prop="applyType"
          label="申请类型"
          :formatter="applyTypeColumnFormatter"
        >
        </el-table-column>

        <el-table-column
          prop="content"
          label="申请理由"
          :show-overflow-tooltip="true"
        ></el-table-column>

        <!-- 处理显示内容 -->
        <!-- 方式一：绑定格式化方法 formatter -->
        <el-table-column
          prop="createTime"
          label="申请时间"
          :show-overflow-tooltip="true"
          :formatter="createTimeColumnFormatter"
        >
        </el-table-column>

        <el-table-column prop="checkState" label="审核状态">
          <!-- 方式二：使用插槽，使用三元运算符 -->
          <template slot-scope="scope">
            <span
              :class="
                scope.row.checkState == 1
                  ? 'green'
                  : scope.row.checkState == 2
                  ? 'red'
                  : 'aqua'
              "
            >
              {{
                scope.row.checkState == 1
                  ? "同意"
                  : scope.row.checkState == 2
                  ? "不同意"
                  : "待审核"
              }}
            </span>
          </template>
        </el-table-column>

        <el-table-column
          prop="checkUser.userName"
          label="当前审核人"
        ></el-table-column>

        <el-table-column fixed="right" label="操作" width="120px">
          <template slot-scope="scope">
            <el-button
              type="primary"
              @click="showDialogRepertory(scope.row)"
              size="small"
              >详情</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-main>

    <el-footer>
      <!-- 分页 -->
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
import { GetAllUserApply } from "@/https/office/userApply.js";
export default {
  data() {
    return {
      tableData: [],
      ruleForm: {
        currentPage: 1,
        pageSize: 10,
      },
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,
      // 分类
      tags: [
        {
          name: "招聘需求",
          type: "",
          path: "/home/office/userApply/applyHiringNeeds",
          routerName: "applyHiringNeeds",
        },
        {
          name: "出差",
          type: "success",
        },
        {
          name: "请假单",
          type: "info",
          path: "/home/office/userApply/applyAskForLeave",
          routerName: "applyAskForLeave",
        },
        {
          name: "离职",
          type: "danger",
        },
        {
          name: "报销",
          type: "warning",
        },
      ],
    };
  },
  computed: {},
  methods: {
    showDetail(item) {
      this.$router.push({ name: item.routerName });
    },
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
    GetData() {
      GetAllUserApply(this.ruleForm)
        .then((res) => {
          console.log("用户个人申请", res);
          this.total = res.total;
          this.tableData = res.data;
        })
        .catch(() => {});
    },
    // 处理申请类型列
    applyTypeColumnFormatter(row, column, cellValue, index) {
      console.log(cellValue);
      switch (cellValue) {
        case 1:
          return "招聘需求";
        case 2:
          return "出差";
        case 3:
          return "请假";
        case 4:
          return "离职";
        case 5:
          return "报销";
      }
    },
    // 处理申请时间列
    createTimeColumnFormatter(row, column, cellValue, index) {
      let t = new Date(cellValue);
      return `${t.getFullYear()}-${
        t.getMonth() + 1
      }-${t.getDate()} ${t.getHours()}:${t.getMinutes()}`;
    },
  },
  created() {
    this.GetData();
  },
};
</script>

<style lang='less' scoped>
.red {
  color: red;
}
.green {
  color: green;
}
.aqua {
  color: rgb(22, 203, 255);
}

.apply-list {
  height: 100%;

  .el-header {
    height: 40px !important;
    line-height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;

    .category-item {
      margin-right: 5px;
    }

    .el-tag {
      height: 30px;
      line-height: 30px;
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