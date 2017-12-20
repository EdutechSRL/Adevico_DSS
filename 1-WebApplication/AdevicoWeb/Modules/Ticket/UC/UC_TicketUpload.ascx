<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TicketUpload.ascx.vb" Inherits="Comunita_OnLine.UC_TicketUpload" %>
<%@ Register TagPrefix="CTRL" TagName="InternalFilesUploader" Src="~/Modules/Repository/UC/UC_ModuleInternalFileMultipleUploader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="RepositoryFilesUploader" Src="~/Modules/Repository/UC/UC_AjaxMultipleFileUploader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="LinkRepositoryItems" Src="~/Modules/Common/UC/UC_OtherModuleLinkRepositoryItems.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLurls" Src="~/Modules/Common/UC/UC_AddUrlItems.ascx"  %>


<div class="dialog <%=UploaderCssClass %>">
    <div class="fieldobject intro" id="DVdescription" runat="server">
        <div class="fieldrow">
            <div class="description">
                <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="fieldobject attachmentinput">
        <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server" MaxFileInputsCount="5" Visible="false" AjaxEnabled="false" />
        <CTRL:RepositoryFilesUploader id="CTRLrepositoryUploader" MaxFileInputsCount="5" AjaxEnabled="false" runat="server" Visible="false" />
        <CTRL:LinkRepositoryItems id="CTRLlinkItems" runat="server"  Visible="false" />
    </div>
    <div class="fieldobject commands" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:Button id="BTNaddAttachment" runat="server" Text="#add" />
            <asp:LinkButton id="LNBcloseAttachmentWindow" runat="server" CssClass="linkMenu close">#close</asp:LinkButton>
        </div>
    </div>
</div>