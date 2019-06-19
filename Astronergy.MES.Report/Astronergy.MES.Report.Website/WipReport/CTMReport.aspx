<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="CTMReport.aspx.cs" Inherits="WipReport_CTMReport" Title="Untitled Page" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<%@ Register assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    
    <script language="javascript" type="text/javascript">
       // <![CDATA[
       var textSeparator = "#";
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
   <div class="searchbar" style="height:240px;padding:5px;width:100%">
    <table class="style1" style="margin:3px;">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="厂别" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="cboFactory" runat="server" Width="60px">
                </dx:ASPxComboBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="测试日期" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxDateEdit ID="deStartDate" runat="server" AutoPostBack="True" 
                    ondatechanged="deStartDate_DateChanged">
                </dx:ASPxDateEdit>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="~" Width="10px">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxDateEdit ID="deEndDate" runat="server" AutoPostBack="True" 
                    ondatechanged="deEndDate_DateChanged">
                </dx:ASPxDateEdit>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="组件批号" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="txtStartSN" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="~" Width="10px">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="txtEndSN" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="分档方式" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="cboPowerType" runat="server" Width="60px">
                </dx:ASPxComboBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="产品型号" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td colspan="7">
                <dx:ASPxDropDownEdit ID="ddeProMode" runat="server" ClientInstanceName="ddeProMode" EnableAnimation="False" Width="100%">
                            <DropDownWindowStyle BackColor="#EDEDED"/>
                            <DropDownWindowTemplate>
                                <dx:ASPxListBox Width="100%" ID="lstProMode" 
                                                ClientInstanceName="lstProMode" 
                                                SelectionMode="CheckColumn"
                                                runat="server">
                                    <Border BorderStyle="None" />
                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddeProMode,e); }" />
                                </dx:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dx:ASPxDropDownEdit>
            </td>
        </tr>
        <tr>
           <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="产品ID" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td colspan="7">
                <dx:ASPxDropDownEdit ID="ddeProID" runat="server" ClientInstanceName="ddeProID" EnableAnimation="False" Width="100%">
                            <DropDownWindowStyle BackColor="#EDEDED"/>
                            <DropDownWindowTemplate>
                                <dx:ASPxListBox Width="100%" ID="lstProID" 
                                                ClientInstanceName="lstProID" 
                                                SelectionMode="CheckColumn"
                                                runat="server">
                                    <Border BorderStyle="None" />
                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddeProID,e); }" />
                                </dx:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dx:ASPxDropDownEdit>
            </td>
        </tr>
        <tr>
           <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="工单号" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td colspan="7">
                <dx:ASPxDropDownEdit ID="ddeWO" runat="server" ClientInstanceName="ddeWO" EnableAnimation="False"  Width="100%">
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
        </tr>
        <tr>
           <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                产品料号
            </td>
            <td colspan="7">
                <dxe:ASPxTextBox ID="txtPartNumber" runat="server" Width="100%">
                </dxe:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="玻璃">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxTextBox ID="txtGlass" runat="server" Width="400px">
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="EVA">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxTextBox ID="txtEVA" runat="server" Width="400px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="硅片供应商" Width="60px">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxTextBox ID="txtSILOT" runat="server" Width="400px">
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="互联条">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxTextBox ID="txtSingle" runat="server" Width="400px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="汇流条">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxTextBox ID="txtBusbar" runat="server" Width="400px">
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="接线盒">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxTextBox ID="txtJunctionbox" runat="server" Width="400px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="设备编码">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxTextBox ID="txtDeviceNum" runat="server" Width="400px">
                </dx:ASPxTextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxButton ID="btnQuery" runat="server" Text="查询" onclick="btnQuery_Click">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <%--<dx:ASPxButton ID="btnExcel" runat="server" Text="Excel" 
                    onclick="btnExcel_Click">
                </dx:ASPxButton>--%>
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
    <dx:ASPxGridView ID="gvCTMReport" runat="server" 
        ondatabound="gvCTMReport_DataBound">
        <SettingsBehavior AllowSort="False" />
        <SettingsPager PageSize="100">
        </SettingsPager>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="expCTMReport" runat="server" 
        GridViewID="gvCTMReport">
    </dx:ASPxGridViewExporter>
   </div>
   <div class="gridview" style="text-align:center">
       <dxchartsui:WebChartControl ID="chartCTMDay" runat="server" 
            Height="300px" Width="1000px">
           <SeriesSerializable>
               <cc1:Series  Name="Series 1" 
                    >
                   <ViewSerializable>
<cc1:LineSeriesView HiddenSerializableString="to be serialized">
                   </cc1:LineSeriesView>
</ViewSerializable>
                   <LabelSerializable>
<cc1:PointSeriesLabel HiddenSerializableString="to be serialized" LineVisible="True">
                       <FillStyle >
                           <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                       </FillStyle>
                   </cc1:PointSeriesLabel>
</LabelSerializable>
                   <PointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                   </cc1:PointOptions>
</PointOptionsSerializable>
                   <LegendPointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized">
                   </cc1:PointOptions>
</LegendPointOptionsSerializable>
               </cc1:Series>
           </SeriesSerializable>
<SeriesTemplate   >
<ViewSerializable>
<cc1:SideBySideBarSeriesView HiddenSerializableString="to be serialized"></cc1:SideBySideBarSeriesView>
</ViewSerializable>

<LabelSerializable>
<cc1:SideBySideBarSeriesLabel HiddenSerializableString="to be serialized" LineVisible="True">
<FillStyle >
<OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized"></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
</cc1:SideBySideBarSeriesLabel>
</LabelSerializable>

<PointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized"></cc1:PointOptions>
</PointOptionsSerializable>

<LegendPointOptionsSerializable>
<cc1:PointOptions HiddenSerializableString="to be serialized"></cc1:PointOptions>
</LegendPointOptionsSerializable>
</SeriesTemplate>

           <DiagramSerializable>
<cc1:XYDiagram>
               <axisx visibleinpanesserializable="-1">
<range sidemarginsenabled="True"></range>
</axisx>
               <axisy visibleinpanesserializable="-1">
<range sidemarginsenabled="True"></range>
</axisy>
           </cc1:XYDiagram>
</DiagramSerializable>

<FillStyle >
<OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized"></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
       </dxchartsui:WebChartControl>
   </div>
   <div class="gridview">
    <div class="gridbar">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-right: 4px;">
                        <dx:ASPxButton ID="btnExcel2" runat="server" Text="Export to XLS" UseSubmitBehavior="False"
                            OnClick="btnExcel2_Click">
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
    <dx:ASPxGridView ID="gvCTMReport2" runat="server" 
        ondatabound="gvCTMReport2_DataBound">
        <SettingsBehavior AllowSort="False" />
        <SettingsPager PageSize="100">
        </SettingsPager>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="expCTMReport2" runat="server" 
        GridViewID="gvCTMReport2">
    </dx:ASPxGridViewExporter>
   </div>
</asp:Content>

