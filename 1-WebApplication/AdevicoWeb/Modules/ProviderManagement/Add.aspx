<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="Add.aspx.vb" Inherits="Comunita_OnLine.AddAuthenticationProvider" ValidateRequest="false" %>

<%@ Register TagPrefix="CTRL" TagName="AuthenticationProvider" Src="UC/UC_AuthenticationProvider.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Translation" Src="UC/UC_AuthenticationProviderTranslation.ascx" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

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
    <div style="text-align:right; height:20px; ">
         <asp:HyperLink ID="HYPbackToManagementTop" runat="server" CssClass="Link_Menu" Text="Back to management"
            Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
    </div>
    <div id="middle-content">
        <div id="data_content">
            <div id="wizard">
                <div class="wiz_header">
                    <div class="wiz_top_nav">
                        <div class="stepButton">
                            <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                            <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                            <asp:Button ID="BTNcompleteTop" runat="server" Text="Next" Visible="false" />
                        </div>
                    </div>
                    <div class="wiz_top_info ">
                        <div class="wiz_top_desc clearfix">
                            <h2>
                                <asp:Label ID="LBstepTitle" runat="server" CssClass="Titolo_Campo"></asp:Label>
                            </h2>
                            <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                        </div>
                    </div>

                </div>
                <div class="wiz_content">
                    <div class="StepData">
                        <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">
                            <asp:View ID="VIWselectProviderType" runat="server">
                                <span class="Field_Row rbl_field_row">
                                    <asp:Label ID="LBselectProviderType_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="RBLproviderTypes">Provider Types:</asp:Label>
                                    <asp:RadioButtonList ID="RBLproviderTypes" runat="server" CssClass="Testo_Campo rbl_vertical"  RepeatLayout="Flow" RepeatDirection="Vertical" ></asp:RadioButtonList>
                                </span>
                            
                            </asp:View>
                            <asp:View ID="VIWproviderData" runat="server">
                                <CTRL:AuthenticationProvider ID="CTRLauthenticationProvider" runat="server"  />
                            </asp:View>
                            <asp:View ID="VIWtranslation" runat="server">
                               <CTRL:Translation ID="CTRLtranslation" runat="server" />
                            </asp:View>
                            <asp:View ID="VIWsummary" runat="server">
                                <br /><br /><br />
                                <asp:Label ID="LBsummary" runat="server"></asp:Label>
                                <br /><br /><br />
                            </asp:View>
                            <asp:View ID="VIWproviderError" runat="server">
                                <span class="LIT_Error">
                                <asp:Literal ID="LTerrors" runat="server"></asp:Literal>
                                </span>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
                <div class="wiz_bot_nav clearfix">
                    <div class="stepButton">
                        <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                        <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
                        <asp:Button ID="BTNcompleteBottom" runat="server" Text="Next" Visible="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>