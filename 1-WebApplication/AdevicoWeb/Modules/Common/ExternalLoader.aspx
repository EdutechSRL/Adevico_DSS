<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.Master" CodeBehind="ExternalLoader.aspx.vb" Inherits="Comunita_OnLine.ExternalLoader" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/Authentication.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPHcontent" runat="server">
    <%--  <asp:UpdatePanel ID="UDPinfoLogon" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>--%>
            <div>
              <br /><br />
                <h2><asp:Label ID="LBmessage" runat="server"></asp:Label></h2>
                <br />
                <asp:Image ID="IMGloading" runat="server" ImageUrl="../../images/Ajax/loading4.gif"  Visible="false" />
            </div>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>