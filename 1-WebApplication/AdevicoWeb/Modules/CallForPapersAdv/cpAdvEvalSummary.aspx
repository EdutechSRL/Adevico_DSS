<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="cpAdvEvalSummary.aspx.vb" Inherits="Comunita_OnLine.cpAdvEvalSummary" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLreport" Src="~/Modules/CallForPapers/UC/UC_SubmissionExport.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapersAdv/UC/Uc_AdvHeader.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyNumber.ascx" TagName="CTRLfuzzyNumber" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapersAdv/UC/Uc_advScoreItem.ascx" TagPrefix="CTRL" TagName="ScoreItem" %>



<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	 <link href="../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
	<CTRL:Header ID="CTRLheader" runat="server"  EnableDropDownButtonsScript="true" EnableTreeTableScript="true" />
		<%--<link href="../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="stylesheet" />
	 <link href="../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
	<link href="../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
	<link href="../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css" rel="Stylesheet" />
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css"/>
	<link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.progressbar.js"></script>--%>
	<script type="text/javascript" language="javascript">
		<% = me.CTRLreport.GetControlScript(HDNdownloadTokenValue.ClientID) %>

		 $(function () {
			$(".progressbar").myProgressBar();
			$(".ddbuttonlist.enabled").dropdownButtonList();
		 });
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="viewbuttons clearfix">
		<div class="ddbuttonlist enabled" id="DVexport" runat="server"><!--
	--><asp:LinkButton ID="LNBexportAllEvaluationsSummaryToCsv" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportAllEvaluationsSummaryToXLS" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportFilteredEvaluationsSummaryToCsv" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportFilteredEvaluationsSummaryToXLS" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportAllEvaluationsSummaryDataToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportAllEvaluationsSummaryData" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportFullEvaluationsSummaryDataToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
	--><asp:LinkButton ID="LNBexportFullEvaluationsSummaryDataToXml" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
	--></div>
		<asp:HyperLink ID="HYPlist" runat="server" Text="List calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
		<asp:HyperLink ID="HYPmanage" runat="server" Text="Manage calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
		<asp:HyperLink ID="HYPtoCommitteesSummary" runat="server" Text="Details" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
		<CTRL:CTRLreport ID="CTRLreport" runat="server" WebOnlyRender="True" isContainer="false" Visible="false" />
		<asp:HyperLink ID="HYPcommissionSummary" runat="server" CssClass="linkMenu">Sommario commissioni</asp:HyperLink>
        <asp:HyperLink ID="HYPevaluationProcess" runat="server" CssClass="linkMenu">Processo di valutazione</asp:HyperLink>
		<asp:LinkButton ID="LKBupdate" runat="server" CssClass="linkMenu">Aggiorna</asp:LinkButton>
	</div>
	<CTRL:Messages ID="CTRLdssMessage"  runat="server" visble="false"/>
	<div class="contentwrapper edit clearfix" id="DVfilter" runat="server" visible="true" >
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
	<asp:MultiView ID="MLVevaluations" runat="server" ActiveViewIndex="0">
		<asp:View ID="VIWlist" runat="server">
			<div class="pager" runat="server" id="DVpagerTop"  visible="false">
				<asp:literal ID="LTpageTop" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
			</div>
			<div class="contentwrapper edit clearfix">
				<asp:Repeater id="RPTevaluations" runat="server">
					<HeaderTemplate>
						<table class="tree_table evaluation basic">
							<thead>
								<tr>
									<th class="submitternumber">
										<asp:HyperLink ID="HYPorderByEvaluationIndexUp" runat="server" cssclass="icon orderUp">#</asp:HyperLink>
										<asp:HyperLink ID="HYPorderByEvaluationIndexDown" runat="server" cssclass="icon orderDown">#</asp:HyperLink>
									</th>
									<th class="submittername">
										<asp:Literal id="LTsubmitterName_t" runat="server"></asp:Literal>
										<asp:HyperLink ID="HYPorderByUserUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
										<asp:HyperLink ID="HYPorderByUserDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>
									</th>
									<th class="submittertype">
										<asp:Literal id="LTsubmitterType_t" runat="server"></asp:Literal>
										<asp:HyperLink ID="HYPorderByTypeUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
										<asp:HyperLink ID="HYPorderByTypeDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>
									</th>
									<th class="points">
										<asp:Literal id="LTsubmissionPoints_t" runat="server"></asp:Literal>
										<asp:HyperLink ID="HYPorderByEvaluationPointsUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
										<asp:HyperLink ID="HYPorderByEvaluationPointsDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>
									</th>
									<th class="status">
										<asp:Literal id="LTevaluationStatus_t" runat="server"></asp:Literal>
										<asp:HyperLink ID="HYPorderByEvaluationStatusUp" runat="server" cssclass="icon orderUp"></asp:HyperLink>
										<asp:HyperLink ID="HYPorderByEvaluationStatusDown" runat="server" cssclass="icon orderDown"></asp:HyperLink>
									</th>
									<th class="actions"><asp:literal ID="LTsubActions_t" runat="server">Actions</asp:literal></th>
								</tr>
							</thead>
							<tbody>
					</HeaderTemplate>
					<ItemTemplate>
								<tr id="subm-<%#Container.DataItem.IdSubmission %>" class="submitter">
									<td class="submitternumber"><%#Container.DataItem.Position%></td>
									<td class="submittername"><%#Container.DataItem.Displayname%></td>
									<td class="submittertype"><%#Container.DataItem.SubmitterType%></td>
									<td class="points">
										
										<CTRL:ScoreItem runat="server" id="Uc_ScoreItem" />

									</td>
									<td class="status">
										<CTRL:StackedBar id="CTRLevaluationStackedBar" runat="server"></CTRL:StackedBar>
										<asp:Literal ID="LTcommissayCount" runat="server">
											<span class="cmcount">
												<span class="cmcountCurrent" title="valutazioni Concluse">{0}</span>
												/
												<span class="cmcountTotal" title="Totale valutatori">{1}</span>
											</span>
										</asp:Literal>
									</td>
									<td class="actions">
										<asp:literal ID="LTemptyActions" runat="server" Text=" "/>
										<span class="icons">                                            
											<CTRL:CTRLreport ID="CTRLreport" runat="server" isContainer="false" OnGetConfigTemplate="CTRLreport_GetConfigTemplate" OnGetContainerTemplate="CTRLreport_GetContainerTemplate" OnGetHiddenIdentifierValueEvent="CTRLreport_GetHiddenIdentifierValueEvent"/>
											<asp:HyperLink ID="HYPviewSubmissionEvaluation" runat="server" CssClass="icon view" Target="_blank" ></asp:HyperLink>
											<asp:HyperLink ID="HYPviewTableSubmissionEvaluation" runat="server" CssClass="icon infoalt" Target="_blank" ></asp:HyperLink>
										</span>
									</td>
								</tr>
					</ItemTemplate>
					<FooterTemplate>
							</tbody>
						</table>

					</FooterTemplate>
				</asp:Repeater>
			</div>
			<div class="pager" runat="server" id="DVpagerBottom" visible="false">
				<asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
			</div>

			<div class="viewbuttons clearfix">
				<asp:LinkButton id="LNBcommclose" runat="server" Enabled="false" Visible="false" Text="Chiudi commissione" CssClass="linkMenu"></asp:LinkButton>
			</div>

		</asp:View>
		<asp:View ID="VIWnoItems" runat="server">
			<br /><br /><br /><br /><br /><br />
			<asp:Label id="LBnoEvaluations" runat="server" ></asp:Label>
		</asp:View>
	</asp:MultiView>
	<asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>