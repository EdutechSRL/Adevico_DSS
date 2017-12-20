<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="LibreriaDomandeSelect.aspx.vb" Inherits="Comunita_OnLine.LibreriaDomandeSelect" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBGestioneQuestionario" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
        <asp:LinkButton ID="LNBGestioneDomande" Visible="true" runat="server" CssClass="Link_Menu">
        </asp:LinkButton>
    </asp:Panel>
    <br />
    <br />
    <asp:Label runat="server" ID="LBSelezionaLibreria"></asp:Label><br />
    <br />
    <asp:DropDownList runat="server" ID="DDLSelectLibreria" DataTextField="nome" DataValueField="id">
    </asp:DropDownList>
    <asp:LinkButton runat="server" ID="LNBSelezionaLibreria" CssClass="Link_Menu" />
    <br />
    <br />
    <asp:Label runat="server" ID="LBSelezionaDomande"></asp:Label>
    <br />
    <br />
    <asp:DataList ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4" ForeColor="#333333"
        Width="100%">
        <ItemTemplate>
            <b>
                <%#Eval("nomePagina")%>
                 <asp:Literal ID="LTidPage" runat="server" Text='<%#Container.DataItem.Id %>' Visible="false"></asp:Literal>
            </b>
            <br />
            <%#Eval("descrizione")%>
            <br />
            <hr />
            <asp:DataList ID="DLDomande" runat="server" OnItemDataBound="loadDomandeOpzioni"
                Width="100%" DataKeyField="id" OnItemCommand="DLDomandeEditCommand">
                <ItemTemplate>
                    <asp:Literal ID="LTidQuestion" runat="server" Text='<%#Container.DataItem.Id %>' Visible="false"></asp:Literal>
                    <asp:Literal ID="LTidPage" runat="server" Text='<%#Container.DataItem.idPagina %>' Visible="false"></asp:Literal>
                    <div style="text-align: right;">
                        (Cod.<%#Eval("id")%>) Diff.<%#Eval("difficolta")%><br />
                        <asp:Label runat="server" ID="LBSelezionaDomanda"></asp:Label><asp:CheckBox runat="server"
                            ID="CHKSelect" />
                    </div>
                    <br />
                    <%#Eval("numero")%>
                    .
                    <%#me.SmartTagsAvailable.TagAll(Eval("testo"))%>
                    <br />
                    <br />
                    <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                    <br />
                    <br />
                </ItemTemplate>
                <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <AlternatingItemStyle BackColor="WHITE" />
                <ItemStyle BackColor="WHITE" />
                <HeaderStyle BackColor="#EFF3FB" Font-Bold="True" ForeColor="White" />
            </asp:DataList>
        </ItemTemplate>
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        <AlternatingItemStyle BackColor="#E3EAEB" />
        <ItemStyle BackColor="#E3EAEB" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
    </asp:DataList>
    <br />
    <asp:LinkButton runat="server" ID="LNBConferma" CssClass="Link_Menu" />
</asp:Content>
