<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddFolder.ascx.vb" Inherits="Comunita_OnLine.UC_AddFolder" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Repository/UC_New/UC_FolderSelector.ascx" %>
<div class="dialog createnewfolder" title="<%=AddFolderDialogTitle()%>">
    <div class="fieldobject upload">
        <div class="fieldrow title">
            <div class="description">
                <asp:Literal ID="LTaddFolderDescription" runat="server" Text="*"></asp:Literal>
            </div>
        </div>
        <div class="fieldrow community" id="DVcommunity" runat="server" visible="false">
            <asp:Label id="LBaddFolderToCommunity_t" runat="server" Text="*Community:" CssClass="fieldlabel" AssociatedControlID="LBaddFolderToCommunity"></asp:Label>
            <asp:Label id="LBaddFolderToCommunity" runat="server" Text="*Community:" CssClass="text"></asp:Label>
        </div>
        <div class="fieldrow path" id="DVcurrentPath" runat="server">
            <asp:Label ID="LBcurrentPath_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcurrentPath" Text="*Current Path"></asp:Label>
            <asp:Label ID="LBcurrentPath" runat="server" CssClass="text" ></asp:Label>
        </div>
        <div class="fieldrow path" id="DVselectFolder" runat="server">
            <asp:Label ID="LBselectDestinationFolderPath" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLfolderSelector" Text="*Current Path"></asp:Label>
            <CTRL:Selector ID="CTRLfolderSelector" runat="server" /> 
        </div>
    </div>
    <div class="fieldobject upload">
        <asp:Repeater id="RPTitems" runat="server">
            <ItemTemplate>
                <div class="fieldrow newfolderinput<%#GetCssClass(Container.ItemIndex ) %>">
                    <asp:Label ID="LBnewFolderName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBnewFolderName">*New folder:</asp:Label>
                    <asp:TextBox ID="TXBnewFolderName" runat="server" MaxLength="2000" CssClass="inputtext"></asp:TextBox>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject upload">
        <div class="fieldrow">
            <asp:Label ID="LBhideFolders_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXhideFolders" Text="*Hide folders:"></asp:Label>
            <div class="inlinewrapper">
                <span class="inputgroup">
                    <asp:CheckBox ID="CBXhideFolders" runat="server" /><asp:Label ID="LBhideFolders" runat="server" AssociatedControlID="CBXhideFolders">*hide folders</asp:Label>
                </span>
            </div>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBallowUploadIntoFolder_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXallowUpload" Text="*Upload:"></asp:Label>
            <div class="inlinewrapper">
                <span class="inputgroup">
                    <asp:CheckBox ID="CBXallowUpload" runat="server" /><asp:Label ID="LBallowUploadIntoFolder" runat="server" AssociatedControlID="CBXallowUpload">*allow upload folder</asp:Label>
                </span>
            </div>
        </div>
    </div>
    <div class="fieldobject upload">
        <div class="fieldrow clearfix commands">
            <div class="left">&nbsp;</div>
            <div class="right">
                <asp:HyperLink ID="HYPcloseAddFolderDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
                <asp:Button ID="BTNaddFolder" runat="server" CssClass="linkMenu" Text="*Add"  CausesValidation="false" />
            </div>
        </div>
    </div>
</div>