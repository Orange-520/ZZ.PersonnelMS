<template>
  <el-container class="department">
    <el-header>
      <div class="top-box">
        <span
        >部门管理 <el-button type="success" size="small" @click="showAddDialog"
          >添加根部门</el-button
        >
      </span>
      </div>
    </el-header>

    <el-main>
      <el-table
        class="table-box"
        :data="departmentList"
        style="width: 100%; margin-bottom: 20px"
        row-key="id"
        border
        default-expand-all
        :tree-props="{ children: 'childrenDepartment' }"
      >
        <el-table-column prop="name" label="名称"> </el-table-column>

        <el-table-column prop="description" label="描述"> </el-table-column>

        <el-table-column fixed="right" label="操作" width="335px">
          <template slot-scope="scope">
            <el-button
              type="success"
              size="mini"
              @click="showAddDialog(scope.row)"
              >添加子部门</el-button
            >
            <el-button
              type="primary"
              size="mini"
              @click="showUpdateDialog(scope.row)"
              >修改</el-button
            >
            <el-button
              type="danger"
              size="mini"
              @click="deleteDepartment(scope.row.id)"
              >删除</el-button
            >
            <el-button
              size="mini"
              @click="
                $router.push({
                  name: 'position',
                  query: {
                    departmentId: scope.row.id,
                    departmentName: scope.row.name,
                  },
                })
              "
              >查看职位</el-button
            >
          </template>
        </el-table-column>
      </el-table>

      <el-dialog
        title="添加部门"
        :visible.sync="departmentDialogVisible"
        center
        width="400px"
        @close="closeAddDialog"
        :modal-append-to-body="false"
        :append-to-body="true"
      >
        <div class="dialog_content">
          <el-form
            :model="departmentForm"
            :rules="departmentRules"
            :hide-required-asterisk="false"
            ref="departmentForm"
            label-width="auto"
            label-position="left"
          >
            <el-form-item label="部门名称" prop="departmentName">
              <el-input v-model="departmentForm.departmentName"></el-input>
            </el-form-item>

            <el-form-item label="部门描述" prop="description">
              <el-input
                v-model="departmentForm.description"
                type="textarea"
                :rows="2"
              />
            </el-form-item>
          </el-form>
        </div>

        <div slot="footer" class="dialog-footer">
          <el-button @click="departmentDialogVisible = false">取 消</el-button>
          <el-button type="primary" @click="submit('departmentForm')"
            >添 加</el-button
          >
        </div>
      </el-dialog>

      <el-dialog
        title="编辑部门"
        :visible.sync="updateDepartmentDialogVisible"
        center
        width="400px"
        @close="closeUpdateDialog"
        :modal-append-to-body="false"
      >
        <div class="dialog_content">
          <el-form
            :model="updateDepartmentForm"
            :rules="departmentRules"
            :hide-required-asterisk="false"
            ref="updateDepartmentForm"
            label-width="auto"
            label-position="left"
          >
            <el-form-item label="部门名称" prop="departmentName">
              <el-input
                v-model="updateDepartmentForm.departmentName"
              ></el-input>
            </el-form-item>

            <el-form-item label="部门描述" prop="description">
              <el-input
                v-model="updateDepartmentForm.description"
                type="textarea"
                :rows="2"
              />
            </el-form-item>
          </el-form>
        </div>

        <div slot="footer" class="dialog-footer">
          <el-button @click="updateDepartmentDialogVisible = false"
            >取 消</el-button
          >
          <el-button
            type="primary"
            @click="submitUpdate('updateDepartmentForm')"
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
  AddDepartment,
  UpdateDepartment,
  DeleteDepartment,
} from "@/https/department.js";
export default {
  data() {
    return {
      departmentDialogVisible: false,
      departmentForm: {
        departmentName: "",
        description: "",
        parentDepartmenId: null,
      },
      departmentRules: {
        departmentName: [
          { required: true, message: "部门名称不能为空", trigger: "blur" },
        ],
      },

      departmentId: null,
      updateDepartmentDialogVisible: false,
      updateDepartmentForm: {
        departmentName: "",
        description: "",
        departmentId: null,
      },
    };
  },
  computed: {
    ...mapState("department", ["departmentList"]),
  },
  created() {
    this.getAllDepartment();
  },
  methods: {
    ...mapActions("department", ["getAllDepartment"]),
    showAddDialog(row) {
      console.log(row);
      // 准备好父部门的id
      this.departmentForm.parentDepartmenId = row.id;
      this.departmentDialogVisible = true;
    },
    closeAddDialog() {
      this.departmentForm.departmentName = "";
      this.departmentForm.description = "";
      this.$refs["departmentForm"].clearValidate();
    },
    submit(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          AddDepartment(this.departmentForm)
            .then((res) => {
              console.log("添加部门结果", res);
              this.$message.success(res.msg);
              this.getAllDepartment();
              this.departmentDialogVisible = false;
            })
            .catch(() => {});
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    closeUpdateDialog() {
      this.updateDepartmentForm.departmentName = "";
      this.updateDepartmentForm.description = "";
      this.$refs["updateDepartmentForm"].clearValidate();
    },
    showUpdateDialog(row) {
      console.log(row);
      this.departmentId = row.id;
      this.updateDepartmentForm.departmentName = row.name;
      this.updateDepartmentForm.description = row.description;
      this.updateDepartmentDialogVisible = true;
    },
    submitUpdate(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          UpdateDepartment(this.departmentId, this.updateDepartmentForm)
            .then((res) => {
              console.log("修改部门结果", res);
              this.$message.success(res.msg);
              this.getAllDepartment();
              this.updateDepartmentDialogVisible = false;
            })
            .catch(() => {});
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    deleteDepartment(departmentId) {
      DeleteDepartment(departmentId)
        .then((res) => {
          console.log("删除部门结果", res);
          this.$message.success(res.msg);
          this.getAllDepartment();
        })
        .catch(() => {});
    },
  },
};
</script>

<style lang='less' scoped>
.department {
  height: 100%;

  .el-header {
    height: 40px !important;
    line-height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;

    .top-box {
    }
  }

  .el-main {
    // 清除默认填充
    padding: 0;
  }
}
</style>