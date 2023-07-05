import Vue from "vue";
import Vuex from "vuex";

import tags from './tags.js';
import record from './record.js';
import resume from "./resume.js";
import department from "./department.js";
import position from "./position.js";

import { GetUserMessage,GetAllRoles } from '@/https/identity/identity.js';

Vue.use(Vuex);

const state = {
  baseURL: 'https://localhost:7147',
  fileUploadURL:'/Commons/Upload',
  userMessage:{},
  roles:[]
};

const actions = {
  // 获取用户信息
  getUserMessage({commit}){
    GetUserMessage().then(res=>{
      console.log('用户信息',res);
      commit('USERMESSAGE',res.data);
    }).catch(()=>{});
  },
  // 获取已定义的所有角色
  getAllRoles({commit}){
    GetAllRoles().then(res=>{
      console.log('已定义的所有角色',res);
      commit('ROLES',res.data);
    }).catch(()=>{});
  }
};

const mutations = {
  // 用户信息赋值
  USERMESSAGE(state,value){
    state.userMessage = value;
  },
  // 角色列表赋值
  ROLES(state,value){
    state.roles = value;
  }
};

const getters = {
  baseURL:state=>state.baseURL
}

export default new Vuex.Store({
  state,
  actions,
  mutations,
  getters,
  modules:{
    tags,
    record,
    resume,
    department,
    position,
  }
});