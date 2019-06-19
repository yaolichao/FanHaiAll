<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustCheckAndWarehouse.aspx.cs" Inherits="WipReport_CustCheckAndWarehouse" %>

<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register src="../Compent/Header.ascx" tagname="Header" tagprefix="uc1" %>



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
        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False"
            OnClick="btnXlsExport_Click" Width="120px">
            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
            </Image>
        </dx:ASPxButton>
    </div>
    <div>
       <div class="grid" >
       <dx:ASPxGridView ID="grid" runat="server"   Width="100%" 
               onbeforecolumnsortinggrouping="grid_BeforeColumnSortingGrouping" >
               <Styles>
               <Cell Wrap="False">
                </Cell>
                <AlternatingRow BackColor="#F2F1FC">
                </AlternatingRow>
               </Styles>
           <SettingsPager AlwaysShowPager="True" Mode="ShowAllRecords">
           </SettingsPager>
           </dx:ASPxGridView>
           
       </div>
    </div>
      <dx:ASPxGridViewExporter ID="exporter" runat="server"  GridViewID="grid">
      </dx:ASPxGridViewExporter>
    </form>
</body>
</html>
