<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TemplateSelector.ascx.vb" Inherits="Comunita_OnLine.UC_TemplateSelector" %>
<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>
<asp:Literal ID="LTidentifier" runat="server"></asp:Literal>
<asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWselector" runat="server">
        <asp:ExtendedDropDown ID="DDLtemplates" runat="server" AutoPostBack="true" CssClass="chzn-select">
        </asp:ExtendedDropDown>
        <span class="icons">
            <asp:HyperLink id="HYPpreview" runat="server" Visible="false" CssClass="icon view" Target="_blank"></asp:HyperLink>
        </span>
    </asp:View>
    <asp:View ID="VIWsessionTimeout" runat="server">
    
    </asp:View>
</asp:MultiView>