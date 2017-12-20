<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="WizardUrlTokenValidateProfile.aspx.vb" Inherits="Comunita_OnLine.WizardUrlTokenValidateProfile" %>

<%@ Register TagPrefix="CTRL" TagName="SelectType" Src="./UC/UC_StepProfileType.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectOrganizations" Src="./UC/UC_StepOrganizations.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ProfileInfo" Src="./UC/UC_StepProfileInfo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Privacy" Src="./UC/UC_StepPrivacy.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Complete" Src="./UC/UC_StepSummary.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="UnknownProfile" Src="./UC/UC_StepUnknownProfile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalCredentials" Src="./UC/UC_StepInternalCredentials.ascx" %>

<%@ MasterType VirtualPath="~/Authentication.Master" %>


<asp:Content ID="CNTtitle" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="CNThead" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        div#Wizard
        {
            text-align: left;
            width: 950px;
            max-width: 950px;
            border: 1px solid black;
            margin-left: auto;
            margin-right: auto;
            padding: 15px;
            background-color: White;
        }
        div#data_content
        {
            padding-bottom: 3em;
            background-color: White;
        }
        div#data_content .Titolo_Campo
        {
            font-weight: bold;
        }
        
        div.StepButton
        {
            width: 949px;
            text-align: right;
        }
        div.PrivacyBox
        {
            width: 850px;
            height: 150px;
            max-height: 850px;
            height: 150px;
            overflow: auto;
            font-size: smaller;
            border: 1px solid black;
            margin-left: auto;
            margin-right: auto;
        }
        
        div.PrivacyBox ol
        {
            padding-left: 2em;
        }
        div.PrivacyBox ol li
        {
            list-style-type: decimal;
            padding-left: 0em;
        }
        
        div#form.Wizard
        {
            padding: 10px 0px 20px 0px !important;
            padding-bottom: 0px !important;
        }
        
        div.StepData
        {
            border: 1px solid black;
            margin: 5px;
            padding: 5px;
        }
        div.StepData span.Titolo_Campo
        {
            width: 12em;
            display: inline-block;
        }
        div.StepData input.Testo_Campo
        {
            width: 300px;
            display: inline-block;
        }
        div.StepData select.Testo_Campo
        {
            width: 300px;
            display: inline-block;
        }
        div.StepData span.Fieldrow
        {
            display: block;
            padding: 0.2em;
        }
        
        div.StepData span.Fieldrow label.full_row
        {
            width: 100% !important;
            text-align: left !important;
        }
        
        div.StepData span.Fieldrow span.Testo_campo input
        {
            vertical-align: top;
            border: 0px solid transparent !important;
        }
            
        div.StepData span.Fieldrow span.Testo_campo label
        {
            display: inline-block;
            margin-bottom: 0.5em;
            width: 90%;
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
</asp:Content>
<asp:Content ID="CNTmodule" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section Wizard">
        <h2>
            <asp:Literal ID="LTtitleWizardInternal" runat="server">Registrazione al sistema</asp:Literal></h2>
    </div>
    <div id="data_content">
        <div id="Wizard">
            <div class="StepButton">
                <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                <asp:Button ID="BTNcompleteTop" runat="server" Text="Next" Visible="false" />
            </div>
            <div class="StepContent">
                <div class="TopDescription">
                    <h1>
                        <asp:Label ID="LBstepTitle" runat="server" CssClass="Titolo_Campo"></asp:Label>
                    </h1>
                    <asp:Label ID="LBuserInfo" class="Titolo_Campo" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                </div>
                <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">
                    <asp:View id="VIWunknownProfile" runat="server">
                        <CTRL:UnknownProfile ID="CTRLunknownProfile" runat="server" />
                    </asp:View>
                     <asp:View id="VIWinternalCredentials" runat="server">
                        <CTRL:InternalCredentials ID="CTRLinternalCredentials" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWorganization" runat="server">
                        <ctrl:selectorganizations id="CTRLorganizations" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWprofileTypes" runat="server">
                        <ctrl:selecttype id="CTRLprofileTypes" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWuserInfo" runat="server">
                        <ctrl:profileinfo id="CTRLprofileInfo" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWprivacy" runat="server">
                        <ctrl:privacy id="CTRLprivacy" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWcomplete" runat="server">
                        <ctrl:complete id="CTRLsummary" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWprofileError" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Literal ID="LTerrors" runat="server"></asp:Literal>
                        <br />
                        <br />
                        <br />
                        <br />
                    </asp:View>
                     <asp:View ID="VIWwaitingMessage" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Literal ID="LTwaitingMessage" runat="server"></asp:Literal>
                        <asp:Image ID="IMGloading" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
                        <br />
                        <br />
                        <br />
                    </asp:View>
                </asp:MultiView>
            </div>
            <div class="StepButton">
                <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
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
                Loading...<br />
                <asp:Image ID="Image1" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
            </div>
            </div>
</asp:Content>