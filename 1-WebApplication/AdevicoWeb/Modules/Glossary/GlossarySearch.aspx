<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="GlossarySearch.aspx.vb" Inherits="Comunita_OnLine.GlossarySearch" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/uc/UC_PagerControl.ascx" %>
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
<asp:Panel runat="server" CssClass="glossarywrapper view" ID="PNLmain">
<div class="glossariescontent">
<div class="DivEpButton">
    <asp:HyperLink ID="HYPglossaryList" runat="server" CssClass="linkMenu">*Lista Glossari</asp:HyperLink>
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
                                <CTRL:AlphabetSelector ID="CTRLalphabetSelector" runat="server" RaiseSelectionEvent="true"></CTRL:AlphabetSelector>
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
        <span class="description">*Change view style</span>
        <span class="viewnav icons">
                        <asp:HyperLink ID="HYPviewMapList" runat="server"><span title="View as stacked list" class="icon stack">*List</span></asp:HyperLink>
                        <span title="View as list" class="icon list active">List</span>
                    </span>
    </div>

    <CTRL:ShareState ID="SSshareState" runat="server" RaiseSelectionEvent="true"></CTRL:ShareState>

    <div class="maincontent grid_12">
        <div class="pagedterms">
            <asp:Repeater runat="server" ID="RPTlist">
                <ItemTemplate>
                    <div class="page first">
                        <div class="title">
                            <h2>
                                <asp:Literal ID="LTletter" runat="server"></asp:Literal>
                            </h2>
                            <asp:Panel CssClass="fieldrow more top" ID="PNLLetterTop" Visible="false" runat="server">
                                <a href="" class="">
                                    <asp:Literal ID="LTmoreTerms" runat="server">* More terms beginning with </asp:Literal>
                                    <span class="letter">
                                                    <asp:Literal ID="LTletterTop" runat="server">*</asp:Literal>
                                                </span>
                                    <asp:Literal ID="LTonPreviousPageTop" runat="server">* on previous page</asp:Literal>
                                </a>
                            </asp:Panel>
                        </div>
                        <asp:Repeater runat="server" ID="RPTterm" OnItemCommand="RPTterm_ItemCommand">
                            <HeaderTemplate>
                                <dl class="terms">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <dt id="term-<%# Container.DataItem.Id%>" class="term <%# GetTileClass(Container.DataItem.Id)%> collapsed">
                                    <div class="termcontent clearfix">
                                        <div class="left">
                                            <span class="handle expander collapsed"></span>
                                            <h3 class="termtitle">
                                                <asp:Literal ID="LTterm" runat="server"></asp:Literal>
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
                                </dt>
                                <dd id="definition-<%# Container.DataItem.Id%>" class="definition first collapsed">
                                    <div class="definitioncontent">
                                        <div class="renderedtext">
                                            <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
                                        </div>
                                        <div class="footer clearfix">
                                            <div class="left">
                                                <div class="attachments">
                                                    <h4 class="title">Attachments:</h4>
                                                    <ol class="attachmentslist">
                                                    </ol>
                                                </div>
                                            </div>
                                            <div class="right">
                                                <span class="details">
                                                                <span class="separator">-</span>
                                                                <span class="author">
                                                                    <asp:Literal ID="LTauthor" runat="server"></asp:Literal>
                                                                </span>
                                                                <span class="lastupdate">
                                                                    <asp:Literal ID="LTlastUpdate" runat="server"></asp:Literal>
                                                                </span>
                                                            </span>
                                            </div>
                                        </div>
                                    </div>
                                </dd>
                            </ItemTemplate>
                            <FooterTemplate>
                                </dl>
                                <asp:Panel CssClass="fieldrow more bottom" ID="PNLLetterBottom" Visible="True" runat="server">
                                    <a href="" class="">
                                        <asp:Literal ID="LTmoreTermsBottom" runat="server">* More terms beginning with </asp:Literal>
                                        <span class="letter">
                                            <asp:Literal ID="LTletterBottom" runat="server">*</asp:Literal>
                                        </span>
                                        <asp:Literal ID="LTonNextPageBottom" runat="server">
                                            * on next page
                                        </asp:Literal>
                                    </a>
                                </asp:Panel>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <%-- <asp:Panel CssClass="fieldrow more bottom" ID="PNLLetterBottom" Visible="True" runat="server">
                                     <a href="" class="">
                                         <asp:Literal ID="LTmoreTermsBottom" runat="server">* More terms beginning with </asp:Literal>
                                         <span class="letter">
                                             <asp:Literal ID="LTletterBottom" runat="server">*</asp:Literal>
                                         </span>
                                         <asp:Literal ID="LTonNextPageBottom" runat="server">
                                                on next page
                                         </asp:Literal>
                                     </a>
                                 </asp:Panel>--%>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="pager">
            <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false" Visible="false"></CTRL:GridPager>
        </div>
    </div>

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