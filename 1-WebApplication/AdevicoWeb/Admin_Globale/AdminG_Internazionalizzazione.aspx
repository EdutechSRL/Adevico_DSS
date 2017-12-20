
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" Codebehind="AdminG_Internazionalizzazione.aspx.vb" Inherits="Comunita_OnLine.AdminG_Internazionalizzazione"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>


<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
		<%--<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>
		<tr>
			<td class="titolo" align="center"><asp:label id="LBtitolo" CssClass="TitoloServizio" Runat="server">- Internazionalizzazione -</asp:label></td>
		</tr>--%>
		<tr>
			<td bgColor="#a3b2cd" height="1"></td>
		</tr>
		<tr>
			<td align="center">
				<asp:panel id="PNLpermessi" Runat="server" HorizontalAlign="Center" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td vAlign="top" height="50">
								&nbsp;
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" Visible="False">
					<asp:Table ID="TBLcont" Runat="server" HorizontalAlign=Left>
						<asp:TableRow>
							<asp:TableCell ID="TCTabelle" Runat="server" Width="100px" CssClass="Internaz_celle_topLeft">
								<asp:ListBox ID="LBXtabelle" Runat="server" Rows=25  Font-Size=9 Font-Name="Tahoma" ForeColor=#00008b AutoPostBack="True"></asp:ListBox>
							</asp:TableCell>
							<asp:TableCell ID="TCCampi" Runat="server" Visible="False" CssClass="Internaz_cella_Campi">
								<asp:DropDownList ID="DDLCampi" Runat="server" Width=190px AutoPostBack="True"></asp:DropDownList>
								<br/>
								<asp:DataGrid ID="DGRecord" Runat="server" Width=190px AutoGenerateColumns="False" Visible="False" ShowHeader=False>
									<Columns>
										<asp:BoundColumn DataField="IDRecord" HeaderText="ID" Visible=True></asp:BoundColumn>
										<asp:ButtonColumn ButtonType="LinkButton" CommandName="elencaValori" DataTextField="descrizione" HeaderText="Descrizione"></asp:ButtonColumn>
										<asp:BoundColumn DataField="descrizione" Visible="False"></asp:BoundColumn>
									</Columns>
								</asp:DataGrid>
								<br/>
								<asp:Button ID="BTNimport" Runat=server Text="Importa" CssClass ="pulsante"></asp:Button>
							</asp:TableCell>
							<asp:TableCell ID="TCDati" Runat="server" Visible="False" Width="400px" CssClass="Internaz_celle_topLeft">
								<asp:DataGrid ID="DGValoriCampo" Runat="server" Width="400px" AutoGenerateColumns="False" DataKeyField="INTR_id">
									<Columns>
										<asp:ButtonColumn ButtonType="LinkButton" CommandName="cambiaValori" DataTextField="LNGU_nome"></asp:ButtonColumn>
										<asp:BoundColumn DataField="LNGU_default" HeaderText="default" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="LNGU_id" HeaderText="IDLingua" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="INTR_Valore" HeaderText="Descrizione"></asp:BoundColumn>
										<asp:BoundColumn DataField="LNGU_Icona" HeaderText="bandiera" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="LNGU_nome" HeaderText="lingua" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="INTR_id" HeaderText="lingua" Visible="False"></asp:BoundColumn>
									</Columns>
								</asp:DataGrid>
								<br/>
								<br/>
								<asp:Panel ID="PNLCambioValore" Runat="server" Visible="False">
									<table>
										<tr>
											<td colspan="2"  class="Internaz_celle_topLeft">
												<asp:Label ID="LBvaloreOld_t" Runat="server">Valore:</asp:Label>
											</td>
											<td colspan="2" class="Internaz_celle_topLeft">
												<asp:Label ID="LBvaloreOld" Runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
										<td   class="Internaz_celle_topLeft">
												<asp:Label ID="LBlingua_t" Runat="server">Lingua:</asp:Label>
											</td>
											<td align=right>
												&nbsp;<asp:Button ID="BTNsuLingua" Runat="server" Text="<" BackColor=#ffffff Font-Name="tahoma" Font-Size=8 Height=20 Width=20></asp:Button>
												&nbsp;<asp:Button ID="BTNgiuLingua" Runat="server" Text=">" BackColor=#ffffff Font-Name="tahoma" Font-Size=8 Height=20 Width=20></asp:Button>
											</td>
											<td  colspan="2">
												<asp:Label ID="LBvalLingua_t" Runat="server"></asp:Label>
											</td>
										</tr>
															
										<tr>
											<td class="Internaz_celle_topLeft">
												<asp:Label ID="LBTraduzione_t" Runat="server">Traduzione:</asp:Label>
																	
											</td>
											<td align=right>
												&nbsp;<asp:Button ID="BTNsxRecord" Runat="server" BackColor=#ffffff Text="<" Font-Name="tahoma" Font-Size=8 Height=20 Width=20></asp:Button>&nbsp;&nbsp;<asp:Button ID="BTNdxRecord" Runat="server" BackColor=#ffffff Text=">" Font-Name="tahoma" Font-Size=8 Height=20 Width=20></asp:Button>
											</td>
											<td  colspan="2">
												<asp:TextBox ID="TBValLingua" Runat="server" MaxLength=500 Columns=60></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td class="Eventi_cella">
												<asp:Button ID="BTNElimina" Runat="server"  Text="ELIMINA"></asp:Button>
											</td>
											<td class="Eventi_cella">
												<asp:Button ID="BTNAnnulla" Runat="server" Text="ANNULLA"></asp:Button>
											</td>
											<td class="Eventi_cella">
												<asp:Button ID="BTNConferma" Runat="server" Text="OK"></asp:Button>
											</td>
										</tr>
										<tr>
											<td colspan="4" class="Eventi_cella">
												<asp:Label ID="LBInfo" Runat="server" ForeColor=#ff0000 Font-Bold=True></asp:Label>
											</td>
										</tr>
									</table>
								</asp:Panel>
								<asp:panel ID="PNLimport" Runat="server" Visible =False >
									<table align =center >
										<tr>
											<td align=center><asp:Label ID="LBimport" Runat=server CssClass =""></asp:Label></td>
										</tr>
										<tr>
											<td align=center><asp:DropDownList ID="DDLlingua" Runat =server Width=190px></asp:DropDownList></td>
																
										</tr>
										<tr>
											<td align=center><asp:Label ID="LBmessaggio" Runat=server CssClass ="avviso"></asp:Label></td>
										</tr>
										<tr>
											<td align=center><asp:Button ID="BTNImportOk" Runat =server CssClass ="pulsante" Text=Ok></asp:Button>
															<asp:Button ID="BTNchiudi" Runat =server CssClass ="pulsante" Text=Chiudi Visible =False ></asp:Button></td>
										</tr>
									</table>
								</asp:panel>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:Panel>
			</td>
		</tr>
	</table>
</asp:Content>



<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Comunità OnLine - Internazionalizzazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>

		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
	</head>
	<body >
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td colSpan="3">

					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</html>--%>
