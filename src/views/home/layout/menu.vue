<template>
  <div>
    <el-menu :default-active="$route.path" class="el-menu-vertical-demo" background-color="rgb(57, 61, 73)"
      text-color="rgb(153,153,153)" active-text-color="#ffd04b" router>

      <el-menu-item index="/home/office/message">
        <i class="el-icon-gonggao"></i>
        <span slot="title">消息提醒</span>
      </el-menu-item>

      <el-submenu index="">
        <template slot="title">
          <i class="el-icon-tongji"></i>
          <span>销售查询统计</span>
        </template>
        <el-menu-item index="/home/notice/noticePublish">公告发布</el-menu-item>
      </el-submenu>
      
    </el-menu>
  </div>
</template>

<script>
export default {

}
</script>

<style>

</style>