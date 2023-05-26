import http from '@/assets/js/http.js';

// 获取所有待审批申请
function GetHiringNeedApply(data) {
  return http({
    method: "POST",
    url: "/JoinUs/HiringNeedApply/GetHiringNeedApply",
    data
  });
}


export {
  GetHiringNeedApply
}