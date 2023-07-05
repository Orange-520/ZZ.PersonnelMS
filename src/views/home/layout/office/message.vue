<template>
  <el-container class="message">
    <!-- 左部类型选择 -->
    <el-aside width="200px">
      <div class="tree-box">
        <div>类型汇总</div>
        <div>
          <el-tree
            :data="treeList"
            :props="defaultProps"
            :highlight-current="true"
            :default-expand-all="true"
            @node-click="handleNodeClick"
          >
            <span slot-scope="{ node, data }" class="slot-t-node">
              <template>
                <i
                  :class="{
                    'el-icon-folder': !node.expanded && data.type !== 2, // 节点收缩时的图标
                    'el-icon-folder-opened': node.expanded && data.type !== 2, // 节点展开时的图标
                    'el-icon-document': data.type === 2, // data.type是后端配合提供的识别字段，最后一级
                  }"
                  style="color: #409eff; padding-right: 5px"
                />
                <span>{{ node.label }}</span>
              </template>
            </span>
          </el-tree>
        </div>
      </div>
    </el-aside>

    <el-container>
      <el-header>
        <!-- 顶部操作区 -->
        <div class="top-box">
          <span class="top-box-item input-1">
            <el-input
              v-model="searchObj.keyWord"
              placeholder="请输入内容关键字"
              style="width: auto"
              size="small"
            ></el-input>
          </span>

          <span class="top-box-item input-2">
            <el-date-picker
              v-model="searchObj.startTime"
              type="date"
              placeholder="选择开始日期"
              size="small"
              value-format="yyyy-MM-dd"
            >
            </el-date-picker>
          </span>

          <span class="top-box-item input-2">
            <el-date-picker
              v-model="searchObj.endTime"
              type="date"
              placeholder="选择结束日期"
              size="small"
              value-format="yyyy-MM-dd"
            >
            </el-date-picker>
          </span>

          <span class="top-box-item input-3">
            <el-select
              v-model="searchObj.hasRead"
              placeholder="状态"
              size="small"
            >
              <el-option
                v-for="item in options"
                :key="item.value"
                :label="item.label"
                :value="item.value"
              >
              </el-option>
            </el-select>
          </span>

          <span>
            <el-button type="primary" size="small" @click="search"
              >查询</el-button
            >
          </span>

          <span>
            <el-button type="success" size="small" @click="reset"
              >清空</el-button
            >
          </span>
        </div>
      </el-header>

      <el-main>
        <!-- 使用 Container 布局容器，便可实现区域内滚动 -->
        <!-- 为表格设置高度便可固定表头 -->
        <el-table
          class="table-box"
          :data="tableData"
          border
          height="100%"
          style="width: 100%"
          :fit="true"
          :cell-style="{ 'text-align': 'center', width: 'auto' }"
          :header-cell-style="{ 'text-align': 'center' }"
        >
          <el-table-column label="" width="70" align="left">
            <template slot-scope="scope">
              {{
                scope.$index +
                1 +
                (searchObj.pageIndex - 1) * searchObj.pageSize
              }}
            </template>
          </el-table-column>

          <el-table-column prop="hasRead" label="状态">
            <template>
              <el-badge is-dot class="item" :hidden="false">
                <i class="el-icon-message hasRead"></i>
              </el-badge>
            </template>
          </el-table-column>

          <el-table-column prop="replyTypeName" label="类型"></el-table-column>

          <el-table-column
            prop="replyTitle"
            label="标题"
            :show-overflow-tooltip="true"
          ></el-table-column>

          <el-table-column
            prop="publisherUser.userName"
            label="发布人"
          ></el-table-column>

          <el-table-column
            prop="creationTime"
            label="发布时间"
          ></el-table-column>

          <el-table-column fixed="right" label="操作" width="200px">
            <template slot-scope="scope">
              <el-button
                type="primary"
                size="small"
                @click="showDialogRepertory(scope.row)"
                >详情</el-button
              >
              <el-button
                type="danger"
                size="small"
                @click="showDialogRepertory(scope.row)"
                >删除</el-button
              >
            </template>
          </el-table-column>
        </el-table>
      </el-main>

      <el-footer>
        <!-- 分页 -->
        <el-pagination
          class="pagination_box"
          background
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          :current-page.sync="searchObj.pageIndex"
          :page-sizes="pageSizes"
          :page-size="searchObj.pageSize"
          layout="total, sizes, prev, pager, next, jumper"
          :total="total"
        >
        </el-pagination>
      </el-footer>
    </el-container>
  </el-container>
