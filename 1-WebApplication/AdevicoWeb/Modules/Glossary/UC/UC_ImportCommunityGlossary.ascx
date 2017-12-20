<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ImportCommunityGlossary.ascx.vb" Inherits="Comunita_OnLine.UC_ImportCommunityGlossary" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_SwitchClientSide.ascx" %>
<asp:Repeater runat="server" ID="RPTCommunites">
    <ItemTemplate>
        <ul class="communities defaultservicecontainers clearfix">
            <li class="community defaultservicecontainer default toolbar collapsable">
                <div class="innerwrapper">
                    <div class="itemheader clearfix">
                        <div class="left">
                            <h4 class="title">
                                <span class="handle expander"></span>
                                <asp:Label ID="LBcommunityName" runat="server" CssClass="text"></asp:Label>
                            </h4>
                        </div>
                        <div class="right">
                            <CTRL:Switch ID="SWHcommunity" runat="server" DataDisable=".community.defaultservicecontainer"/>
                        </div>
                        <div class="clearer"></div>
                    </div>
                    <div class="itemcontent">
                        <span class="radiobuttonlist">
                            <asp:Repeater runat="server" ID="RPTglossary" OnItemDataBound="RPTglossary_ItemDataBound">
                                <ItemTemplate>
                                    <span class="item">
                                        <asp:CheckBox runat="server" ID="CBXglossary" />
                                        <asp:Label ID="LBglossary" runat="server" AssociatedControlID="CBXglossary"></asp:Label>
                                    </span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </span>
                        <span class="commands">
                            <asp:Label ID="LBselectAll" runat="server" CssClass="command selectall" >*Select All</asp:Label>
                            <asp:Label ID="LBselectNone" runat="server" CssClass="command selectnone" >*Select None</asp:Label>
                        </span>
                    </div>
                </div>
            </li>
        </ul>
    </ItemTemplate>
</asp:Repeater>