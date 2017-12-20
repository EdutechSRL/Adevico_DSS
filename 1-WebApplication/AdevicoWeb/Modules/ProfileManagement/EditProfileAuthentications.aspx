<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditProfileAuthentications.aspx.vb" Inherits="Comunita_OnLine.EditProfileAuthentications" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Authentications" Src="./UC/UC_ProfileAuthenticationProviders.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201602221000lm" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;"
        runat="server">
        <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
             <asp:HyperLink ID="HYPbackToEdit" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:LinkButton ID="LNBaddNewProvider" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
    </div>
     <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;">
        <asp:MultiView ID="MLVprofiles" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWauthentications" runat="server">
                <CTRL:Authentications id="CTRLauthentications" runat="server"></CTRL:Authentications>
            </asp:View>
            <asp:View ID="VIWmessage" runat="server">
                <br /><br /><br /><br /><br />
                <asp:Label ID="LBmessage" runat="server"></asp:Label>
                <br /><br /><br /><br /><br />
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>