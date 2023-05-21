import http from '@/assets/js/http.js';

function LOGIN(data) {
  return http({
    method: "POST",
    url: "/Identity/Login",
    data,
  });
}

export {
  LOGIN
}