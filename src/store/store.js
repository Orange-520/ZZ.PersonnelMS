import Vue from "vue";
import Vuex from "vuex";

import tags from './tags'

Vue.use(Vuex);

const state = {
  baseURL: 'https://localhost:7147',
  fileUploadURL:'/Commons/Upload',
};
const actions = {};
const mutations = {};
const getters = {
  baseURL:state=>state.baseURL
}

export default new Vuex.Store({
  state,
  actions,
  mutations,
  getters,
  modules:{
    tags
  }
});