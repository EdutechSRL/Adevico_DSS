<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RadioButtonList.ascx.vb" Inherits="Comunita_OnLine.UC_RadioButtonList" %>
<asp:RadioButtonList ID="RBLitems" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="RblClass"></asp:RadioButtonList>

<%--<asp:Literal ID="LITrblClass" runat="server" Visible="false"></asp:Literal>--%>
<asp:Literal ID="LITrblItemClass" runat="server" Visible="false">inlinewrapper</asp:Literal>
<asp:literal ID="LITrblLayout" runat="server" Visible="false">
	{text}
	<div class="description">{description}</div>
</asp:literal>