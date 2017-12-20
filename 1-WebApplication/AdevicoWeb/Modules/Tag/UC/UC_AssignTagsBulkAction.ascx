<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AssignTagsBulkAction.ascx.vb" Inherits="Comunita_OnLine.UC_AssignTagsBulkAction" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Tag/UC/UC_TagsSelectorForCommunity.ascx" %>
<div class="inner">
    <div class="fieldobject clearfix">
        <div class="fieldrow title">
            <asp:Label ID="LBassignTagsBulkTitle" CssClass="title" runat="server">*Bulk actions</asp:Label>
        </div>
        <div class="fieldrow selector">
            <label class="fieldlabel" for=""><asp:Literal ID="LTtagSelector_t" runat="server">*Select:</asp:Literal></label>
            <CTRL:Selector ID="CTRLtagsSelector" runat="server" /> 
        </div>
    </div>
    <div class="fieldobject clearfix">
        <div class="fieldrow commands left">
            <span class="inputgroup">
                <asp:CheckBox ID="CBXselectAll" runat="server" /><asp:Label ID="LBselectOnAllPages" runat="server" AssociatedControlID="CBXselectAll">*Apply on all the pages</asp:Label>
            </span>
        </div>
        <div class="fieldrow commands right">
            <asp:Button ID="BTNapplyTagsOnSelectedCommunities" runat="server" Text="*Save" />
        </div>
    </div>
</div>