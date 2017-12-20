<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryUpload.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryUpload" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLuploader" Src="~/Modules/Repository/UC/UC_SingleFileUploader.ascx" %>

<asp:Content ID="ContentHeader" runat="server" ContentPlaceHolderID="HeadContent">
    <script language="javascript" type="text/javascript">

        var regex = "[^\"'?|\\/*+^:;]"; //regular expression per consentire lettere numeri underscore e trattino
        $(document).ready(function () {
            $(".alphaNumeric").keypress(function (event) {
                var c = String.fromCharCode(event.which);
                if (c.match(regex)) {
                    return true;
                } else {
                    return false;
                }
            });
        });

        function ProgressStart() {
            getRadProgressManager().startProgressPolling(); 
        }
    </script>
    <style type="text/css">
        .TableFolder
        {
        }
        .TableFile
        {
        }
        .TableRow
        {
            clear: both;
        }
        .TableCellLeft
        {
            float: left;
            text-align: left;
        }
        .TableCellRight
        {
        }
        .TableHeader
        {
        }
        .h25
        {
            height: 25px;
        }
        
        .h120
        {
            height: 120px;
        }
        .h150
        {
            height: 150px;
        }
        .w100
        {
            width: 100px;
        }
        .w150
        {
            width: 150px;
        }
        .w200
        {
            width: 200px;
        }
        .DetailsContainerFile
        {
            border-top: solid 2px;
            border-bottom: solid 1px;
            border-left: solid 2px;
            border-right: solid 2px;
            height: 210px;
        }
        .DetailsContainerFolder
        {
            border-top: solid 2px;
            border-bottom: solid 1px;
            border-left: solid 2px;
            border-right: solid 2px;
            height: 150px;
        }
        .TableAdvancedRow
        {
            clear: both;
            padding-left: 10px;
        }
        .AdvancedContainer
        {
            border-top: solid 1px;
            border-bottom: solid 2px;
            border-left: solid 2px;
            border-right: solid 2px;
            clear: both;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
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

                <asp:Button ID="BTNupload" runat="server" OnClientClick="ProgressStart()" CssClass="Link_Menu"/>
                <asp:Button ID="BTNcreate" runat="server" Visible="false" CssClass="Link_Menu"/>
            </span>
        </div>
    </div>
    <asp:MultiView ID="MLVupload" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWupload" runat="server">
            <div style="text-align: center;">
                <CTRL:CTRLuploader ID="CTRLuploader" runat="server" AjaxEnabled="false" UpdatePermissionButton="False"  />
            </div>
            <div style="text-align: right; width: 900px; padding-top:10px;">
                <asp:Button ID="BTNuploadBottom" runat="server" OnClientClick="ProgressStart()" CssClass="Link_Menu"/>
                <%--<asp:Button ID="BTNuploadAndPermission" runat="server" OnClientClick="ProgressStart()"/>--%>
                <asp:Button ID="BTNcreateBottom" runat="server" Visible="false" CssClass="Link_Menu"/>
                <%--<asp:Button ID="BTNcreateAndPermission" runat="server" Visible="false" />--%>
            </div>
        </asp:View>
        <asp:View ID="VIWpermissionToUpload" runat="server">
            <div style="padding-top:100px; padding-bottom:100px;">
                <asp:Label ID="LBnoPermissionToUpload" runat="server" CssClass=""></asp:Label>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>