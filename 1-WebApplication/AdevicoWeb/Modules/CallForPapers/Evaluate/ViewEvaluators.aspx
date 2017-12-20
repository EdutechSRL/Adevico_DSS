<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ViewEvaluators.aspx.vb" Inherits="Comunita_OnLine.ViewEvaluators" %>

<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/Evaluate/UC/UC_WizardEvaluationCommitteesSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
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
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.progressbar.js"></script>
    
   <script language="javascript" type="text/javascript">
        $(function () {
            $(".progressbar").myProgressBar();
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
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start Assignments -->
                                <div class="treetop clearfix">
                                    <div class="visibilitynav left">
                                        <asp:Label ID="LBcollapseAllTop" cssclass="collapseAll" runat="server">Collapse</asp:Label>
                                        <asp:Label ID="LBexpandAllTop" cssclass="expandAll" runat="server">Expand</asp:Label>
                                    </div>
                                    <div class="DivEpButton clearfix">

                                    </div>
                                </div>
                                <div class="tree">
                                    <a name="#section_0"></a>
						            <asp:Repeater ID="RPTcommittees" runat="server">
                                        <HeaderTemplate>
                                            <ul class="sections playmode committees">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li class="section clearfix autoOpen" id="displaycommittee_<%#Container.DataItem.Id %>">
                                                <div class="externalleft">
                                                    <asp:Label ID="LBmoveCommittee" cssclass="movesection" runat="server" Visible="false"></asp:Label>
					                            </div>
                                                <div class="sectioncontent">
					                                <span class="switchsection handle">+</span>
                                                    <div class="innerwrapper">
                                                        <div class="internal clearfix">
								                            <span class="left">
                                                                <a name="#committee_<%#Container.DataItem.Id %>"></a>
                                                                <asp:Literal ID="LTidCommittee" runat="server" Visible="false"></asp:Literal>
                                                                <asp:Label ID="LBcommitteeName_t" cssclass="title" runat="server" AssociatedControlID="LBcommitteeName">Commission:</asp:Label>
                                                                <asp:Label ID="LBcommitteeName" cssclass="itemname" runat="server"></asp:Label>
                                                            </span>
								                            <span class="right">
								                                <span class="icons">
								                                </span>
							                                </span>
						                                </div>
                                                    </div>	
   					                                <div class="clearer"></div>
                                                    <ul class="fields">
                                                        <li class="sectiondesc clearfix autoOpen" id="sectiondesc_<%#Container.DataItem.Id %>">
						                                    <div class="externalleft"></div>
						                                    <div class="fieldcontent">  
    							                                <div class="fielddetails">
								                                    <div class="fieldobject">
									                                    <div class="fieldrow fielddescription">
                                                                            <asp:Label ID="LBcommitteeDescription_t" CssClass="fieldlabel" runat="server" AssociatedControlID="LBcommitteeDescription">Description:</asp:Label>
                                                                            <div class="fielddescription">
                                                                                <asp:Label ID="LBcommitteeDescription" runat="server"></asp:Label>
                                                                            </div>
									                                    </div>
								                                    </div>
							                                    </div>
						                                    </div>
                                                            <div class="clearer"></div>
						                                </li>
                                                        <li class="cfield clearfix autoOpen" id="evaluators_<%#Container.DataItem.Id %>">
                                                            <div class="externalleft"></div>
                                                            <div class="fieldcontent">
                                                                <table class="evaluators minimal onecommission">
                                                                    <thead>
                                                                    <tr>
                                                                        <th class="evaluator"><asp:Literal ID="LTevaluatorName_t" runat="server"></asp:Literal></th>
                                                                        <th class="status"><asp:Literal ID="LTevaluationsStatus_t" runat="server"></asp:Literal></th>
                                                                        <th class="actions"></th>
                                                                    </tr>
                                                                    </thead>
                                                                    <tbody>
                                                        <asp:Repeater ID="RPTevaluators" runat="server" DataSource="<%#Container.DataItem.Evaluators%>" OnItemDataBound="RPTevaluators_ItemDataBound">
                                                            <ItemTemplate>
                                                                    <tr class="<%#GetEvaluatorCssClass(Container.DataItem.Status,Container.DataItem.Display) %>">
                                                                        <td class="evaluator">
                                                                            <a name="membership_<%#Container.DataItem.IdMembership %>"></a>
                                                                            <span class="fullname"><%#Container.DataItem.DisplayName%></span>
                                                                            <asp:label id="LBextraInfo" runat="server" cssclass="extrainfo">
                                                                                ({0} <span class="fullname">{1}</span>)
                                                                            </asp:label>
                                                                        </td>
                                                                        <td class="status">
                                                                            <CTRL:StackedBar id="CTRLstackedBar" runat="server"></CTRL:StackedBar>
                                                                        </td>
                                                                        <td class="actions">
                                                                            <input type="hidden" id="HDNevaluatorselected" class="evaluatorselected" runat="server" />
                                                                            <span class="icons">
                                                                                <asp:HyperLink ID="HYPreplaceEvaluator" runat="server"  CssClass="icon replaceuser evaluator" Visible="false"></asp:HyperLink>
                                                                                <asp:HyperLink ID="HYPdeleteEvaluator" runat="server"  CssClass="icon delete confirmdialog evaluator" Visible="false"></asp:HyperLink>
                                                                              <%--  <span class="" title="replace">&nbsp;</span>
                                                                                <span class="icon delete confirmdialog evaluator" title="delete">&nbsp;</span>--%>
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                            <div class="clearer"></div>
                                                        </li>
                                                    </ul>
                                                    <div class="sectionfooter clearfix">
                                                        <asp:HyperLink ID="HYPtoTopCommittee" runat="server" class="ui-icon ui-icon-arrowthickstop-1-n ui-icon-circle-arrow-n"></asp:HyperLink>
                                                    </div>
					                            </div>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <div style="display:none;">

                                    </div>
                                <!-- @End EDITOR -->
                            </div>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>