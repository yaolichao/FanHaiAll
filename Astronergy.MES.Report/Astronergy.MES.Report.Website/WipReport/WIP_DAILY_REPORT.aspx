﻿<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="WIP_DAILY_REPORT.aspx.cs" Inherits="WipReport_FACTORY_DAILY_REPORT" Title="<%$ Resources:lang,PageTitle %>" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <style type="text/css">
        .v1
        {
        	color:#ff;       
        	}
    </style>
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
  
    <div class="searchbar2">
    <div style=" min-width:950px; overflow:hidden; display:block;">
        <div class="mutilsearchbar">
            <span>
                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="车间名称">
                </dxe:ASPxLabel>
            </span>
            <span>
                <dxe:ASPxComboBox ID="cmbWorkPlace" runat="server"
                    TextField="LOCATION_NAME" ValueField="LOCATION_KEY" ValueType="System.String" 
                    Width="100px" onselectedindexchanged="cmbWorkPlace_SelectedIndexChanged">
                </dxe:ASPxComboBox>
            </span>        
            <span>
                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="产品类型">
                </dxe:ASPxLabel>
            </span>
            <span>
                 <dxe:ASPxDropDownEdit ID="checkComboBoxForType" ClientInstanceName="checkComboBoxForType" runat="server" Width="200px">
                    <DropDownWindowTemplate>
                    <dxe:ASPxListBox Width="100%" ID="lbx_type" 
                                    ClientInstanceName="checkListBox" 
                                    SelectionMode="CheckColumn" 
                                    runat="server" 
                                    SkinID="CheckComboBoxListBox">
                            <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,checkComboBoxForType,e); }"/>
                    </dxe:ASPxListBox>            
                    </DropDownWindowTemplate>      
                </dxe:ASPxDropDownEdit>
            </span>
             <span>
                <dxe:ASPxLabel ID="lblRoute" runat="server" Text="产品ID号">
                </dxe:ASPxLabel>
            </span>
            <span>       
                <dxe:ASPxDropDownEdit ID="checkComboBox" ClientInstanceName="checkComboBox" runat="server" Width="100px">
                    <DropDownWindowTemplate>
                    <dxe:ASPxListBox Width="100%" ID="lbx_proid" 
                                    ClientInstanceName="checkListBox" 
                                    SelectionMode="CheckColumn" 
                                    runat="server"  
                                    SkinID="CheckComboBoxListBox">
                            <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,checkComboBox,e); }"/>
                    </dxe:ASPxListBox>            
                    </DropDownWindowTemplate>      
                </dxe:ASPxDropDownEdit>
            </span>
            <span>
                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="工单号">
                </dxe:ASPxLabel>
            </span>
            <span>
                <dxe:ASPxDropDownEdit ID="checkComboBoxForWo" ClientInstanceName="checkComboBoxForWo" runat="server" Width="100px">
                    <DropDownWindowTemplate>
                    <dxe:ASPxListBox Width="100%" ID="lbx_wo" 
                                    ClientInstanceName="checkListBox" 
                                    SelectionMode="CheckColumn" 
                                    runat="server"  SkinID="CheckComboBoxListBox">     
                        <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,checkComboBoxForWo,e); }" />
                    </dxe:ASPxListBox>
                    </DropDownWindowTemplate>
                </dxe:ASPxDropDownEdit>        
            </span>
            <span>
                <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="班别">
                </dxe:ASPxLabel>
            </span>
            <span>
                <dxe:ASPxComboBox ID="cmbShift" runat="server"
                  TextField="SHIFT_NAME" ValueField="SHIFT_KEY" ValueType="System.String" 
                    Width="100px" >
                </dxe:ASPxComboBox>
            </span>
        </div>
        <div class="mutilsearchbar">
            <span style=" display:block; margin-top:-10px; overflow:hidden;">
               <dxe:ASPxRadioButtonList ID="rbtnLst" runat="server" 
                     RepeatDirection="Horizontal" 
                     onselectedindexchanged="rbtnLst_SelectedIndexChanged" SelectedIndex="0" 
                     AutoPostBack="True">
                     <Items>
                         <dxe:ListEditItem Text="日期查询" Value="day" />
                         <dxe:ListEditItem Text="时间查询" Value="hour" />
                     </Items>
                 </dxe:ASPxRadioButtonList>
            </span>
            <span>
                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="开始时间" >
                </dxe:ASPxLabel>
            </span>
            <span>
                <dxe:ASPxDateEdit ID="startDate" runat="server" Width="100">
                </dxe:ASPxDateEdit>  
            </span>
            <span>
                <dxe:ASPxTimeEdit ID="startTime" runat="server" Width="100">
                </dxe:ASPxTimeEdit>
            </span>
            <span> 
                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="结束时间" >
                </dxe:ASPxLabel>
           </span>
           <span>
                <dxe:ASPxDateEdit ID="endDate" runat="server" Width="100">
                </dxe:ASPxDateEdit>
           </span>
            <span>
                <dxe:ASPxTimeEdit ID="endTime" runat="server" Width="100">
                </dxe:ASPxTimeEdit>        
            </span>
            <span>
            <asp:Button ID="btnQuery" runat="server" Text="查 询" OnClick="btnQuery_Click" />
            </span>
        </div>
    </div>   
    </div>
    
    <div class="gridview">
        <div class="gridbar">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-right: 4px;">
                        <dxe:ASPxButton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False"
                            OnClick="btnXlsExport_Click">
                            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
                            </Image>
                        </dxe:ASPxButton>
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
        <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" 
            runat="server" 
            Width="100%"  
            KeyFieldName="PRO_NAME" 
            onhtmlrowcreated="grid_HtmlRowCreated" 
            oncustomcolumndisplaytext="grid_CustomColumnDisplayText1"  >
            <SettingsBehavior AllowSort="False" />
            <Styles>
                <Cell Wrap="False"></Cell>
                <AlternatingRow BackColor="#F2F1FC">
                </AlternatingRow>
            </Styles>
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
        </dxwgv:ASPxGridView>      
        <dxwgv:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="grid">
        </dxwgv:ASPxGridViewExporter>
        
        <asp:HiddenField ID="hidLoactionKey" runat="server" />
        <asp:HiddenField ID="hidPartType" runat="server" />
        <asp:HiddenField ID="hidPartTypeForCustCheck" runat="server" />
        <asp:HiddenField ID="hidStartDate" runat="server" />
        <asp:HiddenField ID="hidStartTime" runat="server" />
        <asp:HiddenField ID="hidProid" runat="server" />
        <asp:HiddenField ID="hidProidForCustCheck" runat="server" />
        <asp:HiddenField ID="hidWorkorder" runat="server" />
        <asp:HiddenField ID="hidWorkOrderFOrCustCheck" runat="server" />
        <asp:HiddenField ID="hidEndTime" runat="server" />
        <asp:HiddenField ID="hidEndDate" runat="server" />
        <asp:HiddenField ID="hidLayoutType" runat="server" />
        <asp:HiddenField ID="hidShiftKey" runat="server" />
    </div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
