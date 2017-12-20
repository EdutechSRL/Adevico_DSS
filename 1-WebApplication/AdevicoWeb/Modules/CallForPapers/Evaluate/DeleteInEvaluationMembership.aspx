<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="DeleteInEvaluationMembership.aspx.vb" Inherits="Comunita_OnLine.DeleteInEvaluationMembership" %>
<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
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
    <link href="../../../Graphics/Template/Wizard/css/wizard.css" type="text/css" rel="stylesheet" />
<%--    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>--%>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.progressbar.js"></script>
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
                                <asp:HyperLink ID="HYPbackToViewTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <div id="Wizard">
                                    <div class="wiz_header">
                                        <div class="wiz_top_nav">
                                            <div class="right stepButton">
                                                <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                                                <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                                                <asp:Button ID="BTNcompleteTop" cssClass="submitBlock" runat="server" Text="Next" Visible="false" />
                                            </div>
                                        </div>
                                        <div class="wiz_top_info ">
                                            <div class="wiz_top_desc clearfix">
                                                <h3>
                                                    <asp:Label ID="LBstepRemoveTitle" runat="server" CssClass="title"></asp:Label>
                                                </h3>
                                                <asp:Label ID="LBstepRemoveDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="wiz_content">
                                <asp:MultiView ID="MLVreplaceWizard" runat="server">
                                    <asp:View ID="VIWdefault" runat="server">
                                        <div class="StepData IW_Step1">
                                            <span class="Fieldrow">
                                                <br/><br/><br/><br/>
                                                <asp:Label ID="LBemptyStepInfo" runat="server"></asp:Label>
                                                <br/><br/><br/><br/>
                                            </span>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWdetails" runat="server">
                                        <div class="StepData IW_Step1">
                                            <span class="Fieldrow">
                                                <div class="infotext">
                                                    <asp:literal ID="LTuserInfo" runat="server">
                                                        {0} <span class="fullname">"{1}"</span> {2}.    
                                                    </asp:literal>
                                                </div>
                                                <div class="infotext">
                                                    <asp:literal ID="LTuserStepInfo" runat="server">
                                                        {0} <em>{1}</em> {2} <em>{3}</em> {4}
                                                    </asp:literal>
                                                </div>
                                            </span>
                                            <div class="fieldobject details">
                                                <div class="fieldrow">
                                                    <asp:Label ID="LBevaluationsInfo_t" runat="server" CssClass="fieldlabel">Evaluations:</asp:Label>
                                                    <span class="status completion">
                                                        <span class="statusitem">
                                                            <asp:Label id="LBnotstartedCount" runat="server" CssClass="gray"></asp:Label>
                                                            <asp:Label id="LBnotstarted" runat="server" CssClass="label"></asp:Label>
                                                        </span>
                                                        <span class="statusitem">
                                                            <asp:Label id="LBstartedCount" runat="server" CssClass="yellow"></asp:Label>
                                                            <asp:Label id="LBstarted" runat="server" CssClass="label"></asp:Label>
                                                        </span>
                                                        <span class="statusitem">
                                                            <asp:Label id="LBcompletedCount" runat="server" CssClass="green"></asp:Label>
                                                            <asp:Label id="LBcompleted" runat="server" CssClass="label"></asp:Label>
                                                        </span>
                                                    </span>
                                                </div>
                                            </div>
                                            <span class="Fieldrow">
                                                <table class="table minimal">
                        	                        <thead>
                        	                            <tr>
                        	                                <th class="name"><asp:literal ID="LTsubmitterName_t" runat="server"></asp:literal></th>
                        	                                <th class="status"><asp:literal ID="LTevaluationStatus_t" runat="server"></asp:literal></th>
                        	                            </tr>
                        	                        </thead>
                                                    <tbody>
                                                <asp:Repeater ID="RPTevaluations" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <%# Container.DataItem.DisplayName%>
                                                            </td>
                                                            <td class="status">
                                                                <span class="icons">
                                                                    <asp:label ID="LBstatusIcon" runat="server" CssClass="icon status">&nbsp;</asp:label>
                                                                </span>
                                                                <asp:label ID="LBstatus" runat="server">&nbsp;</asp:label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                        	                        </tbody>
                        	                    </table>
                                            </span>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWactionForRemove" runat="server">
                                        <div class="StepData IW_Step1">
                                            <div class="fieldobject">
                                                <div class="fieldrow">
                                                    <dl class="options replaceevaluator">
                                                        <dt>
                                                            <asp:RadioButton ID="RBremoveAll" runat="server" GroupName="replace" />
                                                            <asp:Label ID="LBremoveAll" runat="server" AssociatedControlID="RBremoveAll">Reassign all</asp:Label>
                                                        </dt>
                                                        <dd>
                                                            <asp:Literal ID="LTremoveAll" runat="server"></asp:Literal>
                                                        </dd>
                                                        <dt>
                                                            <asp:RadioButton ID="RBkeepEvaluatedFromRemoving" runat="server" GroupName="replace" />
                                                            <asp:Label ID="LBkeepEvaluatedFromRemoving" runat="server" AssociatedControlID="RBkeepEvaluatedFromRemoving">Keep the completed ones</asp:Label>
                                                        </dt>
                                                        <dd>
                                                            <asp:Literal ID="LTkeepEvaluatedFromRemoving" runat="server"></asp:Literal>
                                                        </dd>
                                                    </dl>
            	                                </div>
            	                            </div>
                                        </div>
                                    </asp:View>
                                </asp:MultiView>
                                    </div>
                                    <div class="wiz_bot_nav clearfix">
                                        <div class="right stepButton">
                                            <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                                            <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
                                            <asp:Button ID="BTNcompleteBottom" cssClass="submitBlock" runat="server" Text="Next" Visible="false"  />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackToViewBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
</asp:MultiView>
</asp:Content>