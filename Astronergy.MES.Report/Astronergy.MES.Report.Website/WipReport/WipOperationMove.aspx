<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="WipOperationMove.aspx.cs" Inherits="WipReport_WipOperationMove" Title="<%$ Resources:lang,PageTitle %>" %>

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
            }
            else 
            {
                $("#divStartTime").css("display", "none");
                $("#divEndTime").css("display", "none");
            }
        }
        $(document).ready(function() {
            ShowTimeControl(!rbtHistory.GetChecked());
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        })
        
        function BeginRequestHandler(sender, args) {
            $("#<%=this.btnQuery.ClientID%>").attr("disabled", "disabled");
        }

        function EndRequestHandler(sender, args) {
            $("#<%=this.btnQuery.ClientID%>").removeAttr("disabled");
            $("#divGrid").width($(".searchbar").width());
            ShowTimeControl(!rbtHistory.GetChecked());
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelContent" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXlsExport"/>
        </Triggers>
        <ContentTemplate>        
            <div class="searchbar" style="height:auto;padding:0px">
                <table class="searchbar_table" border="0" width="100%" style="margin:5px;">
                    <tr>
                        <td class="searchbar_table_td">
                            车间
                        </td>
                        <td class="searchbar_table_td">
                            <dxe:ASPxComboBox ID="cmbWorkPlace" runat="server" TextField="LOCATION_NAME"
                                ValueField="LOCATION_KEY" ValueType="System.String">
                            </dxe:ASPxComboBox>
                        </td>                        
                        <td class="searchbar_table_td">
                            工单号
                        </td>
                        <td class="searchbar_table_td">
                            <dxe:ASPxComboBox ID="cmbWorkOrderNumber" runat="server"
                                TextField="WORK_ORDER_NO" ValueField="WORK_ORDER_KEY" 
                                DropDownStyle="DropDown" EnableIncrementalFiltering="True" 
                                AutoPostBack="True">
                            </dxe:ASPxComboBox>
                        </td>
                        <td class="searchbar_table_td">
                           产品料号
                        </td>
                        <td class="searchbar_table_td">
                            <dxe:ASPxComboBox ID="cmbPartNumber" runat="server" TextField="PART_NUMBER"
                                ValueField="PART_NUMBER" DropDownStyle="DropDown">
                            </dxe:ASPxComboBox>
                        </td>
                        <td class="searchbar_table_td">
                           产品ID号
                        </td>
                        <td class="searchbar_table_td">
                            <dxe:ASPxComboBox ID="cmbProduct" runat="server" TextField="PRODUCT_CODE"
                                ValueField="PRODUCT_CODE" DropDownStyle="DropDown" EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="searchbar_table_td" colspan="2" valign="top">
                            <span>
                                <dxe:ASPxRadioButton ID="rbtHistory" runat="server" Text="按日期查询"  ClientInstanceName="rbtHistory" GroupName="query">
                                    <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(false) }" />
                                </dxe:ASPxRadioButton>
                            </span>
                            <span>
                                <dxe:ASPxRadioButton ID="rbtCurrent" runat="server" Text="按时间查询"  ClientInstanceName="rbtCurrent" Checked="true" GroupName="query">
                                    <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(true) }" />
                                </dxe:ASPxRadioButton>
                            </span>
                        </td>
                        <td class="searchbar_table_td" valign="top">开始时间</td>
                        <td class="searchbar_table_td" valign="top">
                            <span>
                                        <dxe:ASPxDateEdit ID="dateStart" runat="server">
                                        </dxe:ASPxDateEdit>
                            </span>
                            <span>~</span>
                            <span>
                                        <div id="divStartTime" style="display:none">
                                        <dxe:ASPxTimeEdit ID="deStartTime" runat="server" EditFormat="Custom" EditFormatString="HH:mm:ss">
                                        </dxe:ASPxTimeEdit>
                                        </div>
                                    </span>
                        </td>
                        <td class="searchbar_table_td" valign="top">结束时间</td>
                        <td class="searchbar_table_td" valign="top">
                            <span>

                                        <dxe:ASPxDateEdit ID="dateEnd" runat="server">
                                        </dxe:ASPxDateEdit>

                            </span>
                            <span>~</span>
                            <span>
                                        <div id="divEndTime" style="display:none">
                                        <dxe:ASPxTimeEdit ID="deEndTime" runat="server" EditFormat="Custom" EditFormatString="HH:mm:ss">
                                        </dxe:ASPxTimeEdit>
                                        </div>
                                    </span>
                        </td>
                        <td class="searchbar_table_td" colspan="2" align="center" valign="middle">
                        <asp:Button ID="btnQuery" runat="server" Text="查 询" class="searchbar_table_btu" OnClick="btnQuery_Click" Width="80px"/>
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/Loading2.gif"  alt="Loading..." class="progressImage" />                       
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </div>
       
            <div class="gridview" style="text-align:center">
                <dxchartsui:WebChartControl ID="chartMove" runat="server" Height="300px" Width="1000" ShowLoadingPanel="true">
                    <SeriesTemplate  
                         
                        >
                        <ViewSerializable>
<cc1:SideBySideBarSeriesView HiddenSerializableString="to be serialized">
                        </cc1:SideBySideBarSeriesView>
</ViewSerializable>
                        <LabelSerializable>
<cc1:SideBySideBarSeriesLabel HiddenSerializableString="to be serialized" LineVisible="True">
                            <FillStyle >
                                <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                            </FillStyle>
                        </cc1:SideBySideBarSeriesLabel>
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
                    <FillStyle >
                        <OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                    </FillStyle>
                </dxchartsui:WebChartControl>
            </div>
            <div class="gridview">
                <div class="gridbar">
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
                <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" Width="100%"
                    OnCustomColumnDisplayText="grid_CustomColumnDisplayText" 
                    Enabled="false" OnHtmlRowCreated="grid_HtmlRowCreated" ondatabound="grid_DataBound">
                    <SettingsPager PageSize="10000">
                    </SettingsPager>
                    <%-- BeginRegion Grid Columns --%>
                    <%-- EndRegion --%>
                </dxwgv:ASPxGridView>
                <dxwgv:ASPxGridView ID="gridExport" runat="server" OnCustomColumnDisplayText="grid_CustomColumnDisplayText" 
                    Visible="false" ondatabound="grid_DataBound">
                    <SettingsPager PageSize="10000">
                    </SettingsPager>
                </dxwgv:ASPxGridView>
                <dxwgv:ASPxGridViewExporter ID="exporter" runat="server" 
                    onrenderbrick="exporter_RenderBrick" GridViewID="gridExport"/>
            </div>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
