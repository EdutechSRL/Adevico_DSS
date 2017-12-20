<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="activateFromMail.aspx.vb"
    Inherits="Comunita_OnLine.activateFromMail" MasterPageFile="~/Authentication.master" %>
<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
     <div id="form" class="section">
        <asp:Panel ID="PNLInfo" runat="server" Visible="true">
            <table align="center" width="600px">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="LBinfoAccesso" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <asp:Button ID="BTNchiudi" runat="server" Text="chiudi finestra"></asp:Button>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>