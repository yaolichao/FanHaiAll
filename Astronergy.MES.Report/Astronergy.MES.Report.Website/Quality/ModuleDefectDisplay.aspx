﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="ModuleDefectDisplay.aspx.cs" Inherits="Quality_ModuleDefectDisplay" %>

<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
     // <![CDATA[
     
     //------------------------------------------------------------------------------------
        var textSeparator2 = ";";
        var ddl = null;
        var combox = null;
        function OnListBoxSelectionChanged(dropDown, sender, args) {
            ddl = dropDown;
            combox = sender;
         if (args.index == 0)
             args.isSelected ? ddl.SelectAll() : ddl.UnselectAll();
         //SynchronizeListBoxValues(ddl, args);
         UpdateSelectAllItemState();
         UpdateText();
     }
     //更新选择项状态
     function UpdateSelectAllItemState() {
         IsAllSelected() ? ddl.SelectIndices([0]) : ddl.UnselectIndices([0]);
     }
     //全选
     function IsAllSelected() {
         for (var i = 0; i < ddl.GetItemCount(); i++)
             if (!ddl.GetItem(i).selected)
             return false;
         return true;
     }
     //不选
     function UnselectAll() {
         for (var i = 0; i < ddl.GetItemCount(); i++)
             if (ddl.GetItem(i).selected)
             return false;
         return true;
     }
     function UpdateText() {
         var selectedItems = ddl.GetSelectedItems();
         combox.SetText(GetSelectedItemsText(selectedItems));
     }
     function SynchronizeListBoxValues(ddl, args) {
         UnselectAll();
         var texts = ddl.GetText().split(textSeparator2);
         var values = GetValuesByTexts( texts);
         ddl.SelectValues(values);
         UpdateSelectAllItemState();
         UpdateText();  // for remove non-existing texts
     }
     function GetSelectedItemsText(items) {
         var texts = [];
         for (var i = 0; i < items.length; i++)
             if (items[i].index != 0)
             texts.push(items[i].text);
         return texts.join(textSeparator2);
     }
     function GetValuesByTexts( texts) {
         var actualValues = [];
         var value = "";
         for (var i = 0; i < texts.length; i++) {
             value = GetValueByText( texts[i]);
             if (value != null)
                 actualValues.push(value);
         }
         return actualValues;
     }
     function GetValueByText(text) {
         for (var i = 0; i < s.GetItemCount(); i++)
             if (s.GetItem(i).text.toUpperCase() == text.toUpperCase())
                 return s.GetItem(i).value;
         return null;
     }
     // ]]>
    </script>
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    <div class="searchbar" style="height:auto;padding:0px;width:100%">
            <table class="searchbar_table" border="0" width="100%"><!-- style="margin:5px;"-->
                <tr>
                    <td class="searchbar_table_td" style="width:10%">
                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="车间名称">
                        </dx:ASPxLabel>
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                        <dx:ASPxComboBox ID="cmbWorkPlace" runat="server"
                        TextField="LOCATION_NAME" ValueField="LOCATION_KEY" ValueType="System.String"
                         >
                        </dx:ASPxComboBox>
                    </td>
                    <td class="searchbar_table_td" style="width:10%">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="产品型号">
                        </dx:ASPxLabel>
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                        <dx:ASPxDropDownEdit ID="checkComboBoxForType" ClientInstanceName="checkComboBoxForType" runat="server" >
                            <DropDownWindowTemplate>
                            <dx:ASPxListBox Width="100%" ID="lbx_type" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" runat="server"  SkinID="CheckComboBoxListBox">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,checkComboBoxForType,e); }"/>
                            </dx:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dx:ASPxDropDownEdit>
                    </td>
                    <td class="searchbar_table_td" style="width:10%">
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="产品ID号">
                        </dx:ASPxLabel>
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                           <dx:ASPxDropDownEdit ID="checkComboBox" ClientInstanceName="checkComboBox" runat="server" >
                                <DropDownWindowTemplate>
                                <dx:ASPxListBox Width="100%" ID="lbx_proid" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" runat="server"  SkinID="CheckComboBoxListBox">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,checkComboBox,e); }"/>
                                </dx:ASPxListBox>
                                </DropDownWindowTemplate>
                            </dx:ASPxDropDownEdit>
                    </td>
                    <td class="searchbar_table_td"></td>
                </tr>
                <tr>
                    <td class="searchbar_table_td" style="width:10%">
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="工单号">
                        </dx:ASPxLabel>
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                         <dx:ASPxDropDownEdit ID="checkComboBoxForWo" ClientInstanceName="checkComboBoxForWo" runat="server" >
                            <DropDownWindowTemplate>
                            <dx:ASPxListBox Width="100%" ID="lbx_wo" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" runat="server"  SkinID="CheckComboBoxListBox">
                                <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,checkComboBoxForWo,e); }" />
                            </dx:ASPxListBox>

                        </DropDownWindowTemplate>
                        </dx:ASPxDropDownEdit>
                    </td>
                    <td class="searchbar_table_td" style="width:10%">
                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="产品料号">
                        </dx:ASPxLabel>
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                        <dx:ASPxTextBox ID="textPartNumber" runat="server" >
                        </dx:ASPxTextBox>
                    </td>
                    <td class="searchbar_table_td" colspan="3"></td>
                </tr>
                <tr>
                    <td class="searchbar_table_td" colspan="2" valign="top">
                         <dx:ASPxRadioButtonList ID="rbtnLst" runat="server"
                             RepeatDirection="Horizontal"
                             onselectedindexchanged="rbtnLst_SelectedIndexChanged" SelectedIndex="0"
                             AutoPostBack="True">
                             <Items>
                                 <dx:ListEditItem Text="日期查询" Value="day" />
                                 <dx:ListEditItem Text="时间查询" Value="hour" />
                             </Items>
                         </dx:ASPxRadioButtonList>
                    </td>
                    <td class="searchbar_table_td">
                       <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="开始时间">
                            </dx:ASPxLabel>
                    </td>
                    <td class="searchbar_table_td">
                        <span>
                            <dx:ASPxDateEdit ID="startDate" runat="server" Width="100">
                            </dx:ASPxDateEdit>
                        </span>
                        <span>
                            <dx:ASPxTimeEdit ID="startTime" runat="server" Width="100">
                            </dx:ASPxTimeEdit>
                        </span>
                    </td>
                    <td class="searchbar_table_td">
                       <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="结束时间">
                            </dx:ASPxLabel>
                    </td>
                    <td class="searchbar_table_td">
                        <span>
                        <dx:ASPxDateEdit ID="endDate" runat="server" Width="100">
                            </dx:ASPxDateEdit>
                        </span>
                        <span>
                            <dx:ASPxTimeEdit ID="endTime" runat="server" Width="100">
                        </dx:ASPxTimeEdit>
                        </span>
                    </td>
                    <td class="searchbar_table_td">
                        <asp:Button ID="btnQuery" runat="server" Text="查 询" OnClick="btnQuery_Click" />
                    </td>
                </tr>
            </table>
            </div>


      <div class="gridview">
        <div class="gridbar">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-right: 4px;">
                        <dx:aspxbutton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False"
                            OnClick="btnXlsExport_Click">
                            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
                            </Image>
                        </dx:aspxbutton>
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
        <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" Width="100%" KeyFieldName="CODE"
            oncustomcolumndisplaytext="grid_CustomColumnDisplayText"  >
            <SettingsBehavior AllowSort="False" />
            <Styles>
                <Cell Wrap="False">
                </Cell>
                <AlternatingRow BackColor="#F2F1FC">
                </AlternatingRow>
            </Styles>
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
        </dx:ASPxGridView>  
                    
            
        <asp:HiddenField ID="hidLoactionKey" runat="server" />
        <asp:HiddenField ID="hidPartType" runat="server" />
        <asp:HiddenField ID="hidStartDate" runat="server" />
        <asp:HiddenField ID="hidStartTime" runat="server" />
        <asp:HiddenField ID="hidProid" runat="server" />
        <asp:HiddenField ID="hidWorkorder" runat="server" />
        <asp:HiddenField ID="hidEndTime" runat="server" />
        <asp:HiddenField ID="hidEndDate" runat="server" />
        <asp:HiddenField ID="hidLayoutType" runat="server" />
        <asp:HiddenField ID="hidPartNumber" runat="server" />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

