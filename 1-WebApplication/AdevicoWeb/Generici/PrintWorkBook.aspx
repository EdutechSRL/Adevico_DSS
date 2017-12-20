<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintWorkBook.aspx.vb"
    Inherits="Comunita_OnLine.PrintWorkBook" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.Base.DomainModel" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Comunità On Line - WorkBook</title>
    <link href="./../Styles.css" type="text/css" rel="stylesheet" />
</head>

<script language="javascript" type="text/javascript">
    function ChiudiMi() {
        this.window.close();
    }

    function stampa() {
        this.window.print();
    }
</script>

<body style="margin-top: 0px; margin-right: 0px; margin-left: 0px;" id="popup">
     <form id="aspnetForm" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
    <div style="margin: 0 auto; bottom: 0px 0px; width: 95%">
        <div style="width: 100%; text-align: right; padding: 5px 5px;">
            <asp:Button runat="server" ID="BTNclose" Text="Chiudi" CssClass="LINK_MENU" OnClientClick="return ChiudiMi();">
            </asp:Button>&nbsp;
            <asp:Button runat="server" ID="BTNprintItems" Text="Stampa" CssClass="LINK_MENU"
                OnClientClick="stampa();return false;"></asp:Button>
        </div>
        <div style="width: 100%; text-align: center; border: 1px; border-color: Black; border-style: solid;
            clear: both;">
            <div style="width: 100%; text-align: center; height: 24px;" class="DiarioLezioni_DGheader">
                <asp:Label ID="LBtitle" runat="server">Items</asp:Label>
            </div>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <asp:Repeater ID="RPTitemsDetails" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td scope="row" width="900px" align="left" class="DiarioLezioni_HeaderRigaLezione_Bold"
                                valign="middle">
                                <div style="clear: both; width: 900px;">
                                    <div style="float: left; padding-left: 5px; width: 22px; text-align: left; height: 20px;
                                        vertical-align: middle;">
                                    </div>
                                    <div style="float: left; padding-left: 10px; padding-top: 2px; width: 582px; text-align: left;
                                        height: 20px; vertical-align: middle;">
                                        <a name="<%# Container.DataItem.Item.ID.tostring() %>">&nbsp;</a><asp:Literal ID="LTitemHeader"
                                            runat="server" EnableViewState="false"></asp:Literal>
                                    </div>
                                    <div style="float: left; padding-right: 5px; padding-top: 2px; text-align: right;
                                        width: 180px; height: 20px; vertical-align: middle;">
                                    </div>
                                    <div style="float: left; padding-right: 5px; padding-top: 2px; text-align: right;
                                        width: 80px; height: 20px; vertical-align: middle;">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                             <td width="900px">
                                <div id="DIVtitolo" style="padding: 5px; text-align: left; word-wrap: break-word;"
                                    runat="server">
                                    <asp:Label runat="server" ID="LBtitolo_t" CssClass="Titolo_campoSmall">Titolo:</asp:Label>
                                    <span class="Testo_campoSmall">
                                        <%# Container.DataItem.Item.Title%></span>
                                </div>
                                <div id="DIVtext" style="padding: 0px 5px 5px 5px; text-align: left;" runat="server">
                                    <asp:Label runat="server" ID="LBprogramma_t" CssClass="Titolo_campoSmall">Programma:</asp:Label>
                                    <span class="Testo_campoSmall">
                                        <%# Container.DataItem.Item.Body%></span>
                                </div>
                                <div id="DIVnote" style="padding: 0px 5px 5px 5px; text-align: left;" runat="server">
                                    <asp:Label runat="server" ID="LBnote_t" CssClass="Titolo_campoSmall">Note personali:</asp:Label>
                                    <span class="Testo_campoSmall">
                                        <%# Container.DataItem.Item.Note%></span>
                                </div>
                                <div id="DIVmateriale" style="padding: 0px 5px 5px 5px; text-align: left;" runat="server">
                                    <div style="float: left; vertical-align: text-top; text-align: left;">
                                        <asp:Label runat="server" ID="LBmateriale_t" CssClass="Titolo_campoSmall">Materiale:</asp:Label>&nbsp;
                                    </div>
                                    <div style="float: left; vertical-align: text-top; text-align: left; margin-left: -30px;
                                        width: 640px;">
                                        <asp:Repeater ID="RPTitemFiles" runat="server">
                                            <HeaderTemplate>
                                                <ul>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li style="display: inline; width: 380px; vertical-align: text-top;">
                                                    <asp:Label ID="LBnomeFile" runat="server" CssClass="Testo_campoSmall" Visible="true"></asp:Label>
                                                    <asp:Label ID="LBdimensione" runat="server" CssClass="Testo_campoSmall"></asp:Label>&nbsp;
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                <div id="DIVitemSeparator" style="padding: 0px 5px 5px 5px; text-align: left; clear: both;"
                                    runat="server">
                                    &nbsp;&nbsp;&nbsp;
                                </div>
                                <div id="DIVadminPanel" style="padding: 5px 5px 5px 5px; text-align: left; clear: both;
                                    background-color: Beige; height: 20px; vertical-align: middle;" runat="server">
                                    <asp:Label runat="server" ID="LBstatusItem_t" CssClass="Titolo_campoSmall">Status:</asp:Label>&nbsp;
                                    <asp:Label ID="LBstatusItem" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div style="padding: 150px 0px; margin: 0px auto; width: 100%; text-align: center;
            height: 400px; vertical-align: middle;" runat="server" id="DIVpermessi">
            <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio">Spiacente, non dispone dei permessi necessari per visualizzare il diaro personale.</asp:Label>
        </div>
        <br />
    </div>
    </form>
</body>
</html>
