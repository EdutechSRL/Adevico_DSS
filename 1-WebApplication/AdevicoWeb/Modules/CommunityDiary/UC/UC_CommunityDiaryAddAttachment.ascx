<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityDiaryAddAttachment.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityDiaryAddAttachment" %>
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
     <div class="fieldobject eventiteminfo" id="DVeventItemInfo" runat="server" visible="false" >
        <div class="fieldrow" id="DVtitle" runat="server">
            <asp:Label ID="LBtitle_t" runat="server" CssClass="fieldlabel">*Title:</asp:Label>
            <div class="inlinewrapper">
                <asp:Label ID="LBtitle" runat="server" CssClass="field"></asp:Label>
            </div>
        </div>
        <div class="fieldrow" id="DVtime" runat="server">
            <asp:Label ID="LBitemTime_t" runat="server" CssClass="fieldlabel">*Time:</asp:Label>
            <div class="inlinewrapper">
                <asp:Label ID="LBitemTime" runat="server" CssClass="field"></asp:Label>
            </div>
        </div>
    </div>
    <div class="fieldobject attachmentinput">
        <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server" MaxFileInput="5" MaxItems="10" Visible="false" AjaxEnabled="false" PostBackTriggers="BTNaddAttachment" />
        <CTRL:LinkRepositoryItems id="CTRLlinkItems" runat="server"  Visible="false"  MaxSelectorWidth="900px" TreeSelect="cascadeselect" RemoveEmptyFolders="true" FolderSelectable="false"/>
        <CTRL:RepositoryItemsUploader id="CTRLrepositoryItemsUploader" runat="server" DisplayHideCommands="false" MaxFileInput="5" MaxItems="10" Visible="false" AjaxEnabled="false" PostBackTriggers="BTNaddAttachment" DisplayErrorInline="true" />
    </div>
    <div class="fieldobject upload" id="DVhiddenItem" runat="server" visible="false" >
        <div class="fieldrow">
            <asp:Label ID="LBhideItemFiles_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXhideItemFiles" Text="*Hide files:"></asp:Label>
            <div class="inlinewrapper">
                <span class="inputgroup">
                    <asp:CheckBox ID="CBXhideItemFiles" runat="server" /><asp:Label ID="LBhideItemFiles" runat="server" AssociatedControlID="CBXhideItemFiles">*hide files</asp:Label>
                </span>
            </div>
        </div>
    </div>
     <div class="fieldobject upload" id="DVhiddenRepository" runat="server" visible="false" >
        <div class="fieldrow">
            <asp:Label ID="LBhideRepositoryFiles_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXhideRepositoryFiles" Text="*Hide files:"></asp:Label>
            <div class="inlinewrapper">
                <span class="inputgroup">
                    <asp:CheckBox ID="CBXhideRepositoryFiles" runat="server" /><asp:Label ID="LBhideRepositoryFiles" runat="server" AssociatedControlID="CBXhideRepositoryFiles">*hide files</asp:Label>
                </span>
            </div>
        </div>
    </div>
    <div class="fieldobject commands" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:LinkButton id="LNBcloseAttachmentWindow" runat="server" CssClass="linkMenu close"></asp:LinkButton>
            <asp:Button id="BTNaddAttachment" runat="server" Text="Add" OnClientClick="return submitUploadWindow();" />
        </div>
    </div>
</div>