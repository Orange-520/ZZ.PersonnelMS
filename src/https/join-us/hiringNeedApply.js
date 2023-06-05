import http from '@/assets/js/http.js';

// 获取招聘需求
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