<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SessionTimeout.aspx.vb" Inherits="Comunita_OnLine.SessionTimeout" %>
<%@ Register TagPrefix="FOOTER" TagName="CTRLfooter" Src="UC/UC_PortalFooter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title runat="server">Comunità On Line - Sessione scaduta</title>
    <LINK href="./styles.css" type="text/css" rel="stylesheet"/>
</head>
<body>
     <form id="aspnetForm" runat="server">
      <div id="DVcontenitore" align="center">
		  <div id="DVheader" style="width: 900px; height: 150px" align="center">
			 &nbsp;
		  </div>
		  <div align="center" style="width: 900px; height: 350px; text-align:center; vertical-align:middle ">
			 <br /><br /><br />
			 <asp:Label ID="LBsessioneScaduta" runat="server" CssClass="HelpTitolo">Spiacenti, ma la sua sessione di lavoro è scaduta per continuare è necessario effettuare un nuovo accesso al sistema.</asp:Label>
			 <br /><br /><br />
			 <asp:Button ID="BTNaccesso" runat="server" Text="Accedi al sistema" CssClass="pulsante"/>
		  </div>
	   </div>
	   <FOOTER:CTRLfooter id="CtrLFooter" runat="server"></FOOTER:CTRLfooter>
    </form>
</body>
</html>
