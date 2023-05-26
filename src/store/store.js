import Vue from "vue";
import Vuex from "vuex";

import tags from './tags'

Vue.use(Vuex);

const state = {};
const actions = {};
const mutations = {};

export default new Vuex.Store({
  state,
  actions,
  mutations,
  modules:{
    tags
  }
});