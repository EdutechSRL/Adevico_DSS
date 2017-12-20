<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EnrollBulkAction.ascx.vb" Inherits="Comunita_OnLine.UC_EnrollBulkAction" %>
<div class="inner">
    <div class="fieldobject clearfix">
        <div class="fieldrow title">
            <asp:Label ID="LBenrollToCommunitiesBulkTitle" CssClass="title" runat="server">*Bulk actions</asp:Label>
        </div>
        <div class="fieldrow commands">
            <asp:Button ID="BTNenrollToSelectedCommunities" runat="server" Text="*Enroll" />
            <span class="inputgroup">
                <asp:CheckBox ID="CBXselectAll" runat="server" /><asp:Label ID="LBenrollToSelectOnAllPages" runat="server" AssociatedControlID="CBXselectAll">*Apply on all the pages</asp:Label>
            </span>
            <asp:Label ID="LBextraInfoForCommunitiesSelected" runat="server" CssClass="extrainfo alert" Visible="false">*other {0} communities selected on page {1}</asp:Label>
        </div>
    </div>
</div>
