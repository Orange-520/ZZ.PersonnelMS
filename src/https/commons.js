import http from '@/assets/js/http.js';

// 获取所有部门
function GetAllDepartment() {
  return http({
    method: "GET",
    url: "/Commons/GetAllDepartment"
  });
}

// 寻找部门下的职位
function FindPositionByDepartment(id) {
  return http({
    method: "GET",
    url: "/Commons/FindPositionByDepartment?id="+id
  });
}



export {
  GetAllDepartment,FindPositionByDepartment
}