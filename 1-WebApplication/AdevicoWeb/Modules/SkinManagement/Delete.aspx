<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Delete.aspx.vb" Inherits="Comunita_OnLine.DeleteSkin" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
 <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPback" runat="server" CssClass="Link_Menu" Text="#Back" />
         <asp:Button ID="BTNdeleteSkin" runat="server"/>
    </div>
     <asp:MultiView ID="MLVeditSkin" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBskinMessage" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWdata" runat="server">
            <br />
            <div>
                <asp:Label ID="LBinfoDelete" runat="server" CssClass="Testo_Campo"></asp:Label>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
