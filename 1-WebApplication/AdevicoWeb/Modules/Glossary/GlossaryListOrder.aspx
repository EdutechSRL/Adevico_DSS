<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="GlossaryListOrder.aspx.vb" Inherits="Comunita_OnLine.GlossaryListOrder" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/query.collapsableTreeAdv.js"></script>
    <link rel="stylesheet" href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css">
    <script src="<%=GetBaseUrl()%>Jscript/Modules/Glossary/glossary.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Panel runat="server" CssClass="glossarywrapper list reorder" ID="PNLmain">
        <div class="glossariescontent">

            <div class="DivEpButton">
                <asp:LinkButton ID="LNBsave" runat="server" CssClass="linkMenu">*Save</asp:LinkButton>
                <asp:HyperLink ID="HYPback" runat="server" CssClass="linkMenu">*Back</asp:HyperLink>
            </div>

            <div class="messages">
                <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>

                <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
                    <div class="fieldrow">
                        <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false" />
                    </div>
                </div>
            </div>

            <input type="text" class="serialize_output" id="HIFreorderedData" runat="server" style="display: none;" />

            <div class="fieldrow objectheader">
                <%-- <h4 class="title">
                    <asp:Literal ID="LTglossariesSorting" runat="server">*Glossaries sorting</asp:Literal>
                </h4>--%>

                <div class="fieldrow description">
                    <%--   Arrange glossaries order by dragging them up or down, press save to confirm.--%>
                    <asp:Literal ID="LTglossariesSortingInfo" runat="server">*Arrange glossaries order by dragging them up or down, press save to confirm.</asp:Literal>
                </div>
                <!-- valutare se inserire descrizione -->
            </div>

            <asp:Repeater runat="server" ID="RPTlistDefault">
                <HeaderTemplate>
                    <ol class="sortabletree glossaries root default">
                </HeaderTemplate>
                <ItemTemplate>
                    <li id="srt-<%# Container.DataItem.Id%>" class="sortableitem<%# GetGlossaryClass(Container.DataItem.Id)%>">
                        <span class="text">
                            <asp:Literal ID="LTname" runat="server"></asp:Literal>
                            <asp:Label runat="server" CssClass="infotag" ID="LBinfotag">unpublished</asp:Label>
                        </span>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ol>
                </FooterTemplate>
            </asp:Repeater>


            <asp:Repeater runat="server" ID="RPTlist">
                <HeaderTemplate>
                    <ol class="sortabletree glossaries root">
                </HeaderTemplate>
                <ItemTemplate>
                    <li id="srt-<%# Container.DataItem.Id%>" class="sortableitem<%# GetGlossaryClass(Container.DataItem.Id)%>">
                        <span class="text">
                            <asp:Literal ID="LTname" runat="server"></asp:Literal>
                            <asp:Label runat="server" CssClass="infotag" ID="LBinfotag">unpublished</asp:Label>
                        </span>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ol>
                </FooterTemplate>
            </asp:Repeater>

        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>

</asp:Content>
