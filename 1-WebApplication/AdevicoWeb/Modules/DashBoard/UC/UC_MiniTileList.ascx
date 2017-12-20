<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MiniTileList.ascx.vb" Inherits="Comunita_OnLine.UC_MiniTileList" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLlist" Src="~/Modules/DashBoard/UC/UC_ListView.ascx" %>

<ctrl:CTRLlist id="CTRLlistMyCommunities" runat="server" Visible="false"></ctrl:CTRLlist>

<div class="othercommunities clearfix" id="DVminiTiles" runat="server" visible="false">
    <div class="sectionheader clearfix">
        <div class="left">
            <h3 class="sectiontitle clearifx"><asp:Literal ID="LTminiTileTitle" runat="server">*Other communities</asp:Literal></h3>
        </div>
        <div class="right"></div>
    </div>
    <div class="tiles minitiles clearfix">
        <div class="combytag clearfix">
            <asp:Repeater ID="RPTminiTiles" runat="server">
                <ItemTemplate>
                    <div id="DVtile" runat="server" class="tile">
                        <asp:Literal ID="LTlinkOpen" runat="server"><a href="{0}" title="{1}"></asp:Literal>
                        <div class="innerwrapper" id="DVtitle" runat="server">
                            <div class="tileheader clearfix">
                			    <div class="left">
                                    <h3><asp:Literal ID="LTtileTitle" runat="server"></asp:Literal></h3>
                                </div>
                                <div class="right"></div>
                            </div>
                            <div class="tilecontent clearfix">
                                <div class="icon" id="DVicon" runat="server"><asp:Image ID="Image1" runat="server" Visible="false" /></div>
                                <div class="icon comtype_48 <%#GetTitleCssClass(Container.DataItem)%>"></div>
                            </div>
                            <div class="tilefooter">
								<div class="left"></div>
								<div class="right"></div>
						    </div>
                        </div>
                        <div class="innerwrapper" id="DVcustom" runat="server" visible="false">
                            <div class="tileheader clearfix">
                                <div class="left">
								    <h3><asp:Literal ID="LTcustomTileTitle" runat="server"></asp:Literal></h3>
								</div>
								<div class="right"></div>
							</div>
							<div class="tilecontent clearfix">
								<div class="icon">
                                    <asp:Image ID="IMGtileIcon" runat="server" />
								</div>
							</div>
                            <div class="tilefooter">
								<div class="left"></div>
								<div class="right"></div>
						    </div>
						</div>
                        <asp:Literal ID="LTlinkClose" runat="server"></a></asp:Literal>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="tile grid_4" id="DVtileViewMore" runat="server" visible="false">

            </div>
            <div class="viewmore grid_12" id="DVviewMore" runat="server">
                <div class="innerwrapper">
                    <asp:LinkButton ID="LNBviewAll" runat="server" CssClass="linkMenu" Visible="false" >*View all</asp:LinkButton>
                    <asp:LinkButton ID="LNBviewLess" runat="server" CssClass="linkMenu" Visible="false">*View less</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
</div>
<asp:Literal ID="LTcssClassDefaultTile" runat="server" Visible="false">tile</asp:Literal>
<asp:Literal ID="LTcssClassCustomTile" runat="server" Visible="false">custom</asp:Literal>
<asp:Literal ID="LTcssClassDefaultItemClass" runat="server" Visible="false">community</asp:Literal>
