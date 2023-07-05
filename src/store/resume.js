import {ChangeJoinUsResult} from "@/https/join-us/resume.js";
export default {
  namespaced: true,
  state:{
    // 从应聘列表页面点击结果处理中的入职，跳转到添加档案页面的变量
    resume:null,
    joinUsResultRuleForm:null
  },
  actions:{
    changeJoinUsResult({state},value){
      ChangeJoinUsResult(state.joinUsResultRuleForm).then(res=>{
        console.log('应聘结果',res);
      }).catch(()=>{})
    }
  },
  mutations:{
    RESUME(state,value){
      state.resume = value;
    },
    JOINUSRESULTRULEFORM(state,value){
      state.joinUsResultRuleForm = value;
    }
  }
}