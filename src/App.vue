<template>
  <div id="app">
    <router-view></router-view>
  </div>
</template>

<script>

export default {
  name: 'App',
  created(){
    //在页面加载时读取sessionStorage里的状态信息
    if(sessionStorage.getItem('storeState')){
      //replaceState，替换store的根状态
      this.$store.replaceState(Object.assign({},this.$store.state,JSON.parse(sessionStorage.getItem('storeState'))))
    }
 
    //在页面刷新时将vuex里的信息保存到sessionStorage里
    window.addEventListener('beforeunload',()=>{
      sessionStorage.setItem('storeState',JSON.stringify(this.$store.state))
    })
  }
}
</script>

<style>
#app {
  height: 100vh;
}

*{
  margin: 0;
  padding: 0; 
  box-sizing: border-box;
  outline: none;
}

li {
    list-style: none;
}

.pointer{
  cursor: pointer;
}
.el-cascader-menu .el-radio {
    display: table;
    vertical-align: middle;
    width: 90%;
    height: 100%;
    position: absolute;
    box-sizing: border-box;
    margin-left: -20px;
    padding-left: 10px;
}
.el-cascader-menu .el-radio .el-radio__input {
    display: table-cell;
    vertical-align: middle;
    visibility: hidden;
}

/* 滚动条 */
::-webkit-scrollbar {
  width: 1px;
  height: 1px;
}
::-webkit-scrollbar-thumb { 
  border-radius: 5px;
  background-color: #d9d9d900;
}
::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px rgb(217, 236, 255,0);
  background: #ededed00;
  border-radius: 5px;
}
</style>
