<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditSimple.aspx.vb"  Inherits="Comunita_OnLine.EditSimpleNoticeboardMessage" ValidateRequest="false" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLeditorHTML" Src="~/Modules/Noticeboard/UC/UC_BaseEditor.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%=PageUtility.ApplicationUrlBase & "/Graphics/Modules/NoticeBoard/noticeboard.lb.css"%>" />
    <link rel="Stylesheet" type="text/css" href="<%=PageUtility.ApplicationUrlBase & "/Style/Latex/Latex.css"%>" />
    <script type="text/javascript" src="<%=PageUtility.ApplicationUrlBase & "Jscript/Modules/Common/jquery.ddbuttonlist.js"%>"></script>
    <script type="text/javascript" src="<%=PageUtility.ApplicationUrlBase & "Jscript/Modules/Noticeboard/noticeboard.js"%>"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWcontainer" runat="server">
        <div class="fieldobject toolbar clearfix">
            <div class="fieldrow left">&nbsp;</div>
            <div class="fieldrow right">
                <asp:HyperLink ID="HYPbackFromEditor" runat="server" Visible="false" CssClass="linkMenu">*Back</asp:HyperLink>
                <asp:LinkButton ID="LNBsaveMessage" Visible="false" runat="server" CssClass="linkMenu">*Save</asp:LinkButton>
                <asp:LinkButton ID="LNBsaveMessageAndExit" Visible="false" runat="server" CssClass="linkMenu">*Save</asp:LinkButton>
            </div>
        </div>
        <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
        <div class="noticeboard advanced">
            <div class="noticeboardwrapper editor content clearfix">
                  <CTRL:CTRLeditorHTML id="CTRLeditor" runat="server" EditorAlign="left" UseRealFontSize="true"></CTRL:CTRLeditorHTML>
            </div>
        </div>
    </asp:View>
     <asp:View ID="VIWnoPermissions" runat="server">
        <div class="fieldobject toolbar clearfix">
            <div class="fieldrow left">
            </div>
            <div class="fieldrow right">
                 <asp:HyperLink ID="HYPbackFromEditorNoPermissions" runat="server" Visible="false" CssClass="linkMenu">*Back</asp:HyperLink>
            </div>
        </div>
        <div class="noticeboard advanced">
            <div class="noticeboardwrapper editor content clearfix">
                <asp:Literal ID="LTmessage" runat="server"></asp:Literal>
            </div>
        </div>
    </asp:View>
</asp:MultiView>
</asp:Content>