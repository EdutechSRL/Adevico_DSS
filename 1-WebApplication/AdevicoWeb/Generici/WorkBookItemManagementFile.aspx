<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookItemManagementFile.aspx.vb" Inherits="Comunita_OnLine.WorkBookItemManagementFile" %>

<%@ Register TagPrefix="CTRL" TagName="WorkBookUpload" Src="~/Generici/UC/UC_GenericUploadFile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_CompactFileUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ManagementFile" Src="~/Modules/WorkBook/UC/UC_WorkBookItemFiles.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <style type="text/css">
        UL LI
        {
            list-style-type: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function HideCommunityUpload() { 
            $("#<%=Me.DVcommunity.ClientID %>").hide();
            return true;
        }
        function HideWorkBookUpload() {
            ProgressStart();
            $("#<%=Me.DVworkbook.ClientID %>").hide();
            return true;
        }
        function ProgressStart() {
            getRadProgressManager().startProgressPolling();
        }
    </script>
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <asp:HyperLink ID="HYPbackToItems" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Back to items" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToItem" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Back to item" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPmultipleUpload" runat="server" CssClass="Link_Menu"
            Text="Multiple upload" Height="18px" NavigateUrl="~/Generici/WorkBookItemMultipleUpload.aspx"></asp:HyperLink>
    </div>
    <div>
        <div runat="server" id="DVcommunityLink">
            <b>
                <asp:Literal ID="LTaddFromCommunity_t" runat="server" >Import file from community</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                <asp:Literal ID="LTlinkToCommunity" runat="server">In this way you can link one or more community file to this workbook</asp:Literal>
                &nbsp;&nbsp;&nbsp;<asp:Button ID="BTNlinkCommunityFile" runat="server" Text="Link" />
            </div>
            <br />
            <br />
        </div>
        <div runat="server" id="DVcommunity">
            <b>
                <asp:Literal ID="LTuploadToCommunity_t" runat="server" >Upload file into community and workbook</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                <div style="float: left; width: 780;">
                    <CTRL:CommunityFile ID="CTRLCommunityFile" runat="server" AjaxEnabled="false" UpdatePermissionButton="False" />
                </div>
                <div style="float: left; width: 100; padding-top: 210px; padding-left:10px;">
                    <span style="vertical-align:bottom;">
                    <asp:Button ID="BTNaddCommunityFile" runat="server" Text="Link" />
                </div>
            </div>
        </div>
        <div runat="server" id="DVworkbook" style="text-align:left;  clear:both;">
            <br />
            <br />
            <b>
                <asp:Literal ID="LTuploadToWorkBook_t" runat="server" >Upload file ONLY into workbook</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                <div style="float: left; width: 780;">
                    <CTRL:WorkBookUpload ID="CTRLWorkBookUpload" runat="server" InitialFileInputsCount="1"
                        MaxFileInputsCount="1" />
                </div>
                <div style="float: left; width: 100; padding-top: 25px; padding-left:10px;">
                    <asp:Button ID="BTNaddToWorkbook" runat="server" Text="Link" />
                </div>
            </div>
            <br />
            <br />
        </div>
        <div id="DVfileList" style="text-align:left;">
        <br />
        <b>
            <asp:Literal ID="LTitemFiles_t" runat="server" >WorkBook Item's files</asp:Literal></b>
        <hr />
        <div style="width: 900px; padding-bottom: 10px;">
            <CTRL:ManagementFile ID="CTRLmanagementFile" runat="server" />
        </div>
        </div>
    </div>
</asp:Content>
