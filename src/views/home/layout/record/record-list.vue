<template>
  <el-container class="record-list">
    <el-header>
      <!-- 顶部搜索区 -->
      <div class="top-box">
        <span>
          <el-button
            size="small"
            type="primary"
            @click="
              $router.push({
                name:'addRecord'
              })
            "
            >新增用户</el-button
          >
        </span>
        <span>
          <span>
            <el-input
              size="small"
              v-model="searchObj.nameKey"
              placeholder="请输入姓名、登录名"
            ></el-input>
          </span>
          <span>
            <el-cascader
              size="small"
              placeholder="请选择部门"
              ref="elCascader"
              v-model="searchObj.departmentId"
              :options="departmentList"
              :props="cascaderProps"
              @change="changeDepartment"
            >
            </el-cascader>
          </span>
          <span>
            <el-select
              size="small"
              v-model="searchObj.positionId"
              placeholder="请选择职位"
            >
              <el-option
                v-for="(item, index) in positionList"
                :key="index"
                :label="item.name"
                :value="item.id"
              ></el-option>
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
        <el-table-column label="" width="70" align="left">
          <template slot-scope="scope">
            {{
              scope.$index + 1 + (searchObj.pageIndex - 1) * searchObj.pageSize
            }}
          </template>
        </el-table-column>

        <el-table-column prop="name" label="姓名"></el-table-column>

        <el-table-column prop="user.userName" label="登录名"></el-table-column>

        <el-table-column prop="department.name" label="部门"></el-table-column>

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
              @click="showRuleDialog(scope.row)"
              >关联角色</el-button
            >
            <el-button
              type="success"
              size="mini"
              @click="showRecordUpdate(scope.row)"
              >编辑</el-button
            >
            <el-button type="danger" size="mini">删除</el-button>
            <el-button size="mini" @click="showRecordDetail(scope.row)"
              >详情</el-button
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
        :current-page.sync="searchObj.pageIndex"
        :page-sizes="pageSizes"
        :page-size="searchObj.pageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      >
      </el-pagination>
    </el-footer>

    <el-dialog
      title="关联用户角色"
      :visible.sync="roleDialogVisible"
      center
      width="400px"
      @close="closeRoleDialog"
      :modal-append-to-body="false"
      class="roleDialog"
    >
      <div class="dialog_content">
        <div>
          <span>姓名：</span><span>{{ userObj.userName }}</span>
        </div>
        <div>
          <span>部门：</span><span>{{ userObj.department }}</span>
        </div>
        <div>
          <span>职务：</span><span>{{ userObj.position }}</span>
        </div>
        <div>
          <span>用户当前角色信息：</span
          ><span>{{
            userObj.roleName === "" ? "暂无角色信息" : userObj.roleName
          }}</span>
        </div>
        <el-form
          :model="ruleForm"
          :rules="rules"
          :hide-required-asterisk="false"
          ref="ruleForm"
          label-width="auto"
          label-position="left"
        >
          <el-form-item label="选择角色" prop="roleName">
            <el-select
              v-model="ruleForm.roleName"
              style="width: 100%"
              @change="clearValidate('ruleForm', 'roleName')"
            >
              <el-option
                v-for="option in optionsList"
                :key="option.value"
                :label="option.label"
                :value="option.value"
              >
              </el-option>
            </el-select>
          </el-form-item>
        </el-form>
      </div>

      <div slot="footer" class="dialog-footer">
        <el-button @click="roleDialogVisible = false">取 消</el-button>
        <el-button @click="reset('ruleForm')">重 置</el-button>
        <el-button type="primary" @click="foreignRoleAndUser('ruleForm')"
          >关 联</el-button
        >
      </div>
    </el-dialog>
  </el-container>
</template>

