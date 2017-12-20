<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.Master"
    CodeBehind="WizardInternalProfile.aspx.vb" Inherits="Comunita_OnLine.WizardInternalProfile" %>

<%@ Register TagPrefix="CTRL" TagName="Disclaimer" Src="./UC/UC_StepDisclaimer.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectType" Src="./UC/UC_StepProfileType.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectOrganizations" Src="./UC/UC_StepOrganizations.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ProfileInfo" Src="./UC/UC_StepProfileInfo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Privacy" Src="./UC/UC_StepPrivacy.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Complete" Src="./UC/UC_StepSummary.ascx" %>


<%@ MasterType VirtualPath="~/Authentication.Master" %>
<asp:Content ID="CNTtitle" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="CNThead" ContentPlaceHolderID="HeadContent" runat="server">
  <style type="text/css">
        #progressBackgroundFilter
        {
            position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            overflow: hidden;
            padding: 0;
            margin: 0;
            background-color: #000;
            filter: alpha(opacity=50);
            opacity: 0.5;
            z-index: 1000;
        }
        
        #processMessage
        {
            position: fixed;
            top: 30%;
            left: 43%;
            padding: 10px;
            width: 14%;
            z-index: 1001;
            background-color: #fff;
            border: solid 1px #000;
        }
    </style>

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
                        url: "../Modules/Common/AutoComplete.asmx/AgencyNames",
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
                            alert(textStatus);
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
</asp:Content>
<asp:Content ID="CNTmenu" ContentPlaceHolderID="CPHmenu" runat="server">
    <li>
        <asp:HyperLink ID="HYPinternalPage" CssClass="menu-back" runat="server">Torna alla home</asp:HyperLink>
    </li>
</asp:Content>
<asp:Content ID="CNTmodule" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section Wizard">
        <h2><asp:Literal ID="LTtitleWizardInternal" runat="server">Registrazione al sistema</asp:Literal></h2>
    </div>
    <div id="data_content">
        <div id="Wizard">
            <div class="StepButton">
                <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                <asp:Button ID="BTNcompleteTop" runat="server" Text="Next" Visible="false"/>
            </div>
            <div class="StepContent">
                <div class="TopDescription">
                    <h1>
                        <asp:Label ID="LBstepTitle" runat="server" CssClass="Titolo_Campo"></asp:Label>
                    </h1>
                    <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                </div>
                <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWdisclaimer" runat="server">
                        <CTRL:Disclaimer ID="CTRLdisclaimer" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWorganization" runat="server">
                        <CTRL:SelectOrganizations ID="CTRLorganizations" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWprofileTypes" runat="server">
                        <CTRL:SelectType id="CTRLprofileTypes" runat="server"/>
                    </asp:View>
                    <asp:View ID="VIWuserInfo" runat="server">
                        <CTRL:ProfileInfo ID="CTRLprofileInfo" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWprivacy" runat="server">
                        <CTRL:Privacy ID="CTRLprivacy" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWcomplete" runat="server">
                        <CTRL:Complete ID="CTRLsummary" runat="server"/>
                    </asp:View>
                     <asp:View ID="VIWprofileError" runat="server">
                        <br /><br /><br /><br />
                        <asp:Literal ID="LTerrors" runat="server"></asp:Literal>
                        <br /><br /><br /><br />
                    </asp:View>
                </asp:MultiView>
            </div>
            <div class="StepButton">
                <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false"/>
                <asp:Button ID="BTNnextBottom" runat="server" Text="Next"  CausesValidation="true" />
                <asp:Button ID="BTNcompleteBottom" runat="server" Text="Next" Visible="false" />
            </div>
        </div>
    </div>
     <div id="DVupdate" style="display:none">
     <div id="progressBackgroundFilter">
            </div>
            <div id="processMessage">
                <%-- <div id="imgdivLoading" align="center" valign="middle" runat="server" style="border-style: dotted;
                            padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;
                            border-color: #000066 black black black; border-width: medium; background-color: Gray; width: 900px;">--%>
                <asp:Literal ID="LTwaitingLogon" runat="server"></asp:Literal><br />
                <asp:Image ID="Image1" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
            </div>
            </div>
</asp:Content>