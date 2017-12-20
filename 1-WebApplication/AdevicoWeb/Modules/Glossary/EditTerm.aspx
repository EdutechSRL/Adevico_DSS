<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditTerm.aspx.vb" Inherits="Comunita_OnLine.EditTerm" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Editor" Src="~/Modules/Common/Editor/UC_Editor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Common/UC/UC_AttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>


<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"/>
    <CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="glossarywrapper edit">
        <div class="glossariescontent">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPback" runat="server" CssClass="linkMenu">*Indietro</asp:HyperLink>
                <asp:LinkButton ID="LNBsave" runat="server" CssClass="linkMenu">*Salva Termine</asp:LinkButton>
            </div>

            <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>
            <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
                <div class="fieldrow">
                    <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
                </div>
            </div>
            <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLmain">
                <div class="maincontent grid_12">
                    <div class="fieldobject termedit">

                        <div class="fieldrow term">
                            <asp:Label ID="LBtermName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBname">*Termine</asp:Label>
                            <asp:TextBox ID="TXBname" runat="server" CssClass="inputtext big"></asp:TextBox>
                        </div>
                        <div class="fieldrow textarea definition">
                            <asp:Label ID="LBtermDefinition_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLeditorText">*Definizione</asp:Label>
                            <CTRL:Editor ID="CTRLeditorText" runat="server"
                                         ContainerCssClass="textarea" LoaderCssClass="loadercssclass inlinewrapper"
                                         EditorCssClass="editorcssclass" AllAvailableFontnames="false"
                                         MaxHtmlLength="800000"/>
                        </div>
                        <div class="fieldrow status">
                            <asp:Label ID="LBtermStatus_t" runat="server" CssClass="fieldlabel" AssociatedControlID="SWHpublish">*Status</asp:Label>
                            <CTRL:Switch ID="SWHpublish" runat="server" Status="true"/>

                        </div>
                    </div>

                </div>

            </asp:Panel>
            <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
                <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
            </asp:Panel>
        </div>
    </div>

</asp:Content>