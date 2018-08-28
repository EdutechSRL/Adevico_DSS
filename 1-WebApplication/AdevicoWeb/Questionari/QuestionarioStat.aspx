<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionarioStat.aspx.vb" Inherits="Comunita_OnLine.QuestionarioStat"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
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
    <style>
        div.optionChart
        {
            
            display: inline-block;
        }

        div.barChart {
            width: 50%;
            min-width: 250px;
        }

        div.pieChart
        {
            /*KEEP aspect ratio!*/
            width: 450px;
            height: 250px;
        }
        
        html body .question table.tableData
        {
            width: 48%;
            float: left;
            margin-right: 1%;
            margin-bottom: 22px;
        }
		td.statistics {
			border: none;
			border-bottom: solid 1px #ececec;
		}
        table.questionfreetext{
            width:100%;
        }
			
		.questionnairepage.name{
			display:block;
			margin-bottom:6px;
			margin-top:6px;
		}

        html body .question .longLabel table.tableData{
            float:none;
            width:100%;   
        }

        html body .question .longLabel div.optionChart {
            float:none;
            width:100%; /*900px;   */
            /*border: 1px solid red;*/ /*da togliere*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
<div id="QuestionarioStat">
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <asp:PlaceHolder ID="PHStat" runat="server"></asp:PlaceHolder>
            <br />
            <br />
            <asp:Label ID="LBerrorMSG" runat="server" CssClass="errore" Visible="false">No data</asp:Label>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server" CssClass="errore">Non hai i permessi</asp:Label>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</div>
</asp:Content>