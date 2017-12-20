<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TaskMultipleUpload.aspx.vb" MasterPageFile="~/AjaxPortal.Master" Inherits="Comunita_OnLine.TaskMultipleUpload" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="InternalUploader" Src="~/Modules/Repository/UC/UC_ModuleInternalFileMultipleUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RepositoryUploader" Src="~/Modules/Repository/UC/UC_AjaxMultipleFileUploader.ascx" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        UL LI{	
            list-style-type:none;
        }	
        .DivEpButton
        {
            text-align: right;
            padding: 10px 10px 5px 10px;
        }
        
        div.DivEpButton
        {
            width: 900px;
            text-align: right;
            clear:both;
        }
    .right
    {
        text-align: right;
    }
    .Row
    {
        padding-bottom: 10px;
        /*width: 90%;*/
    }

    </style>
    <script type="text/javascript" language="javascript">
        function HideCommunityUpload() {
            return true;
        }
        function HideItemUpload() {
            ProgressStart();
            return true;
        }
        function ProgressStart() {
            getRadProgressManager().startProgressPolling();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" class="DivEpButton" runat="server">
        <asp:HyperLink ID="HYPbackToItems" runat="server" CssClass="Link_Menu" Visible="false" Text="Back to items" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToItem" runat="server" CssClass="Link_Menu" Visible="false" Text="Back to item" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false" Text="Back to file management" Height="18px"></asp:HyperLink>
        <%--<asp:HyperLink ID="HYP" runat="server" CssClass="Link_Menu" Visible="false" Text="Back to file management" Height="18px"></asp:HyperLink>--%>
        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Upload" ID="LNBupload" CausesValidation="false"></asp:LinkButton>
    </div>
    <div>
        <div runat="server" id="DVcommunity">
            <b>
                <asp:Literal ID="LTuploadToCommunity_t" runat="server" >*Upload file into community and workbook</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                <CTRL:RepositoryUploader ID="CTRLRepositoryUpload" runat="server" AjaxEnabled="false" />
            </div>
            <br />
            <br />
        </div>
        <b>
            <asp:Literal ID="LTuploadToDiary_t" runat="server" >*Upload file ONLY into projects or tasks</asp:Literal></b>
        <hr />
        <div style="width: 700px;">
            <CTRL:InternalUploader ID="CTRLmoduleUpload" runat="server" MaxFileInputsCount="3" />
        </div>
        <br />
    </div>
</asp:Content>