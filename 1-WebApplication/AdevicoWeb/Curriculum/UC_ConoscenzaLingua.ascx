<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_ConoscenzaLingua.ascx.vb" Inherits="Comunita_OnLine.UC_ConoscenzaLingua" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" runat="server" id="HDNprsn_id" name="HDNprsn_id"/>
<input type="hidden" runat="server" id="HDNcreu_id" name="HDNcreu_id"/>
<input type="hidden" runat="server" id="HDNcnln_id" name="HDNcnln_id"/>
<asp:Panel ID="PNLinserimento" Runat="server">
	<asp:Table ID="TBLinserimento" Runat="server" HorizontalAlign=left >
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label ID="LBnome_t" Runat="server" CssClass="Titolo_campoSmall">*Lingua:</asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:textbox id="TXBnome" Runat="server" Columns="60" CssClass="Testo_CampoSmall_obbligatorio" MaxLength="100"></asp:textbox>
				<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="Validatori" ControlToValidate="TXBnome"
					Display="Static">*</asp:requiredfieldvalidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow >
			<asp:TableCell Width="120px">&nbsp;</asp:TableCell>
			<asp:TableCell>
				<table width=100%>
					<tr>
						<td><asp:label ID=LBnessuna Runat=server CssClass="Testo_CampoSmall">Nessuna</asp:label></td>
						<td><asp:label ID="LBlimitata" Runat=server CssClass="Testo_CampoSmall">Limitata</asp:label></td>
						<td><asp:label ID="LBdiscreta" Runat=server CssClass="Testo_CampoSmall">Discreta</asp:label></td>
						<td><asp:label ID="LBbuona" Runat=server CssClass="Testo_CampoSmall">Buona</asp:label></td>
						<td><asp:label ID="LBottima" Runat=server CssClass="Testo_CampoSmall">Ottima</asp:label></td>
						<td><asp:label ID="LBmadrelingua" Runat=server CssClass="Testo_CampoSmall">Madrelingua</asp:label></td>
					</tr>
					
				</table>
			</asp:TableCell> 
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="120px">
				<asp:Label ID="LBlettura_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Lettura:</asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:radiobuttonlist Runat="server" ID="RBLlettura" RepeatDirection=Horizontal Width ="100%" >
					<asp:ListItem Value="1" Selected =true>&nbsp;</asp:ListItem>
					<asp:ListItem Value="2">&nbsp;</asp:ListItem>
					<asp:ListItem Value="3">&nbsp;</asp:ListItem>
					<asp:ListItem Value="4">&nbsp;</asp:ListItem>
					<asp:ListItem Value="5">&nbsp;</asp:ListItem>
					<asp:ListItem Value="6">&nbsp;</asp:ListItem>
					</asp:radiobuttonlist>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label ID="LBscrittura_t" Runat="server" CssClass="Titolo_campoSmall" >&nbsp;Abilità in Scrittura:</asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:radiobuttonlist Runat="server" ID="RBLscrittura" RepeatDirection=Horizontal Width ="100%" >
					<asp:ListItem Value="1" Selected =true>&nbsp;</asp:ListItem>
					<asp:ListItem Value="2">&nbsp;</asp:ListItem>
					<asp:ListItem Value="3">&nbsp;</asp:ListItem>
					<asp:ListItem Value="4">&nbsp;</asp:ListItem>
					<asp:ListItem Value="5">&nbsp;</asp:ListItem>
					<asp:ListItem Value="6">&nbsp;</asp:ListItem>
				</asp:radiobuttonlist>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label ID="LBespressioneOrale_t" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Espressione Orale:</asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:radiobuttonlist Runat="server" ID="RBLespressioneOrale" RepeatDirection=Horizontal Width="100%">
					<asp:ListItem Value="1" Selected =true>&nbsp;</asp:ListItem>
					<asp:ListItem Value="2">&nbsp;</asp:ListItem>
					<asp:ListItem Value="3">&nbsp;</asp:ListItem>
					<asp:ListItem Value="4">&nbsp;</asp:ListItem>
					<asp:ListItem Value="5">&nbsp;</asp:ListItem>
					<asp:ListItem Value="6">&nbsp;</asp:ListItem>
				</asp:radiobuttonlist>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label ID="LBrendiPubblico_t" Runat="server" cssclass="Titolo_campoSmall">&nbsp;Rendi Pubblico:&nbsp;</asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:CheckBox ID="CBXrendiPubblico" Runat="server" CssClass="Testo_CampoSmall" Checked="False"></asp:CheckBox>
			</asp:TableCell>
		</asp:TableRow>
		
	</asp:Table>
</asp:Panel>
<asp:Panel ID="PNLlingua" Runat="server">
	
		<asp:Repeater ID="RPTlingua" Runat="server">
			<ItemTemplate>
			<fieldset>
			<Table align="center" width="100%">
				<TR>
					<td width="250px">
						<asp:Label ID="LBnome_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Nome:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBnome" Runat="server" CssClass="TitoloServizio">
							<%#Container.DataItem("CNLN_nome")%>
						</asp:Label>
					</td>
				</TR>
				<TR>
					<td>
						<asp:Label ID="LBlettura_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Lettura:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBlettura" Runat="server" CssClass="Testo_CampoSmall">
							<%#Container.DataItem("oLettura")%>
						</asp:Label>
					</td>
				</TR>
				<TR>
					<td>
						<asp:Label ID="LBscrittura_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Scrittura:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBscrittura" Runat="server" CssClass="Testo_CampoSmall">
							<%#Container.DataItem("oScrittura")%>
						</asp:Label>
					</td>
				</TR>
				<TR>
					<td>
						<asp:Label ID="LBespressioneOrale_s" Runat="server" CssClass="Titolo_campoSmall">&nbsp;Abilità in Espressione Orale:</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBespressioneOrale" Runat="server" CssClass="Testo_CampoSmall">
							<%#Container.DataItem("oEspressioneOrale")%>
						</asp:Label>
					</td>
				</TR>
				<TR>
					<td>
						<asp:Label ID="LBrendiPubblico_s" Runat="server" cssclass="Titolo_campoSmall">&nbsp;Rendi Pubblico:&nbsp;</asp:Label>
					</td>
					<td>
						<asp:Label ID="LBrendiPubblico" Runat="server" cssclass="Testo_CampoSmall">
							<%#Container.DataItem("oCheck")%>
						</asp:Label>
					</td>
				</TR>
			
				<tr>
					<td colspan="2" align=right valign=middle >
						<asp:LinkButton ID="LKBmodifica" Runat="server" CommandName="modifica" CommandArgument='<%#Container.DataItem("CNLN_id")%>' CssClass="Linksmall_Under">Modifica</asp:LinkButton>
						<asp:Image ID="IMpipe" ImageUrl="../images/pipe.gif" Runat=server ImageAlign=AbsMiddle></asp:Image>
						<asp:LinkButton ID="LKBelimina" Runat="server" CommandName="elimina" CommandArgument='<%#Container.DataItem("CNLN_id")%>' CssClass="Linksmall_Under">Elimina</asp:LinkButton>
					</td>
				</tr>
				</Table>
				</fieldset>
				<br>
			</ItemTemplate>
		</asp:Repeater>
		
</asp:Panel>
