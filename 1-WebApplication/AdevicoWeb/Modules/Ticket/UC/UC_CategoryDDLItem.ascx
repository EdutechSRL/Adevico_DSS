<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CategoryDDLItem.ascx.vb" Inherits="Comunita_OnLine.UC_CategoryDDLItem" %>
<%--Nomi Standard: OK--%>
<li class="item">
<asp:Literal ID="LTcatItem" runat="server">
    <a data-id="ctg-{CatId}" data-text="{CatName}" class="ddbutton{CatSel}{CatEn}" title="{CatDesc}"><span class="icon">&nbsp;</span>{CatName}</a>
    </asp:Literal>
    <ul class="items">
    <asp:Repeater ID="RPTitems" runat="server" EnableViewState="true">
        <ItemTemplate>
            <%--<asp:PlaceHolder ID="PHItem" runat="server" EnableViewState="true"></asp:PlaceHolder>--%>
            <asp:Panel ID="PNLitem" runat="server" EnableViewState="true"></asp:Panel>
        </ItemTemplate>
    </asp:Repeater>
    </ul>
</li>