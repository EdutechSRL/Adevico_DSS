<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NotificaMessaggio.aspx.vb"
    Inherits="Comunita_OnLine.NotificaMessaggio" %>
    <%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLrubrica" Src="./../Generici/UC/UC_RubricaMailGenerica.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
<head runat="server" id="Head1">
    <title>Scelta Notifica</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link href="./../Styles.css" type="text/css" rel="stylesheet" />
    <asp:Literal ID="Lit_Skin" runat="server"></asp:Literal>
    <link href="../Graphics/Modules/Forum/forum.new.css" rel="Stylesheet" />

    
</head>
<script language="javascript" type="text/javascript">
    function ChiudiMi() {
        this.window.close();
    }
</script>
<body class="internal_body">
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server">
    </asp:ScriptManager>

        <div class="popup">

            <div class="tab">
                <telerik:radtabstrip id="TBSselezione" runat="server" align="Justify" Width="100%" Height="26px" SelectedIndex="0"
                                causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                                <tabs>
                                    <telerik:RadTab text="Seleziona Destinatari" value="TABselezione" runat="server"></telerik:RadTab>
                                    <telerik:RadTab text="Messaggio Notifica" value="TABmessaggio" runat="server" ></telerik:RadTab>
                                </tabs>
                            </telerik:radtabstrip>

            </div>

            <div class="content">


        <asp:Panel ID="PNLpermessi" runat="server" Visible="False" HorizontalAlign="Center">
            <div class="messages">
                <div class="message error">
                    <span class="icons"><span class="icon">&nbsp;</span></span><asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>  
        </asp:Panel>

        <asp:Panel ID="PNLselezione" runat="server" HorizontalAlign="left">
            <div class="fieldobject clearfix" ID="TBRconferma" runat="server">
                <div class="fieldrow left">
                    <asp:Label ID="LBconferma_t" runat="server" CssClass="RubricaGenerale_Selezione">Premi il pulsante per confermare i destinatari:</asp:Label>
                </div>
                <div class="fieldrow right">
                    <asp:Button ID="BTNconfermaSelezionati" runat="server" CssClass="RubricaGenerale_Pulsante"
                                    Text="Conferma"></asp:Button>
                </div>
            </div>
            <div class="fieldobject">
                <div class="fieldrow">
                    <CTRL:CTRLrubrica ID="CTRLrubrica" runat="server" ShowSelezioneComunita="false" ShowSelezioneDestinatari="false">
                                </CTRL:CTRLrubrica>
                </div>
            </div>                    
        </asp:Panel>
        
        <asp:Panel ID="PNLconferma" runat="server" Visible="false">
            <div class="content">
            <div class="fieldobject">
                <div class="fieldrow title">
                    <asp:Label ID="LBtitoletto_t" CssClass="Titolo_Campo" runat="server"></asp:Label>
                </div>
                <div class="fieldrow">
                    <asp:Label ID="LBdestinatari_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBdestinatari">Destinatari:</asp:Label>
                    <asp:Label ID="LBdestinatari" runat="server" CssClass="RubricaGenerale_Testo"></asp:Label>
                </div>
                <div class="fieldrow">
                    <asp:Label ID="LBsceltaTesto_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBLsceltaTesto">Tipo Messaggio:</asp:Label>
                    <asp:RadioButtonList ID="RBLsceltaTesto" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                    CssClass="RubricaGenerale_Testo" RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Selected="true">leggi ...</asp:ListItem>
                                    <asp:ListItem Value="2">prendi visione ..</asp:ListItem>
                                    <asp:ListItem Value="3">intervieni ...</asp:ListItem>
                                </asp:RadioButtonList>
                </div>
                <div class="fieldrow">
                    <asp:Label ID="LBoggetto_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBoggetto">Oggetto:</asp:Label>
                    <asp:Label ID="LBoggetto" runat="server" CssClass="RubricaGenerale_Testo"></asp:Label>
                </div>
                <div class="fieldrow">
                    <asp:Label ID="LBtesto" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBtesto">Testo:</asp:Label>
                    <asp:TextBox ID="TXBtesto" runat="server" CssClass="RubricaGenerale_Testo" TextMode="MultiLine"
                                    Width="450px" Columns="50" Rows="10" ReadOnly="True"></asp:TextBox>
                </div>
            </div>
            <div class="fieldobject clearfix">
                <div class="fieldrow left">
                    <asp:Button ID="BTNannulla" runat="server" CssClass="RubricaGenerale_Pulsante" Text="Chiudi finestra">
                                </asp:Button>
                </div>
                <div class="fieldrow right">
                    <asp:Button ID="BTNinvia" runat="server" CssClass="RubricaGenerale_Pulsante" Text="Invia">
                                </asp:Button>
                </div>
            </div>
                    
                </div>    
                </asp:Panel>
                
            </div>
            </div>
    
    </form>
</body>
</html>
