<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="UploadFileErrors.aspx.vb" Inherits="Comunita_OnLine.UploadFileErrors" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        UL LI
        {
            list-style-type: none;
        }
         .DivEpButton
        {
            width: 900px;
            text-align: right; 
            padding-top: 5px;
            margin: 0px auto;
            clear: both;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
     <div class="DivEpButton" runat="server" id="DVmenu">
        <asp:HyperLink ID="HYPbackToModule" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back to file management" Height="18px"></asp:HyperLink>
    </div>
     <div id="Div2" style="width: 900px; text-align: left; padding-top: 5px; margin: 0px auto;
        clear: both;">
        <asp:Label ID="LBfileError" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>