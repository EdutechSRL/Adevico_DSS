<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityNewSubscriptions.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityNewSubscriptions" %>
<asp:MultiView ID="MLVdata" runat="server">
    <asp:View ID="VIWlist" runat="server">
        <asp:Repeater ID="RPTsubscriptions" runat="server">
            <HeaderTemplate>
                <table class="table light fullwidth">
                    <tr>
                        <th>
                            <asp:Label ID="LBcommunityName_t" runat="server">Name</asp:Label>
                        </th>
                        <th class="ColRole">
                            <asp:Label ID="LBcommunityRole_t" runat="server">Role</asp:Label>
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="LBcommunityName" runat="server" Text="<%#Container.DataItem.Name%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Literal ID="LTidCommunity" runat="server" Visible="false" Text="<%#Container.DataItem.Id%>"></asp:Literal>
                        <asp:DropDownList ID="DDLrole" runat="server" CssClass="Testo_Campo"></asp:DropDownList>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    </asp:View>
</asp:MultiView>