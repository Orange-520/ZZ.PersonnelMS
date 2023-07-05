import VueRouter from "vue-router";

import login from "@/views/login/login.vue";
import home from "@/views/home/home.vue";
import error from "@/views/error/error.vue";

/* 个人办公模块 */
const message = () => import("@/views/home/layout/office/message.vue");
const userApply = () => import("@/views/home/layout/office/user-apply/user-apply.vue");
const applyList = () => import("@/views/home/layout/office/user-apply/apply-list.vue");
const applyHiringNeeds = () => import("@/views/home/layout/office/user-apply/apply-hiring-needs.vue");
const applyAskForLeave = () => import("@/views/home/layout/office/user-apply/apply-ask-for-leave.vue");
const approve = () => import("@/views/home/layout/office/approve.vue");
// 公告通知
const notice = () => import("@/views/home/layout/office/notice.vue");


/* 行政管理模块 */
// 公告发布
const noticePublish = () => import("@/views/home/layout/xz-management/notice-publish.vue");
const departmentAndPosition = () => import("@/views/home/layout/xz-management/department-and-position/department-and-position.vue");
const department = () => import("@/views/home/layout/xz-management/department-and-position/department.vue");
const position = () => import("@/views/home/layout/xz-management/department-and-position/position.vue");


/* 人事管理模块 */
const record = () => import("@/views/home/layout/record/record.vue");
const recordList = ()=>import("@/views/home/layout/record/record-list.vue");
const addRecord = ()=>import("@/views/home/layout/record/add-record.vue");
const updateRecord = ()=>import("@/views/home/layout/record/update-record.vue");
const recordDetail = ()=>import("@/views/home/layout/record/record-detail.vue");
// 离职列表
const dimissionList = () => import("@/views/home/layout/record/dimission-list.vue");


/* 招聘管理模块 */
const hiringNeedsList = () => import("@/views/home/layout/join-us/hiring-needs-list.vue");
const addResume = () => import("@/views/home/layout/join-us/add-resume.vue");
const resumeList = () => import("@/views/home/layout/join-us/resume-list.vue");
const personLibrary = () => import("@/views/home/layout/join-us/person-library.vue");
const dataLibrary = () => import("@/views/home/layout/join-us/data-library.vue");

// 解决冗余导航问题
const originalPush = VueRouter.prototype.push;

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
				/* 个人办公模块 */
				{
					path:'office/message',
					name:'message',
					component:message
				},
				{
					path:'office/userApply',
					redirect:'office/userApply/applyList',
					component:userApply,
					children:[
						{
							path:'applyList',
							name:'applyList',
							component:applyList
						},
						{
							path:'applyHiringNeeds',
							name:'applyHiringNeeds',
							component:applyHiringNeeds
						},
						{
							path:'applyAskForLeave',
							name:'applyAskForLeave',
							component:applyAskForLeave
						}
					]
				},
				{
					path:'office/approve',
					name:'approve',
					component:approve
				},
				{
					path:'office/notice',
					name:'notice',
					component:notice
				},
				/* 行政管理模块 */
				{
					path:'xz-management/noticePublish',
					name:'noticePublish',
					component:noticePublish
				},
				{
					path:'xz-management/departmentAndPosition',
					component:departmentAndPosition,
					redirect:'xz-management/departmentAndPosition/department',
					children:[
						{
							path:'department',
							name:'department',
							component:department
						},
						{
							path:'position',
							name:'position',
							component:position
						},
					]
				},
				/* 人事管理模块 */
				{
					path:'record',
					component:record,
					redirect:'record/recordList',
					children:[
						{
							path:'recordList',
							name:'recordList',
							component:recordList
						},
						{
							path:'addRecord',
							name:'addRecord',
							component:addRecord
						},
						{
							path:'updateRecord',
							name:'updateRecord',
							component:updateRecord
						},
						{
							path:'recordDetail',
							name:'recordDetail',
							component:recordDetail
						}
					]
				},
				{
					path:'record/dimissionList',
					name:'dimissionList',
					component:dimissionList
				},
				/* 招聘管理模块 */
				{
					path:'joinUs/hiringNeedsList',
					name:'hiringNeedsList',
					component:hiringNeedsList
				},
				{
					path:'joinUs/addResume',
					name:'addResume',
					component:addResume
				},
				{
					path:'joinUs/resumeList',
					name:'resumeList',
					component:resumeList
				},
				{
					path:'joinUs/personLibrary',
					name:'personLibrary',
					component:personLibrary
				},
				{
					path:'joinUs/dataLibrary',
					name:'dataLibrary',
					component:dataLibrary
				},
      ]
		},
  ]
});