<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TagsList.ascx.vb" Inherits="Comunita_OnLine.UC_TagsList" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register Src="~/Modules/Tag/UC/UC_TagEdit.ascx" TagName="Edit" TagPrefix="CTRL" %>

<%@ Register TagPrefix="CTRL" TagName="Filters" Src="~/Modules/Common/UC/UC_Filters.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<div class="list">
    <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
        <div class="message error">
            <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
        </div>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <div ng-controller="filtercontroller" aria-live="polite">
        <div class="messages hidebeforeloading" ng-show="errorMessage!='' && errorDialog==false">
            <div class="message error">
                <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
            </div>
        </div>
        <div class="filters clearfix collapsable" data-id="cl-tagfilters" runat="server" id="DVfilters">
            <div class="sectionheader clearfix">
                <div class="left">
                    <h3 class="sectiontitle clearifx"><asp:Literal ID="LTsearchTagFiltersTitle" runat="server"></asp:Literal><span class="extrainfo expander" id="SPNexpand" runat="server"><asp:Label ID="LBspanExpandList" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="LBspanCollapseList" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
                </div>
                <div class="right hideme">
                </div>
            </div>
            <div class="hideme">
                <div class="filtercontainer container_12 clearfix">
                    <CTRL:Filters id="CTRLfilters" runat="server"></CTRL:Filters>
                </div>
            </div>
            <div class="sectionfooter hideme">
                <div class="viewbuttons bottom">
                    <asp:Button ID="BTNreloadItems" runat="server" Text="*Reload items" CssClass="hiddensubmit" />
                    <asp:LinkButton ID="LNBapplyTagsFilters" runat="server" Text="*Apply" CssClass="linkMenu" ng-click="setFilters()"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="sectionheader clearfix">
        <div class="left">            
            <div class="fieldobject details" runat="server" id="DVstatistics" visible="false">               
                <div class="fieldrow">
                    <asp:Label ID="LBtagsStatistics" CssClass="fieldlabel" runat="server">*Statistics:</asp:Label>
                    <span class="status completion">
                        <span class="statusitem">
                            <asp:Label ID="LBcommunitiesWithNoTags" runat="server"></asp:Label>
                            <asp:Label ID="LBcommunitiesWithNoTags_t" runat="server" CssClass="label" Text="*communities without tags"></asp:Label>
                            <asp:HyperLink ID="HYPcommunitiesWithNoTags" runat="server" Text="*communities without tags" Visible="false"></asp:HyperLink>
                        </span>
                        <span class="statusitem">
                            <asp:Label ID="LBtagsNotTranslated" runat="server"></asp:Label>
                            <asp:Label ID="LBtagsNotTranslated_t" runat="server" CssClass="label" Text="*tags without selected language"></asp:Label>
                        </span>
                    </span>          
                </div>
            </div>
        </div>
        <div class="right">
            <div class="fieldobject">
                <div class="fieldrow">
                    <div class="groupedselector" id="DVlanguageSelector" runat="server" visible="false">
                        <asp:Label ID="LBdisplayLanguageSelectorDescription" runat="server" CssClass="description">*View translations in: </asp:Label>
                        <div class="selectorgroup">
                            <asp:Label ID="LBdisplayLanguageSelected" runat="server" CssClass="selectorlabel"></asp:Label>
                            <span class="selectoricon">&nbsp;</span>
                        </div>
                        <div class="selectormenu">
                            <div class="selectorinner">
                                <div class="selectoritems">
                                    <asp:Repeater ID="RPTdisplayLanguage" runat="server">
                                        <ItemTemplate>
                                            <div class="selectoritem" id="DVitemDisplayLanguage" runat="server">
                                                <asp:LinkButton ID="LNBdisplayLanguage" runat="server"  CssClass="selectorlabel"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="tablewrapper">
        <table class="table light tagslist fullwidth">
            <thead>
                <tr>
                    <th class="name">
                        <span><asp:Literal ID="LTthName" runat="server">*Name</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderByNameUp" runat="server" cssclass="icon orderUp" CommandName="Name" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByNameDown" runat="server" cssclass="icon orderDown" CommandName="Name" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="usedincommunities">
                        <span><asp:Literal ID="LTthUsedInCommunities" runat="server">*Used by</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderByUsedByUp" runat="server" cssclass="icon orderUp" CommandName="UsedBy" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByUsedByDown" runat="server" cssclass="icon orderDown" CommandName="UsedBy" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="translations"><asp:Literal ID="LTthTranslations" runat="server">*Translations</asp:Literal></th>
                    <th class="createdon">
                        <span><asp:Literal ID="LTthModifiedOn" runat="server">*Modified on</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderByModifiedOnUp" runat="server" cssclass="icon orderUp" CommandName="ModifiedOn" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByModifiedOnDown" runat="server" cssclass="icon orderDown" CommandName="ModifiedOn" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="createdby">
                        <span><asp:Literal ID="LTthModifiedBy" runat="server">*Modified by</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderByModifiedByUp" runat="server" cssclass="icon orderUp" CommandName="ModifiedBy" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByModifiedByDown" runat="server" cssclass="icon orderDown" CommandName="ModifiedBy" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="actions">
                        <span class="icons"><asp:Label ID="LBactions" runat="server" CssClass="icon actions"></asp:Label></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RPTtags" runat="server">
                    <ItemTemplate>
                        <tr id="tag-<%#Container.DataItem.Id %>" class="tag <%#GetItemCssClass(Container.DataItem)%>">
                            <td class="name">
                                <asp:Label ID="LBtagLanguageInUse" CssClass="templatelanguage" runat="server"></asp:Label>
                                <span class="text"><%#Container.DataItem.Translation.Title %></span>
                            </td>
                            <td class="usedincommunities"><%#Container.DataItem.CommunityAssignments %></td>
                            <td class="translations">
                                <span class="languages">
                                    <asp:Repeater ID="RPTlanguages" runat="server" DataSource="<%#Container.DataItem.Translations%>" OnItemDataBound="RPTlanguages_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Label ID="LBtemplateLanguage" runat="server" cssclass="templatelanguage"></asp:Label>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </span>
                            </td>
                            <td class="createdon"><asp:Label ID="LBmodifiedOn" runat="server"></asp:Label></td>
                            <td class="createdby"><%#Container.DataItem.ModifiedBy %></td>
                            <td class="actions">
                                <span class="icons">
                                    <asp:Literal ID="LTviewTag" runat="server" Visible="false" >
                                        <span class="icon infoalt" ng-click="getTag({0},true);" title="{1}"></span>
                                    </asp:Literal>
                                    <asp:Literal ID="LTassignTag" runat="server" Visible="false" >
                                        <span class="icon assign" ng-click="getTagLink({0},{1},false);" title="{2}"></span>
                                    </asp:Literal>
                                     <asp:Literal ID="LTeditTag" runat="server" Visible="false" >
                                        <span class="icon edit" ng-click="getTag({0},false);" title="{1}"></span>
                                    </asp:Literal>
                                    <asp:linkbutton ID="LNBhideTag" runat="server" Visible="false" CssClass="icon hidden" CommandName="hide" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBshowTag" runat="server" Visible="false" CssClass="icon visible" CommandName="show" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBvirtualDeleteTag" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBvirtualUnDeleteTag" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument="<%#Container.DataItem.Id %>"></asp:linkbutton>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td colspan="6">
                                <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No tags</asp:Label>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div class="pager" id="DVpager" runat="server" visible="false" >
        <CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
    </div>
    <span class="fieldrow legend hor" id="DVlegend" runat="server">
        <asp:Label ID="LBtableLegend" runat="server" CssClass="fieldlabel">*Legend</asp:Label>
        <span class="group first">
            <asp:Literal ID="LTdraftItem" runat="server"></asp:Literal>
        </span>
        <span class="group last">
        	<asp:Literal ID="LTdefaultItem" runat="server"></asp:Literal>
        </span>
    </span>
