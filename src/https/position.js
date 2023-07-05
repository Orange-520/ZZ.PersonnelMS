import http from '@/assets/js/http.js';

// 添加职位
function AddPosition(data) {
  return http({
    method: "POST",
    url: "/Position/AddPosition",
    data
  });
}

// 寻找部门下的职位
function GetPositionByDepartment(departmentId) {
  return http({
    method: "GET",
    url: "/Position/GetPositionByDepartment?departmentId="+departmentId
  });
}

// 修改职位信息
function UpdatePosition(positionId,data) {
  return http({
    method: "PUT",
    url: "/Position/UpdatePosition?positionId="+positionId,
    data
  });
}

// 删除职位
function DeletePosition(positionId) {
  return http({
    method: "DELETE",
    url: "/Position/DeletePosition?positionId="+positionId
  });
}

export {
  AddPosition,
  GetPositionByDepartment,
  UpdatePosition,
  DeletePosition
}