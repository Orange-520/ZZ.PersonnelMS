import { GetAllDepartment } from "@/https/department.js";

const initState ={
  departmentList:[]
};

export default {
  namespaced: true,
  state:{
    departmentList:[]
  },
  actions:{
    getAllDepartment({commit}){
      GetAllDepartment().then(res=>{
        console.log('部门列表',res);
        commit('DEPARTMENT',res.data);
      }).catch(()=>{});
    }
  },
  mutations:{
    DEPARTMENT(state,value){
      state.departmentList = value;
    },
    // 重置仓库
    resetState(state){
      Object.assign(state,initState);
    }
  }
}