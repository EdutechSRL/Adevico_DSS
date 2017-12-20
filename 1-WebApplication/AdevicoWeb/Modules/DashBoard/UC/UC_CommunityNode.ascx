<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityNode.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityNode" %>
<%@ Register TagPrefix="CTRL" TagName="Menu" Src="~/Modules/DashBoard/UC/UC_CommunityTreeMenu.ascx" %>
<div class="header">
    <span class="handle alt">&nbsp;</span>
    <span class="item">
        <span class="selection">
            <input type="checkbox" runat="server" id="CBselect" />
        </span>
        <span class="text">
            <asp:Label ID="LBnodeName" runat="server" CssClass="name" Visible="true"></asp:Label>
            <asp:LinkButton ID="LNBnodeName" runat="server" CssClass="namelink" Visible="false" CommandName="access"><span class="name">{0}</span></asp:LinkButton>
            <CTRL:Menu id="CTRLmenu" runat="server"></CTRL:Menu>
            <span class="details">
                <asp:Literal ID="LTdetails" runat="server"></asp:Literal>
            </span>
        </span>
    </span>
</div>