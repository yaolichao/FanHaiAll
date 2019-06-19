<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Log.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <center> <h1>电池MES系统升级日志</h1></center>
    </div>
    <div>
        <h2>
          2012.7.31正式版发布 V1.0.0.3073</h2>
        <h3>
            功能改进</h3>
        <ol>
            <li>修改QC检验数据录入同时开两个画面会出现两笔资料的bug</li>
            <li>取消数据采集的时候保存以后提示保存成功对话框</li>
            <li>抽检点设置分页显示历史修改记录</li>
            <li>单独打报废和不良记录备注，显示在报表上</li>
            <li>过账如果打了不良报废数量为零直接结束批次</li>
        </ol>
    </div>
    </form>
</body>
</html>
