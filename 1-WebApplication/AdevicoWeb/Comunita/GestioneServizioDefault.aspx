<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="GestioneServizioDefault.aspx.vb" Inherits="Comunita_OnLine.GestioneServizioDefault"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td align="left" class=RigaTitolo>
				<asp:label id="LBTitolo" Runat="server" >Servizio default</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td align=right >
				<asp:linkbutton id="LKBaggiorna" Runat="server" Text="Aggiorna" CssClass=Link_Menu></asp:linkbutton>
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" CssClass="messaggio" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<table align="center">
						<tr>
							<td align="center">
								<br/><br/>
								<asp:Table ID="TBLservizioDefault" Runat=server HorizontalAlign=Center Width="750px">
									<asp:TableRow >
										<asp:TableCell  HorizontalAlign=Center >
											<asp:Label ID="LBavviso" Runat=server CssClass ="avviso_normal">
											Se nella Comunità è attiva la "cover", cliccando su di essa di accede al servizio default.
											Se la Cover è disattivata si accede direttamente al servizio selezionato di default.
											</asp:Label>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow >
										<asp:TableCell CssClass=top Wrap=False>
											<br/>
											<asp:Label ID="LBpaginaDefault_t" Runat=server CssClass="Titolo_campoSmall">Attiva all'accesso il servizio:</asp:Label>&nbsp;
											<asp:DropDownList ID="DDLpagineDefault" Runat=server cssClass="Testo_campoSmall"></asp:DropDownList>
										</asp:TableCell>
									</asp:TableRow>
														
								</asp:Table> 
							</td>
						</tr>
					</table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line</title>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
	</HEAD>
	<body>
		<form id="aspnetForm" method="post" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
				<tr>
				    <td colspan="3" >
				    <div>
				        <HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>	
				    </div>
				    </td>
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
</HTML>--%>