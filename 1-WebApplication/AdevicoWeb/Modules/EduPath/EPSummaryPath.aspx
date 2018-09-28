<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EPSummaryPath.aspx.vb" Inherits="Comunita_OnLine.EPSummaryPath" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/EduPath/UC/UC_OrderBy.ascx" TagName="OrderBy" TagPrefix="UC"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>edupath.css?v=201605041410lm" rel="Stylesheet" />
    <script src="../../Jscript/Modules/Common/jquery.treeTable.js" type="text/javascript"></script>
    <link href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" rel="Stylesheet" />
    <script type="text/javascript">
        $(function () {
            var check = $(".viewcommunity input[type='checkbox']").is(":checked");
            if (!check) {
                $(".extrainfo").hide();
            }
            $(".viewcommunity input[type='checkbox']").change(function () {
                if ($(this).is(":checked")) {
                    $(".extrainfo").show();
                } else {
                    $(".extrainfo").hide();
                }
            });

            $(function () {
                $(".table.light.epsummary.grouped").treeTable({ initialState: "expanded", clickableNodeNames: true });
            });
        });
</script>
<style>
    span.btnOrderBy
    {        
        display:inline-block;
        *display:inline;
        zoom:1;
        vertical-align: middle;        
    }
</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVsummary">
        <asp:View runat="server" ID="VIWsummary">
            <div class="epsummary epsummary-user">
                <div class="DivEpButton">                    
                    <asp:HyperLink ID="HYPback" runat="server" Text="**back" CssClass="linkMenu"></asp:HyperLink>
                </div>
                <div class="fieldobject filters">
                    <div class="fieldrow filterorganization" runat="server" id="DIVfilterorganization">                        
                        <asp:Label runat="server" ID="LBfilterOrganizationTitle" CssClass="fieldlabel">**Organization</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLfilterOrganization">
                        </asp:DropDownList>                        
                    </div>
                    <div class="fieldrow filtercommunity">                        
                        <asp:Label runat="server" ID="LBfilterCommunityTitle" CssClass="fieldlabel">**Community</asp:Label>
                        <asp:TextBox runat="server" ID="TXBfilterCommunity"></asp:TextBox>
                    </div>
                    <div class="fieldrow filterpath">                        
                        <asp:Label runat="server" ID="LBfilterPathTitle" CssClass="fieldlabel">**EduPath</asp:Label>
                        <asp:TextBox runat="server" ID="TXBfilterPath"></asp:TextBox>
                    </div>
                    
                    <div class="fieldrow filterstartdate">                        
                        <asp:Label runat="server" ID="LBfilterStartDateTitle" CssClass="fieldlabel">**Start date</asp:Label>
                        <telerik:RadDatePicker ID="RDPStartDate" runat="server">
                            <Calendar ID="Calendar1" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x"
                                runat="server">
                            </Calendar>
                            <DateInput ID="DateInput1" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" Width="" runat="server">
                            </DateInput>
                            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                        </telerik:RadDatePicker>
                    </div>
                    <div class="fieldrow filterenddate">
                        <asp:Label runat="server" ID="LBfilterEndDateTitle" CssClass="fieldlabel">**End date</asp:Label>
                        <telerik:RadDatePicker ID="RDPEndDate" runat="server">
                            <Calendar ID="Calendar2" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x"
                                runat="server">
                            </Calendar>
                            <DateInput ID="DateInput2" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" Width="" runat="server">
                            </DateInput>
                            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                        </telerik:RadDatePicker>
                    </div>
                    <div class="fieldrow filterstatus">                        
                        <asp:Label runat="server" ID="LBfilterStatusTitle" CssClass="fieldlabel">**Status</asp:Label>
                        <asp:RadioButtonList runat="server" ID="RBLfilterStatus">                        
                        </asp:RadioButtonList>
                        <%--<label for="all">
                            *all</label><input id="all" type="radio" name="status" value="all" />
                        <label for="all">
                            *unlocked</label><input id="unlocked" type="radio" name="status" value="unlocked" />
                        <label for="all">
                            *locked</label><input id="locked" type="radio" name="status" value="locked" />--%>
                    </div>
                    <div class="fieldrow filterupdate">                        
                        <asp:Button runat="server" CssClass="Link_Menu" ID="BTNupdate" Text="**Update" />
                    </div>
                </div>
                <div class="fieldobject details">
                    <div class="fieldrow">                        
                        <asp:Label runat="server" CssClass="fieldlabel" ID="LBdetailsUsersTitle">**Users:</asp:Label>                        
                        <span class="status completion">                        
                        <span class="statusitem">
                            <asp:Label runat="server" ID="LBdetailsUserNotStarted" CssClass="gray">**250</asp:Label> 
                            <asp:Label runat="server" ID="LBdetailsUserNotStartedLabel" CssClass="label">**not started</asp:Label>                                                       
                        </span>
                        <span class="statusitem">
                            <asp:Label runat="server" ID="LBdetailsUserStarted" CssClass="yellow">**250</asp:Label>
                            <asp:Label runat="server" ID="LBdetailsUserStartedLabel" CssClass="label">**started</asp:Label>                          
                            
                        </span>
                        <span class="statusitem">
                            <asp:Label runat="server" ID="LBdetailsUserCompleted" CssClass="green">**250</asp:Label>                            
                            <asp:Label runat="server" ID="LBdetailsUserCompletedLabel" CssClass="label">**completed</asp:Label>
                        </span>
                   </span>
                    </div>
                </div>
                <div class="commands">
                    <span class="viewcommunity">
                        <asp:CheckBox runat="server" ID="CHBviewcommunity" Text="**View Community" />
                    </span>
                    <asp:Button runat="server" ID="BTNgrouping" CssClass="linkMenu" Text="**View grouped by Community" />                     
                </div>
                <div class="pager top" runat="server" id="DVpagerTop">
                    <asp:literal ID="LTpageTop" runat="server">**Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
                </div>
                        
                <asp:MultiView runat="server" ID="MLVsummaryUser">
                    <asp:View runat="server" ID="VIWsummaryUserUngrouped">
                        <table class="table light epsummary">
                            <thead>
                                <tr>
                                    <th class="name">
                                        <asp:Label runat="server" ID="LBpathNameHeader">**Edupath Name</asp:Label>     
                                        <UC:OrderBy runat="server" ID="OBpathname" OnOnOrderBy="OrderBy" Column="path" />                                     
                                        <br />
                                        <asp:Label runat="server" ID="LBcommunityNameHeader" CssClass="extrainfo">**Community Name</asp:Label>                                        
                                    </th>
                                    <th class="completion">
                                        <asp:Label runat="server" ID="LBcompletionHeader">**Completion Status</asp:Label>
                                    </th>
                                    <th class="timerange">
                                        <asp:Label runat="server" ID="LBtimerangeHeader">**Time Range</asp:Label>
                                        <UC:OrderBy runat="server" ID="OBtimerange" OnOnOrderBy="OrderBy" Column="timerange" />
                                    </th>
                                    <th class="actions">
                                        <asp:Label runat="server" ID="LBactionsHeader">**Actions</asp:Label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody> 
                        <asp:Repeater runat="server" ID="RPTpaths">
                        <ItemTemplate>    
                            <tr>
                                <td class="name">                                    
                                    <asp:Label runat="server" ID="LBpathName">**path name</asp:Label><br/>
                                    <asp:Label runat="server" CssClass="extrainfo" ID="LBcommunityName">**community name</asp:Label>                                    
                                </td>

                                <td class="status completion">
                                    <asp:Label runat="server" CssClass="gray" ID="LBnotstarted">**0</asp:Label>
            	                    <asp:Label runat="server" CssClass="yellow" ID="LBstarted">**0</asp:Label>
                                    <asp:Label runat="server" CssClass="green" ID="LBcompleted">**0</asp:Label>
                                </td>
                                <td class="timerange"><asp:Label runat="server" ID="LBtimerange" CssClass="daterange"></asp:Label></td>
                                <td class="actions">
                                    <span class="icons">
                                        <asp:HyperLink runat="server" ID="HYPstats" CssClass="icon stats"></asp:HyperLink>                                                    
                                        <asp:HyperLink runat="server" ID="HYPedit" CssClass="icon edit"></asp:HyperLink>                                                    
                                        <asp:HyperLink runat="server" ID="HYPsettings" CssClass="icon settings"></asp:HyperLink>
                                    </span>
                                </td>
                            </tr>                        
                        </ItemTemplate>
                        </asp:Repeater>    
                                  </tbody>
                </table>                                                                
                    </asp:View>
                    <asp:View runat="server" ID="VIWsummaryUserGrouped">
                    <table class="table light epsummary grouped">
                            <thead>
                                <tr>
                                    <th class="name">                                        
                                        <asp:Label runat="server" ID="LBcommunityPathNameHeader">**Community \ Edupath name</asp:Label>
                                        <UC:OrderBy runat="server" ID="OBcommunitypath" OnOnOrderBy="OrderBy" Column="community/path" />
                                    </th>
                                    <th class="completion">
                                        <asp:Label runat="server" ID="LBcompletiongroupedHeader">**Completion Status</asp:Label>
                                    </th>
                                    <th class="timerange">
                                        <asp:Label runat="server" ID="LBtimerangegroupedHeader">**Time Range</asp:Label>
                                        <UC:OrderBy runat="server" ID="OBtimerangegrouped" OnOnOrderBy="OrderBy" Column="timerange" />
                                    </th>
                                    <th class="actions">
                                        <asp:Label runat="server" ID="LBactionsgroupedHeader">**Actions</asp:Label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                        <asp:Repeater runat="server" ID="RPTcommunities">                            
                            
                            <ItemTemplate>
                                <tr class="community" id="cmnt-<%# Container.Dataitem.Key %>">
                                    <td class="name">
                                        <asp:label runat="server" ID="LBcommunityName">**Community name</asp:label>
                                    </td>
                                    <td class="completion">&nbsp;</td>
                                    <td class="timerange">&nbsp;</td>
                                    <td class="actions">
            	                        <span class="icons">
                                            <asp:HyperLink runat="server" ID="HYPpathlist" CssClass="icon pathlist"></asp:HyperLink>
            	                        </span>
                                    </td>
                                </tr>
                                <asp:Repeater runat="server" ID="RPTpaths">                                    
                                    <ItemTemplate>
                                    <tr class="path child-of-cmnt-<%# Container.Dataitem.IdCommunity %>" id="cmnt-<%# Container.Dataitem.IdCommunity %>-path-<%# Container.Dataitem.IdPath %>">
                                        <td class="name">
                                            <asp:Label runat="server" ID="LBpathName"></asp:Label>                                    
                                                <span class="icons">
                                                    <span class="icon <%# Me.StatusLocked(Container.Dataitem.PathLocked) %>_xs"></span>
                                                </span> 
                                        </td>
                                        <td class="status completion">            	                            
                                            <asp:Label runat="server" ID="LBdetailsUserNotStarted" CssClass="gray">**250</asp:Label> 
                                            <asp:Label runat="server" ID="LBdetailsUserStarted" CssClass="yellow">**250</asp:Label>
                                            <asp:Label runat="server" ID="LBdetailsUserCompleted" CssClass="green">**250</asp:Label>                                                       
                                        </td>
                                        <td class="timerange"><asp:Label runat="server" ID="LBtimerange" CssClass="daterange"></asp:Label>  </td>
                                        <td class="actions">
                                            <span class="icons">
                                                <asp:HyperLink runat="server" ID="HYPstats" CssClass="icon stats"></asp:HyperLink>                                                    
                                                <asp:HyperLink runat="server" ID="HYPedit" CssClass="icon edit"></asp:HyperLink>                                                    
                                                <asp:HyperLink runat="server" ID="HYPsettings" CssClass="icon settings"></asp:HyperLink>
                                            </span>
                                        </td>
                                    </tr>                                        
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>

                        </asp:Repeater>                        
                        </tbody>
                        </table>
                    </asp:View>
                </asp:MultiView>
                    
                <div class="pager bottom" runat="server" id="DVpagerBottom">
                <asp:literal ID="LTpageBottom" runat="server">**Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWerror">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
