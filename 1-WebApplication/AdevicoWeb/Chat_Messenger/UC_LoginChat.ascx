<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_LoginChat.ascx.vb" Inherits="Comunita_OnLine.UC_LoginChat" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" type="text/javascript">
		function SubmitRicerca(event){
				if (document.all){
				if (event.keyCode == 13){
					event.returnValue=false;
					event.cancel = true;
					try{
						document.forms[0].<%=BTNlogin.ClientID%>.click();}
					catch (ex){
						return false;}
					}
				}
			else if (document.getElementById){
				if (event.which == 13){
					event.returnValue=false;
					event.cancel = true;
					try{
						document.forms[0].<%=BTNlogin.ClientID%>.click();}
					catch(ex){
						return false;}
					}
				}
			else if(document.layers){
				if(event.which == 13){
					event.returnValue=false;
					event.cancel = true;
						try{
						document.forms[0].<%=BTNlogin.ClientID%>.click();}
					catch(ex){
						return false;}
					}
				}
			else
				return true;
		}
		
</script>
<br/>
<input type="hidden" id="HDNcomunita_ID" runat="server" NAME="HDNcomunita_ID"/>
<asp:panel id="PNLLogin" Runat="server" Width="344px">
	<asp:Table id="TBLlogin" Runat="server" CellPadding="1" CellSpacing="0" Width="359px" HorizontalAlign="Center"
		BorderStyle="None">
		<asp:TableRow ID="TBRnoAccess" Visible="False" Width="359">
			<asp:TableCell VerticalAlign="Middle" Width="40px" Text="&lt;img src=&quot;./images/nopage.gif&quot; border=0 align=absmiddle&gt;"></asp:TableCell>
			<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
				<asp:Label runat="server" ID="LBnoAccess" CssClass="Titolo_NoAccesso"></asp:Label>
			</asp:TableCell>
			<asp:TableCell VerticalAlign="Middle" Width="40px" Text="&lt;img src=&quot;./images/nopage.gif&quot; border=0 align=absmiddle&gt;"></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Width="359">
			<asp:TableCell Width="40px" Text="&amp;nbsp;"></asp:TableCell>
			<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
				<asp:Label runat="server" ID="LBerrore" CssClass="errore"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="40px" Text="&amp;nbsp;"></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Width="359">
			<asp:TableCell Width="40px" Text="&amp;nbsp;"></asp:TableCell>
			<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
				<asp:Table runat="server" CellPadding="0" BackColor="#999999" Width="350px" HorizontalAlign="Center"
					CellSpacing="1" ID="Table1">
					<asp:TableRow>
						<asp:TableCell>
							<asp:Table runat="server" CellPadding="2" Width="100%" CellSpacing="0" ID="TBLcontrolli">
								<asp:TableRow BackColor="#FBFBF6">
									<asp:TableCell ColumnSpan="2" HorizontalAlign="Center" Text="
										&amp;nbsp;
									"></asp:TableCell>
								</asp:TableRow>
								<asp:TableRow BackColor="#FBFBF6">
									<asp:TableCell Width="31%" HorizontalAlign="Right">
										<asp:Label runat="server" ID="LBlogin_t" CssClass="Titolo_campo">Login:</asp:Label>
									</asp:TableCell>
									<asp:TableCell>
										<asp:TextBox runat="server" CssClass="Testo_campo" ID="TXBlogin" MaxLength="50"></asp:TextBox>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow BackColor="#FBFBF6">
									<asp:TableCell Width="31%" HorizontalAlign="Right">
										<asp:Label runat="server" ID="LBpassword_t" CssClass="Titolo_campo">Password:</asp:Label>
									</asp:TableCell>
									<asp:TableCell>
										<asp:TextBox runat="server" TextMode="Password" CssClass="Testo_campo" ID="TXBpwd" MaxLength="50"></asp:TextBox>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow BackColor="#FBFBF6">
									<asp:TableCell Height="10px" ColumnSpan="2" HorizontalAlign="Center" Text="
										&lt;span style=&quot;FONT-SIZE: 2px&quot;&gt;&amp;nbsp;&lt;/span&gt;
									"></asp:TableCell>
								</asp:TableRow>
								<asp:TableRow BackColor="#FBFBF6">
									<asp:TableCell HorizontalAlign="Right" Text="&amp;nbsp;"></asp:TableCell>
									<asp:TableCell HorizontalAlign="Right">
