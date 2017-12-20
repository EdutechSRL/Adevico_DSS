<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DashboardViews.ascx.vb" Inherits="Comunita_OnLine.UC_DashboardViews" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_SwitchClientSide.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Range" Src="~/Modules/Dashboard/Settings/UC/UC_RangeSettings.ascx" %>
<div class="fieldobject box commonsettings first">
    <div class="fieldrow objectheader">
        <h4 class="title">
            <asp:Literal ID="LTsettingsCommonTitle" runat="server" Text="*Common settings"></asp:Literal></h4>
        <div class="fieldrow description">
            <asp:Literal ID="LTsettingsCommonDescription" runat="server">*Common settings for page layout and features availability.</asp:Literal></div>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsFullWidth_t" CssClass="fieldlabel" AssociatedControlID="CBXfullWidth" runat="server">*Fullwidth:</asp:Label>
        <asp:CheckBox ID="CBXfullWidth" runat="server" Checked="true" /><asp:label ID="LBsettingsFullWidth" runat="server" Text="*Yes"></asp:label>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsNoticeboard_t" CssClass="fieldlabel" AssociatedControlID="RBLnoticeboard" runat="server">*Noticeboard:</asp:Label>
        <asp:RadioButtonList ID="RBLnoticeboard" CssClass="inputgroup"  RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" AutoPostBack="true">
        </asp:RadioButtonList>
    </div>
    <asp:MultiView ID="MLVcommonSettings" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstandard" runat="server">
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsOnLoadSettings_t" CssClass="fieldlabel" AssociatedControlID="RBLonLoadSettings" runat="server">*Settings On Load:</asp:Label>
                <asp:RadioButtonList ID="RBLonLoadSettings" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:RadioButtonList>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsDefaultView_t" CssClass="fieldlabel" AssociatedControlID="RBLdefaultView" runat="server">*Default view:</asp:Label>
                <asp:RadioButtonList ID="RBLdefaultView" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:RadioButtonList>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsDisplaySearchItems_t" CssClass="fieldlabel" AssociatedControlID="RBLdisplaySearchItems" runat="server">*Search:</asp:Label>
                <asp:RadioButtonList ID="RBLdisplaySearchItems" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" AutoPostBack="true">
                </asp:RadioButtonList>
            </div>
            <hr />
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsAvailableOrderItemsBy_t" CssClass="fieldlabel" AssociatedControlID="CBLavailableOrderItemsBy" runat="server">*Available Orderby:</asp:Label>
                <asp:CheckBoxList ID="CBLavailableOrderItemsBy" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:CheckBoxList>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsDefaultOrderItemsBy_t" CssClass="fieldlabel" AssociatedControlID="RBLdefaultOrderItemsBy" runat="server">*Default Orderby:</asp:Label>
                <asp:RadioButtonList ID="RBLdefaultOrderItemsBy" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:RadioButtonList>
            </div>
            <hr />
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsAvailableGroupItemsBy_t" CssClass="fieldlabel" AssociatedControlID="CBLavailableGroupItemsBy" runat="server">*Available GroupItemsBy:</asp:Label>
                <asp:CheckBoxList ID="CBLavailableGroupItemsBy" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:CheckBoxList>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsDefaultGroupItemsBy_t" CssClass="fieldlabel" AssociatedControlID="RBLdefaultGroupItemsBy" runat="server">*Default GroupItemsBy:</asp:Label>
                <asp:RadioButtonList ID="RBLdefaultGroupItemsBy" CssClass="inputgroup"  RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:RadioButtonList>
            </div>
        </asp:View>
        <asp:View ID="VIWempty" runat="server"></asp:View>
    </asp:MultiView>
