<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Details.aspx.vb" Inherits="Comunita_OnLine.CommunityDetails" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdetails" Src="~/Modules/Dashboard/UC/UC_CommunityDetails.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdetailsHeader" Src="~/Modules/Dashboard/UC/UC_CommunityDetailsHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
     <CTRL:CTRLdetailsHeader runat="server" ID="CTRLdetailsHeader"></CTRL:CTRLdetailsHeader>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton">
        <asp:HyperLink ID="HYPgoToPreviousPage" runat="server" Text="*Back" CssClass="linkMenu" Visible="false"></asp:HyperLink>
    </div>
     <asp:MultiView ID="MLVdetails" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWdetails" runat="server">
            <CTRL:CTRLdetails runat="server" ID="CTRLcommmunityDetails"></CTRL:CTRLdetails>
        </asp:View>
        <asp:View ID="VIWunknown" runat="server">
            <div class="homecontent">
            <h3><asp:Label id="LBunkownCommunityInfo" runat="server">*Unknown community</asp:Label> </h3>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>