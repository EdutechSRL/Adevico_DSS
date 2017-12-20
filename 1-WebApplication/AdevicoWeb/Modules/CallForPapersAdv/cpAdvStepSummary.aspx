<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="cpAdvStepSummary.aspx.vb" Inherits="Comunita_OnLine.cpAdvStepSummary" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapersAdv/UC/Uc_AdvHeader.ascx" %>
<%@ Register Src="~/Modules/CallForPapersAdv/UC/Uc_advScoreItem.ascx" TagPrefix="CTRL" TagName="ScoreItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<CTRL:Header ID="CTRLheader" runat="server"  EnableDropDownButtonsScript="true" EnableTreeTableScript="true" />
	<link href="../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="stylesheet" />
	<link href="../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
	<script type="text/javascript">
		$(document).ready(function(){
			/* toggle columns */
			$(".showCommission").click(function(){
				$(this).hide();
				$(".hideCommission").show();
				$(".points.commission").removeClass("hide");
			});
			$(".hideCommission").click(function(){
				$(this).hide();
				$(".showCommission").show();
				$(".points.commission").addClass("hide");
			});
			/* script checked fino a dove fai click*/
			$("table.tree_table.evaluation td.check input").click(function(e){
				var index = $("table.tree_table.evaluation td.check input").index(e.target);
				if (!$(e.target).is(':checked'))
					index--;
				$("table.tree_table.evaluation td.check input").prop("checked", false);
				$("table.tree_table.evaluation td.check input").slice(0,index+1).prop("checked", true);
			});
			/* fine script checked fino a dove fai click*/
		});
	</script>

  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	Riepilogo valutazioni
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	
	<div class="viewbuttons clearfix">
		
		<asp:HyperLink ID="HypStep" runat="server" CssClass="linkMenu">Torna al processo di valutazione</asp:HyperLink>
	</div>
	<div class="contentwrapper editsteps clearfix">
	<asp:MultiView ID="MLVevaluations" runat="server" ActiveViewIndex="0">
		<asp:View ID="VIWlist" runat="server">
			<span class="showhide">
				<asp:hyperlink ID="hypShow" runat="server" CssClass="linkMenu showCommission" style="display:none;">Mostra commissioni</asp:hyperlink>
				<asp:hyperlink ID="hypHide" runat="server" CssClass="linkMenu hideCommission">Nascondi commissioni</asp:hyperlink>
			</span>
			<table class="tree_table evaluation basic">
				<thead>
					<tr>
						<th class="submitternumber" style="width: 1%;">
						<%--<asp:HyperLink ID="HYPorderByEvaluationIndexUp" runat="server" cssclass="icon orderUp">#</asp:HyperLink>
							<asp:HyperLink ID="HYPorderByEvaluationIndexDown" runat="server" cssclass="icon orderDown">#</asp:HyperLink>--%>
						</th>
						<th class="submittername">
							<asp:Literal id="LTsubmitterName_t" runat="server">Nome sottomittore</asp:Literal>
							<%--<asp:HyperLink ID="HYPorderByUserUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
							<asp:HyperLink ID="HYPorderByUserDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>--%>
						</th>
						<th class="points">
							<asp:Literal id="LTstepPoints_t" runat="server">Punteggio finale</asp:Literal>
							<%--<asp:HyperLink ID="HyperLink1" runat="server" cssclass="icon orderUp"></asp:HyperLink>
							<asp:HyperLink ID="HyperLink2" runat="server" cssclass="icon orderDown"></asp:HyperLink>--%>
						</th>
						<th class="success">
							<asp:Literal id="LTstepCriterion_t" runat="server">Criteri superati</asp:Literal>
						</th>
						<th class="admit">
							<asp:Literal id="LTstepAdmit" runat="server">Ammessi</asp:Literal>
						</th>
						<asp:Repeater ID="RPTcommission" runat="server">
							<ItemTemplate>
								<th class="points commission">
									<%--<asp:Literal id="LTcomm" runat="server">#CommissionName</asp:Literal>--%>
                                    <asp:HyperLink ID="HYPcommissionSummary" runat="server" CssClass="linkCommission">#CommissionName</asp:HyperLink>
									<asp:Literal id="LTcommStatus" runat="server"><span class="status {0}" title="{1}"></span></asp:Literal>
								</th>
							</ItemTemplate>
						</asp:Repeater>
					</tr>
				</thead>
				<tbody>
					<asp:Repeater ID="RPTsubmission" runat="server">
						<ItemTemplate>
							<asp:Literal ID="LTtd" runat="server"><tr class="submitter {0}"></asp:Literal>
								<td class="submitternumber">
									<asp:HiddenField ID="HYDid" runat="server" />
									<asp:literal id="LTsubRank" runat="server">#rank</asp:literal>
								</td>
								<td class="submittername">
									<asp:literal id="LTsubName" runat="server">#subName</asp:literal>
								</td>
								<td class="Score">
									<asp:Literal ID="LTfinaleScore" runat="server">##</asp:Literal>
								</td>
								<td class="passed">
									<asp:Literal ID="LTpassed" runat="server">PASSED</asp:Literal>
								</td>
								<td class="check">
									<asp:CheckBox ID="CBXcheck" runat="server" />
								</td>
								<asp:Repeater ID="RPTcommissionScore" runat="server" OnItemDataBound="RPTcommissionScore_ItemDataBound">
									<ItemTemplate>
										<td class="points commission">
											<CTRL:ScoreItem runat="server" id="Uc_ScoreItem" />
										</td>
									</ItemTemplate>
								</asp:Repeater>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
				</tbody>
			</table>
			<span>
				<asp:LinkButton ID="LKBConfirmStep" runat="server" Text="Conferma ammissioni" CssClass="linkMenu"></asp:LinkButton>
			</span>
		</asp:View>
		<asp:View ID="VIWnoItems" runat="server">
			<br /><br /><br /><br /><br /><br />
			<asp:Label id="LBnoEvaluations" runat="server" >
				Nessuna valuatazione disponibile.
				<br />
				Saranno presenti dopo l'apertura delle valuationi, se sono state ammesse valuationi allo step precedente.
			</asp:Label>
		</asp:View>
	</asp:MultiView>
	</div>
	
</asp:Content>
