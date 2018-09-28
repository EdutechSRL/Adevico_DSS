<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddRepositoryAction.ascx.vb" Inherits="Comunita_OnLine.UC_EduPathAddRepositoryAction" %>
<%@ Register TagPrefix="CTRL" TagName="RepositoryItemsUploader" Src="~/Modules/Repository/Common/UC_ModuleRepositoryUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalFilesUploader" Src="~/Modules/Repository/Common/UC_ModuleInternalUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="LinkRepositoryItems" Src="~/Modules/Repository/Common/UC_ModuleInternalLink.ascx" %>
<style type="text/css">
    /*.right
    {
        text-align: right;
    }
    UL LI
    {
        list-style-type: none;
    }
    .UploaderContainer
    {
        float: left;
        width: 80%;
    }
    .UploaderButtonContainer
    {
        float: left;
        width: 19%;
        padding-top: 210px;
        padding-left: 10px;
    }*/
</style>
<div class="fieldobject commands" id="DVcommandsTop" runat="server" visible="false">
    <div class="fieldrow">
        <b><asp:Literal ID="LTcurrentAction" runat="server"></asp:Literal></b><hr />
    </div>
   <div class="fieldrow buttons right">
        <asp:Button ID="BTNcloseAddActionWindowTop" Text="*Close" runat="server" />
        <asp:Button ID="BTNselectActionTop" Text="*change action" runat="server"  Visible="false" />
        <asp:Button ID="BTNLinkToModuleTop" runat="server" Text="* Link repository item" Visible="false"  />
        <asp:Button ID="BTNaddCommunityFileTop" runat="server" Text="*Upload to repository and to activity" Visible="false"  />
        <asp:Button ID="BTNaddInternalToItemTop" runat="server" Text="*Upload to activity" Visible="false"  />
    </div>
</div>

<asp:MultiView ID="MLVcontrol" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server"></asp:View>
    <asp:View ID="VIWactionRepositorySelector" runat="server">
        <asp:Repeater ID="RPTactions" runat="server">
            <ItemTemplate>
                <div class="Row">
                    <div class="ContainerLeft">
                        <asp:Button ID="BTNaddAction" Text="Add action" runat="server" />
                    </div>
                    <div class="ContainerRight">
                        <asp:Literal ID="LTaddAction" runat="server"></asp:Literal>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:View>
    <asp:View ID="VIWdownloadActionCommunityFile" runat="server">
        <div class="fieldobject attachmentinput">
            <div class="Row">
                <CTRL:RepositoryItemsUploader id="CTRLrepositoryItemsUploader" runat="server" MaxFileInput="5" MaxItems="10" AjaxEnabled="false" PostBackTriggers="BTNaddCommunityFileBottom,BTNaddCommunityFileTop" DisplayErrorInline="true" />
            </div>
        </div> 
    </asp:View>
    <asp:View ID="VIWdownloadActionInternalFile" runat="server">
        <div class="fieldobject attachmentinput">
            <div class="Row">
                <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server"  MaxFileInput="5" MaxItems="10"  AjaxEnabled="false" PostBackTriggers="BTNaddInternalToItemBottom,BTNaddInternalToItemTop" />
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdownloadActionSelectFile" runat="server">
        <div class="fieldobject attachmentinput">
            <div class="Row">
                <CTRL:LinkRepositoryItems id="CTRLlinkItems" runat="server"  MaxSelectorWidth="900px" TreeSelect="cascadeselect" RemoveEmptyFolders="true" FolderSelectable="false"/>
            </div>
        </div>
    </asp:View>
</asp:MultiView>
<div class="fieldobject commands" id="DVcommandsBottom" runat="server" visible="false">
    <div class="fieldrow buttons right">
        <asp:Button ID="BTNcloseAddActionWindowBottom" Text="Close" runat="server" />
        <asp:Button ID="BTNselectActionBottom" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNLinkToModuleBottom" runat="server" Text="Link" Visible="false"  />
        <asp:Button ID="BTNaddCommunityFileBottom" runat="server" Text="Upload and link" Visible="false"  />
        <asp:Button ID="BTNaddInternalToItemBottom" runat="server" Text="Upload and link"  Visible="false"  />
    </div>
</div>