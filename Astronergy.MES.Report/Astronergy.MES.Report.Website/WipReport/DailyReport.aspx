<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="DailyReport.aspx.cs" Inherits="WipReport_DailyReport" Title="<%$ Resources:lang,PageTitle %>" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Scripts/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
     function ShowTimeControl(isShow) {
         if (isShow == true) {
             var valStart = $(".dateStart").val();
             var valEnd = $(".dateEnd").val();
             if (valStart.length > 0 && valEnd.length <= 10) {
                 $(".dateStart").val(valStart + " 08:00:00");
             }
             if (valEnd.length > 0 && valEnd.length<=10) {
                 $(".dateEnd").val(valEnd + " 08:00:00");
             }
         }
         else {
             var valStart = $(".dateStart").val();
             var valEnd = $(".dateEnd").val();
             if (valStart.length > 10) {
                 $(".dateStart").val(valStart.slice(0, 10));
             }
             if (valEnd.length > 10) {
                 $(".dateEnd").val(valEnd.slice(0, 10));
             }
         }
     }
     $(document).ready(function() {
         ShowTimeControl(!rbtByDay.GetChecked());
         Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

         $(".dateStart").focus(function() {
            var id = $(".dateEnd").attr("id");
            var hh=24;
            var datafmt = "yyyy-MM-dd HH:00:00";
            if (rbtByDay.GetChecked()) {
                datafmt = "yyyy-MM-dd";
                hh=24*31;
            }
            WdatePicker({ dateFmt: datafmt,isShowClear:false, readOnly: true, maxDate: '#F{$dp.$D(\'' + id + '\')}' });
         });
         $(".dateEnd").focus(function() {
             var id = $(".dateStart").attr("id");
             var datafmt = "yyyy-MM-dd HH:00:00";
             if (rbtByDay.GetChecked()) {
                 datafmt = "yyyy-MM-dd";
             }
             WdatePicker({ dateFmt: datafmt, isShowClear: false, readOnly: true, minDate: '#F{$dp.$D(\'' + id + '\')}' });
         })
     })
     function ValidateInputValue() {
         var valStart = $(".dateStart").val();
         var valEnd = $(".dateEnd").val();
         var byDay = rbtByDay.GetChecked();
         var sDate = new Date(Date.parse(valStart.replace(/-/g, "/")));
         var eDate = new Date(Date.parse(valEnd.replace(/-/g, "/")));
         if (byDay) {
             if (parseInt(Math.abs(eDate - sDate) / 1000 / 60 / 60 / 24) > 31) {
                 alert("结束时间和开始时间相差不能超过31天。");
                 return false;
             }
         }
         else {
             if (parseInt(Math.abs(eDate - sDate) / 1000 / 60 / 60) > 24) {
                 alert("结束时间和开始时间相差不能超过24小时。");
                 return false;
             }
         }
         return true;
     }
     
     function BeginRequestHandler(sender, args) {
         $("#<%=this.btnQuery.ClientID%>").attr("disabled", "disabled");
         $("#<%=this.btnQuery.ClientID%>").val("查询中...");
     }

     function EndRequestHandler(sender, args) {
         $("#<%=this.btnQuery.ClientID%>").removeAttr("disabled");
         $("#<%=this.btnQuery.ClientID%>").val("查 询");
         $("#divChartPassYield").width($(".searchbar").width());
         ShowTimeControl(!rbtByDay.GetChecked());
     }
     
     // <![CDATA[
     var textSeparator = ",";
     //当ASPxListBox中选中项目改变时函数。
     function OnListBoxSelectionChanged(lst, cmb, args) {
         if (args.index == 0)
             args.isSelected ? lst.SelectAll() : lst.UnselectAll();
         UpdateSelectAllItemState(lst);
         UpdateText(cmb, lst);
     }
     //更新第一个项为选中。
     function UpdateSelectAllItemState(lst) {
         IsAllSelected(lst) ? lst.SelectIndices([0]) : lst.UnselectIndices([0]);
     }
     //是否全部选中了ASPxListBox中的项目。
     function IsAllSelected(lst) {
         var selectedDataItemCount = lst.GetItemCount() - (lst.GetItem(0).selected ? 0 : 1);
         return lst.GetSelectedItems().length == selectedDataItemCount;
     }
     //更新AspxDropDownEdit控件的文本值为ASPxListBox的选择项。
     function UpdateText(cmb, lst) {
         var selectedItems = lst.GetSelectedItems();
         if (IsAllSelected(lst)) {
             cmb.SetText(selectedItems[0].text);
         }
         else {
             cmb.SetText(GetSelectedItemsText(selectedItems));
         }
     }
     //获取选择项目的文本值，使用分隔符分开。
     function GetSelectedItemsText(items) {
         var texts = [];
         for (var i = 0; i < items.length; i++)
             if (items[i].index != 0)
             texts.push(items[i].text);
         return texts.join(textSeparator);
     }
     // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXlsExport"/>
        </Triggers>
        <ContentTemplate>
            <div class="searchbar" style="height:auto;padding:0px;"><!--height:90px;-->
            <table class="searchbar_table" border="0" width="100%">
                <tr>
                    <td class="searchbar_table_td" style="width:10%">
                        车间
                    </td>
                    <td class="searchbar_table_td" style="width:15%"><!-- Width="100px"-->
                        <asp:UpdatePanel ID="UpdatePanelWorkPlace" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <dxe:ASPxComboBox ID="cmbWorkPlace" runat="server" TextField="LOCATION_NAME" AutoPostBack="true"
                            ValueField="LOCATION_KEY" ValueType="System.String" OnSelectedIndexChanged="cmbWorkPlace_SelectedIndexChanged">
                        </dxe:ASPxComboBox>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="searchbar_table_td" style="width:10%">
                       产品型号
                    </td>
                    <td class="searchbar_table_td"  style="width:15%">
                         <dxe:ASPxDropDownEdit ID="ddeProductModel" runat="server" ClientInstanceName="ddeProductModel" EnableAnimation="False">
                            <DropDownWindowStyle BackColor="#EDEDED"/>
                            <DropDownWindowTemplate>
                                <dxe:ASPxListBox Width="100%" ID="lstProductModel" 
                                                ClientInstanceName="lstProductModel" 
                                                SelectionMode="CheckColumn"
                                                runat="server">
                                    <Border BorderStyle="None" />
                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddeProductModel,e); }" />
                                </dxe:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dxe:ASPxDropDownEdit>
                    </td>
                    <td class="searchbar_table_td" style="white-space:nowrap;word-break:keep-all;width:10%">
                        班别
                    </td>
                    <td class="searchbar_table_td" style="width:15%">
                        <dxe:ASPxComboBox ID="cmbShift" runat="server" 
                                        TextField="SHIFT_NAME" 
                                        ValueField="SHIFT_KEY" 
                                        ValueType="System.String" 
                                         >
                        </dxe:ASPxComboBox>
                    </td>
                    <td class="searchbar_table_td" style="white-space:nowrap;word-break:keep-all;width:10%">生产排班</td>
                    <td class="searchbar_table_td" style="width:15%">
                        <dxe:ASPxComboBox ID="cmbFactoryShift" runat="server" SelectedIndex="0" 
                            ValueType="System.String">
                            <Items>
                                <dxe:ListEditItem Selected="True" Text="ALL" Value="ALL" />
                                <dxe:ListEditItem Text="A" Value="A" />
                                <dxe:ListEditItem Text="B" Value="B" />
                                <dxe:ListEditItem Text="C" Value="C" />
                                <dxe:ListEditItem Text="D" Value="D" />
                            </Items>
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    
                    <td class="searchbar_table_td">
                        工单号
                    </td>
                    <td class="searchbar_table_td">
                        <asp:UpdatePanel ID="UpdatePanelWorkOrderNumber" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <dxe:ASPxDropDownEdit ID="ddeWorkOrderNumber" runat="server" 
                                ClientInstanceName="ddeWorkOrderNumber" EnableAnimation="False" 
                                AutoPostBack="true">
                            <DropDownWindowStyle BackColor="#EDEDED"/>
                            <DropDownWindowTemplate>
                                <dxe:ASPxListBox Width="100%" ID="lstWorkOrderNumber" 
                                                ClientInstanceName="lstWorkOrderNumber" 
                                                SelectionMode="CheckColumn"
                                                runat="server">
                                    <Border BorderStyle="None" />
                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddeWorkOrderNumber,e); }" />
                                </dxe:ASPxListBox>
                            </DropDownWindowTemplate>
                             <ClientSideEvents />
                        </dxe:ASPxDropDownEdit>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="searchbar_table_td">
                       产品料号
                    </td>
                    <td class="searchbar_table_td">
                        <dxe:ASPxTextBox ID="txtPartNumber" runat="server" >
                        </dxe:ASPxTextBox>
                    </td>
                    <td class="searchbar_table_td">
                       产品ID号
                    </td>
                    <td class="searchbar_table_td">
                        <dxe:ASPxDropDownEdit ID="ddeProductId" runat="server" ClientInstanceName="ddeProductId" EnableAnimation="False">
                            <DropDownWindowStyle BackColor="#EDEDED"/>
                            <DropDownWindowTemplate>
                                <dxe:ASPxListBox Width="100%" ID="lstProductId" 
                                                ClientInstanceName="lstProductId" 
                                                SelectionMode="CheckColumn"
                                                runat="server">
                                    <Border BorderStyle="None" />
                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddeProductId,e); }" />
                                </dxe:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dxe:ASPxDropDownEdit>
                    </td>
                    <td class="searchbar_table_td" colspan="2"></td>
                </tr>
                <tr>
                    <td class="searchbar_table_td" colspan="2" valign="top" style="/*border:solid 1px;*/">
                        <span>
                        <dxe:ASPxRadioButton ID="rbtByDay" runat="server" Text="按天查询" Checked="True" 
                            GroupName="query" ClientInstanceName="rbtByDay">
                            <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(false) }" />
                        </dxe:ASPxRadioButton>
                        </span>
                        <span>
                        <dxe:ASPxRadioButton ID="rbtByTimeRange" runat="server" Text="按小时查询" GroupName="query">
                            <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(true) }" />
                        </dxe:ASPxRadioButton>
                        </span>
                    </td>
                    <td class="searchbar_table_td" style="white-space:nowrap;word-break:keep-all;">开始时间</td>
                    <td class="searchbar_table_td">
                        <input id="dateStart" type="text" runat="server"  class="Wdate dateStart" style="width:170px"/>
                    </td>
                    <td class="searchbar_table_td">结束时间</td>
                    <td class="searchbar_table_td" >
                        <input id="dateEnd" type="text" runat="server"  class="Wdate dateEnd" style="width:170px"/>
                    </td>
                    <td class="searchbar_table_td" colspan="2">
                         <asp:UpdatePanel ID="UpdatePanelQuery" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:Button ID="btnQuery" runat="server" Text="查 询" class="searchbar_table_btu"
                                                      OnClick="btnQuery_Click" Width="80px" 
                                                      OnClientClick="return ValidateInputValue();"/>
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                <ProgressTemplate>
                                    <img src="../Images/Loading2.gif"  alt="Loading..." class="progressImage" />                       
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                </tr>
            </table>
            </div>
            
            <asp:UpdatePanel ID="UpdatePanelResult" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="gridview">
            <div class="gridbar" id="divGridBar">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        
                        <td style="padding-right: 4px">
                            <dxe:ASPxButton ID="btnXlsExport" runat="server" Text="导出到EXCEL" UseSubmitBehavior="False"
                                OnClick="btnXlsExport_Click">
                                <Image Url="~/Images/xls.jpg" Width="16" Height="16">
                                </Image>
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
            <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server"
                OnCustomColumnDisplayText="grid_CustomColumnDisplayText" 
                Width="100%"
                Enabled="false"
                OnHtmlRowCreated="grid_HtmlRowCreated" 
                ondatabound="grid_DataBound">
                <SettingsBehavior AllowSort="False" />
                <Styles>
                    <Cell Wrap="False"></Cell>
                    <AlternatingRow BackColor="#F2F1FC">
                    </AlternatingRow>
                </Styles>
                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>
                <Settings ShowVerticalScrollBar="true" ShowHorizontalScrollBar="true" VerticalScrollableHeight="500" />
                <%-- BeginRegion Grid Columns --%>
                <%-- EndRegion --%>
            </dxwgv:ASPxGridView>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
    </div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
