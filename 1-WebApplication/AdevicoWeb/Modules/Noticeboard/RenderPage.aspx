<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RenderPage.aspx.vb" Inherits="Comunita_OnLine.NoticeboardRenderPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<%@ Register TagPrefix="CTRL" TagName="CTRLrender" Src="./UC/UC_NoticeboardRender.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="TLtitleCommunityName"></title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-32" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
  
    <link href="<%=ResolveUrl("~/Content/themes/"& Me.SystemSettings.Style.jqueryTheme &"/jquery-ui-"&Me.SystemSettings.Style.jqueryUIVersion &".css")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/scripts/jquery-"&Me.SystemSettings.Style.jqueryVersion &".min.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/scripts/jquery-ui-"&Me.SystemSettings.Style.jqueryUIVersion &".min.js")%>"></script>

    <script type="text/javascript" language="javascript">
        $(function(){
            $('.command.print').click(function() {
                $("iframe").get(0).contentWindow.focus();
                $("iframe").get(0).contentWindow.print();

                /*window.frames["frameright"].focus();
                window.frames["frameright"].print();*/
            });
        });
    </script>
    <link rel="Stylesheet" type="text/css" href="../../Graphics/Generics/css/3_COL_Reset.css" />    
    <link rel="Stylesheet" type="text/css" href="../../Graphics/Generics/css/4_UI_Elements.css" /> 
    <link rel="Stylesheet" type="text/css" href="../../Graphics/Generics/Command/css/command.css" />    
    <link rel="Stylesheet" type="text/css" href="../../Graphics/Modules/NoticeBoard/noticeboard.lb.css" />    
</head>
<body class="noticeboard advanced">
    <form id="form1" runat="server">
        <asp:MultiView ID="MLVdisplay" runat="server">
            <asp:View id="VIWnoAccess" runat="server">
               <div class="noticeboardwrapper content clearfix">
                    <div class="column left noticeboard">
                        <div class="header">

                        </div>
                        <div class="content">
                                <asp:Literal ID="LTsessionTimeout" runat="server"></asp:Literal>
                        </div>
                        <div class="footer clearfix" runat="server" id="DVfooterCommands" visible="false">

                        </div>
                    </div>
                    <div class="column left history">
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWmessage" runat="server">
                <CTRL:CTRLrender id="CTRLrender" runat="server" ></CTRL:CTRLrender>
            </asp:View>
        </asp:MultiView>    
    </form>
</body>
</html>