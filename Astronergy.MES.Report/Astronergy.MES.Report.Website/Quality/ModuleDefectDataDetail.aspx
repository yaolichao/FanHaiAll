<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModuleDefectDataDetail.aspx.cs" Inherits="Quality_ModuleDefectDataDetail" %>

<%@ Register src="../Compent/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <div id="header">         
        <uc1:Header ID="Header1" runat="server" />          
    </div>
   <div>
          <div class="gridview" >            
                <dx:ASPxButton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False"
           Width="120px" onclick="btnXlsExport_Click">
            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
            </Image>
        </dx:ASPxButton>
        <dx:ASPxGridView ID="gvPatchDisplay" runat="server" EnableCallBacks="False" 
                    onbeforecolumnsortinggrouping="gvPatchDisplay_BeforeColumnSortingGrouping">
            <SettingsPager AlwaysShowPager="True" Mode="ShowAllRecords">           
            </SettingsPager>
            
        </dx:ASPxGridView>
        </div>
    
    </div>
    <dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="gvPatchDisplay">
    </dx:ASPxGridViewExporter>
    </form>
</body>
</html>
