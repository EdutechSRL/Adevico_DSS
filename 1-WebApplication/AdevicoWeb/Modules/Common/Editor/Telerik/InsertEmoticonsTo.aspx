<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InsertEmoticonsTo.aspx.vb" Inherits="Comunita_OnLine.InsertEmoticonsTo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><asp:Literal ID="LTemoticonsTitle" runat="server"></asp:Literal></title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function getRadWindow() //mandatory for the RadWindow dialogs functionality
        {
            if (window.radWindow) {
                return window.radWindow;
            }
            if (window.frameElement && window.frameElement.radWindow) {
                return window.frameElement.radWindow;
            }
            return null;
        }

        function initDialog() //called when the dialog is initialized
        {
            var clientParameters = getRadWindow().ClientParameters;
        }
        if (window.attachEvent) {
            window.attachEvent("onload", initDialog);
        }
        else if (window.addEventListener) {
            window.addEventListener("load", initDialog, false);
        }

        function insertEmoticon(url) //fires when the Insert Link button is clicked
        {
            var closeArgument = {};
            closeArgument.image = url
            getRadWindow().close(closeArgument);
        }
    </script>

    <style type="text/css">
        img
        {
            cursor: hand;
        }
    </style>
    <br /><br />
<asp:Literal ID="LTemoticonsInfo" runat="server" Visible="false">*Click on Emoticon to insert</asp:Literal>
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil1.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil2.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil9.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil3.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil4.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil5.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil6.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil7.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil8.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil10.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/smil11.GIF" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/1.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/2.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/3.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/4.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/5.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/6.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/7.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/8.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/9.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/10.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/11.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/12.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/13.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/14.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/15.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/16.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/17.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/18.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/19.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/20.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/21.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/22.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/23.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/24.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/25.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/26.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/27.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/28.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/29.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/30.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/31.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/32.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/33.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/34.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/35.gif"  onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/36.gif"  onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/37.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/38.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/39.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/40.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/41.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/42.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/43.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/44.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/45.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/46.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/47.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/48.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/49.gif" onclick="insertEmoticon(this.src)" alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/50.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/51.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/52.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/53.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/54.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/55.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/56.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/57.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/58.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/59.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/60.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/61.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/62.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/63.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/64.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/65.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/66.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/67.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/68.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/69.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/70.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/71.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/72.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/73.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/74.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/75.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/76.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/77.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/78.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/79.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/80.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/81.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/82.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/83.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/84.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/85.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/86.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/87.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/88.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/89.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/90.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/91.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/92.gif" onclick="insertEmoticon(this.src)"alt="" />
        <img src="<%= me.AppUrl %>/Graphics/Modules/Editor/img/Emoticons/93.gif" onclick="insertEmoticon(this.src)"alt="" />
    <!--</fieldset> -->
    </form>
</body>
</html>