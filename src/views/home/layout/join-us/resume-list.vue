<template>
  <div class="resume-list">
    <!-- 顶部搜索区 -->
    <div class="top-box">
      <span>
        <el-input
          placeholder="请输入应聘者姓名"
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

        <el-table-column
          prop="joinUsStep"
          label="当前环节"
          :formatter="joinUsStepColumnFormatter"
        ></el-table-column>

        <el-table-column
          prop="checkUser.userName"
          label="当前处理人"
        ></el-table-column>

        <el-table-column fixed="right" label="操作" width="200px">
          <template slot-scope="scope">
            <el-button
              type="primary"
              size="small"
              @click="joinUsStepRuleForm.resumeId = scope.row.id ;joinUsStepRuleForm.joinUsStep = scope.row.joinUsStep; joinUsStepDialogVisible = true;"
              >环节处理</el-button
            >
            <el-button
              type="danger"
              size="small"
              @click="joinUsResultRuleForm.resumeId = scope.row.id ; joinUsResultDialogVisible = true;"
              >结果处理</el-button
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

    <!-- 环节处理对话框 -->
      <el-dialog
        title="应聘环节处理"
        :visible.sync="joinUsStepDialogVisible"
        center
        width="400px"
        @close="closeDialog"
        :modal-append-to-body="false"
      >
        <div class="dialog_content">
          <el-form
            :model="joinUsStepRuleForm"
            :rules="joinUsStepRules"
            :hide-required-asterisk="false"
            ref="joinUsStepRuleForm"
            label-width="auto"
            label-position="left"
          >
            <el-form-item label="应聘环节" prop="joinUsStep">
              <el-select
                v-model="joinUsStepRuleForm.joinUsStep"
                style="width: 100%"
              >
                <el-option
                  v-for="option in joinUsStepConfig"
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
          <el-button @click="joinUsStepDialogVisible = false"
            >取 消</el-button
          >
          <el-button
            type="primary"
            @click="changeJoinUsStep('joinUsStepRuleForm')"
            >改 变 环 节</el-button
          >
        </div>
      </el-dialog>

       <!-- 应聘结果对话框 -->
      <el-dialog
        title="应聘结果处理"
        :visible.sync="joinUsResultDialogVisible"
        center
        width="400px"
        @close="closeDialog"
        :modal-append-to-body="false"
      >
        <div class="dialog_content">
          <el-form
            :model="joinUsResultRuleForm"
            :rules="joinUsResultRules"
            :hide-required-asterisk="false"
            ref="joinUsResultRuleForm"
            label-width="auto"
            label-position="left"
          >
            <el-form-item label="应聘环节" prop="joinUsResult">
              <el-select
                v-model="joinUsResultRuleForm.joinUsResult"
                style="width: 100%"
                @change="clearValidate('joinUsResultRuleForm', 'joinUsResult')"
              >
                <el-option
                  v-for="option in joinUsResultConfig"
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
          <el-button @click="joinUsResultDialogVisible = false"
            >取 消</el-button
          >
          <el-button
            type="primary"
            @click="changeJoinUsResult('joinUsResultRuleForm')"
            >应 聘 结 果</el-button
          >
        </div>
      </el-dialog>
  </div>
</template>

<script>
import {GetResumeList,ChangeJoinUsStep,ChangeJoinUsResult} from '@/https/join-us/resumeList.js'
export default {
  data() {
    // 应聘环节
    const joinUsStepConfig = [
        {label:'简历筛选',value:1},
        {label:'初试',value:2},
        {label:'面试',value:3},
        {label:'背景调查',value:4},
        {label:'终面',value:5},
        {label:'录用决策中',value:6},
        {label:'发出录用通知书',value:7},
        {label:'入职培训',value:8},
        {label:'跟踪反馈',value:9}
    ];
    // 应聘结果
    const joinUsResultConfig = [
        {label:'入职',value:1},
        {label:'放入人才库',value:2},
        {label:'放入资料库',value:3},
        {label:'不合格',value:4}
    ];
    
    return {
      joinUsStepConfig,
      joinUsResultConfig,

      joinUsStepDialogVisible:false,
      joinUsResultDialogVisible:false,

      tableData: [],
      ruleForm:{
        pageIndex: 1,
        pageSize: 10,
        nameKey: "",
        // 获取简历结果为 None 的
        "joinUsResult": 0
      },
       // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,

      joinUsStepRuleForm:{
        resumeId: null,
        joinUsStep: null
      },
      joinUsStepRules:{
         joinUsStep: [{ required: true, message: "请选择应聘环节", trigger: "blur" }],
      },

      joinUsResultRuleForm:{
        resumeId: null,
        joinUsResult: null
      },
      joinUsResultRules:{
         joinUsResult: [{ required: true, message: "请选择处理结果", trigger: "blur" }],
      }
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
    // 获取应聘列表
    GetData(){
      GetResumeList(this.ruleForm).then(res=>{
        console.log('应聘列表',res);
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
    // 处理招聘环节列
    joinUsStepColumnFormatter(row, column, cellValue, index){
      for (const item of this.joinUsStepConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    closeDialog(){},
    // 处理应聘环节
    changeJoinUsStep(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          ChangeJoinUsStep(this.joinUsStepRuleForm).then(res=>{
            this.$message.success(res.msg);
            this.GetData();
            this.joinUsStepDialogVisible = false;
          }).catch(()=>{});
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    // 处理应聘结果
    changeJoinUsResult(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          // 如果选择不合格，则弹出提示
          if (this.joinUsResultRuleForm.joinUsResult === 4) {
            this.$confirm('此操作将移除此应聘简历！', '提示', {
              confirmButtonText: '确定',
              cancelButtonText: '取消',
              type: 'warning'
            }).then(()=>{
              // 改变应聘结果
              ChangeJoinUsResult(this.joinUsResultRuleForm).then(res=>{
                this.$message.success(res.msg);
                this.GetData();
                this.joinUsResultDialogVisible = false;
              }).catch(()=>{});
            }).catch(() => {
              return;
            });
          }else{
            // 改变应聘结果
            ChangeJoinUsResult(this.joinUsResultRuleForm).then(res=>{
              this.$message.success(res.msg);
              this.GetData();
              this.joinUsResultDialogVisible = false;
            }).catch(()=>{});
          }
          
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
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
.resume-list {
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