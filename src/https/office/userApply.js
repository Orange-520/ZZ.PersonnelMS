import http from '@/assets/js/http.js';

// 添加招聘需求
function AddHiringNeedApply(data) {
  return http({
    method: "POST",
    url: "/Office/UserApply/AddHiringNeedApply",
    data,
  });
}

// 添加请假单
function AddAskForLeaveApply(data) {
  return http({
    method: "POST",
    url: "/Office/UserApply/AddAskForLeaveApply",
    data,
  });
}

// 获取所有自己的申请
function GetAllUserApply(data) {
  return http({
    method: "POST",
    url: "/Office/UserApply/GetAllUserApply",
    data
  });
}

export {
  AddHiringNeedApply,
  AddAskForLeaveApply,
  GetAllUserApply
}