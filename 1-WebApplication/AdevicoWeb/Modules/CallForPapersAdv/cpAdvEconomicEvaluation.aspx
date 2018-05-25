<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="cpAdvEconomicEvaluation.aspx.vb" Inherits="Comunita_OnLine.cpAdvEconomicEvaluation" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapersAdv/UC/Uc_AdvHeader.ascx" %>
<%@ Register Src="~/Modules/CallForPapersAdv/UC/Uc_AdvTableExport.ascx" TagPrefix="CTRL" TagName="Uc_AdvTableExport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	 <CTRL:Header ID="CTRLheader" runat="server"  EnableTreeTableScript="true" EnableDropDownButtonsScript="true"   />
	<link href="../../Graphics/Modules/CallForPapers/css/callforpeaperADV.css?v=201605041410lm" rel="stylesheet" />
	<link href="../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
	<script type="text/javascript">
	AdvMoneyFormat = {
		bindInputList : function(inputs_jQ){
			inputs_jQ.each(function(index, inputTarget){
				AdvMoneyFormat.bindInput($(inputTarget));
			});
		},
		bindInput : function (input_jQ) {
			input_jQ.val(AdvMoneyFormat.getMoneyStr(input_jQ.val()));
			var keyUpTimer = null;
			input_jQ.on("keyup", function(e){
				var thisTarget_jQ = $(e.target);
				clearTimeout(keyUpTimer);
				keyUpTimer = setTimeout(function(){
					var _myMoney = AdvMoneyFormat.getMoneyStr(thisTarget_jQ.val());
					thisTarget_jQ.val(_myMoney);
				},800);
			});
			input_jQ.on("focusout", function(e){
				var thisTarget_jQ = $(e.target);
				var _myMoney = AdvMoneyFormat.getMoneyStr(thisTarget_jQ.val());
				thisTarget_jQ.val(_myMoney);
			});
		},
		getMoneyStr : function(strValue) {
			var _thisFloat = AdvMoneyFormat.getFloatFromMoneyValue(strValue);
			return AdvMoneyFormat.getMoneyStrFromFloat(_thisFloat);
		},
		getMoneyStrFromFloat : function(floatValue) {
			var returnVal = floatValue.toFixed(2).replace(/\./g, ',').replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.");
			return returnVal;
		},
		getFloatFromMoneyValue : function(strValue) {
			if(!strValue)
				return 0;
			try{
				return parseFloat(strValue.replace(/\./g, '').replace(/\,/g, '.').replace(/[^0-9\.-]+/g,""));
			}catch(e){
				return "not a number.";
			}
		}
	}

	jQuery(document).ready(function() {
		AdvMoneyFormat.bindInputList($(".moneyValue"));
		var CalcTables = $("table.tree_table.evaluation.detail");
		CalcTables.each(function(index){
			var elTable = $(this);
			var strError = "";
			var timerKeyUp = null;
			elTable.find("tr").each(function(index){
				var elTR = $(this);
				var admitValueInput = elTR.find('td input.admitvalue').first();
				var admitQtaInput = elTR.find('td input.admitQta').first();
				var checkEnableTr = elTR.find('td.tdCheckBox input[type="checkbox"]').first();
				checkEnableTr.change(function() {
					if(!checkEnableTr.is(':checked')){
						admitValueInput.val("0,00");
						admitQtaInput.val("0");
						admitValueInput.focusout(); // per aggionare la formattazione del dato
						admitValueInput.prop("disabled", true);
						admitQtaInput.prop("disabled", true);
					}else{
						admitQtaInput.val(elTR.find(".quantityRequest").first().html());
						admitValueInput.val(elTR.find(".economicRequest").first().html());
						admitValueInput.focusout(); // per aggionare la formattazione del dato
						admitValueInput.prop("disabled", false);
						admitQtaInput.prop("disabled", false);
					}
				});                
				if(admitValueInput.size() > 0){                    
					if(!checkEnableTr.is(':checked')){
						admitValueInput.prop("disabled", true);
						admitQtaInput.prop("disabled", true);
					}else{
						admitValueInput.prop("disabled", false);
						admitQtaInput.prop("disabled", false);
					}
					admitValueInput.on("focusout", function(e){
						var target_jQ = $(e.target);
						target_jQ.keyup();
					});
					admitValueInput.on("keyup", function(e){
						clearTimeout(timerKeyUp);
						timerKeyUp = setTimeout(function(){
							var totalPoints = 0;
							var strError = "";
							elTable.find('input.admitvalue').each(function(index, elTarget){
								var floatNamber = AdvMoneyFormat.getFloatFromMoneyValue($(elTarget).val());
								if(isNaN(floatNamber)){
									strError = "Not a number.<br />";
									floatNamber = 0;
									$(elTarget).addClass("errorBordered");
								}else{
									$(elTarget).removeClass("errorBordered");
								}
								totalPoints += floatNamber;
							});
							var maxFloat = AdvMoneyFormat.getFloatFromMoneyValue(elTable.find(".admitmax:first").html());
							var totalMoneyStr = AdvMoneyFormat.getMoneyStrFromFloat(totalPoints);
							if((maxFloat < totalPoints || strError) && maxFloat > 0){
								elTable.find(".admitcontainer .tableResult").addClass("overBudget");
							}else{
								elTable.find(".admitcontainer .tableResult").removeClass("overBudget");
							}

							if(strError)
								elTable.find(".admitcontainer .admit:first").html(strError + ": " + totalMoneyStr);
							else{
								elTable.find(".admitcontainer .admit:first").html(totalMoneyStr);
								var totalOne = 0;
								var totalOneMAX = AdvMoneyFormat.getFloatFromMoneyValue($(".sumbmissiondetails .admitGlobalMax:first").html());
								CalcTables.find(".admitcontainer .admit:first").each(function(indexToT, elTot){
									totalOne += AdvMoneyFormat.getFloatFromMoneyValue($(elTot).html());
								});
								if(totalOneMAX < totalOne)
									$(".sumbmissiondetails .admitGlobalTotal:first").addClass("overBudget");
								else
									$(".sumbmissiondetails .admitGlobalTotal:first").removeClass("overBudget");

								$(".sumbmissiondetails .admitGlobalTotal:first").html(AdvMoneyFormat.getMoneyStrFromFloat(totalOne))
							}
						},1000);
					});
				}
			});
			elTable.find('td input.admitvalue').first().focusout();
		});
	});

	function getStrFloatFromMoneyValue(strValue){
		return parseFloat(strValue.replace(/\./g, '').replace(/\,/g, '.').replace(/[^0-9\.-]+/g,""));
	}
	function moneyFloatPrettify(floatValue){
		return parseFloat(floatValue.toFixed(2)).toLocaleString();
	}
	</script>
	<style>
		table.tree_table.evaluation.detail tr td input,
		table.tree_table.evaluation.detail tr td textarea{
			width:100%;
		}
		/*.overBudget,
		.errorBordered{
			background-color:#ad2424 !important;
			padding:0 6px;
			color:#fff;
		}*/
		#container{
			min-width:920px;
			width:90%;
		}
		#container .contentwrapper {
			width:100%;
		}
		.tree_table tr textarea{
			font-size: inherit;
		}
	</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="contentwrapper edit clearfix">
	<asp:MultiView id="MLVevals" runat="server" ActiveViewIndex="1">
		<asp:View ID="VIWempty" runat="server">
			<br /><br /><br /><br />
			<asp:Label ID="LBerror" runat="server"></asp:Label>
			<br /><br /><br /><br />
		</asp:View>
		<asp:View ID="VIWeval" runat="server">
			<div class="innerwrapper clearfix">
				<div class="left">
					<ul class="sumbmissiondetails">
						<li class="submitter">
							<asp:Label ID="LTowner_t" runat="server">Proprietario:</asp:Label>
							<asp:Label ID="LBowner" runat="server" />
						</li>
						<li class="submittertype">
							<asp:Label ID="LTsubmitterType_t" runat="server">Tipo partecipante:</asp:Label>
							<asp:Label ID="LBsubmitterType" runat="server" />
						</li>
						<li>
							<asp:Label ID="LBFinalTotalRequest_t" runat="server">Totale sottomissione:</asp:Label>
							<asp:Label ID="LBFinalTotalRequest" runat="server">###</asp:Label> &euro;
						</li>
						 <li>
							<asp:Label ID="LBFinalTotalAdmit_t" runat="server">Totale Ammesso:</asp:Label>
							<asp:Label ID="LBFinalTotalAdmit" runat="server" CssClass="admitGlobalTotal">###</asp:Label> &euro;
						</li>
						 <li class="hide">
							<asp:Label ID="LBFinalMAXadmit_t" runat="server">Massimo Ammesso:</asp:Label>
							<asp:Label ID="LBFinalMAXadmit" runat="server" CssClass="admitGlobalMax">###</asp:Label> &euro;
							 <%--
                                 Classe da reimpostare: overBudget
                                 File Css: inline, vedi sopra.
                            --%>
						</li>
						<%--<li class="status">
							<asp:Literal ID="LTsubmissionStatus_t" runat="server"></asp:Literal>&nbsp;<asp:Label
								ID="LBsubmissionStatus" runat="server"></asp:Label></li>--%>
						<%--<li class="submissiondate" id="LIsubmissionInfo" runat="server" visible="false">
							<asp:Literal ID="LTsubmittedOn_t" runat="server"></asp:Literal>&nbsp;
							<asp:Label ID="LBsubmittedOnData" runat="server" CssClass="date" />&nbsp;<asp:Label
								ID="LBsubmittedOnTime" runat="server" CssClass="time" />
							<span class="submittedby" runat="server" id="SPNsubmittedBy">&nbsp;<asp:Literal ID="LTsubmittedBy_t"
								runat="server"></asp:Literal>&nbsp;
								<asp:Label ID="LBsubmittedBy" runat="server" />
							</span></li>--%>
					</ul>
				</div>
				<div class="right">
					<asp:HyperLink ID="HypSummary" runat="server" CssClass="linkMenu">Sommario</asp:HyperLink>
					<asp:LinkButton ID="LKBSave" runat="server" CssClass="linkMenu">Salva bozza</asp:LinkButton>
					<asp:LinkButton ID="LKBconfirm" runat="server" CssClass="linkMenu">Conferma definitiva</asp:LinkButton>
					<asp:LinkButton ID="LKBdraft" runat="server" CssClass="linkMenu">Rimetti in bozza</asp:LinkButton>
					<asp:LinkButton ID="LKBClose" runat="server" CssClass="linkMenu">Chiudi valutazione</asp:LinkButton>
                    <CTRL:Uc_AdvTableExport runat="server" id="Uc_AdvTableExport" />
				</div>
			</div>

			<asp:Repeater ID="RPTtables" runat="server" EnableViewState="true">
				<ItemTemplate>
					<div class="tablecontainer">

						<asp:Label ID="LBLfieldName" runat="server" CssClass="fieldName">#nome </asp:Label>
						<br />
						<asp:Label ID="LBLfielddescription" runat="server" CssClass="fieldDescription">#descrizione</asp:Label>

						<asp:HiddenField ID="HFfieldId" runat="server" />
						
								<table id="tree_table" class="tree_table table evaluation detail">
									<thead>
										<tr>
											<%--<th>
												<asp:Literal ID="LTname_t" runat="server">Nome</asp:Literal>
											</th>
											<th>
												<asp:Literal ID="LTdescription_t" runat="server">Descrizione</asp:Literal>
											</th>--%>
											<asp:Repeater ID="RPTtableHeader" runat="server" OnItemDataBound="RPTtableHeader_ItemDataBound">
												<ItemTemplate>
													<th>
														<asp:Literal ID="LTcontentHead" runat="server"></asp:Literal>
													</th>
												</ItemTemplate>
											</asp:Repeater>
											<th>
												<asp:Literal ID="LTquantity_t" runat="server">Quantità</asp:Literal>
											</th>
											<th>
												<asp:Literal ID="LTunitPrice_t" runat="server">Prezzo unitario</asp:Literal>
											</th>
											<th>
												<asp:Literal ID="LTtotal_t" runat="server">Totale richiesto</asp:Literal>
											</th>
											<th>
												<asp:Literal ID="LTisAmmessa_t" runat="server">Approvato</asp:Literal>
											</th>
											<th>
												<asp:Literal ID="LTAmmessaQntt_t" runat="server">Quantità ammessa</asp:Literal>
											</th>
											<th>
												<asp:Literal ID="LTAmmessaValut_t" runat="server">Spesa ammessa</asp:Literal>
											</th>
											<th>
												<asp:Literal ID="LTComment" runat="server">Commenti</asp:Literal>
											</th>
										</tr>
									</thead>
									<tbody>
							
							<asp:Repeater ID="RTPtable" runat="server" OnItemDataBound="RTPtable_ItemDataBound" EnableViewState="true">
								<ItemTemplate>
									<tr>
										<asp:HiddenField ID="HFitmId" runat="server" />
										<asp:Repeater ID="RPTtableValue" runat="server" OnItemDataBound="RPTtableValue_ItemDataBound">
											<ItemTemplate>
												<td>
													<asp:Literal ID="LTcontent" runat="server"></asp:Literal>
												</td>
											</ItemTemplate>
										</asp:Repeater>
										 <td>
											<span class="quantityRequest"><asp:Literal ID="LTquantity" runat="server">Quantità</asp:Literal></span>
										</td>
									   <td>
											<asp:Literal ID="LTunitPrice" runat="server">###</asp:Literal> &euro;
										</td>
										<td>
											<span class="economicRequest"><asp:Literal ID="LTtotal" runat="server">###</asp:Literal></span> &euro;
										</td>
										<td class="tdCheckBox">
											<asp:CheckBox ID="CBXadmit" runat="server" />
										</td>
										<td>
											<asp:TextBox ID="TXBadmitQntt" runat="server" CssClass="admitQta"></asp:TextBox>							
										</td>
										<td>
											<asp:TextBox ID="TXBadmitValue" runat="server" CssClass="admitvalue moneyValue" style="width:90%;"></asp:TextBox> &euro;
										</td>
										<td>
											<asp:TextBox ID="TXBcomment" runat="server" TextMode="MultiLine"></asp:TextBox>
										</td>
									</tr>
							</ItemTemplate>
						</asp:Repeater>	
									<tr class="totals">
										<asp:Literal ID="LTcolSpan" runat="server">
											<td colspan="{0}" class="empty">&nbsp;</td>
										</asp:Literal>
										<td class="label">
											<asp:Label ID="LBtotaliFoot_t" runat="server">Totali:</asp:Label>
										</td>
										<td class="request">
											<span class="economic total"><asp:Literal ID="LTtotalReqFoot" runat="server">###</asp:Literal></span> &euro;
										</td>
										<td class="empty">
											&nbsp;
										</td>
										<td class="empty">
											
										</td>
										<td class="admitcontainer">
											<span class="tableResult">
												<span class="admit"><asp:Literal ID="LTAdmitTotalFoot" runat="server">###</asp:Literal></span> &euro;
												<br />Max: <span class="admitmax"><asp:Literal ID="LTadmitMax" runat="server"><span class="maxvalue">{0}</span></asp:Literal></span> &euro; 
											</span>
										</td>
										<td>
											&nbsp;
										</td>
									</tr>
									</tbody>
								</table>
					</div>
				</ItemTemplate>

			</asp:Repeater>
			<div class="viewbuttons clearfix">

			</div>
			<div class="innerwrapper clearfix">
				<div class="left">
				</div>
				<div class="right">
					<asp:HyperLink ID="HypSummary_bot" runat="server" CssClass="linkMenu">Sommario</asp:HyperLink>
					<asp:LinkButton ID="LKBSave_bot" runat="server" CssClass="linkMenu">Salva bozza</asp:LinkButton>
					<asp:LinkButton ID="LKBconfirm_bot" runat="server" CssClass="linkMenu">Conferma definitiva</asp:LinkButton>
					<asp:LinkButton ID="LKBdraft_bot" runat="server" CssClass="linkMenu">Rimetti in bozza</asp:LinkButton>
					<asp:LinkButton ID="LKBClose_bot" runat="server" CssClass="linkMenu">Chiudi valutazione</asp:LinkButton>
				</div>
			</div>
		</asp:View>
	</asp:MultiView>
	</div>
      
</asp:Content>
