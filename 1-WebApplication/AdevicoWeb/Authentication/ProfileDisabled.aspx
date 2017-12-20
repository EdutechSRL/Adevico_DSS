<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="ProfileDisabled.aspx.vb" Inherits="Comunita_OnLine.ProfileDisabled" %>

<%@ MasterType VirtualPath="~/Authentication.Master" %>

<asp:Content ID="CNTtitle" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="CNThead" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="CNTmenu" ContentPlaceHolderID="CPHmenu" runat="server">
    <asp:Literal ID="LTexternalWebLogon" runat="server" Visible="false"/>
    <asp:Literal ID="LTaccess" runat="server" Visible="false"/>
</asp:Content>
<asp:Content ID="CNTmodule" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section">
        <h2>
            <asp:Literal ID="LTtitleProfileDisabled" runat="server">
                Accesso al sistema non consentito.
            </asp:Literal>
        </h2>

        <div id="submit-feedback" >
            <span class="" runat="server" id="SPNmessages">
                <asp:Literal ID="LTprofileDisabled" runat="server">
                    Il suo account risulta disabilitato.
                    <br />
                </asp:Literal>
                <asp:Literal ID="LTsupportInfo" runat="server">
                </asp:Literal>
            </span>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>