<template>
  <div class="header">
    <div class="left">
      <div>
        <i class="el-icon-s-fold icon"></i>
      </div>
    </div>

    <div class="right">
      <div>
        <i class="el-icon-refresh-right icon"></i>
      </div>
      <div>
        <i class="el-icon-rank icon"></i>
      </div>
      <div>
        <i class="el-icon-s-custom icon"></i>
        <span>管理员</span>
      </div>
      <div>
        <i class="el-icon-arrow-right icon"></i>
        <span>注销</span>
      </div>
    </div>
    <!-- <ul class="nav-left">
      <li class="nav-item">
        <i class="el-icon-s-unfold"></i>
      </li>
      <li class="nav-item">
        <i class="el-icon-s-unfold"></i>
      </li>
    </ul> -->
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  computed:{
    ...mapState(['userMessage'])
  },
}
</script>

<style lang='less' scoped>
  .header{
    position: fixed;
    top: 0;
    left: 220px;
    right: 0;
    width: calc(100% - 220px);
    height: 50px;
    background: #1E9FFF;
    box-shadow: -2px 0px 2px 0 rgb(0 0 0 / 10%);
    // border: 1px red solid;
    display: flex;
    justify-content: space-between;
    align-items: center;

    .left{
      display: flex;
      position: absolute!important;
      left: 0px;
      padding-left: 30px;

      .icon{
        margin-right: 40px;
      }
      div{
        display: flex;
      }
    }
    
    .right{
      display: flex;
      position: absolute!important;
      right: 0;
      padding-right: 20px;

      .icon{
        margin-left: 40px;
      }
      span{
        font-size: 0.9rem;
        color: #fff;
        margin-left: 2px;
      }
      div{
        display: flex;
      }
    }

    .icon{
      font-size: 1.2rem;
      color: #fff;
    }
    // .nav-left{
    // border: 1px red solid;
    //   position: relative;
    //   padding: 0 10px;
    //   left: 0px;
    //   top: 0;
    // }

    // .nav-item{
    //   position: absolute!important;
    //   display: inline-block;
    //   vertical-align: middle;
    //   margin: 0 20px;
    //   height: 50px;
    //   line-height: 50px;
    // border: 1px red solid;
    // }
  }
  
</style>