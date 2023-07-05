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

    <el-table-column prop="schoolName" label="学校名称"></el-table-column>

    <el-table-column
      prop="currentEducation"
      label="学历"
      :formatter="currentEducationFormatter"
    ></el-table-column>

    <el-table-column
      width="120"
      prop="startTime"
      label="入学时间"
      :formatter="TimeColumnFormatter"
    ></el-table-column>

    <el-table-column
      width="120"
      prop="endTime"
      :formatter="TimeColumnFormatter"
      label="毕业时间"
    ></el-table-column>

    <el-table-column
      prop="learningStyle"
      label="学习方式"
      :formatter="learningStyleFormatter"
    ></el-table-column>

    <el-table-column prop="score" label="学习成绩"></el-table-column>

    <el-table-column prop="major" label="专业"></el-table-column>

    <el-table-column
      prop="degree"
      label="学位"
      :formatter="degreeFormatter"
    ></el-table-column>

    <el-table-column
      prop="degreeCreateTime"
      label="学位授予时间"
      :formatter="TimeColumnFormatter"
      width="120"
    ></el-table-column>

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
import config from "@/assets/js/config.js";
export default {
  props: {
    tableData: {
      type: Array,
      require: true,
      default: [],
    },
  },
  methods: {
    // 格式化当前学历表格列
    currentEducationFormatter(row, column, cellValue, index) {
      for (const item of config.currentEducationConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    // 格式化学习方式表格列
    learningStyleFormatter(row, column, cellValue, index) {
      for (const item of config.learningStyleConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    // 格式化学位表格列
    degreeFormatter(row, column, cellValue, index) {
      for (const item of config.degreeConfig) {
        if (item.value === cellValue) {
          return item.label;
        }
      }
    },
    // 处理时间列
    TimeColumnFormatter(row, column, cellValue, index) {
      if (cellValue == null || cellValue =='') {
        return null;
      }
      let t = new Date(cellValue);
      return `${t.getFullYear()}-${
        t.getMonth() + 1
      }-${t.getDate()} ${t.getHours()}:${t.getMinutes()}`;
    },
  },
};
</script>

<style>
</style>