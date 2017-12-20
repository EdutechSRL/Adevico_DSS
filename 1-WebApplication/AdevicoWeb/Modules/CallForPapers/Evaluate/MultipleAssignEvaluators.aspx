<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="MultipleAssignEvaluators.aspx.vb" Inherits="Comunita_OnLine.MultipleAssignEvaluators" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/Evaluate/UC/UC_WizardEvaluationCommitteesSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" EnableTreeTableScript="true" />
   
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#tree_table").treeTable({
                clickableNodeNames: true,
                initialState: "collapsed",
                persist: false
            });


        });

    </script>
    <style type="text/css">
        table.evaluatormatch
        {
            width: 100%;
            table-layout: fixed;

        }

        table.evaluatormatch th.head
        {
            width: 40px;
        }

        table.evaluatormatch tr.commission
        {
            background-color: #eee;
            border-left: 2px solid #999 !important;
            border-right: 2px solid #999 !important;
        }

        table.evaluatormatch tr.commission.first
        {
            border-top: 2px solid #999 !important;
        }

        table.evaluatormatch tr.commission.last
        {
            border-bottom: 2px solid #999 !important;
        }

        table.evaluatormatch td.head
        {
            padding-left: 1.5em;
        }

    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
   <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
             <CTRL:Messages ID="CTRLemptyMessage"  runat="server"/>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNassignEvaluatorsToAllSubmissionTop" runat="server" Text="Set to all"/>
                                <asp:button ID="BTNsaveMultipleCommitteeAssignmentsTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start ASSIGNMENTS -->
                                <asp:MultiView ID="MLVassignments" runat="server">
                                    <asp:View ID="VIWdisplayInfo" runat="server">
                                        <div class="">
                                            <br /><br /><br /><br />
                                            <asp:label ID="LBdisplayInfo" runat="server"></asp:label>
                                            <br /><asp:label ID="LBdisplayInfoAction" runat="server"></asp:label>
                                            <br />
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWstartup" runat="server">
                                        <div class="fieldobject">
                                            <asp:label ID="LBdisplayStartupInfo" runat="server" AssociatedControlID="RBLmultipleAssignmentStartup"></asp:label>
                                            <br />
                                            <span class="options">
                                                <asp:RadioButtonList ID="RBLmultipleAssignmentStartup"  runat="server"  RepeatLayout="Flow" RepeatDirection="Vertical">
                                                    <asp:ListItem Selected="True" Value="True">Assign evaluators to all submission</asp:ListItem>
                                                    <asp:ListItem Value="False">Assign manually evaluators to submission</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </span>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWassignments" runat="server">
                                        <div class="pager" runat="server" id="DVpagerTop"  visible="false">
                                            <asp:literal ID="LTpageTop" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
                                        </div>
                                        <asp:Repeater ID="RPTsubmissions" runat="server">
                                            <HeaderTemplate>
                                                <table id="tree_table" class="evaluatormatch table light treeTable">
                                                    <thead>
                                                        <th class="head">&nbsp;</th>
                                                        <th colspan="2"><asp:literal ID="LTsubmittername_t" runat="server">Submitter</asp:literal></th>
                                                        <th colspan="2"><asp:literal ID="LTsubmitterType_t" runat="server">Type</asp:literal></th>
                                                        <th class="short"><asp:literal ID="LTevaluators_t" runat="server">Evaluators</asp:literal></th>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                        <tr class="submitter" id="sub-<%#Container.DataItem.IdSubmission %>">
                                                            <td class="head">&nbsp;</td>
                                                            <td colspan="2">
                                                                <a name="submission_<%#Container.DataItem.IdSubmission %>"></a>
                                                                <span class="fullname"><%#Container.DataItem.DisplayName%></span>
                                                                <asp:Literal ID="LTidSubmission" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmission %>'></asp:Literal>
                                                                <asp:Literal ID="LTidSubmitterType" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmitterType %>'></asp:Literal>
                                                            </td>
                                                            <td colspan="2">
                                                                <%#Container.DataItem.SubmitterType%>
                                                            </td>
                                                            <td><%#Container.DataItem.EvaluatorsCount%></td>
                                                        </tr>
                                                        <tr id="cmmh-sub-<%#Container.DataItem.IdSubmission %>" class="commission head first child-of-sub-<%#Container.DataItem.IdSubmission %>" >
                                                            <td>&nbsp;</td>
                                                            <td><asp:literal ID="LTcommittee_t" runat="server">Committee</asp:literal></td>
                                                            <td colspan="4"><asp:literal ID="LTevaluators_t" runat="server">Evaluators</asp:literal></td>
                                                        </tr>
                                                        <asp:Repeater ID="RPTcommittees" runat="server" DataSource="<%#Container.DataItem.Committees%>" OnItemDataBound="RPTcommittees_ItemDataBound">
                                                             <ItemTemplate>
                                                                 <tr id="cmm-<%#Container.DataItem.IdCommittee %>-sub-<%#Container.DataItem.IdSubmission %>" class="commission child-of-sub-<%#Container.DataItem.IdSubmission %> <%#GetCommitteeCssClass(Container.DataItem.Display) %>">
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <%#Container.DataItem.Name%>
                                                                        <asp:Literal ID="LTidCommittee" runat="server" Visible="false" Text='<%#Container.DataItem.IdCommittee %>'></asp:Literal>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <div class="choseselect clearfix">
                                                                            <div class="left">
                                                                                <select runat="server" id="SLBevaluators" class="partecipants chzn-select" multiple tabindex="2">            
                                                                                </select>
                                                                            </div>
                                                                            <div class="right">
											                                    <span class="icons">
												                                    <span class="icon selectall" title="All" runat="server" id="SPNsubmissionEvaluatorsSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNsubmissionEvaluatorsSelectNone">&nbsp;</span>
											                                    </span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                 </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                    </tbody>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <div class="pager" runat="server" id="DVpagerBottom" visible="false">
                                            <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
                                        </div>
                                    </asp:View>
                                </asp:MultiView>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNassignEvaluatorsToAllSubmissionBottom" runat="server" Text="Set to all"/>
                                <asp:button ID="BTNsaveMultipleCommitteeAssignmentsBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>