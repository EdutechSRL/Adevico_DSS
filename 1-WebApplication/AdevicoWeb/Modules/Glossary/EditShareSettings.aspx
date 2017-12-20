<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditShareSettings.aspx.vb" Inherits="Comunita_OnLine.EditShareSettings" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Common/UC/UC_GenericWizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelectorHeader" Src="~/Modules/Common/UC/UC_ModalCommunitySelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GShare" Src="~/Modules/Glossary/UC/UC_GlossaryShare.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CommunitySelectorHeader ID="CTRLcommunitySelectorHeader" runat="server" ModalTitle="* select communities to share this glossary" Width="940" Height="600" MinHeight="300" MinWidth="700"  SelectionMode="Multiple"></CTRL:CommunitySelectorHeader>
    <CTRL:Header ID="CTRLheader" runat="server"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Panel runat="server" CssClass="contentwrapper glossarysettings edit clearfix persist-area" ID="PNLmain">
        <div class="column left persist-header copyThis">
            <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
        </div>
        <div class="column right resizeThis">
            <div class="rightcontent">
                <CTRL:GShare runat="server" ID="CTRLglossaryShare"></CTRL:GShare>
            </div>
        </div>
    </asp:Panel>
    <CTRL:CommunitySelector ID="CTRLcommunity" runat="server" Visible="false" SelectionMode="Multiple"></CTRL:CommunitySelector>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>