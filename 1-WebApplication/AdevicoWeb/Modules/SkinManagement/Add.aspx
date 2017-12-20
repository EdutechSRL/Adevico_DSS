<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="Add.aspx.vb" Inherits="Comunita_OnLine.AddSkin" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .DIV_MP_Content .DVmenu {
            text-align: right;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPback" runat="server" CssClass="Link_Menu" Text="#Back" />
        <asp:Button ID="BTNaddSkin" runat="server" />
    </div>
    <asp:MultiView ID="MLVaddSkin" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBskinMessage" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWdata" runat="server">
            <br /><br /><br />
            <div>
                <asp:Label ID="LBdisplayInfo" runat="server"></asp:Label><br /><br /><br />
            </div>
            <div>
                <asp:Label ID="LBskinName_t" runat="server" CssClass="Titolo_Campo">#Nome:</asp:Label>
                <asp:TextBox ID="TXBskinName" runat="server" CssClass="Testo_Campo" Columns="100" MaxLength="255"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVskinName" ControlToValidate="TXBskinName" runat="server"
                    SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
