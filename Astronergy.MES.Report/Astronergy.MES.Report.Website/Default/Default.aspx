<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="WipReport_Default" Title="报表首页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
  window.onload = WindowLoading;
  function WindowLoading()
  {
	//document.getElementById('ControlExpand').style.display='none';
  }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Image ID="Image1" runat="server"  ImageUrl="~/Images/bg.jpg"  />
</asp:Content>

