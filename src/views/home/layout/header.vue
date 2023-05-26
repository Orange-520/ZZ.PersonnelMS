<template>
  <div class="header">
    <ul class="nav-left">
      <li class="nav-item">
        <i class="el-icon-s-unfold"></i>
      </li>
    </ul>
  </div>
</template>

<script>
export default {

}
</script>

<style lang='less' scoped>
  .header{
    position: fixed;
    top: 0;
    left: 220px;
    width: 100%;
    height: 50px;
    background: #1E9FFF;
    box-shadow: -2px 0px 2px 0 rgb(0 0 0 / 10%);
    // border: 1px red solid;

    .nav-left{
      position: relative;
      padding: 0 10px;
      left: 0px;
      top: 0;
    }

    .nav-item{
      position: absolute!important;
      display: inline-block;
      vertical-align: middle;
      margin: 0 20px;
      height: 50px;
      line-height: 50px;
    }
  }
  
</style>