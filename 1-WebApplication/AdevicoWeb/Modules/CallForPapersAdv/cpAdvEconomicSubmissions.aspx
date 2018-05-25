<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="cpAdvEconomicSubmissions.aspx.vb" Inherits="Comunita_OnLine.cpAdvEconomicSubmissions" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapersAdv/UC/Uc_AdvHeader.ascx" %>

<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapersAdv/UC/Uc_AdvTableExport.ascx" TagPrefix="CTRL" TagName="Uc_AdvTableExport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	 <CTRL:Header ID="CTRLheader" runat="server"  EnableTreeTableScript="true" EnableDropDownButtonsScript="true"   />
	<link href="../../Graphics/Modules/CallForPapers/css/callforpeaperADV.css" rel="stylesheet" />
	<link href="../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="contentwrapper edit clearfix">
	<div class="viewbuttons clearfix">
		<asp:HyperLink ID="HYPback" runat="server" CssClass="linkMenu">Processo di valutazione</asp:HyperLink>
	</div>
	<asp:MultiView id="MLVstatistics" runat="server" ActiveViewIndex="1">
		<asp:View ID="VIWempty" runat="server">
			<br /><br /><br /><br />
			<asp:Label ID="LBerror" runat="server"></asp:Label>
			<br /><br /><br /><br />
		</asp:View>
		<asp:View ID="VIWstatistics" runat="server">
			<div class="globalwrapper hide">
				<div class="progressbarwrapper">
					<asp:Label ID="LBevaluationsStatus_t" CssClass="label" runat="server">Evaluation status</asp:Label>
					<CTRL:StackedBar id="CTRLstackedBar" runat="server" ContainerCssClass="global"></CTRL:StackedBar>
				</div>

			</div>
			<div class="commissionwrapper">
				<table id="tree_table" class="tree_table table evaluation detail">
						<thead>
							<tr>
								<th class="submitternumber">
									<asp:Label ID="LBLrank_t" runat="server">Rank</asp:Label>
								</th>
								<th class="submittername">
									<asp:Label  id="LBsubmitterName_t" runat="server">Domanda</asp:Label>
								</th>
								<th class="submittertype">
									<asp:Label id="LBsubmitterType_t" runat="server">Tipo</asp:Label>
								</th>
								<th class="points">
									<asp:Label id="LBsubmissionPoints_t" runat="server">Punti</asp:Label>
								</th>
								<th class="funded">
									<asp:Label id="LBFound_t" runat="server">Finanziato</asp:Label>
								</th>
								<th class="status">
									<asp:Label id="LBstatus_t" runat="server">Stato valutazione</asp:Label>
								</th>
								<th class="actions" >
									<asp:Label id="LBaction_t" runat="server">Azioni</asp:Label>
								</th>
							</tr>
						</thead>
					<tbody>
						<asp:Repeater ID="RPTsubmission" runat="server">
							<ItemTemplate>
								<tr>
									<td class="submitternumber">
										<asp:Literal ID="LTrank" runat="server">##</asp:Literal>
									</td>
									<td class="submittername">
										<asp:Literal ID="LTname" runat="server">#nome</asp:Literal>
									</td>
									<td class="submittertype">
										<asp:Literal ID="LTtype" runat="server">#tipo</asp:Literal>
									</td>
									<td>
										<span class="votefinal">
											<span class="number"><asp:Literal ID="LTscore" runat="server"></asp:Literal></span>
										</span>
									</td>
									<td class="funded">
										<asp:Literal ID="LTfunded" runat="server"></asp:Literal> &euro;
									</td>
									<td  class="status">
										<asp:Literal ID="LTspanStatus" runat="server">
											<span class="status {0}">{1}</span>
										</asp:Literal>
									</td>
									<td class="actions icons">
										<asp:HyperLink ID="HYPeval" runat="server" CssClass="icon evaluate">Valuta</asp:HyperLink>
										<asp:HyperLink ID="HYPsubmission" runat="server" CssClass="icon submission">Vista completa</asp:HyperLink>
										<CTRL:Uc_AdvTableExport runat="server" ID="Uc_AdvTableExport" LinkClass="icon download" LinkText="" LinkToolTip="Esporta tabelle in .xlsx" />
									</td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
					</tbody>
					<tfoot>
						<tr>
							<td colspan="3">&nbsp;</td>
							<td>Totale finanziato</td>
							<td><span class="total"><asp:Literal ID="LTtotal" runat="server"></asp:Literal></span> &euro;</td>
						</tr>
					</tfoot>
				</table>
			</div>
			</asp:View>
		</asp:MultiView>
		</div>
		<div class="viewbuttons clearfix">
			<asp:LinkButton ID="LKBcloseCommission" runat="server" CssClass="linkMenu" Visible="false">Chiudi commissione</asp:LinkButton>
		</div>
</asp:Content>
