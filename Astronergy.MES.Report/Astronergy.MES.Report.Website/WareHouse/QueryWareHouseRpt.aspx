<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="QueryWareHouseRpt.aspx.cs" Inherits="WareHouse_QueryWareHouseRpt" Title="<%$ Resources:lang,PageTitle %>" %>

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

<script runat="server">

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Scripts/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            $("#divGrid").width($(".searchbar").width());
            $(".dateStart").focus(function() {
                WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', readOnly: true });
            });
            $(".dateEnd").focus(function() {
                var id = $(".dateStart").attr("id");
                WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', readOnly: true, minDate: '#F{$dp.$D(\'' + id + '\')}' });
            });
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

    <style type="text/css">
        .style1
        {
            width: 10%;
            height: 22px;
        }
        .style2
        {
            width: 20%;
            height: 22px;
        }
        .style3
        {
            width: 187px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXlsExport" />
        </Triggers>
        <ContentTemplate>
            <div class="searchbar" style="height: 40px; padding: 0px; width: 100%">
                <table border="0" width="98%" style="margin: 5px;">
                    <tr>
                        <td class="style1">
                            工单号
                        </td>
                        <td colspan="3" class="style2">
                            <dxe:ASPxTextBox ID="ddeWorkOrderNumber" runat="server" Width="150px">
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="white-space: nowrap;">
                            入库时间
                        </td>
                        <td class="style3">
                            <input id="dateStart" type="text" runat="server" class="Wdate dateStart" style="width: 170px" />
                        </td>
                        <td>
                            至
                        </td>
                        <td>
                            <input id="dateEnd" type="text" runat="server" class="Wdate dateEnd" style="width: 170px" />
                        </td>
                        <td align="center" valign="middle">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnQuery" runat="server" Text="查 询" OnClick="btnQuery_Click" 
                                        Width="80px" />
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
                        <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
                            Width="100%" Enabled="true" OnHtmlRowCreated="grid_HtmlRowCreated" OnDataBound="grid_DataBound">
                            <SettingsPager PageSize="20">
                            </SettingsPager>
                            <SettingsBehavior AllowDragDrop="false" ColumnResizeMode="Control" />
                            <Settings ShowVerticalScrollBar="true" VerticalScrollableHeight="500" ShowHorizontalScrollBar="true"
                                UseFixedTableLayout="false" GridLines="Vertical" />
                        </dxwgv:ASPxGridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
