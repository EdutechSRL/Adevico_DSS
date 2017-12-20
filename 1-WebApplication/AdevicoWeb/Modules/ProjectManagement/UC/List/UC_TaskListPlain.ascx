<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TaskListPlain.ascx.vb" Inherits="Comunita_OnLine.UC_TaskListPlain" %>
<%@ Register TagPrefix="CTRL" TagName="input" Src="~/Modules/Common/UC/UC_InLineInput.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Dialog" Src="~/Modules/ProjectManagement/UC/List/UC_TaskInfo.ascx" %>

<div class="tablewrapper">
    <table class="table tasks <%=GetTableCssClass() %> treetable light fullwidth plain">
        <thead>
            <tr>
                <th class="name">
                    <asp:Literal ID="LTtaskName_t" runat="server">*Activity Name</asp:Literal>
                    <asp:LinkButton ID="LNBorderByTaskNameUp" runat="server" cssclass="icon orderUp" CommandName="taskname" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LNBorderByTaskNameDown" runat="server" cssclass="icon orderDown" CommandName="taskname" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                </th>
                <th class="project" id="THproject" runat="server">
                    <asp:Literal ID="LTtaskProjectName_t" runat="server">*Project Name</asp:Literal>
                    <asp:LinkButton ID="LNBorderByProjectNameUp" runat="server" cssclass="icon orderUp" CommandName="projectname" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LNBorderByProjectNameDown" runat="server" cssclass="icon orderDown" CommandName="projectname" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                </th>
                <th class="status">
                    <span class="statuslight"><span class="text"><asp:Literal ID="LTtaskCompletion_t" runat="server">*Completion</asp:Literal></span></span>
                    <asp:LinkButton ID="LNBorderByTaskCompletionUp" runat="server" cssclass="icon orderUp" CommandName="completion" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LNBorderByTaskCompletionDown" runat="server" cssclass="icon orderDown" CommandName="completion" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                </th>
                <th class="projectstatus">
                    <asp:Literal ID="LTtaskStatus_t" runat="server">*Status</asp:Literal>
                </th>
                <th class="deadline">
                    <asp:Literal ID="LTtaskDeadline_t" runat="server">*Deadline</asp:Literal>
                    <asp:LinkButton ID="LNBorderByTaskDeadlineUp" runat="server" cssclass="icon orderUp" CommandName="deadline" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LNBorderByTaskDeadlineDown" runat="server" cssclass="icon orderDown" CommandName="deadline" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                </th>
                <th class="completeness" id="THmyCompleteness" runat="server" visible="false"> <asp:Literal ID="LTtaskMyCompletion_t" runat="server">*My completion</asp:Literal></th>
                
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="RPTtasks" runat="server">
                <ItemTemplate>
                <tr class="task <%#RowCssClass(Container.DataItem) %>" id="task-<%#Container.DataItem.Id %>">
                    <td class="name">
                        <a name="tsk<%# Container.DataItem.Id.ToString() %>"></a><asp:HyperLink ID="HYPtaskName" runat="server" CssClass="opendlgtask left" data-dlg='<%#Container.DataItem.Id %>'></asp:HyperLink>
                        <span class="icons right">
                            <asp:Label ID="LBattachments" runat="server" Visible="false" CssClass="icon xs attacchment" data-dlg='<%#Container.DataItem.Id %>'>&nbsp;</asp:Label>
                        </span>
                        <asp:Literal ID="LTidTask" runat="server" Visible="false" Text="<%# Container.DataItem.Id %>"></asp:Literal>
                        <asp:Literal ID="LTidAssignment" runat="server" Visible="false" Text="<%# Container.DataItem.IdAssignment %>"></asp:Literal>
                        <CTRL:dialog id="CTRLcompletionDialog" runat="server"></CTRL:dialog>
                    </td>
                    <td class="project" id="TDproject" runat="server">
                        <span class="text">
                            <a name="prj<%# Container.DataItem.Id.ToString() %>"></a><asp:HyperLink ID="HYPprojectName" runat="server"></asp:HyperLink>
                        </span>
                        <asp:Label ID="LBcommunityName" runat="server" CssClass="extrainfo"></asp:Label>
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
                    <td class="completeness" id="TDmyCompletenessEmpty" runat="server" visible="false">
                    &nsbp;
                    </td>
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr id="TRempty" runat="server" visible="false">
                        <td id="TDemptyItems" runat="server">
                            <asp:Label ID="LBemptyItems" runat="server">*No tasks</asp:Label>
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
                                <asp:LinkButton ID="LNBtoggleProjectCommunity" runat="server" CssClass="command toggleinfo" CausesValidation="false" OnClientClick="return false;" >Toggle Community</asp:LinkButton>
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
<asp:Literal ID="LThasdeadline" runat="server" Visible="false">hasdeadline</asp:Literal>
<asp:Literal ID="LTnodeadline" runat="server" Visible="false">nodeadline</asp:Literal>
<asp:Literal ID="LTmyCompletenessCssClass" runat="server" Visible="false">item personal</asp:Literal>
<asp:Literal ID="LTtdmyCompleteness" runat="server" Visible="false">completeness</asp:Literal>