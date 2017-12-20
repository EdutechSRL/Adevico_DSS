<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityNodes.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityNodes" %>
<%@ Register TagPrefix="CTRL" TagName="Node" Src="~/Modules/DashBoard/UC/UC_CommunityNode.ascx" %>
<asp:Repeater ID="RPTchildren" runat="server" OnItemDataBound="RPTchildren_ItemDataBound">
    <HeaderTemplate>
        <ul class="nestedtree communities">
    </HeaderTemplate>
    <ItemTemplate>
        <CTRL:node id="CTRLnode" runat="server"  OnEnrollTo="CTRLmenu_EnrollTo" OnAccessTo="CTRLmenu_AccessTo" OnUnsubscribeFrom="CTRLmenu_UnsubscribeFrom"></CTRL:node>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
<asp:Literal ID="LTtreeKeepAutoOpenCssClass" runat="server" Visible="false">autoOpen</asp:Literal>
<asp:Literal ID="LTtreeCommunityCssClass" runat="server" Visible="false">community</asp:Literal>
<asp:Literal ID="LTtreeVirtualCssClass" runat="server" Visible="false">virtual</asp:Literal>