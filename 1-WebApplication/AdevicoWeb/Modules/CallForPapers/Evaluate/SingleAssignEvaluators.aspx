<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="SingleAssignEvaluators.aspx.vb" Inherits="Comunita_OnLine.SingleAssignEvaluators" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/Evaluate/UC/UC_WizardEvaluationCommitteesSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" />
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
                                <asp:button ID="BTNassignEvaluatorsToAllSubmissionTop" runat="server" Text="Save"/>
                                <asp:button ID="BTNsaveSingleAssignmentsTop" runat="server" Text="Save"/>
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
                                            <asp:label ID="LBdisplayStartupInfo" runat="server" AssociatedControlID="RBLsingleAssignmentStartup"></asp:label>
                                            <br />
                                            <span class="options">
                                                <asp:RadioButtonList ID="RBLsingleAssignmentStartup"  runat="server"  RepeatLayout="Flow" RepeatDirection="Vertical">
                                                    <asp:ListItem Selected="True" Value="True">Assign evaluators to all submission</asp:ListItem>
                                                    <asp:ListItem Value="False">Assign manually evaluators to submission</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </span>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWassignments" runat="server">
                                        <asp:Repeater ID="RPTassignments" runat="server">
                                            <HeaderTemplate>
                                                <table class="evaluators light match">
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
                                                                <span class="fullname"><%#Container.DataItem.DisplayName%></span>
                                                                <asp:Literal ID="LTidSubmission" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmission %>'></asp:Literal>
                                                                <asp:Literal ID="LTidSubmitterType" runat="server" Visible="false" Text='<%#Container.DataItem.IdSubmitterType %>'></asp:Literal>
                                                            </td>
                                                            <td class="submittedon">
                                                                <%#Container.DataItem.SubmitterType%>
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
                                </asp:MultiView>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNassignEvaluatorsToAllSubmissionBottom" runat="server" Text="Save"/>
                                <asp:button ID="BTNsaveSingleAssignmentsBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>