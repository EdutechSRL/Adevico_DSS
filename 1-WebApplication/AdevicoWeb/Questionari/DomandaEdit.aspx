<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="DomandaEdit.aspx.vb" Inherits="Comunita_OnLine.DomandaEdit" EnableEventValidation="false"
    ValidateRequest="false" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>--%>
    <div align="center">
        <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
            <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
            <asp:LinkButton ID="LNBGestioneDomande" Visible="true" runat="server" CssClass="Link_Menu">
            </asp:LinkButton>
            <asp:LinkButton ID="LNBAggiungiLibreria" Visible="false" runat="server" CssClass="Link_Menu">
            </asp:LinkButton>
            <asp:LinkButton ID="LNBSalva" Visible="true" runat="server" CssClass="Link_Menu">
            </asp:LinkButton>
        </asp:Panel>
        <br />
        <br />
        <asp:MultiView runat="server" ID="MLVquestionari">
            <asp:View ID="VIWdati" runat="server">
               <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                <center>
                    <asp:PlaceHolder runat="server" ID="PHOpzioni"></asp:PlaceHolder>
                </center>
                <br />
                &nbsp;<asp:Label ID="LBConferma" runat="server" Visible="false"></asp:Label>
                <asp:Panel runat="server" ID="PNLSelectLibreria" Visible="false">
                    <br />
                    <asp:Label ID="LBMessage" runat="server"></asp:Label>
                    <asp:DropDownList ID="DDLLibrerie" runat="server" DataTextField="nome" DataValueField="idQuestionarioMultilingua">
                    </asp:DropDownList>
                    <asp:Button ID="BTNConferma" runat="server" Text="" />
                </asp:Panel>
                <asp:LinkButton ID="LNBSalva2" Visible="true" runat="server" CssClass="Link_Menu">
                </asp:LinkButton>
            </asp:View>
            <asp:View runat="server" ID="VIWSceltaTipoDomanda">
                <asp:Label runat="server" ID="LBSceltaDomanda" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                <hr color="#00008B" />
                <table runat="server" id="TBLSceltaDomanda">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="LBSceltaMultipla" Font-Bold="true"></asp:Label><br />
                            <br />
                            <asp:LinkButton runat="server" ID="LNBMultiplaWIZ" CssClass="Link_Menu"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="LNBMultiplaStandard" CssClass="Link_Menu"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Image runat="server" ID="IMBSceltaMultipla" ImageUrl="img/sceltamultipla.png"
                                BorderWidth="1" BorderColor="Black" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="LBTestoLibero" Font-Bold="true"></asp:Label><br />
                            <br />
                            <asp:LinkButton runat="server" ID="LNBTestoLiberoWIZ" CssClass="Link_Menu"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="LNBTestoLiberoStandard" CssClass="Link_Menu"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Image runat="server" ID="IMBTestoLibero" ImageUrl="img/testolibero.png" BorderWidth="1"
                                BorderColor="Black" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="LBNumerica" Font-Bold="true"></asp:Label><br />
                            <br />
                            <asp:LinkButton runat="server" ID="LNBNumericaWIZ" CssClass="Link_Menu"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="LNBNumericaStandard" CssClass="Link_Menu"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Image runat="server" ID="IMBNumerica" ImageUrl="img/numerica.png" BorderWidth="1"
                                BorderColor="Black" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="LBRating" Font-Bold="true"></asp:Label><br />
                            <br />
                            <asp:LinkButton runat="server" ID="LNBRatingWIZ" CssClass="Link_Menu"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="LNBRatingStandard" CssClass="Link_Menu"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Image runat="server" ID="IMGRating" ImageUrl="img/rating.png" BorderWidth="1"
                                BorderColor="Black" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="LBRatingStars" Font-Bold="true"></asp:Label><br />
                            <br />
                            <asp:LinkButton runat="server" ID="LNBRatingStarsWIZ" CssClass="Link_Menu"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="LNBRatingStarsStandard" CssClass="Link_Menu"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Image runat="server" ID="IMGRatingStar" ImageUrl="img/ratingstars.png" BorderWidth="1"
                                BorderColor="Black" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="LBDropDown" Font-Bold="true"></asp:Label><br />
                            <br />
                            <asp:LinkButton runat="server" ID="LNBDropDownWIZ" CssClass="Link_Menu"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="LNBDropDownStandard" CssClass="Link_Menu"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:ImageButton runat="server" ID="IMBDropDown" ImageUrl="img/dropdown.png" BorderWidth="1"
                                BorderColor="Black" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View runat="server" ID="VIWmessaggi">
                <asp:Label ID="LBerrore" runat="server"></asp:Label>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