</template>

<script>
import { GetAllReply } from "@/https/office/message.js";
export default {
  data() {
    return {
      treeList: [
        {
          label: "全部",
          type: 1,
          children: [
            {
              label: "个人申请",
              type: 2,
              value: 1,
            },
            {
              label: "公告通知",
              type: 2,
              value: 2,
            },
            {
              label: "人事档案",
              type: 2,
              value: 3,
            },
          ],
        },
      ],
      defaultProps: {
        children: "children",
        label: "label",
      },
      pickerOptions: {
        shortcuts: [
          {
            text: "最近一周",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近一个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近三个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              picker.$emit("pick", [start, end]);
            },
          },
        ],
      },
      options: [
        {
          label: "已读",
          value: 1,
        },
        {
          label: "未读",
          value: 2,
        },
      ],

      tableData: [],
      time: "",
      // 搜索
      searchObj: {
        pageIndex: 1,
        pageSize: 10,
        keyWord: null,
        startTime: null,
        endTime: null,
        hasRead: null,
        replyType: null,
      },
      // 多少条每页
      pageSizes: [10, 20, 50, 100],
      // 总页数
      total: 0,
    };
  },
  created() {
    this.GetData();
  },
  methods: {
    // 搜索
    search() {
      this.GetData();
    },
    // 清空
    reset() {
      let searchObj = {
        pageIndex: 1,
        pageSize: 10,
        keyWord: null,
        startTime: null,
        endTime: null,
        hasRead: null,
        replyType: null,
      };
      Object.assign(this.searchObj, searchObj);
      this.GetData();
    },
    // pageSize改变时调用
    handleSizeChange(val) {
      // 改变每页显示条目个数
      this.searchObj.pageSize = val;
      this.GetData();
    },
    // 当前页触发调用
    handleCurrentChange(val) {
      this.searchObj.pageIndex = val;
      this.GetData();
    },
    // 获取数据
    GetData() {
      GetAllReply(this.searchObj)
        .then((res) => {
          console.log("消息提醒列表", res);
          this.total = res.total;
          this.tableData = res.data.map((item) => {
            // 为每一项添加一个新属性
            // writable:属性的值是否能被赋值运算符更改
            Object.defineProperty(item, "replyTypeName", {
              value: "",
              writable: true,
            });
            switch (item.replyType) {
              case 1:
                item.replyTypeName = "个人申请";
                break;
              case 2:
                item.replyTypeName = "公告通知";
                break;
              case 3:
                item.replyTypeName = "人事档案";
                break;
            }
            let t = new Date(item.creationTime);
            item.creationTime = `${t.getFullYear()}-${
              t.getMonth() + 1
            }-${t.getDate()} ${t.getHours()}:${t.getMinutes()}:${t.getSeconds()}`;
            return item;
          });
        })
        .catch(() => {});
    },

    // 申请类型改变时
    handleNodeClick(data) {
      if (data.type === 1) {
        this.searchObj.replyType = null;
        this.GetData();
        return;
      }
      this.searchObj.replyType = data.value;
      this.GetData();
      console.log(data);
    },
  },
};
</script>

<style lang='less' scoped>
.message {
  height: 100%;

  .el-aside {
    // 节点控件
    .tree-box {
      // margin-top:10px;
      margin: 10px 5px 0 5px;
      border: 2px solid rgb(248, 248, 248);

      & > div:first-child {
        text-align: center;
        padding: 10px 0;
        background: rgb(248, 248, 248);
        color: rgba(0, 0, 0, 0.8);
      }
    }
  }

  .el-header {
    height: 40px !important;
    line-height: 40px;
    background: rgb(242, 242, 242);
    white-space: nowrap;
    padding: 0 10px;

    .top-box {
      display: flex;

      & > span {
        padding-right: 10px;
      }
    }
  }

  .el-main {
    padding: 0;

    .table-box {
      .el-badge {
        margin: 10px;
      }
      .el-icon-message {
        font-size: 2rem;
      }
      /deep/.el-badge__content {
        margin-top: 5px;
        margin-right: 2px;
      }
    }
  }

  .el-footer {
    z-index: 3;
    height: 60px;
    display: flex;
    justify-content: flex-end;
    align-items: center;
    box-shadow: 0 -2px 2px 0 rgb(0 0 0 / 10%);
    overflow: hidden;
  }
}
</style>