</div>
 <div class="fieldobject box plainviewsettings">
    <div class="fieldrow objectheader">
        <h4 class="title">
            <asp:Literal ID="LTsettingsListTitle" runat="server" Text="*Plain view settings"></asp:Literal></h4>
              
        <CTRL:Switch ID="CTRLlistView" runat="server" DataDisable=".fieldobject.box"/>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsListNoticeboard_t" CssClass="fieldlabel" AssociatedControlID="RBLlistNoticeboard" runat="server">*Noticeboard:</asp:Label>
        <asp:RadioButtonList ID="RBLlistNoticeboard" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" AutoPostBack="true">
        </asp:RadioButtonList>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsListPlainLayout_t" CssClass="fieldlabel" AssociatedControlID="RBLplainLayout" runat="server">*PlainLayout:</asp:Label>
        <asp:RadioButtonList ID="RBLplainLayout" CssClass="inputgroup"  RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
        </asp:RadioButtonList>
    </div>
    <div class="fieldrow fieldlongtext" id="DVorganizationList" runat="server">
        <asp:Label ID="LBsettingsListExpandOrganizationList_t" CssClass="fieldlabel" AssociatedControlID="CBXexpandOrganizationList" runat="server">*ExpandOrganizationList:</asp:Label>
        <asp:CheckBox ID="CBXexpandOrganizationList" CssClass="inputgroup" runat="server" Checked="false" /><asp:Label ID="LBsettingsListExpandOrganizationList" runat="server">*Yes</asp:Label>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsListMaxItems_t" CssClass="fieldlabel" AssociatedControlID="TXBlistMaxItems" runat="server">*Display:</asp:Label>
        <span class="inputgroup">
            <asp:TextBox ID="TXBlistMaxItems" runat="server" MaxLength="3">15</asp:TextBox><asp:Label ID="LBsettingsListMaxItems" runat="server">*communities on list</asp:Label>
            <asp:RangeValidator ID="RNVlistMaxItems" runat="server" ControlToValidate="TXBlistMaxItems" Type="Integer" MinimumValue="1" MaximumValue="50" Display="Dynamic" ErrorMessage="1<=x<=50" SetFocusOnError="true"></asp:RangeValidator>
        </span>
    </div>
     <div class="fieldrow fieldlongtext">
        <CTRL:Range ID="CTRLlistRangeSettings" runat="server"></CTRL:Range>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsListMaxMoreItems_t" CssClass="fieldlabel" AssociatedControlID="TXBlistMaxMoreItems" runat="server">*Display:</asp:Label>
        <span class="inputgroup">
            <asp:TextBox ID="TXBlistMaxMoreItems" runat="server" MaxLength="3">25</asp:TextBox><asp:Label ID="LBsettingsListMaxMoreItems" runat="server">*communities when click on "more communities</asp:Label>
            <asp:RangeValidator ID="RNVlistMaxMoreItems" runat="server" ControlToValidate="TXBlistMaxMoreItems" Type="Integer" MinimumValue="1" MaximumValue="100" Display="Dynamic" ErrorMessage="1<=x<=50" SetFocusOnError="true"></asp:RangeValidator>
            <asp:CompareValidator ID="CMVlistMaxMoreItems" runat="server" ControlToCompare="TXBlistMaxItems"  ControlToValidate="TXBlistMaxMoreItems" SetFocusOnError="true" Type="Integer" Operator="GreaterThan" ErrorMessage="*" ></asp:CompareValidator>
        </span>
    </div>
