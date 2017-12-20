<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_FiltriUtente.ascx.vb" Inherits="Comunita_OnLine.UC_FiltriUtente" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Table ID="TBLfiltriUtente" Runat=server>
	<asp:TableRow ID="TBRfiltroPaginazione">
		<asp:TableCell>
			<fieldset>
				<legend><asp:Label ID="LBpaginazioneLegend" CssClass="legendaSmall" Runat=server></asp:Label></legend>
				<asp:Table ID="Table1" Runat=server>
					<asp:TableRow>
						<asp:TableCell>
							<asp:RadioButtonList ID="RBLpaginazione" Runat="server" CssClass="FiltroCampoSmall"  AutoPostBack="True" RepeatDirection=Horizontal RepeatLayout=Flow>
								<asp:ListItem Value=0 Selected=True >No</asp:ListItem>
								<asp:ListItem Value=1>Yes</asp:ListItem>
							</asp:RadioButtonList>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="TBRpaginazioneDDL">
						<asp:TableCell>
							<asp:Label ID="LBpaginazione_t" Runat=server CssClass="FiltroCampoSmall"></asp:Label>
							<asp:DropDownList ID="DDLpaginazione" Runat=server CssClass="FiltroCampoSmall"  AutoPostBack=True>
								<asp:ListItem Value=30>30</asp:ListItem>
								<asp:ListItem Value=45>45</asp:ListItem>
								<asp:ListItem Value=65>65</asp:ListItem>
								<asp:ListItem Value=75>75</asp:ListItem>
								<asp:ListItem Value=100>100</asp:ListItem>
							</asp:DropDownList>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</fieldset>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRinfra0">
		<asp:TableCell>
			&nbsp;
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRfiltroRuolo">
		<asp:TableCell>
			<fieldset>
				<legend><asp:Label ID="LBruoloLegend" CssClass="legendaSmall" Runat=server>Roles:</asp:Label></legend>
				<asp:Table ID="TBLruoli" Runat=server>
					<asp:TableRow>
						<asp:TableCell>
							<asp:CheckBox ID="CBXall" Runat=server CssClass="FiltroCampoSmall" Checked=True  AutoPostBack=True></asp:CheckBox><br/>
							<asp:CheckBoxList ID="CBLruoli" Runat=server CssClass="FiltroCampoSmall" RepeatLayout=Flow RepeatDirection=Vertical >
							
							</asp:CheckBoxList>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</fieldset>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRinfra1">
		<asp:TableCell>
			&nbsp;
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRfiltroAttivazione">
		<asp:TableCell>
			<fieldset>
				<legend><asp:Label ID="LBattivazioneLegend" CssClass="legendaSmall" Runat=server>Activation:</asp:Label></legend>
				<asp:Table ID="TBLattivazione" Runat=server>
					<asp:TableRow>
						<asp:TableCell>
							<asp:CheckBox ID="CBXattivazione" Runat=server CssClass="FiltroCampoSmall" Checked=True AutoPostBack=True ></asp:CheckBox><br/>
							<asp:CheckBoxList ID="CBLattivazione" Runat=server CssClass="FiltroCampoSmall" RepeatDirection=Vertical RepeatLayout=Flow >
							
							</asp:CheckBoxList>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</fieldset>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell HorizontalAlign=Right >
			<asp:Button ID="BTNaggiorna" Runat=server CssClass="PulsanteSmall" Text="Aggiorna"></asp:Button>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>
<input type=hidden name="HDN_CMNT_ID" runat=server ID="HDN_CMNT_ID" />
<input type=hidden name="HDN_CMNT_Path" runat=server ID="HDN_CMNT_Path"/>