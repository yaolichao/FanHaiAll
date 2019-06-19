<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ELDefectYieldDataDetail.aspx.cs" Inherits="Quality_ELDefectYieldDataDetail" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="../Compent/Header.ascx" TagName="Header" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
    EL不良分布明细-<%= this.PagetTitle%>
    </title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <uc2:Header ID="Header1" runat="server" />
    </div>
    <div>
    <table border="0" width="100%">
        <tr>
            <td align="left">
                <dxe:ASPxLabel ID="lblTypeName" runat="server" Text="名称：">
                </dxe:ASPxLabel>
                <dxe:ASPxLabel ID="lblType" runat="server" Text="">
                </dxe:ASPxLabel>
            </td>
            <td align="right">
                <dxe:ASPxLabel ID="lblCalcMethodName" runat="server" Text="公式：">
                </dxe:ASPxLabel>
                <dxe:ASPxLabel ID="lblCalcMethod" runat="server" Text="">
                </dxe:ASPxLabel>
            </td>
        </tr>
    </table>
    </div>
    <div>
        <dxe:ASPxButton ID="btnXlsExport" runat="server" Text="导出到EXCEL" UseSubmitBehavior="False"
            OnClick="btnXlsExport_Click">
            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
            </Image>
        </dxe:ASPxButton>
    </div>
    <dx:ASPxGridView ID="gvResults" runat="server" Width="100%"
            KeyFieldName="KEY_VALUE" ondatabound="grid_DataBound" 
            OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
            SettingsBehavior-AllowDragDrop="false" 
            SettingsBehavior-AllowSort="false">
        <SettingsPager PageSize="100">
        </SettingsPager>
        <Templates>
            <DetailRow>
               <div>
                    <dxe:ASPxButton ID="btnDetailXlsExport" runat="server" Text="导出到EXCEL" UseSubmitBehavior="False"
                        OnClick="btnDetailXlsExport_Click">
                        <Image Url="~/Images/xls.jpg" Width="16" Height="16">
                        </Image>
                    </dxe:ASPxButton>
                </div>
                <dx:ASPxGridView ID="detailGrid" runat="server" Width="100%"
                        OnBeforePerformDataSelect="detailGrid_DataSelect"
                        OnCustomColumnDisplayText="detailGrid_CustomColumnDisplayText">
                    <SettingsPager PageSize="50">
                    </SettingsPager>
                    <Settings ShowFooter="True" />
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
        <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="false"/>
        <Settings ShowFooter="True" />
    </dx:ASPxGridView>
     <dxwgv:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="gvResults" />
    </form>
</body>
</html>
