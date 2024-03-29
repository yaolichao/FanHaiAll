<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="PassYieldData.aspx.cs" Inherits="Quality_PassYieldData" Title="<%$ Resources:lang,PageTitle %>" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function ShowTimeControl(isShow) 
        {
            if (isShow == true) 
            {
                $("#divStartTime").css("display", "block");
                $("#divEndTime").css("display", "block");
                $("#divChartPassYield").css("display", "none");
            }
            else 
            {
                $("#divStartTime").css("display", "none");
                $("#divEndTime").css("display", "none");
                $("#divChartPassYield").css("display", "block");
            }
        }
        $(document).ready(function() {
            ShowTimeControl(!rbtByDay.GetChecked());
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        })
        function BeginRequestHandler(sender, args) 
        {
            $("#<%=this.btnQuery.ClientID%>").attr("disabled", "disabled");
            $("#<%=this.btnQuery.ClientID%>").val("查询中...");
        }

        function EndRequestHandler(sender, args) 
        {
            $("#<%=this.btnQuery.ClientID%>").removeAttr("disabled");
            $("#<%=this.btnQuery.ClientID%>").val("查 询");
            $("#divGrid").width($(".searchbar").width());
            ShowTimeControl(!rbtByDay.GetChecked());
        }
        // <![CDATA[
        var textSeparator = ",";
        //当ASPxListBox中选中项目改变时函数。
        function OnListBoxSelectionChanged(lst,cmb, args) {
            if (args.index == 0)
                args.isSelected ? lst.SelectAll() : lst.UnselectAll();
            UpdateSelectAllItemState(lst);
            UpdateText(cmb,lst);
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
    <style type="text/css">
        .style1
        {
            width: 52px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXlsExport"/>
        </Triggers>
        <ContentTemplate>
            <div class="searchbar" style="height:auto;padding:0px;width:100%">
            <table class="searchbar_table" border="0" width="100%">
                <tr>
                    <td class="searchbar_table_td" style="width:10%">
                        车间
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                        <asp:UpdatePanel ID="UpdatePanelWorkPlace" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <dxe:ASPxComboBox ID="cmbWorkPlace" runat="server" TextField="LOCATION_NAME" AutoPostBack="true"
                            ValueField="LOCATION_KEY" ValueType="System.String" OnSelectedIndexChanged="cmbWorkPlace_SelectedIndexChanged">
                        </dxe:ASPxComboBox>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="searchbar_table_td" style="width:10%">
                       客户类别
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                        <dxe:ASPxDropDownEdit ID="ddeCustomerType" runat="server" ClientInstanceName="ddeCustomerType" EnableAnimation="False" ReadOnly="true">
                            <DropDownWindowStyle BackColor="#EDEDED"/>
                            <DropDownWindowTemplate>
                                <dxe:ASPxListBox Width="100%" ID="lstCustomerType" 
                                                ClientInstanceName="lstCustomerType" 
                                                SelectionMode="CheckColumn"
                                                runat="server">
                                    <Border BorderStyle="None" />
                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e){ OnListBoxSelectionChanged(s,ddeCustomerType,e); }" />
                                </dxe:ASPxListBox>
                            </DropDownWindowTemplate>
                        </dxe:ASPxDropDownEdit>
                    </td>
                    <td class="searchbar_table_td" style="width:10%">
                       产品型号
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                         <dxe:ASPxDropDownEdit ID="ddeProductModel" runat="server" ClientInstanceName="ddeProductModel" EnableAnimation="False" ReadOnly="true">
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
                    <td class="searchbar_table_td"></td>
                </tr>
                <tr>
                    <td class="searchbar_table_td" style="width:10%">
                        工单号
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                        <asp:UpdatePanel ID="UpdatePanelWorkOrderNumber" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <dxe:ASPxDropDownEdit ID="ddeWorkOrderNumber" runat="server" ClientInstanceName="ddeWorkOrderNumber" EnableAnimation="False" ReadOnly="true">
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
                        </dxe:ASPxDropDownEdit>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="searchbar_table_td" style="width:10%">
                        产品料号
                    </td>
                    <td class="searchbar_table_td" style="width:20%">
                        <dxe:ASPxTextBox ID="textPartNumber" runat="server" Width="170px">
                             </dxe:ASPxTextBox>
                    </td>
                    <td class="searchbar_table_td" colspan="3"></td>
                </tr>
                <tr>
                    <td class="searchbar_table_td" colspan="2" valign="top">
                        <span>
                                    <dxe:ASPxRadioButton ID="rbtByDay" runat="server" Text="按天累计查询" Checked="True"
                                        GroupName="query" ClientInstanceName="rbtByDay">
                                        <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(false) }" />
                                    </dxe:ASPxRadioButton>
                        </span>
                        <span>
                                    <dxe:ASPxRadioButton ID="rbtByTimeRange" runat="server" Text="按时间段查询" GroupName="query">
                                        <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(true) }" />
                                    </dxe:ASPxRadioButton>
                        </span>
                    </td>
                    <td class="searchbar_table_td">开始时间</td>
                    <td class="searchbar_table_td">
                                    <dxe:ASPxDateEdit ID="dateStart" runat="server">
                                        <CalendarProperties TodayButtonText="今天" ClearButtonText="清空">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                    <div id="divStartTime" style="display:none">
                                    <dxe:ASPxTimeEdit ID="deStartTime" runat="server" EditFormat="Custom" EditFormatString="HH:mm:ss">
                                    </dxe:ASPxTimeEdit>
                                    </div>
                                </td>
                    <td class="searchbar_table_td">结束时间</td>
                    <td class="searchbar_table_td">
                                    <dxe:ASPxDateEdit ID="dateEnd" runat="server">
                                        <CalendarProperties TodayButtonText="今天" ClearButtonText="清空">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                <div id="divEndTime" style="display:none">
                                    <dxe:ASPxTimeEdit ID="deEndTime" runat="server" EditFormat="Custom" EditFormatString="HH:mm:ss">
                                    </dxe:ASPxTimeEdit>
                                 </div>
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
            <div class="gridview" style="text-align:center" id="divChartPassYield">
                <asp:UpdatePanel ID="UpdatePanelChart" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <dxchartsui:WebChartControl ID="chartPassYield" runat="server" Height="300px" 
                        Width="1000px" >
                    <SeriesSerializable>
                        <cc1:Series   >
                        </cc1:Series>
                    </SeriesSerializable>
                    <SeriesTemplate  
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
                    </SeriesTemplate>
                    <DiagramSerializable>
