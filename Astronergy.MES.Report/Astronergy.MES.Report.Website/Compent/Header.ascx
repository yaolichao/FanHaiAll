<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Compent_Header" %>
<link href="<%= Page.ResolveUrl("~/") %>styles/ddsmoothmenu.css" rel="stylesheet" type="text/css" />

<div id="header">  
  <div class="top" style="margin-top:-15px;">
		<div class="logo">		
		<a href="<%= Page.ResolveUrl("~/") %>"><asp:Image ID="imgLogo" 
                ImageUrl="~/images/login_121.jpg" runat="server" 
                style="height:70px" Width="150px" /></a>
    	</div>
		
	</div>
     
    </div>
