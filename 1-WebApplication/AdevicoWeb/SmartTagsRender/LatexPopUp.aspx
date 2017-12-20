<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Latexpopup.aspx.vb" Inherits="Comunita_OnLine.LatexPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Latex</title>
    <script src="../Style/Latex/Latex.js" language="javascript" type="text/javascript" ></script>
</head>
<body>
     <form id="aspnetForm" runat="server">
    <div>
    Latex:<br />
    <asp:TextBox Width="100%" Height="170px" runat="server" ID="TXBlatex" TextMode="MultiLine" Text="" ReadOnly="true" onfocus="selectAllText(this)" onclick="selectAllText(this)"></asp:TextBox>
    <asp:Image runat="server" ID="IMGlatex" />
    </div>
    </form>
</body>
</html>