<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DasboardListTopControl.ascx.vb" Inherits="Comunita_OnLine.UC_DasboardListTopControl" %>
<%@ Register TagPrefix="CTRL" TagName="Attachment" Src="~/Modules/ProjectManagement/UC/UC_DialogProjectAttachments.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:MultiView ID="MLVtopItem" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWdefault" runat="server">
        <div class="fieldobject summary mysummary compressed">
            <div class="summarygroup compact">
                <div class="fieldrow summaryheader clearfix">
                    <div class="divsummarytitle">
                        <div class="left">
                            <h3><asp:Literal ID="LTsummaryTitleTop" runat="server">*My summary</asp:Literal></h3>
                            <asp:Literal ID="LTsummarySubtitleTop" runat="server"></asp:Literal>
                        </div>
                        <div class="right">
                            <div class="divsummary">
                                <div class="divsummaryinner">
                                    <asp:Label ID="LBsummaryActivitiesnumber_t" runat="server" CssClass="fieldlabel">*Activities:</asp:Label>
                                    <span class="summaryitem first">
                                        <a href="">
                                            <asp:Label ID="LBsummaryResourceActivities" runat="server" CssClass="quantity">0</asp:Label>
                                            <asp:Label ID="LBsummaryResourceActivities_t" runat="server" CssClass="text">*as resource</asp:Label>
                                        </a>
                                    </span>
                                    <span class="summaryitem last">
                                        <a href="">
                                            <asp:Label ID="LBsummaryManageActivities" runat="server" CssClass="quantity">0</asp:Label>
                                            <asp:Label ID="LBsummaryManageActivities_t" runat="server" CssClass="text">*to manage</asp:Label>
                                        </a>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="summarygroup extended">
                <div class="fieldrow summaryheader clearfix">
                    <div class="divsummarytitle">
                        <div class="left">
                            <h3><asp:Literal ID="LTsummaryTitle" runat="server">*My summary</asp:Literal></h3>
                            <asp:Literal ID="LTsummarySubtitle" runat="server"></asp:Literal>
                        </div>
                        <div class="right">
                            <div class="summaryselector" id="DVsummaryDisplay" runat="server">
                	            <span class="btnswitchgroup small"><!--
                                --><asp:Repeater ID="RPTdisplayItems" runat="server" OnItemCommand="RPTitems_ItemCommand">
                                    <ItemTemplate><!--
                                        --><asp:LinkButton ID="LNBdisplayItems" runat="server" CommandName="display" CommandArgument='<%#cint(Container.DataItem.Value) %>'></asp:LinkButton><!--
                                    --></ItemTemplate>  
                                </asp:Repeater><!--
                                --></span>
                	        </div>
                            <div class="summaryselector">
                                <span class="btnswitchgroup small"><!--
                                --><asp:Repeater ID="RPTtimeline" runat="server" OnItemCommand="RPTitems_ItemCommand">
                                    <ItemTemplate><!--
                                        --><asp:LinkButton ID="LNBtimeline" runat="server" CommandName="timeline" CommandArgument='<%#cint(Container.DataItem.Value) %>' CausesValidation="false"></asp:LinkButton><!--
                                    --></ItemTemplate>  
                                </asp:Repeater><!--
                                --></span>
                            </div>
                            <span class="icons"><asp:Label ID="LBcloseSummary" runat="server" CssClass="icon close handle alt"></asp:Label></span>
                        </div>
                        </div>
                    </div>
                <div class="fieldrow summarycontent">
                    <div class="divsummary">
                        <asp:Repeater ID="RPTsummaryItems" runat="server">
                            <ItemTemplate>
                                <div class="summarywrapper <%#GetItemCssClass(Container.DataItem.DisplayAs)%>">
                                    <asp:Label id="LBsummaryInfo_t" runat="server" CssClass="fieldlabel"></asp:Label>
                                    <div class="divsummaryinner">
                                    <asp:Repeater ID="RPTsummaryActivities" runat="server" DataSource="<%#Container.DataItem.Activities %>" OnItemDataBound="RPTsummaryActivities_ItemDataBound" OnItemCommand ="RPTitems_ItemCommand">
                                        <ItemTemplate>
                                            <span class="summaryitem <%#GetItemCssClass(Container.DataItem)%>" ><asp:HyperLink ID="HYPviewDashBoard" runat="server"></asp:HyperLink><asp:Literal ID="LTviewDashboard" runat="server" Visible="false"></asp:Literal></span>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
        <div class="fieldobject">
            <telerik:radtabstrip id="TBSselector" runat="server" align="Justify" width="100%" height="20px" causesvalidation="false" autopostback="false" skin="Outlook" enableembeddedskins="true">
                <tabs>
                    <telerik:RadTab text="*Resources" value="Resource" visible="false"></telerik:RadTab>
                    <telerik:RadTab text="*Manager" value="Manager" visible="false"></telerik:RadTab>
                    <telerik:RadTab text="*Administration" value="Administration" visible="false"></telerik:RadTab>
                </tabs>
            </telerik:radtabstrip>
        </div>
        <div class="fieldobject" id="DVprojectInfo" runat="server" visible="false">
            <div class="fieldrow">
                <asp:Label ID="LBdashboardProjectName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBdashboardProjectName"></asp:Label>
                <span class="projectname">
                    <asp:Label ID="LBdashboardProjectName" runat="server" CssClass="text"></asp:Label>
                    <span class="icons">
                        <asp:Label ID="LBattachments" runat="server" Visible="false" CssClass="icon xs attacchment">&nbsp;</asp:Label>
                    </span>
                </span>
            </div>
        </div>
        <div class="fieldobject" id="DVfilterBy" runat="server" visible="false">
            <div class="fieldrow">
                <asp:Label ID="LBfilterItemsBy_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLfilterBy"></asp:Label>
                <asp:DropDownList ID="DDLfilterBy" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="fieldrow" id="DVcurrentCommunity" runat="server">
                <asp:Label ID="LBcurrentCommunityFilter_t" runat="server" AssociatedControlID="LBcurrentCommunityFilter" CssClass="fieldlabel"></asp:Label>
                <asp:Label ID="LBcurrentCommunityFilter" runat="server" CssClass="communityname"></asp:Label>
            </div>
        </div>
         <div class="fieldobject clearfix">
            <div class="fieldrow left">
                <asp:Label ID="LBgroupBy_t" runat="server">*Group by:</asp:Label>
                <span class="btnswitchgroup small"><asp:Repeater ID="RPTgroupBy" runat="server"><ItemTemplate><asp:HyperLink ID="HYPgroupBy" runat="server"></asp:HyperLink></ItemTemplate></asp:Repeater></span>
            </div>
            <div class="fieldrow right">
                <asp:Label ID="LBfilterByProjectFilterStatus_t" runat="server">*Filter by projects:</asp:Label>
                <span class="btnswitchgroup small"><asp:Repeater ID="RPTfilterStatus" runat="server" ><ItemTemplate><asp:Literal ID="LTstatus" runat="server" Visible="false"></asp:Literal><asp:HyperLink ID="HYPfilterStatus" runat="server" ></asp:HyperLink></ItemTemplate></asp:Repeater></span>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWempty" runat="server"></asp:View>
</asp:MultiView>
<asp:Literal ID="LTsummaryItemSelected" runat="server" Visible="false">active</asp:Literal>
<asp:Literal ID="LTbtnswitch" runat="server" Visible="false">btnswitch</asp:Literal>
<asp:Literal ID="LTtemplateSummarySubtitle" runat="server" Visible="false"><h4>{0}</h4></asp:Literal>
<asp:Literal ID="LTbtnswitchDisabledCssClass" runat="server" Visible="false">disabled</asp:Literal>
<asp:Literal ID="LTdefaultSummaryItem" runat="server" Visible="false">
    <span class="quantity">{0}</span>
    <span class="text">{1}</span>
    <span class="timeframe">{2}</span>
</asp:Literal>
<CTRL:Attachment id="CTRLattachment" runat="server" CssClass="dlgprojectattachments"></CTRL:Attachment>