<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CommunityNewsRead.aspx.vb" Inherits="Comunita_OnLine.CommunityNewsRead" Theme="Materiale" EnableTheming="true" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" style="width:900px; text-align:right; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
        <asp:HyperLink id="HYPbackHistory" runat="server" EnableViewState="false" CssClass="Link_Menu" Visible="false" Text="Back" Height="18px"></asp:HyperLink>
    </div>
    <div id="Div3" style="width:900px; text-align:left; padding-top:100px; margin: 0px auto;  clear:both;" runat="server">
	    <br />
        <asp:Literal id="LTnoCommunityAccess" runat="server"></asp:Literal>
    </div>
</asp:Content>