<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
 Codebehind="AdminG_MenuComunita.aspx.vb" Inherits="Comunita_OnLine.AdminG_MenuComunita"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>


<%@ Register TagPrefix="radt" Namespace="Telerik.WebControls" Assembly="RadTreeView.NET2" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <script type="text/javascript" language="javascript">
	    function ProcessClientDrop(sourceNode, destNode) {
		    /* if (destNode.Category > 0
		    alert (sourceNode.Category)
		    alert (destNode.Category)*/
		    return confirm('Confermare ?');

	    }

	    function ExpandAll() {
		    var i;
		    for (i = 0; i < Tree.AllNodes.length; i++) {
		        var node = Tree.AllNodes[i];
		        if (node.Nodes.length > 0) {
		            node.Expand();
		        }
		    }
	    }

	    function CollapseAll() {
		    var i;
		    for (i = 0; i < Tree.AllNodes.length; i++) {
		        var node = Tree.AllNodes[i];
		        if (node.Nodes.length > 0) {
		            node.Collapse();
		        }
		    }
	    }
	    function ContextMenuClick(node, itemText) {
		    if (itemText == 'Cancella')
		        return confirm('Sicuro di voler cancellare l\'elemento selezionato ?')
		    else if (itemText == 'Delete')
		        return confirm('Are you sure to delete the selected item ?')
		    else
		        return true;
	    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
<%--	<table cellSpacing="0" cellPadding="0" width="900px" border="0">
		<tr>
			<td class="RigaTitoloAdmin" align="left">
				<asp:Label ID="LBtitolo" Runat="server">Gestione Menu Comunità</asp:Label>
			</td>
		</tr>
		<tr>
			<td align=right>

			</td>
		</tr>
	</table>--%>
	<table  align="center">
		<tr>
			<td class=top>
				<br/>
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign=Center>
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
	</table>
</asp:Content>




<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>Comunità OnLine - Amministrazione Menu</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
    <LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>

  </head>
  <body >

    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		<table cellSpacing="0" cellPadding="0" width="900px" align="center" border="0">
			<tr>
				<td colSpan="3">
					<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
				</td>
			</tr>
			<tr>
				<td colspan="3" valign="top">

				</td>
			</tr>
		</table>
		<FOOTER:CTRLFOOTER id="CTRLFooter" runat="server"></FOOTER:CTRLFOOTER>
    </form>
  </body>
</html>--%>