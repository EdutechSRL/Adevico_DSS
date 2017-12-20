<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Add.aspx.vb" Inherits="Comunita_OnLine.AddGlossary" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GSetting" Src="~/Modules/Glossary/UC/UC_GlossarySettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Common/UC/UC_GenericWizardSteps.ascx" %>

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
    <asp:Panel runat="server" CssClass="contentwrapper edit clearfix persist-area" ID="PNLmain">
        <div class="vwizard viewsetsettings step1 hasheader">
            <div class="column left persist-header copyThis">
                <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
            </div>
            <div class="column right resizeThis">
                <div class="rightcontent">
                    <CTRL:GSetting runat="server" ID="CTRLglossarySettings"></CTRL:GSetting>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>