<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditEvaluationCommittees.aspx.vb" Inherits="Comunita_OnLine.EditEvaluationCommittees" %>
<%@ Register Src="~/Modules/CallForPapers/Evaluate/UC/UC_AddCriterion.ascx" TagName="CTRLAddCriterion" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/Evaluate/UC/UC_EditCriterion.ascx" TagName="CTRLeditCriterion" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Dss/UC/UC_AggregationSelector.ascx" TagName="CTRLaggregation" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyInput.ascx" TagName="CTRLfuzzyInput" TagPrefix="CTRL" %>

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

         <script language="javascript" type="text/javascript">
             $(document).ready(function () {
                 $('#addCriterion').dialog({
                     appendTo: "form",
                     closeOnEscape: false,
                     autoOpen: false,
                     draggable: true,
                     modal: true,
                     title: "",
                     width: 840,
                     height: 650,
                     minHeight: 450,
                     //                minWidth: 700,
                     zIndex: 1000,
                     open: function (type, data) {
                         //                $(this).dialog('option', 'width', 700);
                         //                $(this).dialog('option', 'height', 600);
                         //$(this).parent().appendTo("form");
                         
                         $(".ui-dialog-titlebar-close", this.parentNode).hide();
                     }

                 });
                 //$(".addnewcriteria").fixedEqualizer();
                
             });

             function showDialog(id) {
                 var hash = location.hash.replace('#', '');

                 if (hash != '') {
                     // Show the hash if it's set
                     //alert(hash);

                     // Clear the hash in the URL
                     location.hash = '';
                 }
                 $('#' + id).dialog("open");
                 //fixHeight();
                 return false;
             }

             function fixHeight() {
                 $(".addnewcriteria").fixedEqualizer();
             }

             function closeDialog(id) {
                 $('#' + id).dialog("close");
             }


             function pageLoad(sender, args) {
                 
                 $(document).ready(function () {
                     fixHeight();

                 });

            }                                                        
             
    </script>
        <asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
            <script  type="text/javascript" language="javascript">
                $(function () {
                    showDialog("addCriterion");
                });
            </script>
        </asp:Literal>
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
                                <asp:button ID="BTNsaveCommitteesTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                            <CTRL:Messages ID="CTRLdssMessages"  runat="server" Visible="false" />
                            <CTRL:CTRLaggregation ID="CTRLcallAggregation" runat="server" Visible="false" RaiseEvents="false" IsDefaultForCall="true" CssClass="commissionslevel" RaiseEventForRatingSetSelect="true" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start EDITOR -->
                                <div class="treetop clearfix">
                                    <div class="visibilitynav left">
                                        <asp:Label ID="LBcriteriaHideTop" cssclass="fieldsHide" runat="server">Hide Criteria</asp:Label>
                                        <asp:Label ID="LBcriteriaShowTop" cssclass="fieldsShow" runat="server">Show Criteria</asp:Label>
                                        <asp:Label ID="LBcollapseAllTop" cssclass="collapseAll" runat="server">Collapse</asp:Label>
                                        <asp:Label ID="LBexpandAllTop" cssclass="expandAll" runat="server">Expand</asp:Label>
                                    </div>
                                    <div class="DivEpButton clearfix">
                                        <asp:Button ID="BTNaddCommitteeTop" runat="server" text="Add commission"/>
                                    </div>
                                </div>
                                <div class="tree">
                                    <a name="#section_0"></a>
						            <asp:Repeater ID="RPTcommittees" runat="server">
                                        <HeaderTemplate>
                                            <ul class="sections playmode committees">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li class="section clearfix autoOpen" id="committee_<%#Container.DataItem.Id %>">
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
                                                                <asp:Label ID="LBcommitteeName_t" cssclass="title" runat="server" AssociatedControlID="TXBcommitteeName">Commission:</asp:Label>
                                                                <asp:TextBox ID="TXBcommitteeName" runat="server" CssClass="itemname"></asp:TextBox>
                                                            </span>
								                            <span class="right">
								                                <span class="icons">
                                                                    <asp:Button ID="BTNaddCriteria" runat="server" Text="A" CssClass="icon addcriteria" CommandName="addCriteria"/>
                                                                    <asp:Button ID="BTNdeleteCommittee" runat="server" Text="D" CssClass="img_btn icon delete needconfirm" CommandName="virtualDelete"/>
								                                </span>
							                                </span>
						                                </div>
                                                    </div>
   					                                <div class="clearer"></div>
                                                    <ul class="fields criteria">
                                                        <li class="sectiondesc clearfix autoOpen" id="sectiondesc_<%#Container.DataItem.Id %>">
						                                    <div class="externalleft"></div>
						                                    <div class="fieldcontent">  
    							                                <div class="fielddetails">
								                                    <div class="fieldobject">
									                                    <div class="fieldrow fielddescription">
                                                                            <asp:Label ID="LBcommitteeDescription_t" CssClass="fieldlabel" runat="server" AssociatedControlID="TXBcommitteeDescription">Description:</asp:Label>
                                                                            <asp:TextBox ID="TXBcommitteeDescription" runat="server" Columns="40" class="textarea" TextMode="MultiLine"></asp:TextBox>
									                                    </div>
								                                    </div>
                                                                    <div class="fieldrow" id="DVsubmitterTypes" runat="server">
                                                                        <fieldset class="light expandable disabled hideall">
                                                                            <legend><asp:CheckBox ID="CBXadvancedSubmittersInfo" runat="server" /><label class="inline"><asp:Literal ID="LTadvancedSubmittersInfo" runat="server">Attiva gestione avanzata partecipanti</asp:Literal></label></legend>
                                                                            <div class="choseselect clearfix">
                                                                                <div class="left">
                                                                                    <asp:Label ID="LBcommitteeSubmitters_t" runat="server" CssClass="fieldlabel" AssociatedControlID="SLBsubmitters"></asp:Label>
                                                                                    <select runat="server" id="SLBsubmitters" class="partecipants chzn-select" multiple tabindex="2">
                                                                        
                                                                                    </select>
                                                                                </div>
                                                                                <div class="right">
											                                        <span class="icons">
												                                        <span class="icon selectall" title="All" runat="server" id="SPNcommitteeSubmittersSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNcommitteeSubmittersSelectNone">&nbsp;</span>
											                                        </span>
                                                                                </div>
									                                        </div>
                                                                        </fieldset>
                                                                    </div>
                                                                    <CTRL:CTRLfuzzyInput ID="CTRLfuzzyInput" runat="server" Visible="false" CssClass="commissionlevel" />
                                                                    <CTRL:CTRLaggregation ID="CTRLcallAggregation" runat="server" Visible="false" IsDefaultForCall="false" CssClass="commissionlevel" OnSelectedMethod="SelectedMethod" OnRequireWeights="RequireWeights" RaiseEventForRatingSetSelect="true" />
							                                    </div>
						                                    </div>
                                                            <div class="clearer"></div>
						                                </li>     
                                                        <asp:Repeater ID="RPTcriteria" runat="server" OnItemDataBound="RPTcriteria_ItemDataBound" OnItemCommand="RPTcriteria_ItemCommand">
                                                            <ItemTemplate>
                                                            <li class="cfield clearfix autoOpen" id="criterion_<%#Container.DataItem.Id %>">
						                                        <div class="externalleft">
                                                                    <asp:Label ID="LBmoveCriterion" cssclass="movecfield" runat="server">M</asp:Label>
						                                        </div>
                                                                <div class="fieldcontent">
                                                                    <span class="switchcfield handle">+</span>
							                                        <div class="internal clearfix">
                                                                        <span class="left">
                                                                            <a name="#criterion_<%#Container.DataItem.Id %>"></a>
                                                                            <asp:Literal ID="LTidCriterion" runat="server" Visible="false"></asp:Literal>
                                                                            <asp:Label ID="LBcriterionName_t" cssclass="title" runat="server">Field:</asp:Label>
                                                                            <asp:TextBox ID="TXBcriterionName" runat="server" CssClass="itemname"></asp:TextBox>
                                                                            <asp:Label ID="LBcriterionType" cssclass="type" runat="server"></asp:Label>
                                                                        </span>
								                                        <span class="right">
                                         								   <span class="icons">
                                                                                <asp:Button ID="BTNdeleteCriterion" runat="server" Text="D" CssClass="icon delete needconfirm" CommandName="virtualDelete"/>
								                                            </span>
							                                            </span>
                                                                    </div>
							                                        <div class="fielddetails">
                                                                        <input type="hidden" id="HDNcommitteeOwner" runat="server" class="hiddensort"/>
                                                                        <input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorder"/>
                                                                        <div class="fieldobject singleline">
                                                                            <CTRL:CTRLeditCriterion ID="CTRLeditCriterion" runat="server" OnAddOption="AddOption" OnRemoveOption="RemoveOption" OnChangeToIntegerType="ChangeToIntegerType" OnChangeToDecimalType="ChangeToDecimalType" />
                                                                        </div>
                                                                        <CTRL:CTRLfuzzyInput ID="CTRLfuzzyInput" runat="server" Visible="false" CssClass="criterialevel" />
								                                    </div>
						                                        </div>
                                                                <div class="clearer"></div>
					                                        </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
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
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNsaveCommitteesBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
            </div>
            <div id="addCriterion" style="display: none;" class="addnewcriteria">
                <div id="DVaddCriterionTitle" class="addnewfield">
                     <div class="dialogheader">
                        <asp:Label ID="LBaddCriterionDialgoHeader" runat="server"></asp:Label>
                     </div>
                    <asp:UpdatePanel ID="UDPaddCriterion" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="dialogcontent clearfix">
                                <CTRL:CTRLaddCriterion ID="CTRLaddCriterion" runat="server" AjaxEnabled="true" />
                            </div>                            
                        </ContentTemplate>                        
                    </asp:UpdatePanel>
                    <div class="dialogfooter">
                        <asp:Button ID="BTNcloseCreateCriterionWindow" runat="server" CausesValidation="false" />
                        <asp:Button ID="BTNcreateCriterion" runat="server" CausesValidation="false" />
                    </div>
                </div>
            </div>
            <input type="hidden" id="HDNidCommittee" class="hiddencurrentsection" runat="server" />
<%--            <input type="hidden" class="hiddenselectedtype" id="HDNselectedType" runat="server" />--%>
        </asp:View>
    </asp:MultiView>
</asp:Content>