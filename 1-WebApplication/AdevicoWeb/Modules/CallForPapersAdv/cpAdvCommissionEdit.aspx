<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="cpAdvCommissionEdit.aspx.vb" Inherits="Comunita_OnLine.cpAdvCommissionEdit" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/Evaluate/UC/UC_WizardEvaluationCommitteesSteps.ascx" %>--%>

<%@ Register TagPrefix="CTRL" TagName="WizardMenu" Src="~/Modules/CallForPapersAdv/UC/Uc_advCpWizSteps.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="FileUploader" Src="~/Modules/Repository/UC/UC_CompactInternalFileUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayFile" Src="~/Modules/Repository/UC/UC_ModuleRepositoryAction.ascx" %>

<%@ Register Src="~/Modules/CallForPapers/Evaluate/UC/UC_AddCriterion.ascx" TagName="CTRLAddCriterion" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/Evaluate/UC/UC_EditCriterion.ascx" TagName="CTRLeditCriterion" TagPrefix="CTRL" %>

<%@ Register Src="~/Modules/Common/UC/UC_HTMLExport.ascx" TagPrefix="CTRL" TagName="UC_HTMLExport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<CTRL:Header ID="CTRLheader" runat="server" />
	<script src="<%=PageUtility.BaseUrl %>Jscript/Modules/CallForPapers/callforpapersAdv.js"></script>
	<style type="text/css">
		#box-buttons .Link_Menu{
			float:right;
			margin-left:6px;
		}
		.fieldWithLabels .status{	
			display: inline-block;
			padding: 4px 10px;
			margin-right: 6px;
		}

		ul.tagit {
			vertical-align:middle;
		}
		.fieldWithLabels .status.Draft{
			background-color: #ffcd40;
		}
		.fieldWithLabels .status.ViewSubmission{
			background-color: #ffcd40;
		}
		.fieldWithLabels .status.Started{
			background-color: #99c74a;
		}
		.fieldWithLabels .status.ValutationEnded{
			background-color: #99c74a;
		}
		.fieldWithLabels .status.ValutationConfirmed{
			background-color: #99c74a;
		}
		.fieldWithLabels .status.Closed{
			background-color: #c74a4;
		}
		
		.fieldWithLabels .status.showsubs{
			background-color: #99c74a;
		}
		.fieldWithLabels .status.hidesubs{
			background-color: #ffcd40;
		}
		
		/*
		Draft                       giallo
		(ViewSubmission) è draft    giallo  
		Started                     verde
		Locked                      verde
		ValutationEnded             verde
		ValutationConfirmed         verde
		Closed                      rosso
		
		verde   #99c74a
		grigio  #ccc
		giallo  #ffcd40
		rosso   #c74a4

	*/
		
		div.fieldrow ul.tagit {
			width: 450px;
			display: inline-block;
		}

		.tagit-label{
			margin:0 !important;
		}

		a.tagit-close span.ui-icon
		{
			margin: 0;
		}	
	</style>

	<script language="javascript" type="text/javascript">
		$(document).ready(function () {
			$(".view-modal.view-users").dialog({
				appendTo: "form",
				closeOnEscape: false,
				modal: true,
				width: 890,
				height: 450,
				minHeight: 300,
				minWidth: 700,
				title: '<%=DialogTitleTranslation() %>',
				open: function (type, data) {
					//$(this).parent().appendTo("form");
					$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
				}
			});
			$("ul.criteriaAdv").sortable({
				update: function (event, ui) {
					var Data = $(this).sortable("serialize");
					var sectionId = $(this).attr("id");
					$(this).find(".hiddensort").val(sectionId);
					var x = 0;
					$(this).find(".hiddendisplayorder").each(function () {
						x += 1;
						$(this).val(x);
					});
					$.ajax({
						type: "POST",
						url: "../../Modules/CallForPapers/Evaluate/EvaluationReordering.asmx/CriteriaReorderAdv",
						data: "{'position':'" + Data + "', 'idCommittee':'" + sectionId + "'}",
						processData: false,
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (msg) {
							//alert(msg.d);
						},
						error: function (result) {
							//alert("Error: (" + result.status + ') [' + result.statusText + ']');

						}
					});
				}
			});

			if ($("span.cbxmaster input").prop("checked")) {
				$("div.stepeval").removeClass("hide");
			} else {
				$("div.stepeval").addClass("hide");
			}

			$("span.cbxmaster input").change(
				function () {
					if ($(this).prop("checked"))
					{
						$("div.stepeval").removeClass("hide");
					} else {
						$("div.stepeval").addClass("hide");
					}
				}
			);

		 });
	</script>
	<CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />


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

	<!-- input con id statico, store dei tag condivisi -->
	<asp:textbox ID="tagsinputSuggest" ClientIDMode="Static" runat="server" data-role="tagsinput" CssClass="hide"></asp:textbox>

	   <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
		<asp:View ID="VIWempty" runat="server">
			<CTRL:Messages ID="CTRLemptyMessage"  runat="server"/>
			<br /><br /><br /><br />
		</asp:View>
		<asp:View ID="VIWsettings" runat="server">
			<div class="contentwrapper edit clearfix persist-area">
				<div class="column left persist-header copyThis">
					<%--<CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>--%>
					<CTRL:WizardMenu runat="server" Id="CTRLmenu"></CTRL:WizardMenu>
				</div>
				<div class="column right resizeThis">
					<div class="rightcontent">
						<div class="header">
							<div class="DivEpButton">
								<asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Torna al processo di valutazione"></asp:HyperLink>
								<%--<asp:HyperLink ID="HYPbackBandiTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>--%>
								<asp:button ID="BTNsaveCommitteeTop" runat="server" Text="Save"/>
							</div>
							<CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
							<CTRL:Messages ID="CTRLdssMessages"  runat="server" Visible="false" />
							
						</div>
						<div class="contentouter">
							<div class="content clearfix">
								<fieldset>
									<div class="fieldrow fieldedition">
										<asp:Label ID="LBcommName_t" CssClass="fieldlabel" runat="server">Commissione:</asp:Label>
										<asp:TextBox ID="TXBcommName" size="60" runat="server"></asp:TextBox>
									</div>
									<div class="fieldrow fieldedition">
										<asp:Label ID="LBLcommDescription_t" CssClass="fieldlabel" runat="server">Descrizione:</asp:Label>
										<asp:TextBox ID="TXBcommDescription" style="width: 382px;" TextMode="MultiLine" runat="server"></asp:TextBox>
									</div>
									<div class="fieldrow fieldedition">
										<asp:Label ID="LBLcommTags" CssClass="fieldlabel" runat="server">Tags:</asp:Label>
										<asp:textbox id="TXBtags" runat="Server" CssClass="tagsinputOnlyStored hide"></asp:textbox>
									</div>
									<div class="fieldrow fieldedition">
										<asp:Label ID="LBLcommIsMaster_t" CssClass="fieldlabel" runat="server">Imposta come principale:</asp:Label>
										<asp:CheckBox ID="CBXisMaster" runat="server" Text="Commissione Master" CssClass="cbxmaster" />
									</div>
									<div class="fieldrow fieldedition fieldWithLabels">
										<asp:Label ID="LBLcommStatus_t" CssClass="fieldlabel" runat="server">Stato della commissione:</asp:Label>
										<span id="SPNstatus" runat="server" class="status">
											<asp:Label ID="LBLcommStatus" runat="server"></asp:Label>
										</span>
										<span id="SPNsubmissionVisibility" runat="server" class="status">
											<asp:Label ID="LBLsubmissionVisibility" runat="server"></asp:Label>
										</span>
									</div>
									<asp:PlaceHolder ID="PHevalutation" runat="server">
										<div class="fieldrow fieldedition">
											<asp:Label ID="LBLevalType_t" CssClass="fieldlabel" runat="server" ToolTip="Metodo di aggregazione delle valutazioni dei singoli commissari per ottenere il punteggio della COMMISSIONE.">Aggregazione commissari:</asp:Label>
											<asp:DropDownList ID="DDLevalType" runat="server">
												<asp:ListItem Text="Media" Value="1" Selected="true"></asp:ListItem>
												<asp:ListItem Text="Somma" Value="2"></asp:ListItem>
											</asp:DropDownList>
										</div>
										<div class="fieldrow fieldedition">
											<asp:Label ID="LBLevalMinValue_t" CssClass="fieldlabel" runat="server">Valore minimo commissione:</asp:Label>
											<asp:TextBox ID="TXBevalMinValue" runat="server">0</asp:TextBox>
										</div>
										<div class="fieldrow fieldedition">
											<asp:Label ID="LBLevalBoolType" CssClass="fieldlabel" runat="server">Vincola valori binari:</asp:Label>
											<asp:CheckBox ID="CBXevalBoolType" runat="server" checked="true"/>
										</div>
										<div ID="DVevaluationStep" runat="server" class="stepeval">
											<div class="fieldrow fieldedition">
												<asp:Label ID="LBLevalStep_t" CssClass="fieldlabel" runat="server" ToolTip="Metodo di aggregazione delle valutazioni delle commissioni per ottenere il punteggio finale dello STEP.">Aggregazione step:</asp:Label>
												<asp:DropDownList ID="DDLevalStep" runat="server">
													<asp:ListItem Text="Media" Value="1" Selected="true"></asp:ListItem>
													<asp:ListItem Text="Somma" Value="2"></asp:ListItem>
												</asp:DropDownList>
											</div>
										</div>
									</asp:PlaceHolder>
									<asp:PlaceHolder ID="PHeconomic" runat="server">
										<div class="fieldrow fieldmaxTotal">
											<asp:Label ID="LBLmaxTotal_t" CssClass="fieldlabel" runat="server">Massimale singola domanda:</asp:Label>
											<asp:TextBox ID="TXBmaxTotal" runat="server">0</asp:TextBox>
											<asp:Label ID="LBLmaxTotal_info" runat="server">(Se = 0, nessun limite)</asp:Label>
										</div>
									</asp:PlaceHolder>
								</fieldset>
								<asp:MultiView ID="MLVmain" runat="server">
									<asp:View ID="VMembres" runat="server" >
										<fieldset>
											<div class="fieldrow fieldedition">
												<asp:Label ID="LBLpresident_t" CssClass="fieldlabel" runat="server">Presidente: </asp:Label>
												<asp:Label ID="LBLpresident" CssClass="fieldlabel" runat="server"></asp:Label>
												<asp:LinkButton ID="LKBeditPresident" runat="server" CssClass="icons"><i class="icon edit">&nbsp;</i></asp:LinkButton>
											</div>
											<div class="fieldrow fieldedition">
												<asp:Label ID="LBLsecretary_t" CssClass="fieldlabel" runat="server">Segretario: </asp:Label>
												<asp:Label ID="LBLsecretary" CssClass="fieldlabel" runat="server"></asp:Label>
												<asp:LinkButton ID="LKBeditSecretary" runat="server" CssClass="icons"><i class="icon edit">&nbsp;</i></asp:LinkButton>
											</div>
											<div class="fieldrow fieldedition">
												<asp:Label ID="LBmembers_t" CssClass="fieldlabel" runat="server">Membri: </asp:Label><asp:LinkButton ID="LKBaddMember" CssClass="icons" runat="server">Aggiungi <i class="icon new">&nbsp;</i></asp:LinkButton>
											</div>
											<asp:Repeater ID="RPTmembers" runat="server">
												<HeaderTemplate>
													<table class="evaluators onecommission light">
														<thead>
															<tr>
																<th class="evaluator">Valutatore</th>
																<th class="actions">Azioni</th>
															</tr>
														</thead>
														<tbody>
												</HeaderTemplate>
												<ItemTemplate>
													<tr>
														<td class="evaluator">
															<asp:Label ID="LBmemName" runat="server"></asp:Label>
														</td>
														<td class="actions">
															<span class="icons">
																<%--<asp:LinkButton ID="LKBeditMember" runat="server" CssClass="icon edit"></asp:LinkButton>--%>
																<asp:LinkButton ID="LKBdelMember" runat="server" CssClass="icon delete"></asp:LinkButton>
																<asp:LinkButton ID="LKBupdateMember" runat="server" CssClass="icon edit"></asp:LinkButton>
															</span>
														</td>
													</tr>
												</ItemTemplate>
												<FooterTemplate>
														</tbody>
													</table>
												</FooterTemplate>
											</asp:Repeater>
										


											<div class="view-modal view-users" id="DVselectUsers" runat="server" visible="false">
												 <CTRL:SelectUsers ID="CTRLselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
												  DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="false" DefaultMaxPreviewItems="20" 
												  ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false"/>
											</div>
										</fieldset>

										<asp:Panel ID="PNLpresident" runat="server">
											<fieldset>
												<%--<div class="fieldrow fieldedition">
													<asp:Label ID="LBLpresidentDescription" runat="server">Testo descrittivo</asp:Label>
												</div>--%>
												<div id="box-buttons" class="fieldrow fieldstatusaction clearfix">
													<asp:LinkButton ID="LKBStatShowSubmission" runat="server" CssClass="Link_Menu">Mostra sottomissioni</asp:LinkButton>
													<asp:LinkButton ID="LKBStatHideSubmission" runat="server" CssClass="Link_Menu">Nascondi sottomissioni</asp:LinkButton>

													<asp:LinkButton ID="LKBStatStart" runat="server" CssClass="Link_Menu">Attiva la commissione</asp:LinkButton>

													<asp:LinkButton ID="LKBStatLockEvaluation" runat="server" CssClass="Link_Menu">Blocca valutazioni</asp:LinkButton>
													<asp:LinkButton ID="LKBStatUnLockEvaluation" runat="server" CssClass="Link_Menu">Riattiva le valutazioni</asp:LinkButton>

													<asp:LinkButton ID="LKBStatCheckEvaluation" runat="server" CssClass="Link_Menu">Convalida valutazioni</asp:LinkButton>

													<%--<asp:LinkButton ID="LKBStatDownload" runat="server" CssClass="Link_Menu">Scarica bozza verbale</asp:LinkButton>
													<asp:LinkButton ID="LKBStatUpload" runat="server" CssClass="Link_Menu">Allega verbale</asp:LinkButton>--%>
													
													<CTRL:FileUploader ID="CTRLfileUploader" runat="server" ViewTypeSelector="false" />
													<asp:linkbutton ID="LKBupload" runat="server" CssClass="linkMenu">Carica</asp:linkbutton>

													<CTRL:DisplayFile ID="CTRLdisplayFile" runat="server"/>
													<%--<span class="icons">
														<asp:Button ID="BTNremoveFile" runat="server" CssClass="icon delete" />
													</span>--%>

													<CTRL:UC_HTMLExport runat="server" id="UC_HTMLExport" ShowHideBtn="false" ShowDocx="false" ShowEditor="false" ShowPdf="false" ShowRtf="true" RTFText="Bozza verbale" />

													<asp:HyperLink ID="HYPevalSummary" runat="server" CssClass="Link_Menu">Valutazioni</asp:HyperLink>
													<asp:HyperLink ID="HYPsubmissionSummary" runat="server" CssClass="Link_Menu">Sottomissioni</asp:HyperLink>

													<%--<asp:Panel ID="PNLStatVerbale" runat="server">
														<asp:LinkButton ID="LKBStatVerbaleDownload" runat="server">Verbale</asp:LinkButton>
														<asp:Label ID="LBLstatVerbaleData" runat="server">12/08/2018</asp:Label>
													</asp:Panel>--%>
												</div>
											</fieldset>


										</asp:Panel>
									</asp:View>

									 <asp:View ID="VCriterion" runat="server">

										 <!-- @Start EDITOR -->
										<div class="treetop clearfix">                                            
											<span class="icons right" style="margin-top: 8px;">
												<asp:Button ID="BTNaddCriteria" runat="server" Text="A" CssClass="icon addcriteria" CommandName="addCriteria"/>
											</span>
											<div class="visibilitynav left">
												<asp:Label ID="LBcriteriaHideTop" cssclass="fieldsHide" runat="server">Hide Criteria</asp:Label>
												<asp:Label ID="LBcriteriaShowTop" cssclass="fieldsShow" runat="server">Show Criteria</asp:Label>
												<%--<asp:Label ID="LBcollapseAllTop" cssclass="collapseAll" runat="server">Collapse</asp:Label>
												<asp:Label ID="LBexpandAllTop" cssclass="expandAll" runat="server">Expand</asp:Label>--%>
											</div>
											<%--<div class="DivEpButton clearfix">
												<asp:Button ID="BTNaddCommitteeTop" runat="server" text="Add commission"/>
											</div>--%>
										</div>
										 <div class="sectioncontent">
										 <ul id="committee_<%=(Request("cnId") + "") %>" class="sections fieldsAdv criteriaAdv" style="margin-left:0;">
											<asp:Repeater ID="RPTcriteria" runat="server"><%-- OnItemDataBound="RPTcriteria_ItemDataBound" OnItemCommand="RPTcriteria_ItemCommand">--%>
												<ItemTemplate>
												<li class="cfield clearfix autoOpen" style="margin: .5em 0 1em 0;" id="criterion_<%#Container.DataItem.Id %>">
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
																<asp:TextBox ID="TXBcriterionName" runat="server" size="60" CssClass="itemname"></asp:TextBox>
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
																		
														</div>
													</div>
													<div class="clearer"></div>
												</li>
												</ItemTemplate>
											</asp:Repeater>
										</ul>
										</div>


									</asp:View>
									<asp:View ID="VSubmission" runat="server" >
										<fieldset>
											<div class="fieldrow fieldedition">
												<asp:LinkButton ID="LKBassignAll" runat="server" CssClass="Link_Menu">Assegna non assegnati</asp:LinkButton>
												<asp:Label ID="LBLassignAll" runat="server">Assegna alla commissione tutte le sottomissioni non assegnate.</asp:Label>
											</div>
										</fieldset>

									</asp:View>
								</asp:MultiView>





								<%--
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
																	
																</div>
															</div>
															<div class="clearer"></div>
														</li>     
														
													</ul>
													
													<div class="sectionfooter clearfix">
														<asp:HyperLink ID="HYPtoTopCommittee" runat="server" class="ui-icon ui-icon-arrowthickstop-1-n ui-icon-circle-arrow-n"></asp:HyperLink>
													</div>
													
												</div>
												
											</li>
									--%>
											
									<div style="display:none;">

									</div>
								<!-- @End EDITOR -->
							</div>
						</div>
						<div class="footer">
							<div class="DivEpButton">
								
								<asp:button ID="BTNsaveCommitteeBottom" runat="server" Text="Save"/>
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
								<CTRL:CTRLaddCriterion ID="CTRLaddCriterion" runat="server" AjaxEnabled="true" IsAdvance="True" />
							</div>                            
						</ContentTemplate>                        
					</asp:UpdatePanel>
					<div class="dialogfooter">
						<asp:Button ID="BTNcloseCreateCriterionWindow" runat="server" CausesValidation="false" />
						<asp:Button ID="BTNcreateCriterion" runat="server" CausesValidation="false" />
					</div>
				</div>
			</div>
			
			<script src="../../Jscript/tag-it-new/tag-it-new.js"></script>
		<link href="../../Graphics/Plugins/tagit/jquery.tagit.css" rel="Stylesheet" />
		<script type="text/javascript">
			$(function () { //aspetto il ready di jquery

				function getSuggestArr() { //funzione che ritorna l'array di autocomplate
					return (jQuery("#tagsinputSuggest").val() + "").split(',');  // prendo  i tag presenti in #tagsinputSuggest
				};

				$('input.tagsinputOnlyStored').tagit({ //inizializzazione di tagit su tutte le input con classe tagsinputOnlyStored
					autocomplete: { sourceFN: getSuggestArr, minLength: 0 },
					allowSpaces: true,
					showAutocompleteOnFocus:true,
					beforeTagAdded: function (event, ui) {
						if (getSuggestArr().indexOf(ui.tagLabel) < 0) {
							return false;
						}
						return true;
					}
				});

			});
		</script>

			<input type="hidden" id="HDNidCommittee" class="hiddencurrentsection" runat="server" />
<%--            <input type="hidden" class="hiddenselectedtype" id="HDNselectedType" runat="server" />--%>
		</asp:View>
	</asp:MultiView>
</asp:Content>
