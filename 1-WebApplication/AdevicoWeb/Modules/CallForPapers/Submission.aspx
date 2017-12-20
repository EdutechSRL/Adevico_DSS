<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
	CodeBehind="Submission.aspx.vb" Inherits="Comunita_OnLine.Submission" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputRequiredFile.ascx" TagName="CTRLrequiredFile" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_RenderField.ascx" TagName="CTRLrenderField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLreport" Src="~/Modules/CallForPapers/UC/UC_SubmissionExport.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
																												   
<%@ Register TagPrefix="CTRL" TagName="FileUploader" Src="~/Modules/Repository/UC/UC_CompactInternalFileUploader2.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayFile" Src="~/Modules/Repository/UC/UC_ModuleRepositoryAction.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="PrintDraft" src="~/Modules/CallForPapers/UC/UC_PrintDraft.ascx" %>
<%@ Register Src="~/Modules/CallForPapersAdv/UC/Uc_Integration.ascx" TagPrefix="CTRL" TagName="Integration" %>
<%@ Register Src="~/Modules/CallForPapersAdv/UC/Uc_AdvComments.ascx" TagPrefix="CTRL" TagName="CTRLcomments" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<CTRL:Header ID="CTRLheader" runat="server" EnableScripts="true" />
	<link href="../../Graphics/Modules/CallForPapers/css/callforpeaper2017.css" rel="stylesheet" />
	<script type="text/javascript">
		<% = me.CTRLreport.GetControlScript(HDNdownloadTokenValue.ClientID) %>
	</script>
	<script type="text/javascript">
		$(function () {
			
<%--			jQuery("#<%=LKBupload.ClientID%>").click(function (e) {
				if (jQuery("#<%=PNLSigns.ClientID%> input[type='file']:first").val() + "" == "") {
					e.preventDefault();
					e.stopPropagation();
					return;
				}
				jQuery("body").append('<div class="overleo"><div style=\"margin-top:25%;\">Please wait, loading...</div></div>');
			});--%>
			
			$(".fieldobject.checkboxlist").each(function () {
				if ($(this).find(".extraoption").size() > 0) {
					var $extraoption = $(this).find(".extraoption");
					var $textoption = $(this).find(".textoption");
					$extraoption.next("label").after($textoption);
					if ($extraoption) {
						if ($extraoption.is(":checked") || ($textoption.find(".extraoption input[type='checkbox']") && $extraoption.find(".extraoption input[type='checkbox']").is(":checked"))) {
							$textoption.find("input").attr("disabled", false);
							$textoption.removeClass("disabled");
						} else {
							$textoption.find("input").attr("disabled", true);
							$textoption.addClass("disabled");
						}
					}
				}
			});
			$(".fieldobject.radiobuttonlist").each(function () {
				if ($(this).find(".extraoption").size() > 0) {
					var $extraoption = $(this).find(".extraoption input[type='radio']");
					var $textoption = $(this).find(".textoption");

					$extraoption.next("label").after($textoption);
					if ($(this).find("input[type='radio']")) {
						if ($(this).is(":checked")) {
							$textoption.find("input").attr("disabled", false);
							$textoption.removeClass("disabled");
						} else {
							$textoption.find("input").attr("disabled", true);
							$textoption.addClass("disabled");
						}
					}
				}
			});
			$(".fieldobject.radiobuttonlist input[type='radio']").change(function () {

				if ($(this).parents("span.extraoption").first().size() > 0) {
					var $textoption = $(this).parents(".radiobuttonlist").first().find(".textoption");
					$textoption.find("input").attr("disabled", false);
					$textoption.removeClass("disabled");
				} else {
					var $textoption = $(this).parents(".radiobuttonlist").first().find(".textoption");
					$textoption.find("input").attr("disabled", true);
					$textoption.addClass("disabled");
				}
			});

			$(".fieldobject.checkboxlist input[type='checkbox']").change(function () {

				if ($(this).is(".extraoption")) {
					var ischecked = $(this).is(":checked");
					var $textoption = $(this).parents(".checkboxlist").first().find(".textoption");
					$textoption.find("input").attr("disabled", !ischecked);
					$textoption.toggleClass("disabled");
				}
			});
		});
		$(function () {
			$("fieldset.section.collapsed").each(function () {
				var $fieldset = $(this);
				var $legend = $fieldset.children().filter("legend");
				var $children = $fieldset.children().not("legend");
				$children.toggle();
			});

			$("fieldset.section.collapsable legend").click(function () {
				var $legend = $(this);
				var $fieldset = $legend.parent();
				var $children = $fieldset.children().not("legend");
				$children.toggle();
				$fieldset.toggleClass("collapsed");
			});

			$(".persist-area").semiFixed()
			$(".fieldobject.checkboxlist").checkboxList({
				listSelector: "span.inputcheckboxlist",
				errorSelector: ".fieldrow.fieldinput label",
				checkOnStart: true,
				error: {
					min: ".minmax .min",
					max: ".minmax .max"
				}
			});

			$(".fieldobject.disclaimer.custom").checkboxList({
				listSelector: "span.inputcheckboxlist",
				errorSelector: "self",
				checkOnStart: true,
				error: {
					min: ".minmax .min",
					max: ".minmax .max"
				}
			});

			$(".fieldobject.singleline .fieldrow.fieldinput").textVal({
				textSelector: "input.inputtext",
				charAvailable: ".fieldinfo .maxchar .availableitems",
				errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
				charMax: ".fieldinfo .maxchar .totalitems"

			});

			$(".fieldobject.multiline .fieldrow.fieldinput").textVal({
				textSelector: "textarea.textarea",
				charAvailable: ".fieldinfo .maxchar .availableitems",
				errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
				charMax: ".fieldinfo .maxchar .totalitems"
			});

			/* mostra e nascondi commenti */
			if($(".box-msg-submitter").size() > 0 || $(".box-msg-secretary").size() > 0){    
				$(".linkopenclose.open").click(function(){
					$(".box-msg-secretary,.box-msg-submitter").fadeIn();
			
				});
				$(".linkopenclose.close").click(function(){
					$(".box-msg-secretary,.box-msg-submitter").fadeOut();
				});
			}else{
				 $(".linkopenclose.open, .linkopenclose.close").hide();
			}

			//Mostra nascondi
			$(".hideShowMain .btn-show-cmmt").click(function(){
				$(".btn-show-cmmt").addClass("hide");
				$(".btn-hide-cmmt").removeClass("hide");
				$(".comment.container.openclose").removeClass("hide");
			});    
			$(".hideShowMain .btn-hide-cmmt").click(function(){
				$(".btn-show-cmmt").removeClass("hide");
				$(".btn-hide-cmmt").addClass("hide");
				$(".comment.container.openclose").addClass("hide");
			});
		});
	</script>
	  <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
	
	<style>
		.persist-area > .topbar {
			max-height: 300px;
			overflow: auto;
		}
		.persist-area > .topbar.floatingHeader{
			box-shadow: 0 4px 8px rgba(0,0,0,.4);
			max-height: 30%;    
		}
		div.commentContainer
		{

		}
		ul.comment.container{
			padding:8px 12px;
			background-color:#fff;
			margin-top:0;
		}
		div.commentContainer li.comment
		{
			line-height:18px;
			border-bottom:solid 1px #cecece;
			margin-bottom:6px;
			padding-bottom:3px;
		}

		div.commentContainer li.comment span.icon
		{

		}

		div.commentContainer li.comment span.member
		{
			font-weight: bold;
		}
		.hideShowMain span:not(.hide){
			background-color:#fff;
			padding: 2px 6px;
			display:inline-block;
			line-height:20px;
			border:none;
			margin-top:6px;
			cursor:pointer;
		}
		span.btn-show-cmmt{
			border-bottom:solid 1px #cecece !important;
		}
		li.comment .icon.confirm{
			background-color: #99c74a;
			border-radius:50%;
			display:inline-block;
			text-indent:-10000px;
			width:12px;
			height:12px;
			line-height:12px;
		}
		li.comment span.Comment{
			background-color: #aaa;
			border-radius: 4px;
			padding: 2px 4px;
			color: #fff;
			display: inline-block;
			vertical-align: bottom;
			line-height: 14px;
		}
		
		li.comment span.date{
			color:#999;
		}
		
		.overleo{
			position:fixed;
			z-index:1231;
			left:20px;
			top:20px;
			right:20px;
			bottom:20px;
			background-color:#fff;
			background-color:rgba(255,255,255,.8);
			border: solid 1px #cecece;
			color:#333;
			box-shadow: 0 2px 4px rgba(0,0,0,.2);
			font-size:40px;
			text-align:center;
		}
	
	</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="contentwrapper edit clearfix">
		<div class="view compiled">
			<div class="persist-area">
				<div class="topbar persist-header" id="DVtopMenu" runat="server" visible="false">
					<div class="innewrapper">
						<CTRL:Messages ID="CTRLerrorMessages"  runat="server" />
					</div>
					<div class="innerwrapper clearfix">
						<div class="clearfix">
							<div class="left">
								<ul class="sumbmissiondetails">
									<li class="submitter">
										<asp:Literal ID="LTowner_t" runat="server"></asp:Literal>&nbsp;<asp:Label ID="LBowner"
											runat="server" /></li>
									<li class="submittertype">
										<asp:Literal ID="LTsubmitterType_t" runat="server"></asp:Literal>&nbsp;<asp:Label
											ID="LBsubmitterType" runat="server" /></li>
									<li class="status">
										<asp:Literal ID="LTsubmissionStatus_t" runat="server"></asp:Literal>&nbsp;<asp:Label
											ID="LBsubmissionStatus" runat="server"></asp:Label></li>
									<li class="submissiondate" id="LIsubmissionInfo" runat="server" visible="false">
										<asp:Literal ID="LTsubmittedOn_t" runat="server"></asp:Literal>&nbsp;
										<asp:Label ID="LBsubmittedOnData" runat="server" CssClass="date" />&nbsp;<asp:Label
											ID="LBsubmittedOnTime" runat="server" CssClass="time" />
										<span class="submittedby" runat="server" id="SPNsubmittedBy">&nbsp;<asp:Literal ID="LTsubmittedBy_t"
											runat="server"></asp:Literal>&nbsp;
											<asp:Label ID="LBsubmittedBy" runat="server" />
										</span></li>
									<li class="submissionrevisions" id="LIrevisions" runat="server" visible="false">
										<asp:Literal ID="LTrevisionListTitle_t" runat="server">Revisions:</asp:Literal>&nbsp;
										<asp:DropDownList ID="DDLrevisions" runat="server" CssClass="revisionhistory" AutoPostBack="true">
										</asp:DropDownList>
									</li>
								</ul>
							</div>
							<div class="right">
								<asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu"
									Visible="false"></asp:HyperLink>
								<asp:HyperLink ID="HYPsubmissionsList" runat="server" Text="Gestione bandi" CssClass="Link_Menu"
									Visible="false"></asp:HyperLink>
                                
								<span class="icons large">
									<CTRL:CTRLreport ID="CTRLreport" runat="server" isContainer="false" />
									<CTRL:PrintDraft ID="CTRLprintDraf" runat="server" ButtonCssClass="icon export pdf"/>
									<span class="icon separator" id="SPNmanage" runat="server">&nbsp;</span>
									<asp:Button ID="BTNaccept" CssClass="icon accept" runat="server" CommandName="accept"
										Visible="false" ToolTip="Accept" />
									<asp:Button ID="BTNrefuse" CssClass="icon refuse" runat="server" CommandName="refuse"
										Visible="false" ToolTip="Refuse" />
									<asp:Button ID="BTNsubmitForUser" CssClass="icon submitFor" runat="server" CommandName="submit"
										Visible="false" ToolTip="Submit for user" />
									<asp:Button ID="BTNreview" CssClass="icon requestreview" runat="server" CommandName="review" Visible="false" ToolTip="Review" />
								</span>
                                <asp:Panel ID="PNLsendIntegration" runat="server" Visible="false">
                                    <asp:Label ID="LBsendItegrationEnd_t" runat="server" AssociatedControlID="RDPintegrationEnd" CssClass="Titolo_campo alignr first">Revisione entro il:</asp:Label>
                                    <telerik:RadDateTimePicker id="RDPintegrationEnd" runat="server" >
                                        <TimeView
                                            Interval="00:30:00"
                                            Columns="4"
                                            Culture="it-IT"
                                            >
                                        </TimeView>

                                    </telerik:RadDateTimePicker>
                                    <asp:LinkButton ID="LKBsendIntegration" CssClass="Link_Menu" runat="server" >Notifica integrazioni</asp:LinkButton>
                                </asp:Panel>
							</div>
						</div>
						<CTRL:CTRLcomments runat="server" id="Uc_AdvComments" />
					</div>

					 <asp:panel ID="PNLSigns" runat="server">
						<div class="innerwrapper clearfix signcontainer">
							<div class="signTitle">
								<asp:Label runat="server" ID="LBL_Sign_t" CssClass="title">*Controfirma</asp:Label>
								<br/>
								<asp:Label runat="server" ID="LBL_Sign" CssClass="fieldDescription">Controfirma per...</asp:Label>
							</div>
							<div class="signContainer">
								<asp:multiview id="MLVsign" runat="server" Visible="false">
									<asp:View runat="server" ID="VnotSigned">
										
										<asp:Label runat="server" id="LBnotSigned_t" >*Prevista controfirma, ma non allegata</asp:Label>    
									</asp:View>
									<asp:View runat="server" ID="VtoSubmit">
										<asp:Label runat="server" id="LBtoSubmit_t">*Stampare, controfirmare, allegare...</asp:Label>
										<asp:linkbutton id="LKBprintForSign" runat="server" Text="Stampa sottomissione" CssClass="linkMenu"></asp:linkbutton>
										<CTRL:FileUploader ID="CTRLfileUploader" runat="server" ViewTypeSelector="false" />
										
										<asp:linkbutton ID="LKBupload" runat="server" CssClass="hide" >*Carica</asp:linkbutton>
									</asp:View>
									<asp:View runat="server" ID="Vsubmitted">
										<asp:Label runat="server" id="LBsubmitted">*Controfirma allegata:</asp:Label>    
										<CTRL:DisplayFile ID="CTRLdisplayFile" runat="server"/>
									</asp:View>

								</asp:multiview>
							</div>
						</div>
					</asp:panel>

					<div class="revisionsettings innerwrapper clearfix" runat="server" visible="false" id="DVrevision">
						<div class="fieldobject multiline">
							<div class="fieldrow fieldinput" id="DVdeadline" runat="server" visible="false">
								<asp:Label ID="LBdeadline_t" runat="server" AssociatedControlID="RDPdeadline">Entro il:</asp:Label>
								<telerik:raddatetimepicker id="RDPdeadline" runat="server">
								</telerik:raddatetimepicker>
							</div>
							<div class="fieldrow fieldinput">
								<asp:Label ID="LBrequestReason_t" runat="server" AssociatedControlID="TXBreason">Motivazione:</asp:Label>
								<asp:TextBox runat="server" ID="TXBreason" TextMode="multiline"
									CssClass="textarea"></asp:TextBox>
								<asp:Label runat="server" ID="LBreasonHelp" CssClass="inlinetooltip"></asp:Label>
								<br />
								<span class="fieldinfo ">
									<span class="maxchar" runat="server">
										<asp:Literal ID="LTmaxCharsrequest" runat="server"></asp:Literal>
										<span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
									</span>
								</span>
							</div>
							<div class="fieldrow fieldinput">
								<asp:Button ID="BTNaddRequest" runat="server" CommandName="addRequest"
									Text="Add" CssClass="Link_Menu" />
								<asp:Button ID="BTNundoRequest" runat="server" CommandName="cancelRequest" 
									Text="Undo" CssClass="Link_Menu"/>
							</div>
						</div>
					</div>
				
					<%--Todo: elenco commenti--%>
				</div>
				<div class="ShoHideButton">
					<asp:HyperLink ID="HYPopen" runat="server" CssClass="linkopenclose open">Mostra integrazioni</asp:HyperLink>
					<asp:HyperLink ID="HYPclose" runat="server" CssClass="linkopenclose close">Nascondi integrazioni</asp:HyperLink>
				</div>
				<div id="DVmessages" class="messages" runat="server" visible="false">
					<asp:MultiView ID="MLVpendingMessage" runat="server" ActiveViewIndex="0">
						<asp:View runat="server" ID="VIWpendingEmpty">
							
						</asp:View>
						<asp:View runat="server" ID="VIWpendingUser">
							<div class="message alert">
								<div class="revisionalert clearfix">
									<asp:Label ID="LBrequiredRevision_t" CssClass="revisionrequested" runat="server">
										La tua richiesta di Revisione non è stata ancora accettata. Vuoi annullare?
									</asp:Label>
									<div class="DivEpButton big">
										<asp:HyperLink ID="HTPreviewUserSubmission" runat="server" Visible="false" CssClass="Link_Menu">R</asp:HyperLink>
										<asp:HyperLink ID="HTPviewUserRequest" runat="server" Visible="false" CssClass="Link_Menu">V</asp:HyperLink>
										<asp:Button ID="BTNcancelUserRequest" runat="server" CssClass="Link_Menu" Visible="false"/>
										<asp:Button ID="BTNrefuseUserRequest" runat="server" CssClass="Link_Menu"  Visible="false"/>
										<asp:Button ID="BTNacceptUserRequest" runat="server" CssClass="Link_Menu" Visible="false"/>
									</div>
								</div>
							</div>
							<br />
						</asp:View>
						<asp:View runat="server" ID="VIWpendingManager">
							<div class="message alert">
								<div class="revisionalert clearfix">
									<span class="revisionneeded">
										<asp:Literal ID="LTrevisionRequired_t" runat="server">E' stata richiesta una revisione</asp:Literal>
										
										<span class="revisionapplicant">
											<asp:Literal ID="LTrevisionRequiredBy_t" runat="server">da</asp:Literal>
											<asp:label ID="LBrevisionRequiredBy" runat="server" CssClass="name"></asp:label>
										</span>
									</span>
									<asp:Label ID="LBrevisionMessage" runat="server" cssclass="revisionmsg clearfix"></asp:Label>
									<span class="revisiondate">
										<span class="revisiondeadline">
											<asp:Literal ID="LTdeadline_t" runat="server">entro il</asp:Literal>
											<asp:Label ID="LBdeadlineDate" runat="server" CssClass="date"></asp:Label>
										</span>
									</span>
									<div class="DivEpButton big">
										<asp:HyperLink ID="HTPreviewManagerSubmission" runat="server" CssClass="Link_Menu" Visible="false">R</asp:HyperLink>
										<asp:Button ID="BTNcancelManagerReview" runat="server" CssClass="Link_Menu" Visible="false"/>
									</div>
								</div>
							</div>
							<br />
						</asp:View>
					</asp:MultiView>
					<asp:MultiView ID="MLVrevisionInfo" runat="server" ActiveViewIndex="0">
						<asp:View runat="server" ID="VIWinfoEmpty">
							
						</asp:View>
						<asp:View runat="server" ID="VIWinfo">
							<div class="message info">
								<div class="revisionalert clearfix">
									<span class="revisionneeded">
										<asp:Literal ID="LTrevisionRequiredInfo_t" runat="server">E' stata richiesta una revisione</asp:Literal>
										
										<span class="revisionapplicant">
											<asp:Literal ID="LTrevisionRequiredByInfo_t" runat="server">da</asp:Literal>
											<asp:label ID="LBrevisionRequiredByInfo" runat="server" CssClass="name"></asp:label>
										</span>
									</span>
									<asp:Label ID="LBrevisionMessageInfo" runat="server" cssclass="revisionmsg clearfix"></asp:Label>
									<span class="revisionconfirm">
										<span class="revisionmessage">
											<asp:Literal ID="LTrevisionStatus" runat="server"></asp:Literal>
											<span class="revisionapplicant">
												<asp:Literal ID="LTrevisionManagedByInfo_t" runat="server">da</asp:Literal>
												<asp:label ID="LBrevisionManagedByInfo" runat="server" CssClass="name"></asp:label>
											</span>
										</span>
										<span class="revisiondate">
											<asp:Literal ID="LTrevisionDate_t" runat="server">il</asp:Literal>
											<asp:Label ID="LBrevisionDate" runat="server" cssclass="date"></asp:Label>
										</span>
									</span>
								</div>
							</div>
							<br />
						</asp:View>
					</asp:MultiView>
				</div>
				<asp:MultiView ID="MLVpreview" runat="server">
					<asp:View ID="VIWempty" runat="server">
						<br />
						<br />
						<br />
						<br />
						<asp:Label ID="LBemptyMessage" runat="server"></asp:Label>
						<br />
						<br />
						<br />
						<br />
					</asp:View>
					<asp:View ID="VIWcall" runat="server">
						<fieldset class="section collapsable cfpintro collapsed" runat="server" id="FLDcallInfo" visible="false">
							<legend>
								<span class="switchsection handle">&nbsp;</span>
								<span class="title">
									<asp:Label ID="LBcallDescriptionTitle" runat="server"></asp:Label>
								</span>
								</legend>
							<div class="cfpdescription">
								<div class="renderedtext"><asp:Literal ID="LTcallDescription" runat="server" /></div>
							</div>
							<div class="cfpdetails">
								<span class="expiration">
									<asp:Label ID="LBtimeValidity_t" runat="server">Validità</asp:Label>
									<asp:Label ID="LBstartDate" CssClass="startdate" runat="server"></asp:Label>&nbsp;-&nbsp;
									<asp:Label ID="LBendDate" CssClass="enddate" runat="server"></asp:Label>
								</span>
								<asp:Label runat="server" ID="LBwinnerinfo" class="winnerinfo" Visible="false"></asp:Label>
							</div>
						</fieldset>
						<asp:Repeater ID="RPTattachments" runat="server">
							<HeaderTemplate>
								<fieldset class="section collapsable attachments collapsed">
									<legend>
										<span class="switchsection handle">&nbsp;</span>
										<span class="title">
											<asp:Literal ID="LTattachmentsTitle" runat="server"></asp:Literal>
										</span>
										</legend>
									<div class="fieldobject">
										<div class="fieldrow">
											<ul class="attachedfiles">
							</HeaderTemplate>
							<ItemTemplate>
								<li class="attachedfile">
									<CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false"  />
									<div class="cfpdescription" runat="server" id="DVdescription" visible="false">
										<asp:Label ID="LBattachmentDescription" runat="server"></asp:Label>
									</div>
								</li>
							</ItemTemplate>
							<FooterTemplate>
								</ul> </div> </div> </fieldset>
							</FooterTemplate>
						</asp:Repeater>
						<asp:Repeater ID="RPTsections" runat="server">
							<ItemTemplate>
								<fieldset class="section collapsable">
									<legend>
										<span class="switchsection handle">&nbsp;</span>
										<span class="title">
											<asp:Literal ID="LTsectionTitle" runat="server"></asp:Literal>
										</span>
										</legend>
									<div class="sectiondescription">
										<asp:Literal ID="LTsectionDescription" runat="server"></asp:Literal>
									</div>
									<asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>"
										OnItemDataBound="RPTfields_ItemDataBound">
										<ItemTemplate>
											<CTRL:CTRLrenderField ID="CTRLrenderField" runat="server" />
											<CTRL:Integration runat="server" id="CTRLIntegration" />
										</ItemTemplate>
									</asp:Repeater>
								</fieldset>
							</ItemTemplate>
						</asp:Repeater>
						<asp:Repeater ID="RPTrequiredFiles" runat="server">
							<HeaderTemplate>
								<fieldset class="section collapsable">
									<legend>
										<span class="switchsection handle">&nbsp;</span>
										<span class="title">
											<asp:Literal ID="LTrequiredFilesTitle" runat="server"></asp:Literal>
										</span>
										</legend>
									<div class="sectiondescription">
										<asp:Literal ID="LTrequiredFilesDescription" runat="server"></asp:Literal>
									</div>
							</HeaderTemplate>
							<ItemTemplate>
								<CTRL:CTRLrequiredFile ID="CTRLrequiredFile" runat="server" />
							</ItemTemplate>
							<FooterTemplate>
								</fieldset>
							</FooterTemplate>
						</asp:Repeater>
					</asp:View>
				</asp:MultiView>
			</div>
		</div>
	</div>
	<asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
	<div class="hide">
		<asp:Literal ID="LTintegrationEndOn" runat="server">lunedì 13 novembre 2017 ore 12.00</asp:Literal>
	</div>
</asp:Content>