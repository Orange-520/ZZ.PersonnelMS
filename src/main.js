import Vue from 'vue'
import App from './App.vue'

import store from '@/store/store';

//路由
import VueRouter from 'vue-router';
Vue.use(VueRouter);
import router from './router/index.js';

import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
Vue.use(ElementUI);

Vue.config.productionTip = false;

new Vue({
  render: h => h(App),
  store,
  router,
  beforeCreate() {
		Vue.prototype.$bus = this //安装全局事件总线
	},
}).$mount('#app')
