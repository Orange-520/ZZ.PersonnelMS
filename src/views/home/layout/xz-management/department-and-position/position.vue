<template>
  <el-container class="position">
    <el-header>
      <!-- 顶部 -->
      <div class="top-box">
        <span>
          {{ $route.query.departmentName }}
          <el-button size="small" type="primary" @click="showAddDialog"
            >新增职位</el-button
          ></span
        >
        <span
          class="pointer s_close"
          @click="$router.push({ name: 'department' })"
        >
          <i class="el-icon-close"></i>
        </span>
      </div>
    </el-header>

    <el-main>
      <el-table
        class="table-box"
        :data="positionList"
        border
        height="100%"
        style="width: 100%"
        :fit="true"
        :cell-style="{ 'text-align': 'center', width: 'auto' }"
        :header-cell-style="{ 'text-align': 'center' }"
      >
        <el-table-column label="" width="70" align="left">
          <template slot-scope="scope">
            {{ scope.$index + 1 }}
          </template>
        </el-table-column>

        <el-table-column prop="name" label="职位名称"></el-table-column>

        <el-table-column prop="description" label="职位描述"></el-table-column>

        <el-table-column fixed="right" label="操作" width="320px">
          <template slot-scope="scope">
            <el-button
              type="success"
              size="mini"
              @click="showUpdateDialog(scope.row)"
              >编辑</el-button
            >
            <el-button
              type="danger"
              size="mini"
              @click="deletePosition(scope.row.id)"
              >删除</el-button
            >
          </template>
        </el-table-column>
      </el-table>

      <el-dialog
        title="添加职位"
        :visible.sync="positionDialogVisible"
        center
        width="400px"
        @close="closeAddDialog"
        :modal-append-to-body="false"
      >
        <div class="dialog_content">
          <el-form
            :model="positionForm"
            :rules="positionRules"
            :hide-required-asterisk="false"
            ref="positionForm"
            label-width="auto"
            label-position="left"
          >
            <el-form-item label="职位名称" prop="positionName">
              <el-input v-model="positionForm.positionName"></el-input>
            </el-form-item>

            <el-form-item label="职位描述" prop="description">
              <el-input
                v-model="positionForm.description"
                type="textarea"
                :rows="2"
              />
            </el-form-item>
          </el-form>
        </div>

        <div slot="footer" class="dialog-footer">
          <el-button @click="positionDialogVisible = false">取 消</el-button>
          <el-button type="primary" @click="submit('positionForm')"
            >添 加</el-button
          >
        </div>
      </el-dialog>

      <el-dialog
        title="编辑职位"
        :visible.sync="updatePositionDialogVisible"
        center
        width="400px"
        @close="closeUpdateDialog"
        :modal-append-to-body="false"
      >
        <div class="dialog_content">
          <el-form
            :model="updatePositionForm"
            :rules="positionRules"
            :hide-required-asterisk="false"
            ref="updatePositionForm"
            label-width="auto"
            label-position="left"
          >
            <el-form-item label="职位名称" prop="positionName">
              <el-input v-model="updatePositionForm.positionName"></el-input>
            </el-form-item>

            <el-form-item label="职位描述" prop="description">
              <el-input
                v-model="updatePositionForm.description"
                type="textarea"
                :rows="2"
              />
            </el-form-item>
          </el-form>
        </div>

        <div slot="footer" class="dialog-footer">
          <el-button @click="updatePositionDialogVisible = false"
            >取 消</el-button
          >
          <el-button type="primary" @click="submitUpdate('updatePositionForm')"
            >修 改</el-button
          >
        </div>
      </el-dialog>
    </el-main>
  </el-container>
</template>

<script>
import { mapActions, mapState } from "vuex";
import {
  AddPosition,
  UpdatePosition,
  DeletePosition,
} from "@/https/position.js";
export default {
  data() {
    return {
      positionDialogVisible: false,
      positionForm: {
        positionName: "",
        description: "",
        departmentId: null,
      },
      positionRules: {
        positionName: [
          { required: true, message: "职位名称不能为空", trigger: "blur" },
        ],
      },

      positionId: null,
      updatePositionDialogVisible: false,
      updatePositionForm: {
        positionName: "",
        description: "",
        departmentId: null,
      },
    };
  },
  computed: {
    ...mapState("position", ["positionList"]),
  },
  // 路由前置守卫
  beforeRouteEnter(to, from, next) {
    // 去哪的
    console.log(to, "to");
    // 哪来的
    console.log(from, "from");
    if (to.query.departmentId === undefined) {
      // 如果没有departmentId参数，哪来的滚哪去
      next({ path: from.path });
    } else {
      next();
    }
  },
  beforeDestroy(){
    // 页面销毁前重置positionList
    this.$store.commit('position/resetState');
  },
  created() {
    // 路由信息
    console.log(this.$route, "route");
    this.positionForm.departmentId = this.$route.query.departmentId;
    this.getPositionByDepartment(this.$route.query.departmentId);
  },
  methods: {
    ...mapActions("position", ["getPositionByDepartment"]),
    showAddDialog() {
      this.positionDialogVisible = true;
    },
    closeAddDialog() {
      this.positionForm.positionName = "";
      this.positionForm.description = "";
      this.$refs["positionForm"].clearValidate();
    },
    submit(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          AddPosition(this.positionForm)
            .then((res) => {
              console.log("添加职位结果", res);
              this.$message.success(res.msg);
              this.getPositionByDepartment(this.$route.query.departmentId);
              this.positionForm.positionName = "";
              this.positionForm.description = "";
              this.positionDialogVisible = false;
            })
            .catch(() => {});
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    closeUpdateDialog() {
      this.updatePositionForm.positionName = "";
      this.updatePositionForm.description = "";
      this.$refs["updatePositionForm"].clearValidate();
    },
    showUpdateDialog(row) {
      console.log(row);
      this.positionId = row.id;
      this.updatePositionForm.positionName = row.name;
      this.updatePositionForm.description = row.description;
      this.updatePositionDialogVisible = true;
    },
    submitUpdate(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          UpdatePosition(this.positionId, this.updatePositionForm)
            .then((res) => {
              console.log("修改职位结果", res);
              this.$message.success(res.msg);
              this.getPositionByDepartment(this.$route.query.departmentId);
              this.updatePositionDialogVisible = false;
            })
            .catch(() => {});
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    deletePosition(positionId) {
      DeletePosition(positionId)
        .then((res) => {
          console.log("删除职位结果", res);
          this.$message.success(res.msg);
          this.getPositionByDepartment(this.$route.query.departmentId);
        })
        .catch(() => {});
    },
  },
};
</script>

<style lang='less' scoped>
// .roleDialog {
//   /deep/.el-dialog__body {
//     padding-top: 0;
//   }

//   .dialog_content div {
//     margin: 20px 0;
//   }
//   .dialog_content div:last-child {
//     margin: 0;
//   }
// }

.position {
  height: 100%;

  .el-header {
    height: 40px !important;
    line-height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;

    .top-box {
      display: flex;
      justify-content: space-between;
    }
  }

  .el-main {
    // 清除默认填充
    padding: 0;
  }
}
</style>