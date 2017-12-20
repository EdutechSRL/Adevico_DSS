<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
CodeBehind="ImportGlossary.aspx.vb" Inherits="Comunita_OnLine.ImportGlossary" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Glossarygroup" Src="UC/UC_GlossarySelect.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="~/uc/UC_SearchCommunityByService.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="Lit_PageTitle_top" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=GetBaseUrl()%>Graphics/Template/Wizard/css/wizard.css" rel="Stylesheet" type="text/css"/>
    <link href="<%=GetBaseUrl()%>Graphics/Modules/Glossary/css/ImportGlossary.css" rel="Stylesheet" type="text/css"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal ID="Lit_PageTitle" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="height: 20px; text-align: right;">
        <asp:HyperLink ID="HYPmanage" runat="server" Text="Torna alla lista" CssClass="Link_Menu" Visible="true" NavigateUrl="~/Glossary/GroupList"></asp:HyperLink>
    </div>
    <div id="middle-content">
        <div id="data_content">
            <div id="Wizard">
                <div class="wiz_header">
                    <div class="wiz_top_nav">
                        <div class="stepButton">
                            <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false"/>
                            <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true"/>
                            <asp:Button ID="BTNcompleteTop" runat="server" Text="Conferma" Visible="false"/>
                        </div>
                    </div>

                    <div class="wiz_top_info clearfix">
                        <div class="wiz_top_desc clearfix">
                            <h2>
                                <asp:Label ID="LBstepTitle" runat="server" CssClass="Titolo_Campo"></asp:Label>
                            </h2>
                            <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                            <asp:Label ID="Lbl_Error" runat="server" CssClass="Testo_Campo errore"></asp:Label>
                        </div>
                        <div class="wiz_export"></div>
                    </div>
                </div>
                <div class="wiz_content">
                    <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">
                        <asp:View ID="VIWsourceCom" runat="server">
                            <div class="StepData IW_Community">
                                <CTRL:CTRLsearch ID="UC_Community" runat="server"/>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWsourceGlossary" runat="server">
                            <div class="StepData IW_Glossary">
                                <CTRL:Glossarygroup ID="UC_Glossary" runat="server"/>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWconfirm" runat="server">
                            <div class="StepData IW_Confirm">
                                <asp:Label ID="Lbl_SelectedGlossary_t" CssClass="Titolo_campo" runat="server">Selected glossary:</asp:Label>
                                <asp:Repeater ID="Rpt_Summary" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:Label ID="Lbl_selGlo" runat="server" CssClass="Titolo_campo"></asp:Label>
                                            <span class="Testo_Campo">(<asp:Literal ID="lit_NumElement" runat="server">#</asp:Literal>
                                            <asp:Literal ID="lit_NumElement_t" runat="server">#</asp:Literal>)</span>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate></ul></FooterTemplate>
                                </asp:Repeater>
                                <asp:Label ID="Lbl_NoItemSelected" runat="server" CssClass="Testo_Campo no_item">NO ITEM</asp:Label>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWcomplete" runat="server">
                            <div class="StepData IW_complete">
                                <asp:Label ID="Lbl_Complete_pre" runat="server" CssClass="Titolo_campo"></asp:Label>
                                <asp:Label ID="Lbl_Complete_post" runat="server" CssClass="Testo_Campo"></asp:Label>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
                <div class="wiz_bot_nav clearfix">
                    <div class="stepButton">
                        <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false"/>
                        <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true"/>
                        <asp:Button ID="BTNcompleteBottom" runat="server" Text="Conferma" Visible="false"/>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>