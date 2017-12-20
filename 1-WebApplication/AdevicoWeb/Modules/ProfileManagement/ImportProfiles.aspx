<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ImportProfiles.aspx.vb" Inherits="Comunita_OnLine.ImportProfiles" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="SourceUserSubmission" Src="./UC/UC_IMsourceUserSubmission.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SourceSelector" Src="./UC/UC_IMsourceSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SourceCSV" Src="./UC/UC_IMsourceCSV.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FieldsMatcher" Src="./UC/UC_IMfieldsMatcher.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ItemsSelector" Src="./UC/UC_IMitemsSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectOrganizations" Src="~/Authentication/UC/UC_StepOrganizations.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectCommunities" Src="~/Modules/Common/UC/UC_SelectCommunities.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectCommunitiesHeader" Src="~/Modules/Common/UC/UC_SelectCommunitiesHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Subscriptions" Src="./UC/UC_CommunityNewSubscriptions.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailTemplate" Src="./UC/UC_IMmailTemplate.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailHeader" Src="~/Modules/Common/UC/UC_MailEditorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Summary" Src="./UC/UC_IMsummary.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Template/Wizard/css/wizard.css?v=201602221000lm" type="text/css" rel="stylesheet" />

    <link href="../Common/UC/UC_MailEditor.css?v=201602221000lm" type="text/css" rel="stylesheet" />
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201602221000lm" rel="Stylesheet" type="text/css" />
    <link href="ImportProfile.css?v=201602221000lm" type="text/css" rel="stylesheet" />

    <%-- <script language="javascript" type="text/javascript">
        function AfterCheckHandler(sender, eventArgs) {
            var tree = $find("=TreeViewClientID ");
            var nodes = tree.get_allNodes();
            var check = false;
            var node = eventArgs.get_node();
            if (node.get_checked()) {
                check = true;
            }
            alert(node);
            alert(node.get_value());
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].get_checked() != check && nodes[i] != node && nodes[i].get_value() == node.get_value()) {
                    (check) ? nodes[i].check() : nodes[i].uncheck();
                }
            }
            return false;
            //tree.UpdateState();
        }</script>--%>
    <script language="javascript" type="text/javascript">
        $('form').submit(function () {
            $('.submitBlock').attr('disabled', 'disabled');
            $('.submitBlock').css('border', '3px solid red');
            // On submit disable its submit button
            // $('input[type=submit]', this).attr('disabled', 'disabled');
        });
    </script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $(".AgencyText").autocomplete({
                autoFocus: true,
                appendTo: "form",
                source: function (request, response) {
                    var dataparameter = $(this.element).siblings(".AgencyValue").attr("data-parameters");
                    if (dataparameter == undefined || dataparameter == "") {
                        dataparameter = "";
                    } else {
                        dataparameter = "," + dataparameter;
                    }

                    $.ajax({
                        url: "../Common/AutoComplete.asmx/AgencySystemNames",
                        data: "{ 'filter': '" + request.term + "'" + dataparameter + "}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.Name,
                                    id: item.Id
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {

                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    $(this).siblings(".AgencyValue").val(ui.item.id);
                },
                change: function (event, ui) {
                    if (!$(this).hasClass("permitNew") && ui.item == null) {
                        $(this).siblings(".AgencyValue").val("");
                        $(this).val("");
                    }
                },
                close: function (event, ui) {

                }
            }).keydown(function () {
                $(this).siblings(".AgencyValue").val("");
            });
        });
    </script>

    <CTRL:SelectCommunitiesHeader ID="CTRLselectCommunitiesHeader" LoadCss="true" LoadScripts="true" runat="server" />
    <CTRL:MailHeader ID="CTRLmailHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="middle-content">
        <div id="data_content">

            <div class="button">
                <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
                    Height="18px" CausesValidation="false"></asp:HyperLink>
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
                                    <asp:Button ID="BTNcompleteTop" CssClass="submitBlock" runat="server" Text="Next" Visible="false" />
                                </div>
                            </div>

                            <div class="wiz_top_info ">
                                <div class="wiz_top_desc clearfix">
                                    <h2>
                                        <asp:Label ID="LBstepTitle" runat="server"></asp:Label>
                                    </h2>

                                    <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                                </div>

                                <%--<div class="wiz_export"></div>--%>
                            </div>
                        </div>

                        <div class="wiz_content">
                            <div class="StepData">
                                <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="VIWselectSource" runat="server">
                                        <CTRL:SourceSelector ID="CTRLsourceSelector" runat="server" CurrentRange="All"></CTRL:SourceSelector>
                                    </asp:View>
                                    <asp:View ID="VIWsourceCSV" runat="server">
                                        <CTRL:SourceCSV ID="CTRLsourceCSV" runat="server" CSVfolder="\UserImport\"></CTRL:SourceCSV>
                                    </asp:View>
                                    <asp:View ID="VIWsourceRFM" runat="server">
                                        <CTRL:SourceUserSubmission ID="CTRLsourceUserSubmission" runat="server"/>
                                    </asp:View>
                                    <asp:View ID="VIWfieldsMatcher" runat="server">
                                        <CTRL:FieldsMatcher ID="CTRLfieldsMatcher" runat="server"></CTRL:FieldsMatcher>
                                    </asp:View>
                                    <asp:View ID="VIWitemsSelector" runat="server">
                                        <CTRL:ItemsSelector ID="CTRLitemsSelector" runat="server"></CTRL:ItemsSelector>
                                    </asp:View>
                                    <asp:View ID="VIWorganizations" runat="server">
                                        <div class="StepData IMSelectOrgn">
                                            <CTRL:SelectOrganizations ID="CTRLorganizations" runat="server"></CTRL:SelectOrganizations>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWselectCommunities" runat="server">
                                        <div class="IMSelectCom">
                                            <CTRL:SelectCommunities ID="CTRLselectCommunities" AdministrationMode="True" runat="server"></CTRL:SelectCommunities>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWmailTemplate" runat="server">
                                        <div class="IMMail">
                                            <CTRL:MailTemplate ID="CTRLmailTemplate" runat="server"></CTRL:MailTemplate>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWsubscriptionsSettings" runat="server">
                                        <div class="IMSubscription">
                                            <br />
                                            <CTRL:Subscriptions ID="CTRLsubscriptions" runat="server"></CTRL:Subscriptions>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWsummary" runat="server">
                                        <div class="IMSummary">
                                            <div runat="server" id="DVsummary" class="summary">
                                                <CTRL:Summary ID="CTRLsummary" runat="server"></CTRL:Summary>
                                            </div>
                                            <div runat="server" id="DVimportProfile" style="display: inline-block;" class="importProfiles">
                                                <telerik:RadProgressManager ID="RPMimportProfiles" runat="server" />
                                                <telerik:RadProgressArea ID="RPAimportProfiles" runat="server" CssClass="progressTelerik" />
                                            </div>
                                        </div>
                                    </asp:View>
                                    <asp:View ID="VIWcomplete" runat="server">
                                        <div class="IMComplete">
                                            <span class="Fieldrow">
                                                <asp:Label ID="LBcompleteInfo" runat="server" CssClass="Testo_Campo"></asp:Label>
                                            </span>
                                            <span class="Fieldrow" id="SPNprofiles" runat="server" visible="false">
                                                <asp:Label ID="LBprofileErrors_t" runat="server" CssClass="Titolo_Campo">Profili non importati:</asp:Label>
                                                <asp:Label runat="server" ID="LBprofiles" CssClass="Testo_Campo"></asp:Label>
                                            </span>
                                            <span class="Fieldrow" id="SPNorganizations" runat="server" visible="false">
                                                <asp:Label ID="LBorganizationErrors_t" runat="server" CssClass="Titolo_Campo">Organizzazioni non associate:</asp:Label>
                                                <asp:Label runat="server" ID="LBorganizations" CssClass="Testo_Campo"></asp:Label>
                                            </span>
                                            <span class="Fieldrow" id="SPNcommunities" runat="server" visible="false">
                                                <asp:Label ID="LBcommunitiesErrors_t" runat="server" CssClass="Titolo_Campo">Iscrizioni non effettuate:</asp:Label>
                                                <asp:Label runat="server" ID="LBcommunities" CssClass="Testo_Campo"></asp:Label>
                                            </span>
                                            <span class="Fieldrow" id="SPNmail" runat="server" visible="false">
                                                <asp:Label ID="LBmailErrors_t" runat="server" CssClass="Titolo_Campo">Mail non inviate a:</asp:Label>
                                                <asp:Label runat="server" ID="LBmail" CssClass="Testo_Campo"></asp:Label>
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

                        <div class="wiz_bot_nav clearfix">
                            <div class="stepButton">
                                <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                                <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
                                <asp:Button ID="BTNcompleteBottom" CssClass="submitBlock" runat="server" Text="Next" Visible="false" />
                            </div>
                        </div>

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
