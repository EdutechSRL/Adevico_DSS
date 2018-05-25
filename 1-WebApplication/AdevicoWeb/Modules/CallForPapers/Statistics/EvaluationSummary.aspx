<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="EvaluationSummary.aspx.vb" Inherits="Comunita_OnLine.EvaluationSummary" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/Evaluate/UC/UC_DialogComment.ascx" TagName="Comment" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyNumber.ascx" TagName="CTRLfuzzyNumber" TagPrefix="CTRL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	 <CTRL:Header ID="CTRLheader" runat="server"  EnableTreeTableScript="true" EnableDropDownButtonsScript="true"   />
	 <link href="../../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
		<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.slider.extend.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.progressbar.js"></script>
<%-- <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
	<link href="../../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css" rel="Stylesheet" />
	<link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
	<link rel="stylesheet" href="../../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.progressbar.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.slider.extend.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>--%>
	<!-- EXPORT -->
	<script type="text/javascript">
		var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
		var CookieName = "<% = Me.CookieName %>";
		var DisplayMessage = "<% = Me.DisplayMessageToken %>";
		var DisplayTitle = "<% = Me.DisplayTitleToken %>";
	</script>
	<!--<style type="text/css">
		.ui-dialog .ui-dialog-titlebar-min{ position: absolute; right: 23px; top: 50%; width: 19px; margin: -10px 0 0 0; padding: 1px; height: 18px; }
		.ui-dialog .ui-dialog-titlebar-min span { display: block; margin: 1px; }
		.ui-dialog .ui-dialog-titlebar-min:hover, .ui-dialog .ui-dialog-titlebar-min:focus { padding: 0; }
	</style>-->
	<script type="text/javascript">
		var fileDownloadCheckTimer;
		function blockUIForDownload() {
			var token = new Date().getTime(); //use the current timestamp as the token value
			$("input[id='" + TokenHiddenFieldId + "']").val(token);
			$.blockUI({ message: DisplayMessage, title: DisplayTitle, draggable: false, theme: true });
			fileDownloadCheckTimer = window.setInterval(function () {
				var cookieValue = $.cookie(CookieName);
				if (cookieValue == token)
					finishDownload();
			}, 1000);
		}

		function finishDownload() {
			window.clearInterval(fileDownloadCheckTimer);
			$.cookie(CookieName, null); //clears this cookie value
			$.unblockUI();
		}
	</script>

	<!-- TEMPORARY - TO REMOVE-->
	 <script type="text/javascript" language="javascript">
		 /*var page=0;
		 var lastpage= 5;
		 var pagesize = 5;*/

		 $(function () {

			 $(".ddbuttonlist.enabled").dropdownButtonList();

			 $(".tree_table").treeTable({
				 clickableNodeNames: true,
				 initialState: "collapsed",
				 persist: false
			 });


			 $(".tree_table").data("page", 0);
			 //$(".tree_table").data("lastpage",5);     //  copy all of those from table attributes/class
			 $(".tree_table").data("pagesize", 5);     // egual for each? maybe yes


			 $(".tree_table").each(function () {
				 var n = $(this).find("th.evaluator").size() - $(this).data("pagesize");
				 if (n < 0) { n = 0; }
				 $(this).data("lastpage", n);
			 })



			 $(".progressbar").myProgressBar();

			 $(".tree_table").each(function () {

				 var page = $(this).data("page");
				 var pagesize = $(this).data("pagesize");
				 var lastpage = $(this).data("lastpage");

				 $(this).find(".slider.min").html(1 + page);
				 $(this).find(".slider.max").html(pagesize + page);
				 $(this).find(".slider.all").html(lastpage + pagesize);
			 });



			 $(".colslider").slider({
				 value: 0,
				 min: 0,
				 max: 10,
				 step: 1,
				 slide: function (event, ui) {
					 var $table = $(this).parents("table.tree_table");

					 var page = ui.value;
					 var pagesize = $table.data("pagesize");
					 var lastpage = $table.data("lastpage");



					 $table.data("page", page);

					 $table.find(".colslider").not($(this)).slider("value", page);

					 //$(".colslider").not($(this)).slider("value",page);
					 $table.find(".slider.min").html(1 + page);
					 $table.find(".slider.max").html(pagesize + page);
					 $table.find(".slider.all").html(lastpage + pagesize);


					 $table.find("tr").each(function () {
						 var $tr = $(this);
						 var $ths = $tr.children("th.evaluator");
						 var $tds = $tr.children("td.evaluator");

						 $ths.hide();
						 $tds.hide();
						 for (x = 0; x < pagesize; x++) {
							 $ths.eq(x + page).show();
							 $tds.eq(x + page).show();
						 }

					 });
				 }
				 //});
			 }).sliderAccess({
				 touchonly: false,
				 upIcon: 'ui-icon-triangle-1-e',
				 downIcon: 'ui-icon-triangle-1-w',
				 upText: "",
				 downText: ""
			 });

			 $(".ui-slider-access button").click(function () {
				 return false;
			 });

			 $(".colslider").each(function () {
				 $(this).slider("option", "max", $(this).parents("table.tree_table").data("lastpage"));
			 })


			 $(".tree_table tr").each(function () {
				 var $tr = $(this);
				 var $ths = $tr.children("th.evaluator");
				 var $tds = $tr.children("td.evaluator");

				 var $table = $(this).parents("table.tree_table");

				 var page = $table.data("page");
				 var pagesize = $table.data("pagesize");
				 var lastpage = $table.data("lastpage");

				 $ths.hide();
				 $tds.hide();

				 for (x = 0; x < pagesize; x++) {
					 $ths.eq(x + page).show();
					 $tds.eq(x + page).show();
				 }

			 });

			 var LastX = 0;
			 var LastY = 0;

			 $(".comment").click(function () {

				 $parents = $(this).parents("td");

				 var x = $(this).parents("td").find(".commentdialog").not(".pinned").not(".open");

				 x.dialog({
					 width: 500,
					 appendTo: $parents,
					 position: "center",
					 open: function () {
						 var $el = $(this).parent().find(".ui-dialog-titlebar");
						 //console.log( $parents.find(".ui-dialog-titlebar").size());
						 $el.find(".ui-dialog-titlebar-min").remove();
						 var $dialog = $(this);
						 $(this).addClass("open");
						 $(this).removeClass("pinned");

						 $el.append('<a href="#" class="dialog-minimize ui-dialog-titlebar-min ui-corner-all" role="button"><span class="ui-icon ui-icon-unlocked"></span></a>')
						 //$parents.find(".ui-dialog-titlebar").append(myIcon);
						 $(".ui-dialog-content.commentdialog").not(".pinned").not(this).dialog("close");
						 //$(".ui-dialog-titlebar-min").hover(function () { $(this).addClass("ui-state-hover") }, function () { $(this).removeClass("ui-state-hover") })
						 $(".ui-dialog-titlebar-min").click(function () {
							 $(this).children().toggleClass("ui-icon-unlocked").toggleClass("ui-icon-locked");
							 $dialog.toggleClass("pinned");
						 });
					 },
					 close: function () {
						 $(this).removeClass("pinned");
						 $(this).removeClass("open");
					 }
				 });

			 });

			 //$(".closecomments").hide();

			 $(".closecomments").click(function () {

				 $(".ui-dialog-content.commentdialog").dialog("close");
				 //$(".ui-dialog.commentdialog").dialog("close");
			 });

			 $(".dialog.legend").dialog("option", "width", 600);

			 $(".openlegend").click(function () {
				 var href = $(this).attr("href");
				 $(href).dialog("open");
				 return false;
			 });

		 });



	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="contentwrapper edit clearfix">
		<div class="viewbuttons clearfix">
			<div class="ddbuttonlist enabled" id="DVexport" runat="server"><!--
		--><asp:LinkButton ID="LNBexportCommitteesEvaluationsToCsv" runat="server" Text="*Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
		--><asp:LinkButton ID="LNBexportCommitteesEvaluationsOneColumnToCsv" runat="server" Text="*Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
		--><asp:LinkButton ID="LNBexportCommitteeEvaluationsToCsv" runat="server" Text="*Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
		--><asp:LinkButton ID="LNBexportCommitteeEvaluationsOneColumnToCsv" runat="server" Text="*Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
		--></div>
		</div>
	 <asp:MultiView id="MLVstatistics" runat="server" ActiveViewIndex="1">
		<asp:View ID="VIWempty" runat="server">
			<br /><br /><br /><br />
			<asp:Label ID="LBnoSubmission" runat="server"></asp:Label>
			<br /><br /><br /><br />
		</asp:View>
		<asp:View ID="VIWnoItems" runat="server">
			<br /><br /><br /><br />
			<asp:Label ID="LBnoEvaluations" runat="server"></asp:Label>
			<br /><br /><br /><br />
		</asp:View>
		<asp:View ID="VIWstatistics" runat="server">
			<div class="view">
				<div class="infobar">
					<h3><asp:Literal ID="LTsubmitterEvaluations_t" runat="server">*Submitter evaluations</asp:Literal></h3>
					<div class="fieldobject">
						<div class="fieldrow">
							<asp:Label ID="LBcallName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcallName">*Call:</asp:Label>
							<asp:Label ID="LBcallName" runat="server" CssClass="fieldtext"></asp:Label>
						</div>
						 <div class="fieldrow">
							<asp:Label ID="LBowner_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBowner_t">*Submitter:</asp:Label>
							<asp:Label ID="LBowner" runat="server" CssClass="fieldtext"></asp:Label>
						</div>
						<div class="fieldrow">
							<asp:Label ID="LBsubmittedOn_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsubmittedOn">*Submitted on:</asp:Label>
							<asp:Label ID="LBsubmittedOn" runat="server" CssClass="fieldtext"></asp:Label>
							<asp:Label ID="LBsubmittedBy_t" runat="server" CssClass="fieldlabel" Visible="false" AssociatedControlID="LBsubmittedBy"></asp:Label>
							<asp:Label ID="LBsubmittedBy" runat="server" CssClass="fieldtext" Visible="false"></asp:Label>
						</div>
						 <div class="fieldrow clearfix">
								<span class="fieldlabel">
									<asp:Literal ID="LTdisplayEvaluationsStatus_t" runat="server">*Evaluation:</asp:Literal>
								</span>
								<span class="status completion">
									<span class="statusitem" id="SPNsingleCommittee" runat="server">
										<asp:Label ID="LBcommitteeEvaluation" runat="server"></asp:Label>
									</span>
									<asp:Repeater ID="RPTevaluationStatus" runat="server">
										<ItemTemplate>
											<span class="statusitem"><span class="label">
												<asp:Literal ID="LTinCommittee" runat="server">*in</asp:Literal>
												<asp:Label ID="LBcommitteeName" runat="server"></asp:Label>
											</span>
												<asp:Label ID="LBcommitteeEvaluation" runat="server"></asp:Label>
											</span>
										</ItemTemplate>
									</asp:Repeater>
								</span>
							</div>
						<div class="fieldrow">
							<asp:Label ID="LBcommitteesList_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLcommittees">*View committee:</asp:Label>
							<asp:DropDownList ID="DDLcommittees" runat="server" CssClass="commissionsselect" AutoPostBack="true"></asp:DropDownList>
						</div> 
					</div>  
					<div class="fieldrow buttons clearfix">
						<asp:HyperLink ID="HYPviewSubmission" runat="server" CssClass="linkMenu" Visible="false" Target="_blank">*View submission</asp:HyperLink>
						<asp:HyperLink ID="HYPviewExtendedEvaluation" runat="server" CssClass="linkMenu" Visible="false">*View evaluation</asp:HyperLink>
					</div>
				</div>
				<CTRL:Messages ID="CTRLdssMessage"  runat="server" visble="false"/>
				<div class="visibilityNav">
					<asp:HyperLink ID="HYPcloseComments" runat="server" NavigateUrl="#" CssClass="closecomments" Text="Close comments"></asp:HyperLink>
				</div>
				<asp:Repeater ID="RPTcommittees" runat="server">
					<ItemTemplate>
						<table id="commission-<%#Container.DataItem.IdCommittee %>" class="tree_table evaluation user detail commissiondetail scrolltable <%#CssTableEvaluatorsCount(Container.DataItem) %>">
						<thead>
							<tr class="sliderrow">
								<th class="commissionmame empty">&nbsp;</th>
								<th class="points empty">&nbsp;</th>
								<th colspan="5" class="slidercell commissions">
									<span >
										<asp:Literal ID="LTevaluatorsSliderTop" runat="server"></asp:Literal>
										<div class="colslider" id="slider"></div>
										<span class="slider min"></span>-<span class="slider max"></span>/<span class="slider all"></span>
									</span>
								</th>
							</tr>
							<tr>
								<th class="commissionmame"><asp:Literal id="LTcommitteeName_t" runat="server"></asp:Literal></th>
								<th class="points"><asp:Literal id="LTsubmissionPointsSingle_t" runat="server"></asp:Literal></th>
								<asp:Repeater ID="RPTevaluators" runat="server" OnItemDataBound="RPTevaluators_ItemDataBound" DataSource="<%#Container.DataItem.Evaluators %>">
									<ItemTemplate>
										<th class="evaluator child-of-commission-<%#Container.DataItem.IdCommittee %>" id="THevaluator" runat="server"><asp:label ID="LBevaluatorName" runat="server"></asp:label></th>    
									</ItemTemplate>
								</asp:Repeater>
								<th class="evaluator empty" runat="server" visible="false" id="THevaluatorPlaceHolder"></th>
							</tr>
						</thead>
						<tbody>
							<tr class="commissionglobal">
								<td class="commissionmame"><%#Container.DataItem.Name%></td>
								<td class="points">
									<asp:Label ID="LBcommitteeRating" runat="server"></asp:Label>
									<span class="text" runat="server" visible="false" id="SPNfuzzy"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
								</td>
								<asp:Repeater ID="RPTevaluatorEvaluations" runat="server" DataSource="<%#Container.DataItem.Evaluators %>"  OnItemDataBound="RPTevaluatorEvaluations_ItemDataBound">
									<ItemTemplate>
										<td class="evaluator" id="TDevaluator" runat="server">
											<span class="vote " id="SPNvoteContainer" runat="server">
												<asp:Label ID="LBvote" runat="server" cssclass="number"></asp:Label>
												<asp:HyperLink ID="HYPvote" runat="server" CssClass="number" Visible="false" Target="_blank"><span class="text" runat="server" visible="false" id="SPNfuzzy"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span></asp:HyperLink>
												<span class="icons">
													<asp:Label ID="LBiconComment" runat="server" CssClass="icon comment" Visible="false">C</asp:Label>
												</span>
											</span>
											<CTRL:Comment id="CTRLcomment" runat="server" visible="false" />
											</td>    
									</ItemTemplate>
								</asp:Repeater>
								<td class="evaluator empty" runat="server" visible="false" id="TDevaluatorPlaceHolder"></td>
							</tr>
							<asp:Repeater ID="RPTcriteria" runat="server" DataSource="<%#Container.DataItem.Criteria %>" OnItemDataBound="RPTcriteria_ItemDataBound">
								<ItemTemplate>
									<tr id="criteria-<%#Container.DataItem.Id %>" class="criteria initialized <%#CssCriterion(Container.DataItem.DisplayAs) %>">
										<td class="criteria"><asp:literal ID="LTcriterionName" runat="server"></asp:literal></td>
										<td class="points"><asp:Label ID="LBcriteriaRating" runat="server"></asp:Label></td>
										<asp:Repeater ID="RPTcriteriaEvaluations" runat="server" DataSource="<%#Container.DataItem.Evaluations %>"  OnItemDataBound="RPTcriteriaEvaluations_ItemDataBound">
											<ItemTemplate>
												<td class="evaluator" id="TDcriterion" runat="server">
													<span class="vote " id="SPNvoteContainer" runat="server">
														<asp:Label ID="LBvote" runat="server" cssclass="number"></asp:Label>
														<span id="SPNfuzzyNumber" runat="server" visible="false" class="text"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
														<span class="icons">
															<asp:Label ID="LBiconComment" runat="server" CssClass="icon comment"  Visible="false">C</asp:Label>
														</span>
													</span>
													<CTRL:Comment id="CTRLcomment" runat="server" visible="false" />
													</td>    
											</ItemTemplate>
										</asp:Repeater>
										<td class="evaluator empty" runat="server" visible="false" id="TDcriteriaPlaceHolder"></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
						 </tbody>
						<tfoot>
							<tr class="sliderrow">
								<td colspan="2" class="empty">&nbsp;</td>
								<td colspan="5" class="slidercell">
									<span >
										<asp:Literal ID="LTevaluatorsSliderBottom" runat="server"></asp:Literal>
										<div class="colslider"></div>
										<span class="slider min">1</span>-<span class="slider max">5</span>/<span class="slider all">10</span>
									</span>
								</td>
							</tr>
						</tfoot>
					</table>
					</ItemTemplate>
				</asp:Repeater>
			</div>
		</asp:View>
	</asp:MultiView>
	<asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
	</div>
</asp:Content>