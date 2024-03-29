<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true"
    CodeFile="ConergyFlashDateExport.aspx.cs" Inherits="ConergyFlashDataExport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>



<%@ Register Assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            $("#divGrid").width($(".searchbar").width());
        })
        function BeginRequestHandler(sender, args) {
            $("#<%=this.btnQuery.ClientID%>").attr("disabled", "disabled");
            $("#<%=this.btnQuery.ClientID%>").val("查询中...");
        }

        function EndRequestHandler(sender, args) {
            $("#<%=this.btnQuery.ClientID%>").removeAttr("disabled");
            $("#<%=this.btnQuery.ClientID%>").val("查 询");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXlsExport" />
        </Triggers>
        <ContentTemplate>
            <div class="searchbar" style="height: 120px; padding: 5px; width: 100%">
                <table class="style1" style="margin: 3px;">
                    <tr>
                        <td width="8%">
                            <dx:ASPxLabel ID="lblShipingDate" runat="server" Text="出货日期">
                            </dx:ASPxLabel>
                        </td>
                        <td width="170px">
                            <dx:ASPxDateEdit ID="deStartShippingData" runat="server" AutoPostBack="True" 
                                DateOnError="Today">
                            </dx:ASPxDateEdit>
                        </td>
                        <td width="2%">
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="~">
                            </dx:ASPxLabel>
                        </td>
                        <td width="22%">
                            <dx:ASPxDateEdit ID="deEndShippingData" runat="server" AutoPostBack="True">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="8%">
                            <dx:ASPxLabel ID="lblInvoiceNumber" runat="server" Text="发 票 号">
                            </dx:ASPxLabel>
                        </td>
                        <td width="170px">
                            <dx:ASPxTextBox ID="txtInvoiceNumber" runat="server" Width="170px">
                            </dx:ASPxTextBox>
                        </td>
                        <td width="2%">
                            &nbsp;</td>
                        <td width="22%">
                            &nbsp;</td>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="8%">
                            <dx:ASPxLabel ID="lblDeliveryNumber" runat="server" Text="集装箱号">
                            </dx:ASPxLabel>
                        </td>
                        <td width="23%">
                            <dx:ASPxTextBox ID="txtDeliveryNumber" runat="server" Width="170px">
                            </dx:ASPxTextBox>
                        </td>
                        <td width="2%">
                            &nbsp;
                        </td>
                        <td width="22%">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" Width="70px" />
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
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
                        <dxwgv:ASPxGridView ID="gvConergyFlash" runat="server" Width="100%" Enabled="true">
                            <SettingsPager PageSize="20">
                            </SettingsPager>
                            <SettingsBehavior AllowDragDrop="false" ColumnResizeMode="Control" />
                            <Settings ShowVerticalScrollBar="true" VerticalScrollableHeight="500" ShowHorizontalScrollBar="true"
                                UseFixedTableLayout="false" />
                        </dxwgv:ASPxGridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--            <div>
                <asp:UpdatePanel ID="UpdatePanelResult" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="gridview">
                            <div class="gridbar" id="divGridBar">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 4px">
                                            <dxe:ASPxButton ID="btnXlsExport" runat="server" Text="导出到EXCEL" UseSubmitBehavior="False">
                                                <Image Url="~/Images/xls.jpg" Width="16" Height="16">
                                                </Image>
                                            </dxe:ASPxButton>
                                        </td>
                                        <td style="padding-right: 4px">
                                        </td>
                                        <td style="padding-right: 4px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <dx:ASPxGridView ID="gvConergyFlash" runat="server" Width="100%" ClientInstanceName="gvConergyFlash">
                                    <SettingsBehavior AllowDragDrop="False" ColumnResizeMode="Control" />
                                    <SettingsPager PageSize="20">
                                    </SettingsPager>
                                    <Settings ShowHorizontalScrollBar="True" ShowVerticalScrollBar="True" VerticalScrollableHeight="500" />
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
