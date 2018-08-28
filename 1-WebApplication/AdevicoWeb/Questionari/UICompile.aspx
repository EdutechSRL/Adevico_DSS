<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="UICompile.aspx.vb" Inherits="Comunita_OnLine.UICompile_" ValidateRequest="false" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
	Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   <link href="./../Styles.css?v=201604071200lm" type="text/css" rel="stylesheet" />
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link media="screen" href="stileResponseCompile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link media="screen" href="./../Graphics/Modules/Editor/ContentArea/EditorContent_LV.css?v=20180413Adv" type="text/css" rel="StyleSheet" />
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
			width:900px;
		}
	</style>

	<style type="text/css">
		
		div.div_paneltempo_container
		{
			display:none;
			width: 100%;
			position: fixed;
			bottom: 0px;            
			display: none;            
			z-index: 9999;
			text-align:center; 
		}
		
		div.time_visible
		{ display: block; }
		
		
		div.div_paneltempo_container div.DIVpanelTempo
		{
			display: inline-block;            
			text-align: center;            
			position: relative;
			top:0;
			background:yellow;
			font-weight:bold;         
			padding:40px 20px;
			font-size:20px;            
			width:900px;
			}
			
		
		
		div.div_paneltempo_container div.blue_alert  
		{
			background-color: #AAF; 
			background:yellow;
			color:Red;
			}
		div.div_paneltempo_container div.red_alert  
		{
			 background-color: #F77; 
			background:Red;
			color:yellow; 
			 }
		/*
		div.panel_tempo_container
		{
			top: 100px;
			height: 45px;
			width: 200px;
			border: 1px solid black;
			padding: 2px;
			text-align: center;
			z-index: 200;
		}
		
		div.panel_tempo_container span.time_text
		{
			
			}
			
		
		
		.DIVpanelTimer { background-color: White;}*/
		
		/*
		div#header
		{
			top: 23px;
			z-index: 10;
		}

		div#header *
		{
			z-index: 10;
		}*/
		
	</style>
	<%--
	jQuery.fn.outerHTML = function () {
			return $('<div>').append(this.eq(0).clone()).html();
		};

		$(document).ready(function () {

			var pass = "Password  ";
			  if ($.browser.msie) {
			  pass = "Password ";
			}
			var Cpass = "Conferma Password  ";
			if ($.browser.msie) {
				Cpass = "Conferma Password ";
			}
			setPassField(pass);
			setPassField(Cpass);
		});
		 

		 function setPassField(value) {

			var spanPassword = $("span").filter(function () {
				return $(this).text() == value;
			});

			var tablePassword = spanPassword.parents("table").first();

			tablePassword.find("input[type='checkbox']").attr("checked", "true").hide();

			var name = spanPassword.parents("td.Risposte").first().children("input:last").attr("name");

			if ($.browser.msie) {
				var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");

				var html = oldInput.outerHTML();

				html = html.replace("<INPUT", '<INPUT type="password"');

				var newInput = $(html);
				var myName = oldInput.attr("name")
				//newInput.attr("type", "password");

				newInput.attr("name", "new");

				newInput.insertBefore(oldInput);

				oldInput.remove();

				newInput.attr("name", myName);
			} else {
				var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");
				var newInput = oldInput.clone();
				var myName = oldInput.attr("name")
				//oldInput.attr("type", "password");
				newInput.attr("type", "password");
				newInput.attr("name", "new");
				newInput.insertBefore(oldInput);
				oldInput.remove();
				newInput.attr("name", myName);
			}

		}--%>
	 <script type="text/javascript">
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
				 //                minWidth: 700,
				 zIndex: 1000,
				 open: function (type, data) {
					 //                $(this).dialog('option', 'width', 700);
					 //                $(this).dialog('option', 'height', 600);
					 //$(this).parent().appendTo("form");
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
				 //                minWidth: 700,
				 zIndex: 1000,
				 open: function (type, data) {
					 //                $(this).dialog('option', 'width', 700);
					 //                $(this).dialog('option', 'height', 600);
					 //$(this).parent().appendTo("form");
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
		 });
	</script>      
 <script type="text/javascript">
	 var secs;
	 var timerID = null;
	 var timerRunning = false;
	 var delay = 1000;
	 var tempo;
	 var HIDtempo;
	 var isStart = true;
	 var tempoBlu = 300;  //300;
	 var tempoRosso = 60; // 60;

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
		 //el = document.getElementById("< %=me.DIVpanelTempo.clientId % >");
		 el = document.getElementById("DIVpanelTempo");
		 
		 starter = document.getElementById("<%=me.HIDstarter.clientId %>");
		 HIDtempo = document.getElementById("<%=me.HIDtempoRimanente.clientId %>");

		 if (starter.value == "1") {
			 //alert("Starter = 1");

			 if (isStart) {
				 el.style.display = "block";
				 //el.style.backgroundColor = "white";
				 secs = HIDtempo.value - 0.5;
				 isStart = false;
			 }
			 if (secs < tempoBlu && secs > tempoRosso) {
				 //el.style.backgroundColor = "blue";
				 //el.style.color = "white";
				 el.className = "DIVpanelTempo blue_alert";
				 //alert(el.className.toString());
			 }
			 else {
				 if (secs < tempoRosso && secs > 0) {
					 //                        if ( el.style.backgroundColor=="white" )
					 {
						 //el.style.backgroundColor = "red";
						 el.className = "DIVpanelTempo red_alert";
						 //alert(el.className.toString());
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
			 //"<span class=\"time_text\">" + + "<\span>"
			 el.innerHTML = "<%=me.HIDmessaggio.value %>".replace("{secondi}", minSec.toLocaleTimeString());
		 } else {
			 //alert("Starter != 1");
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PreHeaderContent" runat="server">
		<div class="NEW_upper">
			<div id="DIVpanelTimer" runat="server">
				<div id="DIVpanelTempo_Container"  runat="server">
					<div id="DIVpanelTempo" class="DIVpanelTempo">
						<asp:Label ID="LBTempoRimanente" Visible="false" runat="server" CssClass="remainingTime" />
					</div>
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
		<asp:Timer ID="TMSessione" runat="server" Enabled="true">
		</asp:Timer>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
<div id="QuestionariCompile">
	 <input id="HIDmessaggio" runat="server" type="hidden" />
		<div class="NEW_containerQuest" runat="server" id="DIVContainer">
			<div>
				<asp:MultiView runat="server" ID="MLVquestionari">
					<%--attenzione!! non spostare VIWdati, oppure correggere tutti i riferimenti a MLVquestionari.activeViewIndex nel vb--%>
					<asp:View ID="VIWdati" runat="server">
						<asp:Panel ID="PNLmenu" runat="server" CssClass="panelMenu" >
							<asp:LinkButton ID="LNBdescrizione" runat="server" CssClass="Link_Menu" Visible="true"></asp:LinkButton>
						</asp:Panel>
						<asp:label ID="LBisMandatoryInfoTop" runat="server"></asp:label>
						<asp:Label ID="LBTroppeRispostePagina" Visible="false" runat="server" CssClass="Errore"></asp:Label>
						<asp:Label runat="server" ID="LBnoRisposta" CssClass="Errore" Visible="false"></asp:Label>
						<asp:PlaceHolder runat="server" ID="PHucValutazione"></asp:PlaceHolder>
						<asp:Panel ID="PNLElenco" runat="server" CssClass="elenco">
							<asp:DataList ID="DLPagine" ShowFooter="true" runat="server" DataKeyField="id" CssClass="datalistPagine" RepeatLayout="Flow">
								<ItemTemplate>
									<div class="NomePagina" id="DVpageName" runat="server">
										<h3>
											<%#Eval("nomePagina")%>
										</h3>
									</div>
									<div class="TestoDomanda" id="DVpageDescription" runat="server">
										<i>
											<%#Eval("descrizione")%>
										</i>
									</div>
									<asp:DataList ID="DLDomande" runat="server" DataKeyField="id" 
										OnItemDataBound="loadDomandeOpzioni"
										CssClass="datalistDomande"
										RepeatLayout="Flow">
										<ItemTemplate>
											<div class="ContenitoreDomanda0" runat="server" id="DIVDomanda">
												<a name='<%#Eval("numero")%>'></a>
												<div class="TestoDomanda">
													<div class='<%-- #Me.displayDifficulty --%>hide difficultyInfo'>
														(Cod.<%#Eval("id")%>
														<asp:Label runat="server" ID="LBDifficoltaTesto" Text='<%#Eval("difficoltaTesto")%>'></asp:Label>)
														<%--Visible='<%# me.showDifficulty %>'--%>
													</div>
													 <div class="question-number" title="<%#MandatoryToolTip(Container.Dataitem)%>">
														<asp:Label ID="LBquestionNumber" runat="server" Text='<%#Eval("numero") & "."%>'></asp:Label>
														 <%#MandatoryDisplay(Container.Dataitem)%>
													</div>
													<div class="question-name"><%#me.SmartTagsAvailable.TagAll(Eval("testo"))%></div>
												</div>
											</div>
											<div class="Risposte">
												<asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
											</div>
											<asp:Label runat="server" ID="LBSuggerimento" Text='<%#Eval("suggerimento")%>' Visible="false"></asp:Label>
										</ItemTemplate>
										<FooterStyle CssClass="footer"/>
										<SelectedItemStyle CssClass="item-question Selected"/>
										<AlternatingItemStyle CssClass="item-question Alternate"/>
										<ItemStyle CssClass="item-question"/>
										<HeaderStyle CssClass="header"/>
									</asp:DataList>
									<div class="NomePaginaFooter" id="DVpageNameBottom" runat="server">
										<%#Eval("nomePagina")%>
									</div>
								</ItemTemplate>
								<FooterStyle CssClass="footer"/>
								<SelectedItemStyle CssClass="item-page Selected"/>
								<AlternatingItemStyle CssClass="item-page alternate" />
								<ItemStyle CssClass="item-page"/>
								<HeaderStyle CssClass="header"/>
							</asp:DataList>
							<%--inserire qui RPTrisposte dell'ucStatGen--%>
						</asp:Panel>
						<div id="DIVNumeriPagina" class="numeriPagina">
							<asp:Label ID="LBpagina" runat="server"></asp:Label>
							<%--<asp:ImageButton ID="IMBprima" ImageUrl="img/indietro.gif" runat="server" Visible="False">
							</asp:ImageButton>--%>
							<asp:LinkButton ID="LkbBack" runat="server" CssClass="button prev" Visible="false"></asp:LinkButton>
							<asp:PlaceHolder ID="PHnumeroPagina" runat="server"></asp:PlaceHolder>
							<asp:LinkButton ID="LkbNext" runat="server" CssClass="button next" Visible="false"></asp:LinkButton>
							<%--<asp:ImageButton ID="IMBdopo" runat="server" ImageUrl="img/avanti.gif" Visible="False"></asp:ImageButton>--%>
						</div>
						<asp:label ID="LBisMandatoryInfoBottom" runat="server"></asp:label>
						<span class="footerInfo">
							<span class="labelContainer">
								<asp:Label runat="server" ID="LBAvvisoSalva" Visible="true"></asp:Label>
								<asp:Literal ID="LTsaveAndExit" runat="server"><br /></asp:Literal>
								<asp:Button runat="server" ID="BTNSalvaEEsci" Visible="false" />
								<asp:Button runat="server" ID="BTNSalvaContinua" Visible="true" />
							</span>
							<asp:Label runat="server" ID="LBAvvisoFine" Visible="true"></asp:Label>
							<span class="buttonContainer">
								<asp:Button runat="server" ID="BTNDopo" Visible="false" />
								<asp:Button runat="server" ID="BTNFine" Visible="true" />
							</span>
						</span>
					</asp:View>
					<asp:View ID="VIWdescrizione" runat="server">
						<asp:Panel ID="PNLIndietro" runat="server"  class="divBack" Visible="false">
							<asp:LinkButton ID="LNBindietro" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
						</asp:Panel>
                        <br /> 
						<asp:Label ID="LBnomeVIWDescrizione" runat="server" Text='<%#Eval("nome")%>' CssClass="RigaTitolo"></asp:Label>
                        <br /><br /><br />
						<asp:Label ID="LBdescrizioneVIWDescrizione" runat="server" Text='<%#Eval("descrizione")%>'></asp:Label>
						<br />
						<br />
						<br />
						<asp:Label ID="LBTempoRimanenteVIWDescrizione" runat="server" Visible="false"></asp:Label>
						<asp:Label ID="LBAvvisoRispostaNonEditabile" runat="server" Visible="false"></asp:Label>
						<asp:Label ID="LBdurata" runat="server" Visible="false"></asp:Label>
						<br />
						<br />
						<p class="buttonContainer">
							<span class="passwordContainer">
								<asp:Label runat="server" ID="LBLPassword" Text="Password: " Visible='<%#Eval("isPassword")%>'></asp:Label>
								<asp:TextBox ID="TXBPassword" runat="server"></asp:TextBox>
								<asp:Label runat="server" ID="LBLErrorePassword" Style="display: none;" cssclass="labelPasswordError"></asp:Label>
							</span>
							<asp:Button ID="BTNinizia" runat="server" Text="" ForeColor="Black"></asp:Button>
							<asp:Button ID="BTNIniziaFacile" Visible="false" runat="server" Text="" ForeColor="Black">
							</asp:Button>
							<asp:Button ID="BTNIniziaMedio" Visible="false" runat="server" Text="" ForeColor="Black">
							</asp:Button>
							<asp:Button ID="BTNIniziaDifficile" Visible="false" runat="server" Text="" ForeColor="Black">
							</asp:Button>
							<asp:Button ID="BTNIniziaMisto" Visible="false" runat="server" Text="" ForeColor="Black">
							</asp:Button>
						</p>
					</asp:View>
					<asp:View runat="server" ID="VIWmessaggi">
						<br />
						<br />
						<CTRL:Messages runat="server" ID="CTRLerrorMessages" Visible="false" />
						<asp:Label ID="LBConferma" runat="server" Visible="false"></asp:Label>
						<br />
						<asp:Button ID="BTNRestartAutoEval" runat="server" Visible="false" />
					</asp:View>
				</asp:MultiView>
			</div>
		</div>
	<div class="dialog dlgconfirmsubmit" runat="server" id="DVconfirmSubmit" visible="false">                
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
	<asp:Literal ID="LTcloseDialogCssClass" runat="server" Visible="false">closedlgconfirmsubmit</asp:Literal><asp:Literal ID="LTdlgconfirmsubmit" runat="server" Visible="false">dlgconfirmsubmit</asp:Literal><asp:Literal ID="LTconfirmDialogCssClass" runat="server" Visible="false">opendlgconfirmsubmit</asp:Literal><asp:Literal ID="LTconfirmExitDialogCssClass" Visible="false" runat="server">opendlgconfirmexit</asp:Literal>
</div>
</asp:Content>