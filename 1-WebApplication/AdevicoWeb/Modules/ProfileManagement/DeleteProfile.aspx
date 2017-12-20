<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="DeleteProfile.aspx.vb" Inherits="Comunita_OnLine.DeleteProfile" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
        <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201602221000lm" rel="Stylesheet" type="text/css" />
     <script language="Javascript" type="text/javascript">
        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }     
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
    </div>
    <div class="DeleteProfile_Del">
        <asp:Label ID="LBelimina_t" runat="server"></asp:Label>
        <div class="DeleteProfile_Update">
            <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Delete" ID="LNBconfirmDelete"
                CausesValidation="false" Visible="false" OnClientClick="return onUpdating();"></asp:LinkButton>
        </div>
    </div>
</asp:Content>
