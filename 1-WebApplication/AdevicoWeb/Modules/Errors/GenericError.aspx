<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenericError.aspx.vb" Inherits="Comunita_OnLine.GenericError" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <asp:Literal ID="LTskin" runat="server"></asp:Literal>
</head>
<body>
    <form id="form1" runat="server">
        <CTRL:Messages ID="CTRLmessage" runat="server"/>
    </form>
</body>
</html>