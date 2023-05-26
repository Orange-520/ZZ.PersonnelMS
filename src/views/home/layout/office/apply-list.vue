<template>
  <div class="apply-list">
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
        <el-table-column label="序号" width="70" align="left">
          <template slot-scope="scope">
            {{ scope.$index + 1 + (ruleForm.currentPage - 1) * ruleForm.pageSize }}
          </template>
        </el-table-column>

        <el-table-column prop="applyTypeName" label="申请类型"> </el-table-column>

        <el-table-column
          prop="content"
          label="申请理由"
          :show-overflow-tooltip="true"
        ></el-table-column>

        <el-table-column
          prop="createTime"
          label="申请时间"
          :show-overflow-tooltip="true"
        >
        </el-table-column>

        <el-table-column prop="checkState" label="审核状态">
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
    </div>

    <div class="footer-box">
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
    </div>
  </div>
</template>

<script>
import { GetAllUserApply } from "@/https/office/userApply.js";
export default {
  data() {
    return {
      tableData: [],
      ruleForm:{
        currentPage: 1,
        pageSize: 10,
      },
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,
    };
  },
  computed: {},
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
    GetData() {
      GetAllUserApply(this.ruleForm)
        .then((res) => {
          console.log("用户个人申请", res);
          this.total = res.total;
          this.tableData = res.data.map((item) => {
            Object.defineProperty(item, "applyTypeName", {
              value: "",
              writable: true,
            });
            switch (item.applyType) {
              case 1:
                item.applyTypeName = "招聘需求";
                break;
              case 2:
                item.applyTypeName = "出差";
                break;
              case 3:
                item.applyTypeName = "请假";
                break;
              case 4:
                item.applyTypeName = "离职";
                break;
              case 5:
                item.applyTypeName = "报销";
                break;
              default:
                break;
            }

            // 处理时间戳
            let t = new Date(item.createTime);
            item.createTime = `${t.getFullYear()}-${
              t.getMonth() + 1
            }-${t.getDate()} ${t.getHours()}:${t.getMinutes()}:${t.getSeconds()}`;

            return item;
          });
        })
        .catch(() => {});
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

.apply-list{
  // border: 1px solid red;
  height: 100%;
  display: flex;
  flex-direction: column;

  .content-box{
    flex: 1;

    .table-box{
      height: 100%;
    }
  }

  // 底部分页
  .footer-box{
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