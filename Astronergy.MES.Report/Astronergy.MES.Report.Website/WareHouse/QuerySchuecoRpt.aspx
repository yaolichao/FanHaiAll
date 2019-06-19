<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="QuerySchuecoRpt.aspx.cs" Inherits="QuerySchuecoRpt" Title="Untitled Page" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 68px;
        }
        .style3
        {
            width: 14px;
        }
        .style4
        {
            width: 130px;
        }
        .style5
        {
            width: 130px;
        }
        .style6
        {
            width: 50px;
        }
        .style7
        {
        	width:73px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="searchbar" style="height:30px;padding:0px;width:100%">
    <table class="style1">
        <tr>
            <td class="style6">
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="厂别：" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td class="style7">
                <dx:ASPxComboBox ID="cboFactory" runat="server" Width="70px">
                </dx:ASPxComboBox>
            </td>
            <td class="style2">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="入库日期：" Width="60px">
                </dx:ASPxLabel>
            </td>
            <td class="style4">
                <dx:ASPxDateEdit ID="deStartDate" runat="server" Width="120px">
                </dx:ASPxDateEdit>
            </td>
            <td class="style3">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="~" Width="10px">
                </dx:ASPxLabel>
            </td>
            <td class="style5">
                <dx:ASPxDateEdit ID="deEndDate" runat="server" Width="120px">
                </dx:ASPxDateEdit>
            </td>
            <td>
                <dx:ASPxButton ID="btnQuey" runat="server" onclick="btnQuey_Click" Text="查询">
                </dx:ASPxButton>
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
    <dx:ASPxGridView ID="gvWareHouse" runat="server" 
            ondatabound="gvWareHouse_DataBound" 
            onhtmlrowcreated="gvWareHouse_HtmlRowCreated">
            <SettingsBehavior AllowSort="False" />
            <SettingsPager PageSize="20">
            </SettingsPager>
            <Settings ShowColumnHeaders="False" />
        </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="gvWareHouse">
    </dx:ASPxGridViewExporter>
  </div>
</asp:Content>

