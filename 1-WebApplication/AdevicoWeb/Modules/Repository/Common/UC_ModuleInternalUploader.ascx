<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleInternalUploader.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleInternalUploader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="fieldobject upload" id="DVuploader" runat="server">
    <div class="fieldrow files">
        <div class="asyncupload">
            <asp:Label ID="LBselectAsyncFiles_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RAUfiles" Text="*Select files"></asp:Label>
            <div class="inlinewrapper">
                <telerik:RadAsyncUpload runat="server" ID="RAUfiles" ChunkSize="1048576" HideFileInput="true" MultipleFileSelection="Automatic" MaxFileInputsCount="8"/>
            </div>
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
<%--<div id="uploadError">
    <asp:UpdatePanel ID="UDPerrors" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
        <ContentTemplate><asp:Label ID="LBerrorNotification" runat="server"></asp:Label></ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:Literal ID="LTscript" runat="server"></asp:Literal>--%>