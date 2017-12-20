<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AsyncUpload.ascx.vb" Inherits="Comunita_OnLine.UC_AsyncUpload" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="inlinewrapper">
    <ul class="qsf-list">
        <li runat="server" id="LIallowedTypes" visible="false">
            <asp:Literal ID="LTallowedTypes_t" runat="server">*Allowed file types:</asp:Literal>
            <asp:Literal ID="LTallowedTypes" runat="server"></asp:Literal>
        </li>
        <li runat="server" id="LIallowedSize" visible="false">
            <asp:Literal ID="LTallowedSize_t" runat="server">*Allowed file size:</asp:Literal>
            <asp:Literal ID="LTallowedSize" runat="server"></asp:Literal>
        </li>
    </ul>
    <telerik:RadAsyncUpload runat="server" ID="RAUcontrol" AutoAddFileInputs="false" 
        MaxFileInputsCount="1"
      
        EnableInlineProgress="true" 
      
        MultipleFileSelection="Disabled"
        OnClientValidationFailed="validationFailed"
        OnClientFileSelected="OnClientFileSelected "
        OnClientFilesUploaded="OnClientFilesUploaded"
        PostbackTriggers="BTNasyncUpload">
    </telerik:RadAsyncUpload>
    <asp:Button ID="BTNasyncUpload" runat="server" CssClass="hiddensubmit"/>                
</div>