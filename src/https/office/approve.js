import http from '@/assets/js/http.js';

// 获取所有待审批申请
function GetAllCheckApply() {
  return http({
    method: "GET",
    url: "/Office/Approve/GetAllCheckApply"
  });
}

// 审核申请
function CheckApply(data) {
  return http({
    method: "POST",
    url: "/Office/Approve/CheckApply",
    data
  });
}



export {
  GetAllCheckApply,
  CheckApply
}