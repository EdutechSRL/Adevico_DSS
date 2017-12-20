<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="Preview.aspx.vb" Inherits="Comunita_OnLine.PreviewSettings" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLnoticeboardBlock" Src="~/Modules/DashBoard/UC/UC_NoticeboardBlock.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLlist" Src="~/Modules/DashBoard/UC/UC_ListView.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLminiTile" Src="~/Modules/DashBoard/UC/UC_MiniTileList.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtiles" Src="~/Modules/DashBoard/UC/UC_Tiles.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FiltersHeader" Src="~/Modules/Common/UC/UC_FiltersHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=GetBaseUrl()%>Graphics/Modules/Dashboard/Css/homepage.css?v=201604071200lm" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
<link rel="stylesheet" href="<%=GetBaseUrl()%>Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.css?v=201604071200lm"/>
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Dashboard/dashboard.js"></script>

      <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>

    <script src="<%=GetBaseUrl()%>Scripts/angular.min.js" type="text/javascript"></script>    
    <CTRL:FiltersHeader id="CTRLfiltersHeader" FilterModuleCode="SRVDSHBOARD" runat="server"></CTRL:FiltersHeader>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
<asp:Literal ID="LTtemplateviewTile" Visible="false" runat="server"><span title="{0}" class="icon tile {2}">{1}</span></asp:Literal>
<asp:Literal ID="LTtemplateviewCombined" Visible="false" runat="server"><span title="{0}" class="icon combo {2}">{1}</span></asp:Literal>
<asp:Literal ID="LTtemplateviewList" Visible="false" runat="server"><span title="{0}" class="icon list {2}">{1}</span></asp:Literal>
<asp:Literal ID="LTcssActiveClass" Visible="false" runat="server">active</asp:Literal>
<asp:Literal ID="LTgroupItemsByTemplate" Visible="false" runat="server"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:Literal>
<asp:Literal ID="LTcssClassActive" runat="server" Visible="false">active</asp:Literal>
<asp:Literal ID="LTcssClassGroupBy" runat="server" Visible="false">selectoritem</asp:Literal>
<div class="homepage">
<asp:MultiView ID="MLVdashboard" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWactive" runat="server">
            <div class="toolbar container_12 view clearfix" id="DVtoolbar" runat="server">
                <div class="viewstyle grid_4 alpha">
                    <span class="viewnav icons" id="SPNviewSelector" runat="server">
                        <asp:LinkButton ID="LNBgotoTileView" runat="server"></asp:LinkButton>
                        <asp:LinkButton ID="LNBgotoCombinedView" runat="server"></asp:LinkButton>
                        <asp:LinkButton ID="LNBgotoListView" runat="server"></asp:LinkButton>
                    </span>
                    <asp:Label ID="LBviewSelectorDescription" runat="server" CssClass="description">*Change view style</asp:Label>
                </div>
                <div class="grid_4 viewoptions">
                    <div class="groupedselector" id="DVgroupedSelector" runat="server" visible="false">
                        <asp:Label ID="LBgroupedSelectorDescription" runat="server" CssClass="description">*Group by: </asp:Label>
                        <div class="selectorgroup">
                            <asp:Label ID="LBgroupBySelected" runat="server" CssClass="selectorlabel"></asp:Label>
                            <span class="selectoricon">&nbsp;</span>
                        </div>
                        <div class="selectormenu">
                            <div class="selectorinner">
                                <div class="selectoritems">
                                    <asp:Repeater ID="RPTgroupBy" runat="server">
                                        <ItemTemplate>
                                            <div class="selectoritem" id="DVitemGroupBy" runat="server" >
                                                <asp:LinkButton ID="LNBgroupItemsBy" runat="server"  CssClass="selectorlabel"/>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="search right grid_4 omega">
                    <div class="input-group" id="DVsearch" runat="server">
                        <asp:TextBox ID="TXBsearchByName" runat="server" CssClass="form-control"></asp:TextBox>
                        <span class="input-group-btn">
                            <asp:Button ID="BTNsearchByName" runat="server" CssClass="btn btn-default" Text="*Search" />
                        </span>
                    </div>
                </div>
            </div>
        <asp:MultiView ID="MLVviews" runat="server" ActiveViewIndex="4">
            <asp:View ID="VIWlist" runat="server">
                <div class="homecontent">
                    <div class="list plain container_12 <%=GetNoticeboardPosition()%> clearfix">
                        <ctrl:CTRLnoticeboardBlock id="CTRLnoticeboard" runat="server" IsPreview="True"></ctrl:CTRLnoticeboardBlock>
                        <div class="maincontent grid_<%=GetContentItemColspan()%>">
                            <ctrl:CTRLlist id="CTRLlistMyCommunities" runat="server" IsPreview="True" TitleCssClass="community" Visible="false"></ctrl:CTRLlist>
                            <ctrl:CTRLlist id="CTRLlistMyOrganizations" runat="server" IsPreview="True" AllowAutoUpdateCookie="false" TitleCssClass="organization" IsCollapsable="true" Visible="false" DataIdForCollapse="cl-organizations-preview" UseDefaultStartupItems="true" DefaultStartupItems="5"></ctrl:CTRLlist>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWcombined" runat="server">
                <div class="homecontent">
                    <div class="list combined container_12 <%=GetNoticeboardPosition()%> clearfix">
                        <ctrl:CTRLnoticeboardBlock id="CTRLnoticeboardCombinedBlock" runat="server" IsPreview="True" ></ctrl:CTRLnoticeboardBlock>
                        <div class="maincontent grid_<%=GetContentItemColspan()%>">
                            <ctrl:CTRLminiTile id="CTRLminiTile" runat="server" Visible="false" IsPreview="True" ></ctrl:CTRLminiTile>
                        </div>
                    </div>
                </div>      
            </asp:View>
            <asp:View ID="VIWtile" runat="server">
                <div class="homecontent">
                    <div class="list tiles container_12 <%=GetNoticeboardPosition()%> clearfix">
                        <CTRL:CTRLtiles id="CTRLtiles" runat="server" IsPreview="True" ></CTRL:CTRLtiles>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWsearch" runat="server">
                <div class="list search container_12 clearfix noticeboardright" ng-app="ngfilters" ng-controller="filtercontroller">
                    <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
                        <div class="message error">
                            <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
                        </div>
                    </div>
                    <div class="asidecontent grid_12">
                        <div class="filters clearfix collapsable <%=GetCollapsedCssClass()%>" data-id="cl-filters-searchpreview"  aria-live="polite">
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
                    <div class="maincontent grid_12">
                        <CTRL:CTRLlist ID="CTRLsearchList" runat="server" IsPreview="True"></CTRL:CTRLlist>
                    </div>
                 </div>
            </asp:View>
            <asp:View ID="VIWempty" runat="server">

            </asp:View>
        </asp:MultiView>
    </asp:View>
    <asp:View ID="VIWwrongSettings" runat="server">

    </asp:View>
</asp:MultiView>
</div>
<asp:Literal ID="LTcssClassCollapsed" runat="server" Visible="false">collapsed</asp:Literal>
    
<asp:Literal ID="LTcssClassTitle" runat="server" Visible="false">community</asp:Literal>
</asp:Content>