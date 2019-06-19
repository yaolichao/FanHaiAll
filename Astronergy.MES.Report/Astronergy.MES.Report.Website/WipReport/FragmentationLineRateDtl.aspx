<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FragmentationLineRateDtl.aspx.cs"
    Inherits="WipReport_FragmentationLineRateDtl" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="../Compent/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<%@ Register assembly="DevExpress.XtraCharts.v18.1.Web, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <uc1:Header ID="Header1" runat="server" />
    </div>
    <div>
        <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Height="200px" 
            Width="1100px">
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

<FillStyle >
<OptionsSerializable>
<cc1:SolidFillOptions HiddenSerializableString="to be serialized"></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
        </dxchartsui:WebChartControl>
        <br />
        <asp:GridView ID="gdvPatchTop" runat="server">
        </asp:GridView>
    </div>
    <div>
        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="Export to XLS" UseSubmitBehavior="False"
            OnClick="btnXlsExport_Click" Width="120px">
            <Image Url="~/Images/xls.jpg" Width="16" Height="16">
            </Image>
        </dx:ASPxButton>
    </div>
    <div>
        <div class="grid">
            <dx:ASPxGridView ID="grid" runat="server" Width="100%" OnBeforeColumnSortingGrouping="grid_BeforeColumnSortingGrouping">
                <Styles>
                    <Cell Wrap="False">
                    </Cell>
                    <AlternatingRow BackColor="#F2F1FC">
                    </AlternatingRow>
                </Styles>
                <SettingsPager AlwaysShowPager="True" Mode="ShowAllRecords">
                </SettingsPager>
            </dx:ASPxGridView>
        </div>
    </div>
    <dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="grid">
    </dx:ASPxGridViewExporter>
    </form>
</body>
</html>
