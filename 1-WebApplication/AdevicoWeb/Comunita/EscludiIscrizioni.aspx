<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="EscludiIscrizioni.aspx.vb" Inherits="Comunita_OnLine.EscludiIscrizioni"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>
<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
<%--		<tr>
			<td class="titolo" align="center"><asp:label id="LBtitolo" CssClass="TitoloServizio" Runat="server">- Elenco comunità a cui si è iscritti -</asp:label></td>
		</tr>--%>
		<tr>
			<td bgColor="#a3b2cd" height="1"></td>
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
								<asp:Label id=LBNopermessi Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td vAlign=top height=50>
								&nbsp; 
							</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLData" Runat="server" HorizontalAlign="Center">
					<radt:RadTreeView ID="RDTcomunita" runat="server" CssFile="~/RadControls/TreeView/Skins/Comunita/StyleImport.css" width="700px" 
					ImagesBaseDir="~/RadControls/TreeView/Skins/Comunita/" skin="Comunita" />
				</asp:Panel>
			</td>
		</tr>
	</table>

    <input type=hidden id="HDNazione" runat=server NAME="HDNazione"/>

</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
  <head runat="server">
		<title>Comunità On Line - Escludi Iscrizione se iscritto ad altra comunità</title>
		
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
		
</HEAD>
	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" encType="multipart/form-data" runat="server">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3" >
						<HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>
					</td>
				</tr>
				<tr>
					<td colSpan="3">

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
