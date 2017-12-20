<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="WizardImportaDati.aspx.vb" Inherits="Comunita_OnLine.WizardImportaDati"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLsceltaDati" Src="./UC_WizardImportaDati/UC_WZDid_Fase1SceltaDati.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLimpostazioni" Src="./UC_WizardImportaDati/UC_WZDid_Fase2Impostazioni.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsceltaUtenti" Src="./UC_WizardImportaDati/UC_WZDid_Fase3Utenti.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsceltaMateriale" Src="./UC_WizardImportaDati/UC_WZDid_Fase4materiale.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsceltaDiario" Src="./UC_WizardImportaDati/UC_WZDid_Fase5DiarioLezione.ascx" %>
<%--<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>
<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_header.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<input type=hidden id="HDN_ComunitaAttualeID" runat=server NAME="HDN_ComunitaAttualeID"/>
	<input type=hidden id="HDN_ComunitaCreataID" runat=server NAME="HDN_ComunitaCreataID"/>
	<input type=hidden id="HDNazione" value="gestioneTipo" runat=server NAME="HDNazione"/>
	<asp:Table ID="TBLprincipale" Runat=server CellPadding=0 GridLines=None Width=900px CellSpacing=0>
<%--		<asp:TableRow>
			<asp:TableCell HorizontalAlign=Left CssClass="RigaTitolo">
				<asp:Label ID="LBTitolo" Runat="server">Creazione comunità</asp:Label>
			</asp:TableCell>
		</asp:TableRow>--%>
		<asp:TableRow ID="TBRmenu">
			<asp:TableCell HorizontalAlign=Right>
				&nbsp;<asp:linkbutton id="LNBindietro" Runat="server" Text="Torna all'elenco" CssClass=Link_Menu CausesValidation=False></asp:linkbutton>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass=top>
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<br/>
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
				</asp:panel>
				<asp:Panel ID="PNLnavigazione2" Runat=server HorizontalAlign=Right Width=100% BorderWidth=1>
					<table cellSpacing=0 cellPadding=0 border=0 align=right>
						<tr>
							<td>
								<asp:Button ID="BTNelenco2" Runat="server" Text="Torno all'elenco" CssClass="PulsanteFiltro" CausesValidation=False ></asp:Button>										
								<asp:Button ID="BTNsalva2" Runat="server" Text="Salva" CssClass="PulsanteFiltro" CausesValidation=true Visible=False></asp:Button>
							</td>
							<td width=35>&nbsp;</td>
							<td width=100 nowrap="nowrap">
								<asp:Button id=BTNindietro2 Runat="server" CssClass="PulsanteFiltro" Text="< Indietro" CausesValidation="False"></asp:Button>
							</td>
							<td width=5>&nbsp;</td>
							<td width=100 nowrap="nowrap" >
								<asp:Button id=BTNavanti2 Runat="server" CssClass="PulsanteFiltro" Text="Avanti >" CausesValidation="True"></asp:Button>
								<asp:Button id=BTNconferma2 runat="server" CssClass="PulsanteFiltro" Text="Crea"></asp:Button>
							</td>
							<td width=20>&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" Width=900px BorderWidth=1>
					<asp:Table Runat=server id="TBLinserimento" CellPadding=0 CellSpacing=0 Width=900px Height=450px>
						<asp:TableRow>
							<asp:TableCell>&nbsp;</asp:TableCell>
							<asp:TableCell HorizontalAlign=left CssClass=top>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLsceltaDati" Width=800px Visible=true GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
											<CTRL:CTRLsceltaDati id="CTRLsceltaDati" runat="server"></CTRL:CTRLsceltaDati>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLimpostazioni" Width=800px Visible=False GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell >
											<CTRL:CTRLimpostazioni id="CTRLimpostazioni" runat="server"></CTRL:CTRLimpostazioni>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLsceltaUtenti" Width=800px Visible=False GridLines=none  >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
											<CTRL:CTRLsceltaUtenti id="CTRLsceltaUtenti" runat="server"></CTRL:CTRLsceltaUtenti>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLsceltaMateriale" Width=800px Visible=False GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
											<br/>
											<CTRL:CTRLsceltaMateriale id="CTRLsceltaMateriale" runat="server"></CTRL:CTRLsceltaMateriale>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLsceltaDiario" Width=800px Visible=False GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
											<CTRL:CTRLsceltaDiario id="CTRLsceltaDiario" runat="server"></CTRL:CTRLsceltaDiario>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLfinale" Width=800px Visible=False GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
															
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
							</asp:TableCell>
							<asp:TableCell Width=5px>&nbsp;</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:Panel>
				<asp:Panel ID="PNLnavigazione" Runat=server HorizontalAlign=Right Width=100% BorderWidth=1>
					<table cellSpacing=0 cellPadding=0 border=0 align=right>
						<tr>
							<td>
								<asp:Button ID="BTNelenco" Runat="server" Text="Annulla" CssClass="PulsanteFiltro" CausesValidation=False ></asp:Button>										
								<asp:Button ID="BTNsalva" Runat="server" Text="Salva" CssClass="PulsanteFiltro" CausesValidation=true Visible=False></asp:Button>
							</td>
							<td width=35>&nbsp;</td>
							<td width=100 nowrap="nowrap">
								<asp:Button id=BTNindietro Runat="server" CssClass="PulsanteFiltro" Text="< Indietro" CausesValidation="False"></asp:Button>
							</td>
							<td width=5>&nbsp;</td>
							<td width=100 nowrap="nowrap" >
								<asp:Button id=BTNavanti Runat="server" CssClass="PulsanteFiltro" Text="Avanti >" CausesValidation="True"></asp:Button>
								<asp:Button id=BTNconferma runat="server" CssClass="PulsanteFiltro" Text="Crea"></asp:Button>
							</td>
							<td width=20>&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</asp:Content>

<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>Comunità On Line - Wizard Importa Dati comunità</title>
    <LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
	
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
  </head>
   <script language="javascript" type="text/javascript">
			function SubmitRicerca(event){
				 if (document.all){
					if (event.keyCode == 13){
						event.returnValue=false;
						event.cancel = true;
						return false;
						}
					}
				else if (document.getElementById){
					if (event.which == 13){
						event.returnValue=false;
						event.cancel = true;
						return false;
						}
					}
				else if(document.layers){
					if(event.which == 13){
						event.returnValue=false;
						event.cancel = true;
							return false;
						}
					}
				else
					return true;
			}
		</script>
<body onkeydown="return SubmitRicerca(event);">
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