</div>
<CTRL:Edit id="CTRLedit" runat="server"></CTRL:Edit>
<asp:Literal ID="LTcssClassSelectBy" runat="server" Visible="false">selectoritem</asp:Literal>
<asp:Literal ID="LTcssClassActive" runat="server" Visible="false">active</asp:Literal>
<asp:Literal ID="LTcssClassDraft" runat="server" Visible="false">draft</asp:Literal>
<asp:Literal ID="LTcssClassDefault" runat="server" Visible="false">defaulttag</asp:Literal>
<asp:Literal ID="LTcssClassInfoToDo" runat="server" Visible="false">gray</asp:Literal>
<asp:Literal ID="LTcssClassInfoWarning" runat="server" Visible="false">yellow</asp:Literal>
<asp:Literal ID="LTcssClassInfo" runat="server" Visible="false">green</asp:Literal>
<asp:Literal ID="LTtemplateLegendItem" runat="server" Visible="false">
    <span class="legenditem" title="{0}">
    <span class="legendicon {1}">&nbsp;</span>
        <span class="legendtext">{0}</span>
    </span>
</asp:Literal>
<asp:Literal ID="LTtemplateMessageDetails" runat="server" Visible="false"><ul class="messagedetails">{0}</ul></asp:Literal>
<asp:Literal ID="LTtemplateMessageDetail" runat="server" Visible="false"><li class="messagedetail">{0}</li></asp:Literal>