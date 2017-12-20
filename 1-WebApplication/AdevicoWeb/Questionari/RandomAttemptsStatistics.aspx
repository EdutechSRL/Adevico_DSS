<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="RandomAttemptsStatistics.aspx.vb" Inherits="Comunita_OnLine.RandomAttemptsStatistics" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Statistics" Src="~/Questionari/UserControls/ucUserAttemptsStatistics.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
       <script type="text/javascript" src="../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
      <script type="text/javascript">
          var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
          var CookieName = "<% = Me.CookieName %>";
          var DisplayMessage = "<% = Me.DisplayMessageToken %>";
          var DisplayTitle = "<% = Me.DisplayTitleToken %>";
    </script>
     <script type="text/javascript" language="javascript">
         $(function () {
             $(".ddbuttonlist.enabled").dropdownButtonList();
         });
     </script>
        <script type="text/javascript">
            var fileDownloadCheckTimer;
            function blockUIForDownload() {
                var token = new Date().getTime(); //use the current timestamp as the token value
                $("input[id='" + TokenHiddenFieldId + "']").val(token);
                $.blockUI({ message: DisplayMessage, title: DisplayTitle, draggable: false, theme: true });
                fileDownloadCheckTimer = window.setInterval(function () {
                    var cookieValue = $.cookie(CookieName);
                    if (cookieValue == token)
                        finishDownload();
                }, 1000);
            }

            function finishDownload() {
                window.clearInterval(fileDownloadCheckTimer);
                $.cookie(CookieName, null); //clears this cookie value
                $.unblockUI();
            }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <CTRL:Statistics runat="server" ID="CTRLuserStatistics" Visible="false"  OnGetBlockUIinfos="GetBlockUIinfos" />
            <br />
            <br />
            <asp:Label ID="LBerrorMSG" runat="server" CssClass="errore" Visible="false">No data</asp:Label>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server" CssClass="errore">Non hai i permessi</asp:Label>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>