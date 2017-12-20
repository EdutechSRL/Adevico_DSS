<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="SubmissionsToEvaluate.aspx.vb" Inherits="Comunita_OnLine.SubmissionsToEvaluateList" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyNumber.ascx" TagName="CTRLfuzzyNumber" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<CTRL:Header ID="CTRLheader" runat="server"  EnableDropDownButtonsScript="true" EnableTreeTableScript="true" />
		<link href="../../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.progressbar.js"></script>
	<script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.slider.extend.js"></script>
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
	<style type="text/css">
		.label-state:before{            
			content: ' ';
			display: inline-block;
			height: 10px;
			width: 10px;
			margin-right: 6px;
			border-radius:50%;
		} 
		.label-state.state_0:before{
			background-color: #ccc;
		}
		.label-state.state_1:before{
			background-color: #99c74a;
		}
		.label-state.state_2:before{
			background-color: #ffcd40;
		}
		.label-state.state_3:before{
			background-color: #c74a4a;
		}


        .label-state.stateBool_0:before{
			background-color: #c74a4a;
		}
		.label-state.stateBool_1:before{
			background-color: #99c74a;
		}

        span.statusitem span.blue
        {
            /*font-size: 1em;
	        min-width: 24px;
            padding: 1px 3px 1px 3px;
            text-transform: none;
            text-decoration: none;
	        width: auto;
            font-weight: bold;
            display: inline-block;
            text-align: center;*/
            border-radius: 2px;
            display: inline-block;
            padding: 1px 3px 1px 3px;
            box-shadow: inset 1px 1px 3px rgba(0,0,0,0.3);
            text-shadow: 0 1px 0 rgba(255,255,255,0.5);
            background-color: #4a9ac7;
            color: #fff;
            width: 24px;
            min-width: 24px;
            text-align: center;
        }

        div.globalwrapper span.commissionname
        {
            width: 53%;
        }
	</style>
	<script type="text/javascript">
		var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
		var CookieName = "<% = Me.CookieName %>";
		var DisplayMessage = "<% = Me.DisplayMessageToken %>";
		var DisplayTitle = "<% = Me.DisplayTitleToken %>";
	</script>
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
			 //             $(".tree_table").treeTable({
			 //                 clickableNodeNames: true,
			 //                 initialState: "collapsed",
			 //                 persist: false
			 //             });


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

			 $(".comment").click(function () {
				 var x = $(this).parents("td").find(".commentdialog");
				 if (x.data("dialog") != "open") {
					 x.data("dialog", "open");
					 var clone = x.clone();

					 clone.dialog({
						 close: function () {
							 x.removeData("dialog");
						 }
					 });
				 }
			 });

			 $(".closecomments").click(function () {
				 $(".commentdialog").dialog("close");
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
	<div class="viewbuttons clearfix">
		<div class="ddbuttonlist enabled" id="DVexport" runat="server"><!--
	--><asp:LinkButton ID="LNBexportSelfCommitteesStatisticsToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="csv" OnClientClick="blockUIForDownload(6);return true;" ></asp:LinkButton><!--
	<asp:LinkButton ID="LNBexportSelfCommitteesStatisticsToXLS" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="xml" OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportSelfCommitteeFilteredStatisticsToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="csv" OnClientClick="blockUIForDownload(6);return true;" ></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportSelfCommitteeStatisticsToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="csv" OnClientClick="blockUIForDownload(6);return true;" ></asp:LinkButton><!--
	<asp:LinkButton ID="LNBexportSelfCommitteeStatisticsToXLS" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="xml" OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
	--></div>
		<asp:HyperLink ID="HYPlist" runat="server" Text="List calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
		<asp:HyperLink ID="HYPadvCommission" runat="server" Text="Gestione Commissione" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
		<asp:HyperLink ID="HYPmanage" runat="server" Text="Manage calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <asp:LinkButton ID="LKBupdate" runat="server" CssClass="linkMenu">Aggiorna</asp:LinkButton>
	</div>
	 <asp:MultiView id="MLVstatistics" runat="server" ActiveViewIndex="1">
		<asp:View ID="VIWempty" runat="server">
			<br /><br /><br /><br />
			<asp:Label ID="LBnocalls" runat="server"></asp:Label>
			<br /><br /><br /><br />
		</asp:View>
		<asp:View ID="VIWstatistics" runat="server">
			<div class="globalwrapper">
				<div class="progressbarwrapper">
					<asp:Label ID="LBevaluationsStatus_t" CssClass="label" runat="server">Evaluation status</asp:Label>
					<CTRL:StackedBar id="CTRLstackedBar" runat="server" ContainerCssClass="global"></CTRL:StackedBar>
				</div>
				<asp:Label ID="LBcommitteesInfo_t" runat="server" cssclass="iteminfo"></asp:Label>
				<div class="statinfo">
					<ul class="commissions">
						<asp:Repeater ID="RPTcommittees" runat="server">
							<ItemTemplate>
								<li class="commission" runat="server" id="LIcommission">
									<span class="commissionname">
										<asp:HyperLink ID="HYPcommitteeDashboard" runat="server" Visible="false"></asp:HyperLink>
										<asp:Literal ID="LTcommitteeName" runat="server" Visible="false"></asp:Literal>
									</span>
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
                                        <span class="statusitem">
											<asp:Label id="LBconfirmedCount" runat="server" CssClass="blue">0</asp:Label>
											<asp:Label id="LBconfirmed" runat="server" CssClass="label">Confermati</asp:Label>
										</span>
								   </span>
								</li>
							</ItemTemplate>
						</asp:Repeater>
					</ul>
				</div>
			</div>
			<div class="commission" id="DVcommittee" runat="server">
				<div class="title">
					<a href="committee_<%=IdCurrentCommittee %>"></a>
					<asp:Label ID="LBdashboardCommitteeName" runat="server" cssclass="commissionname"></asp:Label>
					<asp:label ID="LBcommitteeName" runat="server" cssclass="commissionname"></asp:label>
					<asp:label ID="LBcommitteeEvaluationEndOn" runat="server" cssclass="commissionname"></asp:label>
				</div>
				<div class="clearfix" id="DVfilter" runat="server" visible="true" >
					<div class="left">
						<asp:Label ID="LBsearchEvaluationsFor_t" runat="server" AssociatedControlID="TXBusername" CssClass="fieldlabel"></asp:Label>
						<asp:TextBox ID="TXBusername" runat="server" CssClass="inputtext"></asp:TextBox>
						<asp:Button id="BTNfindEvaluations" runat="server" />
						<br />
						<div class="evaluationfilter" id="DVsubmitterType" runat="server">
							<asp:Label ID="LBsubmitterType_t" runat="server" AssociatedControlID="DDLsubmitterTypes" CssClass="fieldlabel"></asp:Label>
							<asp:DropDownList ID="DDLsubmitterTypes" runat="server" CssClass="inputtext" AutoPostBack="true"></asp:DropDownList>
						</div>
						<div class="evaluationfilter" id="DVstatusfilter" runat="server">
							<asp:Label ID="LBevaluationStatusFilter_t" runat="server" AssociatedControlID="RBLevaluationStatus" CssClass="fieldlabel"></asp:Label>
							<asp:RadioButtonList ID="RBLevaluationStatus" runat="server" AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputradiobuttonlist"></asp:RadioButtonList>
						</div>

					</div>
					<div class="right">            
					</div>
				</div>
				<div class="commissionwrapper">
					<div class="iteminfo top">
						<a href="#<%=DVlegend.ClientId %>" class="openlegend">
							<span class="icons">
								<asp:Label ID="LBlegendTop" runat="server"></asp:Label><asp:Label ID="LBiconLegendTop" runat="server" CssClass="icon info"></asp:Label>
							</span>
						</a>
					</div>
					<table id="tree_table" class="tree_table evaluation detail <%=CssTableCriteriaCount() %>">
						<thead>
							<tr class="sliderrow">
								<th class="submitternumber empty">&nbsp;</th>
								<th class="submittername empty">&nbsp;</th>
								<th class="submittertype empty">&nbsp;</th>
								<th class="points empty">&nbsp;</th>
								<th colspan="5" class="slidercell commissions">
									<span >
										<div class="colslider" id="slider"></div>
										<span class="slider min"></span>-<span class="slider max"></span>/<span class="slider all"></span>
									</span>
								</th>
							</tr>
							<tr>
								<th class="submitternumber" rowspan="2">
									<asp:HyperLink ID="HYPorderByEvaluationIndexUp" runat="server" cssclass="icon orderUp" Visible="true">#</asp:HyperLink>
									<asp:HyperLink ID="HYPorderByEvaluationIndexDown" runat="server" cssclass="icon orderDown" Visible="false">#</asp:HyperLink>
								</th>
								<th class="submittername" rowspan="2">
									<asp:Literal id="LTsubmitterName_t" runat="server"></asp:Literal>
									<asp:HyperLink ID="HYPorderByUserUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
									<asp:HyperLink ID="HYPorderByUserDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>
								</th>
								<th class="submittertype" rowspan="2">
									<asp:Literal id="LTsubmitterType_t" runat="server"></asp:Literal>
									<asp:HyperLink ID="HYPorderByTypeUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
									<asp:HyperLink ID="HYPorderByTypeDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>
								</th>
								<th class="points" rowspan="2">
									<asp:Literal id="LTsubmissionPoints_t" runat="server"></asp:Literal>
									<asp:HyperLink ID="HYPorderByEvaluationPointsUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
									<asp:HyperLink ID="HYPorderByEvaluationPointsDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>
								</th>
								<th class="commission" id="committee_<%=IdCurrentCommittee %>" colspan="5">
									<CTRL:StackedBar id="CTRLcommitteeStackedBar" runat="server" ContainerCssClass="commission"></CTRL:StackedBar>
								</th>
								<th class="actions" rowspan="2"> <asp:Literal id="LTactions_t" runat="server"></asp:Literal></th>
							</tr>
							<tr>
								<asp:Repeater ID="RPTcriteria" runat="server">
									<ItemTemplate>
										<th class="evaluator child-of-commission-<%=IdCurrentCommittee %>"><asp:label ID="LBcriterionName" runat="server"></asp:label></th>    
									</ItemTemplate>
								</asp:Repeater>
								<th class="evaluator child-of-commission-<%=IdCurrentCommittee %>" runat="server" visible="false" id="THcriterionPlaceHolder"></th>
							</tr>
						</thead>
					<tbody>
						<asp:Repeater ID="RPTevaluations" runat="server">
							<ItemTemplate>
								<tr id="subm-<%#Container.DataItem.Id %>" class="submitter">
									<td class="submitternumber"><%#Container.DataItem.Position%></td>
									<td class="submittername"><asp:Literal ID="LTdisplayName" runat="server"></asp:Literal></td>
									<td class="submittertype"><%#Container.DataItem.SubmitterType%></td>
									<td class="points">
										<span class="vote " id="SPNvoteContainer" runat="server">
											<asp:Label ID="LBvote" runat="server" CssClass="number"></asp:Label>
											<span class="text" runat="server" visible="false" id="SPNfuzzy"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
										</span>
									</td>
									<asp:Repeater ID="RPTcriteriaEvaluated" runat="server" DataSource="<%#Container.DataItem.Criteria %>" OnItemDataBound="RPTcriteriaEvaluated_ItemDataBound">
										<ItemTemplate>
											<td class="evaluator">
												<asp:label ID="LBcriterionEvaluated" runat="server"></asp:label>
												<CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber>
												<asp:Literal ID="LTbool" runat="server"><span class="label-state stateBool_{0}" title="{1}">{1}</span></asp:Literal>
											</td>    
										</ItemTemplate>
									</asp:Repeater>
									<td class="evaluator" runat="server" visible="false" id="TDcriterionPlaceHolder"></td>
									<td class="actions">
										<span class="icons">
											<asp:HyperLink ID="HYPevaluateSubmission" runat="server" Target="_blank" CssClass="icon evaluate" Visible="false">E</asp:HyperLink>
											<asp:HyperLink ID="HYPviewEvaluation" runat="server" Target="_blank" CssClass="icon view" Visible="false">V</asp:HyperLink>
										</span>
									</td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
						</tbody>
						<tfoot>
							<tr class="sliderrow">
								<td colspan="4" class="empty">&nbsp;</td>
								<td colspan="5" class="slidercell">
									<span >
										<div class="colslider"></div>
										<span class="slider min">1</span>-<span class="slider max">5</span>/<span class="slider all">10</span>
									</span>
								</td>
							</tr>
						</tfoot>
					</table>
					<div class="iteminfo bottom">
						<a href="#<%=DVlegend.ClientId %>" class="openlegend">
							<span class="icons">
								<asp:Label ID="LBlegendBottom" runat="server"></asp:Label><asp:Label ID="LBiconLegendBottom" runat="server" CssClass="icon info"></asp:Label>
							</span>
						</a>
					</div>
					<div class="dialog legend"  id="DVlegend" runat="server">
						<table class="table minimal criterialegend">
							<thead>
								<tr>
									<th class="short"><asp:Literal ID="LTshortCriterionName" runat="server"></asp:Literal></th>
									<th class="long"><asp:Literal ID="LTlongCriterionName" runat="server"></asp:Literal></th>
								</tr>
							</thead>
							<tbody>
							<asp:Repeater ID="RPTlegend" runat="server">
								<ItemTemplate>
									<tr>
										<td class="short"><asp:label ID="LBcriterionName" runat="server"></asp:label></td>
										<td class="long"><%#container.Dataitem.Name %></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							</tbody>
						</table>
					</div>
				</div>
			</div>
		</asp:View>
	</asp:MultiView>
	<asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>