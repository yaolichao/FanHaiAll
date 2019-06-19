<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="FragmentationRate.aspx.cs" Inherits="WipReport_FragmentationRate" Title="Untitled Page" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="searchbar" style="height:auto;padding:0px;width:100%">
    <table class=" searchbar_table" width="100%"><!--style1-->
        <tr>
            <td class="searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="厂别" Width="50px">
                </dx:ASPxLabel>
            </td>
            <td class="searchbar_table_td"><!-- Width="60px"-->
                <dx:ASPxComboBox ID="cboFactory" runat="server">
                </dx:ASPxComboBox>
            </td>
            <td class="searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="日期" Width="30px">
                </dx:ASPxLabel>
            </td>
            <td class="searchbar_table_td"><!-- Width="120px"-->
                <dx:ASPxDateEdit ID="deStartDate" runat="server">
                </dx:ASPxDateEdit>
            </td>
            <td class="searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="~" Width="10px">
                </dx:ASPxLabel>
            </td>
            <td class="searchbar_table_td"><!-- Width="120px"-->
                <dx:ASPxDateEdit ID="deEndDate" runat="server">
                </dx:ASPxDateEdit>
            </td>
            <td class="searchbar_table_td">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="班别" Width="30px">
                </dx:ASPxLabel>
            </td>
            <td class="searchbar_table_td"><!-- Width="60px"-->
                <dx:ASPxComboBox ID="cboShif" runat="server">
                </dx:ASPxComboBox>
            </td>
            <td class="searchbar_table_td">
                <dx:ASPxButton ID="btnQuery" runat="server" Text="查询" onclick="btnQuery_Click" 
                    Width="60px">
                </dx:ASPxButton>
            </td>
            <!--<td class="searchbar_table_td">-->
                <%--<dx:ASPxButton ID="btnExcel" runat="server" Text="Excel"
                    onclick="btnExcel_Click">
                </dx:ASPxButton>--%>
            <!--</td>-->
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
    <dx:ASPxGridView ID="gvFragmentation" runat="server" 
    KeyFieldName="FITME" 
          oncustomcolumndisplaytext="gvFragmentation_CustomColumnDisplayText">
        <SettingsPager PageSize="100">
        </SettingsPager>
        <Settings ShowColumnHeaders="False" />
    </dx:ASPxGridView>
      <asp:HiddenField ID="hidLoactionKey" runat="server" />
      <asp:HiddenField ID="hidPartType" runat="server" />
      <asp:HiddenField ID="hidShiftName" runat="server" />
      <asp:HiddenField ID="hidStartDate" runat="server" />
      <asp:HiddenField ID="hidEndDate" runat="server" />
    <dx:ASPxGridViewExporter ID="FrateExporter" runat="server" 
        GridViewID="gvFragmentation">
    </dx:ASPxGridViewExporter>
  </div>
</asp:Content>

