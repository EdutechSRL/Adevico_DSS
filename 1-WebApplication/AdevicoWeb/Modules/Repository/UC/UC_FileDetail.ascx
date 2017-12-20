<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FileDetail.ascx.vb"
    Inherits="Comunita_OnLine.UC_FileDetail" %>
<asp:MultiView ID="MLVdetails" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWitem" runat="server">
        <table width="500px">
            <tr>
                <td valign="top">
                    <asp:Label ID="LBcommunity_t" runat="server" CssClass="Titolo_campoSmall">Community:</asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBcommunity" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="LBpath_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBpath" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="LBname_t" runat="server" CssClass="Titolo_campoSmall">Name:</asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBname" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LBcreatedOn_t" runat="server" CssClass="Titolo_campoSmall">Name:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="LBcreatedOn" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr id="TRdescription" runat="server">
                <td valign="top">
                    <asp:Label ID="LBdescription_t" runat="server" CssClass="Titolo_campoSmall">Description:</asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBdescription" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr id="TRtype" runat="server" visible="false">
                <td valign="top">
                    <asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall">Type:</asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBtype" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr id="TRdownloaded" runat="server">
                <td valign="top">
                    <asp:Label ID="LBdownloaded_t" runat="server" CssClass="Titolo_campoSmall">Downloaded:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="LBdownloaded" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="Titolo_campoSmall">Visible To:</asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBvisibleTo" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </td>
            </tr>
            <tr id="TRallow" runat="server" visible="false">
                <td valign="top">
                    <asp:Label ID="LBallow_t" runat="server" CssClass="Titolo_campoSmall">Allow:</asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBallow" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                    <div id="DVallowRoles" runat="server" visible="false">
                        <div style="float: left;">
                            <asp:Label ID="LBallowRoles_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="LBallowRoles" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                        </div>
                    </div>
                    <div id="DVallowPersons" runat="server" visible="false">
                        <div style="float: left;">
                            <asp:Label ID="LBallowPersons_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="LBallowPersons" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr id="TRdeny" runat="server" visible="false">
                <td valign="top">
                    <asp:Label ID="LBdeny_t" runat="server" CssClass="Titolo_campoSmall">Deny to:</asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="LBdeny" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                    <div id="DVdenyRoles" runat="server" visible="false">
                        <div style="float: left;">
                            <asp:Label ID="LBdenyRoles_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="LBdenyRoles" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                        </div>
                    </div>
                    <div id="DVdenyPersons" runat="server" visible="false">
                        <div style="float: left;">
                            <asp:Label ID="LBdenyPersons_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="LBdenyPersons" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="VIWerror" runat="server">
        <div style="padding-top: 50px; padding-bottom: 50px;">
            <asp:Label ID="LBitemNotFound" runat="server" CssClass="Testo_campoSmall"></asp:Label>
        </div>
    </asp:View>
</asp:MultiView>