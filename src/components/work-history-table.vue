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

    <el-table-column
      prop="startTime"
      width="120"
      label="开始日期"
      :formatter="TimeColumnFormatter"
    ></el-table-column>

    <el-table-column
      prop="endTime"
      width="120"
      label="结束日期"
      :formatter="TimeColumnFormatter"
    ></el-table-column>

    <el-table-column prop="companyName" label="公司名称"></el-table-column>

    <el-table-column prop="companyAddress" label="所在地址"></el-table-column>

    <el-table-column prop="position" label="职位"></el-table-column>

    <el-table-column prop="dimissionCause" label="离职原因"></el-table-column>

    <el-table-column prop="biggestGain" label="最大收获"></el-table-column>

    <el-table-column fixed="right" label="操作" width="200px">
      <template slot-scope="scope">
        <el-button
          type="primary"
          size="small"
          >修改</el-button
        >
        <el-button
          type="danger"
          size="small"
          @click="console.log(scope)"
          >删除</el-button
        >
      </template>
    </el-table-column>
  </el-table>
</template>

<script>
export default {
  props:{
    tableData:{
      type:Array,
      require:true,
      default:[]
    }
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