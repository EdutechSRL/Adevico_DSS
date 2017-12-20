<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="Edit.aspx.vb" Inherits="Comunita_OnLine.EditAuthenticationProvider"  ValidateRequest="false" MaintainScrollPositionOnPostback="true"%>
<%@ Register TagPrefix="CTRL" TagName="AuthenticationProvider" Src="UC/UC_AuthenticationProvider.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Translation" Src="UC/UC_AuthenticationProviderTranslation.ascx" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProviderManagement/Css/ProviderManagement.css" type="text/css" rel="stylesheet" />
    <link href="../../Graphics/Template/Wizard/css/wizard.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/ProviderManagement/ProviderManagement.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPbackToManagementTop" runat="server" CssClass="Link_Menu" Text="Back to management"
            Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
         <asp:HyperLink ID="HYPadvancedSettings" runat="server" Visible="false" CssClass="Link_Menu"></asp:HyperLink>
        <asp:LinkButton ID="LNBsaveTop" runat="server" CssClass="Link_Menu" Visible="false"></asp:LinkButton>
    </div>
    <asp:MultiView ID ="MLVdata" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWempty" runat="server">
              <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="LBmessage" runat="server"></asp:Label>
                <br />
                <br />
                <br />
                <br />
                <br />
        </asp:View>
        <asp:View ID="VIWproviderData" runat="server">
            <CTRL:AuthenticationProvider ID="CTRLauthenticationProvider" runat="server"  />
            <div id="DVlanguages">
                <telerik:RadTabStrip ID="TBSlanguages" runat="server"  Align="Justify"
                    Skin="Outlook" AutoPostBack="true"  EnableEmbeddedSkins="true" CssClass="InfoTab">
                    <Tabs>

                    </Tabs>
                </telerik:RadTabStrip>

                <CTRL:Translation ID="CTRLtranslation" runat="server" />
            </div>
            
        </asp:View>
    </asp:MultiView>
</asp:Content>