<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditCall.aspx.vb" Inherits="Comunita_OnLine.EditCall" MaintainScrollPositionOnPostback="true"  %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_EditField.ascx" TagName="CTRLeditField" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_AddField.ascx" TagName="CTRLAddField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_TextAreaEditorHeader.ascx" TagPrefix="CTRL" TagName="UC_TextAreaEditorHeader" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	 <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
	  <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
	 <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
	<script type="text/javascript">
		$(document).ready(function(){
			var inputCampiTabella = $(".inputtext.dinamycCols");
			
			DynamicTableColumns = function() {
				var _self = this;
				_self.inputTarget = null;
				_self.columns = [];
				_self.rootObj = null;
				_self.init = function(_inputTarget){
					_self.inputTarget = _inputTarget;
					_self.inputTarget.hide();
					_self.rootObj = $("<div style=\"display: table-cell;\">" +
										"<ul style=\"margin: 0 0 12px 0;padding: 0;\" class=\"column-list icons\"></ul>" + 
									 "</div>");
					_self.rootObj.insertBefore(_self.inputTarget);
					_self.drawColonne();
				};
				_self.drawColonne = function(){
					var value = _self.inputTarget.val();
					if(value)
						_self.columns = value.split('|');
					
					_self.rootObj.find("ul.column-list:first").html("");
					for	(var i=0; i < _self.columns.length; i++){
						var MyLi = $("<li class=\"column-list-item\" style=\"width:200px;margin-bottom:4px;\"></li>");
						var MyLiInput = $("<input type=\"text\" />");
						MyLiInput.addClass("");
						MyLiInput.val(_self.columns[i]);
						MyLiInput.on("keyup", _self.addKeyUpListener(i));
						MyLi.append(MyLiInput);
						_self.rootObj.find("ul.column-list:first").append(MyLi);
						// creo link
						var myLink = document.createElement('a');
						var linkText = document.createTextNode("x");
						myLink.appendChild(linkText);
						myLink.addEventListener('click', _self.removeColumn(i), true);
						myLink.className = "icon delete";
						myLink = $(myLink);
						MyLi.append(myLink);
					}
					var myLiNew = $("<li class=\"column-list-item\"><input class=\"new-item-input\" type=\"text\" value=\"\" /></li>")
					_self.rootObj.find("ul.column-list:first").append(myLiNew);
					var myLinkNew = document.createElement('a');
					var linkTextNew = document.createTextNode("+");
					myLinkNew.appendChild(linkTextNew);
					myLinkNew.addEventListener('click', _self.addColumn(), true);
					myLinkNew.className = "icon addfield";
					myLinkNew = $(myLinkNew);
					myLiNew.append(myLinkNew);
				};
				_self.addKeyUpListener = function(index){
					return function(e){							
						this.intAddColumn = function(index){
							var thisEl = $(e.target);
							var value = thisEl.val();							
							_self.columns[index] = value;
							_self.inputTarget.val(_self.columns.join("|"));
							thisel.focusin();
							_self.drawColonne();						
						};
						this.intAddColumn(index);
					}					
				};
				_self.addColumn = function(){
					return function(){							
						this.intAddColumn = function(){		
							var nameColumn = _self.rootObj.find('.new-item-input').val();
							var value = _self.inputTarget.val();
							if(value)
								value = value + "|" + nameColumn;
							else
								value = nameColumn;
							
							_self.inputTarget.val(value);
							_self.drawColonne();						
						};
						this.intAddColumn();
					}					
				};
				_self.removeColumn = function(index){
					return function(){							
						this.intRemoveColumn = function(index){
							if(index < _self.columns.length)
								_self.columns.splice(index, 1);
							_self.inputTarget.val(_self.columns.join("|"));
							_self.drawColonne();								
						};
						this.intRemoveColumn(index);
					}					
				};
			}
			
			inputCampiTabella.each(function(i, val){
				new DynamicTableColumns().init($(val));
			});
			
			
		});
	</script>
	 <script language="javascript" type="text/javascript">
		 $(document).ready(function () {
			 $('#addField').dialog({
				 appendTo: "form",
				 closeOnEscape: false,
				 autoOpen: false,
				 draggable: true,
				 modal: true,
				 title: "<%=AddFieldDialogTitle() %>",
				 width: 815,
				 height: 600,
				 minHeight: 400,
				 //                minWidth: 700,
				 zIndex: 1000,
				 open: function (type, data) {
					 //                $(this).dialog('option', 'width', 700);
					 //                $(this).dialog('option', 'height', 600);
					 //$(this).parent().appendTo("form");
					 $(".ui-dialog-titlebar-close", this.parentNode).hide();
				 }

			 });


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
			 return false;
		 }

		 function closeDialog(id) {
			 $('#' + id).dialog("close");
		 }
	</script>
		<asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
			<script language="javascript" type="text/javascript">
				$(function () {
					showDialog("addField");
				});
			</script>
		</asp:Literal>
	
	<style>
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

	
	<CTRL:UC_TextAreaEditorHeader runat="server" id="UC_TextAreaEditorHeader" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
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
								<asp:HyperLink ID="HYPpreviewCallTop" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
								<asp:button ID="BTNsaveEditorTop" runat="server" Text="Save"/>
								<asp:button ID="BTNsaveTagsTop" runat="server" Text="Aggiorna Tag" Visible="false"/>
							</div>
							<CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
						</div>
						<div class="contentouter">
							<div class="content">
								<!-- @Start EDITOR -->
								<div class="treetop clearfix">
									<div class="visibilitynav left">
										<asp:Label ID="LBfieldsHideTop" cssclass="fieldsHide" runat="server">Hide Fields</asp:Label>
										<asp:Label ID="LBfieldsShowTop" cssclass="fieldsShow" runat="server">Show Fields</asp:Label>
										<asp:Label ID="LBcollapseAllTop" cssclass="collapseAll" runat="server">Collapse</asp:Label>
										<asp:Label ID="LBexpandAllTop" cssclass="expandAll" runat="server">Expand</asp:Label>
									</div>
									<div class="DivEpButton clearfix">
										<asp:Button ID="BTNaddSectionTop" runat="server" text="Add section"/>
									</div>
								</div>
								<div class="tree">
									<a name="#section_0"></a>
									<asp:Repeater ID="RPTsections" runat="server">
										<HeaderTemplate>
											<ul class="sections playmode">
										</HeaderTemplate>
										<ItemTemplate>
											<li class="section clearfix autoOpen" id="section_<%#Container.DataItem.Id %>">
												<div class="externalleft">
													<asp:Label ID="LBmoveSection" cssclass="movesection" runat="server">M</asp:Label>
												</div>
												<div class="sectioncontent">
													<span class="switchsection handle">+</span>
													<div class="innerwrapper">
														<div class="internal clearfix">
															<span class="left">
																<a name="#section_<%#Container.DataItem.Id %>"></a>
																<input type="hidden" id="HDNdisplayOrderSection" runat="server" class="hiddendisplayordersection"/>
																<asp:Literal ID="LTidSection" runat="server" Visible="false"></asp:Literal>
																<asp:Label ID="LBsectionName_t" cssclass="title" runat="server">Section:</asp:Label>
																<asp:TextBox ID="TXBsectionName" runat="server" CssClass="itemname"></asp:TextBox>
															</span>
															<span class="right">
																<span class="icons">
																	<asp:Button ID="BTNcloneSection" runat="server" Text="*Clone" CssClass="icon copy" CommandName="clonesection"/>
																	<asp:Button ID="BTNaddField" runat="server" Text="A" CssClass="icon addfield" CommandName="addfield"/>
																	<asp:Button ID="BTNdeleteSection" runat="server" Text="D" CssClass="img_btn icon delete" CommandName="virtualDelete"/>
																</span>
															</span>
														</div>
													</div>	
													<div class="clearer"></div>
													<ul class="fields">
														<li class="sectiondesc clearfix autoOpen">
															<div class="externalleft"></div>
															<div class="fieldcontent">  
																<div class="fielddetails">
																	<div class="fieldobject">
																		<div class="fieldrow fielddescription">
																			<asp:Label ID="LBsectionDescription_t" CssClass="fieldlabel" runat="server" >Description:</asp:Label>
																			<asp:TextBox ID="TXBsectionDescription" runat="server" Columns="40" class="textarea" TextMode="MultiLine"></asp:TextBox>
																		</div>
																	</div>
																</div>
															</div>
															<div class="clearer"></div>
														</li>     
														<asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>" OnItemDataBound="RPTfields_ItemDataBound" OnItemCommand="RPTfields_ItemCommand">
															<ItemTemplate>
															<li class="cfield clearfix autoOpen" id="field_<%#Container.DataItem.Id %>">
																<div class="externalleft">
																	<asp:Label ID="LBmoveField" cssclass="movecfield" runat="server">M</asp:Label>
																</div>
																<div class="fieldcontent">
																	<span class="switchcfield handle">+</span>
																	<div class="internal clearfix">
																		<span class="left">
																			<a name="#field_<%#Container.DataItem.Id %>"></a>
																			<asp:Literal ID="LTidField" runat="server" Visible="false"></asp:Literal>
																			<asp:Label ID="LBfieldName_t" cssclass="title" runat="server">Field:</asp:Label>
																			<asp:TextBox ID="TXBfieldName" runat="server" CssClass="itemname"></asp:TextBox>
																			<asp:Label ID="LBfieldType" cssclass="type" runat="server"></asp:Label>
																		</span>
																		<span class="right">
																			<asp:Label ID="LBfieldMandatory_t" runat="server">Mandatory</asp:Label>
																			<input type="checkbox" id="CBXmandatory" class="mandatory" runat="server" />
																			<span class="icons">
																				<asp:Button ID="BTNcloneField" runat="server" Text="*Clone" CssClass="icon copy" CommandName="clonefield"/>
																				<asp:Button ID="BTNdeleteField" runat="server" Text="D" CssClass="icon delete" CommandName="virtualDelete"/>
																			</span>
																		</span>
																	</div>
																	<div class="fielddetails">
																		<input type="hidden" id="HDNsectionOwner" runat="server" class="hiddensort"/>
																		<input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorder"/>
																		<div class="fieldobject">
																			<CTRL:CTRLeditField ID="CTRLeditField" runat="server"  OnAddOption="AddOption" OnRemoveOption="RemoveOption" OnSetAsDefaultOption="SetAsDefaultOption" OnSaveDisclaimerType="SaveDisclaimerType"/>
																		</div>
																		<div class="fieldfooter">
																			<div class="choseselect clearfix">
																				 <div class="left">
																					<asp:Label ID="LBfieldSubmitters_t" runat="server" CssClass="fieldlabel" AssociatedControlID="SLBsubmitters"></asp:Label>
																					<select runat="server" id="SLBsubmitters" class="partecipants chzn-select" multiple tabindex="2">
																		
																					</select>
																				</div>
																				<div class="right">
																					<span class="icons">
																						<span class="icon selectall" title="All" runat="server" id="SPNfieldSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNfieldSelectNone">&nbsp;</span>
																					</span>
																				</div>
																			</div>
																		</div>
																	</div>
																</div><div class="clearer"></div>
															</li>
															</ItemTemplate>
														</asp:Repeater>
													</ul>
													
													<div class="sectionfooter clearfix">
														<asp:HyperLink ID="HYPtoTopSection" runat="server" class="ui-icon ui-icon-arrowthickstop-1-n ui-icon-circle-arrow-n"></asp:HyperLink>
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
								<asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
								<asp:button ID="BTNsaveEditorBottom" runat="server" Text="Save"/>
								<asp:button ID="BTNsaveTagsBottom" runat="server" Text="Aggiorna Tag" Visible="false"/>
							</div>
						</div>
					</div>
				</div>
				</div>
			</div>
			<div id="addField" style="display: none;">
				<div id="DVaddFieldTitle" class="addnewfield">
					 <div class="dialogheader">
						<asp:Label ID="LBaddFieldDialgoHeader" runat="server"></asp:Label>
					 </div>
					<asp:UpdatePanel ID="UDPaddField" runat="server" UpdateMode="Always">
						<ContentTemplate>
							<div class="dialogcontent clearfix">
								<CTRL:CTRLaddField ID="CTRLaddField" runat="server" AjaxEnabled="true" />
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<div class="dialogfooter">
						<asp:Button ID="BTNcloseCreateFieldWindow" runat="server" CausesValidation="false" />
						<asp:Button ID="BTNcreateField" runat="server" CausesValidation="false" />
					</div>
				</div>
			</div>

			<input type="hidden" id="HDNidSection" class="hiddencurrentsection" runat="server" />
<%--            <input type="hidden" class="hiddenselectedtype" id="HDNselectedType" runat="server" />--%>
		</asp:View>
	</asp:MultiView>
	<asp:Literal ID="LTupdateAccordian" runat="server" Visible="false">
	</asp:Literal>
	
	
	<!-- input con id statico, store dei tag condivisi -->
	<asp:textbox ID="tagsinputSuggest" ClientIDMode="Static" runat="server" data-role="tagsinput" CssClass="hide"></asp:textbox>
	
	
	
	<!-- inizio import per tag-it -->
	<script src="../../../Jscript/tag-it-new/tag-it-new.js"></script>
	<link href="../../../Graphics/Plugins/tagit/jquery.tagit.css" rel="Stylesheet" />
	<script type="text/javascript">
		$(function () { //aspetto il ready di jquery

			function getSuggestArr() { //funzione che ritorna l'array di autocomplate
				return (jQuery("#tagsinputSuggest").val() + "").split(',');  // prendo  i tag presenti in #tagsinputSuggest
			};

			$('input.tagsinput').tagit({ //inizializzazione di tagit su tutte le input con classe tagsinput
				autocomplete: { sourceFN: getSuggestArr },
				allowSpaces: true,
				afterTagAdded: function (evt, ui) {
					var _tagLabel = ui.tagLabel;
					var _SuggestArr = getSuggestArr();
					if (_SuggestArr.indexOf(_tagLabel) < 0) {
						_SuggestArr.push(_tagLabel);
						jQuery("#tagsinputSuggest").val(_SuggestArr.join());
					}
				}
			});

			$('input.tagsinputOnlyStored').tagit({ //inizializzazione di tagit su tutte le input con classe tagsinputOnlyStored
				autocomplete: { sourceFN: getSuggestArr },
				allowSpaces: true,
				beforeTagAdded: function (event, ui) {
					if (getSuggestArr().indexOf(ui.tagLabel) < 0) {
						return false;
					}
					return true;
				}
			});

		});
	</script>

</asp:Content>