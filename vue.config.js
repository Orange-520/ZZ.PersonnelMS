// const { defineConfig } = require('@vue/cli-service')
// module.exports = defineConfig({
//   transpileDependencies: true
// })

module.exports = {
  lintOnSave: false, //关闭语法检查
  pages: {
    index: {
      entry: 'src/main.js' //设置入口文件
    }
  },
  // 打包时，添加至所有路径前
  publicPath: "./",
}