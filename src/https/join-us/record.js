import http from '@/assets/js/http.js';

// 添加档案
function AddRecord(data) {
  return http({
    method: "POST",
    url: "/JoinUs/Record/AddRecord",
    data
  });
}

// 获取档案列表
function GetRecordList(data) {
  return http({
    method: "POST",
    url: "/JoinUs/Record/GetRecordList",
    data
  });
}



export {
  AddRecord,
  GetRecordList
}