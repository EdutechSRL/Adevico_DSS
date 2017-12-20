<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SimplePager.ascx.vb" Inherits="Comunita_OnLine.UC_SimplePager" %>


<span id="spanbutton" runat="server">
    <asp:repeater ID="RPTpages" runat="server">
        <ItemTemplate>
            <asp:LinkButton ID="LNBpage" runat="server" CssClass="PagerLink"></asp:LinkButton>
        </ItemTemplate>
    </asp:repeater>
</span>