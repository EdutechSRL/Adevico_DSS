<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="AssignSubmissionWithNoEvaluation.aspx.vb" Inherits="Comunita_OnLine.AssignSubmissionWithNoEvaluation" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/Evaluate/UC/UC_WizardEvaluationCommitteesSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
    <link rel="stylesheet" href="../../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.treeTable.js"></script>

     <script language="javascript" type="text/javascript">
         $(function () {
             $("#tree_table").treeTable({
                 clickableNodeNames: true,
                 persist: false
             });
             $(".view-modal.view-confirm").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 850,
                 height: 450,
                 minHeight: 650,
                 minWidth: 350,
                 title: '<%=DialogTitleTranslation() %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });
         });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
   <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnocalls" runat="server"></asp:Label>
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
                                <asp:button ID="BTNassignToSubmissionsWithNoEvaluationTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start ASSIGNMENTS -->
                                <div class="fieldobject fielddescription">
                                    <div class="fieldrow">
                                        <asp:label ID="LBdisplayStartupInfo" runat="server" ></asp:label>
                                    </div>
                                </div>
                                <asp:MultiView ID="MLVassignments" runat="server">
                                    <asp:View ID="VIWsingleCommitteeAssignments" runat="server">
                                        <asp:Repeater ID="RPTassignments" runat="server">
                                            <HeaderTemplate>
                                                <table class="evaluators light match table">
                                                    <thead>
                                                        <th class="submittername"><asp:literal ID="LTsubmittername_t" runat="server">Submitter</asp:literal></th>
                                                        <th class="submittedon"><asp:literal ID="LTsubmitterType_t" runat="server">Type</asp:literal></th>
                                                        <th class="evaluators"><asp:literal ID="LTevaluators_t" runat="server">Evaluators</asp:literal></th>
                                                    </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                    <tbody>
                                                        <tr>
                                                            <td class="submittername">
                                                                <a name="submission_<%#Container.DataItem.IdSubmission %>"></a>
                                                                <asp:Label ID="LBsubmitterName" runat="server" Text='<%#Container.DataItem.DisplayName%>' CssClass="fullname"></asp:Label>
                                                                <asp:Literal ID="LTidSubmission" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmission %>'></asp:Literal>
                                                                <asp:Literal ID="LTidSubmitterType" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmitterType %>'></asp:Literal>
                                                            </td>
                                                            <td class="submittedon">
                                                                <asp:Literal ID="LTsubmitterType" runat="server" Text='<%#Container.DataItem.SubmitterType%>'></asp:Literal>
                                                            </td>
                                                            <td class="evaluators">
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
                                                    </tbody>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </asp:View>
                                    <asp:View ID="VIWmultipleCommitteeAssignments" runat="server">
                                        <asp:Repeater ID="RPTsubmissions" runat="server">
                                            <HeaderTemplate>
                                                <div class="fieldobject">
                                                <div class="fieldrow">
                                                <table id="tree_table" class="table light fullwidth evaluatormatch treetable">
                                                    <thead>
                                                        
                                                        <th class="submittername"><asp:literal ID="LTsubmittername_t" runat="server">Submitter</asp:literal></th>
                                                        <th class="submittertype"><asp:literal ID="LTsubmitterType_t" runat="server">Type</asp:literal></th>
                                                        <th class="evaluatorsnumber"><asp:literal ID="LTevaluators_t" runat="server">Evaluators</asp:literal></th>
                                                    </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                    <tbody>
                                                        <tr class="submitter" id="sub-<%#Container.DataItem.IdSubmission %>">
                                                            
                                                            <td class="submittername">
                                                                <a name="submission_<%#Container.DataItem.IdSubmission %>"></a>
                                                                <asp:Label ID="LBsubmitterName" runat="server" Text='<%#Container.DataItem.DisplayName%>' CssClass="fullname"></asp:Label>
                                                                <asp:Literal ID="LTidSubmission" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmission %>'></asp:Literal>
                                                                <asp:Literal ID="LTidSubmitterType" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmitterType %>'></asp:Literal>
                                                            </td>
                                                            <td class="submittertype">
                                                                <asp:Literal ID="LTsubmitterType" runat="server" Text='<%#Container.DataItem.SubmitterType%>'></asp:Literal>
                                                            </td>
                                                            <td class="evaluatorsnumber"><%#Container.DataItem.EvaluatorsCount%></td>
                                                        </tr>
                                                        <tr id="cmmh-sub-<%#Container.DataItem.IdSubmission %>" class="commission head first child-of-sub-<%#Container.DataItem.IdSubmission %>" >
                                                            
                                                            <td class="submittername"><asp:literal ID="LTcommittee_t" runat="server">Committee</asp:literal></td>
                                                            <td colspan="2" class="evaluators"><asp:literal ID="LTevaluators_t" runat="server">Evaluators</asp:literal></td>
                                                        </tr>
                                                        <asp:Repeater ID="RPTcommittees" runat="server" DataSource="<%#Container.DataItem.Committees%>" OnItemDataBound="RPTcommittees_ItemDataBound">
                                                             <ItemTemplate>
                                                                 <tr id="cmm-<%#Container.DataItem.IdCommittee %>-sub-<%#Container.DataItem.IdSubmission %>" class="commission child-of-sub-<%#Container.DataItem.IdSubmission %> <%#GetCommitteeCssClass(Container.DataItem.Display) %>">                                                                    
                                                                    <td class="submittername">
                                                                        <asp:Literal ID="LTcommitteeName" runat="server" Text='<%#Container.DataItem.Name%>'></asp:Literal>
                                                                        <asp:Literal ID="LTidCommittee" runat="server" Visible="false" Text='<%#Container.DataItem.IdCommittee %>'></asp:Literal>
                                                                    </td>
                                                                    <td colspan="2" class="evaluators">
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
                                                    </tbody>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                                </div>
                                                </div>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </asp:View>
                                </asp:MultiView>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNassignToSubmissionsWithNoEvaluationBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="view-modal view-confirm" id="DVconfirmAction" runat="server" visible="false">
        <div class="fieldobject fielddescription">
            <div class="fieldrow">
                <asp:label ID="LBdefaultConfirmDescription" runat="server"></asp:label>
            </div>
        </div>
        <div class="fieldobject">
            <div class="fieldrow">
                <asp:Repeater ID="RPTconfirmItems" runat="server">
                    <HeaderTemplate>
                        <table class="evaluators light match table">
                            <thead>
                                <th class="submittername"><asp:literal ID="LTsubmittername_t" runat="server">Submitter</asp:literal></th>
                                <th class="submittedtype"><asp:literal ID="LTsubmitterType_t" runat="server">Type</asp:literal></th>
                                <th class="committeename" id="THcommittee" runat="server"><asp:literal ID="LTcommitteeNameHeader_t" runat="server">Committee</asp:literal></th>
                                <th class="evaluators"><asp:literal ID="LTevaluatorsHeader_t" runat="server">Evaluators</asp:literal></th>
                            </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td class="submittername">
                                        <a name="submission_<%#Container.DataItem.IdSubmission %>"></a>
                                        <span class="fullname"><%#Container.DataItem.SubmitterName%></span>
                                    </td>
                                    <td class="submittedtype">
                                        <%#Container.DataItem.SubmitterType%>
                                    </td>
                                    <td class="committeename" id="TDcommitteeName" runat="server">
                                       <%#Container.DataItem.CommitteeName%>
                                    </td>
                                    <td class="evaluators">
                                         <%#Container.DataItem.AssignedEvaluators%>
                                         <asp:literal ID="LTevaluators" runat="server">*su</asp:literal>
                                         <%#Container.DataItem.AvailableEvaluators%>
                                    </td>
                                </tr>
                            </tbody>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="fieldobject clearfix">
            <div class="fieldrow right">
                <asp:Button ID="BTNcloseConfirmInEvaluationSettings" runat="server" />
                <asp:Button ID="BTNconfirmInEvaluationSettings" runat="server"  />
            </div>
        </div>
    </div>
</asp:Content>