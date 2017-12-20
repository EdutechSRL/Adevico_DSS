<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TextAction.ascx.vb" Inherits="Comunita_OnLine.UC_TextAction" %>

<asp:MultiView ID="MLVcontrol" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
        <span class="objectRender <%= ContainerCSS %>">
            <asp:Label ID="LBempty" runat="server"></asp:Label>
        </span>
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <span class="objectRender <%= ContainerCSS %>">
            <asp:Literal ID="LTidentifier" runat="server"></asp:Literal>
            <span class="leftDetail" id="SPNdetail" runat="server">
                <asp:Label ID="LBplace0" runat="server" CssClass="plh plh0" Visible="false"></asp:Label>
                <span class="itemTitle" runat="server" id="SPNitemTitle">
                    <span class="textAction">&nbsp;</span>
                    <asp:Label ID="LBtextAction" CssClass="wrapper renderedtext" runat="server"></asp:Label>
                </span>
                <asp:Label ID="LBplace1" runat="server" CssClass="plh plh1" Visible="false"></asp:Label>
                <asp:linkButton ID="LNBexecute"  CssClass="linkMenu taskDone" runat="server" Visible="false"></asp:linkButton>
                <asp:Label ID="LBplace2" runat="server" CssClass="plh plh2" Visible="false"></asp:Label>
            </span>
            <asp:Label ID="LBplace3" runat="server" CssClass="plh plh3" Visible="false"></asp:Label>
            <asp:Repeater ID="RPTactions" runat="server">
                <HeaderTemplate>
                    <span class="itemActions">
                </HeaderTemplate>
                <ItemTemplate>
                    <span class="action">
                        <asp:HyperLink ID="HYPaction" runat="server">&nbsp;</asp:HyperLink>
                    </span>
                </ItemTemplate>
                <FooterTemplate>
                    </span>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Label ID="LBplace4" runat="server" CssClass="plh plh4" Visible="false"></asp:Label>
        </span>
    </asp:View>
</asp:MultiView>