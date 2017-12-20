<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EnrollToCommunities.ascx.vb" Inherits="Comunita_OnLine.UC_EnrollToCommunities" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Bulk" Src="~/Modules/Dashboard/UC/UC_EnrollBulkAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EnrollDialog" Src="~/Modules/DashBoard/UC/UC_EnrollDialog.ascx" %>

<div class="list search container_12 clearfix noticeboardright" ng-app="ngfilters" ng-controller="filtercontroller">
    <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
        <div class="message error">
            <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
        </div>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <div class="asidecontent grid_12" id="DVfilters" runat="server">
        <div class="filters clearfix collapsable <%=GetCollapsedCssClass()%>" data-id="cl-subscribefilters" ng-show="filters.length>0"  aria-live="polite">
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
                    <asp:LinkButton ID="LNBapplySearchFilters" runat="server" Text="*Apply" CssClass="linkMenu" ng-click="setFilters()" TabIndex="1"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="maincontent grid_12">
        <div class="communitylist clearfix first" >
            <div class="sectionheader clearfix">
                <div class="left">
                    <div class="icon<%=GetTitleCssClass()%>"><asp:Image ID="IMGtileIcon" runat="server" Visible="false"/></div>
                    <h3 class="sectiontitle clearifx"><asp:Literal ID="LTtitle" runat="server"></asp:Literal><span class="extrainfo expander" id="Span1" runat="server" visible="false"><asp:Label ID="Label1" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="Label2" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
                </div>
                <div class="right ">
                    <div class="groupedselector" id="DVorderBySelector" runat="server" visible="false">
                        <asp:Label ID="LBorderBySelectorDescription" runat="server" CssClass="description">*Sort by: </asp:Label>
                        <div class="selectorgroup">
                            <asp:Label ID="LBorderBySelected" runat="server" CssClass="selectorlabel"></asp:Label>
                            <span class="selectoricon">&nbsp;</span>
                        </div>
                        <div class="selectormenu">
                            <div class="selectorinner">
                                <div class="selectoritems">
                                    <asp:Repeater ID="RPTorderBy" runat="server">
                                        <ItemTemplate>
                                            <div class="selectoritem" id="DVitemOrderBy" runat="server">
                                                <asp:LinkButton ID="LNBorderItemsBy" runat="server"  CssClass="selectorlabel"/>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="tablewrapper hasbulk <%=KeepOpenCssClass %>">
                <div class="top bulk">
                    <CTRL:Bulk id="CTRLbulkActionTop" runat="server"></CTRL:Bulk>
                </div>
                <table class="table minimal comlist subscription fullwidth">
                    <asp:Repeater ID="RPTcommunities" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <th class="select" id="THselect" runat="server"><input class="checkbox" type="checkbox" runat="server" id="CBXselectAll"/></th>
                                <th class="infowarning">&nbsp;</th>
                                <th class="communityname" id="THname" runat="server">
                                    <asp:label ID="LBthname" runat="server" CssClass="text">*Name</asp:label>
                                    <asp:LinkButton ID="LNBorderByNameUp" runat="server" cssclass="icon orderUp" CommandArgument="Name.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderByNameDown" runat="server" cssclass="icon orderDown" CommandArgument="Name.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="owner" id="THowner" runat="server" visible="false">
                                    <asp:label ID="LBthowner" runat="server" CssClass="text">*Owner</asp:label>
                                    <asp:LinkButton ID="LNBorderByResponsibleUp" runat="server" cssclass="icon orderUp" CommandArgument="Responsible.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderByResponsibleDown" runat="server" cssclass="icon orderDown" CommandArgument="Responsible.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="year" id="THyear" runat="server" visible="false">
                                    <asp:label ID="LBthyear" runat="server" CssClass="text">*Year</asp:label>
                                    <asp:LinkButton ID="LNBorderByYearUp" runat="server" cssclass="icon orderUp" CommandArgument="Year.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderByYearDown" runat="server" cssclass="icon orderDown" CommandArgument="Year.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="period" id="THcoursetime" runat="server" visible="false">
                                    <asp:label ID="LBthcoursetime" runat="server" CssClass="text">*Timespan</asp:label>
                                    <asp:LinkButton ID="LNBorderByTimespanUp" runat="server" cssclass="icon orderUp" CommandArgument="TimespanUp.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderByTimespanDown" runat="server" cssclass="icon orderDown" CommandArgument="TimespanUp.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="seats" id="THmaxsubscribers" runat="server" visible="false">
                                    <asp:label ID="LBthmaxsubscribers" runat="server" CssClass="text">*Seats</asp:label>
                                    <asp:LinkButton ID="LNBorderByMaxUsersUp" runat="server" cssclass="icon orderUp" CommandArgument="MaxUsers.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderByMaxUsersDown" runat="server" cssclass="icon orderDown" CommandArgument="MaxUsers.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="coursetype" id="THdegreetype" runat="server" visible="false">
                                    <asp:label ID="LBthdegreetype" runat="server" CssClass="text"></asp:label>
                                    <asp:LinkButton ID="LNBorderByDegreeTypeUp" runat="server" cssclass="icon orderUp" CommandArgument="DegreeType.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderByDegreeTypeDown" runat="server" cssclass="icon orderDown" CommandArgument="DegreeType.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="startdate date" id="THstartsubscriptionon" runat="server" visible="false">
                                    <asp:label ID="LBthstartsubscriptionon" runat="server" CssClass="text">*From</asp:label>
                                    <asp:LinkButton ID="LNBorderBySubscriptionOpenOnUp" runat="server" cssclass="icon orderUp" CommandArgument="SubscriptionOpenOn.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderBySubscriptionOpenOnDown" runat="server" cssclass="icon orderDown" CommandArgument="SubscriptionOpenOn.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="enddate date" id="THendsubscriptionon" runat="server" visible="false">
                                    <asp:label ID="LBthendsubscriptionon" runat="server" CssClass="text">*To</asp:label>
                                    <asp:LinkButton ID="LNBorderBySubscriptionClosedOnUp" runat="server" cssclass="icon orderUp" CommandArgument="SubscriptionClosedOn.True" Visible="false" CommandName="orderby"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBorderBySubscriptionClosedOnDown" runat="server" cssclass="icon orderDown" CommandArgument="SubscriptionClosedOn.False" Visible="false" CommandName="orderby"></asp:LinkButton>
                                </th>
                                <th class="actions" id="THactions" runat="server" visible="false">
                                    <span class="icons">
                                        <span class="icon actions"></span>
                                    </span>
                                </th>
                            </thead>
                            <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="community <%#GetCommunityCssClass(Container.DataItem)%>" title="<%#GetCommunityTitle(Container.DataItem)%>">
                                <td class="select" id="TDselect" runat="server" visible="false"><input class="checkbox" type="checkbox" id="CBselect" runat="server"/></td>
                                <td class="infowarning">
                                    <span class="icons">
                                        <asp:HyperLink ID="HYPcommunityInfo" runat="server" CssClass="icon infoalt" Target="_blank"></asp:HyperLink>
                                    </span>
                                </td>
                                <td class="communityname">
                                    <asp:Label ID="LBcommunityName" CssClass="title" Text="<%#Container.DataItem.Community.Name %>" runat="server"></asp:Label>
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
                                    <asp:literal ID="LTidCommunity" runat="server" Visible="false" Text="<%#Container.DataItem.Community.Id %>"></asp:literal>
                                    <asp:literal ID="LTpath" runat="server" Visible="false" Text="<%#Container.DataItem.PrimaryPath%>"></asp:literal>
                                </td>
                                <td class="owner" id="TDowner" runat="server" visible="false">
                                    <%#Container.DataItem.Community.Responsible%>
                                </td>
                                <td class="year" id="TDyear" runat="server" visible="false">
                                    <%#Container.DataItem.Community.Year%>
                                </td>
                                <td class="period" id="TDcoursetime" runat="server" visible="false">
                                    <%#Container.DataItem.Community.CourseTime%>
                                </td>
                                <td class="seats" id="TDmaxsubscribers" runat="server" visible="false">
                                    <asp:label ID="LBmaxsubscribers" runat="server"></asp:label>
                                </td>
                                <td class="coursetype" id="TDdegreetype" runat="server" visible="false">
                                     <%#Container.DataItem.Community.DegreeType%>
                                </td>
                                <td class="startdate date" id="TDstartsubscriptionon" runat="server" visible="false">
                                    <asp:label ID="LBstartDate" runat="server"></asp:label>
                                </td>
                                <td class="enddate date" id="TDendsubscriptionon" runat="server" visible="false">
                                    <asp:label ID="LBendDate" runat="server"></asp:label>
                                </td>
                                <td class="actions" id="TDactions" runat="server" visible="false">
                                    <span class="icons"><asp:LinkButton ID="LNBsubscribe" runat="server" Visible="false" CausesValidation="false" CommandName="enroll"  OnClientClick="return onUpdating();" CssClass="icon subscribe"></asp:LinkButton></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                <tr id="TRempty" runat="server" visible="false">
                                    <td id="TDemptyItems" runat="server" colspan="7">
                                        <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No communities</asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
                <div class="bottom bulk">
                    <CTRL:Bulk id="CTRLbulkActionBottom" runat="server" visible="false"></CTRL:Bulk>
                </div>
            </div>
            <span class="fieldrow legend hor" id="SPNlegend" runat="server" visible="false">
                <asp:Label ID="LBenrollingTableLegend" runat="server" CssClass="fieldlabel">*Legend</asp:Label>
        	    <span class="group first last">
        	        <span class="legenditem" title="<%=Resource.getValue("LBlengendItemEnrollNotAvailable.ToolTip")%>">
        	            <span class="legendicon nosubscribe">&nbsp;</span>
                        <asp:Label ID="LBlengendItemEnrollNotAvailable" runat="server" CssClass="legendtext">*no subscribe</asp:Label>
        	        </span>
                    <span class="legenditem reqseats reqstartdate reqenddate" title="<%=Resource.getValue("LBlengendReason.ToolTip")%>">
        	            <span class="legendicon nosubscribe">&nbsp;</span>
                        <asp:Label ID="LBlengendReason" runat="server" CssClass="legendtext">*reason</asp:Label>
        	        </span>
        	     </span>
            </span>
            <div class="sectionfooter">
                <div class="pager" id="DVpager" runat="server" visible="false" >
                    <CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
    </div>
