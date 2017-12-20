<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
Codebehind="AdminG_SchedaInformativa.aspx.vb" Inherits="Comunita_OnLine.AdminG_SchedaInformativa"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>


<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<asp:panel id="PNLdettagli" Runat="server" HorizontalAlign="Center" Visible="False">
		<table width="500" align="center" border="0">
			<tr>
				<td align="center">
					<FIELDSET><LEGEND class="tableLegend">Scheda Informativa</LEGEND>
						<DETTAGLI:CTRLDettagli id="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>
					</FIELDSET>
				</td>
			</tr>
			<tr>
			<td align=center ><asp:Button ID="BTNindietro" Runat=server CssClass ="pulsante" Visible =False Text =Indietro></asp:Button></td>
			</tr>
		</table>
	</asp:panel>
	<asp:panel id="PNLnoquery" Runat="server" Visible="False">
		<table align="center">
			<tr>
				<td height="50" colspan=2>&nbsp;</td>
			</tr>
			<tr>
				<td align="center" colspan=2>
					<asp:Label id="LBnoquery" CssClass="messaggio" Runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td height="50">&nbsp;<asp:Button runat="server" CssClass ="pulsante" ID="BTNlistacmnt" Text="Lista Comunità"></asp:Button></td>
				<td height="50">&nbsp;<asp:Button runat="server" CssClass ="pulsante" ID="BTNricercacmnt" Text="Ricerca per Persona"></asp:Button></td>
			</tr>
						
		</table>
	</asp:panel>
</asp:Content>

<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Scheda Informativa</title>

		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
	</HEAD>
	<body>
		<form id="aspnetForm" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table id="table1" cellSpacing="1" cellPadding="1" width="780" align="center" border="0">
				<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td colSpan="3" align=center>

					</td>
				</tr>
				<tr>
					<td colSpan="3"></td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>--%>

