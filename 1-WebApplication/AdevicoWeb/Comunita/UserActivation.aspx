<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="UserActivation.aspx.vb" Inherits="Comunita_OnLine.UserActivation"  Theme="Materiale" EnableTheming="true"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width:900px; text-align:right; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
        <asp:HyperLink id="HYPbackHistory" runat="server" EnableViewState="false" CssClass="Link_Menu" Visible="false" Text="Back" Height="18px"></asp:HyperLink>
    </div>
    <div id="DVcontent" style="width:900px; text-align:left; padding-top:50px; margin: 0px auto;  clear:both;" runat="server">
        <asp:Label ID="LBinfoAccesso" Runat="server"></asp:Label>
    </div>
</asp:Content>