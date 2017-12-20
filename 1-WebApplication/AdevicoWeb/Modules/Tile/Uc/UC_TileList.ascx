<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TileList.ascx.vb" Inherits="Comunita_OnLine.UC_TileList" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Filters" Src="~/Modules/Common/UC/UC_Filters.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<div class="list">
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <div ng-app="ngfilters" ng-controller="filtercontroller" aria-live="polite" runat="server" id="DVfilters">
        <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
            <div class="message error">
                <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
            </div>
        </div>
        <div class="filters clearfix collapsable" data-id="cl-tilesfilters">
            <div class="sectionheader clearfix">
                <div class="left">
                    <h3 class="sectiontitle clearifx"><asp:Literal ID="LTsearchTileFiltersTitle" runat="server"></asp:Literal><span class="extrainfo expander" id="SPNexpand" runat="server"><asp:Label ID="LBspanExpandList" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="LBspanCollapseList" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
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
                    <asp:LinkButton ID="LNBapplyTileFilters" runat="server" Text="*Apply" CssClass="Link_Menu" ng-click="setFilters()"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="sectionheader clearfix">
        <div class="left">            
            <div class="fieldobject details" runat="server" id="DVstatistics" visible="false">               
                <div class="fieldrow">
                    <asp:Label ID="LBtileStatistics" CssClass="fieldlabel" runat="server">*Statistics:</asp:Label>
                    <span class="status completion">
                        <span class="statusitem" id="SPNcommunityTypesWithoutTiles" runat="server" visible="false">
                            <asp:Label ID="LBcommunityTypesWithoutTiles" runat="server"></asp:Label>
                            <asp:Label ID="LBcommunityTypesWithoutTiles_t" runat="server" CssClass="label" Text="*community types without tiles"></asp:Label>
                        </span>
                        <span class="statusitem">
                            <asp:Label ID="LBtilesNotTranslated" runat="server"></asp:Label>
                            <asp:Label ID="LBtilesNotTranslated_t" runat="server" CssClass="label" Text="*tiles without selected language"></asp:Label>
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
                                                <asp:LinkButton ID="LNBdisplayLanguage" runat="server" ><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:LinkButton>
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
        <table class="table light tileslist fullwidth">
            <thead>
                <tr>
                    <th class="name">
                        <span><asp:Literal ID="LTthTileName" runat="server">*Name</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderByNameUp" runat="server" cssclass="icon orderUp" CommandName="Name" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByNameDown" runat="server" cssclass="icon orderDown" CommandName="Name" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="type">
                        <span><asp:Literal ID="LTthTileType" runat="server">*Type</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderTileByTypeUp" runat="server" cssclass="icon orderUp" CommandName="Type" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderTileByTypeDown" runat="server" cssclass="icon orderDown" CommandName="Type" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
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
                <asp:Repeater ID="RPTtiles" runat="server">
                    <ItemTemplate>
                        <tr id="tile-<%#Container.DataItem.Id %>" class="tile <%#GetItemCssClass(Container.DataItem)%>">
                            <td class="name">
                                <asp:Label ID="LBtileLanguageInUse" CssClass="templatelanguage" runat="server"></asp:Label>
                                <a name="tile<%#Container.DataItem.Id %>"></a>
                                <span class="text"><%#Container.DataItem.Translation.Title %></span>
                            </td>
                            <td class="type"><asp:Literal ID="LTtileType" runat="server"></asp:Literal></td>
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
                                    <asp:HyperLink ID="HYPviewTile" runat="server" CssClass="icon infoalt" Visible="false"></asp:HyperLink>
                                    <asp:HyperLink ID="HYPeditTile" runat="server" CssClass="icon edit" Visible="false"></asp:HyperLink>
                                    <asp:linkbutton ID="LNBhideTile" runat="server" Visible="false" CssClass="icon hidden" CommandName="hide" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBshowTile" runat="server" Visible="false" CssClass="icon visible" CommandName="show" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBvirtualDeleteTile" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBvirtualUnDeleteTile" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument="<%#Container.DataItem.Id %>"></asp:linkbutton>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td colspan="6">
                                <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No tiles</asp:Label>
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
        <span class="group first last">
            <asp:Literal ID="LTdraftItem" runat="server"></asp:Literal>
        </span>
    </span>
</div>
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

