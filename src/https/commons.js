import http from '@/assets/js/http.js';

function GetAllDepartment() {
  return http({
    method: "GET",
    url: "/Commons/GetAllDepartment"
  });
}

function FindPositionByDepartment(id) {
  return http({
    method: "GET",
    url: "/Commons/FindPositionByDepartment?id="+id
  });
}



export {
  GetAllDepartment,FindPositionByDepartment
}