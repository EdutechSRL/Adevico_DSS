<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="anteprimaTemplate.aspx.vb" Inherits="Comunita_OnLine.anteprimaTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
      <title>Comunità On Line - Anteprima Template</title>
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link href="./../Styles.css?v=201604071200lm" type="text/css" rel="stylesheet" />
</head>
<body>
     <form id="aspnetForm" runat="server">
          <asp:ScriptManager ID="SCMmanager" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
        </asp:ScriptManager>
    <div>
    <asp:Label runat="server" ID="LBAnteprima"></asp:Label>
    </div>
    </form>
</body>
</html>
