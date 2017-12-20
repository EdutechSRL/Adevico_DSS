<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ExportToFileFromCommunity.aspx.vb" Inherits="Comunita_OnLine.ExportToFileFromCommunity" %>

<%@ Register TagPrefix="CTRL" TagName="CommunityGlossaryTerms" Src="~/Modules/Glossary/UC/UC_CommunityGlossaryTerms.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

    <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>

    <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
        <div class="fieldrow">
            <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
        </div>
    </div>

    <asp:Panel runat="server" CssClass="glossarywrapper importexport import" ID="PNLmain">

        <div class="DivEpButton">
            <asp:HyperLink ID="HYPback" runat="server" CssClass="big linkMenu">*Back</asp:HyperLink>
            <asp:LinkButton ID="LNBExportGlossary" runat="server" CssClass="linkMenu" Visible="false">*Export</asp:LinkButton>
        </div>

        <div class="fieldrow objectheader">
            <h4 class="title">
                <asp:Literal runat="server" ID="LTglossaryExportToFile_s"></asp:Literal>
            </h4>
            <!-- valutare se inserire descrizione -->
            <asp:Label ID="LBglossaryExportToFileDescription_s" runat="server" CssClass="description"></asp:Label>
        </div>
        <CTRL:CommunityGlossaryTerms ID="CTRCommunityGlossaryTerms" runat="server"/>
    </asp:Panel>

    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>

</asp:Content>