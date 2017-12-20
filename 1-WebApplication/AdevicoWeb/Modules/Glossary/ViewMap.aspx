<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ViewMap.aspx.vb" Inherits="Comunita_OnLine.ViewGlossaryMap" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ShareState" Src="~/Modules/Glossary/UC/UC_GlossaryShareState.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
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
    <asp:Panel runat="server" CssClass="glossarywrapper view map" ID="PNLmain">
        <div class="glossariescontent">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPglossaryList" runat="server" CssClass="linkMenu">*Lista Glossari</asp:HyperLink>
                <asp:HyperLink ID="HYPRecycleBin" runat="server" CssClass="linkMenu">*Cestino</asp:HyperLink>
                <asp:HyperLink ID="HYPtermAdd" runat="server" CssClass="linkMenu">*Aggiungi Termine</asp:HyperLink>
                <asp:HyperLink ID="HYPmanageShareGlossary" runat="server" CssClass="linkMenu">*Gestisci Condivisione</asp:HyperLink>
            </div>
            <div class="termscontainer container_12 clearfix">

                <div class="asidecontent grid_12">
                    <div class="filters clearfix collapsable collapsed">
                        <div class="sectionheader clearfix">
                            <div class="left">
                                <h3 class="sectiontitle clearfix">
                                    <asp:Literal ID="LTsearchTools_t" runat="server">*Search tools</asp:Literal>
                                    <span class="extrainfo expander">
                                        <asp:Label runat="server" CssClass="on" ID="LBclickExpand_t">*click to expand</asp:Label>
                                        <asp:Label runat="server" CssClass="off" ID="LBclickCollapse_t">*click to collapse</asp:Label>
                                    </span>
                                </h3>
                            </div>
                            <div class="right">
                                <div class="input-group">
                                    <asp:TextBox ID="TXBsearch" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="LNBsearch" runat="server" CssClass="btn btn-default">*Search</asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </div>

                        <div class="filtercontainer hideme container_12 clearfix">
                            <div class="filter grid_6 textselect">
                                <div class="filterinner">
                                    <asp:Label runat="server" CssClass="title" ID="LBlemma_t">*Lemma</asp:Label>
                                    <span class="content">
                                        <asp:DropDownList runat="server" CssClass="input" ID="DDLsearchType" />
                                        <asp:TextBox ID="TXBsearchLemma" runat="server" CssClass="input"></asp:TextBox>
                                    </span>
                                </div>
                            </div>
                            <div class="filter grid_6 text">
                                <div class="filterinner">

                                    <asp:Label runat="server" CssClass="title" ID="LBlemmacontent_t">*Contenuto Lemma</asp:Label>
                                    <span class="content">
                                        <asp:TextBox ID="TXBsearchLemmaContent" runat="server" CssClass="input"></asp:TextBox>
                                    </span>
                                </div>
                            </div>
                            <div class="filter grid_6 select">
                                <div class="filterinner">
                                    <asp:Label runat="server" CssClass="title" ID="LBstatus_t">*Termini</asp:Label>
                                    <span class="content">
                                        <asp:DropDownList runat="server" CssClass="input" ID="DDLsearchVisibility" />
                                    </span>
                                </div>
                            </div>
                            <div class="filter grid_6 buttons">
                                <div class="viewbuttons">
                                    <asp:LinkButton ID="LNBreset" runat="server" CssClass="linkMenu">*Reset</asp:LinkButton>
                                    <asp:LinkButton ID="LNBsearchApply" runat="server" CssClass="linkMenu">*Apply</asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="sectionfooter hideme"></div>

                        <div class="filtercontainer">
                            <span class="btnswitchgroup">
                                <CTRL:AlphabetSelector ID="CTRLalphabetSelector" runat="server" RaiseSelectionEvent="true"  AutoSelectLetter="true"></CTRL:AlphabetSelector>
                            </span>
                        </div>
                    </div>
                </div>


                <div class="glossaryheader grid_8">
                    <h3>
                        <asp:Literal ID="LTglossaryTitle" runat="server"></asp:Literal>
                    </h3>
                </div>
                <div class="viewnaw grid_4">
                    <asp:Label ID="LBchangeViewStyle_t" runat="server" CssClass="description">*Change view style</asp:Label>

                    <span class="viewnav icons">
                        <span title="<%=GetViewAsStackedListText()%>" class="icon stack active">  <asp:Literal ID="LTstack_t" runat="server">*Stack</asp:Literal></span>
                        <asp:HyperLink ID="HYPviewList" runat="server"><span title="<%=GetViewAsListText()%>" class="icon list"> <asp:Literal ID="LTlist_t" runat="server">*List</asp:Literal></span></asp:HyperLink>
                    </span>
                   
                </div>

                <CTRL:ShareState ID="SSshareState" runat="server" RaiseSelectionEvent="true"></CTRL:ShareState>

                <asp:Repeater runat="server" ID="RPTlist">
                    <HeaderTemplate>
                        <div class="maincontent grid_12">
                        <div class="pagedterms">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="page <%# Container.DataItem.CssClass%>">
                            <div class="title">
                                <h2>
                                    <asp:Literal ID="LTletter" runat="server"></asp:Literal>
                                </h2>
                            </div>
                            <asp:Repeater runat="server" ID="RPTterm" OnItemCommand="RPTterm_ItemCommand">
                                <HeaderTemplate>
                                    <div class="termsmap container_12 clearfix">
                                </HeaderTemplate>
                                <ItemTemplate>

                                    <div class="term grid_4 <%# Container.DataItem.CssClass%>">
                                        <div class="termcontent clearfix">
                                            <div class="left">
                                                <h3 class="termtitle">
                                                    <a name="term_<%# Container.DataItem.Id%>"></a>
                                                    <asp:HyperLink runat="server" ID="HYPterm" CssClass="icon edit"></asp:HyperLink>
                                                    <asp:Label ID="LBtermUnpublished" runat="server" CssClass="infotag" Visible="False">*unpublished</asp:Label>
                                                </h3>
                                            </div>
                                            <div class="right">
                                                <span class="icons">
                                                    <asp:HyperLink runat="server" ID="HYPeditTerm" CssClass="icon edit"></asp:HyperLink>
                                                    <asp:LinkButton runat="server" ID="LNBvirtualDeleteTerm" CssClass="icon virtualdelete"></asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <div class="messages">
                <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>

                <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
                    <div class="fieldrow">
                        <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
                    </div>
                </div>
            </div>

        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>