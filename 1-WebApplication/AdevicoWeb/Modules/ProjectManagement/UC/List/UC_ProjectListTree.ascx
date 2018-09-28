<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProjectListTree.ascx.vb" Inherits="Comunita_OnLine.UC_ProjectListTree" %>
<%@ Register TagPrefix="CTRL" TagName="Roles" Src="~/Modules/ProjectManagement/UC/list/UC_InLineProjectRoles.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Completion" Src="~/Modules/Common/UC/UC_AdvancedProgressBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<div class="tablewrapper">
    <table class="table projects <%=GetTableCssClass() %>  treetable light fullwidth">
        <thead>
            <tr>
                <th class="name">
                    <asp:Literal ID="LTprojectName_t" runat="server">*Project Name</asp:Literal>
                </th>
                <th class="status">
                    <span class="statuslight"><span class="text"><asp:Literal ID="LTprojectCompletion_t" runat="server">*Project completion</asp:Literal></span></span>
                </th>
                <th class="projectstatus">
                    <asp:Literal ID="LTprojectStatus_t" runat="server">*Project Status</asp:Literal>
                </th>
                <th class="deadline">
                    <asp:Literal ID="LTprojectDeadline_t" runat="server">*Deadline</asp:Literal>
                </th>
                <th class="roles" id="THroles" runat="server" visible="true">
                    <asp:Literal ID="LTprojectRoles_t" runat="server">*My roles</asp:Literal>
                </th>
                <th class="completeness" id="THmyCompleteness" runat="server" visible="false"> <asp:Literal ID="LTmyCompletion_t" runat="server">*My completion</asp:Literal></th>
                <th class="actions">
                    <span class="icons"><asp:Label ID="LBprojectActions_t" runat="server" CssClass="icon actions"/></span>
                </th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="RPTcontainer" runat="server">
                <ItemTemplate>
                    <tr class="<%#Container.DataItem.CssClass%>" id="<%#Container.DataItem.IdRow %>">
                        <td class="name" colspan="<%#Container.DataItem.CellsCount%>">
                            <span class="text"><%#Container.DataItem.Name%></span>
                        </td>       
                    </tr>
                    <tr runat="server" visible="false" id="TRpreviousPage">
                        <td colspan="<%#Container.DataItem.CellsCount%>"><asp:LinkButton ID="LNBcontinueFromPreviousRow" runat="server" CommandName="gotopage"></asp:LinkButton></td>
                    </tr>
                    <asp:Repeater ID="RPTprojects" runat="server" DataSource="<%#Container.DataItem.Projects%>" OnItemCommand="RPTprojects_ItemCommand" OnItemDataBound="RPTprojects_ItemDataBound">
                        <ItemTemplate>
                        <tr class="project child-of-<%#Container.DataItem.IdFatherRow %>" id="prj-<%#Container.DataItem.Id %>">
                            <td class="name">
                                <span class="text"><a name="<%# Container.DataItem.Id.ToString() %>"></a><asp:HyperLink ID="HYPprojectName" runat="server"></asp:HyperLink>

                                       <span class="icons right">
                                           <asp:linkbutton ID="LNBattachments" CommandName="attachment" CommandArgument="<%#Container.DataItem.Id %>" CssClass="icon xs attacchment"></asp:linkbutton>
                            <span title="3 attachments" class="icon xs attacchment">&nbsp;</span>
                        </span>
                                </span>
                             
                                <asp:Label ID="LBcommunityName" runat="server" CssClass="extrainfo"></asp:Label>
                            </td>
                            <td class="status">
                                <asp:Label ID="LBprojectStatusCompleteness" CssClass="statuslight" runat="server"></asp:Label>
                            </td>
                            <td class="projectstatus">
                                <asp:Label ID="LBprojectStatus" runat="server"></asp:Label>
                            </td>
                            <td class="deadline">
                                <asp:label ID="LBdeadline" runat="server" CssClass="text"></asp:label>
                            </td>
                            <td class="roles" id="TDroles" runat="server" visible="true">
                                <CTRL:Roles id="CTRLroles" runat="server" />
                            </td>
                            <td class="completeness" id="TDmyCompleteness" runat="server" visible="false">
                                <CTRL:Completion ID="CTRLmyCompletion" Visible="false" runat="server" />
                            </td>
                            <td class="actions">
                                <span class="icons">
                                    <asp:HyperLink ID="HYPeditProjectMap" runat="server" Visible="false" CssClass="icon view"></asp:HyperLink>
                                    <asp:HyperLink ID="HYPviewProjectMap" runat="server" Visible="false" CssClass="icon view"></asp:HyperLink>
                                    <asp:HyperLink ID="HYPeditProjectResources" runat="server" Visible="false" CssClass="icon users"></asp:HyperLink>
                                    <asp:HyperLink ID="HYPeditProject" runat="server" Visible="false" CssClass="icon edit"></asp:HyperLink>
                                    <asp:linkbutton ID="LNBvirtualDeleteProject" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdetete" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBvirtualUnDeleteProject" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument="<%#Container.DataItem.Id %>"></asp:linkbutton>
                                    <asp:HyperLink ID="HYPdeleteProject" runat="server" Visible="false" CssClass="icon delete needconfirm"></asp:HyperLink>
                                </span>
                            </td>
                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" visible="false" id="TRnextPage">
                        <td colspan="<%#Container.DataItem.CellsCount%>"><asp:LinkButton ID="LNBcontinueToNextRow" runat="server" CommandName="gotopage"></asp:LinkButton></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr id="TRempty" runat="server" visible="false">
                        <td id="TDemptyItems" runat="server">
                            <asp:Label ID="LBemptyItems" runat="server">*No projects</asp:Label>
                        </td>
                    </tr>
                </FooterTemplate> 
            </asp:Repeater>
        </tbody>
        <tfoot>
            <tr>
                <td id="TDfooter" runat="server" visible="false">
                    <div class="clearfix">
                        <div class="left">
                        </div>
                        <div class="right">
                            <span class="commands" runat="server" id="SPNcommands">
                                <asp:LinkButton ID="LNBtoggleProjectStatus" runat="server" CssClass="command togglestatus" CausesValidation="false" OnClientClick="return false;">Toggle Status</asp:LinkButton>
                                <asp:LinkButton ID="LNBtoggleProjectRoles" runat="server" CssClass="command toggleroles" CausesValidation="false" OnClientClick="return false;">Toggle Roles</asp:LinkButton>
                                <asp:LinkButton ID="LNBtoggleProjectCommunity" runat="server" CssClass="command toggleinfo" CausesValidation="false" OnClientClick="return false;" Visible="false">Toggle Community</asp:LinkButton>
                            </span>
                        </div>
                    </div>
                </td>
            </tr>
        </tfoot>
    </table>
    <div class="pager" runat="server" id="DVpagerBottom" visible="false">
        <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal>
        <CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
    </div>
