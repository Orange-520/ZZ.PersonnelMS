import axios from "axios";
import { Message } from 'element-ui';

const instance  = axios.create({
  baseURL: 'https://localhost:7147',
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
  Message.error(error.response.data.msg);
  return Promise.reject(error);
});

export default instance;