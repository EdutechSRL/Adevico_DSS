<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_Fase3ComunitaPadri.ascx.vb" Inherits="Comunita_OnLine.UC_Fase3ComunitaPadri" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="radTree" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>

<script language=javascript type="text/javascript">
		function UpdateAllChildren(nodes, checked,value){
			var i;
			for (i=0; i<nodes.length; i++){
				if (nodes[i].Value ==value){
					if (checked)
						nodes[i].Check()
					else
						nodes[i].UnCheck()
					}
				if (nodes[i].Nodes.length > 0)
					UpdateAllChildren(nodes[i].Nodes, checked,value);   
			}
		}

		function CheckChildNodes(node){
			RootNode = FindRoot(node)
			UpdateAllChildren(RootNode.Nodes, node.Checked,node.Value);
			}
			
		function FindRoot(node){
			if (!node.Parent){
				
				return node
				}
			else
				return FindRoot(node.Parent)
		}
</script>

<input id="HDN_ORGN_ID" type="hidden" name="HDN_ORGN_ID" runat="server"/>
<input type=hidden id=HDNcmnt_ID runat=server NAME="HDNcmnt_ID"/>
<input type=hidden id="HDNhasSetup" runat=server NAME="HDNhasSetup"/>
<INPUT id="HDN_ComunitaAttualeID" type="hidden" name="HDN_ComunitaAttualeID" runat="server"/>
<INPUT id="HDN_ComunitaAttualePercorso" type="hidden" name="HDN_ComunitaAttualePercorso" runat="server"/>
<INPUT id="HDN_Livello" type="hidden" name="HDN_Livello" runat="server"/>
<INPUT id="HDN_ServizioCode" type="hidden" name="HDN_ServizioCode" runat="server"/>
<INPUT id="HDN_hasComunitaForServizio" type="hidden" name="HDN_hasComunitaForServizio" runat="server"/>
					
<asp:Table ID="TBLresponsabili" Runat=server HorizontalAlign=Center GridLines=none width="800px">
	<asp:TableRow>
		<asp:TableCell>
			<asp:Label ID="LBinfoComunita" Runat="server">Scelta delle altre comunità in cui rendere disponibile quella che si intende creare:</asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>

		<asp:Table ID="TBLFiltroCom" Runat="server" CellPadding="0" CellSpacing="0">
				
			<asp:TableRow id="TBRchiudiFiltro" Height=22px Visible=False>
				<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
					<asp:LinkButton ID="LNBchiudiFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
				</asp:TableCell>
				<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=650px Height=22px>&nbsp;
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow id="TBRapriFiltro" Height=22px>
				<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
					<asp:LinkButton ID="LNBapriFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
				</asp:TableCell>
				<asp:TableCell CssClass="Filtro_Cellnull" Width=650px Height=22px>&nbsp;
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TBRfiltri" Visible="False">
				<asp:TableCell HorizontalAlign="Center" ColumnSpan="2" CssClass="Filtro_CellFiltri" width="100%">
					
					<asp:Table ID="TBLfiltro" Runat="server" HorizontalAlign="Center" width="100%">
						<asp:TableRow>
							<asp:TableCell ColumnSpan="2">
								<table cellpadding=0 cellspacing=0>
									<tr>
										<td>
											<asp:Label ID="LBorganizzazione_c" Runat="server" CssClass="FiltroVoceSmall10">Organizzazione:&nbsp;</asp:Label>
											&nbsp;
											<asp:DropDownList ID="DDLorganizzazione" Runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall10"></asp:DropDownList>		
										</td>
										<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
										<td>
											<asp:Label ID="LBtipoComunita_c" Runat="server" CssClass="FiltroVoceSmall10">Tipo Comunita</asp:Label>
											&nbsp;
											<asp:dropdownlist id="DDLTipo" runat="server" CssClass="FiltroCampoSmall10" AutoPostBack="true"></asp:dropdownlist>
										</td>
									</tr>
								</table>
							</asp:TableCell>
						</asp:tablerow>
						<asp:TableRow>
							<asp:TableCell ColumnSpan="2">
								<table cellpadding=0 cellspacing=0>
									<tr>
										<td>
											<asp:Label ID="LBtipoRicerca_c" Runat="server" CssClass="FiltroVoceSmall10">Tipo Ricerca</asp:Label>
											&nbsp;
											<asp:dropdownlist id="DDLTipoRicerca" Runat="server" CssClass="FiltroCampoSmall10">
												<asp:ListItem Value="-2" Selected="true">Nome inizia per</asp:ListItem>
												<asp:ListItem Value="-7">Nome contiene</asp:ListItem>
												<asp:ListItem Value="-3">Creata dopo il</asp:ListItem>
												<asp:ListItem Value="-4">Creata prima del</asp:ListItem>
											</asp:dropdownlist>
										</td>
										<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
										<td>
											<asp:Label ID="LBvalore_c" Runat="server" CssClass="FiltroVoceSmall10">Valore</asp:Label>
											&nbsp;
											<asp:textbox id="TXBValore" Runat="server" CssClass="FiltroCampoSmall10" MaxLength="100" AutoPostBack="False" Columns=50></asp:textbox>
										</td>
										<td>&nbsp;</td>
										<td>
											<asp:Label ID="LBstatoComunita_t" Runat="server" CssClass="FiltroVoceSmall">Stato:</asp:Label>&nbsp;
											<asp:dropdownlist ID="DDLstatoComunita" Runat=server CssClass="FiltroCampoSmall" AutoPostBack=True >
												<asp:ListItem Value=0 Selected=true>Attivate</asp:ListItem>
												<asp:ListItem Value=1>Archiviate</asp:ListItem>
												<asp:ListItem Value=2>Bloccate</asp:ListItem>
											</asp:dropdownlist>
										</td>
									</tr>
								</table>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow >
							<asp:TableCell ColumnSpan="2" HorizontalAlign="Right">
								<asp:button id="BTNCerca" Runat="server" CssClass="PulsanteFiltro" Text="Cerca"  CausesValidation=False></asp:button>
							</asp:TableCell>
						</asp:TableRow>		
					</asp:Table>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>

		<asp:table ID="TBLDdlCom" Runat="server" >
			<asp:TableRow>
				<asp:TableCell>
					<radTree:RadTreeView ID="RDTcomunita" runat="server" Height="450px" Width="750px"  CssFile="~/RadControls/TreeView/Skins/Comunita/Style.css"
						ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" skin="Comunita" CheckBoxes=true CausesValidation=False AfterClientCheck=CheckChildNodes/>
				</asp:TableCell>
			</asp:TableRow>
		</asp:table>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>