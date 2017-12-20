<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_LanguageChange.ascx.vb" Inherits="Comunita_OnLine.UC_LanguageChange" %>
<%--Nomi Standard: OK--%>

<asp:Label ID="LBlanguage_t" runat="server" AssociatedControlID="DDLLanguage" CssClass="fieldlabel auto">Lingua: </asp:Label><%--
--%><asp:DropDownList ID="DDLLanguage" runat="server" CssClass="field ddl"></asp:DropDownList>
<span class="commands">
	<asp:LinkButton ID="LNBchangeLang" runat="server" CssClass="command">Modifica lingua</asp:LinkButton>
</span>