<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
	CodeBehind="QuestionarioCompile.aspx.vb" Inherits="Comunita_OnLine.QuestionarioCompile"
	ValidateRequest="false" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
	Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link media="screen" href="stileResponseCompile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link media="screen" href="./../Graphics/Modules/Editor/ContentArea/EditorContent_LV.css?v=20180413Adv" type="text/css" rel="StyleSheet" />
   <style>
		div#ctl00_CPHservice_PNLmenu,
		div#ctl00_CPHservice_DIVSalvaQuestionario
		{
			display:none;
		}
   </style>
<%--     <script type="text/javascript">
		 $(function () {
			 $('.dialog.dlgconfirmsubmit').dialog({
				 appendTo: "form",
				 closeOnEscape: false,
				 autoOpen: false,
				 draggable: true,
				 modal: true,
				 title: "",
				 width: 600,
				 height: 280,
				 minHeight: 200,
				 zIndex: 1000,
				 open: function (type, data) {
					  $(".ui-dialog-titlebar-close", this.parentNode).hide();
				 }

			 });
			 $(".opendlgconfirmsubmit").click(function () {
				 $(".dialog.dlgconfirmsubmit").dialog("open");
				 return false;
			 });

			 $(".closedlgconfirmsubmit").click(function () {
				 $(".dialog.dlgconfirmsubmit").dialog("close");
				 return false;
			 });

			 $('.dialog.dlgconfirmexit').dialog({
				 appendTo: "form",
				 closeOnEscape: false,
				 autoOpen: false,
				 draggable: true,
				 modal: true,
				 title: "",
				 width: 600,
				 height: 280,
				 minHeight: 200,
				 zIndex: 1000,
				 open: function (type, data) {
					 $(".ui-dialog-titlebar-close", this.parentNode).hide();
				 }

			 });
			 $(".opendlgconfirmexit").click(function () {
				 $(".dialog.dlgconfirmexit").dialog("open");
				 return false;
			 });

			 $(".closedlgconfirmexit").click(function () {
				 $(".dialog.dlgconfirmexit").dialog("close");
				 return false;
			 });

			 $('.dialog.dlgundo').dialog({
				 appendTo: "form",
				 closeOnEscape: false,
				 autoOpen: false,
				 draggable: true,
				 modal: true,
				 title: "",
				 width: 600,
				 height: 280,
				 minHeight: 200,
				 zIndex: 1000,
				 open: function (type, data) {
					 $(".ui-dialog-titlebar-close", this.parentNode).hide();
				 }

			 });
			
			 $(".opendlgundo").click(function () {
				 $(".dialog.dlgundo").dialog("open");
				 return false;
			 });
			 $(".closedlgundo").click(function () {
				 $(".dialog.dlgundo").dialog("close");
				 return false;
			 });
		 });
	</script>--%>    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="Server">
