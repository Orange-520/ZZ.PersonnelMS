import http from '@/assets/js/http.js';

// 获取档案列表
function GetRecordList(data) {
  return http({
    method: "POST",
    url: "/JoinUs/Record/GetRecordList",
    data
  });
}

export {
  GetRecordList
}