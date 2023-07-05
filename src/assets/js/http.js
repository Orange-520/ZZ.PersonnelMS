import axios from "axios";
import { Message } from 'element-ui';

// 引入仓库下的store.js文件
import store from "@/store/store.js";

const instance  = axios.create({
  // 对象的一种访问方式
  // baseURL: store.getters['baseURL'],
  baseURL: "https://localhost:7147",
  timeout: 5000,
  headers: {'Authorization': "Bearer "+window.localStorage.getItem('token')}
});

// 添加请求拦截器
instance.interceptors.request.use(function (config) {
  return config;
}, function (error) {
  return Promise.reject(error);
});

// 添加响应拦截器
instance.interceptors.response.use(function (response) {
  return response.data;
}, function (error) {
  // console.log(error,'axios');
  if (error.response.status === 403) {
    Message.error("当前用户没有访问权限");
  }
  else if (error.response.status === 401) {
    Message.error("Jwt过期，请重新登录");
  }else if (error.response.status === 500){
    Message.error("服务器有未经处理的异常");
  }
  else {
    Message.error(error.response.data.msg);
  }
  return Promise.reject(error);
});

export default instance;