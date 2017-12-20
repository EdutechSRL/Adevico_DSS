<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookItemMultipleUpload.aspx.vb" Inherits="Comunita_OnLine.WorkBookItemMultipleUpload"%>

<%@ Register TagPrefix="CTRL" TagName="WorkBookUpload" Src="~/Generici/UC/UC_GenericUploadFile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_MultipleFileUploader.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
<style type="text/css">
    UL LI	{	
	list-style-type:none;
	}	
</style>
 <script type="text/javascript" language="javascript">
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
        <asp:HyperLink ID="HYPbackToFileManagement" runat="server" 
            CssClass="Link_Menu" Visible="false" Text="Back to file management" Height="18px"></asp:HyperLink>
        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Upload" ID="LNBupload"
                CausesValidation="false"></asp:LinkButton>
    </div>
    <div>
        <div runat="server" id="DVcommunity">
            <b>
                <asp:Literal ID="LTuploadToCommunity_t" runat="server" >Upload file into community and workbook</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                <CTRL:CommunityFile ID="CTRLCommunityFile" runat="server" AjaxEnabled="false" AllowPersonalPermission="false" />
            </div>
            <br />
            <br />
        </div>
        <b>
            <asp:Literal ID="LTuploadToWorkBook_t" runat="server" >Upload file ONLY into workbook</asp:Literal></b>
        <hr />
        <div style="width: 600px;">
            <CTRL:WorkBookUpload ID="CTRLWorkBookUpload" runat="server" InitialFileInputsCount="3"
                MaxFileInputsCount="3" />
        </div>
        <br />
    </div>
</asp:Content>