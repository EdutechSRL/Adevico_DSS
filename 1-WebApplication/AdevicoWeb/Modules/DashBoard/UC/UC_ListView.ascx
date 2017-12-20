<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ListView.ascx.vb" Inherits="Comunita_OnLine.UC_ListView" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="unsubscriptionDialog" Src="~/Modules/DashBoard/UC/UC_UnsubscriptionDialog.ascx" %>
<div class="communitylist clearfix <%=GetContainerCssClass()%>" data-id="<%=GetDataIdForCookie()%>">
    <div class="sectionheader clearfix">
        <div class="left">
            <div class="icon<%=GetTitleCssClass()%>"><asp:Image ID="IMGtileIcon" runat="server" Visible="false"/></div>
            <h3 class="sectiontitle clearifx"><asp:Literal ID="LTtitle" runat="server"></asp:Literal><span class="extrainfo expander" id="SPNexpand" runat="server" visible="false"><asp:Label ID="LBspanExpandList" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="LBspanCollapseList" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
        </div>
        <div class="right <%=GetHideMeCssClass()%>">
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
    <div class="tablewrapper <%=GetHideMeCssClass()%>">
        <table class="table minimal comlist fullwidth <%=GetSearchCssClass()%>">
            <thead>
                <tr>
                    <th class="infowarning">&nbsp;</th>
                    <th class="communityname">&nbsp;
                        <asp:label ID="LBthcommunityName" runat="server" CssClass="text"></asp:label>
                        <span class="sortgroup">
                            <asp:LinkButton ID="LNBorderByNameUp" runat="server" cssclass="icon orderUp" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                            <asp:LinkButton ID="LNBorderByNameDown" runat="server" cssclass="icon orderDown" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        </span>
                    </th>
                    <th class="info" runat="server" id="THgenericDateTime">
                        <asp:label ID="LBthgenericDateTime" runat="server" CssClass="text"></asp:label>
                        <span class="sortgroup">
                            <asp:LinkButton ID="LNBorderByUp" runat="server" cssclass="icon orderUp" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                            <asp:LinkButton ID="LNBorderByDown" runat="server" cssclass="icon orderDown" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        </span>
                    </th>
                    <th class="info status" runat="server" id="THsubscriptionInfo"><span class="text"><asp:Literal ID="LTthcommunityInfo" runat="server">*Info</asp:Literal></span></th>
                    <th class="actions" runat="server" id="THactions" visible="false">
                        <span class="icons">
                            <span class="icon actions"></span>
                        </span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RPTcommunities" runat="server">
                    <ItemTemplate>
                        <tr class="community <%#GetCommunityCssClass(Container.DataItem)%>">
                            <td class="infowarning">
                                <span class="icons">
                                    <asp:HyperLink ID="HYPcommunityInfo" runat="server" CssClass="icon infoalt" Target="_blank"></asp:HyperLink>
                                </span>
                            </td>
                            <td class="communityname">
                                <asp:LinkButton ID="LNBcommunityAccess" runat="server" CausesValidation="false" CssClass="title"></asp:LinkButton>
                                <asp:Label ID="LBcommunityAccess" runat="server" CssClass="title"></asp:Label>
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
                            <td class="info date" runat="server" id="TDgenericDateDime"><asp:Literal ID="LTgenericDateDime" runat="server"></asp:Literal></td>
                            <td class="info status" runat="server" id="TDsubscriptionInfo">
                                <span class="icons">
                                    <asp:HyperLink ID="HYPnews" runat="server" Visible="false"  cssclass="icon hasnews"></asp:Hyperlink>
                                    <asp:Label ID="LBstatus" runat="server" Visible="true" cssclass="icon statuslight"></asp:Label>
                                </span>
                            </td>
                            <td class="actions" runat="server" id="TDactions" visible="false">
                                <span class="icons"><asp:LinkButton ID="LNBvirtualDeleteSubscription" runat="server" Visible="false" CausesValidation="false" CommandName="unsubscribe"  CssClass="icon delete needconfirm-unsubscribefromcommunity"></asp:LinkButton></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td id="TDemptyItems" runat="server" colspan="4">
                                <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No communities</asp:Label>
                            </td>
                        </tr>
                    </FooterTemplate> 
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div class="sectionfooter <%=GetHideMeCssClass()%>">
        <div class="pager" id="DVpager" runat="server" visible="false" >
            <CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
        </div>
        <div class="viewbuttons bottom">
            <asp:LinkButton ID="LNBviewAll" runat="server" CssClass="linkMenu" Visible="false" >*View all</asp:LinkButton>
            <asp:LinkButton ID="LNBviewLess" runat="server" CssClass="linkMenu" Visible="false">*View less</asp:LinkButton>
        </div>
    </div>
</div>
<div class="clearfix"></div>
<CTRL:unsubscriptionDialog id="CTRLconfirmUnsubscription" runat="server" Visible="false" DisplayDescription="true" RaiseCommandEvents="true" DisplayCommands="true" ></CTRL:unsubscriptionDialog>


<asp:Literal ID="LTcssClassStatus" runat="server" Visible="false">icon statuslight</asp:Literal>
<asp:Literal ID="LTcssClassStatusActive" runat="server" Visible="false">green</asp:Literal>
<asp:Literal ID="LTcssClassStatusWaiting" runat="server" Visible="false">yellow</asp:Literal>
<asp:Literal ID="LTcssClassStatusBlocked" runat="server" Visible="false">red</asp:Literal>
<asp:Literal ID="LTcssClassHideme" runat="server" Visible="false">hideme</asp:Literal>
<asp:Literal ID="LTcssClassCollapsable" runat="server" Visible="false">collapsable</asp:Literal>
<asp:Literal ID="LTcssClassActive" runat="server" Visible="false">active</asp:Literal>
<asp:Literal ID="LTcssClassOrderBy" runat="server" Visible="false">selectoritem</asp:Literal>
<asp:Literal ID="LTorderItemsByTemplate" Visible="false" runat="server"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:Literal>
<asp:Literal ID="LTcssClassDefaultItemClass" runat="server" Visible="false">community</asp:Literal>
<asp:Literal ID="LTcssClassTileIcon" runat="server" Visible="false">comtype_48</asp:Literal>
<asp:Literal ID="LTcssClassCustomTile" runat="server" Visible="false">custom</asp:Literal>
<asp:Literal ID="LTcssClassCollapsed" runat="server" Visible="false">collapsed</asp:Literal>
<asp:Literal ID="LTcssClassSearch" runat="server" Visible="false">search</asp:Literal>
<asp:Literal ID="LTtemplateMessageDetails" runat="server" Visible="false"><ul class="messagedetails">{0}</ul></asp:Literal>
<asp:Literal ID="LTtemplateMessageDetail" runat="server" Visible="false"><li class="messagedetail">{0}</li></asp:Literal>
<asp:Literal ID="LTcssClassConstraints" runat="server" Visible="false">requirements</asp:Literal>