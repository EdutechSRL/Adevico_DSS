<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="PublicSubmission.aspx.vb" Inherits="Comunita_OnLine.PublicSubmission" %>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputRequiredFile.ascx" TagName="CTRLrequiredFile" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_RenderField.ascx" TagName="CTRLrenderField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLreport" Src="~/Modules/CallForPapers/UC/UC_SubmissionExport.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="PrintDraft" src="~/Modules/CallForPapers/UC/UC_PrintDraft.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<CTRL:Header ID="CTRLheader" runat="server"  EnableScripts="true"/>

	<script type="text/javascript">
		<% = me.CTRLreport.GetControlScript(HDNdownloadTokenValue.ClientID) %>
	</script>
	<script type="text/javascript">
		$(function () {
			$(".fieldobject.checkboxlist").each(function () {
				if ($(this).find(".extraoption").size() > 0) {
					var $extraoption = $(this).find(".extraoption");
					var $textoption = $(this).find(".textoption");

					$extraoption.next("label").after($textoption);
					if ($extraoption) {
						if ($extraoption.is(":checked")) {
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
		});
	</script>
	<CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
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
							</ul>
						</div>
						<div class="right">
							<asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu"
								Visible="false"></asp:HyperLink>
							<asp:HyperLink ID="HYPmanage" runat="server" Text="Gestione bandi" CssClass="Link_Menu"
								Visible="false"></asp:HyperLink>
							<span class="icons large">
								<CTRL:CTRLreport ID="CTRLreport" runat="server" isContainer="false" />
								<CTRL:PrintDraft ID="CTRLprintDraf" runat="server" ButtonCssClass="icon export pdf"/>
							</span>
						</div>
					</div>
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
									<span id="Span1" class="maxchar" runat="server">
										<asp:Literal ID="LTmaxCharsrequest" runat="server"></asp:Literal>
										<span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
									</span>
								</span>
							</div>
							<div class="fieldrow fieldinput">
								<asp:Button ID="BTNaddRequest" runat="server" CommandName="addRequest"
									Text="Add" />
								<asp:Button ID="BTNundoRequest" runat="server" CommandName="cancelRequest" 
									Text="Undo" />
							</div>
						</div>
					</div>
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
								<asp:Label ID="LBcallDescriptionTitle" runat="server"></asp:Label></legend>
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
										<asp:Literal ID="LTattachmentsTitle" runat="server"></asp:Literal></legend>
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
										<asp:Literal ID="LTsectionTitle" runat="server"></asp:Literal></legend>
									<div class="sectiondescription">
										<asp:Literal ID="LTsectionDescription" runat="server"></asp:Literal>
									</div>
									<asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>"
										OnItemDataBound="RPTfields_ItemDataBound">
										<ItemTemplate>
											<CTRL:CTRLrenderField ID="CTRLrenderField" runat="server" />
										</ItemTemplate>
									</asp:Repeater>
								</fieldset>
							</ItemTemplate>
						</asp:Repeater>
						<asp:Repeater ID="RPTrequiredFiles" runat="server">
							<HeaderTemplate>
								<fieldset class="section collapsable">
									<legend>
										<asp:Literal ID="LTrequiredFilesTitle" runat="server"></asp:Literal></legend>
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
</asp:Content>