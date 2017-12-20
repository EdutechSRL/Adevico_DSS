<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Display.aspx.vb" Inherits="Comunita_OnLine.DisplayNoticeBoardMessage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="TLtitleCommunityName"></title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-32" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
<%--    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <meta http-equiv="expires" content="0" />--%>
    

    <link rel="Stylesheet" type="text/css" href="../../Graphics/Modules/NoticeBoard/noticeboard.css" />    
    <link rel="Stylesheet" type="text/css" href="../../Style/Bacheca/StampaBacheca.css" media="print" />
    <link rel="Stylesheet" type="text/css" href="../../Style/Latex/Latex.css" />
    <asp:Literal ID="LTpreloadCss" runat="server"></asp:Literal>
    <script src="../../Style/Latex/Latex.js" type="text/javascript" ></script>
</head>
<body id="BDdisplay" runat="server">
    <form id="form1" runat="server">
        <div id="datacontainer">
            <div class="datacontent">
                <asp:MultiView ID="MLVdisplay" runat="server">
                    <asp:View id="VIWsessionTimeout" runat="server">
                        <br /><br /><br /><br />
                        <asp:Literal ID="LTsessionTimeout" runat="server"></asp:Literal>
                        <br /><br /><br /><br />
                    </asp:View>
                    <asp:View ID="VIWnopermission" runat="server">
                        <br /><br /><br /><br />
                        <asp:Literal ID="LTnoPermission" runat="server"></asp:Literal>
                        <br /><br /><br /><br />
                    </asp:View>
                     <asp:View ID="VIWmessageError" runat="server">
                        <br /><br /><br /><br />
                        <asp:Literal ID="LTdisplay" runat="server"></asp:Literal>
                        <br /><br /><br /><br />
                    </asp:View>
                    <asp:View ID="VIWmessage" runat="server">
                        <div class="renderedtext">
                            <asp:literal ID="LTmessage" runat="server"></asp:literal>
                        </div>
                    </asp:View>
                </asp:MultiView>    
            </div>
        </div>
    </form>
</body>
</html>