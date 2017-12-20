<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EditProfile.aspx.vb" Inherits="Comunita_OnLine.EditProfile" %>

<%@ Register TagPrefix="CTRL" TagName="ProfileData" Src="./UC/UC_ProfileData.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
        <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201602221000lm" rel="Stylesheet" type="text/css" />
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
                          url: "../Common/AutoComplete.asmx/AgencyNamesByUser",
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;"
        runat="server">
        <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPtoProfileAuthentications" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:LinkButton ID="LNBsaveTop" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
    </div>
    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;">
        <asp:MultiView ID="MLVprofiles" runat="server" ActiveViewIndex="1">
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
            <asp:View ID="VIWedit" runat="server">
                <asp:Label ID="LBerrors" runat="server"></asp:Label>
                <CTRL:ProfileData id="CTRLprofileData" runat="server"></CTRL:ProfileData>
            </asp:View>
        </asp:MultiView>
    </div>
    <div style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;">
        <asp:LinkButton ID="LNBsaveBottom" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
    </div>
</asp:Content>
