<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminG_AggiungiTipoRuolo.aspx.vb"
    Inherits="Comunita_OnLine.AdminG_AggiungiTipoRuolo" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Comunita on line - Definizione nuovo Ruolo</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link href="./../Styles.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <script type="text/javascript" src="../scripts/jquery-1.6.1.min.js"></script>
    <script type="text/javascript" language="javascript">

            
            $(document).ready(function () {
                var Azione = window.opener.$("input[id$='HDNazione']").val();

                try{
						if (!(Azione=='gestioneTipo'))
							this.close();
					}
				catch(e){ 
						this.close();
					}			
				}

                });

            <%-- 
			function VerificaPadre(){
				try{
						if (!(this.opener.document.forms[0].HDNazione.value=='gestioneTipo'))
							this.close();
					}
				catch(e){ 
						this.close();
					}			
				}

            --%>
			function CloseAndReaload(){
				try{
					 if (!(this.opener.document.forms[0].HDNazione.value=='gestioneTipo'))
						this.close();
					else
						this.opener.document.forms[0].submit(); //this.opener.document.forms[0].__doPostBack(); //this.window.close();
					}
				catch(e){this.close();} 
			
			}
            
    </script>
</head>
<body>
    <%-- onload="VerificaPadre()">--%>
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server">
    </asp:ScriptManager>
    <table align="center" border="0" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td class="RigaTitolo">
                <asp:Label ID="LBtitoloRuolo" runat="server">Definizione nuovo ruolo</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" height="30px" class="top">
                &nbsp;<asp:LinkButton ID="LNBchiudi" runat="server" Text="Chiudi (x)" CssClass="Link_Menu"
                    CausesValidation="False"></asp:LinkButton>
                <asp:LinkButton ID="LNBcrea" runat="server" Text="Salva" CssClass="Link_Menu"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False">
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server">
                    <input type="hidden" id="HDNtprl_ID" runat="server" name="HDNtprl_ID" />
                    <asp:Table ID="TBLdati" runat="server" CellSpacing="1" CellPadding="0" HorizontalAlign="Center"
                        Width="600px">
                        <asp:TableRow ID="TBR_1">
                            <asp:TableCell Width="120px">
                                <asp:Label ID="LBnome_t" runat="server" CssClass="Titolo_campoSmall">*Nome Ruolo:</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="480px">
                                <asp:TextBox ID="TXBnome" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                    MaxLength="255" Columns="60"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" CssClass="Validatori"
                                    Display="Static" ControlToValidate="TXBnome">*</asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell CssClass="top" ColumnSpan="2">
                                <table border="1" align="left" bgcolor="#FFFBF7" style="border-color: #CCCCCC" cellpadding="0"
                                    cellspacing="0">
                                    <tr>
                                        <td>
                                            <table border="0" align="left" bgcolor="#FFFBF7" cellpadding="0" cellspacing="0">
                                                <asp:Repeater ID="RPTnome" runat="server">
                                                    <HeaderTemplate>
                                                        <tr>
                                                            <td colspan="2" height="20px">
                                                                <asp:Label ID="LBlinguaNome_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td align="right" width="120px" height="22px">
                                                                <asp:Label ID="LBlinguaID" Text='<%# Databinder.eval(Container.DataItem, "ID")%>'
                                                                    runat="server" Visible="false" />
                                                                <asp:Label ID="LBlingua_Nome" Text='<%# Databinder.eval(Container.DataItem, "Nome")%>'
                                                                    runat="server" Visible="true" CssClass="Repeater_Voce" />&nbsp;
                                                            </td>
                                                            <td align="left" height="22px">
                                                                <asp:TextBox ID="TXBtermine" runat="server" CssClass="Testo_campoSmall" MaxLength="255"
                                                                    Columns="60"> </asp:TextBox>&nbsp;&nbsp;
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr>
                                                            <td colspan="2" class="nosize0">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBR_2">
                            <asp:TableCell Width="120px">
                                <asp:Label ID="LBdescrizione_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Descrizione:</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TXBdescrizione" runat="server" CssClass="Testo_campoSmall" MaxLength="500"
                                    Columns="60"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell CssClass="top" ColumnSpan="2">
                                <table border="1" align="left" bgcolor="#FFFBF7" style="border-color: #CCCCCC" cellpadding="0"
                                    cellspacing="0">
                                    <tr>
                                        <td>
                                            <table border="0" align="left" bgcolor="#FFFBF7" cellpadding="0" cellspacing="0">
                                                <asp:Repeater ID="RPTdescrizione" runat="server">
                                                    <HeaderTemplate>
                                                        <tr>
                                                            <td colspan="2" height="20px">
                                                                <asp:Label ID="LBlinguaDescrizione_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td align="right" width="120px" height="22px">
                                                                <asp:Label ID="LBlingua2ID" Text='<%# Databinder.eval(Container.DataItem, "LNGU_ID")%>'
                                                                    runat="server" Visible="false" />
                                                                <asp:Label ID="LBlingua2_Nome" Text='<%# Databinder.eval(Container.DataItem, "LNGU_nome")%>'
                                                                    runat="server" Visible="true" CssClass="Repeater_Voce" />&nbsp;
                                                            </td>
                                                            <td align="left" height="22px">
                                                                <asp:TextBox ID="TXBtermine2" runat="server" CssClass="Testo_campoSmall" MaxLength="500"
                                                                    Columns="60"> </asp:TextBox>&nbsp;&nbsp;
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr>
                                                            <td colspan="2" class="nosize0">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBR_3">
                            <asp:TableCell Width="120px">
                                <asp:Label ID="LBlivelloRuolo_t" runat="server" CssClass="Titolo_campoSmall">&nbsp;Livello:</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:RadioButtonList ID="RBlivello" runat="server" RepeatDirection="Horizontal" CssClass="Testo_CampoSmall"
                                    RepeatLayout="Flow">
                                </asp:RadioButtonList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBR_4">
                            <asp:TableCell Width="100%" Height="35px" ColumnSpan="2" HorizontalAlign="Center">
                                <br />
                                <asp:Table ID="TBLlivelli" runat="server" CellSpacing="0" CellPadding="0" BorderColor="#3300cc"
                                    BorderWidth="1">
                                    <asp:TableRow>
                                        <asp:TableCell CssClass="ROW_header_Small_Center" Width="100px">
                                            <asp:Label ID="LBlivello_t" runat="server">LIVELLO</asp:Label></asp:TableCell>
                                        <asp:TableCell CssClass="ROW_header_Small_Center" Width="400px">
                                            <asp:Label ID="LBruoli_t" runat="server">RUOLI</asp:Label></asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
