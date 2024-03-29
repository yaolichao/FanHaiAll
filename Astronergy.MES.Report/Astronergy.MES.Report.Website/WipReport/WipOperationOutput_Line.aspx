<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="WipOperationOutput_Line.aspx.cs" Inherits="WipReport_WipOperationOutput_Line" Title="<%$ Resources:lang,PageTitle %>" %>

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
    </script>
    <style type="text/css">
        .style1
        {
            width: 170px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelContent" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXlsExport"/>
        </Triggers>
        <ContentTemplate>        
            <div class="searchbar" style="height:140px;padding:0px">
                <table border="0" width="98%" style="margin:5px;">
                    <tr>
                        <td>
                            车间
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cmbWorkPlace" runat="server" Width="100px" TextField="LOCATION_NAME"
                                ValueField="LOCATION_KEY" ValueType="System.String">
                            </dxe:ASPxComboBox>
                        </td>                       
                        <td>
                            工单号
                        </td>
                     <td class="style1">
                            <dxe:ASPxComboBox ID="cmbWorkOrderNumber" Width="161px" runat="server"
                                TextField="WORK_ORDER_NO" ValueField="WORK_ORDER_KEY" 
                                DropDownStyle="DropDown" EnableIncrementalFiltering="True" 
                                AutoPostBack="True" Height="20px">
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                           产品料号
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cmbPartNumber" Width="150px" runat="server" TextField="PART_NUMBER"
                                ValueField="PART_NUMBER" DropDownStyle="DropDown">
                            </dxe:ASPxComboBox>
                        </td>
                         <td>
                           产品ID号
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cmbProduct" Width="150px" runat="server" TextField="PRODUCT_CODE"
                                ValueField="PRODUCT_CODE" DropDownStyle="DropDown" EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                     <td>
                           线别
                        </td>
                        <td>
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
                        <td colspan="2" valign="top">
                            <table border="0" width="100%" style="border:solid 1px">
                                <tr>
                                    <td colspan="2">
                                        <dxe:ASPxRadioButton ID="rbtHistory" runat="server" Text="按日期查询"  ClientInstanceName="rbtHistory" GroupName="query">
                                            <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(false) }" />
                                        </dxe:ASPxRadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dxe:ASPxRadioButton ID="rbtCurrent" runat="server" Text="按时间查询"  ClientInstanceName="rbtCurrent" Checked="true" GroupName="query">
                                            <ClientSideEvents CheckedChanged="function(s, e) { ShowTimeControl(true) }" />
                                        </dxe:ASPxRadioButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2"  valign="top">
                            <table border="0" width="100%"  style="border:solid 1px">
                                <tr>
                                    <td>开始时间</td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dateStart" runat="server">
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <div id="divStartTime" style="display:none">
                                        <dxe:ASPxTimeEdit ID="deStartTime" runat="server" EditFormat="Custom" EditFormatString="HH:mm:ss">
                                        </dxe:ASPxTimeEdit> 
                                        </div>    
                                    </td>
                                </tr>
                                <tr>
                                    <td>结束时间</td>
                                    <td>

                                        <dxe:ASPxDateEdit ID="dateEnd" runat="server">
                                        </dxe:ASPxDateEdit>

                                    </td>
                                    <td>
                                        <div id="divEndTime" style="display:none">
                                        <dxe:ASPxTimeEdit ID="deEndTime" runat="server" EditFormat="Custom" EditFormatString="HH:mm:ss">
                                        </dxe:ASPxTimeEdit>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2" align="center" valign="middle">
                        <asp:Button ID="btnQuery" runat="server" Text="查 询" OnClick="btnQuery_Click" Width="80px"/>
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
                <dxchartsui:WebChartControl ID="chartOutput" runat="server" Height="300px" Width="1000" ShowLoadingPanel="true">
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
