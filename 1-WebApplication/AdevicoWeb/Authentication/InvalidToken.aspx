<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="InvalidToken.aspx.vb" Inherits="Comunita_OnLine.InvalidToken" %>
<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <asp:Literal ID="LTredirect" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
     <asp:Literal ID="LTexternalWebLogon" runat="server" Visible="false"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
     <div id="form" class="section">
        <h2>
            <asp:Literal ID="LTtitleInvalidToken" runat="server">
                Accesso automatico
            </asp:Literal>
        </h2>

        <div id="submit-feedback" >
            <span class="" runat="server" id="SPNmessages">
                <asp:Literal ID="LTinvalidToken" runat="server">
                    Il suo account risulta disabilitato.
                    <br />
                </asp:Literal>
                <br />
                <asp:Literal ID="LTtokenSupportInfo" runat="server">
                </asp:Literal>
                <br />
                <asp:Literal ID="LTtokenUrl" runat="server">
                </asp:Literal>
            </span>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>
