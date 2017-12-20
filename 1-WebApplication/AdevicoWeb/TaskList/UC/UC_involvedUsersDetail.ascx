<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_involvedUsersDetail.ascx.vb" Inherits="Comunita_OnLine.UC_involvedUsersDetail" %>
<div>
    <asp:Repeater ID="RPTinvolvedUsers" runat="server" >
        <HeaderTemplate>
             <table id="tableMap" border="1" width="880px" cellspacing="0" >
            
                <tr class="ROW_header_Small_Center">
                    <td runat="server" id="TDd">
                        <asp:Label ID="LBuserName" runat="server" Text="*Username"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBroles" runat="server" Text="*Roles"></asp:Label> 
                    </td>
                    <td>
                        <asp:Label ID="LBcompleteness" runat="server" Text="*Completeness"></asp:Label>
                    </td>
                </tr>                        
        </HeaderTemplate>
        
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Literal ID="LTusername" runat="server" Text=""></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="LTroles" runat="server" Text=""></asp:Literal> 
                </td>
                <td>
                    <asp:Literal ID="LTcompleteness" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
        </ItemTemplate>
        
        <AlternatingItemTemplate>
             <tr>
                <td>
                    <asp:Literal ID="LTusername" runat="server" Text=""></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="LTroles" runat="server" Text=""></asp:Literal> 
                </td>
                <td>
                    <asp:Literal ID="LTcompleteness" runat="server" Text=""></asp:Literal>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
