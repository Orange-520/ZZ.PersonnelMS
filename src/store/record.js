export default {
  namespaced: true,
  state:{
    record:{},
  },
  actions:{},
  mutations:{
    RECORD(state,value){
      state.record = value;
    }
  }
}