<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="Add.aspx.vb" Inherits="Comunita_OnLine.AddMenuBar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="Menu.Common.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right;" align="center" runat="server">
     <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>&nbsp;
        <asp:LinkButton ID="LNBcreate" runat="server" CssClass="Link_Menu">Create</asp:LinkButton>
    </div>
    <div>
        <asp:Label ID="LBmenubarName" runat="server" CssClass="Titolo_campo">*Name</asp:Label>
        <asp:TextBox ID="TXBname" runat="server" CssClass="Testo_campo" Columns="50"></asp:TextBox>
        <br />
        <asp:Label ID="LBcssClass" runat="server" CssClass="Titolo_campo">*Css class</asp:Label>
        <asp:TextBox ID="TXBcssClass" runat="server" CssClass="Testo_campo" Columns="10"></asp:TextBox>
        <br />
        <asp:Label ID="LBmenubarType" runat="server" CssClass="Titolo_campo">*Type</asp:Label>
        <asp:DropDownList ID="DDLtype" runat="server" CssClass="Testo_campo">
        </asp:DropDownList>
        <br />
        <asp:Label ID="LBdescription" runat="server" CssClass="Titolo_campo">Specificando i campi sottostanti il sistema crea un numero di elementi di primo e di secondo livello pari a quelli indicati:</asp:Label>
        <br />
        <asp:Label ID="LBtopItemsCount_t" runat="server" CssClass="Titolo_campo">Elementi 1° livello (MAX 10): </asp:Label>
        <asp:TextBox ID="TXBtopItemsnumber" CssClass="Testo_campo" MaxLength="10" runat="server" Columns="5"></asp:TextBox>
        <asp:RangeValidator ID="RNVtopItemsNumber" ControlToValidate="TXBtopItemsnumber"
            MinimumValue="0" MaximumValue="10" Type="Integer" runat="server"></asp:RangeValidator>
        <br />
        <asp:Label ID="LBsubItemsCount_t" runat="server" CssClass="Titolo_campo">Elementi 2° livello (MAX 30): </asp:Label>
        <asp:TextBox ID="TXBsubItemsnumber" CssClass="Testo_campo" MaxLength="100" runat="server" Columns="5"></asp:TextBox>
        <asp:RangeValidator ID="RNVsubItemsnumber" ControlToValidate="TXBsubItemsnumber"
            MinimumValue="0" MaximumValue="30" Type="Integer" runat="server"></asp:RangeValidator>
        <br />
    </div>
</asp:Content>
