<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookError.aspx.vb" Inherits="Comunita_OnLine.WorkBookError" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <asp:HyperLink ID="HYPbackToItems" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back to items" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToItem" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back to item" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back to file management" Height="18px"></asp:HyperLink>
    </div>
    <div id="Div2" style="width: 900px; text-align: left; padding-top: 5px; margin: 0px auto;
        clear: both;">
        <asp:Label ID="LBfileError" runat="server"></asp:Label>
    </div>
</asp:Content>
