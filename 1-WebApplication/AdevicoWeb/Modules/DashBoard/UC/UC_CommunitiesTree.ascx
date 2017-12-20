<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunitiesTree.ascx.vb" Inherits="Comunita_OnLine.UC_ViewCommunitiesTree" %>
<%@ Register TagPrefix="CTRL" TagName="Node" Src="~/Modules/DashBoard/UC/UC_CommunityNode.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="UnsubscriptionDialog" Src="~/Modules/DashBoard/UC/UC_UnsubscriptionDialog.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Filters" Src="~/Modules/Common/UC/UC_Filters.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EnrollDialog" Src="~/Modules/DashBoard/UC/UC_EnrollDialog.ascx" %>
<CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
<div class="list search container_12 clearfix noticeboardright" ng-app="ngfilters" ng-controller="filtercontroller">
    <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==false">
        <div class="message error">
            <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
        </div>
    </div>
    <div class="asidecontent grid_12"  runat="server" id="DVfilters" visible="false">
        <div class="filters clearfix collapsable <%=GetCollapsedCssClass()%>" data-id="cl-tree-filters-<%=ReferenceIdCommunity()%>-<%=AdvancedMode()%>" ng-show="filters.length>0" aria-live="polite">
            <div class="sectionheader clearfix">
                <div class="left">
                    <h3 class="sectiontitle clearifx"><asp:Literal ID="LTtreeFiltersTitle" runat="server" Text="*Filters"></asp:Literal><span class="extrainfo expander" id="SPNexpand" runat="server"><asp:Label ID="LBspanExpandList" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="LBspanCollapseList" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
                </div>
                <div class="right hideme">
                </div>
            </div>
            <div class="hideme">
                <div class="filtercontainer container_12 clearfix">
                    <CTRL:filters runat="server" ID="CTRLfilters" />
                </div>
            </div>
            <div class="sectionfooter hideme">
                <div class="viewbuttons bottom">
                    <asp:LinkButton ID="LNBapplyTreeFilters" runat="server" Text="*Apply" CssClass="linkMenu" ng-click="setFilters()"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="maincontent grid_12">
        <asp:MultiView ID="MLVtree" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWtree" runat="server">
                <div class="treewrapper">
                    <ul class="nestedtree noselect root communities">
                        <asp:Repeater ID="RPTchildren" runat="server">
                            <ItemTemplate>
                                <asp:MultiView ID="MLVnode" runat="server">
                                    <asp:View ID="VIWtypeOpenCommunityNode" runat="server">
                                        <asp:Literal ID="LTnodeOpenCommunityNode" runat="server"><li class="treenode community {0}" id="{1}"><div class="content"></asp:Literal>
                                    </asp:View>
                                    <asp:View ID="VIWtypeOpenVirtualNode" runat="server">
                                        <asp:Literal ID="LTnodeOpenVirtualNode" runat="server"><li class="treenode virtual {0}" id="{1}"><div class="content"></asp:Literal>
                                    </asp:View>
                                    <asp:View ID="VIWtypeCommunity" runat="server">
                                        <CTRL:node ID="CTRLnode" runat="server" />
                                    </asp:View>
                                    <asp:View ID="VIWtypeNoChildren" runat="server">
                                        <div class="footer"></div>
                                    </asp:View>
                                    <asp:View ID="VIWtypeOpenChildren" runat="server">
                                        <div class="footer">
                                            <ul class="nestedtree  communities">
                                    </asp:View>
                                    <asp:View ID="VIWtypeCloseChildren" runat="server">
                                            </ul>
                                        </div>
                                    </asp:View>
                                     <asp:View ID="VIWtypeCloseNode" runat="server">
                                        </div></li>
                                    </asp:View>
                                </asp:MultiView>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </asp:View>
            <asp:View ID="VIWmessage" runat="server" >
                <h3><asp:literal ID="LTmessage" runat="server"></asp:literal></h3>
            </asp:View>
        </asp:MultiView>
    </div>
</div>
<CTRL:UnsubscriptionDialog id="CTRLconfirmUnsubscription" runat="server" Visible="false" DisplayDescription="true" RaiseCommandEvents="true" DisplayCommands="true" ></CTRL:UnsubscriptionDialog>
<CTRL:EnrollDialog id="CTRLconfirmEnrollToCommunity" runat="server" Visible="false" DisplayDescription="true" RaiseCommandEvents="true" DisplayCommands="true" ></CTRL:EnrollDialog>
<asp:Literal ID="LTcssClassCollapsed" runat="server" Visible="false">collapsed</asp:Literal>
<asp:Literal ID="LTtemplateMessageDetails" runat="server" Visible="false"><ul class="messagedetails">{0}</ul></asp:Literal>
<asp:Literal ID="LTtemplateMessageDetail" runat="server" Visible="false"><li class="messagedetail">{0}</li></asp:Literal>
<asp:Literal ID="LTtreetypeCssClassNoSelect" runat="server" Visible="false">noselect</asp:Literal>
<asp:Literal ID="LTtreeKeepAutoOpenCssClass" runat="server" Visible="false">autoOpen</asp:Literal>