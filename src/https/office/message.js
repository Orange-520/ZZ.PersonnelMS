import http from '@/assets/js/http.js';

// 获取所有消息提醒
function GetAllReply(data) {
  return http({
    method: "POST",
    url: "/Office/Message/GetAllReply",
    data
  });
}

export {
  GetAllReply
}