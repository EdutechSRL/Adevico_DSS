<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ScormSettings.aspx.vb" Inherits="Comunita_OnLine.ScormSettings" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLheader" Src="~/Modules/Repository/UC_New/UC_RepositoryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsettings" Src="~/Modules/Repository/UC_New/UC_ScormSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLheader runat="server" ID="CTRLheader" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton" id="DVmenu" runat="server" visible="true">
        <asp:HyperLink ID="HYPbackToPreviousUrl" runat="server" CssClass="linkMenu" Visible="false">*Back</asp:HyperLink>
        <asp:Button ID="BTNsaveScormSettings" runat="server" CssClass="linkMenu" Text="*Save" Visible="false"></asp:Button>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWcontent" runat="server">
            <CTRL:CTRLsettings id="CTRLsettings" runat="server" Visible="false"></CTRL:CTRLsettings>
        </asp:View>
        <asp:View ID="VIWempty" runat="server"></asp:View>
    </asp:MultiView>
</asp:Content>