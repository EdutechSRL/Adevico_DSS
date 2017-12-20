<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ImportUsersFromCSV.aspx.vb" Inherits="Comunita_OnLine.ImportUsersFromCSV" %>
<%@ Register TagPrefix="CTRL" TagName="CSVselector" Src="~/Modules/Common/UC/UC_GenericCSVuploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FieldsMatcher" Src="~/Modules/Common/UC/UC_GenericFieldsMatcher.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ItemsSelector" Src="~/Modules/Common/UC/UC_GenericItemsSelector.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ register tagprefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<link href="./../Style/Common/Wizard.css" type="text/css" rel="stylesheet" />--%>
    <link href="./../Graphics/Template/Wizard/css/wizard.css?v=201604071200lm" type="text/css" rel="stylesheet" />
    <style type="text/css" media="all">
        div#wizard div.StepData div.IMsourceCSV .Titolo_campo { width: 210px; padding-bottom: 1em; display: inline-block; }
    
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="middle-content">
        <div id="data_content">

            <div class="button">
                <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
            </div>

            <asp:MultiView ID="MLVcsvImport" runat="server">

                <asp:View ID="VIWempty" runat="server">
                    <br /><br /><br /><br />
                    <asp:Label ID="LBnoPermissionToAdd" runat="server"></asp:Label>
                    <br /><br /><br /><br />
                </asp:View>

                <asp:View ID="VIWwizard" runat="server">

                    <div id="wizard">
                        <div class="wiz_header">

                            <div class="wiz_top_nav"><div class="stepButton">
                                <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                                <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                                <asp:Button ID="BTNcompleteTop" cssClass="submitBlock" runat="server" Text="Next" Visible="false" />
                            </div></div>

                            <div class="wiz_top_info clearfix">

                                <div class="wiz_top_desc clearfix">
                                    <h2>
                                        <asp:Label ID="LBstepTitle" runat="server" CssClass="Titolo_Campo"></asp:Label>
                                    </h2>
                                    <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                                </div>

                                <%--<div class="wiz_export"></div>--%>
                            </div>

                        </div>

                        <div class="wiz_content">
                            <div class="StepData">
                            
                                <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">

                                    <asp:View ID="VIWsourceCSV" runat="server">
                                        <div class="IMsourceCSV">
                                            <ctrl:CSVselector id="CTRLcsvSelector" runat="server" ClearPreviousFiles="True"></ctrl:CSVselector>
                                        </div>
                                    </asp:View>

                                    <asp:View ID="VIWfieldsMatcher" runat="server">
                                        <div class="IMfieldsMatcher">
                                            <ctrl:FieldsMatcher id="CTRLfieldsMatcher" runat="server"></ctrl:FieldsMatcher>
                                        </div>
                                    </asp:View>
                                
                                    <asp:View ID="VIWitemsSelector" runat="server">
                                         <div class="IMitemSelector">
                                            <ctrl:ItemsSelector id="CTRLitemsSelector" runat="server"></ctrl:ItemsSelector>
                                         </div>
                                    </asp:View>
                                
                                    <asp:View ID="VIWsummary" runat="server">
                                        <div class="IMSummary">
                                            <div runat="server" id="DVsummary" class="summary">
                                                <asp:Label ID="LBsummary" runat="server" CssClass="Testo_Campo"></asp:Label> 
                                            </div>
                                            <div runat="server" id="DVimportUsers" style="display: inline-block;" class="importUsers">
                                                <telerik:RadProgressManager id="RPMimportUsers" runat="server" />
                                                <telerik:RadProgressArea id="RPAimportUsers" runat="server"  CssClass="progressTelerik" />  
                                            </div>
                                        </div>
                                    </asp:View>

                                    <asp:View ID="VIWcomplete" runat="server">
                                        <div class="IMComplete">
                                            <span class="Fieldrow">
                                                <asp:Label ID="LBcompleteInfo" runat="server" CssClass="Testo_Campo"></asp:Label>
                                            </span>
                                            <span class="Fieldrow" id="SPNusers" runat="server" visible="false">
                                                <asp:Label ID="LBinvitedUsersErrors_t" runat="server" CssClass="Titolo_Campo">Utenti non importati:</asp:Label>
                                                <asp:Label runat="server" ID="LBusers" CssClass="Testo_Campo"></asp:Label>
                                            </span>
                                        </div>
                                    </asp:View>

                                    <asp:View ID="VIWerror" runat="server">
                                        <div class="IMError">
                                            <span class="LIT_Error">
                                                <asp:Literal ID="LTerrors" runat="server"></asp:Literal>
                                            </span>
                                        </div>
                                    </asp:View>

                            </asp:MultiView>
                            </div>
                        </div>

                        <div class="wiz_bot_nav clearfix"><div class="StepButton">
                                <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                                <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
                                <asp:Button ID="BTNcompleteBottom" cssClass="submitBlock" runat="server" Text="Next" Visible="false"  />
                        </div></div>
                    </div>

            </asp:View>
            </asp:MultiView>

        </div>
    </div>
</asp:Content>