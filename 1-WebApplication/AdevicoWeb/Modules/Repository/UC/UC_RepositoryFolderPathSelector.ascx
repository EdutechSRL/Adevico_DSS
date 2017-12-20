<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RepositoryFolderPathSelector.ascx.vb"
    Inherits="Comunita_OnLine.UC_RepositoryFolderPathSelector" %>
<div style="text-align: left; width: 100%; font-size: medium;">
    <b>
        <asp:Label ID="LBpath_t" runat="server">Path</asp:Label></b>&nbsp;
    <asp:Repeater ID="RPTpath" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:HyperLink ID="HYPfolder" runat="server"></asp:HyperLink>
            <asp:LinkButton ID="LNBfolder" runat="server" CausesValidation="false" CommandName="folder"/>
        </ItemTemplate>
        <SeparatorTemplate>
             / 
        </SeparatorTemplate>
        <FooterTemplate>

        </FooterTemplate>
    </asp:Repeater>
</div>