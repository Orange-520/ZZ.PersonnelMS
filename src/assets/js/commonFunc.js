// 公共方法

// arr:对象数组 fieldName:字段名
// 将 是|否 改为 true|false
// 返回值：加工后的数组
function transitionIfuse(arr, fieldName) {
  let list = arr.map((item) => {
    if (item[fieldName] == '是') {
      item[fieldName] = true;
    } else {
      item[fieldName] = false
    }
    return item
  })
  return list
};

// money:要检测的值;
// 判断是否为钱类型;
// 返回值：布尔值，true为符合，false为不符合
// let regExp = /^[0-9]+(.[0-9]{1,2})?$/g; 也可检验是否为钱类型
function ifMoney(money){
  let regExp = /(^[1-9](\d+)?(\.\d{1,2})?$)|(^0$)|(^\d\.\d{1,2}$)/g;
  let result = regExp.test(money);
  console.log(result);
  return result;
}

// 判断时间是否大于当前天
// date:要检测的时间
// 返回值：布尔值，true为符合，false为不符合
function greaterThanNowDay(value){
  let nowDate = `${new Date().getFullYear()}-${new Date().getMonth()+1}-${new Date().getDate()}`
  let start = new Date(Date.parse(nowDate.replace(/-/g, "/")));
  let end = new Date(Date.parse(value.replace(/-/g, "/")));
  return start.getTime()>=end.getTime()
}

// 判断是否为图片格式和文件大小
// 返回值：对象，其中包含状态
function isJPGAndIsLt2M(file){
  let isJPG = false;
  const Image_format=[
    "image/gif",
    "image/png",
    "image/jpeg"
  ];
  for (const item of Image_format) {
    if(file.type == item){
      isJPG = true;
      break
    }
  }
  if (!isJPG) {
    return {code:500,msg:'上传头像图片只能是 图片 格式!',data:false};
  }

  const isLt2M = file.size / 1024 / 1024 < 2; 
  if (!isLt2M) {
    return {code:500,msg:'上传头像图片大小不能超过 2MB!',data:false};
  }
  
  return {code:200,msg:'通过审验',data:true};
}

// 判断是否为6到12位字母，数字,下划线，不包含空格组成的密码
// 参数pwd：要检测的密码
// 返回值：布尔值，true为符合，false为不符合
function isPwdFormat(pwd){
  let str = /^\w+$/g;
  let result = str.test(pwd);
  return pwd.length > 5 && pwd.length < 13 && result;
}

// 判断是否为正整数
// 参数 number：要检测的数值
// 返回值：布尔值，true为符合，false为不符合
function isPositiveIntegerNumber(number){
  let regExp = /^[1-9]\d*$/g;
  let result = regExp.test(number);
  return result;
}

// 判断是否为11位的数字格式
// 参数 phone：要检测的数值
// 返回值：布尔值，true为符合，false为不符合
function isPhone(phone){
  let str = /^\d{11}$/;
  let result = str.test(phone);
  return result;
}
export {
  transitionIfuse,
  ifMoney,
  greaterThanNowDay,
  isJPGAndIsLt2M,
  isPwdFormat,
  isPositiveIntegerNumber,
  isPhone
}