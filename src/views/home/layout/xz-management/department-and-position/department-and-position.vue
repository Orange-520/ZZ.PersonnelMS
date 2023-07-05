<template>
  <div class="department-and-position">
    <router-view></router-view>
  </div>
</template>

<script>
export default {

};
</script>

<style lang='less' scoped>
.department-and-position {
  // border: 1px solid red;
  display: flex;
  flex-direction: column;
  height: 100%;
}
</style>