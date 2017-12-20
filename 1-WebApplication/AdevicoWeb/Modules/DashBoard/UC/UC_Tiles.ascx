<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Tiles.ascx.vb" Inherits="Comunita_OnLine.UC_Tiles" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtile" Src="~/Modules/DashBoard/UC/UC_Tile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLnoticeboard" Src="~/Modules/DashBoard/UC/UC_NoticeboardTile.ascx" %>

<div class="maincontent grid_12">
    <div class="bytag clearfix">
        <ctrl:CTRLnoticeboard id="CTRLnoticeboard" runat="server" Visible="false"></ctrl:CTRLnoticeboard>
        <asp:Repeater ID="RPTtiles" runat="server">
            <ItemTemplate>
                <CTRL:CTRLtile id="CTRLtile" runat="server"></CTRL:CTRLtile>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="viewmore grid_12" id="DVviewMoreOrLess" runat="server" visible="false">
        <div class="innerwrapper">
            <asp:LinkButton ID="LNBviewAll" runat="server" CssClass="linkMenu" Visible="false" >*View all</asp:LinkButton>
            <asp:LinkButton ID="LNBviewLess" runat="server" CssClass="linkMenu" Visible="false">*View less</asp:LinkButton>
        </div>
    </div>
    <div class="clearfix"></div>
</div>
<div class="asidecontent grid_12">&nbsp;</div>