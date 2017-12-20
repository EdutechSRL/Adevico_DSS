
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FindCommunitiesByService.ascx.vb"
    Inherits="Comunita_OnLine.UC_FindCommunitiesByService" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>


<%--<script type="text/javascript" language="Javascript">

    function ChangeCommunityCheckBoxState(id, checkState) {
        var cb = document.getElementById(id);
        if (cb != null)
            cb.checked = checkState;
    }

    function ChangeCommunityAllCheckBoxStates(checkState) {
        // Toggles through all of the checkboxes defined in the CheckBoxIDs array
        // and updates their value to the checkState input parameter
        if (CheckCommunityBoxIDs != null) {
            for (var i = 0; i < CheckCommunityBoxIDs.length; i++)
                ChangeCommunityCheckBoxState(CheckCommunityBoxIDs[i], checkState);
        }
    }
</script>--%>

<asp:MultiView ID="MLVsearch" runat="server" ActiveViewIndex="1">
    <asp:View ID="VIWsessionTimeout" runat="server">
        <br /><br /><br />
        <asp:Label ID="LBsessionTimeout" runat="server"></asp:Label>
        <br /><br /><br />
    </asp:View>
    <asp:View ID="VIWlist" runat="server">
        <div>
            <div class="fieldobject filters communities container_12 clearfix" ng-app="ngfilters" ng-controller="filtercontroller">
                <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
                    <div class="message error">
                        <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
                    </div>
                </div>
                <div class="asidecontent grid_12" id="DVfilters" runat="server">
                    <div class="filters clearfix collapsable collapsed" data-id="selector-communities-filters"  aria-live="polite">
                        <div class="sectionheader clearfix">
                            <div class="left">
                                <h3 class="sectiontitle clearifx"><asp:Literal ID="LTsearchFiltersTitle" runat="server"></asp:Literal><span class="extrainfo expander" id="SPNexpand" runat="server"><asp:Label ID="LBspanExpandList" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="LBspanCollapseList" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
                            </div>
                            <div class="right hideme">
                            </div>
                        </div>
                        <div class="hideme">
                            <div class="filtercontainer container_12 clearfix">
                                <div class="filter grid_{{filter.GridSize}} {{filter.Type}} {{filter.CssClass}}" ng-class="{autoupdate: filter.AutoUpdate}" ng-repeat="filter in filters" >
                                    <ng-include src="'<%=GetBaseUrl()%>filters/filter-'+filter.Type+'.html'"> </ng-include>
                                </div>
                            </div>
                        </div>
                        <div class="sectionfooter hideme">
                            <div class="viewbuttons bottom">
                                <asp:LinkButton ID="LNBapplySearchFilters" runat="server" Text="*Apply" CssClass="linkMenu" ng-click="setFilters()"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <!--<div class="maincontent grid_12"></div>-->
            </div>
            <div class="CommunitiesTable">
                <asp:Repeater ID="RPTcommunities" runat="server">
                    <HeaderTemplate>
                        <table class="table light fullwidth">
                            <tr>
                                <th class="check">
                                    <asp:Label ID="LBthcommunitySelect" runat="server">S</asp:Label>
                                </th>
                                <th class="communityname">
                                    <asp:Label ID="LBthcommunityName" runat="server">Name</asp:Label>
                                </th>
                                <th class="communitytype">
                                    <asp:Label ID="LBthcommunityType" runat="server">Type</asp:Label>
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="community">
                            <td class="check">
                                <asp:RadioButton ID="RBselect" runat="server" GroupName="selectsingleradio"  />
                                <asp:CheckBox ID="CBXselect" runat="server" />
                            </td>
                            <td class="communityname">
                                <asp:literal ID="LTidCommunity" runat="server" Text="<%#Container.DataItem.Community.Id%>" Visible="false"></asp:literal>
                                <asp:Label ID="LBcommunityName" runat="server" Text="<%#Container.DataItem.Community.Name%>"></asp:Label>
                                 <div class="extrainfo" id="DVtag" runat="server" visible="false">
                                    <asp:Label ID="LBcommunityTagsTitle" CssClass="label" runat="server">*Tags:</asp:Label>
                                    <span class="tags">
                                        <asp:Label ID="LBtagCommunityType" runat="server" CssClass="tag type" Visible="false"></asp:Label>
                                    <asp:Repeater ID="RPTtags" runat="server" DataSource="<%#Container.DataItem.Community.Tags %>">
                                        <ItemTemplate>
                                            <span class="tag"><%#Container.DataItem %></span>
                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                            <span class="sep">|</span>
                                        </SeparatorTemplate>
                                    </asp:Repeater>
                                    </span>
                                </div>
                            </td>
                            <td class="communitytype">
                                <asp:Label ID="LBcommunityType" runat="server" Text="<%#Container.DataItem.Community.CommunityType%>"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            <tr id="TRempty" runat="server" visible="false">
                                <td id="TDemptyItems" runat="server" colspan="3">
                                    <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No communities</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="sectionfooter" id="DVfooter" runat="server">
                <div class="pager clearfix">
                    <div class="left">
                        <asp:Label ID="LBpagesize_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label>&nbsp;
                        <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="25" Selected="True">25</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="right">
                        <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false"
                            Visible="false"></CTRL:GridPager>
                    </div>
                 </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>