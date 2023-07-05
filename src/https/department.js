import http from '@/assets/js/http.js';

// 添加部门
function AddDepartment(data){
  return http({
    method:'POST',
    url:'/Department/AddDepartment',
    data
  })
}

// 获取所有部门
function GetAllDepartment() {
  return http({
    method: "GET",
    url: "/Department/GetAllDepartment"
  });
}

// 获取一个部门的下一级所有部门
function GetChildDepartment(departmentId) {
  return http({
    method: "GET",
    url: "/Department/GetChildDepartment?departmentId=" + departmentId
  });
}

// 修改部门信息
function UpdateDepartment(departmentId,data) {
  return http({
    method: "PUT",
    url: "/Department/UpdateDepartment?departmentId=" + departmentId,
    data
  });
}

// 删除部门
function DeleteDepartment(departmentId) {
  return http({
    method: "DELETE",
    url: "/Department/DeleteDepartment?departmentId=" + departmentId
  });
}

export {
  AddDepartment,
  GetAllDepartment,
  GetChildDepartment,
  UpdateDepartment,
  DeleteDepartment
}