</div>

<CTRL:EnrollDialog id="CTRLconfirmEnrollToCommunity" runat="server" Visible="false" DisplayDescription="true" RaiseCommandEvents="true" DisplayCommands="true" ></CTRL:EnrollDialog>

<asp:Literal ID="LTcssClassCollapsed" runat="server" Visible="false">collapsed</asp:Literal>
<asp:Literal ID="LTcssClassTitle" runat="server" Visible="false">community</asp:Literal>
<asp:Literal ID="LTcssClassActive" runat="server" Visible="false">active</asp:Literal>
<asp:Literal ID="LTcssClassOrderBy" runat="server" Visible="false">selectoritem</asp:Literal>
<asp:Literal ID="LTorderItemsByTemplate" Visible="false" runat="server"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:Literal>
<asp:Literal ID="LTcssClassDefaultItemClass" runat="server" Visible="false">community</asp:Literal>
<asp:Literal ID="LTcssClassTileIcon" runat="server" Visible="false">comtype_48</asp:Literal>
<asp:Literal ID="LTcssClassCustomTile" runat="server" Visible="false">custom</asp:Literal>
<asp:Literal ID="LTtemplateMessageDetails" runat="server" Visible="false"><ul class="messagedetails">{0}</ul></asp:Literal>
<asp:Literal ID="LTtemplateMessageDetail" runat="server" Visible="false"><li class="messagedetail">{0}</li></asp:Literal>
<asp:Literal ID="LTcssClassEnrollUnavailable" runat="server" Visible="false">nosubscribe</asp:Literal>
<asp:Literal ID="LTcssClassStartDate" runat="server" Visible="false">reqstartdate</asp:Literal>
<asp:Literal ID="LTcssClassEndDate" runat="server" Visible="false">reqenddate</asp:Literal>
<asp:Literal ID="LTcssClassSeats" runat="server" Visible="false">reqseats</asp:Literal>
<asp:Literal ID="LTcssClassConstraints" runat="server" Visible="false">requirements</asp:Literal>
<asp:Literal ID="LTcssClassKeepOpen" runat="server" Visible="false">keepopen</asp:Literal>
<asp:Literal ID="LTcssClassBulkOn" runat="server" Visible="false">bulkon</asp:Literal>
<asp:Literal ID="LTcssClassBulkOff" runat="server" Visible="false">bulkoff</asp:Literal>