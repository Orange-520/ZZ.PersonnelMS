import { GetPositionByDepartment } from "@/https/position.js";

const initState = {
  positionList:[]
};

export default {
  namespaced: true,
  state:{
    positionList:[]
  },
  actions:{
    async getPositionByDepartment({commit},value){
      return await GetPositionByDepartment(value).then(res=>{
        console.log('该部门下的职位',res);
        commit('POSITION',res.data);
      }).catch(()=>{});
    }
  },
  mutations:{
    POSITION(state,value){
      state.positionList = value;
    },
     // 重置仓库
     resetState(state){
      Object.assign(state,initState);
    }
  }
}