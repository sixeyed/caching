<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Page2.aspx.cs" Inherits="Sixeyed.Caching.Tests.Stubs.Website.Page2" %>
<%@ OutputCache CacheProfile="Disk"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h1>ASP.NET WebForms with Output Caching</h1>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="timestampLabel" runat="server"/>
    </div>
    </form>
</body>
</html>
