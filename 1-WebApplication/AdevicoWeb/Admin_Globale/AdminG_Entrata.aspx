<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_Entrata.aspx.vb" Inherits="Comunita_OnLine.AdminG_Entrata"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table Width="900px" align="center" border=0>
<%--		<tr class="RigaTitoloAdmin">
			<td align="left" class="RigaTitoloAdmin">
				<asp:label id="LBtitolo" Runat="server">Comunicazioni generali -</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td align=right >&nbsp;</td>
		</tr>
		<tr>
			<td align="center">
				<asp:panel id="PNLpermessi" Runat="server" HorizontalAlign="Center" Visible="False">
					<table align=center>
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
						<tr>
							<td align=center>
								<asp:Label id=LBNopermessi Runat="server" CssClass="messaggio">Spiacente, non dispone dei permessi neccessari per accedere a tale servizio</asp:Label>
							</td>
						</tr>
						<tr>
							<td height=50>&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center" Width="900px">
					<asp:Label ID="LBcontenuto" Runat=server>Benvenuto nella sezione di amministrazione globale !</asp:Label>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>

<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
  <head runat="server">
		<title>Comunità On Line - Amministrazione Globale</title>

		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
</HEAD>
	<body>
		<form id="aspnetForm" method="post" encType="multipart/form-data" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0"  align="center" border="0">
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
</HTML>
--%>