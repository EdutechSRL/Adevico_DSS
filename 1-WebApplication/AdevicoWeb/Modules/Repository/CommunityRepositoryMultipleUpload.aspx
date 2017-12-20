<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryMultipleUpload.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryMultipleUpload" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLmultipleUploader" Src="~/Modules/Repository/UC/UC_MultipleFileUploader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <style type="text/css">
        UL LI
        {
            list-style-type: none;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function ProgressStart() {
            getRadProgressManager().startProgressPolling();
        }
    </script>

    <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        padding-bottom: 5px;">
        <div style="float: left; text-align: left; width: 420px">
            &nbsp;
        </div>
        <div style="float: left; text-align: right; width: 480px;">
            <span style="vertical-align: text-bottom;">
                <asp:HyperLink ID="HYPbackToDownloads" runat="server" CssClass="Link_Menu" Visible="false"
                    Text="Back to downloads" Height="18px"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Visible="false"
                    Text="Back to management" Height="18px"></asp:HyperLink>
            </span>
        </div>
    </div>
    <asp:MultiView ID="MLVupload" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWupload" runat="server">
            <div style="text-align: center;">
                <CTRL:CTRLmultipleUploader ID="CTRLuploader" runat="server" AjaxEnabled="false" AllowPersonalPermission="false" />
            </div>
            <div style="text-align: right; width: 900px; padding-top: 10px;">
                <asp:Button ID="BTNupload" runat="server" OnClientClick="ProgressStart()" CssClass="Link_Menu" />
                <asp:Button ID="BTNuploadAndPermission" runat="server" OnClientClick="ProgressStart()" Visible="false" CssClass="Link_Menu"/>
            </div>
        </asp:View>
        <asp:View ID="VIWpermissionToUpload" runat="server">
            <div style="padding-top: 100px; padding-bottom: 100px;">
                <asp:Label ID="LBnoPermissionToUpload" runat="server" CssClass=""></asp:Label>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>