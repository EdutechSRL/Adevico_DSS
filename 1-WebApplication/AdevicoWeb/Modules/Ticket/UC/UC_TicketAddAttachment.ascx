<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TicketAddAttachment.ascx.vb" Inherits="Comunita_OnLine.UC_TicketAddAttachment" %>
<%@ Register TagPrefix="CTRL" TagName="RepositoryItemsUploader" Src="~/Modules/Repository/Common/UC_ModuleRepositoryUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalFilesUploader" Src="~/Modules/Repository/Common/UC_ModuleInternalUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="LinkRepositoryItems" Src="~/Modules/Repository/Common/UC_ModuleInternalLink.ascx" %>
<div class="dialog <%=UploaderCssClass %>">
    <div class="fieldobject intro" id="DVdescription" runat="server">
        <div class="fieldrow">
            <div class="description">
                <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="fieldobject attachmentinput">
        <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server" DisplayTypeSelector="false" MaxFileInput="2"  MaxItems="5" Visible="false" AjaxEnabled="false" PostBackTriggers="BTNaddAttachment" />
        <CTRL:LinkRepositoryItems id="CTRLlinkItems" runat="server"  Visible="false"  MaxSelectorWidth="900px" TreeSelect="cascadeselect" RemoveEmptyFolders="true" FolderSelectable="false"/>
        <CTRL:RepositoryItemsUploader id="CTRLrepositoryItemsUploader" runat="server" MaxFileInput="2" MaxItems="5" Visible="false" AjaxEnabled="false" PostBackTriggers="BTNaddAttachment" DisplayErrorInline="true" />
    </div>
    <div class="fieldobject commands" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:LinkButton id="LNBcloseAttachmentWindow" runat="server" CssClass="linkMenu close"></asp:LinkButton>
            <asp:Button id="BTNaddAttachment" runat="server" Text="*Add" OnClientClick="return submitUploadWindow();" />
        </div>
    </div>
</div>