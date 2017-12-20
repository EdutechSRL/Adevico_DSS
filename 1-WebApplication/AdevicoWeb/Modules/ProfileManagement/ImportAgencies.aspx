<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ImportAgencies.aspx.vb" Inherits="Comunita_OnLine.ImportAgencies" %>
<%@ Register TagPrefix="CTRL" TagName="CSVselector" Src="~/Modules/Common/UC/UC_GenericCSVuploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FieldsMatcher" Src="~/Modules/Common/UC/UC_GenericFieldsMatcher.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ItemsSelector" Src="~/Modules/Common/UC/UC_GenericItemsSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="organizationsSelector" Src="~/Modules/ProfileManagement/UC/UC_IMagencyOrganizations.ascx" %>
<%@ register tagprefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Template/Wizard/css/wizard.css?v=201604071200lm" type="text/css" rel="stylesheet" />
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201604071200lm" rel="Stylesheet" type="text/css" />
    <link href="ImportProfile.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
        $('form').submit(function () {
            $('.submitBlock').attr('disabled', 'disabled');
            $('.submitBlock').css('border', '3px solid red');
            // On submit disable its submit button
            // $('input[type=submit]', this).attr('disabled', 'disabled');
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
   <div id="middle-content">
        <div id="data_content">
            <div class="button">
                <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back" CausesValidation="false"></asp:HyperLink>
            </div>
            <asp:MultiView ID="MLVimport" runat="server" ActiveViewIndex="1">
                <asp:View ID="VIWdefault" runat="server">
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
                <asp:View ID="VIWwizard" runat="server">
                    <div id="wizard">
                        <div class="wiz_header">
                            <div class="wiz_top_nav">
                                <div class="stepButton">
                                    <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                                    <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                                    <asp:Button ID="BTNcompleteTop" cssClass="submitBlock" runat="server" Text="Next" Visible="false" />
                                </div>
                            </div>
                            <div class="wiz_top_info ">
                                <div class="wiz_top_desc clearfix">
                                    <h2>
                                        <asp:Label ID="LBstepTitle" runat="server"></asp:Label>
                                    </h2>
                                
                                    <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="wiz_content">
                            <div class="StepData">
                                <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="VIWsourceCSV" runat="server">
                                            <ctrl:CSVselector id="CTRLcsvSelector" runat="server" ClearPreviousFiles="True"></ctrl:CSVselector>
                                        </asp:View>
                                        <asp:View ID="VIWfieldsMatcher" runat="server">
                                           <ctrl:FieldsMatcher id="CTRLfieldsMatcher" runat="server"></ctrl:FieldsMatcher>
                                        </asp:View>
                                        <asp:View ID="VIWitemsSelector" runat="server">
                                            <ctrl:ItemsSelector id="CTRLitemsSelector" runat="server"></ctrl:ItemsSelector>
                                        </asp:View>
                                        <asp:View ID="VIWorganizationAvailability" runat="server">
                                           <CTRL:organizationsSelector ID="CTRLorganizations" runat="server"></CTRL:organizationsSelector> 
                                        </asp:View>
                                        <asp:View ID="VIWsummary" runat="server">
                                            <div class="IMSummary">
                                                <div runat="server" id="DVsummary" class="summary">
                                                    <span class="Fieldrow">
                                                        <asp:Label ID="LBsummaryAgency_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBsummaryAgency"></asp:Label>
                                                        <asp:Label ID="LBsummaryAgency" runat="server" CssClass="Testo_Campo"></asp:Label>
                                                    </span>
                                                    <span class="Fieldrow">
                                                        <asp:Label ID="LBsummaryOrganization_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBsummaryOrganization"></asp:Label>
                                                        <asp:Label ID="LBsummaryOrganization" runat="server" CssClass="Testo_Campo"></asp:Label>
                                                    </span>
                                                    
                                                </div>
                                                <div runat="server" id="DVimportAgencies" style="display: inline-block;" class="importProfiles">
                                                    <telerik:RadProgressManager id="RPMimportAgencies" runat="server" />
                                                    <telerik:RadProgressArea id="RPAimportAgencies" runat="server"  CssClass="progressTelerik" />  
                                                </div>
                                            </div>
                                        </asp:View>
                                        <asp:View ID="VIWcomplete" runat="server">
                                            <div class="IMComplete">
                                                <span class="Fieldrow">
                                                    <asp:Label ID="LBcompleteInfo" runat="server" CssClass="Testo_Campo"></asp:Label>
                                                </span>
                                                <span class="Fieldrow" id="SPNagencies" runat="server" visible="false">
                                                    <asp:Label ID="LBimportedAgenciesErrors_t" runat="server" CssClass="Titolo_Campo">Enti non importati:</asp:Label>
                                                    <asp:Label runat="server" ID="LBimportedAgencies" CssClass="Testo_Campo"></asp:Label>
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
                        <div class="wiz_bot_nav clearfix"><div class="stepButton">
                            <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                            <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
                            <asp:Button ID="BTNcompleteBottom" cssClass="submitBlock" runat="server" Text="Next" Visible="false"  />
                        </div></div>

                    </div>
                </asp:View>
            </asp:MultiView>

        </div>
        <div id="DVprogress" style="display: none;">
            <div id="progressBackgroundFilter">
            </div>
            <div id="processMessage">
                <asp:Literal ID="LTprogress" runat="server" />
                <br />
                <asp:Image ID="imgLoading" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
            </div>
        </div>
    </div>
</asp:Content>