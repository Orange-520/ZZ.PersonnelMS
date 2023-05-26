<template>
  <div class="home">
    <Menu></Menu>
    <Header></Header>
    <Tags></Tags>
    <Content></Content>
  </div>
</template>

<script>
import Menu from "./layout/menu.vue";
import Content from "./layout/content.vue";
import Header from "./layout/header.vue";
import Tags from "./layout/tags.vue";
export default {
  name: "home",
  components: { Menu, Content, Header, Tags },
};
</script>

<style lang='less' scoped>
  // .home{
  //   height: 100%;
  // }
</style>