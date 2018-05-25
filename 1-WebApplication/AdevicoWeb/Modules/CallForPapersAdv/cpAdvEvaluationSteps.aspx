<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="cpAdvEvaluationSteps.aspx.vb" Inherits="Comunita_OnLine.cpAdvEvaluationSteps" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	 <CTRL:Header ID="CTRLheader" runat="server" EnableScripts="true"  EnableTreeTableScript="true" EnableDropDownButtonsScript="true"  />
	<link href="../../Graphics/Modules/CallForPapers/css/callforpeaperADV.css?v=201707191642adv" rel="stylesheet" />
	<script type="text/javascript" language="javascript">
		 
		$(function () {
			$(".box-ordinable-steps").sortable({
				handle: "span.movecfielditem.active",
				tolerance: 'pointer',
				placeholder: 'ui-state-highlightHelper',
				forcePlaceholderSize: true,
				forceHelperSize: true,
				dragOnEmpty: true,
				refreshPositions: true,
				axis: 'y',
				helper: "clone",
				start: function (event, ui) {
					$(ui.item).addClass("dragging");
				},
				stop: function (event, ui) {
					$(ui.item).removeClass("dragging");
				}
			});
			$(".box-ordinable-steps").sortable({
				update: function (event, ui) {                    
					var callId = $(this).attr("id");
					var steps = [];
					$(".box-ordinable-steps .stepcontainer").each(function  (index, item){
						var arr = (jQuery(item).attr('class')+"").split(' ');
						var num = "";
						for(var i = 0; i < arr.length; i++){
							var el = arr[i];
							if((el+"").indexOf("Step_") > -1){
								num = (el+"").replace("Step_","");
								break;
							}
						}
						if(num)
							steps.push(num);
					});
					var orders = steps.join(',');
					$("#LTstepStatusOrder").val(orders);


					$.ajax({
						type: "POST",
						url: "../../Modules/CallForPapers/Evaluate/EvaluationReordering.asmx/StepsReorderAdv",
						data: "{'position':'" + orders + "', 'callId':'" + callId + "'}",
						processData: false,
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (msg) {
							//alert(msg.d);
						},
						error: function (result) {
							//alert("Error: (" + result.status + ') [' + result.statusText + ']');

						}
					});
				}
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	Processo di valutazione
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="viewbuttons clearfix">
		<asp:HyperLink runat="server" ID="HYPshowSubmission" CssClass="Link_Menu" Visible="false">Mostra tutte le sottomissioni</asp:HyperLink>
		<asp:HyperLink ID="HypBackManage" runat="server" CssClass="Link_Menu">Gestione bandi</asp:HyperLink>
		<asp:HyperLink ID="HypBackList" runat="server" CssClass="Link_Menu">Lista bandi</asp:HyperLink>
	</div>
	<div class="contentwrapper editsteps clearfix">

		<div class="stepcontainer first">
			<div class="stepinfo validation">
				<span class="iconcontainer icons">
					<asp:literal ID="LTstepValStatus" runat="server"><a href="#" class="icon {0}"></a></asp:literal>
				</span>
				<asp:Label runat="server" ID="LBLstepValName" CssClass="title">Step 0</asp:Label>
				<br/>
				<asp:HyperLink ID="HYPstepValstatus" runat="server"></asp:HyperLink>
			</div>
			<div class="commissionbox validation">
				
				<asp:Label runat="server" ID="LBLcomValName" CssClass="title">Commissione validazione</asp:Label>
				<span class="description">
					<asp:literal ID="LTcomValDesciption" runat="server">...descrizione commissione...</asp:literal>
                    <br />
                    <asp:HyperLink ID="HYPvalutationValid" runat="server">Riepilogo valutazioni</asp:HyperLink>
				</span>
                
				<span class="iconcontainer icons">
					<asp:literal ID="LTcomValStatus" runat="server"><a href="#" class="icon {0}" title="{1}"></a></asp:literal>
					<asp:HyperLink runat="server" ID="HYPcomValEditMember" ToolTip="Configurazione commissione" CssClass="icon committeesettings"></asp:HyperLink>
					<asp:HyperLink runat="server" ID="HYPcomValEditRules" ToolTip="Configurazione criteri" CssClass="icon settings"></asp:HyperLink>
					<asp:HyperLink runat="server" ID="HYPcomValShowSubmission" ToolTip="Valutazioni" CssClass="icon evaluate"></asp:HyperLink>
					<asp:HyperLink runat="server" ID="HYPcomAllShowSubmission" ToolTip="Sottomissioni" CssClass="icon submission"></asp:HyperLink>
					
					<%--<asp:linkbutton runat="server" id="LKBcomValEditMember" CssClass="icon committeesettings"></asp:linkbutton>
					<asp:linkbutton runat="server" id="LKBcomValEditRules" CssClass="icon submission"></asp:linkbutton>--%>
				</span>
			</div>
			
		</div>
		<div class="stepcontainer addSteps">
			<span class="iconcontainer icons">
					<asp:linkbutton runat="server" id="LkbAddStep" CssClass="icon new"></asp:linkbutton>
					<asp:literal runat="server" ID="LTaddStep_t">Aggiungi step</asp:literal>
			</span>
			<asp:HiddenField ID="LTstepStatusOrder" ClientIDMode="Static" runat="server" />
		</div>
		<div id="callId_<%=(Request("cId") + "") %>" class="box-ordinable-steps">
			<asp:repeater ID="RPTsteps" runat="server">
				<ItemTemplate>
					<div class="stepcontainer steps" id="DVstepContainer" runat="server">					
						<div class="stepinfo validation">
							<span class="handlers">
								<asp:Literal ID="LTdrag" runat="server"><span class="movecfielditem active"></span></asp:Literal>
							</span>
							<span class="iconcontainer icons">
								<asp:literal ID="LTstepStatus" runat="server"><a href="#" class="icon {0}" title="{1}"></a></asp:literal>
							</span>
							<asp:Label runat="server" ID="LBLstepName" CssClass="title">Step {0}: {1}</asp:Label>
							<br />
							<asp:HyperLink ID="HYPstepstatus" runat="server"></asp:HyperLink>
						</div>
						<div class="commissionboxconteiner flex-row steps">
							<asp:repeater ID="RPTcommiss" runat="server" OnItemCommand="RPTcommis_ItemCommand">
								<ItemTemplate> 
									<div class="commissionbox validation">
										<asp:PlaceHolder ID="PHcommMaster" runat="server" Visible="false">
											<span class="icons right-icons">
												<span class="icon favorite"></span>
											</span>
										</asp:PlaceHolder>
										<asp:Label runat="server" ID="LBLcomStepName" CssClass="title">Nome</asp:Label>
										<span class="description">
											<asp:literal ID="LTcomStepDesciption" runat="server">...descrizione commissione...</asp:literal>
                                             <br />
                                            <asp:HyperLink ID="HYPvalutation" runat="server">Riepilogo valutazioni</asp:HyperLink>
										</span>
										<span class="iconcontainer icons">
											<asp:literal ID="LTcomStepStatus" runat="server"><a href="#" class="icon {0}" title="{1}"></a></asp:literal>
											<%--<asp:linkbutton runat="server" id="LKBcomStepEditMember" CssClass="icon committeesettings"></asp:linkbutton>
											<asp:linkbutton runat="server" id="LKBcomStepEditRules" CssClass="icon submission"></asp:linkbutton>--%>
											<asp:HyperLink runat="server" ID="HYPcomStepEditMember" ToolTip="Configurazione commissione" CssClass="icon committeesettings"></asp:HyperLink>
											<asp:HyperLink runat="server" ID="HYPcomStepEditRules" ToolTip="Configurazione criteri" CssClass="icon settings"></asp:HyperLink>
											<asp:HyperLink runat="server" ID="HypcomSubmission" ToolTip="Valutazioni" CssClass="icon evaluate"></asp:HyperLink>
											<asp:HyperLink runat="server" ID="HYPshowALLSubmission" ToolTip="Sottomissioni" CssClass="icon submission"></asp:HyperLink>
											<asp:linkbutton runat="server" id="LKBcomStepAddCom" ToolTip="Nuova commissione" CssClass="icon new"></asp:linkbutton>
											<asp:linkbutton runat="server" id="LKBcomStepDelCom" ToolTip="Cancella commissione" CssClass="icon delete"></asp:linkbutton>
										</span>
									</div>                             
								</ItemTemplate>
							</asp:repeater>
						</div>        
					</div>
				</ItemTemplate>
			</asp:repeater>
		</div>
		
<%--		<div class="stepcontainer addSteps">
			<span class="iconcontainer icons">
					<asp:linkbutton runat="server" id="LKBconfirmReorder" CssClass="linkmenu">Conferma ordinamento</asp:linkbutton>
			</span>
			<asp:HiddenField ID="HiddenField1" ClientIDMode="Static" runat="server" />
		</div>--%>
		
		<div class="stepcontainer last" runat="server" id="DVecoContainer">
			<div class="stepinfo validation">
				<span class="iconcontainer icons">
					<asp:literal ID="LTstepEcoStatus" runat="server"><a href="#" class="icon {0}" title="{1}"></a></asp:literal>
				</span>
				<asp:Label runat="server" ID="LBLstepEcoName" CssClass="title">Ultimo step</asp:Label>
			</div>
			<div class="commissionbox validation">
				
				<asp:Label runat="server" ID="LBLcomEcoName" CssClass="title">Commissione economica</asp:Label>
				<span class="description">
					<asp:literal ID="LTcomEcoDesciption" runat="server">...descrizione commissione...</asp:literal>
				</span>
				<span class="iconcontainer icons">
					<asp:literal ID="LTcomEcostatus" runat="server"><a href="#" class="icon {0}"></a></asp:literal>
					<asp:HyperLink runat="server" ID="HYPcomEcoEditMember" CssClass="icon committeesettings"></asp:HyperLink>
					<%--<asp:linkbutton runat="server" id="LKBcomEcoEditMember" CssClass="icon committeesettings"></asp:linkbutton>--%>
					<%--<asp:linkbutton runat="server" id="LKBcomEcoEditRules" CssClass="icon submission"></asp:linkbutton>--%>
					<%--<asp:HyperLink runat="server" ID="HYPcomEcoEditRules" CssClass="icon submission"></asp:HyperLink>--%>
					<asp:HyperLink runat="server" ID="HYPEcoEvaluate" CssClass="icon submission"></asp:HyperLink>
				</span>
			</div>
		</div>
	</div>

	<asp:literal ID="LTcomStepStatusSource" runat="server" Visible="false"><a href="#" class="icon {0}" title="{1}"></a></asp:literal>
</asp:Content>
