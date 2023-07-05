<template>
  <el-table
    :data="tableData"
    border
    style="width: 100%"
    :fit="true"
    :cell-style="{ 'text-align': 'center', width: 'auto' }"
    :header-cell-style="{ 'text-align': 'center' }"
  >
    <el-table-column label="" width="70" align="left">
      <template slot-scope="scope">
        {{ scope.$index + 1 }}
      </template>
    </el-table-column>

    <el-table-column prop="name" label="证书名称"></el-table-column>

    <el-table-column
      prop="getTime"
      width="120"
      label="获取时间"
      :formatter="TimeColumnFormatter"
    ></el-table-column>

    <el-table-column prop="level" label="等级"></el-table-column>

    <el-table-column
      prop="certificateNumber"
      label="证书编号"
    ></el-table-column>

    <el-table-column prop="organization" label="颁发机构"></el-table-column>

    <el-table-column prop="remark" label="备注"></el-table-column>

    <el-table-column fixed="right" label="操作" width="200px">
      <template slot-scope="scope">
        <el-button
          type="primary"
          size="small"
          @click="showDialogRepertory(scope.row)"
          >修改</el-button
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
</template>

<script>
export default {
  props: {
    tableData: {
      type: Array,
      require: true,
      default: [],
    },
  },
  methods:{
    // 处理申请时间列
    TimeColumnFormatter(row, column, cellValue, index) {
      let t = new Date(cellValue);
      return `${t.getFullYear()}-${
        t.getMonth() + 1
      }-${t.getDate()} ${t.getHours()}:${t.getMinutes()}`;
    },
  }
};
</script>

<style>
</style>