<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TemplateAssociation.ascx.vb" Inherits="Comunita_OnLine.UC_TemplateAssociation" %>
<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>

<asp:Literal ID="LTidentifier" runat="server"></asp:Literal>
<span>
    <span>
        <asp:Label ID="LBtemplateTitle_t" runat="server" CssClass="Titolo_Campo"></asp:Label>
    </span>
    <span class="DDLContainer">
        <asp:ExtendedDropDown ID="DDLtemplates" runat="server" CssClass="Testo_Campo" AutoPostBack="true">
        </asp:ExtendedDropDown>
    </span>
    <span>
        <asp:HyperLink id="HYPpreview" runat="server" Visible="false" CssClass="linkMenu" Target="_blank"></asp:HyperLink>
    </span>
</span>
<br />