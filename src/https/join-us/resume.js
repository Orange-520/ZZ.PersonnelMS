import http from '@/assets/js/http.js';

// 添加简历
function AddResume(data) {
  return http({
    method: "POST",
    url: "/JoinUs/Resume/AddResume",
    data
  });
}

// 获取应聘列表
function GetResumeList(data) {
  return http({
    method: "POST",
    url: "/JoinUs/Resume/GetResumeList",
    data
  });
}

// 处理简历环节
function ChangeJoinUsStep(data) {
  return http({
    method: "POST",
    url: "/JoinUs/Resume/ChangeJoinUsStep",
    data
  });
}

// 处理简历结果
function ChangeJoinUsResult(data) {
  return http({
    method: "POST",
    url: "/JoinUs/Resume/ChangeJoinUsResult",
    data
  });
}


export {
  AddResume,GetResumeList,ChangeJoinUsStep,ChangeJoinUsResult
}