<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="Evaluate.aspx.vb" Inherits="Comunita_OnLine.EvaluateSubmission" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLreport" Src="~/Modules/CallForPapers/UC/UC_SubmissionExport.ascx" %>
<%@ Register Src="~/Modules/CallForPapers/Evaluate/UC/UC_inputCriterion.ascx" TagName="CTRLinputCriterion" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_RenderField.ascx" TagName="CTRLrenderField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	 <CTRL:Header ID="CTRLheader" runat="server" EnableSemiFixedScript="true" />
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.checkboxList.js"></script>
	<!--<link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
	<link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	
   <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.textVal.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery-semifixed.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>-->
	
	<script type="text/javascript">
		<% = me.CTRLreport.GetControlScript(HDNdownloadTokenValue.ClientID) %>
	</script>
    <style type="text/css">        
	.tagSelectorItem{
		padding: 2px 6px;
		border: 1px solid #ccc;
		background-color: #fff;
		-moz-box-shadow: inset 1px 1px 0 rgba(255, 255, 255, 0.5), 1px 1px 1px rgba(0, 0, 0, 0.3);
		-webkit-box-shadow: inset 1px 1px 0 rgba(255, 255, 255, 0.5), 1px 1px 1px rgba(0, 0, 0, 0.3);
		box-shadow: inset 1px 1px 0 rgba(255, 255, 255, 0.5), 1px 1px 0 rgba(0, 0, 0, 0.15);
		text-transform: uppercase;
		word-wrap: normal;
		white-space: normal;
		display: inline-block;
		cursor:pointer;
        padding-left: 12px;
        border-top-left-radius: 24px;
        border-bottom-left-radius: 24px;
	}
	.tagSelectorItem.active{
		background-color:#333;
		color:#fff;
	}
	.tagConteinerItem{
		display:none !important;
	}
	.tagConteinerItem.mostra{
		display:block !important;
	}

    div.boolCriteria
    {
        display: inline-block;
    }

    div.boolCriteria span input,
    div.boolCriteria span label
    {
        display: inline-block !important;
    }
