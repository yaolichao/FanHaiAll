﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TableToExcelTemple.aspx.cs" Inherits="Master_TableToExcelTemple" %>

<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>    
        <dx:ASPxGridView ID="grid" runat="server" Width="100%">
        </dx:ASPxGridView>    
    </div>
    <dx:ASPxGridViewExporter ID="expores" runat="server" GridViewID="grid" >
    </dx:ASPxGridViewExporter>
    </form>
</body>
</html>
