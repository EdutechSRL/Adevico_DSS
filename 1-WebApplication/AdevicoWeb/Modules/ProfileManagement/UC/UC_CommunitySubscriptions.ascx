<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunitySubscriptions.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProfileCommunitySubscriptions" %>


<asp:MultiView ID="MLVdata" runat="server">
    <asp:View ID="VIWlist" runat="server">
        <asp:Repeater ID="RPTsubscriptions" runat="server">
            <HeaderTemplate>
                <table border="1" cellpadding="0" cellspacing="0" id="TBfiles" width="100%">
                    <tr class="ROW_header_Small_Center" style="height: 25px;">
                        <th>
                            <asp:Label ID="LBnewSubscription_t" runat="server">is new</asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="LBcommunityName_t" runat="server">Name</asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="LBcommunityRole_t" runat="server">Role</asp:Label>
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class='<%# me.BackGroundItem(Container.itemtype)%>'>
                    <td>
                        <asp:Label ID="LBnewSubscription" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBcommunityName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Literal ID="LTidCommunity" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="LTidSubscriptions" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="LTidPreviousRole" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="LTpath" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="LTmostLikelyPath" runat="server" Visible="false"></asp:Literal>
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