</style>
	 <script type="text/javascript" language="javascript">
		 $(function () {


			 /* tagSelectorItem   */
			 var arrSel_tagContainer = [];
			 jQuery(".tagConteinerItem").addClass("mostra");
			 jQuery(".tagSelectorItem").click(function (e) {
				 var el = jQuery(this);
				 var tagId = el.attr("id");
                 var isVediTutti =  (tagId == "tag_Clear");
                 if(!isVediTutti){
				     var indexEl = arrSel_tagContainer.indexOf(tagId);
				     if (indexEl >= 0) {
					     arrSel_tagContainer.splice(indexEl, 1);
				     } else {
					     arrSel_tagContainer.push(tagId);
				     }
                 }
                 if(arrSel_tagContainer.length < 1 || isVediTutti){
			        jQuery(".tagConteinerItem").addClass("mostra");
				    jQuery(".tagSelectorItem").removeClass("active");
                    jQuery("#tag_Clear").addClass("active");
                    arrSel_tagContainer = [];
                 }else{
				    jQuery(".tagSelectorItem").removeClass("active");
				    jQuery(".tagConteinerItem").removeClass("mostra");
				    for (var i = 0; i < arrSel_tagContainer.length; i++) {
					     var thisTagId = arrSel_tagContainer[i];
					     var elTagTab = jQuery("#" + thisTagId);
					     elTagTab.addClass("active");
					     jQuery(".tagConteinerItem." + thisTagId).addClass("mostra");
				    }
                }
			 });



			 $(".fieldobject.checkboxlist").each(function () {
				 if ($(this).find(".extraoption").size() > 0) {
					 var $extraoption = $(this).find(".extraoption");
					 var $textoption = $(this).find(".textoption");

					 $extraoption.next("label").after($textoption);
					 if ($(this).find("input[type='checkbox']")) {
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
				 if ($(this).parents("span.extraoption").first().size()>0) {
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
		 function ReturnValue(e) {
			 return !e.hasClass("ClientMode");
		 }

		 function ShowNotification(message) {
			 jQuery.noticeAdd({
				 text: message,
				 stay: false,
				 keepOpen: false,
				 inEffectDuration: 0,
				 type: 'succes'
			 });
		 }

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

			 $(".persist-area").semiFixed();

			 $(".fieldobject.multiline .fieldrow.fieldinput").textVal({
				 textSelector: "textarea.textarea",
				 charAvailable: ".fieldinfo .maxchar .availableitems",
				 errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
				 charMax: ".fieldinfo .maxchar .totalitems"
			 });

			 $(".tabs").tabs({
				 collapsible: true,
				 selected: -1,
				 select: function (event, ui) {
					 $("#tabs-0").show();
				 },
				 show: function (event, ui) {
					 $("#tabs-0").hide();
				 }
			 });

			 $(".tabcloser").click(function () {
				 $(".tabs").tabs("select", -1);
			 });

			 /*
			 These are the defaults settings for the noticeAdd function

			 var defaults = {
			 inEffect:               {opacity: 'show'},      // in effect
			 inEffectDuration:       600,                    // in effect duration in miliseconds
			 stayTime:               3000,                   // time in miliseconds before the item has to disappear
			 text:                   '',                     // content of the item
			 stay:                   false,                  // should the notice item stay or not?
			 type:                   'notice'                // could also be error, succes
			 }

			 You can overwrite these defaults like this:

			 jQuery.noticeAdd({
			 text: 'This is a notification that you have to remove',
			 stay: true
			 });

			 */
			 $(".closer").click(function () {
				 $(".hideme").toggle();
				 $(this).toggleClass("up");
				 $(".projectDescription").toggleClass("height_limited");
			 });

			 $(".legendForClose").click(function () {
				 $(".hideme").toggle();
				 $(this).parent().find(".closer").toggleClass("up");
				 $(".projectDescription").toggleClass("height_limited");
			 });


			 $(".ReloadParent").live("click", function () {
				 if (window.opener != null)
					 window.opener.location.reload();
				 return ReturnValue($(this));
			 });
			 $(".CloseMe").live("click", function () {
				 window.close();
				 return ReturnValue($(this));
			 });
		 });
	</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	 <div class="contentwrapper edit clearfix">
		<div class="view compiled">
			<div class="persist-area">
				<div class="evaluationbar persist-header" id="DVevaluationTab" runat="server" visible="false">
					<div class="innerwrapper clearfix">
						<div class="tabs criteriatabs">
							<div class="criteriatabswrapper clearfix">
								<ul>
									<li><a href="#tabs-0" class="tabcloser">&nbsp;</a></li>
									<asp:Repeater id="RPTcriteriaTabs" runat="server">
										<ItemTemplate>
											<li class="tab">
												<a href="#tabs-<%#Container.DataItem.DisplayId%>" title="<%#Container.DataItem.Name%>">
													<asp:Label ID="LBcriterionStatus" runat="server" CssClass="statuslight"></asp:Label>
													<%#Container.DataItem.Name%>
												</a>
											</li>
										</ItemTemplate>
									</asp:Repeater>
								</ul>
							</div>
							<div class="noselectedtab" id="tabs-0">
								<asp:Label ID="LBcriterionSelect_t" runat="server">Seleziona un Criterio</asp:Label>
							</div>
							<asp:Repeater id="RPTcriteria" runat="server">
								<ItemTemplate>
									<div id="tabs-<%#Container.DataItem.DisplayId%>">
										<CTRL:CTRLinputCriterion ID="CTRLinputCriterion" runat="server" visible="false" />

										 <div class="fieldobject multiline" id="DVcomment" runat="server" Visible="false">
											<div class="fieldrow fielddescription">
												<asp:Label runat="server" ID="LBevaluationDescription" CssClass="description"></asp:Label>
											</div>
											<div class="fieldrow fieldinput">
												<asp:Label runat="server" id="LBcommentEvaluation_t" AssociatedControlID="TXBcomment" CssClass="fieldlabel">Text</asp:Label>
												<asp:TextBox runat="server" ID="TXBcomment" TextMode="multiline" CssClass="textarea"></asp:TextBox>
												<asp:Label runat="server" ID="LBcommentHelp" CssClass="inlinetooltip"></asp:Label>     
												<br/>
												<span class="fieldinfo ">
													<span class="maxchar" runat="server" id="SPNmaxCharsComment" >
														<asp:Literal ID="LTmaxCharsComment" runat="server"></asp:Literal>
														<span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
													</span>
													<asp:Label ID="LBerrorMessageComment" runat="server" Visible="false" cssClass="generic"></asp:Label>
												</span>        
											</div>
										</div>
									</div>
								</ItemTemplate>
							</asp:Repeater>
						</div>
						<div class="clearfix">&nbsp;</div>
						<div class="left">
							<asp:Label ID="LBevaluationInfo" runat="server" CssClass="extrainfo"></asp:Label>
						</div>
						<div class="commands right">
							<asp:Button ID="BTNsaveEvaluation" runat="server" CssClass="Link_Menu" />
							<asp:Button ID="BTNcompleteEvaluation" runat="server" CssClass="Link_Menu ReloadParent" />
						</div>
						<div class="clearfix">&nbsp;</div>
					</div>
				</div>
				 <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
				<div class="topbar" id="DVtopMenu" runat="server" visible="false">
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
									</span>
								</li>
							</ul>
						</div>
						<div class="right">
							<span class="icons large">
								<CTRL:CTRLreport ID="CTRLreport" runat="server" isContainer="false" />
							</span>
							<%--<asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu"
								Visible="false"></asp:HyperLink>
							<asp:HyperLink ID="HYPsubmissionsList" runat="server" Text="Gestione bandi" CssClass="Link_Menu"
								Visible="false"></asp:HyperLink>
							<span class="icons large">
							</span>--%>
						</div>
					</div>
				</div>
			<asp:MultiView ID="MLVsubmissionDisplay" runat="server">
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
				<asp:View ID="VIWsubmission" runat="server">

				<fieldset class="section collapsable cfpintro" id="FStagContainer" runat="Server">
					<legend>
						<span class="switchsection handle">&nbsp;</span>
						<span class="title">
							<asp:Label id="LBcallTags" runat="server">Filtri "Tag"</asp:Label>
						</span>    
					</legend>
					
					<asp:repeater ID="RPTtag" runat="server">
						<HeaderTemplate>
							<ul class="clearfix">
							    <li class="left" style="margin-right:6px;">
								    <span id="tag_Clear" class="tagSelectorItem active">Vedi tutti</span>
							    </li>
						</HeaderTemplate>
						<ItemTemplate>
							<li class="left" style="margin-right:6px;">
								<asp:literal id="LTtag" runat="server">
									<span id="{0}" class="tagSelectorItem">{1}</span>
								</asp:literal>
							</li>
							<%--{0} = tag
								{1} = name--%>
						</ItemTemplate>
						<FooterTemplate>
							</ul>
						</FooterTemplate>
					</asp:repeater>

				</fieldset>    


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
									</ItemTemplate>
								</asp:Repeater>
							</fieldset>
						</ItemTemplate>
					</asp:Repeater>
				</asp:View>
			</asp:MultiView>
			</div>
		</div>
	</div>
	<asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>