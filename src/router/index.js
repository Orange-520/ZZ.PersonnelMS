import VueRouter from "vue-router";

import login from "@/views/login/login.vue";
import home from "@/views/home/home.vue";
import error from "@/views/error/error.vue";

const message = () => import("@/views/home/layout/office/message.vue");
const userApply = () => import("@/views/home/layout/office/user-apply.vue");
// const applyList = () => import("@/views/home/layout/office/apply-list.vue");
// const applyHiringNeeds = () => import("@/views/home/layout/office/apply-hiring-needs.vue");
// const applyAskForLeave = () => import("@/views/home/layout/office/apply-ask-for-leave.vue");
const approve = () => import("@/views/home/layout/office/approve.vue");
const notice = () => import("@/views/home/layout/office/notice.vue");

const noticePublish = () => import("@/views/home/layout/notice/notice-publish.vue");

const addRecord = () => import("@/views/home/layout/record/add-record.vue");
const dimissionList = () => import("@/views/home/layout/record/dimission-list.vue");

const hiringNeedsList = () => import("@/views/home/layout/join-us/hiring-needs-list.vue");
const addResume = () => import("@/views/home/layout/join-us/add-resume.vue");
const resumeList = () => import("@/views/home/layout/join-us/resume-list.vue");
const personLibrary = () => import("@/views/home/layout/join-us/person-library.vue");
const dataLibrary = () => import("@/views/home/layout/join-us/data-library.vue");

// 解决冗余导航问题
const originalPush = VueRouter.prototype.push

VueRouter.prototype.push = function push(location) {
  return originalPush.call(this, location).catch(err => err)
}


export default new VueRouter({
  mode:"hash",
  routes:[
    {
			// 路径不存在则匹配此项
			path:"*",
			component:error
		},
		// 默认路由，默认进入此页面
		{
			path: '/',
			redirect: '/login'
		},
		{
			path: '/login',
			component: login
		},
		{
			path: '/home',
			component: home,
			redirect:'home/office/message',
      children:[
				{
					path:'office/message',
					component:message
				},
				{
					path:'office/userApply',
					// redirect:'office/userApply/applyList',
					component:userApply,
					// children:[
					// 	{
					// 		path:'applyList',
					// 		component:applyList
					// 	},
					// 	{
					// 		path:'applyHiringNeeds',
					// 		component:applyHiringNeeds
					// 	},
					// 	{
					// 		path:'applyAskForLeave',
					// 		component:applyAskForLeave
					// 	}
					// ]
				},
				{
					path:'office/approve',
					component:approve
				},
				{
					path:'office/notice',
					component:notice
				},
				{
					path:'notice/noticePublish',
					component:noticePublish
				},
				{
					path:'record/addRecord',
					component:addRecord
				},
				{
					path:'record/dimissionList',
					component:dimissionList
				},
				{
					path:'joinUs/hiringNeedsList',
					component:hiringNeedsList
				},
				{
					path:'joinUs/addResume',
					component:addResume
				},
				{
					path:'joinUs/resumeList',
					component:resumeList
				},
				{
					path:'joinUs/personLibrary',
					component:personLibrary
				},
				{
					path:'joinUs/dataLibrary',
					component:dataLibrary
				},
      ]
		},
  ]
});