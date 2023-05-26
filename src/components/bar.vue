<template>
  <div>
    <div class="tags">
      <!-- 定义插槽 -->
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  data(){
    return{
      
    }
  }
};
</script>

<style lang='less' scoped>
  .tags{
    height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    line-height: 40px;
    padding-left: 10px;
  }
</style>