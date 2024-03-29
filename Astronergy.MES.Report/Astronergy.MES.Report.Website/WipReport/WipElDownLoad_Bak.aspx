<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="WipElDownLoad_Bak.aspx.cs" Inherits="WipElDownLoad_Bak" Title="<%$ Resources:lang,PageTitle %>" %>

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
    <link href="../Scripts/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function EnterTextBox(button) {
            if (event.keyCode == 13) {
                var text = txtGuiNum.GetText();
                if (text == "") {
                    return false;
                }
                __doPostBack("ctl00_ContentPlaceHolder1_ASPTXTGUINUM_I", null);
                return true;
            }
            return false;
        }
        function EnterTextBox1(button) {
            if (event.keyCode == 13) {
                var text = txtPalletNo.GetText();
                if (text == "") {
                    return false;
                }
                __doPostBack("ctl00_ContentPlaceHolder1_ASPTXTPALLETNO_I", null);

                return true;
            }
            return false;
        }
        function EnterMemo(button) {
            if (event.keyCode == 13) {
                var text = txtXueLieNum.GetText();
                if (text == "") {
                    return false;
                }
                __doPostBack("ctl00_ContentPlaceHolder1_ASPMEMOXULIENUM_I", null);

                return true;
            }
            return false;
        } 
    </script>

    <style>
        .SearchList_Search_td
        {
            border-right: 1px solid #cccccc;
            border-bottom: 1px solid #cccccc;
            border-top-style: none;
            border-left-style: none;
            border-right-style: dashed;
            border-bottom-style: dashed;
            text-align: left;
            padding-left: 4px;
            padding-right: 4px;
            padding-top: 2px;
            padding-bottom: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownLoad" />
            <asp:PostBackTrigger ControlID="btnIvDownLoad" />
            <asp:PostBackTrigger ControlID="btnTCardDownLoad" />
            <asp:PostBackTrigger ControlID="btnXlsExport" />
            <asp:PostBackTrigger ControlID="grid" />
        </Triggers>
        <ContentTemplate>
            <div class="searchbar" style="height: 105px; padding: 0px; width: 100%">
                <table border="0" width="98%" style="margin: 5px; height: 29px; border: solid 1px #cccccc;">
                    <tr>
                        <td class="SearchList_Search_td">
                            出 货 单 号
                        </td>
                        <td style="width: 200px" class="SearchList_Search_td">
                            <dxe:ASPxTextBox ID="ASPTXTGUINUM" runat="server" Width="98%" ClientInstanceName="txtGuiNum"
                                OnTextChanged="ASPTXTGUINUM_TextChanged">
                                <ClientSideEvents KeyPress="function(s, e) { EnterTextBox(s); }" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="width: 100px" class="SearchList_Search_td">
                            托盘号
                        </td>
                        <td style="width: 200px" class="SearchList_Search_td">
                            <dxe:ASPxTextBox ID="ASPTXTPALLETNO" runat="server" Width="98%" ClientInstanceName="txtPalletNo"
                                OnTextChanged="ASPTXTPALLETNO_TextChanged">
                                <ClientSideEvents KeyPress="function(s, e) { EnterTextBox1(s); }" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px" class="SearchList_Search_td">
                            组件序列号
                        </td>
                        <td colspan="3" class="SearchList_Search_td">
                            <dxe:ASPxTextBox ID="ASPMEMOXULIENUM" runat="server" Width="99%">
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SearchList_Search_td">
                            <asp:Button ID="btnSelect" runat="server" Text="查 询" Width="100px" UseSubmitBehavior="true"
                                OnClick="btnSelect_Click" />
                        </td>
                        <td class="SearchList_Search_td">
                            <asp:Button ID="btnDownLoad" runat="server" Text="下载EL图片" Width="100px" UseSubmitBehavior="false"
                                OnClick="btnDownLoad_Click" />
                            <asp:Button ID="btnIvDownLoad" runat="server" Text="下载IV测试图片" Width="100px" OnClick="btnIvDownLoad_Click" />
                            <asp:Button ID="btnTCardDownLoad" runat="server" Text="下载TCard图片" Width="100px" OnClick="btnTCardDownLoad_Click" />
                        </td>
                        <td class="SearchList_Search_td" colspan="2">
                            <asp:Button ID="btnXlsExport" runat="server" Text="EXCL导出" Width="100px" OnClick="btnXlsExport_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel ID="UpdatePanelResult" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="gridview">
                        <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" Width="100%"
                            Enabled="true" OnHtmlRowPrepared="grid_HtmlRowPrepared" OnCustomColumnDisplayText="grid_CustomColumnDisplayText">
                            <SettingsPager PageSize="20">
                            </SettingsPager>
                            <SettingsText EmptyDataRow="没有查到数据!" />
                            <SettingsBehavior AllowDragDrop="false" ColumnResizeMode="Control" />
                            <Settings ShowVerticalScrollBar="true" VerticalScrollableHeight="500" ShowHorizontalScrollBar="true"
                                UseFixedTableLayout="false" />
                            <Columns>
                                <dxwgv:GridViewDataColumn FieldName="行号" Caption="行号" VisibleIndex="0" Width="4%" />
                                <dxwgv:GridViewDataColumn FieldName="出货单号" Caption="出货单号" VisibleIndex="1" Width="8%" />
                                <dxwgv:GridViewDataColumn FieldName="托号" Caption="托号" VisibleIndex="2" Width="9%" />
                                <dxwgv:GridViewDataColumn FieldName="组件序列号" Caption="组件序列号" VisibleIndex="3" Width="12%" />
                                <dxwgv:GridViewDataColumn FieldName="测试时间" Caption="测试时间" VisibleIndex="4" Width="7%" />
                                <dxwgv:GridViewDataColumn FieldName="EL图片地址" Caption="EL图片地址" VisibleIndex="5" Width="15%" />
                                <dxwgv:GridViewDataColumn FieldName="IV测试图片地址" Caption="IV测试图片地址" VisibleIndex="6"
                                    Width="15%" />
                                <dxwgv:GridViewDataColumn FieldName="TCard图片地址(1)" Caption="TCard图片地址(1)" VisibleIndex="7"
                                    Width="15%" />
                                <dxwgv:GridViewDataColumn FieldName="TCard图片地址(2)" Caption="TCard图片地址(2)" VisibleIndex="8"
                                    Width="15%" />
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
