<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortalModal.Master" CodeBehind="ViewScormSettingsOnModal.aspx.vb" Inherits="Comunita_OnLine.ScormSettingsOnModal" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLheader" Src="~/Modules/Repository/UC_New/UC_RepositoryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsettings" Src="~/Modules/Repository/UC_New/UC_ScormSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortalModal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLheader runat="server" ID="CTRLheader" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWcontent" runat="server">
            <CTRL:CTRLsettings id="CTRLsettings" runat="server"></CTRL:CTRLsettings>
        </asp:View>
        <asp:View ID="VIWempty" runat="server"></asp:View>
    </asp:MultiView>
</asp:Content>