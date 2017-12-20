<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="UserSettings.aspx.vb" Inherits="Comunita_OnLine.UserSettings" %>
<%--Nomi Standard: OK--%>
<%--<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettings.ascx" %>--%>

<%@ Register TagPrefix="CTRL" TagName="ActionMessages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register tagPrefix="CTRL" tagName="TkSettings" src="~/Modules/Ticket/UC/UC_TicketUserSettings.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal runat="server" ID="LTpageTitle"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal runat="server" ID="LTtitle"></asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    
<%--    To Do
    
    <CTRL:MailSettings runat="Server" id="UC_MailSettings"></CTRL:MailSettings>--%>
<div class="tickets mgmgt globalsettings">

    <div class="pageheader">
        <%--<h2>Title</h2>--%>
        <div class="DivEpButton top">
            <%--<a class="linkMenu wide" href="#">Wide view</a>
            <a class="linkMenu narrow">Narrow view</a>--%>
            <asp:linkbutton ID="LKBsaveTop" runat="server" CssClass="linkMenu">*Save</asp:linkbutton>
            <asp:linkbutton ID="LKBbackTop" runat="server" CssClass="linkMenu" Visible="False">*Back</asp:linkbutton>
            <%--<a class="linkMenu" href="#">Back</a>
            <a class="linkMenu" href="#">Save</a>--%>
        </div>
        
        <CTRL:ActionMessages ID="UCactionMessages" runat="server" Visible="false" />

    </div>
    
    <CTRL:TkSettings runat="server" ID="UCtkSet" />
    
    <div class="DivEpButton bottom">
        <%--<a class="linkMenu wide" href="#">Wide view</a>
        <a class="linkMenu narrow">Narrow view</a>--%>
        <asp:linkbutton ID="LKBsaveBot" runat="server" CssClass="linkMenu">*Save</asp:linkbutton>
        <asp:linkbutton ID="LKBbackBot" runat="server" CssClass="linkMenu" Visible="False">*Back</asp:linkbutton>
        <%--<a class="linkMenu" href="#">Back</a>
        <a class="linkMenu" href="#">Save</a>--%>
    </div>
    
</div>
</asp:Content>