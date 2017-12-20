<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AggiungiFaccina.aspx.vb" Inherits="Comunita_OnLine.AggiungiFaccina" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Aggiungi Emoticon</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema"/>
	</HEAD>
	<body >
		<table style="height:354px" cellSpacing="0" cellPadding="0" width="205" border="0">
			<tr vAlign="top">
				<td width="205" height="354">
					 <form id="aspnetForm" method="post" runat="server">
						<table style="height:188" cellSpacing="0" cellPadding="0" width="342" border="0">
							<tr vAlign="top">
								<td width="21" height="8"></td>
								<td width="3"></td>
								<td width="8"></td>
								<td width="8"></td>
								<td width="24"></td>
								<td width="7"></td>
								<td width="33"></td>
								<td width="16"></td>
								<td width="64"></td>
								<td width="41"></td>
								<td width="28"></td>
								<td width="11"></td>
								<td width="78"></td>
							</tr>
							<tr vAlign="top">
								<td colSpan="10" height="8"></td>
								<td rowSpan="2"><input id="UplImg" type="image" runat="server"/></td>
								<td colSpan="2"></td>
							</tr>
							<tr vAlign="top">
								<td height="32"></td>
								<td colSpan="4"><asp:label id="LBname_t" runat="server" Width="40px">Nome</asp:label></td>
								<td></td>
								<td colSpan="4"><asp:textbox id="TXBnome" runat="server" Width="152px"></asp:textbox></td>
								<td></td>
								<td><asp:requiredfieldvalidator id="RFVNome" runat="server" ErrorMessage="*" ForeColor="Blue" ControlToValidate="TXBnome"></asp:requiredfieldvalidator></td>
							</tr>
							<tr vAlign="top">
								<td colSpan="3" height="24"></td>
								<td colSpan="10"><asp:regularexpressionvalidator id="REVNome" runat="server" Width="296px" ErrorMessage="Caratteri consentiti: (0-1)  (a-z)  (A-Z) Max: 25"
										ForeColor="Blue" ControlToValidate="TBNome" Height="24px" ValidationExpression="([0-9]|[a-z]|[A-Z]){0,25}"></asp:regularexpressionvalidator></td>
							</tr>
							<tr vAlign="top">
								<td colSpan="2" height="32"></td>
								<td colSpan="6"><asp:label id="LBabbreviazione_t" runat="server" Width="88px">Abbreviazione :</asp:label></td>
								<td colSpan="3"><asp:textbox id="TXBabbreviazione" runat="server" Width="129px"></asp:textbox></td>
								<td colSpan="2"><asp:requiredfieldvalidator id="RFVAbb" runat="server" ErrorMessage="*" ForeColor="Blue"
										ControlToValidate="TXBabbreviazione"></asp:requiredfieldvalidator></td>
							</tr>
							<tr vAlign="top">
								<td colSpan="2" height="32"></td>
								<td colSpan="3"><asp:label id="LBemoticon" runat="server">Emoticon :</asp:label></td>
								<td colSpan="8"><input id="UplFile" type="file" size="26" name="UplFile" runat="server"/></td>
							</tr>
							<tr vAlign="top">
								<td colSpan="2" height="32"></td>
								<td colSpan="5"><asp:button id="BTNupload" runat="server" Width="56px" Text="Upload"></asp:button></td>
								<td colSpan="2"><asp:button id="BTNnew" runat="server" Width="56px" Text="Nuova"></asp:button></td>
								<td colSpan="2"><asp:button id="BTNback" runat="server" Width="56px" Text="Indietro"></asp:button></td>
								<td colSpan="2"></td>
							</tr>
							<tr vAlign="top">
								<td colSpan="4" height="20"></td>
								<td colSpan="3"><asp:label id="LBerrore" runat="server" CssClass="errore"></asp:label></td>
								<td colSpan="6"></td>
							</tr>
						</table>
					</form>
				</td>
			</tr>
		</table>
	</body>
</HTML>
