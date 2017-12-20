<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModalCommunitySelector.ascx.vb" Inherits="Comunita_OnLine.UC_ModalCommunitySelector" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Common/UC/UC_FindCommunitiesByService.ascx" %>

<div class="dlgaddcommunities hiddendialog">
    <div class="fieldobject fielddescription" id="DVdescription" runat="server" visible="false" >
        <div class="fieldrow">
            <asp:Label ID="LBselectCommunityDescription" runat="server" CssClass="description"></asp:Label>
        </div>
    </div>
    <CTRL:Selector ID="CTRLcommunity"  runat="server" SelectionMode="Multiple" RaiseCommunityChangedEvent="True" />
    <div class="fieldobject clearfix">
        <div class="fieldrow right">
            <asp:HyperLink ID="HYPcloseSelectCommunityDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
            <asp:Button ID="BTNselectCommunity" runat="server" CssClass="Link_Menu" />
        </div>
    </div>
</div>
<asp:Literal ID="LTidentifier" runat="server" Visible="false">dlgaddcommunities</asp:Literal>