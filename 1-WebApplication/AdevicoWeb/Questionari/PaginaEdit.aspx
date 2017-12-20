<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="PaginaEdit.aspx.vb" Inherits="Comunita_OnLine.PaginaEdit" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
        <asp:LinkButton ID="LNBGestioneDomande" Visible="true" runat="server" CssClass="Link_Menu">
        </asp:LinkButton>
        <asp:LinkButton ID="LNBSalva" Visible="true" runat="server" CssClass="Link_Menu">
        </asp:LinkButton>
    </asp:Panel>
    <br />
    <br />
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <asp:FormView ID="FRVPagina" runat="server" CellPadding="4" ForeColor="#333333">
                <ItemTemplate>
                    <asp:Label ID="LBNomePagina" runat="server" Text=""></asp:Label><br />
                    <asp:TextBox ID="TXBNomePagina" runat="server" Text='<%#Eval("nomePagina")%>' Width="300px"></asp:TextBox><br />
                    <asp:Label ID="LBDescrizione" runat="server" Text=""></asp:Label><br />
                    <asp:TextBox ID="TXBDescrizione" runat="server" Text='<%#Eval("descrizione")%>' TextMode="MultiLine"
                        Width="500px"></asp:TextBox>
                    <!-- <asp:CheckBox ID="CBRandomOrdine" runat="server" Text="Random Ordine Domande" Checked='<%#DataBinder.Eval(Container, "DataItem.randomOrdineDomande") %>'/>-->
                    <br />
                    <br />
                </ItemTemplate>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <RowStyle BackColor="#EFF3FB" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            </asp:FormView>
            <asp:Button runat="server" ID="BTNContinua" />
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
