<template>
  <div class="menu">
    <div class="logo">
      ZZ人事管理系统
    </div>
    <el-menu :default-active="$route.path" class="el-menu-vertical" background-color="rgb(57, 61, 73)"
      text-color="rgb(210 210 210)" active-text-color="#ffd04b" router>

      <el-submenu v-for="(menu,index) in menuData" 
      :key="index"
      :index='menu.url'
      >
        <template slot="title">
          <i :class="menu.icon"></i>
          <span>{{menu.name}}</span>
        </template>
        <el-menu-item v-for="(item,index) in menu.children" 
        :key="index"
        :index="item.url"
        @click="clickMenu(item)"
        >
        {{item.name}}
        </el-menu-item>
      </el-submenu>
      
    </el-menu>
  </div>
</template>

<script>
export default {
  data(){
    return{
      menuData:[
        {
          icon: 'el-icon-caret-right',
          name: '个人办公',
          url: "0",
          children: [
            {
              name: "消息提醒",
              url: '/home/office/message',
            },
            {
              name: "个人申请",
              url: '/home/office/userApply',
            },
            {
              name: "申请审批",
              url: '/home/office/approve',
            },
            {
              name: "公告通知",
              url: '/home/office/notice',
            },
          ]
        },
        {
          icon: 'el-icon-caret-right',
          name: '行政管理',
          url: "1",
          children: [
            {
              name: "公告发布",
              url: '/home/notice/noticePublish',
            }
          ]
        },
        {
          icon: 'el-icon-caret-right',
          name: '人事管理',
          url: "2",
          children: [
            {
              name: "添加档案",
              url: '/home/record/addRecord',
            },
            {
              name: "离职档案",
              url: '/home/record/dimissionList',
            }
          ]
        },
        {
          icon: 'el-icon-caret-right',
          name: '招聘管理',
          url: "3",
          children: [
            {
              name: "招聘需求",
              url: '/home/joinUs/hiringNeedsList',
            },
            {
              name: "应聘登记",
              url: '/home/joinUs/addResume',
            },
            {
              name: "应聘列表",
              url: '/home/joinUs/resumeList',
            },
            {
              name: "人才库",
              url: '/home/joinUs/personLibrary',
            },
            {
              name: "资料库",
              url: '/home/joinUs/dataLibrary',
            }
          ]
        },
        {
          icon: 'el-icon-caret-right',
          name: '系统管理',
          url: "4",
          children: [
            {
              name: "账号管理",
              url: '/home/system/account',
            },
            {
              name: "部门和职位管理",
              url: '/home/system/departmentAndPosition',
            }
          ]
        },
      ]
    }
  },
  methods:{
    clickMenu(value) {
    //通过vuex将数据存储在store中
        this.$store.commit('tags/mutationSelectTags', value)
    }
  }
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
  
  .logo{
    height: 50px;
    text-align:center;
    color:rgb(255, 255, 255);
    padding:10px 0;
    font-size:1.2rem;
    background: rgb(30,158,255);
    // background: rgb(57, 61, 73);
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