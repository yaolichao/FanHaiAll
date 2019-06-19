<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryWareHouseData_Detail.aspx.cs" Inherits="WareHouse_QueryWareHouseData_Detail" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="../Compent/Header.ascx" TagName="Header" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>入库组件档位分布</title>
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <uc2:Header ID="Header1" runat="server" />
    </div>
    <div>
        <%--<dxe:ASPxButton ID="btnXlsExport" runat="server" Text="导出到EXCEL" UseSubmitBehavior="False"
            >
            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
            </Image>
        </dxe:ASPxButton>--%>
    </div>
    <dx:ASPxGridView ID="gvDetail" runat="server" Width="100%">
        <SettingsPager PageSize="30">
        </SettingsPager>
        <Settings ShowFooter="True" />
    </dx:ASPxGridView>
    </form>
</body>
</html>