</div>
<asp:MultiView ID="MLVviews" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWportal" runat="server">
        <div class="fieldobject box combinedviewsettings">
            <div class="fieldrow objectheader">
                <h4 class="title">
                    <asp:Literal ID="LTsettingsCombinedTitle" runat="server" Text="*Combined view settings"></asp:Literal></h4>
                <CTRL:Switch ID="CTRLcombinedView" runat="server" DataDisable=".fieldobject.box" />
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsCombinedNoticeboard_t" CssClass="fieldlabel" AssociatedControlID="RBLcombinedNoticeboard" runat="server">*Noticeboard:</asp:Label>
                <asp:RadioButtonList ID="RBLcombinedNoticeboard" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" >
                </asp:RadioButtonList>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsCombinedPlainLayout_t" CssClass="fieldlabel" AssociatedControlID="RBLcombinedPlainLayout" runat="server">*PlainLayout:</asp:Label>
                <asp:RadioButtonList ID="RBLcombinedPlainLayout" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:RadioButtonList>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsCombinedTileLayout_t" CssClass="fieldlabel" AssociatedControlID="RBLcombinedTileLayout" runat="server">*Mini tile:</asp:Label>
                <asp:RadioButtonList ID="RBLcombinedTileLayout" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:RadioButtonList>
            </div>
            <div class="fieldrow fieldlongtext" id="DVcombinedDisplayMoreMode" runat="server" visible="false">
                <asp:Label ID="LBsettingsCombinedDisplayMoreItems_t" class="fieldlabel" AssociatedControlID="RBLcombinedDisplayMoreItems" runat="server">*Default more items as:</asp:Label>
                <asp:RadioButtonList ID="RBLcombinedDisplayMoreItems" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                </asp:RadioButtonList>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBsettingsCombinedAutoUpdateLayout_t" class="fieldlabel" AssociatedControlID="CBXcombinedAutoUpdateLayout" runat="server">*AutoUpdateLayout:</asp:Label>
                <asp:CheckBox ID="CBXcombinedAutoUpdateLayout" runat="server" Checked="true" /><asp:Label ID="LBsettingsCombinedAutoUpdateLayout" runat="server" Text="*Yes"></asp:Label>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBcombinedTileDisplayItems_t" class="fieldlabel" AssociatedControlID="TXBcombinedTileDisplayItems" runat="server">*Mini tiles:</asp:Label>
                <asp:TextBox ID="TXBcombinedTileDisplayItems" runat="server" MaxLength="3">6</asp:TextBox><asp:Label ID="LBcombinedTileDisplayItems" class="fieldlabel" AssociatedControlID="TXBcombinedTileDisplayItems" runat="server">*tile on the page</asp:Label>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBcombinedMaxItems_t" class="fieldlabel" AssociatedControlID="TXBcombinedMaxItems" runat="server">*Display:</asp:Label>
                <asp:TextBox ID="TXBcombinedMaxItems" runat="server" MaxLength="3">10</asp:TextBox><asp:Label ID="LBcombinedMaxItems" runat="server">*communities on list</asp:Label>
            </div>
            <div class="fieldrow fieldlongtext">
                <asp:Label ID="LBcombinedMaxMoreItems_t" class="fieldlabel" AssociatedControlID="TXBcombinedMaxMoreItems" runat="server">*Display:</asp:Label>
                <asp:TextBox ID="TXBcombinedMaxMoreItems" runat="server" MaxLength="3">25</asp:TextBox><asp:Label ID="LBcombinedMaxMoreItems" runat="server">*communities when click on "more communities"</asp:Label>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWcommunity" runat="server" />
    <asp:View ID="VIWcommunities" runat="server" />
