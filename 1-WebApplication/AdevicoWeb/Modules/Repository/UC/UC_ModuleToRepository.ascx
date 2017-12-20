<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleToRepository.ascx.vb"
    Inherits="Comunita_OnLine.UC_ModuleToRepository" %>
<%@ Register TagPrefix="CTRL" TagName="InternalUploader" Src="~/Modules/Repository/UC/UC_ModuleInternalFileMultipleUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FileSelector" Src="~/Modules/Repository/UC/UC_SelectCommunityFiles.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RepositoryUploader" Src="~/Modules/Repository/UC/UC_AjaxMultipleFileUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FolderSelector" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<style type="text/css">
    .right
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

<div class="Row" id="DVcommandsTop" runat="server" visible="false">
    <b>
        <asp:Literal ID="LTcurrentAction" runat="server"></asp:Literal>
    </b>
    <hr />
    <div class="ContainerLeft">
        
    </div>
    <div style="text-align:right">
        <asp:Button ID="BTNcloseAddActionWindowTop" Text="Close" runat="server" />
        <asp:Button ID="BTNselectActionTop" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNselectFolderTop" runat="server" Text="Create Action" Visible="false" />
        <asp:Button ID="BTNLinkToModuleTop" runat="server" Text="Link" Visible="false"  />
        <asp:Button ID="BTNaddCommunityFileTop" runat="server" Text="Upload and link" Visible="false"  />
        <asp:Button ID="BTNaddInternalToItemTop" runat="server" Text="Upload and link" Visible="false"  />
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
        <div class="Row">
              <CTRL:RepositoryUploader ID="CTRLRepositoryUpload" runat="server" AjaxEnabled="true" />
        </div>
    </asp:View>
    <asp:View ID="VIWdownloadActionInternalFile" runat="server">
        <div class="Row">
              <CTRL:InternalUploader ID="CTRLinternalUpload" runat="server"/>
        </div>
    </asp:View>
    <asp:View ID="VIWdownloadActionSelectFile" runat="server">
        <div class="Row">
            <CTRL:FileSelector ID="CTRLselectCommunityFile" runat="server" width="90%" TriStateSelection="true"
                FolderSelectable="true" />
        </div>
    </asp:View>
    <asp:View ID="VIWuploadFileAction" runat="server">
        <div class="Row">
              <CTRL:FolderSelector ID="CTRLCommunityFolder" runat="server" width="90%" SelectionMode="Single"
                                        AjaxEnabled="true" />
        </div>
    </asp:View>
</asp:MultiView>
 <div class="Row" id="DVcommandsBottom" runat="server" visible="false">
    <div class="ContainerLeft">
        
    </div>
    <div style="text-align:right">
        <asp:Button ID="BTNcloseAddActionWindowBottom" Text="Close" runat="server" />
        <asp:Button ID="BTNselectActionBottom" Text="" runat="server" Visible="false" />
        <asp:Button ID="BTNselectFolderBottom" runat="server" Text="Create Action" Visible="false"  />
        <asp:Button ID="BTNLinkToModuleBottom" runat="server" Text="Link" Visible="false"  />
        <asp:Button ID="BTNaddCommunityFileBottom" runat="server" Text="Upload and link" Visible="false"  />
        <asp:Button ID="BTNaddInternalToItemBottom" runat="server" Text="Upload and link"  Visible="false"  />
    </div>
</div>