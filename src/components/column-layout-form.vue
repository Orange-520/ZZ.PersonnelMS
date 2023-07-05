<template>
  <!-- 表单 -->
  <el-form
    ref="ruleForm"
    :model="ruleForm"
    :rules="rules"
    :label-width="labelWidth"
    size="small"
  >
    <el-row class="formRowOne">
      <div class="card" v-for="(item, index) in ruleFormOneConfig" :key="index">
        <el-form-item
          :key="item.prop + index"
          :prop="item.prop"
          :label="item.label"
        >
          <template v-if="item.type === 'input'">
            <el-input v-model="ruleForm[item.prop]" />
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

          <template v-if="item.type === 'cascader'">
            <el-cascader
              style="width: 100%"
              ref="elCascader"
              v-model="ruleForm[item.prop]"
              :options="item.optionsList"
              :props="item.cascaderProps"
              @change="changeCascader"
            >
            </el-cascader>
          </template>

          <template v-if="item.type === 'image'">
            <el-upload
              class="avatar-uploader"
              :action="action"
              :show-file-list="false"
              :headers="allowHeader"
              :on-success="handleAvatarSuccess"
              :before-upload="beforeAvatarUpload"
              :on-error="handleAvatarError"
              :on-progress="handleAvatarProgress"
            >
              <el-image
                v-if="ruleForm[item.prop]"
                :src="imageSrc + ruleForm[item.prop]"
                fit="fit"
                class="avatar"
              >
              </el-image>
              <i v-else class="el-icon-plus avatar-uploader-icon"></i>
            </el-upload>
          </template>
        </el-form-item>
      </div>
    </el-row>

    <el-row>
      <template v-for="(item, index) in ruleFormTwoConfig">
        <el-col :span="item.span" :key="index">
          <el-form-item
            :key="item.prop + index"
            :prop="item.prop"
            :label="item.label"
          >
            <template v-if="item.type === 'input'">
              <el-input v-model="ruleForm[item.prop]" />
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

            <template v-if="item.type === 'cascader'">
              <el-cascader
                style="width: 100%"
                ref="elCascader"
                v-model="ruleForm[item.prop]"
                :options="item.optionsList"
                :props="item.cascaderProps"
                @change="changeCascader"
              >
              </el-cascader>
            </template>
          </el-form-item>
        </el-col>
      </template>
    </el-row>
  </el-form>
</template>

<script>
import { isJPGAndIsLt2M } from "@/assets/js/commonFunc.js";
export default {
  props: {
    // 对话框中的表单
    formObj: {
      type: Object,
      require: true,
    },
    // 表单的配置规则
    rules: {
      type: Object,
      require: true,
    },
    // 表单的配置项
    // 对于 Arrary 或 Object 的默认值，通过使用一个函数来返回默认值，Vue会在需要时动态地调用这个函数来生成默认值。
    ruleFormOneConfig: {
      type: Array,
      require: false,
      default: ()=>{
        return [];
      },
    },
    ruleFormTwoConfig: {
      type: Array,
      require: false,
      default: ()=>{
        return [];
      },
    },
    // 表单中提示文字的宽度
    labelWidth: {
      type: String,
      require: false,
      default: "140px",
    },
    action: {
      type: String,
      require: false,
      default:'',
    },
    imageSrc: {
      type: String,
      require: false,
      default:'',
    },
  },
  computed: {
    ruleForm: {
      get() {
        return this.formObj;
      },
      set(value) {
        console.log(value, "改变 ruleForm 的值");
        this.$emit("update:formObj", value); // ruleForm 改变的时候通知父组件
      },
    },
    allowHeader() {
      return {
        Authorization: "Bearer " + window.localStorage.getItem("token"),
      };
    },
  },
  methods: {
    // 清除某一项表单的验证，主要用于select表单控件
    clearValidate(formName, prop) {
      if (this[formName][prop] != "" && this[formName][prop] != null) {
        this.$refs[formName].clearValidate([prop]);
      }
    },
    // 表单数据校验
    formValidate() {
      let result;
      this.$refs["ruleForm"].validate((valid) => {
        result = valid;
        if (!valid) {
          this.$message.error("有未完善的信息，请完善");
          return false;
        }
      });
      return result;
    },
    // 级联选择器改变值时
    changeCascader(event) {
      this.$emit("changeCascader", event);
    },
    // 修改头像
    handleAvatarSuccess(res, file) {
      // 服务器返回的图片地址
      console.log("服务器返回的图片地址", res);
      // 本地获取的文件对象
      // console.log("file,本地获取的文件对象", file);
      // 图片地址赋值
      this.ruleForm.avatar = res.filePath;
      // 暂时绑定图片对象
      // this.ruleForm.avatar = URL.createObjectURL(file.raw);
    },
    handleAvatarError(err, file, fileList) {
      console.log(err, "上传文件失败");
      if (file.status == "fail") {
        this.$message.error(JSON.parse(err.message).message);
      }
    },
    handleAvatarProgress(event, file, fileList) {
      console.log(event, "文件上传时");
      console.log(file, "文件上传时,file");
      console.log(fileList, "文件上传时,fileList");
    },
    // 判断图片格式
    beforeAvatarUpload(file) {
      let objResult = isJPGAndIsLt2M(file);
      if (objResult.code === 200) {
        return objResult.data;
      } else {
        this.$message.error(objResult.msg);
        return objResult.data;
      }
    },
  },
};
</script>

<style lang='less' scoped>
// 罪魁祸首，解决upload添加图片后，有间隙问题
/deep/.el-upload {
  display: block;
}
.avatar-uploader {
  width: 110px;
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  text-align: center;
}
.avatar-uploader:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 110px;
  height: 134.4px;
  line-height: 134.4px;
  text-align: center;
  // border: 1px solid red;
}
.avatar {
  width: 110px;
  height: 134.4px;
  display: block;
  // border: 1px solid green;
}

// ai
.card {
  break-inside: avoid;
}

@media (min-width: 768px) {
  .formRowOne {
    // 分成几列
    column-count: 2;
    // 列间的间距
    // column-gap: 20px;
  }
}
@media (min-width: 1200px) {
  .formRowOne {
    // 列间的边框
    // column-rule-style: solid;
    column-count: 3;
    // 设置列间的间距为0，好像有默认间距。
    column-gap: 0px;
  }
}
</style>