<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="ItemDownloadExternal.aspx.vb" Inherits="Comunita_OnLine.ItemDownloadExternal" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLheader" Src="~/Modules/Repository/UC_New/UC_RepositoryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/ExternalService.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLheader runat="server" ID="CTRLheader"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PreHeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TopBarContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton grid_12 alpha omega" id="DVmenu" runat="server"  Visible="false">
        <asp:HyperLink ID="HYPbackToPreviousUrl" runat="server" CssClass="linkMenu">*Back</asp:HyperLink>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false"/>
    <asp:Literal ID="LTtemplateFile" runat="server"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></asp:Literal>
    <asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>
</asp:Content>