</asp:MultiView>
<div class="fieldobject box tileviewsettings <%=GetSettingsCss("tileviewsettings") %>">
    <div class="fieldrow objectheader">
        <h4 class="title">
            <asp:Literal ID="LTsettingsTileTitle" runat="server" Text="*Tile view settings"></asp:Literal></h4>
        <CTRL:Switch ID="CTRLtileView" runat="server" DataDisable=".fieldobject.box" />
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsTileNoticeboard_t" class="fieldlabel" AssociatedControlID="RBLtileNoticeboard" runat="server">*Noticeboard:</asp:Label>
        <asp:RadioButtonList ID="RBLtileNoticeboard" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
        </asp:RadioButtonList>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsTileLayout_t" class="fieldlabel" AssociatedControlID="RBLtileLayout" runat="server">*TileLayout:</asp:Label>
        <asp:RadioButtonList ID="RBLtileLayout" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
        </asp:RadioButtonList>
    </div>
    <div class="fieldrow fieldlongtext" id="DVtileDisplayMoreMode" runat="server" visible="false">
        <asp:Label ID="LBsettingsTileDisplayMoreItems_t" class="fieldlabel" AssociatedControlID="RBLtileDisplayMoreItems" runat="server">*Default more items as:</asp:Label>
        <asp:RadioButtonList ID="RBLtileDisplayMoreItems" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
        </asp:RadioButtonList>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsTileAutoUpdateLayout_t" class="fieldlabel" AssociatedControlID="CBXtileAutoUpdateLayout" runat="server">*AutoUpdateLayout:</asp:Label>
        <asp:CheckBox ID="CBXtileAutoUpdateLayout" runat="server" Checked="true" /><asp:Label ID="LBsettingsTileAutoUpdateLayout" runat="server" Text="*Yes"></asp:Label>
    </div>
        <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsRedirectTileOn_t" class="fieldlabel" AssociatedControlID="RBLredirectTileOn" runat="server">*Click on tile:</asp:Label>
        <asp:RadioButtonList ID="RBLredirectTileOn" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
        </asp:RadioButtonList>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBtileMaxItems_t" class="fieldlabel" AssociatedControlID="TXBtileMaxItems" runat="server">*Display:</asp:Label>
        <asp:TextBox ID="TXBtileMaxItems" runat="server" MaxLength="3">6</asp:TextBox><asp:Label ID="LBtileMaxItems" class="fieldlabel" AssociatedControlID="TXBtileMaxItems" runat="server">*tile</asp:Label>
    </div>
</div>
<div id="DVsearchSettings" runat="server" class="fieldobject box searchviewsettings">
    <div class="fieldrow objectheader">
        <h4 class="title"><asp:Literal ID="LTsettingsSearchTitle" runat="server" Text="*Search view settings"></asp:Literal></h4>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsSearchMaxItems_t" CssClass="fieldlabel" AssociatedControlID="TXBsearchMaxItems" runat="server">*Display:</asp:Label>
        <asp:TextBox ID="TXBsearchMaxItems" runat="server" MaxLength="3">15</asp:TextBox><asp:Label ID="LBsettingsSearchMaxItems" runat="server">*communities on list</asp:Label>
    </div>
    <div class="fieldrow fieldlongtext">
        <CTRL:Range ID="CTRLsearchRangeSettings" runat="server"></CTRL:Range>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsSearchMaxMoreItems_t" CssClass="fieldlabel" AssociatedControlID="TXBsearchMaxMoreItems" runat="server">*Display:</asp:Label>
        <asp:TextBox ID="TXBsearchMaxMoreItems" runat="server" MaxLength="3">25</asp:TextBox><asp:Label ID="LBsettingsSearchMaxMoreItems" runat="server">*communities when click on "more communities</asp:Label>
    </div>
</div>

<div id="DVsubscribeSettings" runat="server" class="fieldobject box searchviewsettings last">
    <div class="fieldrow objectheader">
        <h4 class="title"><asp:Literal ID="LTsettingsSubscribeTitle" runat="server" Text="*Subscribe view settings"></asp:Literal></h4>
    </div>
    <div class="fieldrow fieldlongtext">
        <asp:Label ID="LBsettingsSubscribeMaxItems_t" CssClass="fieldlabel" AssociatedControlID="TXBsubscribeMaxItems" runat="server">*Display:</asp:Label>
        <asp:TextBox ID="TXBsubscribeMaxItems" runat="server" MaxLength="3">20</asp:TextBox><asp:Label ID="LBsettingsSubscribeMaxItems" runat="server">*communities on list</asp:Label>
    </div>
    <div class="fieldrow fieldlongtext">
        <CTRL:Range ID="CTRLsubscribeRangeSettings" runat="server"></CTRL:Range>
    </div>
</div>
<asp:Literal ID="LTcssClassLastSettings" runat="server" Visible="false">last</asp:Literal>