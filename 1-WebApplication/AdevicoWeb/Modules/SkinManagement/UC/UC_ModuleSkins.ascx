<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleSkins.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleSkins" %>

<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>

<asp:Literal ID="LTidentifier" runat="server"></asp:Literal>
<span class="moduleskinobject clearfix">
    <span class="left">
        <span class="moduleskinlabel">
            <asp:Label ID="LBskinTitle_t" runat="server" CssClass="Titolo_Campo"></asp:Label>
        </span>
        <span class="moduleskinselector">
            <asp:ExtendedDropDown ID="DDLskin" runat="server" CssClass="Testo_Campo" AutoPostBack="true">
            </asp:ExtendedDropDown>
        </span>
    </span>
    <span class="moduleskinicons icons right">
        <asp:HyperLink id="HYPdelete" runat="server" Visible="false" CssClass="icon delete"></asp:HyperLink>
        <asp:HyperLink id="HYPpreview" runat="server" Visible="false" CssClass="icon view" Target="_blank"></asp:HyperLink>
        <asp:HyperLink id="HYPedit" runat="server" Visible="false" CssClass="icon edit"></asp:HyperLink>
        <asp:HyperLink id="HYPadd" runat="server" Visible="false" CssClass="icon addcriteria"></asp:HyperLink>
    </span>
</span>
<br class="moduleskinbr" />