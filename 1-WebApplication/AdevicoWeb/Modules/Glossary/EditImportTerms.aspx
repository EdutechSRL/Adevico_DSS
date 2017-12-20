<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditImportTerms.aspx.vb" Inherits="Comunita_OnLine.EditImportTerms" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Common/UC/UC_GenericWizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityGlossaryTerms" Src="~/Modules/Glossary/UC/UC_CommunityGlossaryTerms.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelectorHeader" Src="~/Modules/Common/UC/UC_ModalCommunitySelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>

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
<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>
    <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
        <div class="fieldrow">
            <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
        </div>
    </div>


    <asp:Panel runat="server" CssClass="contentwrapper glossarysettings edit clearfix persist-area" ID="PNLmain">
        <div class="column left persist-header copyThis">
            <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
        </div>
        <div class="column right resizeThis">
            <div class="rightcontent">
                <div class="header">
                    <div class="fieldrow objectheader">
                        <%--   <h4 class="title">Glossaries import</h4>--%>
                        <%--<span class="description">Donec ullamcorper nulla non metus auctor fringilla. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.</span>--%>
                        <h4 class="title">
                            <asp:Literal runat="server" ID="LTtermImportTitle_t">**Import Term in glossary</asp:Literal>
                        </h4>
                        <asp:Label runat="server" CssClass="description" ID="LBglossaryImportDescription_t">*click to expand</asp:Label>

                    </div>

                    <div class="fieldrow options">
                        <label for="" class="fieldlabel">
                            <asp:Literal runat="server" ID="LTImportOptions">**Import Term in glossary</asp:Literal>
                        </label>
                        <div class="inlinewrapper">
                            <asp:RadioButtonList ID="RBImportOptions" CssClass="option" AutoPostBack="false" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            </asp:RadioButtonList>
                        </div>
                    </div>

                </div>
                <div class="content">
                    <div class="DivEpButton">
                        <asp:LinkButton ID="LNBselectCommunities" runat="server" CssClass="big linkMenu">*Select communities</asp:LinkButton>
                        <asp:LinkButton ID="LNBConfirmImport" runat="server" CssClass="linkMenu" Visible="false">*Confirm Import</asp:LinkButton>
                    </div>
                    <CTRL:CommunityGlossaryTerms ID="CTRCommunityGlossaryTerms" runat="server"/>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="DivEpButton bottom">
        <asp:HyperLink ID="HYPback" runat="server" CssClass="linkMenu">*Back</asp:HyperLink>
    </div>

    <CTRL:CommunitySelector ID="CTRLcommunity" runat="server" Visible="false" SelectionMode="Multiple"></CTRL:CommunitySelector>


    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>