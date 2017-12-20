<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CategoryTreeItem.ascx.vb" Inherits="Comunita_OnLine.UC_CategoryTreeItem" %>
<%--Nomi Standard: OK--%>

<%--<li id="srt-<%=Id%>" class="sortableitem<% =LIcssClass%>"> NON FUNZIONA!--%>
<%--<asp:Literal ID="LTinit" runat="server"><li id="srt-{id}" class="sortableitem{css}"></asp:Literal>
    <asp:Literal ID="LTdefText" runat="server"><span class="text{css}"></asp:literal>
        <asp:Literal ID="LTicon" runat="server"><span class="icons"><span class="icon {icon}"></span></span></asp:Literal>
        <asp:Literal ID="LTtext" runat="server"></asp:Literal>
    </span>--%>

    <asp:Literal ID="LTitem" runat="server">
<li id="srt-{id}" class="sortableitem{css}">
    <span class="text{css}">{icon}{text}</span>
    </asp:Literal>
    <asp:Literal ID="LTiconTemplate" runat="server" Visible="false">
        <span class="icons"><span class="icon {icon}"></span></span>
    </asp:Literal>

    <ul class="children">
        <asp:Repeater ID="RPTchildren" runat="server">
            <ItemTemplate>
                <asp:PlaceHolder ID="PHChildren" runat="server"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</li>