<template>
  <div class="tags">
    <div class="tags-item tags-item1">
      <i class="el-icon-d-arrow-left"></i>
    </div>

    <!-- <div class="tags-item tags-item2">
      <i class="el-icon-s-home"></i>
    </div> -->

    <div class="tags-item tags-item3">
      <el-tag
        closable
        v-for="(tag, index) in tags"
        :key="tag.name"
        :disable-transitions="true"
        :effect="$route.path === tag.url ? 'dark' : 'plain'"
        @close="handleClose(tag, index)"
        @click="handleClick(tag)"
      >
        {{ tag.name }}
      </el-tag>
    </div>

    <div class="tags-item tags-item4">
      <i class="el-icon-d-arrow-right"></i>
    </div>

    <div class="tags-item tags-item5">
      <i class="el-icon-arrow-down"></i>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations } from "vuex";
export default {
  data() {
    return {
      tags: [],
    };
  },
  computed: {
    ...mapState("tags", ["stateTagsList"]),
  },
  created() {
    //stateTagsList是state.js中的存放tags数组的key，stateTagsList的值默认为空数组
    this.tags = this.stateTagsList;
  },
  methods: {
    ...mapMutations('tags',{
      close: "mutationCloseTag",
    }),
    handleClose(tag, index) {
      if (this.tags.length === 1) {
        // 如果只有一个标签则不能关闭
        return;
      }
      this.close(tag); // 删除当前tag
      if (this.$router.path === tag.url) {
        // 如果关闭的标签不是当前路由的话，不做路由跳转
        return;
      } else {
        if (index === this.tags.length - 1) {
          // 关闭最后一个标签,则路由跳转至最后一个
          this.$router.push(this.tags[index].url);
        } else {
          // 路由跳转至下一个标签页
          if (index === 0) {
            this.$router.push(this.tags[0].url);
          } else {
            this.$router.push(this.tags[index - 1].url);
          }
        }
      }
    },
    // 点击tags具体标签
    handleClick(tag) {
      this.$router.push(tag.url);
    },
  },
};
</script>

<style lang="less" scoped>
.tags {
  position: fixed;
  top: 50px;
  right: 0;
  left: 220px;

  height: 40px;
  line-height: 40px;
  background-color: #fff;
  box-shadow: 0 1px 2px 0 rgb(0 0 0 / 10%);

  display: flex;
  justify-content: space-between;

  // z-index: 1;

  & > div {
    width: 40px;
    height: 40px;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 1.2rem;
    
  }

  .tags-item1{
    cursor: pointer;
  }

  .tags-item2{
    cursor: pointer;
  }

  .tags-item3 {
    flex: 1;
    display: flex;
    justify-content: flex-start;
    overflow: hidden;

    .el-tag {
      cursor: pointer;
      height: 40px;
      line-height: 40px;
      font-size: 0.92rem;
      border-radius: 0px;
      border: 0;
      border-right: 1px solid #f5f5f5;
      color: rgba(0,0,0,.85);
    }
    .el-tag--dark {
      color: #1E9FFF;
      background: #fff;
    }
    /deep/.el-tag--dark .el-tag__close {
      color: #c2c2c2 !important;
    }
    /deep/.el-tag--dark .el-tag__close:hover {
      color: #ffffff !important;
    }
    /deep/.el-tag--plain .el-tag__close{
      color: #c2c2c2 !important;
    }
    /deep/.el-tag--plain .el-tag__close:hover{
      color: #ffffff !important;
    }
  }

  .tags-item4{
    cursor: pointer;
  }

  .tags-item5{
    cursor: pointer;
  }
}
</style>