<asp:Button runat="server" ID="BTNlogin" BackColor="#99FFFF" 
											Font-Bold="True" style="WIDTH: 100px" Text="Entra"></asp:Button>

										&nbsp;&nbsp;&nbsp;
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow BackColor="#FBFBF6">
									<asp:TableCell ColumnSpan="2" HorizontalAlign="Center" Text="
										&amp;nbsp;
									"></asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
			<asp:TableCell Width="40px" Text="&amp;nbsp;"></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="40px" Text="&amp;nbsp;"></asp:TableCell>
			<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
				<asp:Table runat="server" ID="TBLmenu">
					<asp:TableRow>
						<asp:TableCell Height="12px" Text="&amp;nbsp;" CssClass="nosize0"></asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Height="18px" HorizontalAlign="Center">

<asp:LinkButton runat="server" ID="LNBPwdDimenticata" CausesValidation="False" CssClass="notaRecuperoPwd" Visible=false><asp:Label runat="server" ID="LBpasswdDimenticata" CssClass="notaRecuperoPwd" Visible=false>"Ho dimenticato la password... </asp:Label></asp:LinkButton>

						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
			<asp:TableCell Width="40px" Text="&amp;nbsp;"></asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</asp:panel>
<asp:panel id="PNL_avvisoMail" Runat="server" Visible="False" Width="359px" HorizontalAlign="Center">
	<TABLE width="359" align="center">
		<TR>
			<TD align="center">
				<asp:Label id="LBInvioMail" Runat="server" CssClass="avviso_normal"></asp:Label></TD>
		</TR>
		<TR>
			<TD align="center"><br/>
				<asp:Button id="BTNGoLogin" Runat="server" Width="200" CssClass="pulsante" CausesValidation="False"></asp:Button></TD>
		</TR>
	</TABLE>
</asp:panel>
<asp:panel id="PNLPwdDimenticata" Runat="server" Visible="False" Width="359px" HorizontalAlign="Center">
	<TABLE width="359" align="center" border="0">
		<TR>
			<TD align="center" colSpan="2">
				<asp:Label id="LBmail_t" Runat="server">Inserisci la mail con la quale sei iscritto al sistema</asp:Label></TD>
		</TR>
		<TR>
			<TD align="center" colSpan="2">
				<asp:TextBox id="TXBEmail" Runat="server" MaxLength="255" cssclass="Testo_campo_obbligatorio"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD align="center" colSpan="2">
				<asp:Button id="BTNPwdDimenticata" Runat="server" Width="100" CssClass="pulsante" CausesValidation="false"></asp:Button></TD>
		</TR>
	</TABLE>
</asp:panel>
<asp:Panel ID="PNLrestart" Runat="server" Visible="True" Width="359px" HorizontalAlign="Center">
	<TABLE width="359" align="center">
		<TR>
			<TD align="center">
				<asp:Label id="LBrestart" Runat="server" CssClass="avviso_normal"></asp:Label></TD>
		</TR>
		<TR>
			<TD align="center"><br/>
			</TD>
		</TR>
	</TABLE>
</asp:Panel>
<asp:panel id="PNLNoUser" Runat="server" Visible="False" Width="359px" HorizontalAlign="Center">
	<asp:Label id="LBuserNotFound" Runat="server">Utente non trovato</asp:Label>
	<br/>
	<asp:Button id="BTNRiprova" Runat="server" CssClass="pulsante"></asp:Button>
	<br/>
	<asp:Label id="LBadminContacts" Runat="server">Per problemi contatta l'Amministratore</asp:Label>
</asp:panel>
<asp:requiredfieldvalidator id="VLDLogin" runat="server" ControlToValidate="TXBlogin" ErrorMessage="Login" Display="none"></asp:requiredfieldvalidator>
<asp:requiredfieldvalidator id="VLDPassword" runat="server" ControlToValidate="TXBPwd" ErrorMessage="Password"
	Display="none"></asp:requiredfieldvalidator>
<asp:validationsummary id="VLDSum" runat="server" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false"
	HeaderText="Non hai inserito i seguenti valori:" Font-Size="12"></asp:validationsummary>
