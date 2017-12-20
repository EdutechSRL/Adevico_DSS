<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TaskListMultipleTree.ascx.vb" Inherits="Comunita_OnLine.UC_TaskListMultipleTree" %>
<%@ Register TagPrefix="CTRL" TagName="input" Src="~/Modules/Common/UC/UC_InLineInput.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Completion" Src="~/Modules/Common/UC/UC_AdvancedProgressBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Dialog" Src="~/Modules/ProjectManagement/UC/List/UC_TaskInfo.ascx" %>
<div class="tablewrapper">
 <table class="table treetable light tasks <%=GetTableContainerCssClass() %>  fullwidth bycommunityproject">
     <thead>
            <tr>
                <th class="name">
                    <asp:Literal ID="LTtaskName_t" runat="server">*Activity Name</asp:Literal>
                </th>
                <th class="status">
                    <span class="statuslight"><span class="text"><asp:Literal ID="LTtaskCompletion_t" runat="server">*Completion</asp:Literal></span></span>
                </th>
                <th class="projectstatus">
                    <asp:Literal ID="LTtaskStatus_t" runat="server">*Status</asp:Literal>
                </th>
                <th class="deadline">
                    <asp:Literal ID="LTtaskDeadline_t" runat="server">*Deadline</asp:Literal>
                </th>
                <th class="completeness" id="THmyCompleteness" runat="server" visible="false"> <asp:Literal ID="LTtaskMyCompletion_t" runat="server">*My completion</asp:Literal></th>
                <th class="actions" id="THactions" runat="server" visible="false">
                    <span class="icons"><asp:Label ID="LBprojectActions_t" runat="server" CssClass="icon actions"/></span>
                </th>
            </tr>
        </thead>
    <tbody>
    <asp:Repeater ID="RPTcommunities" runat="server">
        <ItemTemplate>
            <tr class="community" id="<%#Container.DataItem.IdRow %>">
                <td class="name" id="TDcommunityName" runat="server">
                    <span class="text"><%#Container.DataItem.Name%></span>
                </td>
            </tr>
            <tr runat="server" visible="false" id="TRpreviousPage">
                <td colspan="<%#Container.DataItem.CellsCount%>"><asp:LinkButton ID="LNBcontinueFromPreviousRow" runat="server" CommandName="gotopage"></asp:LinkButton></td>
            </tr>
            <asp:Repeater ID="RPTcontainer" runat="server" OnItemDataBound="RPTcontainer_ItemDataBound" OnItemCommand="RPTcontainer_ItemCommand" DataSource="<%#Container.DataItem.Projects%>">
                <ItemTemplate>
                    <tr class="<%#Container.DataItem.CssClass%>" id="<%#Container.DataItem.IdRow %>">
                        <td class="name">
                            <span class="text">
                                <a name="prj<%# Container.DataItem.Id.ToString() %>"></a><asp:HyperLink ID="HYPprojectName" runat="server"></asp:HyperLink>
                            </span>
                        </td>
                        <td class="status" id="TDprojectStatusCompleteness" runat="server" visible="false">
                            <asp:Label ID="LBprojectStatusCompleteness" CssClass="statuslight" runat="server"></asp:Label>
                        </td>
                        <td class="projectstatus" id="TDprojectStatus" runat="server" visible="false">
                            <asp:Label ID="LBprojectStatus" runat="server"></asp:Label>
                        </td>
                        <td class="deadline" id="TDprojectDeadline" runat="server" visible="false">
                            <asp:label ID="LBdeadline" runat="server" CssClass="text"></asp:label>
                        </td>
                        <td class="completeness" id="TDprojectMyCompleteness" runat="server" visible="false">
                            <CTRL:Completion ID="CTRLmyCompletion" Visible="false" runat="server" /><asp:Literal ID="LTempty" runat="server" Visible="false">&nbsp;</asp:Literal>
                        </td>   
                        <td class="completeness" id="TDmyCompletenessEmpty" runat="server" visible="false">
                        &nsbp;
                        </td>
                        <td class="actions" id="TDprojectActions" runat="server" visible="false">
                            <span class="icons">
                                <asp:HyperLink ID="HYPeditProjectMap" runat="server" Visible="false" CssClass="icon view"></asp:HyperLink>
                                <asp:HyperLink ID="HYPviewProjectMap" runat="server" Visible="false" CssClass="icon view"></asp:HyperLink>
                                <asp:HyperLink ID="HYPeditProject" runat="server" Visible="false" CssClass="icon edit"></asp:HyperLink>
                            </span>
                        </td>    
                    </tr>
                    <tr runat="server" visible="false" id="TRpreviousPage">
                        <td colspan="<%#Container.DataItem.CellsCount%>"><asp:LinkButton ID="LNBcontinueFromPreviousRow" runat="server" CommandName="gotopage"></asp:LinkButton></td>
                    </tr>
                    <asp:Repeater ID="RPTtasks" runat="server" DataSource="<%#Container.DataItem.Tasks%>" OnItemDataBound="RPTtasks_ItemDataBound" OnItemCommand="RPTtasks_ItemCommand">
                        <ItemTemplate>
                        <tr class="task child-of-<%#Container.DataItem.IdFatherRow %>" id="task-<%#Container.DataItem.Id %>">
                             <td class="name">
                                <a name="tsk<%# Container.DataItem.Id.ToString() %>"></a><asp:HyperLink ID="HYPtaskName" runat="server" CssClass="opendlgtask left" data-dlg='<%#Container.DataItem.Id %>'></asp:HyperLink>
                                <span class="icons right">
                                    <asp:Label ID="LBattachments" runat="server" Visible="false" CssClass="icon xs attacchment" data-dlg='<%#Container.DataItem.Id %>'>&nbsp;</asp:Label>
                                </span>
                                <asp:Literal ID="LTidTask" runat="server" Visible="false" Text="<%# Container.DataItem.Id %>"></asp:Literal>
                                <asp:Literal ID="LTidAssignment" runat="server" Visible="false" Text="<%# Container.DataItem.IdAssignment %>"></asp:Literal>
                                <CTRL:dialog id="CTRLcompletionDialog" runat="server"></CTRL:dialog>
                            </td>
                            <td class="status">
                                <asp:Label ID="LBtaskStatusCompleteness" CssClass="statuslight" runat="server"></asp:Label>
                            </td>
                            <td class="projectstatus">
                                <asp:Label ID="LBtaskStatus" runat="server"></asp:Label>
                            </td>
                            <td class="deadline">
                                <asp:label ID="LBdeadline" runat="server" CssClass="text"></asp:label>
                            </td>
                            <td class="completeness" id="TDmyCompleteness" runat="server" visible="false">
                                <span class="completeness">
                                    <span class="item personal" title="<%= GetMyCompletenessTranslation %>">
                                    <CTRL:input runat="server" ID="CTRLmyCompleteness" ContainerCssClass="editable" InputMaxChar="4"  ValidationEnabled="false" ></CTRL:input>
                                    </span>
                                </span>
                            </td>
                            <td class="actions" id="TDactions" runat="server" visible="false">
                                <span class="icons">&nbsp;</span>
                            </td>
                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" visible="false" id="TRnextPage">
                        <td colspan="<%#Container.DataItem.CellsCount%>"><asp:LinkButton ID="LNBcontinueToNextRow" runat="server" CommandName="gotopage"></asp:LinkButton></td>
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
                                <asp:LinkButton ID="LNBtoggleTaskStatus" runat="server" CssClass="command togglestatus" CausesValidation="false" OnClientClick="return false;">Toggle Status</asp:LinkButton>
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
<asp:Literal ID="LTcssClassByProject" runat="server" Visible="false">byproject</asp:Literal>
<asp:Literal ID="LTitemCssClassProject" runat="server" Visible="false">project</asp:Literal>
<asp:Literal ID="LTitemCssClassCommunity" runat="server" Visible="false">community</asp:Literal>
<asp:Literal ID="LTitemCssClassDeadline" runat="server" Visible="false">deadline</asp:Literal>
<asp:Literal ID="LThasdeadline" runat="server" Visible="false">hasdeadline</asp:Literal>
<asp:Literal ID="LTnodeadline" runat="server" Visible="false">nodeadline</asp:Literal>
<asp:Literal ID="LTcssClassContinueTop" runat="server" Visible="false">continue top child-of-</asp:Literal>
<asp:Literal ID="LTcssClassContinueBottom" runat="server" Visible="false">continue bottom child-of-</asp:Literal>
<asp:Literal ID="LTtdmyCompleteness" runat="server" Visible="false">completeness</asp:Literal>