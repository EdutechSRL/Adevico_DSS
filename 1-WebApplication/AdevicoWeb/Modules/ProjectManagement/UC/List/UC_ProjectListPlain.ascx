<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProjectListPlain.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProjectListPlain" %>
<%@ Register TagPrefix="CTRL" TagName="Roles" Src="~/Modules/ProjectManagement/UC/list/UC_InLineProjectRoles.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Completion" Src="~/Modules/Common/UC/UC_AdvancedProgressBar.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachment" Src="~/Modules/ProjectManagement/UC/UC_DialogProjectAttachments.ascx" %>
<div class="tablewrapper">
    <table class="table projects <%=GetTableCssClass() %> light fullwidth plain showinfo">
        <thead>
            <tr>
                <th class="name">
                    <asp:Literal ID="LTprojectName_t" runat="server">*Project Name</asp:Literal>
                    <asp:LinkButton ID="LNBorderByNameUp" runat="server" cssclass="icon orderUp" CommandName="name" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LNBorderByNameDown" runat="server" cssclass="icon orderDown" CommandName="name" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                </th>
                <th class="status">
                    <span class="statuslight"><span class="text"><asp:Literal ID="LTprojectCompletion_t" runat="server">*Project completion</asp:Literal></span></span>
                    <asp:LinkButton ID="LNBorderByCompletionUp" runat="server" cssclass="icon orderUp" CommandName="completion" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LNBorderByCompletionDown" runat="server" cssclass="icon orderDown" CommandName="completion" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                </th>
                <th class="projectstatus">
                    <asp:Literal ID="LTprojectStatus_t" runat="server">*Project Status</asp:Literal>
                </th>
                <th class="deadline">
                    <asp:Literal ID="LTprojectDeadline_t" runat="server">*Deadline</asp:Literal>
                    <asp:LinkButton ID="LNBorderByDeadlineUp" runat="server" cssclass="icon orderUp" CommandName="deadline" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LNBorderByDeadlineDown" runat="server" cssclass="icon orderDown" CommandName="deadline" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
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
            <asp:Repeater ID="RPTprojects" runat="server">
                <ItemTemplate>
                <tr class="project <%#RowCssClass(Container.DataItem) %>" id="prj-<%#Container.DataItem.Id %>">
                    <td class="name">
                        <span class="text">
                            <a name="<%# Container.DataItem.Id.ToString() %>"></a><asp:HyperLink ID="HYPprojectName" runat="server"></asp:HyperLink>
                            <asp:linkbutton runat="server" ID="LNBattachments" CommandName="attachment" CommandArgument="<%#Container.DataItem.Id %>"><span class="icons right"><span title="#title#" class="icon xs attacchment">&nbsp;</span></span></asp:linkbutton>
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
                            <asp:linkbutton ID="LNBvirtualDeleteProject" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                            <asp:linkbutton ID="LNBvirtualUnDeleteProject" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument="<%#Container.DataItem.Id %>"></asp:linkbutton>
                            <asp:HyperLink ID="HYPdeleteProject" runat="server" Visible="false" CssClass="icon delete needconfirm"></asp:HyperLink>
                        </span>
                    </td>
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
                                <asp:LinkButton ID="LNBtoggleProjectCommunity" runat="server" CssClass="command toggleinfo" CausesValidation="false" OnClientClick="return false;">Toggle Community</asp:LinkButton>
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
<CTRL:Attachment id="CTRLattachment" runat="server" Visible="false" CssClass="dlgviewprojectattachments hiddendialog" DialogIdentifier="dlgviewprojectattachments"></CTRL:Attachment>