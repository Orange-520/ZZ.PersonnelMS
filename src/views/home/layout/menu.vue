<template>
  <div class="menu">
    <div>
      ZZ人事管理系统
    </div>
    <el-menu :default-active="$route.path" class="el-menu-vertical" background-color="rgb(57, 61, 73)"
      text-color="rgb(153,153,153)" active-text-color="#ffd04b" router>

      <el-submenu index='1'>
        <template slot="title">
          <i class="el-icon-caret-right"></i>
          <span>个人办公</span>
        </template>
        <el-menu-item index="/home/office/message">消息提醒</el-menu-item>
        <el-menu-item index="/home/office/applyList">个人申请</el-menu-item>
        <el-menu-item index="/home/office/approve">申请审批</el-menu-item>
        <el-menu-item index="/home/office/notice">公告通知</el-menu-item>
      </el-submenu>

      <el-submenu index='2'>
        <template slot="title">
          <i class="el-icon-caret-right"></i>
          <span>行政管理</span>
        </template>
        <el-menu-item index="/home/notice/noticePublish">公告发布</el-menu-item>
      </el-submenu>

      <el-submenu index='3'>
        <template slot="title">
          <i class="el-icon-caret-right"></i>
          <span>人事管理</span>
        </template>
        <el-menu-item index="/home/record/addRecord">添加档案</el-menu-item>
        <el-menu-item index="/home/record/dimissionList">离职档案</el-menu-item>
      </el-submenu>

      <el-submenu index='4'>
        <template slot="title">
          <i class="el-icon-caret-right"></i>
          <span>招聘管理</span>
        </template>
        <el-menu-item index="/home/joinUs/hiringNeedsList">招聘需求</el-menu-item>
        <el-menu-item index="/home/joinUs/addResume">应聘登记</el-menu-item>
        <el-menu-item index="/home/joinUs/resumeList">应聘列表</el-menu-item>
        <el-menu-item index="/home/joinUs/personLibrary">人才库</el-menu-item>
        <el-menu-item index="/home/joinUs/dataLibrary">资料库</el-menu-item>
      </el-submenu>
      
    </el-menu>
  </div>
</template>

<script>
export default {

}
</script>

<style lang="less" scoped>
  .menu{
    width: 220px;
    position: fixed;
    left: 0;
    top: 0;
    bottom: 0;
    background: rgb(57, 61, 73);
  }
  
  .el-menu-vertical{
    overflow-x: hidden;
    height: 100%;
  }

  .el-menu {
    border-right: 0;
    // 选中时的背景效果
    .is-active {
      background-color: rgb(43, 46, 55) !important;
      // 选中时的图标颜色(为什么只用了一个类名，其它也类名也受影响了)
      .el-icon-jurassic_user {
        background-color: #ffd04b;  
      }
    }

    // 悬浮时的高亮效果
    .el-menu-item:hover {
      background-color: rgb(43, 46, 55) !important;
      color: #ffd04b !important;
    }

    /deep/ .el-submenu__title:hover {
      background-color: rgb(43, 46, 55) !important;
      color: #ffd04b !important;
    }
  }
</style>