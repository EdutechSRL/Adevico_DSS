<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ADM_InfoPersona.aspx.vb"
    Inherits="Comunita_OnLine.ADM_InfoPersona" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Comunità on Line</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link href="../Styles.css" type="text/css" rel="stylesheet" />
</head>

<script type="text/javascript" language="javascript">
    function ChiudiMi() {
        this.window.close();
    }
</script>

<body id="popup">
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server">
    </asp:ScriptManager>
    <fieldset>
        <legend class="tableLegend">Info</legend>
        <br />
        <asp:Table CellPadding="0" CellSpacing="0" Width="100%" runat="server" ID="TBLDettagli">
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBorganizzazione_t" runat="server" CssClass="Titolo_campo">Organizzazione Default:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="3">
                    <asp:Label runat="server" ID="LBorganizzazione" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBanagrafica" runat="server" CssClass="Titolo_campo_Rosso">Anagrafica</asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="3">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="dettagli_separatore" ColumnSpan="4">
							<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
								height="2px"/></asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBlogin_c" runat="server" CssClass="Titolo_campo">Login:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBlogin" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2" RowSpan="7" HorizontalAlign="Center">
                    <asp:Image ID="IMFoto" runat="server" Visible="False" ToolTip="Immagine Personale"
                        Height="125px" Width="100px"></asp:Image>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBnome_t" runat="server" CssClass="Titolo_campo">Nome:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBNome" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBcognome_t" runat="server" CssClass="Titolo_campo">Cognome:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBCognome" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBDataNascita_c" runat="server" CssClass="Titolo_campo">Data Nascita:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBDataNascita" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBSex_c" runat="server" CssClass="Titolo_campo">Sesso:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBSex" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBlingua_c" runat="server" CssClass="Titolo_campo">Lingua:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBlingua" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px" ID="TBRCodFiscale">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBCodFiscale_c" runat="server" CssClass="Titolo_campo">Codice Fiscale:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="2">
                    <asp:Label runat="server" ID="LBCodFiscale" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="2">
                    <asp:Label ID="LBrecapito" runat="server" CssClass="Titolo_campo_Rosso">Recapito</asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="dettagli_separatore" ColumnSpan="4">
							<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
								height="2px"/>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBProvincia_c" runat="server" CssClass="Titolo_campo">Provincia:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBProvincia" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBStato_c" runat="server" CssClass="Titolo_campo">Stato:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBStato" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px" ID="TBRindirizzo">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBIndirizzo_c" runat="server" CssClass="Titolo_campo">Indirizzo:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBIndirizzo" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px" ID="TBRcap_citta">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBCap_c" runat="server" CssClass="Titolo_campo">Cap:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBCap" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBCitta_c" runat="server" CssClass="Titolo_campo">Città:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBCitta" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px" ID="TBRtel1_2">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBTel1_c" runat="server" CssClass="Titolo_campo">Telefono1:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBTel1" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBTel2_c" runat="server" CssClass="Titolo_campo">Telefono2:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBTel2" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px" ID="TBRCell_Fax">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBCel_c" runat="server" CssClass="Titolo_campo">Cellulare:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBCel" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBFax_c" runat="server" CssClass="Titolo_campo">Fax:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label runat="server" ID="LBFax" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px" ID="TBRricezSMS">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBricSMS_c" runat="server" CssClass="Titolo_campo">Ricezione SMS:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="3">
                    <asp:Label runat="server" ID="LBricSMS" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px" ID="TBRmail">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBMail_c" runat="server" CssClass="Titolo_campo">Mail:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="3">
                    <asp:HyperLink ID="HLmail" runat="server">
                        <asp:Label runat="server" ID="LBMail" CssClass="Testo_campo"></asp:Label>
                    </asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBmostraMail_c" runat="server" CssClass="Titolo_campo">Mostra e-Mail:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="3">
                    <asp:Label runat="server" ID="LBmostraMail" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TBRhomePage" Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top">
                    <asp:Label ID="LBHomePage_c" runat="server" CssClass="Titolo_campo">Home Page:</asp:Label>
                </asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="3">
                    <asp:Label runat="server" ID="LBHomePage" CssClass="Testo_campo"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TBRdettagliRuolo">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="Top" ColumnSpan="3">
                    <asp:Label ID="LBdettagliRuolo" runat="server" CssClass="Titolo_campo_Rosso">Dettagli Ruolo</asp:Label>
                </asp:TableCell>
                <asp:TableCell>&nbsp;</asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="22px">
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="dettagli_separatore" ColumnSpan="4">
							<img src="./../images/separatore_small.gif" alt="" align="top" border="0" width="100%"
								height="2px"/>
                </asp:TableCell>
                <asp:TableCell Width="5px" CssClass="nosize0">&nbsp;</asp:TableCell>
            </asp:TableRow>
         </asp:Table>
        <br />
    </fieldset>
    <table align="right" border="0">
        <tr>
            <td>
                <asp:Button runat="server" ID="BTNOk" Text="Chiudi" CssClass="pulsante"></asp:Button>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
