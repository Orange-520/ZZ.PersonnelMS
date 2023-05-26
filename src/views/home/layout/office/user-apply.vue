<template>
  <div class="user-apply">
    <!-- 顶部分类 -->
    <Bar>
      <div class="category t_item" v-show="show">
        <el-tag
          class="category-item pointer"
          v-for="(item, index) in tags"
          :key="index"
          :type="item.type"
          @click="showDetail(item)"
          >{{ item.name }}</el-tag
        >
      </div>

      <!-- 详情 -->
      <div class="detail" v-show="!show">
        <span>{{item.name}}</span>
        <span class="pointer s_close" @click="closeDetail">
          <i class="el-icon-close"></i>
        </span>
      </div>
    </Bar>

    <div class="content">
      <!-- 此处尽量改成路由跳转 -->
      <ApplyListVue v-if="item.value === -1"></ApplyListVue>
      <ApplyHiringNeedsVue v-else-if="item.value === 0"></ApplyHiringNeedsVue>
      <ApplyAskForLeaveVue v-else-if="item.value === 2"></ApplyAskForLeaveVue>
      <!-- <router-view></router-view> -->
    </div>
  </div>
</template>

<script>
import Bar from '@/components/bar.vue';
import ApplyListVue from './apply-list.vue';
import ApplyHiringNeedsVue from './apply-hiring-needs.vue';
import ApplyAskForLeaveVue from './apply-ask-for-leave.vue';

export default {
  components:{
    Bar,
    ApplyListVue,
    ApplyHiringNeedsVue,
    ApplyAskForLeaveVue
  },
  data() {
    return {
      tags: [
        {
          name: "招聘需求",
          value: 0,
          type: "",
          // path:'/home/office/userApply/applyHiringNeeds'
        },
        {
          name: "出差",
          value: 1,
          type: "success",
        },
        {
          name: "请假单",
          value: 2,
          type: "info",
          // path:'/home/office/userApply/applyAskForLeave'
        },
        {
          name: "离职",
          value: 3,
          type: "danger",
        },
        {
          name: "报销",
          value: 4,
          type: "warning",
        },
      ],
      show: true,
      item:{
        name:'',
        value:-1
      }
    };
  },
  methods:{
    showDetail(item){
      this.show = false;
      this.item = {...item};
      // this.$router.push({path:item.path});
    },
    closeDetail(){
      this.show = true;
      this.item.name = '';
      this.item.value = -1;
      // this.$router.push({path:'/home/office/userApply'});
    }
  }
};
</script>

<style lang='less' scoped>
.user-apply {
  height: 100%;
  display: flex;
  flex-direction: column;

  .category{
    height: 40px;
    line-height: 40px;
    // border: 1px solid red;

    .category-item {
      margin-right: 5px;
    }

    .el-tag{
      height: 30px;
      line-height: 30px;
    }
  }

  .detail {
    display: flex;
    justify-content: space-between;
    align-items: center;
    
    & > span:first-child {
      font-size: 0.92rem;
      color: rgba(0,0,0,0.8);
    }

    .s_close{
      width: 40px;
      height: 40px;
      display: flex;
      justify-content: center;
      align-items: center;
    }
  }
  
  // 内容
  .content{
    padding: 10px;
    flex: 1;
    // width: 50%;
  }
}


</style>