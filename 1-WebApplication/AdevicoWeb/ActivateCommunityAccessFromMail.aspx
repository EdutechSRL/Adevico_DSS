<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="ActivateCommunityAccessFromMail.aspx.vb" Inherits="Comunita_OnLine.ActivateCommunityAccessFromMail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div>
        <div>
             <asp:Label ID="LBinfoAccesso" runat="server"></asp:Label>
             <br /><br />
        </div>
    </div>
      <asp:Panel ID="PNLInfo" runat="server" Visible="true">
                    <table align="center" width="600px">
                        <tr>
                            <td align="center" colspan="2">
                               
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="BTNhome" runat="server" CssClass="Pulsante" Text="Home"></asp:Button>
                            </td>
                            <td align="right">
                                <asp:Button ID="BTNchiudi" runat="server" CssClass="Pulsante" Text="chiudi finestra">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>
