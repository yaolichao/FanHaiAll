﻿<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="ModelCtmRpt.aspx.cs" Inherits="WipReport_ModelCtmRpt"  Title="<%$ Resources:lang,PageTitle %>" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        })
        function BeginRequestHandler(sender, args) {
            $("#<%=this.btnQuery.ClientID%>").attr("disabled", "disabled");
            $("#<%=this.btnQuery.ClientID%>").val("查询中...");
        }

        function EndRequestHandler(sender, args) {
            $("#<%=this.btnQuery.ClientID%>").removeAttr("disabled");
            $("#<%=this.btnQuery.ClientID%>").val("查 询");
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
           cmb.SetText(GetSelectedItemsText(selectedItems));
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExcel"/>
    </Triggers>
   <ContentTemplate>
    <div class="searchbar" style="height: auto;padding:0px;width:100%">
        <table class="searchbar_table" border="0" width="100%">
            <tr>
                <td class="searchbar_table_td" style="width:10%">
                    车间
                </td>
                <td class="searchbar_table_td" style="width:20%">
                    <dx:ASPxComboBox ID="cobFactory" runat="server" Width="100%">
                    </dx:ASPxComboBox>
                </td>
                <td class="searchbar_table_td">
                    机台号
                </td>
                <td class="searchbar_table_td" style="width:20%">
                    <dx:ASPxTextBox ID="txtDeviceNo" runat="server" Width="100%">
                    </dx:ASPxTextBox>
                </td>
                <td class="searchbar_table_td" style="width:10%">
                    产品ID号
                </td>
                <td class="searchbar_table_td" colspan="2" style="width:20%">
                    <dx:ASPxComboBox ID="cobProID" runat="server" ValueType="System.String" 
                        Width="100%" DropDownStyle="DropDown">
                    </dx:ASPxComboBox>
                </td>
                <td class="searchbar_table_td">
                    <dx:ASPxCheckBox ID="chDefault" runat="server" Text="有效数据">
                    </dx:ASPxCheckBox>
                </td>
            </tr>
            <tr>
                <td class="searchbar_table_td">
                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="工单号：" Width="50px">
                    </dx:ASPxLabel>
                </td>
                <td class="searchbar_table_td">
                     <asp:UpdatePanel ID="UpdatePanelWorkOrderNumber" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                    <dx:ASPxDropDownEdit ID="ddeWO" runat="server" ClientInstanceName="ddeWO" EnableAnimation="False" Width="100%">
                            <DropDownWindowStyle BackColor="#EDEDED"/>
                            <DropDownWindowTemplate>
                                <dx:ASPxListBox Width="100%" ID="lstWO" 
                                                ClientInstanceName="lstWO" 
                                                SelectionMode="CheckColumn"
                                                runat="server">
                                    <Border BorderStyle="None" />
                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddeWO,e); }" />
                                </dx:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dx:ASPxDropDownEdit>
                      </ContentTemplate>
                     </asp:UpdatePanel>
                </td>
                <td class="searchbar_table_td">
                       产品料号
                </td>
                <td class="searchbar_table_td">
                     <dxe:ASPxTextBox ID="txtPartNumber" runat="server" Width="100%">
                    </dxe:ASPxTextBox>
                </td>
                <td class="searchbar_table_td" colspan="4"></td>
            </tr>
            <tr>
                <td class="searchbar_table_td">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Width="60px" Text="组件序号：">
                    </dx:ASPxLabel>
                </td>
                <td class="searchbar_table_td">
                    <dx:ASPxTextBox ID="txtStartSN" runat="server" Width="100%">
                    </dx:ASPxTextBox>
                </td>
                <td class="searchbar_table_td">~</td>
                <td class="searchbar_table_td">
                    <dx:ASPxTextBox ID="txtEndSN" runat="server" Width="100%">
                    </dx:ASPxTextBox>
                </td>
                <td class="searchbar_table_td">
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Width="60" Text="测试日期：">
                    </dx:ASPxLabel>
                </td>
                <td class="searchbar_table_td">
                    <dx:ASPxDateEdit ID="dStartDate" runat="server" Width="100px">
                    </dx:ASPxDateEdit>
                </td>
                <td class="searchbar_table_td">
                    <dx:ASPxDateEdit ID="dEndDate" runat="server" Width="100px">
                    </dx:ASPxDateEdit>
                </td>
                <td class="searchbar_table_td">
                  <asp:UpdatePanel ID="UpdatePanelQuery" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                      <asp:Button ID="btnQuery" runat="server" Text="查 询" class="searchbar_table_btu" OnClick="btnQuery_Click" Width="80px"/>
                      </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanelResult" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div class="gridview">
        <div class="gridbar">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-right: 4px;">
                        <dx:ASPxButton ID="btnExcel" runat="server" Text="Export to XLS" UseSubmitBehavior="False"
                            OnClick="btnExcel_Click">
                            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                    </td>
                    <td style="padding-right: 4px">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        
        <dx:ASPxGridView ID="gvCTMRport" runat="server" 
            AutoGenerateColumns="False" Width="100%" 
            ondatabound="gvCTMRport_DataBound" EnableTheming="True">
            <SettingsPager PageSize="100">
            </SettingsPager>
            <SettingsBehavior AllowDragDrop="false" ColumnResizeMode="Control" />
            <Settings ShowVerticalScrollBar="true" VerticalScrollableHeight="500" ShowHorizontalScrollBar="true" UseFixedTableLayout="false"/>
        </dx:ASPxGridView>
        
        <dx:ASPxGridViewExporter ID="exporter" runat="server" 
    GridViewID="gvCTMRport" onrenderbrick="exporter_RenderBrick">
    </dx:ASPxGridViewExporter>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
   </ContentTemplate>
</asp:UpdatePanel>
 </asp:Content>
