<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_MailSender.aspx.vb" Inherits="Comunita_OnLine.AdminG_MailSender"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLrubricaGlobale" Src="./UC/UC_RubricaMailGlobale.ascx" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE"/>
	<meta http-equiv="PRAGMA" content="NO-CACHE"/>
	<meta http-equiv="expires" content="0"/>

	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>

	<style type="text/css">
		<!--
		#layer1{
			background-color: lightblue;
			height: 100px;
			left: 10px;
			position: absolute;
			top: 50px;
			width: 200px;
			visibility: hidden;
			}
		#layer2{
			background-color: lightblue;
			height: 40px;
			left: 10px;
			position: absolute;
			top: 50px;
			width: 200px;
			visibility: hidden;
			}
		-->
	</style>
	<script type="text/javascript" language="JavaScript" >
		<!--
		function ChangeState(evt, layerRef, state, spX) {
		    var e = (window.event) ? window.event : evt;
		    var PosX, PosY;

		    PosX = e.clientX + spX;
		    PosY = e.clientY + 20;

		    document.getElementById(layerRef).style.visibility = state;
		    document.getElementById(layerRef).style.left = PosX;
		    document.getElementById(layerRef).style.top = PosY;
		}
		function AggiornaFocus() {
		    try {
		        //document.getElementById(TXBBody).focus()
		        document.getElementById(<%=Me.TXBBody.ClientId %>).focus()
		    }
		    catch (e) {
		        return true;
		    }
		}
	    -->
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<div id="layer1" style="LEFT: 0px; POSITION: absolute; TOP: 0px">
			<asp:Label ID="LBLInfoJava" Runat="server">
				Il campo A: (destinatario) puo' contenere un solo indirizzo. <br/> Tutti gli altri verranno inseriti in CCn: (Copia carbone nascosto) automaticamente al momento dell'avvio.
			</asp:Label>
	</div>
	<div id="layer2">
		<asp:Label ID="LBLInfoComJava" Runat="server">
			E' necessario scegliere una comunità per poter inviare la mail.
		</asp:Label>
	</div>

    <table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td class="RigaTitoloAdmin" align="center">
				<asp:Label ID="LBTitolo" Runat="server">- Servizio Mail -</asp:Label>
			</td>
		</tr>--%>
						
		<tr>
			<td align="center">
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign=Center>
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
								
				<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<asp:panel id="PNLemail" Runat="server" Visible="true" HorizontalAlign=Center Width="900">
						<br/>
										
										
						<asp:Table ID="TBLparametri" Runat=server CellPadding="0" CellSpacing="0" Width="800" BorderWidth=0>
							<asp:TableRow ID="TBRfrom" Width="120">
								<asp:TableCell>
									<asp:label id="LBfrom" runat="server" text="Da:" cssclass="titolo_campoSmall"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:textbox id="TXBFrom" runat="server" ReadOnly="true" CssClass="Testo_Mail" Width="450" Columns="60"></asp:textbox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRa" BackColor="#FFC8C8">
								<asp:TableCell>
									<asp:label id="LBa" runat="server" text="A:" cssclass="titolo_campoSmall"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<table width="100%">
										<tr>
											<td>
												<asp:ImageButton ID="IMBrubrica" Runat="server" ImageUrl="./../images/Mailrubrica.jpg" CausesValidation="False" ></asp:ImageButton>&nbsp;
											</td><td>
												<asp:textbox id="TXBa" runat="server" readonly="true" style="WIDTH: 565px" CssClass="Testo_Mail"></asp:textbox>
												<INPUT id="TXBa_n" style="WIDTH: 142px; HEIGHT: 22px" type="hidden" size="18" name="TXBa_n" Runat="server"/>
												<INPUT id="TXBa_n_rlpc" type="hidden" name="TXBa_n_rlpc" Runat="server"/>
											</td>
										</tr>
									</table>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRcc" BackColor="#FFC8C8">
								<asp:TableCell>
									<asp:label id="LBcc" runat="server" text="Cc:" cssclass="titolo_campoSmall"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:textbox id="TXBcc" runat="server" style="WIDTH: 616px" CssClass="Testo_Mail"></asp:textbox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRccn" BackColor="#FFC8C8">
								<asp:TableCell>
									<asp:label id="LBccn" runat="server" text="Ccn:" cssclass="titolo_campoSmall"></asp:label>
								</asp:TableCell>
								<asp:TableCell>
									<table width="100%">
										<tr>
											<td>
												<asp:ImageButton ID="IMBrubricaCCN" Runat="server" ImageUrl="./../images/Mailrubrica.jpg" CausesValidation="False" ></asp:ImageButton>&nbsp;
											</td><td>
												<asp:textbox id="TXBccn" runat="server" style="WIDTH: 565px" CssClass="Testo_Mail" ReadOnly="True"></asp:textbox>
											</td>
										</tr>
									</table>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRrubrica" Visible=false >
								<asp:TableCell CssClass="top">
									<asp:label id="LBaRubrica" runat="server" text="A:" cssclass="titolo_campoSmall"></asp:label>
								</asp:TableCell>
								<asp:TableCell >
													
									<INPUT id="HDNtotaleDestinatari" type="hidden" name="HDNtotaleDestinatari" runat="server"/>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ColumnSpan=2 Height="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRobj">
								<asp:TableCell>
									<asp:label id="LBobj" runat="server" text="Oggetto:" cssclass="titolo_campoSmall"></asp:label>
								</asp:TableCell>
								<asp:TableCell >
									<asp:textbox id="TXBObj" runat="server" CssClass="Testo_Mail" MaxLength="100" Columns="99" Width="597"></asp:textbox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ColumnSpan=2 Height="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow height="23" runat="server" ID="TBRattach">
								<asp:TableCell>
									<asp:label id="LBattach" runat="server" text="Allegati:" cssclass="titolo_campoSmall"></asp:label><br/>&nbsp;
								</asp:TableCell><asp:TableCell>
									<INPUT language="vb" id="fileAllega" type="file" size="80" name="fileAttach" runat="server" class="Testo_Mail"/>&nbsp;
									<asp:button id="BTallega" Runat="server" Text="Allega" CssClass="pulsante" CausesValidation="False"></asp:button>
									<br/>
									<asp:table id="TBLattach" Runat="server" BorderWidth="3"></asp:table>
									<INPUT id="TXBnascosto" type="hidden" name="TXBnascosto" runat="server"/>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ColumnSpan=2 Height="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRbody">
								<asp:TableCell VerticalAlign=Top>
									<table cellpadding="1" cellspacing="1" border="0" style="height:100%">
										<tr>
											<td>
												<asp:label id="LBLBody_t" runat="server" text="Testo:" cssclass="titolo_campoSmall"></asp:label>
											</td>
										</tr>
										<tr>
											<td height="205px">&nbsp;</td>
										</tr>
									</table>
								</asp:TableCell>
								<asp:TableCell>
									<asp:textbox id="TXBbody" runat="server" rows="15" textmode="multiline" Width="790" Columns="97" CssClass="Testo_Mail"></asp:textbox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="TBRinvia">
								<asp:TableCell ColumnSpan=2 HorizontalAlign=Right >
									<table align=right border=0 cellspacing=0>
										<tr>
											<td>
												<asp:CheckBox id="CBXcopiamittente" Runat="server" Text="Invia copia a mittente" Checked=false CssClass="Testo_Mail"></asp:CheckBox>
											</td>
											<td width=30px>&nbsp;</td>
											<td>
												<asp:CheckBox id="CBXricezione" Runat="server" Text="Conferma Ricezione" CssClass="Testo_Mail"></asp:CheckBox>
											</td>
											<td width=30px>&nbsp;</td>
											<td>
												<INPUT id="BTNinvia" class="Pulsante" type="button" value="Invia" name="invia" runat="server" width="80" onserverclick="SendMail"/>
											</td>
										</tr>
									</table>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
												
						</asp:panel>
						<asp:panel id="PNLinviato" Runat="server" Visible="False" HorizontalAlign="Center">
							<br/><br/><br/>
							<table width="500px" align="center">
								<tr>
									<td class="normal" align="center">
										<asp:label ID="LBinviata" Runat=server CssClass="avviso_normal">E-mail inviata con successo</asp:label>
									</td>
								</tr>
								<tr>
									<td align="center">
										<asp:Button id="BTNok" Runat="server" Text="Indietro" CssClass="Pulsante"></asp:Button>
									</td>
								</tr>
							</table>
						</asp:panel>
						<asp:panel id="PNLerrore" Runat="server" Visible="False" HorizontalAlign="Center">
							<br/><br/><br/>
							<table width="500px" align="center">
								<tr>
									<td class="normal" align="center">
										<asp:Label id="LBerrore" Runat="server" CssClass="errore"></asp:Label>
									</td>
								</tr>
								<tr>
									<td align="center"><br/>
										<asp:Button id="BTNokerrore" onclick="ritornadaerrore" Runat="server" Text="ok" CssClass="Pulsante"></asp:Button>
									</td>
								</tr>
							</table>
					</asp:panel>
				</asp:Panel>
				<asp:Panel id="PNLrubrica" Runat="server"  width="530px">
					<table width="530" border="0">
						<tr>
							<td>
								<CTRL:CTRLrubricaGlobale id="CTRLrubrica" runat="server"></CTRL:CTRLrubricaGlobale>
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Button id="BTNcloseRubrica" Runat="server" CssClass="Pulsante" Text="Conferma"></asp:Button>
							</TD>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
	</table>

</asp:Content>



<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
		<title>Comunità On Line - E-Mail</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name=vs_defaultClientScript content="JavaScript"/>
	    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>  

		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		

	
  </head>
  <body bgcolor=#000000>
     <form id="aspnetForm" method="post" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>

			
		<table class="contenitore" align="center">
			<tr class="contenitore">
				<td><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
			</tr>
			
			
			<tr class="contenitore">
				<td colSpan="3">

				</td>
			</tr>
		</table>
		<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>

  </body>
</html>--%>