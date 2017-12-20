<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Search.aspx.vb" Inherits="Comunita_OnLine.PortalDashboardSearch" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBar" Src="~/Modules/DashBoard/UC/UC_DashboardTopBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBarHeader" Src="~/Modules/DashBoard/UC/UC_DashboardTopBarHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLlist" Src="~/Modules/DashBoard/UC/UC_ListView.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="unsubscriptionDialogHeader" Src="~/Modules/DashBoard/UC/UC_UnsubscriptionDialogHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FiltersHeader" Src="~/Modules/Common/UC/UC_FiltersHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>

    <script src="<%=GetBaseUrl()%>Scripts/angular.min.js" type="text/javascript"></script>    
    <CTRL:FiltersHeader id="CTRLfiltersHeader" FilterModuleCode="SRVDSHBOARD" runat="server"></CTRL:FiltersHeader>
 
    <CTRL:CTRLdashboardTopBarHeader runat="server" ID="CTRLtopbarHeader"></CTRL:CTRLdashboardTopBarHeader>
    <CTRL:unsubscriptionDialogHeader id="CTRLunsubscriptionDialogHeader" runat="server"></CTRL:unsubscriptionDialogHeader>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="homepage">
        <CTRL:CTRLdashboardTopBar runat="server" ID="CTRLdashboardTopBar" ShowSurvey="false" ></CTRL:CTRLdashboardTopBar>
        <div class="homecontent">
            <div class="list search container_12 clearfix noticeboardright" ng-app="ngfilters" ng-controller="filtercontroller">
                <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
                    <div class="message error">
                        <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
                    </div>
                </div>
                <div class="asidecontent grid_12" id="DVfilters" runat="server">
                    <div class="filters clearfix collapsable <%=GetCollapsedCssClass()%>" data-id="cl-filters"  aria-live="polite">
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
                    <CTRL:CTRLlist ID="CTRLlistMyCommunities" runat="server" ></CTRL:CTRLlist>
                </div>
            </div>
        </div>
    </div>
    <asp:Literal ID="LTcssClassCollapsed" runat="server" Visible="false">collapsed</asp:Literal>
    <asp:Literal ID="LTcssClassTitle" runat="server" Visible="false">community</asp:Literal>
</asp:Content>