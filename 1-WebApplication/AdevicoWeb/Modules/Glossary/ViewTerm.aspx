<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ViewTerm.aspx.vb" Inherits="Comunita_OnLine.ViewTerm" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>
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
    <asp:Panel runat="server" CssClass="glossarywrapper view viewterm" ID="PNLmain">
        <div class="glossariescontent">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPback" runat="server" CssClass="linkMenu">*Indietro</asp:HyperLink>
                <asp:HyperLink ID="HYPglossaryList" runat="server" CssClass="linkMenu">*GlossaryList</asp:HyperLink>
            </div>
            <div class="termscontainer container_12 clearfix">
                <div class="maincontent grid_12">
                    <dl class="terms">
                        <dt class="term expanded first last">


                            <div class="fieldrow toolbar">
                                <div class="left">
                                    <div class="status">
                                        <CTRL:Switch ID="SWHpublish" runat="server" Status="true"/>
                                        <%--<span class="btnswitchgroup small" data-name="resources" data-rel="" data-table=""><!--
    	
    		                        	    	--><a class="btnswitch on first">Published</a><!--
    	
    		                        	   		--><a class="btnswitch off last active">Not Published</a><!--
    	
    		                        	--></span><!--<span class="desc">This term <span class="value">is# is not# visible to users</span>
    		                        	</span>-->--%>
                                    </div>
                                </div>
                                <div class="right">
                                </div>
                            </div>

                            <div class="termcontent clearfix">
                                <div class="left">
                                    <h3 class="termtitle">
                                        <asp:Literal ID="LTterm" runat="server"></asp:Literal>
                                    </h3>
                                </div>
                                <div class="right">
                                    <span class="icons">
                                        <asp:HyperLink runat="server" ID="HYPedit" CssClass="icon edit"></asp:HyperLink>
                                        <asp:LinkButton runat="server" ID="LNBdelete" CssClass="icon virtualdelete"></asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </dt>
                        <dd id="definition-1" class="definition expanded first last">
                            <div class="definitioncontent">
                                <div class="renderedtext">
                                    <asp:Literal ID="LTdefinition" runat="server"></asp:Literal>
                                </div>
                                <div class="footer clearfix">
                                    <div class="left">
                                        <div class="<%=HasAttachments()%>">
                                            <h4 class="title">Attachments:</h4>
                                        </div>
                                    </div>
                                    <div class="right">
                                        <span class="details">
                                            <span class="separator">-</span>
                                            <span class="author">
                                                <asp:Literal ID="LTAuthor" runat="server"></asp:Literal>
                                            </span>
                                            <span class="lastupdate">
                                                <asp:Literal ID="LTlastUpdate" runat="server"></asp:Literal>
                                            </span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </dd>
                    </dl>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>