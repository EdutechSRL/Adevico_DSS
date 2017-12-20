<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CommonActionSender.aspx.vb" Inherits="Comunita_OnLine.CommonActionSender" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="SMTimer" runat="server" OnAsyncPostBackError="SMTimer_AsyncPostBackError"></asp:ScriptManager>
        <asp:UpdatePanel ID="UPTempo" runat="server"><Triggers><asp:AsyncPostBackTrigger ControlID="TMSessione" EventName="Tick" /></Triggers></asp:UpdatePanel>
        <asp:Timer ID="TMSessione" runat="server" Enabled="true"></asp:Timer>
    </div>
    </form>
</body>
</html>