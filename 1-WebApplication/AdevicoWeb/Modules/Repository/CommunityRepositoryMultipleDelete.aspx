<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryMultipleDelete.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryMultipleDelete" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_SelectCommunityFiles.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        padding-bottom: 5px;">
        <div style="float: left; text-align: left; width: 420px">
            &nbsp;
        </div>
        <div style="float: left; text-align: right; width: 480px;">
            <span style="vertical-align: text-bottom;">
                <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Visible="false"
                    Text="Back to management" Height="18px"></asp:HyperLink>
                <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Delete selected" ID="LNBmultipleDelete" CausesValidation="false"
                    Visible="false"></asp:LinkButton>
            </span>
        </div>
    </div>
    <asp:MultiView ID="MLVmultiple" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWfileSelector" runat="server">
            <div style="text-align: left; clear:both;">
                <CTRL:CommunityFile ID="CTRLCommunityFile" runat="server" width="900px" TriStateSelection="False" />
            </div>
        </asp:View>
        <asp:View ID="VIWpermissionToDelete" runat="server">
            <div style="padding-top: 100px; padding-bottom: 100px; clear:both;">
                <asp:Label ID="LBnoPermissionToDelete" runat="server" CssClass=""></asp:Label>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
