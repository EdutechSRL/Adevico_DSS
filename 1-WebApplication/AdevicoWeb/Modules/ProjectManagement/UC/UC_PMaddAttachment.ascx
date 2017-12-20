<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PMaddAttachment.ascx.vb" Inherits="Comunita_OnLine.UC_PMaddAttachment" %>
<%@ Register TagPrefix="CTRL" TagName="SelectActivity" Src="~/Modules/ProjectManagement/UC/UC_ActivitySelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLurls" Src="~/Modules/Common/UC/UC_AddUrlItems.ascx"  %>
<%@ Register TagPrefix="CTRL" TagName="InternalFilesUploader" Src="~/Modules/Repository/UC/UC_ModuleInternalFileMultipleUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="LinkRepositoryItems" Src="~/Modules/Common/UC/UC_OtherModuleLinkRepositoryItems.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RepositoryFilesUploader" Src="~/Modules/Repository/UC/UC_AjaxMultipleFileUploader.ascx" %>

<div class="dialog <%=UploaderCssClass %>">
    <div class="fieldobject intro" id="DVdescription" runat="server">
        <div class="fieldrow">
            <div class="description">
                <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="fieldobject attachmentinput">
        <CTRL:CTRLurls id="CTRLurls" runat="server" MaxItems="5" Visible="false" />
        <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server" MaxFileInputsCount="5" Visible="false" AjaxEnabled="false" />
        <CTRL:LinkRepositoryItems id="CTRLlinkItems" runat="server"  Visible="false" />
        <CTRL:RepositoryFilesUploader id="CTRLrepositoryUploader" MaxFileInputsCount="5" AjaxEnabled="false" runat="server" Visible="false" />
    </div>
    <div class="fieldobject commands" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:Button id="BTNaddAttachment" runat="server" Text="Add" />
            <asp:LinkButton id="LNBcloseAttachmentWindow" runat="server" CssClass="linkMenu close"></asp:LinkButton>
        </div>
    </div>
</div>