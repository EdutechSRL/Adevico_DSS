<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="BulkTagAssignments.aspx.vb" Inherits="Comunita_OnLine.BulkTagAssignments"  %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="FiltersHeader" Src="~/Modules/Common/UC/UC_FiltersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Bulk" Src="~/Modules/Tag/UC/UC_AssignTagsBulkAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Tag/UC/UC_TagsSelectorForCommunity.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>

    <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <link href="<%=GetBaseUrl()%>Graphics/Modules/TileTag/css/TileTag.css?v=201604071200lm" rel="Stylesheet" />
    <script src="<%=GetBaseUrl()%>Jscript/Modules/TileTag/TileTag.js" type="text/javascript"></script>
    <script src="<%=GetBaseUrl()%>Scripts/angular.min.js" type="text/javascript"></script>    


    <CTRL:FiltersHeader id="CTRLfiltersHeader" FilterModuleCode="SRVDSHBOARD" FilterModuleScope="MultipleTagAssignments" runat="server"></CTRL:FiltersHeader>
    <script type="text/javascript" language="javascript">
           $(function () {
               $(".groupedselector .selectoricon, .groupedselector .selectorlabel").click(function () {
                   var $group = $(this).parents(".groupedselector").first();

                   $(".groupedselector").not($group).removeClass("clicked");
                   $group.toggleClass("clicked");

               });

               $(".groupedselector .selectoritem").click(function () {
                   var $group = $(this).parents(".groupedselector").first();
                   $group.removeClass("clicked");

                   $group.find(".selectoritem").removeClass("active");
                   $(this).addClass("active");

                   $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

               });

               $(".groupedselector").mouseout(function () {
                   //$(this).removeClass("clicked");
               });

               //$(".ddbuttonlist.enabled").dropdownButtonList();

               $(".tablewrapper.hasbulk table.table .select input[type='checkbox']").change(function () {



                   var $tablew = $(this).parents(".tablewrapper.hasbulk");
                   var $table = $(this).parents("table.table").first();
                   var $checks = $table.find(".select input[type='checkbox']").filter(":not([disabled])");

                   var n_checks = $checks.size();

                   var n_checks_checked = $checks.filter(":checked").size();

                   if ($(this).parents(".select").first().is("th.select")) {
                       $checks.prop("checked", $(this).is(":checked"));
                   } else {
                       if (n_checks_checked >= n_checks - 1) {

                           $table.find("th.select input[type='checkbox']").prop("checked", true);
                       } else {

                           $table.find("th.select input[type='checkbox']").prop("checked", false);
                       }
                   }

                   n_checks_checked = $checks.filter(":checked").size();

                   if (n_checks_checked > 0) {
                       $tablew.addClass("bulkon").removeClass("bulkoff");
                   } else {
                       $tablew.removeClass("bulkon").addClass("bulkoff");
                   }


               });
           })
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="tagbulk">
        <div class="list container_12 clearfix" ng-app="ngfilters" ng-controller="filtercontroller">
            <div class="DivEpButton">
                <asp:LinkButton ID="LNBapplyTagsTop" runat="server" Visible="false"  CssClass="linkMenu">*Apply tags</asp:LinkButton>
                <asp:HyperLink ID="HYPgoTo_BackPageFromBulkTop" runat="server" Text="*Back to management" CssClass="linkMenu" Visible="false"></asp:HyperLink>
            </div>
            <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
                <div class="message error">
                    <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
                </div>
            </div>
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
            <div class="asidecontent grid_12" id="DVfilters" runat="server">
                <div class="filters clearfix collapsable <%=GetCollapsedCssClass()%>" data-id="cl-filters.MultipleTags"  aria-live="polite">
                    <div class="sectionheader clearfix">
                        <div class="left">
                            <h3 class="sectiontitle clearifx"><asp:Literal ID="LTmultipleTagAssignmentsFiltersTitle" runat="server"></asp:Literal><span class="extrainfo expander" id="SPNexpand" runat="server"><asp:Label ID="LBspanExpandList" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="LBspanCollapseList" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
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
                            <asp:LinkButton ID="LNBapplySearchFilters" runat="server" Text="*Apply" CssClass="linkMenu" CausesValidation="false" ng-click="setFilters()" TabIndex="1"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="maincontent grid_12">
                <div class="communitylist clearfix first">
                    <div class="tablewrapper hasbulk bulkoff">
                        <div class="top bulk">
                            <CTRL:Bulk id="CTRLbulkActionTop" runat="server"></CTRL:Bulk>
                        </div>
                        <table class="table minimal comlist subscription fullwidth">
                            <thead>
                                <tr>
                                    <th class="select"><input class="checkbox" type="checkbox" runat="server" id="CBXselectAll"/></th>
                                    <th class="communityname">
                                        <span><asp:Literal ID="LTthCommunityName" runat="server">*Community</asp:Literal></span>
                                    </th>
                                    <th class="tags"><span><asp:Literal ID="LTthTagToAssign" runat="server">*Tags</asp:Literal></span></th>
                                </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater ID="RPTcommunities" runat="server">
                                <ItemTemplate>
                                <tr class="community">
                                    <td class="select"><input class="checkbox" type="checkbox" id="CBXcommunity" runat="server"/></td>
                                    <td class="communityname">
                                        <a name="community_<%#Container.DataItem.Id%>"></a>
                                        <span class="title"><%#Container.DataItem.Name   %></span>
                                        <asp:Literal ID="LTidCommunity" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                    </td>
                                    <td class="tags">
                                        <div class="fieldobject clearfix">
                                            <div class="fieldrow selector">
                                               <CTRL:Selector ID="CTRLtagsSelector" runat="server" /> 
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr id="TRempty" runat="server" visible="false">
                                        <td colspan="3">
                                            <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No communities</asp:Label>
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                            </tbody>
                        </table>
                        <div class="bottom bulk">
                            <CTRL:Bulk id="CTRLbulkActionBottom" runat="server" visible="false"></CTRL:Bulk>
                        </div>
                    </div>
                    <div class="sectionfooter">
                        <div class="pager" id="DVpager" runat="server" visible="false" >
                            <CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="DivEpButton" id="DVbottomCommands" runat="server" >
                <asp:LinkButton ID="LNBapplyTagsBottom" runat="server" Visible="false"  CssClass="linkMenu">*Apply tags</asp:LinkButton>
                <asp:HyperLink ID="HYPgoTo_BackPageFromBulkBottom" runat="server" Text="*Back to management" CssClass="linkMenu" Visible="false"></asp:HyperLink>
            </div>
        </div>
    </div>
    <asp:Literal ID="LTcssClassCollapsed" runat="server" Visible="false">collapsed</asp:Literal>
</asp:Content>