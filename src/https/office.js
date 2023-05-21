import http from '@/assets/js/http.js';

function AddHiringNeedApply(data) {
  return http({
    method: "POST",
    url: "/Office/AddHiringNeedApply",
    data,
  });
}

export {
  AddHiringNeedApply
}