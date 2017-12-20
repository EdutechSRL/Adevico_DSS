<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Chat_Help.aspx.vb" Inherits="Comunita_OnLine.Chat_Help"%>
<%@ Register TagPrefix="UC" TagName="ChatHelp" Src="UC_ChatHelp.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>Chat_Help</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
    <LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
  </head>
  <body>

    <form id="aspnetForm" method="post" runat="server">
		<UC:ChatHelp id="UC_ChatHelp" runat="Server"></UC:ChatHelp>
    </form>
  </body>
</html>
