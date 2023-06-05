<template>
  <div class="record-list">
    <router-view></router-view>
  </div>
</template>

<script>
export default {

};
</script>

<style lang='less' scoped>
.record-list {
  // border: 1px solid red;
  display: flex;
  flex-direction: column;
  height: 100%;
}
</style>