</div>
<asp:Literal ID="LTstatusContent" runat="server" Visible="false"><span class="th">{0}%</span></asp:Literal>
<asp:Literal ID="LTstatuslight" runat="server" Visible="false">statuslight</asp:Literal>
<asp:Literal ID="LTcssClassManager" runat="server" Visible="false">asmanager</asp:Literal>
<asp:Literal ID="LTcssClassResource" runat="server" Visible="false">asresource</asp:Literal>
<asp:Literal ID="LTcssClassByDeadline" runat="server" Visible="false">bydeadline</asp:Literal>
<asp:Literal ID="LTcssClassByCommunity" runat="server" Visible="false">bycommunity</asp:Literal>
<asp:Literal ID="LTitemCssClassCommunity" runat="server" Visible="false">community</asp:Literal>
<asp:Literal ID="LTitemCssClassDeadline" runat="server" Visible="false">deadline</asp:Literal>
<asp:Literal ID="LThasdeadline" runat="server" Visible="false">hasdeadline</asp:Literal>
<asp:Literal ID="LTnodeadline" runat="server" Visible="false">nodeadline</asp:Literal>
<asp:Literal ID="LTcssClassContinueTop" runat="server" Visible="false">continue top child-of-</asp:Literal>
<asp:Literal ID="LTcssClassContinueBottom" runat="server" Visible="false">continue bottom child-of-</asp:Literal>
