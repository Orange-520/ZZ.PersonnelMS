<template>
  <div class="login">
    <!-- 卡片 -->
    <div class="login_box">
      <div class="title">ZZ人事管理系统</div>
      <div>
        <el-form label-position="left" label-width="auto" :model="ruleForm">
          <el-form-item label="账号">
            <el-input v-model="ruleForm.userName"></el-input>
          </el-form-item>
          <el-form-item label="密码">
            <el-input v-model="ruleForm.password"></el-input>
          </el-form-item>
          <el-form-item>
            <el-button @click="login">登录</el-button>
          </el-form-item>
        </el-form>
      </div>
    </div>
  </div>
</template>

<script>
import { Login } from "@/https/identity/identity.js";
import { mapActions } from "vuex";
export default {
  data() {
    return {
      ruleForm: {
        userName: "",
        password: "",
      },
    };
  },
  methods: {
    // 获取用户信息
    ...mapActions(["getUserMessage"]),
    // 登录
    async login() {
      window.localStorage.removeItem("token");

      await Login({
        userName: this.ruleForm.userName,
        password: this.ruleForm.password,
      })
        .then( res => {
          console.log("登录", res);
          window.localStorage.setItem("token", res.data);

          // 获取用户信息
          this.getUserMessage().then(() => {
            this.$router.push({ path: "/home/office/message" });
          });
        })
        .catch(() => {});
    },
  },
};
</script>

<style lang='less' scoped>
.login {
  height: 100%;
  // border: 1px red solid;
  display: flex;
  justify-content: center;
  align-items: center;

  .login_box {
    // border: 1px red solid;
    padding: 20px;
    box-shadow: 0 0 16px #3098ff;

    .title {
      padding: 0 0 22px 0;
      font-size: 2rem;
      // 文字间距
      letter-spacing: 0.4rem;
    }
  }
}
</style>