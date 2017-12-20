<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="RecycleBinTerms.aspx.vb" Inherits="Comunita_OnLine.RecycleTerms" %>

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
    <asp:Panel runat="server" CssClass="glossarywrapper list recyclebin" ID="PNLmain">
        <div class="glossariescontent">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPback" runat="server" CssClass="linkMenu">*Indietro</asp:HyperLink>
            </div>
            <div class="tablewrapper">
                <asp:Repeater runat="server" ID="RPTlist">
                    <HeaderTemplate>
                        <table class="table light glossaries fullwidth">
                        <thead>
                        <tr>
                            <th class="name">
                                <asp:Literal ID="LTname_t" runat="server"></asp:Literal>
                            </th>
                            <th class="deletedon">
                                <asp:Literal ID="LTdeleteOn_t" runat="server"></asp:Literal>
                            </th>
                            <th class="deletedfrom">
                                <asp:Literal ID="LTdeleteFrom_t" runat="server"></asp:Literal>
                            </th>
                            <th class="actions">
                                <span class="icons">
                                            <span class="icon actions"></span>
                                        </span>
                            </th>
                        </tr>
                        </thead>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="glossary internal">
                            <td class="name">
                                <asp:Literal ID="LTname" runat="server"></asp:Literal>
                            </td>
                            <td class="deletedon">
                                <asp:Literal ID="LTdeleteOn" runat="server"></asp:Literal>
                            </td>
                            <td class="deletedfrom">
                                <asp:Literal ID="LTdeleteFrom" runat="server"></asp:Literal>
                            </td>
                            <td class="actions">
                                <span class="icons">
                                    <asp:LinkButton runat="server" ID="LNBtermRecover" CssClass="icon recover"></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="LNBtermDelete" CssClass="icon delete"></asp:LinkButton>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td colspan="6">
                                <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No terms</asp:Label>
                            </td>
                        </tr>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>