<script>
import { GetRecordList } from "@/https/join-us/record.js";
import {
  GetRoleByUserId,
  ForeignRoleAndUser,
} from "@/https/identity/identity.js";
import { mapState,mapActions } from "vuex";
export default {
  data() {
    const initSearchObj = {
      pageIndex: 1,
      pageSize: 10,
      nameKey: null,
      departmentId: null,
      positionId: null,
    };
    const initUserObj = {
      userName: "",
      roleName: "",
      department: "",
      position: "",
    };
    const initRuleForm = {
      id: "",
      roleName: "",
    };
    return {
      initSearchObj,
      initUserObj,
      initRuleForm,

      // 部门级联选择器配置项
      cascaderProps: {
        expandTrigger: "click",
        value: "id",
        label: "name",
        children: "childrenDepartment",
        checkStrictly: true,
      },

      // 人员列表
      tableData: [],
      // 搜索
      searchObj: {
        pageIndex: 1,
        pageSize: 10,
        nameKey: null,
        departmentId: null,
        positionId: null,
      },
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,

      // 关联角色显示对话框
      roleDialogVisible: false,
      userObj: {
        userName: "",
        roleName: "",
        department: "",
        position: "",
      },
      ruleForm: {
        id: "",
        roleName: "",
      },
      rules: {
        roleName: [
          { required: true, message: "请选择需要关联的角色", trigger: "blur" },
        ],
      },
      optionsList: [
        {
          value: "admin",
          label: "管理员",
        },
        {
          value: "personnelInCharge",
          label: "人事部主管",
        },
        {
          value: "itInCharge",
          label: "其它部门主管",
        },
        {
          value: "personnel",
          label: "人事部员工",
        },
        {
          value: "it",
          label: "其它部门员工",
        },
      ],
    };
  },
  computed:{
    ...mapState('department',['departmentList']),
    ...mapState('position',['positionList']),
  },
  created() {
    // 获取部门列表
    this.getAllDepartment();

    this.GetData();
  },
  beforeDestroy(){
    // 页面销毁前重置positionList
    this.$store.commit('position/resetState');
  },
  methods: {
    ...mapActions('department',['getAllDepartment']),
    ...mapActions('position',['getPositionByDepartment']),
    // pageSize改变时调用
    handleSizeChange(val) {
      // 改变每页显示条目个数
      this.searchObj.pageSize = val;
      this.GetData();
    },
    // 当前页触发调用
    handleCurrentChange(val) {
      this.searchObj.currentPage = val;
      this.GetData();
    },
    // 获取档案列表
    GetData() {
      GetRecordList(this.searchObj)
        .then((res) => {
          console.log("档案列表", res);
          this.total = res.total;
          this.tableData = res.data;
        })
        .catch(() => {});
    },
    // 搜索重置
    searchReset() {
      this.$store.commit('position/resetState');
      this.searchObj = { ...this.initSearchObj };
      this.GetData();
    },
    closeRoleDialog() {
      this.userObj = { ...this.initUserObj };
      this.ruleForm = { ...this.initRuleForm };
    },
    reset(formName) {
      this.$refs[formName].resetFields();
    },
    // 清除某一项表单的验证，主要用于select表单控件
    clearValidate(formName, prop) {
      if (this[formName][prop] != "" && this[formName][prop] != null) {
        this.$refs[formName].clearValidate([prop]);
      }
    },
    // 打开关联角色对话框
    showRuleDialog(row) {
      console.log(row);
      // 获取用户对应的角色
      GetRoleByUserId({ userId: row.user.id })
        .then((res) => {
          console.log(res, "用户对应的角色信息");
          this.userObj.userName = row.name;
          if (res.data.length > 0) {
            for (const item of this.optionsList) {
              if (item.value === res.data[0]) {
                this.userObj.roleName = item.label;
              }
            }
          }
          this.userObj.department = row.department.name;
          this.userObj.position = row.position.name;
          this.ruleForm.id = row.user.id;

          this.roleDialogVisible = true;
        })
        .catch(() => {});
    },
    // 为用户关联角色
    foreignRoleAndUser(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          ForeignRoleAndUser(this.ruleForm)
            .then((res) => {
              this.$message.success(res.msg);
              this.roleDialogVisible = false;
            })
            .catch(() => {});
        } else {
          this.$message.error("请为用户选择角色");
          return false;
        }
      });
    },
    // 打开详情
    showRecordDetail(row) {
      this.$store.commit("record/RECORD", row);
      this.$router.push({
        name:'recordDetail'
      });
    },
    // 打开编辑
    showRecordUpdate(row) {
      this.$store.commit("record/RECORD", row);
      this.$router.push({
        name:'updateRecord'
      });
    },
    changeDepartment(value) {
      console.log(value,'改变部门级联选择器');
      this.searchObj.departmentId = value[value.length - 1];
      // 先清除上一次选择的职位
      this.searchObj.positionId = "";
    // 按部门获取职位
      this.getPositionByDepartment(value[value.length - 1]);
    },
    // 处理用工性质列
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
    }
  },
};
</script>

<style lang='less' scoped>
.record-list {
  height: 100%;

  .el-header {
    height: 40px !important;
    line-height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;

    // 顶部搜索区
    .top-box {
      display: flex;
      justify-content: space-between;

      & > span:nth-child(2) {
        display: flex;

        & > span {
          padding-left: 10px;
        }
      }
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

  .roleDialog {
    /deep/.el-dialog__body {
      padding-top: 0;
    }

    .dialog_content div {
      margin: 20px 0;
    }
    .dialog_content div:last-child {
      margin: 0;
    }
  }
}
</style>