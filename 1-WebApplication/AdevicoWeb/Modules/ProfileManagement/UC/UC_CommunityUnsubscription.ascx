<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityUnsubscription.ascx.vb" Inherits="Comunita_OnLine.UC_ProfileCommunityUnsubscriptions" %>

<asp:MultiView ID="MLVdata" runat="server">
    <asp:View ID="VIWlist" runat="server">
        <asp:Repeater ID="RPTsubscriptions" runat="server">
            <HeaderTemplate>
                <table border="1" cellpadding="0" cellspacing="0" id="TBfiles" width="100%">
                    <tr class="ROW_header_Small_Center" style="height: 25px;">
                        <td>
                        
                        </td>
                        <td>
                            <asp:Label ID="LBcommunityName_t" runat="server">Name</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LBcommunityRole_t" runat="server">Role</asp:Label>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr >
                    <td>
                        <asp:CheckBox ID="CBXconfirmDelete" runat="server" Checked="true" />
                    </td>
                    <td>
                        <asp:Label ID="LBcommunityName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Literal ID="LTidCommunity" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="LTidSubscriptions" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="LTidPreviousRole" runat="server" Visible="false"></asp:Literal>
                        
                        <asp:Literal ID="LTpath" runat="server" Visible="false"></asp:Literal>
                        <asp:Label ID="LBrole" runat="server"></asp:Label>
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