<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ViewTree.aspx.vb" Inherits="Comunita_OnLine.ViewCommunityTree" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdashboardTopBarHeader" Src="~/Modules/DashBoard/UC/UC_DashboardTopBarHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TreeHeader" Src="~/Modules/DashBoard/UC/UC_CommunitiesTreeHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Tree" Src="~/Modules/DashBoard/UC/UC_CommunitiesTree.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
      <!-- CTRLtreeHeader INIT-->
    <CTRL:TreeHeader id="CTRLtreeHeader" runat="server"></CTRL:TreeHeader>
    <CTRL:CTRLdashboardTopBarHeader runat="server" ID="CTRLtopbarHeader"></CTRL:CTRLdashboardTopBarHeader>
    <!-- CTRLtreeHeader END-->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
  
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton" runat="server" id="DVmenuTop" visible="false">
        <asp:HyperLink ID="HYPgoToPreviousPage" runat="server" Text="*Back" CssClass="linkMenu"></asp:HyperLink>
    </div>
    <div class="homecontent nobar">
        <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWtree" runat="server">
                <CTRL:Tree id="CTRLtree" runat="server" DisplayMessageOnControl="true"></CTRL:Tree>
            </asp:View>
            <asp:View ID="VIWunknown" runat="server">
                <h3><asp:Label id="LBunkownCommunityForTree" runat="server">*Unknown community</asp:Label> </h3>
            </asp:View>
        </asp:MultiView>
    </div>      
</asp:Content>