<div id="QuestionariCompile">
	<%--<script src="/Elle3/Jscript/jquery-1.4.1.min.js"></script>--%>
	<style>
		.paneltimer
		{          
			display:none; 
			width:100%;            
			position:fixed;
			bottom:0;            
			left:0;
			z-index:9999;      
			text-align:center;      
		}
		
		.paneltempo
		{            
			background:yellow;
			font-weight:bold;         
			padding:40px 20px;
			font-size:20px;            
			
		}
	</style>

  
	<script type="text/javascript">


		//        jQuery.fn.outerHTML = function () {
		//            return $('<div>').append(this.eq(0).clone()).html();
		//        };

		//        $(document).ready(function () {

		//            var pass = "Password  ";
		//            if ($.browser.msie) {
		//                pass = "Password ";
		//            }
		//            var Cpass = "Conferma Password  ";
		//            if ($.browser.msie) {
		//                Cpass = "Conferma Password ";
		//            }
		//            setPassField(pass);
		//            setPassField(Cpass);
		//        });


		//        function setPassField(value) {

		//            var spanPassword = $("span").filter(function () {
		//                return $(this).text() == value;
		//            });

		//            var tablePassword = spanPassword.parents("table").first();

		//            tablePassword.find("input[type='checkbox']").attr("checked", "true").hide();

		//            var name = spanPassword.parents("td.Risposte").first().children("input:last").attr("name");

		//            if ($.browser.msie) {
		//                var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");

		//                var html = oldInput.outerHTML();

		//                html = html.replace("<INPUT", '<INPUT type="password"');

		//                var newInput = $(html);
		//                var myName = oldInput.attr("name")
		//                //newInput.attr("type", "password");

		//                newInput.attr("name", "new");

		//                newInput.insertBefore(oldInput);

		//                oldInput.remove();

		//                newInput.attr("name", myName);

		//            } else {
		//                var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");
		//                var newInput = oldInput.clone();
		//                var myName = oldInput.attr("name")
		//                //oldInput.attr("type", "password");
		//                newInput.attr("type", "password");
		//                newInput.attr("name", "new");
		//                newInput.insertBefore(oldInput);
		//                oldInput.remove();
		//                newInput.attr("name", myName);
		//            }

		//        }

		var secs;
		var timerID = null;
		var timerRunning = false;
		var delay = 1000;
		var tempo;
		var HIDtempo;
		var isStart = true;
		var tempoBlu = 300;
		var tempoRosso = 60;

		window.onload = checkTimer;

		function setValue(value) {
			starter = document.getElementById('<%=me.HIDstarter.clientId %>');
			starter.value = value;
		}

		function InitializeTimer() {
			// Set the length of the timer, in seconds
			secs = 60;
			StopTheClock();
			Ticks();
		}

		function StopTheClock() {
			if (timerRunning) {
				clearTimeout(timerID);
			}
			timerRunning = false;
		}

		function Ticks() {
			el = document.getElementById("<%=me.DIVpanelTempo.clientId %>");
			starter = document.getElementById("<%=me.HIDstarter.clientId %>");
			HIDtempo = document.getElementById("<%=me.HIDtempoRimanente.clientId %>");

			if (starter.value == "1") {
				if (isStart) {                                        
					//el.style.backgroundColor = "white";
					secs = HIDtempo.value - 0.5;
					isStart = false;
				}
				if (secs < tempoBlu && secs > tempoRosso) {
					el.style.backgroundColor = "yellow";
					el.style.color = "red";
				}
				else {
					if (secs < tempoRosso && secs > 0) {
						//                        if ( el.style.backgroundColor=="white" )
						{
							el.style.backgroundColor = "red";
							el.style.color = "yellow";
						}
						//                        else
						//                            {
						//                                el.style.backgroundColor="white";
						//                            } 
					}
					else {
						if (secs < 0) {
							secs = 0;
						}
					}
				}
				minSec = new Date(0, 0, 0, 0, 0, secs);
				el.innerHTML = "<%=me.HIDmessaggio.value %>".replace("{secondi}", minSec.toLocaleTimeString());
			}


			self.status = secs;
			secs = secs - 0.5;
			timerRunning = true;
			timerID = self.setTimeout("Ticks()", delay);
		}



		function checkTimer() {
			InitializeTimer();
			HIDtempo = document.getElementById('<%=me.HIDtempoRimanente.clientId %>');
			tempo = HIDtempo.value

			Ticks();
		}
	</script>

	   
	<div>
		 <CTRL:Messages runat="server" ID="CTRLmessage" Visible="false" />
		<input id="HIDmessaggio" runat="server" type="hidden" />
		<div>
			<div>
				<div id="DIVpanelTimer" class="paneltimer" runat="server">
					<div id="DIVpanelTempo" class="paneltempo" runat="server" style="display:none;">
						<asp:Label ID="LBTempoRimanente" Visible="false" runat="server" CssClass="remainingTime" />
					</div>
					<asp:UpdatePanel ID="UPTempo" runat="server">
						<ContentTemplate>
							<input id="HIDtempoRimanente" runat="server" value="0" type="hidden" />
							<input id="HIDstarter" runat="server" value="0" type="hidden" />
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="TMDurata" EventName="Tick" />
							<asp:AsyncPostBackTrigger ControlID="TMSessione" EventName="Tick" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
			</div>
			<asp:Timer ID="TMDurata" runat="server" Enabled="false">
			</asp:Timer>
			<asp:Timer ID="TMSessione" runat="server" Enabled="false">
			</asp:Timer>
			<div>
				<div>
					<asp:MultiView runat="server" ID="MLVquestionari">
						<%--attenzione!! non spostare VIWdati, oppure correggere tutti i riferimenti a MLVquestionari.activeViewIndex nel vb--%>
						<asp:View ID="VIWdati" runat="server">
							<asp:Panel ID="PNLmenu" runat="server" CssClass="panelMenu" Visible="true">
								<asp:LinkButton ID="LNBdescrizione" runat="server" CssClass="Link_Menu" Visible="false"> </asp:LinkButton>
								<asp:LinkButton ID="LNBannulla" Visible="true" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
								<asp:LinkButton ID="LNBTornaHome" Visible="true" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
								<asp:LinkButton ID="LNBFinito" Visible="false" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
								<asp:LinkButton ID="LNBsalvaEsci" Visible="false" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
								<asp:LinkButton ID="LNBsalvaContinua" runat="server" CssClass="Link_Menu" Visible="false"> </asp:LinkButton>
							</asp:Panel>
							<br />
						   <asp:Label ID="LBname" runat="server" CssClass="RigaTitolo"></asp:Label>
							<br /><br /><br />
							<asp:label ID="LBisMandatoryInfoTop" runat="server"></asp:label>
                            <asp:label ID="LBMandatoryNotAnswered" runat="server" CssClass="Errore" Visible="false"></asp:label>
							<asp:Label ID="LBTroppeRispostePagina" Visible="false" runat="server" CssClass="Errore"> </asp:Label>
							<asp:Label runat="server" ID="LBnoRisposta" CssClass="Errore" Visible="false"></asp:Label>
							<asp:PlaceHolder runat="server" ID="PHucValutazione"></asp:PlaceHolder>
							<asp:Panel ID="PNLElenco" runat="server" CssClass="elenco">
								<asp:DataList ID="DLPagine" CssClass="datalistPagine" ShowFooter="true" runat="server" DataKeyField="id" RepeatLayout="Flow" >
									<ItemTemplate>
										<div class="NomePagina" id="DVpageName" runat="server">
											<h3>
												<%#Eval("nomePagina")%>
											</h3>
										</div>
										<div class="TestoDomanda" id="DVpageDescription" runat="server">
											<%#Eval("descrizione")%>
										</div>
										
										<asp:DataList ID="DLDomande" runat="server" DataKeyField="id" 
											OnItemDataBound="loadDomandeOpzioni" CssClass="datalistDomande"
											RepeatLayout="Flow">
											<ItemTemplate>
												<div class="ContenitoreDomanda0" runat="server" id="DIVDomanda">
													<a name='<%#Eval("numero")%>'></a>
													<div class="TestoDomanda">
														
														<div class="<%-- Me.displayDifficulty--%>hide difficultyInfo"> <%--runat="server" id="DIVCode">--%><!--hide = nascosta, show = visibile-->
															(Cod.<%#Eval("id")%>
															<asp:Label runat="server" ID="LBDifficoltaTesto" Text='<%#Eval("difficoltaTesto")%>'></asp:Label>)
														</div>
														<div class="question-number" title="<%#MandatoryToolTip(Container.Dataitem)%>">
															<asp:Label runat="server" Text='<%#Eval("numero") & "."%>' ></asp:Label><%--Visible='<%# me.showDifficulty %>'--%>
															 <%#MandatoryDisplay(Container.Dataitem)%>
														</div>
														<div class="question-name"><%#Me.SmartTagsAvailable.TagAll(Eval("testo"))%></div>                                                        
													</div>
											   
													<div class="Risposte">
														<asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
													</div>
													<div class="suggestion">
														<asp:Label runat="server" ID="LBsuggerimentoOpzione" Visible="false" CssClass="option"></asp:Label>
														<asp:Label runat="server" ID="LBSuggerimento" Text='<%#Eval("suggerimento")%>' Visible="false" CssClass="global"></asp:Label>
													</div>
												</div>
											</ItemTemplate>
											<FooterStyle CssClass="footer"/>
											<SelectedItemStyle CssClass="item-question Selected"/>
											<AlternatingItemStyle CssClass="item-question Alternate"/>
											<ItemStyle CssClass="item-question"/>
											<HeaderStyle CssClass="header"/>
										</asp:DataList>
										<br />
										<div class="NomePaginaFooter" runat="server" id="DIVNomePaginaFooter">
											<%#Eval("nomePagina")%>
										</div>
									</ItemTemplate>
									<FooterStyle CssClass="footer"/>
									<SelectedItemStyle CssClass="item-page Selected"/>
									<AlternatingItemStyle CssClass="item-page alternate" />
									<ItemStyle CssClass="item-page"/>
									<HeaderStyle CssClass="header"/>
								</asp:DataList>
							</asp:Panel>
							<br />
							<div id="DIVNumeriPagina" runat="server" class="numeriPagina">
								<asp:Label ID="LBpagina" runat="server"></asp:Label>
								<asp:LinkButton ID="LkbBack" runat="server" CssClass="button prev" Visible="false"></asp:LinkButton>
								<%--<asp:ImageButton ID="IMBprima" CssClass="button back" ImageUrl="img/indietro.gif" runat="server" Visible="False">
								</asp:ImageButton>--%>
								<asp:PlaceHolder ID="PHnumeroPagina" runat="server"></asp:PlaceHolder>
								<asp:LinkButton ID="LkbNext" runat="server" CssClass="button next" Visible="false"></asp:LinkButton>
								<%--<asp:ImageButton ID="IMBdopo" runat="server" CssClass="button next" ImageUrl="img/avanti.gif" Visible="False">
								</asp:ImageButton>--%>
								<asp:Button runat="server" ID="BTNDopo" EnableViewState="False" Visible="false" />

								<br />
							</div>
							<asp:label ID="LBisMandatoryInfoBottom" runat="server"></asp:label>
							<div runat="server" id="DIVSalvaQuestionario">
								<span class="footerInfo">
									<asp:Label runat="server" ID="LBAvvisoSalva" Visible="true"></asp:Label>
									<asp:Literal ID="LTsaveAndExit" runat="server"><br /></asp:Literal>
									<asp:Button runat="server" ID="BTNSalvaEEsci"  Visible="false" />
									<asp:Button runat="server" ID="BTNSalvaContinua" />
								</span>
							</div>
							<asp:Label runat="server" ID="LBAvvisoFine" Visible="true"></asp:Label>
							<asp:Button runat="server" ID="BTNFine" Visible="true" />
						</asp:View>
						<asp:View ID="VIWdescrizione" runat="server">
							<asp:Panel ID="PNLIndietro" runat="server" class="divBack" Visible="false">
							<asp:LinkButton ID="LNBindietro" Visible="true" runat="server" CssClass="Link_Menu"> </asp:LinkButton>&nbsp;</asp:Panel>
							<asp:Label ID="LBnomeVIWDescrizione" runat="server" Text='<%#Eval("nome")%>' CssClass="lbNome"></asp:Label>
							<asp:Label ID="LBdescrizioneVIWDescrizione" runat="server" CssClass="lbdescrizione"> </asp:Label>
							<asp:Label ID="LBTempoRimanenteVIWDescrizione" runat="server" Visible="false"></asp:Label>
							<asp:Label ID="LBdurata" runat="server" Visible="false"></asp:Label>
							<p class="buttonContainer">
								<asp:Button ID="BTNinizia" runat="server" Text="" CssClass="button start"></asp:Button>
								<asp:Button ID="BTNIniziaFacile" Visible="false" runat="server" Text="" CssClass="button start">
								</asp:Button>
								<asp:Button ID="BTNIniziaMedio" Visible="false" runat="server" Text="" CssClass="button start">
								</asp:Button>
								<asp:Button ID="BTNIniziaDifficile" Visible="false" runat="server" Text="" CssClass="button start">
								</asp:Button>
								<asp:Button ID="BTNIniziaMisto" Visible="false" runat="server" Text="" CssClass="button start">
								</asp:Button>
							</p>
						</asp:View>
						<asp:View runat="server" ID="VIWmessaggi">
							<asp:Panel ID="PNLTornaLista" runat="server" class="panelBack">
								<asp:LinkButton ID="LNBTornaLista" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
								<asp:HyperLink id="HYPnewAttempt" runat="server" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
							</asp:Panel>
							<CTRL:Messages runat="server" ID="CTRLerrorMessages" Visible="false" />
							<asp:Label ID="LBConferma" runat="server" Visible="false"></asp:Label>
							<asp:Button ID="BTNRestartAutoEval" runat="server" Visible="false" />
							<asp:Button runat="server" ID="BTNSalvaAutovalutazione" Visible="false" />
						</asp:View>
					</asp:MultiView>
				</div>
			</div>
		</div>
	</div>
	<div class="dialog dlgconfirmsubmit" runat="server" id="DVconfirmSubmit" visible="true">                
		<div class="fieldobject">
			<div class="fieldrow title">
				<div class="description">
					<asp:Label ID="LBconfirmOptions" runat="server">*</asp:Label>
				</div>                        
			</div>
			<div class="fieldrow commandoptions clearfix">
				<div class="commandoption left">
					<asp:Button Text="* Annulla" runat="server" CssClass="commandbutton editoption1 closedlgconfirmsubmit" ID="BTNundoOption" />
					<asp:Label ID="LBundoOption" runat="server" CssClass="commanddescription" >* Annulla </asp:Label>
				</div>
				<div class="commandoption right">
					<asp:Button Text="* Conferma " runat="server" CssClass="commandbutton editoption2" ID="BTNconfirmOption" />
					<asp:Label ID="LBconfirmOption" runat="server" CssClass="commanddescription" >*</asp:Label>
				</div>
			</div>
		</div>
		<input type="hidden" id="HDNcurrentTime" runat="server" />
	</div>
	<div class="dialog dlgconfirmexit" runat="server" id="DVconfirmExit" visible="false">                
		<div class="fieldobject">
			<div class="fieldrow title">
				<div class="description">
					<asp:Label ID="LBconfirmExitOptions" runat="server">*</asp:Label>
				</div>                        
			</div>
			<div class="fieldrow commandoptions clearfix">
				<div class="commandoption left">
					<asp:Button Text="* Annulla" runat="server" CssClass="commandbutton editoption1 closedlgconfirmexit" ID="BTNundoExitOption" />
					<asp:Label ID="LBundoExitOption" runat="server" CssClass="commanddescription" >* Annulla </asp:Label>
				</div>
				<div class="commandoption right">
					<asp:Button Text="* Conferma " runat="server" CssClass="commandbutton editoption2" ID="BTNconfirmExitOption" />
					<asp:Label ID="LBconfirmExitOption" runat="server" CssClass="commanddescription" >*</asp:Label>
				</div>
			</div>
		</div>
	</div> 
	<div class="dialog dlgundo" runat="server" id="DVundoExit" visible="false">                
		<div class="fieldobject">
			<div class="fieldrow title">
				<div class="description">
					<asp:Label ID="LBundoActionMessage" runat="server">*</asp:Label>
				</div>                        
			</div>
			<div class="fieldrow commandoptions clearfix">
				<div class="commandoption left">
					<asp:Button Text="* Annulla" runat="server" CssClass="commandbutton editoption1 closedlgundo" ID="BTNundoLeaveQuestionnaireOption" />
					<asp:Label ID="LBundoLeaveQuestionnaireOption" runat="server" CssClass="commanddescription" >* Annulla </asp:Label>
				</div>
				<div class="commandoption right">
					<asp:Button Text="* Conferma " runat="server" CssClass="commandbutton editoption2" ID="BTNconfirmLeaveQuestionnaireOption" />
					<asp:Label ID="LBconfirmLeaveQuestionnaireOption" runat="server" CssClass="commanddescription" >*</asp:Label>
				</div>
			</div>
		</div>
	</div> 
	<asp:HiddenField ID="hidUsr" runat="server" />
	<%-- literal con le classi per attivare i pop-up di conferma: --%>
	<asp:Literal ID="LTopenUndoDialogCssClass" runat="server" Visible="false">_opendlgundo_</asp:Literal><asp:Literal ID="LTcloseDialogCssClass" runat="server" Visible="false">_closedlgconfirmsubmit_</asp:Literal><asp:Literal ID="LTdlgconfirmsubmit" runat="server" Visible="false">_dlgconfirmsubmit_</asp:Literal><asp:Literal ID="LTconfirmDialogCssClass" runat="server" Visible="false">_opendlgconfirmsubmit_</asp:Literal><asp:Literal ID="LTconfirmExitDialogCssClass" Visible="false" runat="server">_opendlgconfirmexit_</asp:Literal>
</div>
</asp:Content>