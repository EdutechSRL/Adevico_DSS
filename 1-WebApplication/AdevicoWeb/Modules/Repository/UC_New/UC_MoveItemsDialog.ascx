<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MoveItemsDialog.ascx.vb" Inherits="Comunita_OnLine.UC_MoveItemsDialog" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Repository/UC_New/UC_FolderSelector.ascx" %>

 <div class="dlgmoveitems hiddendialog" title="<%=GetDialogTitle() %>">
     <div class="fieldobject moveitems">
        <div class="fieldrow title">
            <div class="description">
                <asp:Label ID="LBmoveDialogDescription" runat="server"></asp:Label>
            </div>
        </div>
        <div class="fieldrow path" id="DVcurrentPath" runat="server">
            <asp:Label ID="LBcurrentPath_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcurrentPath" Text="*Current Path"></asp:Label>
            <asp:Label ID="LBcurrentPath" runat="server" CssClass="text" ></asp:Label>
        </div>
        <div class="fieldrow path">
            <asp:Label ID="LBselectDestinationFolderPath" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLfolderSelector" Text="*Current Path"></asp:Label>
            <CTRL:Selector ID="CTRLfolderSelector" runat="server" /> 
        </div>
        <div class="fieldrow buttons right">
            <asp:HyperLink ID="HYPcloseMoveConfirmDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
            <asp:Button ID="BTNapplyMoveTo" runat="server" CssClass="linkMenu" Text="*Move"  CausesValidation="false" />
        </div>
     </div>
</div>
<asp:Literal ID="LTcssClassDialog" runat="server" Visible="false">.dlgmoveitems</asp:Literal>