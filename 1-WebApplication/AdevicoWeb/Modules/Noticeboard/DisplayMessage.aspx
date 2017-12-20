<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DisplayMessage.aspx.vb" Inherits="Comunita_OnLine.NoticeboardDisplayMessage"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="TLtitleCommunityName"></title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-32" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
<%--    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <meta http-equiv="expires" content="0" />--%>
    
<%--    <link rel="Stylesheet" type="text/css" href="../../Graphics/Generics/css/3_COL_Reset.css" />    
    <link rel="Stylesheet" type="text/css" href="../../Graphics/Generics/css/4_UI_Elements.css" />    --%>
    <link rel="Stylesheet" type="text/css" href="../../Graphics/Modules/NoticeBoard/noticeboard-render.css" />    
      <style type="text/css" media="print">
        div.printonly
        {
            display: block;
        }
        body{

            padding-top: 5em;
        }
    </style>
    <link rel="Stylesheet" type="text/css" href="../../Style/Latex/Latex.css" />

    <asp:Literal ID="LTpreloadCss" runat="server"></asp:Literal>
    <script src="../../Style/Latex/Latex.js" type="text/javascript" ></script>

</head>
<body id="BDdisplay" runat="server">
    <form id="form1" runat="server">
        <div id="datacontainer">
            <div class="datacontent">
                <div class="title printonly">
                    <h3><asp:literal ID="LTnoticeboardRenderTitle" runat="server"></asp:literal>&nbsp;<asp:Label ID="LBcommunityName" runat="server" CssClass="community"></asp:Label></h3>
                    <h4><asp:Label ID="LBcreatedOn" runat="server" CssClass="date"></asp:Label> - <asp:Label ID="LBcreatedBy" runat="server" CssClass="by"></asp:Label></h4>
                </div>
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