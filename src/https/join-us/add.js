import http from '@/assets/js/http.js';

// 添加简历
function AddResume(data) {
  return http({
    method: "POST",
    url: "/JoinUs/AddResume",
    data
  });
}

// 添加档案
function AddRecord(data) {
  return http({
    method: "POST",
    url: "/JoinUs/AddRecord",
    data
  });
}


export {
  AddResume,
  AddRecord
}