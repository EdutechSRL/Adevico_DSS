<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="Wizard_ProfiliServizi.aspx.vb" Inherits="Comunita_OnLine.Wizard_ProfiliServizi"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLdati" Src="./UC_ProfiliServizi/UC_DatiProfilo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLruoli" Src="./UC_ProfiliServizi/UC_ProfiloRuoli.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLservizi" Src="./UC_ProfiliServizi/UC_ProfiloServizi.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLruoliPermessi" Src="./UC_ProfiliServizi/UC_ProfiloPermessi.ascx" %>


<%--
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <input type=hidden id="HDNazione" value="gestioneTipo" runat=server NAME="HDNazione"/>
	<asp:Table ID="TBLprincipale" Runat=server CellPadding=0 GridLines=None Width=900px CellSpacing=0>
		<asp:TableRow>
			<asp:TableCell HorizontalAlign=Left CssClass=RigaTitolo>
				<asp:Label ID="LBTitolo" Runat="server">Creazione Profilo</asp:Label>
			</asp:TableCell>
		</asp:TableRow>
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
								<asp:Table HorizontalAlign=left Runat=server ID="TBLdati" Width=800px Visible=true GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell >
											<CTRL:CTRLdati id=CTRLdati runat="server"></CTRL:CTRLdati>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLassociazioneRuoli" Width=800px Visible=False GridLines=none  >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
											<CTRL:CTRLruoli id="CTRLruoli" runat="server"></CTRL:CTRLruoli>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLservizi" Width=800px Visible=False GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
											<CTRL:CTRLservizi id="CTRLservizi" runat="server"></CTRL:CTRLservizi>
										</asp:TableCell>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLruoliPermessi" Width=800px Visible=False GridLines=none >
									<asp:TableRow>
										<asp:TableCell Width=15px>&nbsp;</asp:TableCell>
										<asp:TableCell>
											<CTRL:CTRLruoliPermessi id="CTRLruoliPermessi" runat="server"></CTRL:CTRLruoliPermessi>
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
	<input type=hidden id="HDN_profiloID" runat=server NAME="HDN_profiloID"/>
</asp:Content>


<%--Stili e Script lasciati qui, erano commentati
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    	<title>Comunità On Line - Wizard Profilo Servizi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		
		 <style type="text/css">
			td{
			font-size: 11px;
			}
		</style>
		
		
            <style type="text/css">
	    <!--
			
	    #layer1
        {
	        border-right: black 1px solid;
	        border-top: black 1px solid;
	        font-size: x-small;
	        left: 10px;
	        visibility: hidden;
	        margin: 2px;
	        border-left: black 1px solid;
	        width: 200px;
	        border-bottom: black 1px solid;
	        position: absolute;
	        top: 50px;
	        height: 100px;
	        background-color: lightblue;
	        margin-top: 2px;
	        margin-right: 2px;
	        margin-bottom: 2px;
	        margin-left: 2px;
        }

	    #layer1__{
		    background-color: lightblue;
		    height: 100px;
		    left: 10px;
		    position: absolute;
		    top: 50px;
		    width: 200px;
		    visibility: hidden;
		    }
	    -->
    </style>

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
	<script type="text/javascript" language="JavaScript" >
		<!--
			function ChangeState(evt, layerRef, state, valore){
				var e = (window.event) ? window.event : evt;
				var PosX, PosY;
					
				PosX = e.clientX;
				PosY = e.clientY + 20;
				document.getElementById(layerRef).innerHTML=valore
				document.getElementById(layerRef).style.visibility = state;
				document.getElementById(layerRef).style.left = PosX;
				document.getElementById(layerRef).style.top = PosY;
			}
	//-->
	</script>
<body onkeydown="return SubmitRicerca(event);">

    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		<table cellSpacing="0" cellPadding="0" width="900px" align="center" border="0">
			<tr>
				<td colSpan="3">
					<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
				</td>
			</tr>
			<tr class="contenitore">
				<td colSpan="3">

				</td>
			</tr>
		</table>
		<FOOTER:CTRLFOOTER id="CTRLFooter" runat="server"></FOOTER:CTRLFOOTER>
    </form>

  </body>
</html>
--%>