<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MultimediaFileLoader.aspx.vb" Inherits="Comunita_OnLine.MultimediaFileLoader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Player Multimediale</title>
    <!--[RESIZE-FIX-BEGIN] Added this meta-->
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <!--[RESIZE-FIX-END]-->

    <link href="~/Styles.css" type="text/css" rel="stylesheet"/>
    <style type="text/css">
        .Invisible
        {
            width: 100%;
            height: 650px;
            border-width: 0px;
            border-color: #FFFFFF;
            padding: 0px;
            margin: 0px;
        }
    </style>
  
  <script type="text/javascript" src="../../Jscript/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="../../Jscript/jquery.cookie.js"></script>

    <!--[RESIZE-FIX-BEGIN] Added this Script-->
                <script>
                    //Script Resize IFrame on Window Resize
                    var delay = (function () {
                        var timer = 0;
                        return function (callback, ms) {
                            clearTimeout(timer);
                            timer = setTimeout(callback, ms);
                        };
                    })();

                    $(document).ready(function () {
                        var newWindowHeight = $(window).height();
                        $(".divIframe iframe").css("height", newWindowHeight);

                        var easingTime = 100; //set this for change responsiveness

                        $(window).bind("resize", resizeWindow);
                        function resizeWindow(e) {
                            var newWindowHeight = $(window).height();

                            delay(function () {
                                $(".divIframe iframe").css("height", newWindowHeight);
                            }, easingTime);

                        }
                    });
                </script>
                <!--[RESIZE-FIX-END]-->



<%--  <script type="text/javascript" src="<%#ResolveUrl("~/scripts/jquery-ui-"&Me.SystemSettings.Style.jqueryUIVersion &".min.js")%>"></script>--%>
     <script type="text/javascript">
         var enableUpdate = <%= lcase(isnumeric(request.Querystring("LinkId"))) %>;
         function updateOpenerWindow(id) {
             if (enableUpdate){
                 link = location.search;
                  if (link.length > 0) {
                    link = link.substring(1, link.length);
                    jQuery.cookie("fileDownload", link,{ path: '/' });
                 }
             }
            return true;
         }

     </script>
</head>
<body onunload="updateOpenerWindow();">
 <form id="aspnetForm" runat="server">
    <div>
        <asp:MultiView ID="MLVplayer" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWplayer" runat="server">
                <div style="padding: 0px; margin: 0px; width: 100%; height: 10px; display:none;">
                    <div>
                         <asp:Label ID="LBactionContainer" runat="server"></asp:Label>
                    </div>
                    <div style="display:none;">
                        <div >
                            <asp:LinkButton ID="LNBback" runat="server" Text="Ritorna al materiale"></asp:LinkButton>
                        </div>
                    </div>
                    <br />
                </div>
                <div class="divIframe" style="width: 100%; height: 100%">
                    <asp:Label ID="LBplayerContainer" runat="server"></asp:Label>
                </div>
            </asp:View>
            <asp:View ID="VIWfileError" runat="server">
                <div id="Div2" style="width: 900px; text-align: left; padding-top: 300px; margin: 0px auto;
                    clear: both;">
                    <asp:Label ID="LBfileError" runat="server"></asp:Label>
                 </div>
            </asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>