<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="MailSender_home.aspx.vb" Inherits="Comunita_OnLine.MailSender_home"%>

<%--
--%>
<%@ Register TagPrefix="CTRL" TagName="CTRLrubrica" Src="./UC/UC_RubricaMail_NEW.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsorgenteComunita" Src="./../UC/UC_FiltroComunitaByServizio_NEW.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<META HTTP-EQUIV="CACHE-CONTROL" CONTENT="NO-CACHE"/>
	<META HTTP-EQUIV="PRAGMA" CONTENT="NO-CACHE"/>
	<meta http-equiv="expires" content="0"/>
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
		            document.getElementById(TXBBody).focus()
		        }
		        catch (e) {
		            return true;
		        }
		    }
		//-->
	</script>
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
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
    <table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">

		<tr class="contenitore">
			<td colSpan="3">
				<table cellSpacing="0" cellPadding="0" width="900" border="0">
<%--					<tr>
						<td class="RigaTitolo" align="left">
							<asp:Label ID="LBTitolo" Runat="server">Servizio Mail</asp:Label>
						</td>
					</tr>--%>
					<tr>
						<td align="center">
							<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign="Center">
								<TABLE align=center>
									<TR>
										<TD height="50">&nbsp;</TD>
									</TR><TR>
										<TD align=center>
											<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label>
										</TD>
									</TR><TR>
										<TD height="50">&nbsp;</TD>
									</TR>
								</TABLE>
							</asp:Panel>
							<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" width="900px">
								<br/>
								<asp:Table id="TBLContenuto" Runat="server" CellPadding="0" CellSpacing="0">
									<asp:TableRow ID="TBRcomunita">
										<asp:TableCell width="80px" VerticalAlign="Bottom">
											<asp:Label ID="LBcomunita_t" Runat="server" CssClass="titolo_campoSmall">Comunità:</asp:Label>
										</asp:TableCell>
										<asp:TableCell ColumnSpan="2">
											<CTRL:CTRLsorgenteComunita id="CTRLsorgenteComunita" runat="server" Width="800px" LarghezzaFinestraAlbero="800px" ColonneNome="100"></CTRL:CTRLsorgenteComunita>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRfrom">
										<asp:TableCell>
											<asp:label id="LBfrom" runat="server" text="Da:" CssClass="titolo_campoSmall"></asp:label>
										</asp:TableCell>
										<asp:TableCell ColumnSpan="2">
											<asp:textbox id="TXBFrom" runat="server" ReadOnly="true" CssClass="Testo_Mail" Width="450" Columns="60"></asp:textbox>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRa" CssClass="mailFieldTo">
										<asp:TableCell>
											<asp:label id="LBa" runat="server" text="A:" CssClass="titolo_campoSmall"></asp:label>
											</asp:TableCell>
										<asp:TableCell VerticalAlign="Bottom" Width="650">
											<table cellpadding="0" cellspacing="0" border="0" Width="650">
												<tr>
													<td>
														<asp:ImageButton ID="IMBrubrica" Runat="server" ImageUrl="./../images/Mailrubrica.jpg" CausesValidation="False" ></asp:ImageButton>&nbsp;
													</td><td>
														<asp:textbox id="TXBa" runat="server" readonly="true" style="WIDTH: 565px" CssClass="Testo_Mail"></asp:textbox>
													</td><td>
														<asp:Label ID="LBLInfoA" Runat="server" Visible="False" ForeColor="#ff0000" Font-Bold="True" ></asp:Label>
													</td>
												</tr>
											</table>
											<INPUT id="TXBa_n" type="hidden" size="18" name="TXBa_n" Runat="server"/>
											<INPUT id="TXBa_n_rlpc" type="hidden" name="TXBa_n_rlpc" Runat="server"/>
										</asp:TableCell>
										<asp:TableCell Width="150">
													
											&nbsp;
											<asp:LinkButton ID="LNBMostraCC" Runat="server" CssClass="Filtro_Link" CausesValidation="False">Mostra Cc e Ccn</asp:LinkButton>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRcc" Visible="False" CssClass="mailFieldTo">
										<asp:TableCell>
											<asp:label id="LBcc" runat="server" text="Cc:" CssClass="titolo_campoSmall"></asp:label>
										</asp:TableCell>
										<asp:TableCell ColumnSpan="2">
											<asp:textbox id="TXBcc" runat="server" style="WIDTH: 450px" CssClass="Testo_Mail"></asp:textbox>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRccn" Visible="False" CssClass="mailFieldTo">
										<asp:TableCell>
											<asp:label id="LBccn" runat="server" text="Ccn:" CssClass="titolo_campoSmall"></asp:label>
										</asp:TableCell>
										<asp:TableCell ColumnSpan="2">
											<table cellpadding="0" cellspacing="0" border="0">
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
									<asp:TableRow ID="TBRrubrica" Visible="false">
										<asp:TableCell CssClass="top">
											<asp:label id="LBaRubrica" runat="server" text="A:" CssClass="titolo_campoSmall"></asp:label>
										</asp:TableCell>
										<asp:TableCell ColumnSpan="2">
													
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRobj">
										<asp:TableCell>
											<asp:label id="LBobj" runat="server" text="Oggetto:" CssClass="titolo_campoSmall"></asp:label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:Label id="LBerrore" Runat="server" CssClass="errore"></asp:Label>
											<asp:textbox id="TXBObj" runat="server" CssClass="Testo_Mail" MaxLength="100" Columns="99" Width="597"></asp:textbox>
										</asp:TableCell>
										<asp:TableCell>
											&nbsp;&nbsp;
											<asp:LinkButton ID="LNBMostraAttach" Runat="server" CssClass="Filtro_Link" CausesValidation="False" Visible="False" >Mostra Allegati</asp:LinkButton>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow runat="server" ID="TBRattach1" Visible="False">
										<asp:TableCell>
											<asp:label id="LBattach" runat="server" text="Allegati:" CssClass="titolo_campoSmall"></asp:label>
										</asp:TableCell>
										<asp:TableCell>
											<input language="vb" id="fileAllega" type="file" size="80" name="fileAttach" runat="server"/>
			                                <br />
                                            <asp:RegularExpressionValidator ID="REVvalid" runat="server" ControlToValidate="fileAllega" ErrorMessage="Errore" ValidationExpression="^\b" />										
										</asp:TableCell>
										<asp:TableCell>
											<asp:button id="BTallega" Runat="server" Text="Allega" CssClass="pulsante" CausesValidation="False"></asp:button>		
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow runat="server" ID="TBRattach2" Visible="False">
										<asp:TableCell>
											&nbsp;
										</asp:TableCell>
										<asp:TableCell ColumnSpan="2" CssClass="nosize0">&nbsp;
											<INPUT id="TBFrom_att" type="hidden" name="TBFrom_att" Runat="server"/>
											<asp:table id="TBLattach" Runat="server" BorderWidth="2" Width="700">
													
											</asp:table>
											<asp:Button ID="BTN_DelAllAttach" runat="server" Text="Clear" CssClass="pulsante" />						
											<INPUT id="TXBnascosto" type="hidden" name="TXBnascosto" runat="server"/>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell>&nbsp;</asp:TableCell>
										<asp:TableCell ColumnSpan="2">
											<asp:Label ID="LBLErroreAttach" CssClass="errore" Runat="server" ></asp:Label>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRbody" Runat="server">
										<asp:TableCell VerticalAlign="Top">
											<table cellpadding="0" cellspacing="0" border="0" style="height:100%">
												<tr>
													<td>&nbsp;</td>
												</tr><tr>
													<td>
														<asp:label id="LBbody" runat="server" text="Testo:" CssClass="titolo_campoSmall"></asp:label>
													</td>
												</tr>
												<tr>
													<td height="100%">&nbsp;</td>
												</tr>
											</table>
										</asp:TableCell>
										<asp:TableCell ColumnSpan="2" Width="700">
											<asp:textbox id="TXBbody" runat="server" rows="15" textmode="multiline" Width="800" Columns="88" CssClass="Testo_Mail"></asp:textbox>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow ID="TBRinvia">
										<asp:TableCell ColumnSpan="3" HorizontalAlign="Right">
											<table align="right" border="0" cellspacing="0">
												<tr>
													<td>
														<asp:CheckBox id="CBXcopiamittente" Runat="server" Text="Invia copia a mittente" Checked="false"></asp:CheckBox>
													</td>
													<td width="30px">&nbsp;</td>
													<td>
														<asp:CheckBox id="CBXricezione" Runat="server" Text="Conferma Ricezione"></asp:CheckBox>
													</td>
													<td width="30px">&nbsp;</td>
													<td>
														<asp:Label ID="LBInfoCom" Runat="server" Visible="False" ForeColor="#ff0000" Font-Bold="True" ></asp:Label>&nbsp;
														<INPUT id="BTNinvia" class="Pulsante" type="button" value="Invia" name="invia" runat="server" width="80" onserverclick="SendMail"/>
													</td>
												</tr>
											</table>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
							</asp:Panel>
							<asp:Panel id="PNLrubrica" Runat="server" width="530px" Visible="False" >
								<table width="530" border="0">
									<tr>
										<td>&nbsp;</td>
									</tr><tr>
										<td>
											<CTRL:CTRLrubrica id="CTRLrubrica" runat="server" BackColor="#ffffff"></CTRL:CTRLrubrica>
										</td>
									</tr>
									<tr>
										<td align="right">
											<asp:Button id="BTNcloseRubrica" Runat="server" CssClass="Pulsante" Text="Conferma"></asp:Button>
										</td>
									</tr>
								</table>
								<INPUT id="HDNtotaleDestinatari" type="hidden" name="HDNtotaleDestinatari" runat="server"/>
							</asp:Panel>
							<asp:panel id="PNLinviato" Runat="server" Visible="False" HorizontalAlign="Center">
								<br/><br/><br/>
								<table width="500px" align="center">
									<tr>
										<td class="normal" align="center">
											<asp:label ID="LBinviata" Runat=server CssClass="avviso_normal">E-mail inviata con successo -- </asp:label>
										</td>
									</tr><tr>
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
											<asp:Label id="LbErroreInvio" Runat="server" CssClass="errore"></asp:Label>
										</td>
									</tr><tr>
										<td align="center"><br/>
											<asp:Button id="BTNokerrore" Runat="server" Text="ok" CssClass="Pulsante"></asp:Button>
										</td>
									</tr>
								</table>
							</asp:panel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>

<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <head id="Head1" runat="server">
		<title>Comunità On Line - E-Mail</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>

		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>


	</HEAD>

	<body onload="AggiornaFocus()">
		 <form id="aspnetForm" method="post" runat="server" name="aspnetForm" >
		 <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
         	<tr>
				<td colspan="3" >
				    <div>
				        <HEADER:CtrLHEADER id="Intestazione" runat="server" HeaderNewsMemoHeight="85px"></HEADER:CtrLHEADER>
				    </div>
				</td>
			</tr>
			

			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>--%>