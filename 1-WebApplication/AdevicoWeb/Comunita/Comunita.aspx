<%@ Page Language="vb" validateRequest="false" MasterPageFile="~/AjaxPortal.Master" AutoEventWireup="false" Codebehind="Comunita.aspx.vb" Inherits="Comunita_OnLine.Comunita" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <TABLE id="Table1" class="contenitore" align=center>
		<TR class="contenitore">
			<TD colspan="3">
                <h5>
                    <br/>
                    <br/>
                    Sei dentro la comunità. Clicca sul menu in alto per accedere ai servizi offerti.
                </h5>
			</td>
		</tr>
	</table>
</asp:Content>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <head runat="server">
		<title>Comunità On Line</title>
		<link href="./../Styles.css" type="text/css" rel="stylesheet"/>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
  </HEAD>
	<body>
		<form id="aspnetForm" method="post" runat="server" enctype="multipart/form-data">
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
            <TR class="contenitore">
				<td>
                    <HEADER:CTRLHeader id="Intestazione" runat="server"></HEADER:CTRLHeader>
				</td>
			</tr>
			
			<FOOTER:CTRLFooter id="CTRLFooter" runat="server"></FOOTER:CTRLFooter>
		</form>
	</body>
</HTML>--%>