<cc1:XYDiagram>
                        <axisx visibleinpanesserializable="-1">
                            <range sidemarginsenabled="True"></range>
                        </axisx>
                        <axisy visibleinpanesserializable="-1">
                            <range sidemarginsenabled="True"></range>
                            <numericoptions format="Percent"></numericoptions>
                        </axisy>
                        <secondaryaxesy>
                        <cc1:SecondaryAxisY AxisID="0" Name="SecondaryAxisY1" VisibleInPanesSerializable="-1">
                            <range sidemarginsenabled="True"></range>
                            <numericoptions format="Percent"></numericoptions>
                        </cc1:SecondaryAxisY>
                        </secondaryaxesy>
                    </cc1:XYDiagram>
</DiagramSerializable>
                    <FillStyle >
                        <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                    </FillStyle>
                </dxchartsui:WebChartControl>
                </ContentTemplate>
                </asp:UpdatePanel>
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
            <div style="/*position:absolute;*/width:1024px; overflow:auto" id="divGrid">
                <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server"
                    OnCustomColumnDisplayText="grid_CustomColumnDisplayText" Width="100%"
                    Enabled="false" OnHtmlRowCreated="grid_HtmlRowCreated" ondatabound="grid_DataBound">
                    <SettingsPager PageSize="10000">
                    </SettingsPager>
                    <%-- BeginRegion Grid Columns --%>
                    <%-- EndRegion --%>
                </dxwgv:ASPxGridView>
            </div>
            </div>
             <dxwgv:ASPxGridView ID="gridExport" runat="server" OnCustomColumnDisplayText="grid_CustomColumnDisplayText" 
                Visible="false">
                <SettingsPager PageSize="10000">
                </SettingsPager>
            </dxwgv:ASPxGridView>
            <dxwgv:ASPxGridViewExporter ID="exporter" runat="server" onrenderbrick="exporter_RenderBrick" GridViewID="gridExport"/>
            </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
