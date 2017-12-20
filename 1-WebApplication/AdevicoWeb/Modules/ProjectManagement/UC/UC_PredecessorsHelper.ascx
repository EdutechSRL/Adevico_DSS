<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PredecessorsHelper.ascx.vb"
    Inherits="Comunita_OnLine.UC_PredecessorsHelper" %>
<div class="dialog dlglinkshelp" title="<%= PredecessorsHelperDialogTitleTranslation() %>">
    <div class="fieldobject">
        <div class="fieldrow header">
            <label class="fieldlabel title" for=""><asp:Literal ID="LTpredecessorsHelpLinkTypes" runat="server">Available link types:</asp:Literal></label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpLink_t" runat="server" AssociatedControlID="LBpredecessorsHelpLink" CssClass="fieldlabel">[id][type][+/-number];</asp:Label>
            <asp:Label ID="LBpredecessorsHelpLink" runat="server" CssClass="description">predecessor link format</asp:Label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpActivityId_t" runat="server" AssociatedControlID="LBpredecessorsHelpActivityId" CssClass="fieldlabel">[id]</asp:Label>
            <asp:Label ID="LBpredecessorsHelpActivityId" runat="server" CssClass="description">predecessor task id</asp:Label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpType_t" runat="server" AssociatedControlID="LBpredecessorsHelpType" CssClass="fieldlabel">[type]</asp:Label>
            <asp:Label ID="LBpredecessorsHelpType" runat="server" CssClass="description">FS - SS - FF - SF</asp:Label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpFS_t" runat="server" AssociatedControlID="LBpredecessorsHelpFS" CssClass="fieldlabel">FS</asp:Label>
            <asp:Label ID="LBpredecessorsHelpFS" runat="server" CssClass="description">Finish - Start</asp:Label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpSS_t" runat="server" AssociatedControlID="LBpredecessorsHelpSS" CssClass="fieldlabel">SS</asp:Label>
            <asp:Label ID="LBpredecessorsHelpSS" runat="server" CssClass="description">Start - Start</asp:Label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpFF_t" runat="server" AssociatedControlID="LBpredecessorsHelpFF" CssClass="fieldlabel">FF</asp:Label>
            <asp:Label ID="LBpredecessorsHelpFF" runat="server" CssClass="description">Finish - Finish</asp:Label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpSF_t" runat="server" AssociatedControlID="LBpredecessorsHelpSF" CssClass="fieldlabel">SF</asp:Label>
            <asp:Label ID="LBpredecessorsHelpSF" runat="server" CssClass="description">Start - Finish</asp:Label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBpredecessorsHelpLead_t" runat="server" AssociatedControlID="LBpredecessorsHelpLead" CssClass="fieldlabel">[+/-number]</asp:Label>
            <asp:Label ID="LBpredecessorsHelpLead" runat="server" CssClass="description">number > 0 days of lag, < 0 days of lead. Default 0. </asp:Label>
        </div>
    </div>
</div>