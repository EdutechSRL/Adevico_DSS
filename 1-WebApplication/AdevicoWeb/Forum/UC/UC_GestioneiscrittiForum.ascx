<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_GestioneiscrittiForum.ascx.vb" Inherits="Comunita_OnLine.UC_GestioneiscrittiForum" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:panel id="PNLfiltri" Runat="server" Visible="true">
    <div class="filtercontainer container_12 clearfix">
        <div class="filter grid_4 alpha">
            <asp:Label ID="LBruoloForum" runat="server" CssClass="label">Ruolo Forum:</asp:Label>
			<asp:DropDownList id="DDLruoloForumRic" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True></asp:DropDownList>
        </div>
        <div class="filter grid_4">
            <asp:Label ID="LBruoloComunita" runat="server" CssClass="label">Ruolo Comunità:</asp:Label>							
			<asp:DropDownList id="DDLruoloComunitaRic" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=True></asp:DropDownList>
        </div>
        <div class="filter grid_4 omega">&nbsp;</div>
        <div class="filter grid_4 alpha">
            <asp:Label ID="LBnome" runat="server" CssClass="label">Nome:</asp:Label>							
			<asp:TextBox id="TXBnome" Runat="server" MaxLength="30" CssClass="FiltroCampoSmall"></asp:TextBox>
        </div>
        <div class="filter grid_4">
            <asp:Label ID="LBcognome" runat="server" CssClass="label">Cognome:</asp:Label>
			<asp:TextBox id="TXBcognome" Runat="server" MaxLength="30" CssClass="FiltroCampoSmall"></asp:TextBox>
        </div>
        
        <div class="filter grid_2 omega">
            <asp:Button id="BTNcerca" Runat="server" Text="Cerca" CssClass="PulsanteFiltro"></asp:Button>
        </div>
    </div>
    <div class="linewrapper filterletters clearfix">
        <div class="left">
            <asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLink_Click">Tutti</asp:linkbutton>
            <asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLink_Click">Altro</asp:linkbutton>
            <asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLink_Click">A</asp:linkbutton>
            <asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLink_Click">B</asp:linkbutton>
            <asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLink_Click">C</asp:linkbutton>
            <asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLink_Click">D</asp:linkbutton>
            <asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLink_Click">E</asp:linkbutton>
            <asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLink_Click">F</asp:linkbutton>
            <asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLink_Click">G</asp:linkbutton>
            <asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLink_Click">H</asp:linkbutton>
            <asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLink_Click">I</asp:linkbutton>
            <asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLink_Click">J</asp:linkbutton>
            <asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLink_Click">K</asp:linkbutton>
            <asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLink_Click">L</asp:linkbutton>
            <asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLink_Click">M</asp:linkbutton>
            <asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLink_Click">N</asp:linkbutton>
            <asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLink_Click">O</asp:linkbutton>
            <asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLink_Click">P</asp:linkbutton>
            <asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLink_Click">Q</asp:linkbutton>
            <asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLink_Click">R</asp:linkbutton>
            <asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLink_Click">S</asp:linkbutton>
            <asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLink_Click">T</asp:linkbutton>
            <asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLink_Click">U</asp:linkbutton>
            <asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLink_Click">V</asp:linkbutton>
            <asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLink_Click">W</asp:linkbutton>
            <asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLink_Click">X</asp:linkbutton>
            <asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLink_Click">Y</asp:linkbutton>
            <asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLink_Click">Z</asp:linkbutton>
        </div>
        <div class="right">
            <asp:panel id="PNLpaginazione" Runat="server" Visible="true" HorizontalAlign=Right >
								<asp:label cssclass="Filtro_TestoPaginazione" Runat="server" ID="LBpageRecord"> Record per Pagina:</asp:label>
								<asp:DropDownList id="DDLpaginazione" Runat="server" CssClass="Filtro_RecordPaginazione" AutoPostBack="true">
									<asp:ListItem Value="20"> 20</asp:ListItem>
									<asp:ListItem Value="30"> 30</asp:ListItem>
									<asp:ListItem Value="50"> 50</asp:ListItem>
									<asp:ListItem Value="75"> 75</asp:ListItem>
									<asp:ListItem Value="100">100</asp:ListItem>
								</asp:DropDownList>
							</asp:panel>
        </div>
    </div>	
</asp:panel>
<asp:Panel id="PNLiscritti" Runat="server" Visible="true">
	<asp:datagrid id="DGiscrittiForum" runat="server" DataKeyField="RLPC_ID" AllowPaging="true" AutoGenerateColumns="False"
					AllowSorting="true" BackColor="transparent" ShowFooter="false" UseAccessibleHeader="true" CssClass="table light fullwidth persons" Font-Size="8"
					AllowCustomPaging="true" PageSize="10">
					<AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
					<HeaderStyle CssClass=""></HeaderStyle>
					<ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
					<PagerStyle CssClass="ROW_Page_Small" Position=TopAndBottom Mode="NumericPages" Visible="true"
					HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
					<Columns>
						<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
							<ItemTemplate>
							    <asp:LinkButton ID="LNBmodifica" Runat="server" CausesValidation="False" CommandName="modifica"></asp:LinkButton>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10"
							Visible="true">
							<ItemTemplate>
							     <asp:LinkButton ID="LNBremove" Runat="server" CausesValidation="False" CommandName="remove"></asp:LinkButton>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="PRSN_Anagrafica" ItemStyle-CssClass="ROW_TD_Small" HeaderText="Anagrafica" SortExpression="PRSN_Anagrafica"></asp:BoundColumn>
						<asp:BoundColumn DataField="RLPF_TPRF_id" Visible="False"></asp:BoundColumn>
						<asp:BoundColumn DataField="TPRF_nome" ItemStyle-CssClass="ROW_TD_Small" HeaderText="Ruolo forum" SortExpression="TPRF_nome"></asp:BoundColumn>
						<asp:BoundColumn DataField="TPRL_nome" ItemStyle-CssClass="ROW_TD_Small" HeaderText="Ruolo Comunità" SortExpression="TPRL_nome"></asp:BoundColumn>
						<asp:TemplateColumn SortExpression="RLPF_Abilitato">
							<ItemTemplate>
								<asp:LinkButton ID="LKBdisAbilita" runat="server" CommandName="abilitazione" commandargument='<%# DataBinder.Eval(Container.DataItem, "oRLPF_Abilitato") %>'></asp:LinkButton>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid>
</asp:Panel>
<asp:panel id="PNLnorecord" Runat="server" Visible="false">
	<table width="650" align="center">
		<tr>
			<td height="40">&nbsp;</td>
		</tr>
		<tr align="center">
			<td>
				<asp:Label id="LBnorecord" Runat="server" CssClass="Errore"></asp:Label></td>
		</tr>
	</table>
</asp:panel>
<asp:panel id="PNLmodificaRuolo" runat="server" visible="false">
    <INPUT type="hidden" id="HDrlpc" runat="server" NAME="HDrlpc"/>
	<INPUT id="HDNfrumID" type="hidden" runat="server" NAME="HDNfrumID"/>
    <div class="fieldobject editrole">
        <div class="fieldrow">
            <asp:Label ID="LBcognomeNome_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBCognomeNome">Cognome e Nome</asp:Label>
            <asp:label id="LBCognomeNome" Runat="server" CssClass="testo_campoSmall"></asp:label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBruoloForum_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLruoloForum">Ruolo Forum</asp:Label>
            <asp:DropDownList id="DDLruoloForum" Runat="server" CssClass="testo_campoSmall"></asp:DropDownList>
        </div>
    </div>
	
</asp:panel>