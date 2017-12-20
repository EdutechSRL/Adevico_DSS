<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ImportExport.aspx.vb" Inherits="Comunita_OnLine.ImportExport" %>

<%@ Register TagPrefix="CTRL" TagName="CommunityGlossaryTerms" Src="~/Modules/Glossary/UC/UC_CommunityGlossaryTerms.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelectorHeader" Src="~/Modules/Common/UC/UC_ModalCommunitySelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CommunitySelectorHeader ID="CTRLcommunitySelectorHeader" runat="server" ModalTitle="* select communities to share this glossary" Width="940" Height="600" MinHeight="300" MinWidth="700" SelectionMode="Multiple"></CTRL:CommunitySelectorHeader>
    <CTRL:Header ID="CTRLheader" runat="server"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

</asp:Content>