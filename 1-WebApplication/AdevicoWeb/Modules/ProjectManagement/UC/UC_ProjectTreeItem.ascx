<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProjectTreeItem.ascx.vb" Inherits="Comunita_OnLine.UC_ProjectTreeItem" %>
<asp:Literal ID="LTitemContainer" runat="server"><li id="srt-{id}" class="sortableitem"></asp:Literal>
    <span class="text">
        <asp:Label ID="LBtaskName" runat="server" CssClass="name"></asp:Label>
        <span class="details">
            <asp:Label ID="LBtaskDuration" runat="server" CssClass="duration"></asp:Label>
            <asp:Label ID="LBtaskLinks" runat="server" CssClass="links"></asp:Label>
            <asp:Label ID="LBtaskStartDate" runat="server" CssClass="date start">//</asp:Label>
            <asp:Label ID="LBtaskEndDate" runat="server" CssClass="date end">//</asp:Label>
        </span>
    </span>
    <asp:Repeater ID="RPTchildren" runat="server">
        <HeaderTemplate>
            <ul class="children">
        </HeaderTemplate>
        <ItemTemplate>
            <asp:PlaceHolder ID="PLHchild" runat="server"></asp:PlaceHolder>
        </ItemTemplate>
        <FooterTemplate>
            </ul>        
        </FooterTemplate>
    </asp:Repeater>
</li>