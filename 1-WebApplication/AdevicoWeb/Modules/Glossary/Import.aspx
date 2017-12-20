<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Import.aspx.vb" Inherits="Comunita_OnLine.Import" %>

<%@ Register TagPrefix="CTRL" TagName="ImportCommunityGlossary" Src="~/Modules/Glossary/UC/UC_ImportCommunityGlossary.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelectorHeader" Src="~/Modules/Common/UC/UC_ModalCommunitySelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="LTpageTitle_t" runat="server"></asp:Literal>
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
    <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
    <asp:Panel runat="server" CssClass="glossarywrapper importexport import" ID="PNLmain">
        <div class="fieldobject box">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPback" runat="server" CssClass="big linkMenu">*Back</asp:HyperLink>
                <asp:LinkButton ID="LNBselectCommunities" runat="server" CssClass="big linkMenu">*Select communities</asp:LinkButton>
                <asp:LinkButton ID="LNBConfirmImportGlossary" runat="server" CssClass="linkMenu" Visible="false">*Confirm Import</asp:LinkButton>
            </div>

            <div class="fieldrow objectheader">
                <h4 class="title">
                    <asp:Literal runat="server" ID="LTglossaryImports_s"></asp:Literal>
                </h4>
                <!-- valutare se inserire descrizione -->
                <asp:Label ID="LBglossaryImportsDescription_s" runat="server" CssClass="description"></asp:Label>
                <div class="fieldrow options">
                    <label for="" class="fieldlabel">
                        <asp:Literal runat="server" ID="LTImportGlossaryOptions">**Import Term in glossary</asp:Literal>
                    </label>
                    <div class="inlinewrapper">
                        <asp:RadioButtonList ID="RBImportOptions" CssClass="option" AutoPostBack="false" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <CTRL:ImportCommunityGlossary ID="CTRCommunityGlossaryTerms" runat="server"/>
        </div>
    </asp:Panel>
    <CTRL:CommunitySelector ID="CTRLcommunity" runat="server" Visible="false" SelectionMode="Multiple"></CTRL:CommunitySelector>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>