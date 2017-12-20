<%@ Page Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="GestioneNotifiche.aspx.vb" Inherits="Comunita_OnLine.GestioneNotifiche" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register Assembly="RadEditor.Net2" Namespace="Telerik.WebControls" TagPrefix="radE" %>
<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_Header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>--%>
<%@ Register TagPrefix="CTRL" TagName="CTRLsorgenteComunita" Src="./../UC/UC_FiltroComunitaByServizio_NEW.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <script language="Javascript" src="./../jscript/EditorHTML.js" type="text/javascript"></script>
    <LINK href="./../Generici/Bacheca.css" type="text/css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<div id="DVtitle" style="width: 900px; text-align:left;" class="RigaTitolo" align="center">
		<asp:Label ID="LBtitoloServizio" Runat="server">Gestione Notifiche</asp:Label>
	</div>
	<div align="center">
	    <table>
	        <tr>
	            <td>
                    <CTRL:CTRLsorgenteComunita id="CTRLsorgenteComunita" runat="server" Width="800px" LarghezzaFinestraAlbero="800px" ColonneNome="100"></CTRL:CTRLsorgenteComunita>
                </td>
	        </tr>
            <tr>
                <td>
                    <asp:Label ID="LBLcomunita" runat="server"></asp:Label>
                </td>
            </tr>
	    </table>
	</div>
</asp:Content>

<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
<head id="Head1" runat="server">
    <title>Gestione Notifiche</title>

    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>

    <LINK href="./../styles.css" type="text/css" rel="stylesheet"/>

    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>

</head>

<body>
     <form id="aspnetForm" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
	   <div id="DVcontenitore" align="center">
		  <div id="DVheader" style="width: 900px;" align="center">
			 <HEADER:CtrLHeader id="Intestazione" runat="server"></HEADER:CtrLHEADER>
		  </div>

		  <div id="DVfooter" align="center" style="clear: both; width: 900px;">
			
		  </div>
	   </div>
	   <FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>
</body>
</html>--%>