<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="SkinManagement.aspx.vb" Inherits="Comunita_OnLine.SkinManagement" %>

<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="CtrlSkinList" Src="UC/UC_SkinList.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinEdit" Src="UC/UC_SkinEdit.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="SkinManagement.css" />
    <style type="text/css">
        .RadColorPicker, .Label_Color { float: left; }
        .Label_Color { padding-right: 1em; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:LinkButton ID="LNBaddSkin" runat="server" CssClass="Link_Menu" Text="#AddSkin" CausesValidation="false"></asp:LinkButton>
        <asp:LinkButton ID="LNBbackToList" runat="server" CssClass="Link_Menu" Text="#ShowList" CausesValidation="false"></asp:LinkButton>
        <asp:label id="Lbl_Color_t" runat="server" CssClass="Titolo_Campo Label_Color">Colore sfondo:</asp:label>
        <telerik:RadColorPicker ID="RDPbackgorundColor" runat="server" AutoPostBack="true" 
            ShowIcon="true" EnableCustomColor="True" Preset="Web216" ></telerik:RadColorPicker>
    </div>
    <div id="SkinManagement">
        <asp:Panel ID="PNLbgColor" runat="server">
            <asp:MultiView ID="MLVskins" runat="server">
                <asp:View ID="VIWlist" runat="server">
                    <CTRL:CtrlSkinList runat="server" ID="CTRLskinList" />
                </asp:View>
                <asp:View ID="VIWadd" runat="server">
                    <div class="FullCenter">
                        <br /><br />
                        <asp:Label ID="LBnewSkinName_t" runat="server" CssClass="Titolo_Campo">#Nome nuova skin:</asp:Label>
                        <br />
                        <asp:TextBox ID="TXBnewSkinName" runat="server" CssClass="Testo_Campo" Columns="70" MaxLength="255" ></asp:TextBox>
                        <br /><br />
                        <asp:LinkButton ID="LNBcreateNew" runat="server" CssClass="Link_Menu">#Crea</asp:LinkButton>
                    </div>
                    <div class="SkinNote">
                <fieldset>
                    <legend>
                        <asp:literal ID="LITnoteTitle_t" runat="server">#Note</asp:literal>
                    </legend>
                    <asp:literal ID="LITnoteText_t" runat="server">
                        ##
                        Si ricorda che le skin di ORGANIZZAZIONE e di PORTALE sono MUTUALMENTE ESCLUSIVE.<br />
                        Creando una skin che sarà destinata ad un'ORGANIZZAZIONE è quindi necessario caricare gli stili e le immagini relative agli stessi.<br />
                        Per questo motivo si consiglia di creare una COPIA della skin associata al PORTALE,
                        in modo da avere a disposizione tutti gli elementi di default, senza ulteriori operazioni.

                        Le SKIN associate a COMUNITA', invece, recuperano automaticamente gli elementi non presenti dalla relativa Organizzazione o dal Portale e possono quindi essere omessi.
                    </asp:literal>
                </fieldset>
            </div>
                </asp:View>
                <asp:View ID="VIWedit" runat="server">
                    <CTRL:CtrlSkinEdit ID="CTRLedit" runat="server" />
                </asp:View>
            </asp:MultiView>
        </asp:Panel>
    </div>
</asp:Content>