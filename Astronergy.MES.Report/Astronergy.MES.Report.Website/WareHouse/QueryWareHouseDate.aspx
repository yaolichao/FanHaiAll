﻿<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="QueryWareHouseDate.aspx.cs" Inherits="WareHouse_QueryWareHouseDate" Title="Untitled Page" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
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
    <div class="searchbar" style="height:auto;padding:0px;width:100%">
    <table class="searchbar_table" border="0" width="100%">
        <tr>
            <td class="style6 searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="厂别：" Width="40px">
                </dx:ASPxLabel>
            </td>
            <td class="style7 searchbar_table_td">
                <dx:ASPxComboBox ID="cboFactory" runat="server" Width="100%">
                </dx:ASPxComboBox>
            </td>
            <td class="style8 searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="工单号：" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td class="style10 searchbar_table_td">
                <dx:ASPxDropDownEdit ID="ddeWO" runat="server" ClientInstanceName="ddeWO" EnableAnimation="False" ReadOnly="False" Width=
                "100%">
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
            </td>
            <td class=" searchbar_table_td">
            产品料号
            </td>
            <td class=" searchbar_table_td">
                 <dxe:ASPxTextBox ID="txtPartNumber" runat="server" Width="100%">
                </dxe:ASPxTextBox>
            </td>
            <td class=" searchbar_table_td"></td>
        </tr>
        <tr>
            <td class="style9 searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="档位：" Width="40px">
                </dx:ASPxLabel>
            </td>
            <td class="style11 searchbar_table_td">
                <dx:ASPxDropDownEdit ID="ddePowerLevel" runat="server"  
                    ClientInstanceName="ddePowerLevel" EnableAnimation="False" Width="100%">
                   <DropDownWindowStyle BackColor="#EDEDED"/>
                    <DropDownWindowTemplate>
                        <dx:ASPxListBox Width="100%" ID="lstPowerLevel" 
                                        ClientInstanceName="lstPowerLevel" 
                                        SelectionMode="CheckColumn"
                                        runat="server">
                            <Border BorderStyle="None" />
                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                            <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddePowerLevel,e); }" />
                        </dx:ASPxListBox>
                    </DropDownWindowTemplate>
                </dx:ASPxDropDownEdit>
            </td>
            <td class="style2 searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="入库日期：" Width="60px">
                </dx:ASPxLabel>
            </td>
            <td class="style4 searchbar_table_td">
                <dx:ASPxDateEdit ID="deStartDate" runat="server" Width="100%">
                </dx:ASPxDateEdit>
            </td>
            <td class="style3 searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="~" Width="10px">
                </dx:ASPxLabel>
            </td>
            <td class="style5 searchbar_table_td">
                <dx:ASPxDateEdit ID="deEndDate" runat="server" Width="100%">
                </dx:ASPxDateEdit>
            </td>
            <td class=" searchbar_table_td">
                <dx:ASPxButton ID="btnQuey" runat="server" class="searchbar_table_btu" onclick="btnQuey_Click" Text="查询">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
  </div>
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
    <dx:ASPxGridView ID="gvWareHouse" runat="server" 
            ondatabound="gvWareHouse_DataBound" 
            onhtmlrowcreated="gvWareHouse_HtmlRowCreated" 
          oncustomcolumndisplaytext="gvWareHouse_CustomColumnDisplayText" 
          KeyFieldName="WHNO">
            <SettingsBehavior AllowSort="False" />
            <SettingsPager PageSize="20">
            </SettingsPager>
            <Settings ShowColumnHeaders="False" />
        </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="gvWareHouse">
    </dx:ASPxGridViewExporter>
      <asp:HiddenField ID="hidsFactory" runat="server" />
      <asp:HiddenField ID="hidsQstartdate" runat="server" />
      <asp:HiddenField ID="hidsQenddate" runat="server" />
      <asp:HiddenField ID="hidsWO" runat="server" />
      <asp:HiddenField ID="hidsPowerLevel" runat="server" />
  </div>
</asp:Content>
