<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InfoModeratori.aspx.vb"
    Inherits="Comunita_OnLine.InfoModeratori" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Comunità on Line</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link href="../Styles.css" type="text/css" rel="stylesheet" />
    <asp:Literal ID="Lit_Skin" runat="server"></asp:Literal>
    <link href="../Graphics/Modules/Forum/forum.new.css" rel="Stylesheet" />
</head>

<script language="javascript" type="text/javascript">
			function ChiudiMi(){
			this.window.close();
			}
</script>

<body>
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server">
    </asp:ScriptManager>
        <div class="popup">
    <fieldset>
        <legend class="tableLegend">
            <asp:Label ID="LBlegend" runat="server">Info Moderatori</asp:Label></legend>
        
        <asp:DataList ID="DTLmoderatori" runat="server" CssClass="datagridforum" ShowFooter="true">
            <ItemStyle CssClass="ForumNW_RowNormal"></ItemStyle>
            <AlternatingItemStyle CssClass="ForumNW_RowAlternato"></AlternatingItemStyle>
            <HeaderTemplate>
                <table class="table forumtable minimal moderators">
                    <tr class="tableheader">
                        <th class="login">
                            <asp:Label ID="LBlogin_t" runat="server">Login</asp:Label>
                        </th>
                        <th class="user">
                            <asp:Label ID="LBanagrafica_t" runat="server">Anagrafica</asp:Label>
                        </th>
                        <th class="role">
                            <asp:Label ID="LBruolo_t" runat="server">Ruolo</asp:Label>
                        </th>
                        <th class="email">
                            <asp:Label ID="LBemail_t" runat="server">E-mail</asp:Label>
                        </th>
                    </tr>
                
            </HeaderTemplate>            
            <ItemTemplate>                
                    <tr class="tablerow">
                        <td class="login">
                            <asp:Label ID="LBlogin" runat="server"><%# DataBinder.Eval(Container.DataItem, "PRSN_login") %></asp:Label>
                        </td>
                        <td class="user">
                            <asp:Label ID="LBanagrafica" runat="server">
                                <asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' Text='<%# Container.Dataitem("Anagrafica")%>'
                                    runat="server" ID="HYPMail" /></asp:Label>
                        </td>
                        <td class="role">
                            <asp:Label ID="LBruolo" runat="server"><%# DataBinder.Eval(Container.DataItem, "TPRF_nome") %></asp:Label>
                        </td>
                        <td class="email">
                            <asp:Label ID="LBemail" runat="server">
                                <asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem("PRSN_mail")%>' Text='<%# Container.Dataitem("PRSN_mail")%>'
                                    runat="server" ID="HYPMail2" /></asp:Label>
                        </td>
                    </tr>                
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:DataList>
        
    </fieldset>
            <div class="footer">
                <asp:Button runat="server" ID="BTNOk" Text="Chiudi" CssClass="pulsante"></asp:Button>
            </div>
    
        </div>
    </form>
</body>
</html>
