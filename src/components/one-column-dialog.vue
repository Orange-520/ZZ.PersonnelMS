<template>
  <el-dialog
    :title="title"
    :visible.sync="dialogVisible"
    center
    :width="width"
    @close="closeDialog"
    :modal-append-to-body="false"
    :top="top"
  >
    <div class="dialog_content">
      <el-form
        :model="ruleForm"
        :rules="rules"
        :hide-required-asterisk="false"
        ref="ruleForm"
        label-width="auto"
        label-position="left"
      >
        <template v-for="(item, index) in ruleFormConfig">
          <el-form-item :label="item.label" :prop="item.prop" :key="index">
            <template v-if="item.type == 'input'">
              <el-input v-model="ruleForm[item.prop]"></el-input>
            </template>

            <template v-if="item.type === 'textarea'">
              <el-input
                v-model="ruleForm[item.prop]"
                type="textarea"
                :rows="2"
              />
            </template>

            <template v-if="item.type === 'select'">
              <el-select
                v-model="ruleForm[item.prop]"
                style="width: 100%"
                @change="clearValidate('ruleForm', item.prop)"
              >
                <el-option
                  v-for="option in item.optionsList"
                  :key="option.value"
                  :label="option.label"
                  :value="option.value"
                >
                </el-option>
              </el-select>
            </template>

            <template v-if="item.type === 'date-picker'">
              <el-date-picker
                v-model="ruleForm[item.prop]"
                type="date"
                placeholder="选择日期"
                value-format="yyyy-MM-dd"
                style="width: 100%"
              >
              </el-date-picker>
            </template>

            <template v-if="item.type === 'input-number'">
              <el-input-number
                style="width: 100%"
                v-model="ruleForm[item.prop]"
                controls-position="right"
                :precision="2"
                :min="0"
              ></el-input-number>
            </template>
          </el-form-item>
        </template>
      </el-form>
    </div>

    <div slot="footer" class="dialog-footer">
      <el-button @click="dialogVisible = false">取 消</el-button>
      <el-button type="primary" @click="submit('ruleForm')">{{
        btnFont
      }}</el-button>
    </div>
  </el-dialog>
</template>

<script>
export default {
  props: {
    // 对话框标题
    title: {
      type: String,
      require: true,
      default: "",
    },
    // 是否显示
    visible: {
      type: Boolean,
      require: true,
    },
    // 对话框离顶部的距离
    top: {
      type: String,
      require: false,
      default: "15vh",
    },
    // 对话框的宽度
    width: {
      type: String,
      require: false,
      default: "400px",
    },
    btnFont: {
      type: String,
      require: false,
      default: "添 加",
    },
    // 对话框中的表单
    ruleForm: {
      type: Object,
      require: true,
      default: {},
    },
    // 表单的配置规则
    rules: {
      type: Object,
      require: true,
      default: {},
    },
    // 表单的配置项
    ruleFormConfig: {
      type: Array,
      require: true,
      default: [],
    },
  },
  computed: {
    dialogVisible: {
      get() {
        return this.visible;
      },
      set(value) {
        console.log(value, "改变dialogVisible的值");
        this.$emit("update:visible", value); // dialogVisible 改变的时候通知父组件
      },
    },
  },
  data() {
    return {};
  },
  methods: {
    // 提交表单
    submit(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          // 注册自定义事件。调用，传递值
          this.$emit("submitData", this.ruleForm);
        } else {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
    },
    closeDialog() {},
    // 清除某一项表单的验证，主要用于select表单控件
    clearValidate(formName, prop) {
      if (this[formName][prop] != "" && this[formName][prop] != null) {
        this.$refs[formName].clearValidate([prop]);
      }
    },
  },
};
</script>

<style>
</style>