<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TemplateAssociation.ascx.vb" Inherits="Comunita_OnLine.UC_TemplateMessageAssociation" %>
<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>

<asp:Literal ID="LTidentifier" runat="server"></asp:Literal>
<span class="templateselection">
    <span id="SPNtitle" runat="server" visible="false">
        <asp:Label ID="LBtemplateTitle_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLtemplates"></asp:Label>
    </span>
    <span class="DDLContainer">
        <asp:ExtendedDropDown ID="DDLtemplates" runat="server"  AutoPostBack="true">
        </asp:ExtendedDropDown>
    </span>
    <span>
        <asp:HyperLink id="HYPpreview" runat="server" Visible="false" CssClass="Link_Menu" Target="_blank"></asp:HyperLink>
    </span>
</span>