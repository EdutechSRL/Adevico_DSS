<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleQuizAction.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleQuizAction" %>
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
                    <span class="questionnaire actionIco" runat="server" id="SPNico">&nbsp;</span>
                    <asp:Label ID="LBname" CssClass="wrapper" runat="server"></asp:Label>
                </span>
                <asp:Label ID="LBplace1" runat="server" CssClass="plh plh1" Visible="false"></asp:Label>
                <asp:Label ID="LBscore" CssClass="itemDetails" runat="server" Visible="false"></asp:Label>
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

<asp:Literal ID="LTtemplateUrl" runat="server" Visible="false"><a href="{0}" title="{1}" class="ROW_ItemLink_Small">{2}</a></asp:Literal>