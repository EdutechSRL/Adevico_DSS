<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_DettagliComunita.ascx.vb" Inherits="Comunita_OnLine.UC_DettagliComunita" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<input type="hidden" runat="server" id="HDNcomunitaID"/>
<asp:table id="TBLdettagli" Runat="server" Width="760px" GridLines=none >
	<asp:tableRow>
		<asp:tableCell ColumnSpan="4" CssClass="nosize" height="5px">&nbsp;</asp:tableCell>
	</asp:tableRow>
	<asp:TableRow ID="TBRtab" >
		<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
		<asp:TableCell  Height=31px  HorizontalAlign=Left ColumnSpan=2>
             <telerik:RadTabStrip ID="TBSmenu" runat="server" Align="Justify" Width="100%" Height="26px"
                SelectedIndex="0" CausesValidation="false" AutoPostBack="true" Skin="Outlook"
                EnableEmbeddedSkins="true">
                <Tabs>
                    <telerik:RadTab Text="Info principali" Value="TABprincipale" runat="server"/>
                    <telerik:RadTab Text="Dettagli avanzati" Value="TABdettagli" runat="server"/>
                </Tabs>
            </telerik:RadTabStrip>
		</asp:TableCell>
		<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
	</asp:TableRow>
	<asp:tableRow>
		<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
		<asp:TableCell ColumnSpan=2>
			<asp:Table CellPadding=0 CellSpacing=0 GridLines=none Runat=server id="TBLprincipale">
				<asp:TableRow>
					<asp:tableCell CssClass=top  Width="140px">
						<asp:Label ID="LB_Nome" Runat="server" CssClass="dettagli_Voce">Nome:</asp:Label>
					</asp:tableCell>
					<asp:tableCell CssClass=top>
						<asp:Label id="LBNome" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
					</asp:tableCell>
					<asp:TableCell RowSpan=9 CssClass=top>
						<asp:HyperLink ID=HYPfoto Runat=server Target=_blank Visible=False >
						<asp:image id="IMFoto" Runat="server" Visible="False" ToolTip="Immagine Personale" Height="125px"
								Width="100px" ></asp:image>
						</asp:HyperLink>
					</asp:TableCell>
				</asp:TableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px">
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell  Width="140px">
						<asp:Label ID="LB_tipocomunita" Runat="server" CssClass="dettagli_Voce">Tipo Comunità:</asp:Label>
					</asp:tableCell>
					<asp:tableCell >
						<asp:Label id="LBtipocomunita" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px">
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell  Width="140px">
						<asp:Label ID="LBresponsabile_c" Runat="server" CssClass="dettagli_Voce">Responsabile:</asp:Label>
					</asp:tableCell>
					<asp:tableCell >
						<asp:Label id="LBresponsabile" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px">
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRcreatore">
					<asp:tableCell Width="140px">
						<asp:Label ID="LBcreatore_c" CssClass="dettagli_Voce" Runat="server">Creatore:</asp:Label>
					</asp:tableCell>
					<asp:tableCell>
						<asp:Label id="LBcreatore" Runat="server"  CssClass="dettagli_CampoSmall"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRcreatore_sep">
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px">
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell Width="140px">
						<asp:Label ID="LB_isChiusa" CssClass="dettagli_Voce" Runat="server">Condizione:</asp:Label>
					</asp:tableCell>
					<asp:tableCell Width="620px">
						<asp:Label id="LBisChiusa" CssClass="dettagli_CampoSmall" Runat="server"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px">
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%" height="2px"/>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell Width="140px" >
						<asp:Label ID="LBiscrizioni_t" CssClass="dettagli_Voce" Runat="server">Iscrizioni:</asp:Label>
					</asp:tableCell>
					<asp:tableCell Width="620px">
						<asp:Label id="LBiscrizioni" CssClass="dettagli_CampoSmall" Runat="server"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:TableRow ID="TBRdataAperturaChiusura">
					<asp:tableCell Width="140px"  Height=20px>
						<asp:Label ID="LB_apertura" Runat="server" CssClass="dettagli_Voce">Data apertura:</asp:Label>
					</asp:TableCell>
					
					<asp:TableCell Width="620px" ColumnSpan=2  Height=20px>
						<table>
							<tr>
								<td>
									<asp:Label id="LBapertura" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
								</td>
								<td>&nbsp;</td>
								<td>
									&nbsp;<asp:Label ID="LB_chiusura" CssClass="dettagli_Voce" Runat="server">Data chiusura:</asp:Label>
								</td>
								<td>
									&nbsp;<asp:Label id="LBchiusura" CssClass="dettagli_CampoSmall" Runat="server"></asp:Label>
								</td>
							</tr>
						</table>
					</asp:TableCell>
				</asp:TableRow>
				<asp:tableRow ID="TBRdataAperturaChiusura_sep">
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRdataIscrizioni">
					<asp:tableCell  Width="140px" Height=20px>
						<asp:Label ID="LB_inizioIscrizione" CssClass="dettagli_Voce" Runat="server">Inizio Iscrizione:</asp:Label>
					</asp:tableCell>
					<asp:tableCell  Width="620px" ColumnSpan=2 Height=20px>
						<table>
							<tr>
								<td>
									<asp:Label id="LBinizioIscrizione" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
								</td>
								<td>&nbsp;</td>
								<td>
									&nbsp;<asp:Label ID="LB_fineIscrizione" Runat="server" CssClass="dettagli_Voce">Fine Iscrizione:</asp:Label>
								</td>
								<td>
									&nbsp;<asp:Label id="LBfineIscrizione" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
								</td>
							</tr>
						</table>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRdataIscrizioni_sep">
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell Width="140px" Height=20px>
						<asp:Label ID="LBmaxIscritti_c" CssClass="dettagli_Voce" Runat="server">N° Max iscritti:</asp:Label>
					</asp:tableCell>
					<asp:tableCell Width="620px" ColumnSpan=2 Height=20px>
						<table>
							<tr>
								<td>
									<asp:Label id="LBmaxIscritti" CssClass="dettagli_CampoSmall" Runat="server"></asp:Label>
								</td>
								<td>&nbsp;</td>
								<td>
									&nbsp;<asp:Label ID="LBoverIscrizioni_t" CssClass="dettagli_Voce" Runat="server">N° Max iscritti:</asp:Label>
								</td>
								<td>
									&nbsp;<asp:Label id="LBoverIscrizioni" CssClass="dettagli_CampoSmall" Runat="server"></asp:Label>
								</td>
							</tr>
						</table>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell Width="140px" CssClass=top Height=20px>
						<asp:Label ID="LBaccessiSpeciali_t" CssClass="dettagli_Voce" Runat="server">Accessi speciali:</asp:Label>
					</asp:tableCell>
					<asp:tableCell Width="620px" ColumnSpan=2>
						<asp:Label id="LBaccessiSpeciali" CssClass="dettagli_CampoSmall" Runat="server"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell Width="140px" CssClass=top Height=20px>
						<asp:Label ID="LBiscritti_c" CssClass="dettagli_Voce" Runat="server">N° iscritti:</asp:Label>
					</asp:tableCell>
					<asp:tableCell Width="620px" ColumnSpan=2>
						<asp:Label id="LBiscritti" CssClass="dettagli_CampoSmall" Runat="server"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>	
				
				
				<asp:tableRow ID="TBRinfoiscrizione" Visible=False >
					<asp:tableCell Width="140px" CssClass="top" Height=20px>
						<asp:Label ID="LBinfoIscrizione_t" Runat="server" CssClass="dettagli_Voce">Info Iscrizione:</asp:Label>
					</asp:tableCell>
					<asp:tableCell CssClass="top" Width="620px" ColumnSpan=2 Height=20px>
						<asp:Label id="LBinfoIscrizione" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRinfoiscrizione_sep" Visible=False>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRarchiviata" Visible=False >
					<asp:tableCell Width="140px" CssClass="top" Height=20px>
						<asp:Label ID="LBarchiviata_t" Runat="server" CssClass="dettagli_Voce">Archiviata:</asp:Label>
					</asp:tableCell>
					<asp:tableCell CssClass="top" Width="620px" ColumnSpan=2 Height=20px>
						<asp:Label id="LBarchiviata" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRarchiviata_sep" Visible=False>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRbloccata" Visible=False >
					<asp:tableCell Width="140px" CssClass="top" Height=20px>
						<asp:Label ID="LBbloccata_t" Runat="server" CssClass="dettagli_Voce">Bloccata:</asp:Label>
					</asp:tableCell>
					<asp:tableCell CssClass="top" Width="620px" ColumnSpan=2 Height=20px>
						<asp:Label id="LBbloccata" Runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
					</asp:tableCell>
				</asp:tableRow>
				<asp:tableRow ID="TBRbloccata_sep" Visible=False>
					<asp:tableCell CssClass="nosize" Width="140px" height="2px"><img src="./../images/separatore_small_Bianco.gif" alt="" align="top" border="0" width="100%" height="2px"/></asp:tableCell>
					<asp:tableCell CssClass="dettagli_separatore" height="2px" Width="620px" ColumnSpan=2>
						<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
							height="2px"/></asp:tableCell>
				</asp:tableRow>
			</asp:Table>				
		</asp:TableCell>
		<asp:tableCell CssClass="nosize" width="5px" height="5px">&nbsp;</asp:tableCell>
	</asp:tableRow>
	<asp:tableRow>
		<asp:tableCell ColumnSpan="4" CssClass="nosize" height="5px">&nbsp;</asp:tableCell>
	</asp:tableRow>
</asp:table>