<template>
  <el-container class="approve">
    <el-header class="top-box">
      <div>待审批申请</div>
    </el-header>

    <el-main>
      <el-table
        :data="tableData"
        border
        style="width: 100%"
        height="100%"
        :fit="true"
        :cell-style="{ 'text-align': 'center', width: 'auto' }"
        :header-cell-style="{ 'text-align': 'center' }"
      >
        <el-table-column label="序号" width="70" align="left">
          <template slot-scope="scope">
            {{ scope.$index + 1 + (currentPage - 1) * pageSize }}
          </template>
        </el-table-column>

        <el-table-column prop="user.userName" label="申请人"></el-table-column>

        <el-table-column prop="createTime" label="申请时间"></el-table-column>

        <el-table-column
          prop="applyTypeName"
          label="申请类型"
        ></el-table-column>

        <el-table-column
          prop="content"
          label="申请理由"
          :show-overflow-tooltip="true"
        ></el-table-column>

        <el-table-column fixed="right" label="操作" width="120px">
          <template slot-scope="scope">
            <el-button
              type="primary"
              @click="showDialog(scope.row)"
              size="small"
              >详情</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-main>

    <!-- 对话框 -->
    <!-- :modal-append-to-body = 'false' 让遮罩层不在最顶层？ -->
    <el-dialog
      title="详情"
      :visible.sync="dialogVisible"
      width="600px"
      center
      :modal-append-to-body="false"
      top="10vh"
    >
      <div class="dialog-content">
        <div class="dialog-content-title dialog-content-title1">
          <span>申请</span>
        </div>
        <div>
          <span>申请人：</span>
          <span>{{ row.userName }}</span>
        </div>
        <div>
          <span>申请时间：</span>
          <span>{{ row.createTime }}</span>
        </div>
        <div>
          <span>申请类型：</span>
          <span>{{ row.applyTypeName }}</span>
        </div>
        <div>
          <span>申请事由：</span>
          <span>{{ row.content }}</span>
        </div>

        <div class="dialog-content-title dialog-content-title2">
          <span>审核</span>
        </div>

        <el-form
          :model="ruleForm"
          :rules="rules"
          ref="ruleForm"
          label-width="auto"
        >
          <el-form-item label="是否同意" prop="checkState">
            <el-radio-group v-model="ruleForm.checkState" @input="changeRadio">
              <el-radio :label="1">同意</el-radio>
              <el-radio :label="2">不同意</el-radio>
            </el-radio-group>
          </el-form-item>

          <el-form-item label="回复标题" prop="replyTitle">
            <el-input
              placeholder="回复标题"
              v-model="ruleForm.replyTitle"
              clearable
              style="width: 60%"
            >
            </el-input>
          </el-form-item>

          <el-form-item label="回复内容" prop="replyContent">
            <el-input
              type="textarea"
              :rows="2"
              placeholder="回复内容"
              v-model="ruleForm.replyContent"
            >
            </el-input>
          </el-form-item>
        </el-form>
      </div>

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="submitForm('ruleForm')"
          >审 核</el-button
        >
      </div>
    </el-dialog>
  </el-container>
</template>

<script>
import { GetAllCheckApply, CheckApply } from "@/https/office/approve.js";
export default {
  data() {
    return {
      tableData: [],
      dialogVisible: false,
      // 用于计算表格序号
      pageSize: 10,
      currentPage: 1,

      row: {
        userName: "",
        createTime: "",
        applyTypeName: "",
        content: "",
      },

      ruleForm: {
        checkState: 1,
        id: "",
        applyType: "",
        replyTitle: "",
        replyContent: "",
      },
      rules: {
        checkState: [
          { required: true, message: "请选择同意还是不同意", trigger: "blur" },
        ],
        replyTitle: [
          { required: true, message: "请输入回复标题", trigger: "blur" },
        ],
        replyContent: [
          { required: true, message: "请输入回复内容", trigger: "blur" },
        ],
      },
    };
  },
  created() {
    this.GetData();
  },
  methods: {
    showDialog(row) {
      this.dialogVisible = true;
      console.log(row, "row");
      // 赋值申请信息
      this.row.userName = row.user.userName;
      this.row.createTime = row.createTime;
      this.row.applyTypeName = row.applyTypeName;
      this.row.content = row.content;

      // 赋值给审核信息
      this.ruleForm.id = row.id;
      this.ruleForm.applyType = row.applyType;
      this.ruleForm.replyTitle = `您的${row.applyTypeName}申请,审批已完成！`;
      this.ruleForm.replyContent = "批准！";
    },
    changeRadio(value) {
      value === 1
        ? (this.ruleForm.replyContent = "批准！")
        : (this.ruleForm.replyContent = "不批准！");
    },
    // 提交审核
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          CheckApply(this.ruleForm)
            .then((res) => {
              this.$message.success(res.msg);
              this.GetData();
              this.dialogVisible = false;
            })
            .catch(() => {});
        } else {
          this.$message.error("有未完善的信息");
          return false;
        }
      });
    },
    GetData() {
      // 获取待申请的记录
      GetAllCheckApply()
        .then((res) => {
          console.log("待审批的申请", res);
          // 加工列表数据，为每个对象添加一个新的属性
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
            // 处理时间列
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
};
</script>

<style lang='less' scoped>
.approve {
  height: 100%;

  .el-header {
    height: 40px !important;
    line-height: 40px;
    // background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;
  }

  .top-box {
    border-bottom: 2px solid rgb(242, 242, 242);
    margin-bottom: 10px;

    & > div:nth-child(1) {
      height: 40px;
      padding: 0 20px;
      display: inline-block;
      color: rgb(44, 196, 170);
      border-bottom: 2px solid rgb(44, 196, 170);
    }
  }

  .el-main {
    // 清除默认填充
    padding: 0;
  }
}

// 关闭对话框内容的上下填充
/deep/.el-dialog__body {
  padding-top: 0px;
  padding-bottom: 0px;
}

// 对话框
.dialog-content {
  // border: 1px solid red;

  .dialog-content-title {
    font-size: 1.2rem;
    font-weight: bold;
    border-bottom: 2px solid rgb(242, 242, 242);
  }

  .dialog-content-title1 {
    padding-top: 0px;
    // margin: 10px 0;
  }

  .dialog-content-title2 {
    margin: 10px 0;
  }

  & > div {
    padding: 10px 0;
    display: flex;

    & > span {
      white-space: nowrap;
    }
  }
}
</style>