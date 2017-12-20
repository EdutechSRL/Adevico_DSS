<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PermissionDetails.aspx.vb" Inherits="Comunita_OnLine.PermissionDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title runat="server">Comunità On Line - Permessi Default</title>
    <LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
</head>
<body style=" border-top: 0px;">
    <form id="aspnetForm" runat="server">   
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
	   <div style=" width:100%; left: 0px; position: absolute; top: 0px;">
		  <div class="RigaTitolo">
			 <asp:Label id="LBtitoloDettagli" Runat="server">Permessi Default Servizio:&nbsp;</asp:Label>
		  </div>
		  <div align="right" style=" text-align:right">
			  <asp:Button ID="BTNchiudi" runat="server" Text="Chiudi Finestra(x)" CssClass="PulsantiMenu"/>
		  </div>
		  <div align="center" style=" text-align:center;">
			 <asp:MultiView ID="MLVdati" runat="server">
				<asp:View ID="VIWpermessi" runat="server">
				    <br /><br /><br /><br /><br /><br />
				    <asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label>
				    <br /><br /><br /><br /><br /><br />
				</asp:View>
				<asp:View ID="VIWimpostazioni" runat="server">
				    <asp:Table Runat=server HorizontalAlign=center ID="TBLpermessiRuoli" GridLines=Both  CellSpacing="0" CellPadding="3" BorderColor="#2A4DA1" Width=600px>								
				    </asp:Table>
				</asp:View>
				<asp:View ID="VIWnoservizio" runat="server">
				    <br /><br /><br /><br /><br /><br />
				    <asp:Label id="LBnoServizio" Runat="server" CssClass="messaggio"></asp:Label>
				    <br /><br /><br /><br /><br /><br />
				</asp:View>
			 </asp:MultiView>
		  </div>
	   </div>
    </form>
  </body>
</html>
