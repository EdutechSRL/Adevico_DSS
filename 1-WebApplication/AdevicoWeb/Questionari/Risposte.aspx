<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Risposte.aspx.vb" Inherits="Comunita_OnLine.Risposte"  %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"  "http://www.w3.org/TR/html4/strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head id="Head1" runat="server">
    <title>Risposte Questionario</title>
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
    <link href="./../Styles.css?v=201604071200lm" type="text/css" rel="stylesheet" />

    <style type="text/css">
        div.page-wrapper {
	        text-align:left;
        }
    </style>
</head>
<body class="bodyCompile">
    <form runat="server">
         <asp:ScriptManager ID="SCMmanager" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
        </asp:ScriptManager>
        <div id="container" class="page-wrapper">
            <asp:Panel runat="server" ID="PNLerrore" Visible="false">
                <br /><br /><br /><br />
                <h2><asp:Label runat="server" id="LBerrore"></asp:Label></h2>
                <br /><br /><br /><br />
            </asp:Panel>
            <asp:Panel runat="server" ID="PNLDettagli">
                <br />
                <asp:LinkButton Visible="false" runat="server" ID="LNBStampaQuestionario" Text="Stampa Questionario"
                    Width="160px"></asp:LinkButton><br />
                <asp:DataList runat="server" ID="DLquestionari">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="LBnomeQuestionario"></asp:Label>
                        <br />
                        <asp:Label runat="server" ID="LBnomeCompilatore"></asp:Label>
                        <asp:PlaceHolder runat="server" ID="PHquestionario"></asp:PlaceHolder>
                        <p style="page-break-before: always;">
                        </p>
                    </ItemTemplate>
                </asp:DataList>
                <asp:PlaceHolder runat="server" ID="prova"></asp:PlaceHolder>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
