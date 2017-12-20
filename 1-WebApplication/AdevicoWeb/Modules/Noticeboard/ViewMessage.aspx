<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="ViewMessage.aspx.vb" Inherits="Comunita_OnLine.ViewNoticeboardMessage" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%=PageUtility.ApplicationUrlBase & "/Graphics/Modules/NoticeBoard/noticeboard.lb.css"%>" />
        <script type="text/javascript" src="<%=PageUtility.ApplicationUrlBase & "Jscript/Modules/Noticeboard/noticeboard.js"%>"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="noticeboardwrapper content clearfix">
        <div class="column noticeboard" runat="server">
            <div class="header">
                <h2 class="title"><asp:Literal ID="LTnoticeboardTitle" runat="server">*Noticeboard</asp:Literal>:</h2>
            </div>
            <div class="content">
                 <asp:Literal ID="LTrenderNoticeboard" runat="server"></asp:Literal>
            </div>
            <div class="footer clearfix" runat="server" id="DVfooterCommands" visible="false">
                <div class="left">
                    &nbsp;
                </div>
                <div class="right">
                    <asp:HyperLink ID ="HYPprintNoticeboard" runat="server" CssClass="linkMenu command print" Visible="false">*Print</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
    <asp:Literal ID="LTrenderNoticeboardTemplate" Visible="false" runat="server"><iframe class="noticeboardiframe" src="{0}" width="100%" height="450px" frameborder="0"></iframe></asp:Literal>
</asp:Content>