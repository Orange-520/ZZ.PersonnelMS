export default {
  namespaced:true,
  state:{
    stateTagsList:[
      // {
      //   name: "消息提醒",
      //   url: '/home/office/message',
      // },
    ]
  },
  actions:{},
  mutations:{
    mutationSelectTags(state, data){
      let result = false
      for(let i=0; i<state.stateTagsList.length;i++){
        if(state.stateTagsList[i].url == data.url){
          return result = true
        }
      }
      result === false ? state.stateTagsList.push(data) : ''
    },
    mutationCloseTag(state, data){
      let result = state.stateTagsList.findIndex(item => item.url === data.url)
      state.stateTagsList.splice(result, 1)
    }
  }
}