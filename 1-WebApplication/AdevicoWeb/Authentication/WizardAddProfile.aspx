<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardAddProfile.aspx.vb" Inherits="Comunita_OnLine.WizardAddProfile" %>

<%@ Register TagPrefix="CTRL" TagName="Authentication" Src="./UC/UC_StepAuthentication.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectType" Src="./UC/UC_StepProfileType.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectOrganizations" Src="./UC/UC_StepOrganizations.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ProfileInfo" Src="./UC/UC_StepProfileInfo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Complete" Src="./UC/UC_StepSummary.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<link href="./../Style/Common/Wizard.css" type="text/css" rel="stylesheet" />--%>
    <link href="./../Graphics/Template/Wizard/css/wizard.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        div#Wizard div.StepButton { padding-top: 1em; }
        
        /* div.StepData */
        div#Wizard div.StepContent div.StepData .FlotLeft,
        div#Wizard div.StepContent div.StepData .LeftCol { width: auto; display: inline-block; margin-top: 1.2em; vertical-align: top;}
        
        /* Step 1 - Profilo */
        div#Wizard div.IW_Profile .FieldRow .Testo_Campo { display: inline-block; padding-left: 1.5em;}
        
        /* Step 2 */
        div#Wizard div.IW_Orgn div.clear .Titolo_campoSmall { width: 200px; display: inline-block; }
        div#Wizard div.IW_Orgn div.clear .testo_campoSmall {}
        
        /* Step 3 */
        
        /* Step 4 */
        div#Wizard div.IW_ProfileInfo .Fieldrow { padding-top: 1.2em; }
        div#Wizard div.IW_ProfileInfo .Titolo_Campo { width: 200px; }
        
        /* Step 5 */
        div#Wizard div.IW_Summary .Fieldrow{ padding-top: 1em; }
        div#Wizard div.IW_Summary .Titolo_Campo { width: 150px; }
        
        /* Error - Da testare */
        div#Wizard .LIT_Error { padding-top: 100px; color: #f00; }
        
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
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="middle-content">
        <div id="data_content">
            <div class="button">
                <asp:HyperLink ID="HYPmanage" runat="server" Text="Torna alla lista" CssClass="Link_Menu" Visible="true"></asp:HyperLink>        
            </div>

            <div id="Wizard">
                <div class="wiz_header">

                    <div class="wiz_top_nav"><div class="stepButton">
                        <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                        <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                        <asp:Button ID="BTNcompleteTop" runat="server" Text="Next" Visible="false" />
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
                            <asp:View ID="VIWprofileTypes" runat="server">
                                <div class="IW_Profile">
                                    <CTRL:SelectType ID="CTRLprofileTypes" runat="server" />
                                </div>
                            </asp:View>
                            
                            <asp:View ID="VIWorganization" runat="server">
                                <div class="IW_Orgn">
                                    <CTRL:SelectOrganizations ID="CTRLorganizations" runat="server" />
                                </div>
                            </asp:View>
                            
                            <asp:View ID="VIWauthenticationTypes" runat="server">
                                <CTRL:Authentication ID="CTRLauthentication" runat="server" />
                            </asp:View>
                            
                            <asp:View ID="VIWuserInfo" runat="server">
                                <div class="IW_ProfileInfo">
                                    <CTRL:ProfileInfo ID="CTRLprofileInfo" runat="server" />
                                </div>
                            </asp:View>
                            
                            <asp:View ID="VIWcomplete" runat="server">
                                <div class="IW_Summary">
                                    <CTRL:Complete ID="CTRLsummary" runat="server" />
                                </div>
                            </asp:View>
                            
                            <asp:View ID="VIWprofileError" runat="server">
                                <div class="LIT_Error">
                                    <asp:Literal ID="LTerrors" runat="server"></asp:Literal>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>

                <div class="wiz_bot_nav clearfix"><div class="stepButton">
                    <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                    <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
                    <asp:Button ID="BTNcompleteBottom" runat="server" Text="Next" Visible="false" />
                </div></div>
                
            </div>
        </div>
    </div>
</asp:Content>
