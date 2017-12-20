<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddVersion.ascx.vb" Inherits="Comunita_OnLine.UC_AddVersion" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<div class="dialog fileversion" title="<%=AddVersionDialogTitle()%>">
    <div class="fieldobject upload">
        <div class="fieldrow title">
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false"/>
            <div class="description">
                <asp:Literal ID="LTaddVersionDescription" runat="server" Text="*"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="fieldobject upload" id="DVuploader" runat="server">
        <div class="fieldrow files">
            <div class="asyncuploadversion">
                <asp:Label ID="LBaddVersion_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RAUfiles" Text="*Select files"></asp:Label>
                <div class="inlinewrapper">
                    <telerik:RadAsyncUpload runat="server" ID="RAUfiles" ChunkSize="1048576"  PostbackTriggers="BTNaddVersion"
                        AutoAddFileInputs="false" MultipleFileSelection="Disabled" OnClientValidationFailed="OnClientValidationFailedInLine"/>
                </div>
            </div>
        </div>
    </div>
   <div class="fieldobject upload">
        <div class="fieldrow clearfix commands">
            <div class="left">&nbsp;</div>
            <div class="right">
                <asp:HyperLink ID="HYPcloseAddVersionDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
                <asp:Button ID="BTNaddVersion" runat="server" CssClass="linkMenu" Text="*Add"  CausesValidation="false" OnClientClick="return submitUploadWindow();" />
            </div>
        </div>
    </div>
</div><asp:Literal ID="LTcssClassDialog" runat="server" Visible="false">fileversion</asp:Literal><asp:Literal ID="LTtemplateFile" runat="server"  Visible="false"><span class="iteminfo"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>