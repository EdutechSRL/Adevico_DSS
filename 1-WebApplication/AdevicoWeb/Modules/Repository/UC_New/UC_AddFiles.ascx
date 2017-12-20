<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddFiles.ascx.vb" Inherits="Comunita_OnLine.UC_AddFiles" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Repository/UC_New/UC_FolderSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<div class="dialog fileupload" title="<%=AddFilesDialogTitle()%>">
    <div class="fieldobject upload">
        <div class="fieldrow title">
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false"/>
            <div class="description">
                <asp:Literal ID="LTaddFilesDescription" runat="server" Text="*"></asp:Literal>
            </div>
        </div>
        <div class="fieldrow community" id="DVcommunity" runat="server" visible="false">
            <asp:Label id="LBaddFilesToCommunity_t" runat="server" Text="*Community:" CssClass="fieldlabel" AssociatedControlID="LBaddFilesToCommunity"></asp:Label>
            <asp:Label id="LBaddFilesToCommunity" runat="server" Text="*Community:" CssClass="text"></asp:Label>
        </div>
        <div class="fieldrow path" id="DVcurrentPath" runat="server">
            <asp:Label ID="LBcurrentPath_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcurrentPath" Text="*Current Path"></asp:Label>
            <asp:Label ID="LBcurrentPath" runat="server" CssClass="text" ></asp:Label>
        </div>
        <div class="fieldrow path" id="DVfolderSelector" runat="server">
            <asp:Label ID="LBselectDestinationFolderPath" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLfolderSelector" Text="*Current Path"></asp:Label>
            <CTRL:Selector ID="CTRLfolderSelector" runat="server" AutoPostBack="false" /> 
        </div>
    </div>
    <div class="fieldobject upload" id="DVuploader" runat="server">
        <div class="fieldrow files">
            <asp:Label ID="LBselectAsyncFiles_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RAUfiles" Text="*Select files"></asp:Label>
            <div class="inlinewrapper">
                <telerik:RadAsyncUpload runat="server" ID="RAUfiles" ChunkSize="1048576" PostbackTriggers="BTNaddFiles"  OnClientValidationFailed="OnClientValidationFailed"/>
            </div>
        </div>
    </div>
    <div class="fieldobject upload" id="DVitemsType" runat="server">
        <div class="fieldrow">
            <asp:Label ID="LBfilesItemType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBLitemType" Text="*Types:"></asp:Label>
            <div class="inlinewrapper">
                <asp:RadioButtonList ID="RBLitemType" runat="server" CssClass="inputgroup" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
                <div class="description">
                    <asp:Label ID="LBitemTypeDescription" runat="server" CssClass="text">You must select a type for all file uploaded.</asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div class="fieldobject upload" id="DVhidden" runat="server">
        <div class="fieldrow">
            <asp:Label ID="LBhideFiles_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXhideItems" Text="*Hide files:"></asp:Label>
            <div class="inlinewrapper">
                <span class="inputgroup">
                    <asp:CheckBox ID="CBXhideItems" runat="server" /><asp:Label ID="LBhideFiles" runat="server" AssociatedControlID="CBXhideItems">*hide files</asp:Label>
                </span>
            </div>
        </div>
    </div>
    <div class="fieldobject upload">
        <div class="fieldrow clearfix commands">
            <div class="left">&nbsp;</div>
            <div class="right">
                <asp:HyperLink ID="HYPcloseAddFilesDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
                <asp:Button ID="BTNaddFiles" runat="server" CssClass="linkMenu" Text="*Add"  CausesValidation="false" OnClientClick="return submitUploadWindow();" />
            </div>
        </div>
    </div>
</div>