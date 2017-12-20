<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Uc_AdvComments.ascx.vb" Inherits="Comunita_OnLine.Uc_AdvComments" %>

<asp:Repeater ID="RPTcomments" runat="server" Visible="false">
    <HeaderTemplate>
    <div class="commentContainer">
        <div class="hideShowMain">
            <span class="hide btn-show-cmmt" title="Mostra commenti">Mostra</span>
            <span class="btn-hide-cmmt" title="Nascondi commenti">Nascondi</span>
        </div>
        <ul class="comment container openclose">
    </HeaderTemplate>
    <ItemTemplate>
        <li class="comment">
            <!-- css value: draft confirmed -->
            <asp:Literal ID="LTstatus" runat="server">
                <span class="icon {0}" title="Stato valutazione:{1}">{1}</span>
            </asp:Literal>
            <asp:Label ID="LBLcomment" runat="server" CssClass="Comment">###</asp:Label>
            <asp:Label ID="LBLdate" runat="server" CssClass="date" ToolTip="Ultima modifica">###</asp:Label><br />
            <asp:Label ID="LBLmember" runat="server" CssClass="member" ToolTip="Nome valutatore">###</asp:Label>
            <asp:Label ID="LBLcriteria" runat="server" CssClass="criteria" ToolTip="Nome criterio">###</asp:Label>
        </li>
    </ItemTemplate>
    <FooterTemplate>
            </ul>
        </div>
    </FooterTemplate>
</asp:Repeater>