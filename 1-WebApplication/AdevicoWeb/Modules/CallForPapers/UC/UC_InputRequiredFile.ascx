<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InputRequiredFile.ascx.vb" Inherits="Comunita_OnLine.UC_InputRequiredFile" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalFilesUploader" Src="~/Modules/Repository/Common/UC_ModuleInternalUploader.ascx" %>
<asp:MultiView ID="MLVfield" runat="server">
    <asp:View ID="VIWunknown" runat="server"></asp:View>
    <asp:View ID="VIWempty" runat="server"></asp:View>
    <asp:View ID="VIWfile" runat="server">
        <div class="fieldobject fileupload" runat="server" id="DVrequiredFile">
            <div class="fieldrow fieldinput">                    
                <asp:Label runat="server" ID="LBfileinputText" CssClass="fieldlabel">File</asp:Label>
                <div class="fielddescription">
                    <asp:Label runat="server" ID="LBfileinputDescription" CssClass="description">Description</asp:Label>
                </div>
                <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server" DisplayFileSelectLabel="false" MaxFileInput="1" MaxItems="1" DisplayTypeSelector="false"  AjaxEnabled="true" PostBackTriggers="<%=PostBackTriggers %>" />
                <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false" Visible="false"  />
                <asp:Label runat="server" ID="LBrequiredFileHelp" CssClass="inlinetooltip"></asp:Label>    
                <span class="icons">
                    <asp:Button ID="BTNremoveFile" runat="server" Text="R" CssClass="icon delete" />
                </span>
                <br/>
                <span class="fieldinfo">
                    <asp:Label ID="LBerrorMessagefileinput" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>     
            </div>
        </div>
    </asp:View>
</asp:MultiView>