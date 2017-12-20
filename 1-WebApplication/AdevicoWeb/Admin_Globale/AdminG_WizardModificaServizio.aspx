<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" Codebehind="AdminG_WizardModificaServizio.aspx.vb" Inherits="Comunita_OnLine.AdminG_WizardModificaServizio"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLtipoComunita" Src="./UC_WizardCreaServizio/UC_ServizioDefinisciTipoComunita.ascx" %>
<%--<%@ Register TagPrefix="CTRL" TagName="CTRLruoliPermessi" Src="./UC_WizardCreaServizio/UC_AdminServizioModificaDefinizionePermessi.ascx" %>--%>
<%@ Register TagPrefix="CTRL" TagName="CTRLdati" Src="./UC/UC_DatiServizio.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsceltaPermessi" Src="./UC_WizardCreaServizio/UC_SceltaPermessi.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtraduzionePermessi" Src="./UC_WizardCreaServizio/UC_AdminServizioTraduzionePermessi.ascx" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <script type="text/javascript" language="javascript">
        <%= Me.BodyId() %>.onkeydown = SubmitRicerca(event);

    	function SubmitRicerca(event) {
    		if (document.all) {
    		    if (event.keyCode == 13) {
    		        event.returnValue = false;
    		        event.cancel = true;
    		        return false;
    		    }
    		}
    		else if (document.getElementById) {
    		    if (event.which == 13) {
    		        event.returnValue = false;
    		        event.cancel = true;
    		        return false;
    		    }
    		}
    		else if (document.layers) {
    		    if (event.which == 13) {
    		        event.returnValue = false;
    		        event.cancel = true;
    		        return false;
    		    }
    		}
    		else
    		    return true;
    	}	
    </script>
    <style type="text/css">
		td{
			font-size: 11px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<input type=hidden id="HDNazione" value="gestioneTipo" runat=server NAME="HDNazione"/>
	<asp:Table ID="TBLprincipale" Runat=server CellPadding=0 GridLines=None Width=900px CellSpacing=0>
<%--		<asp:TableRow>
			<asp:TableCell HorizontalAlign=Left CssClass=RigaTitoloAdmin>
				<asp:Label ID="LBTitolo" Runat="server">Creazione Servizio</asp:Label>
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
								<asp:Button ID="BTNelenco2" Runat="server" Text="Torna all'elenco" CssClass="PulsanteFiltro" CausesValidation=False ></asp:Button>
								<asp:Button ID="BTNdefault2" Runat="server" Text="Setta default" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>
								<asp:Button ID="BTNreplicaSuTutti2" Runat="server" Text="Replica su tutte" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>
								<asp:Button ID="BTNsalvaDati2" Runat="server" Text="Salva" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>
								<asp:Button ID="BTNconferma2" Runat="server" Text="Salva impostazioni" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>
							</td>	
							<td width=35>&nbsp;</td>
							<td width=100 nowrap="nowrap">
								<asp:Button id="BTNindietro2" Runat="server" CssClass="PulsanteFiltro" Text="< Indietro" CausesValidation="False"></asp:Button>
							</td>
							<td width=5>&nbsp;</td>
							<td width=100 nowrap="nowrap" >
								<asp:Button id="BTNavanti2" Runat="server" CssClass="PulsanteFiltro" Text="Avanti >" CausesValidation="True"></asp:Button>
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
								<asp:Table HorizontalAlign=center Runat=server ID="TBLdati" Width=800px Visible=true >
									<asp:TableRow>
										<asp:TableCell>
											<CTRL:CTRLdati id=CTRLdati runat="server"></CTRL:CTRLdati>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLsceltaPermessi" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell>
											<CTRL:CTRLsceltaPermessi id="CTRLsceltaPermessi" runat="server"></CTRL:CTRLsceltaPermessi>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLtraduzionePermessi" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell>
											<CTRL:CTRLtraduzionePermessi id="CTRLtraduzionePermessi" runat="server"></CTRL:CTRLtraduzionePermessi>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLtipoComunita" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell>
											<CTRL:CTRLtipoComunita id="CTRLtipoComunita" runat="server"></CTRL:CTRLtipoComunita>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLconfermaPrincipali" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell>
											<br/><br/><br/><br/>
											<asp:Label ID="LBinfoConferma" Runat="server" Visible="True"></asp:Label>
											<br/><br/><br/><br/>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<%--<asp:Table HorizontalAlign=center Runat=server ID="TBLruoliPermessi" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell>
											<CTRL:CTRLruoliPermessi id="CTRLruoliPermessi" runat="server"></CTRL:CTRLruoliPermessi>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>--%>
							</asp:TableCell>
							<asp:TableCell Width=5px>&nbsp;</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:Panel>
				<asp:Panel ID="PNLnavigazione" Runat=server HorizontalAlign=Right Width=100% BorderWidth=1>
					<table cellSpacing=0 cellPadding=0 border=0 align=right>
						<tr>
							<td >
								<asp:Button ID="BTNelenco" Runat="server" Text="Annulla" CssClass="PulsanteFiltro" CausesValidation=False ></asp:Button>
								<asp:Button ID="BTNdefault" Runat="server" Text="Setta default" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>
								<asp:Button ID="BTNreplicaSuTutti" Runat="server" Text="Replica su tutte" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>											
								<asp:Button ID="BTNsalvaDati" Runat="server" Text="Salva" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>
								<asp:Button ID="BTNconferma" Runat="server" Text="Conferma" CssClass="PulsanteFiltro" CausesValidation=False Visible=False></asp:Button>
							</td>
							<td width=35>&nbsp;</td>
							<td width=100 nowrap="nowrap">
								<asp:Button id=BTNindietro Runat="server" CssClass="PulsanteFiltro" Text="< Indietro" CausesValidation="False"></asp:Button>
							</td>
							<td width=5>&nbsp;</td>
							<td width=100 nowrap="nowrap" >
								<asp:Button id=BTNavanti Runat="server" CssClass="PulsanteFiltro" Text="Avanti >" CausesValidation="True"></asp:Button>
							</td>
							<td width=20>&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<input type=hidden id="HDN_servizioID" runat=server NAME="HDN_servizioID"/>
</asp:Content>




<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
		<title>Comunità On Line - Wizard Creazione Nuovo Servizio</title>

		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>


  </head>
	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table class="contenitore" align="center">
				<tr class="contenitore">
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr class="contenitore">
					<td colSpan="3">

					</td>
				</tr>
				<tr class="contenitore">
					<td colSpan="3"></td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</html>--%>
