import http from '@/assets/js/http.js';

// 登录
function Login(data) {
  return http({
    method: "POST",
    url: "/Identity/Login",
    data,
  });
}

// 获取用户信息
function GetUserMessage() {
  return http({
    method: "POST",
    url: "/Identity/GetUserMessage"
  });
}

// 获取已定义的所有角色
function GetAllRoles() {
  return http({
    method: "POST",
    url: "/Identity/GetAllRoles"
  });
}

// 获取用户对应的角色
function GetRoleByUserId(data) {
  return http({
    method: "POST",
    url: "/Identity/GetRoleByUserId",
    data
  });
}

// 为用户关联角色
function ForeignRoleAndUser(data) {
  return http({
    method: "POST",
    url: "/Identity/ForeignRoleAndUser",
    data
  });
}

export {
  Login,
  GetUserMessage,
  GetAllRoles,
  GetRoleByUserId,
  ForeignRoleAndUser
}