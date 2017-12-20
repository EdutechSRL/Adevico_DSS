<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_OtherModuleLinkRepositoryItems.ascx.vb" Inherits="Comunita_OnLine.UC_OtherModuleLinkRepositoryItems" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_SelectCommunityFiles.ascx" %>

<ul class="columns">
    <li class="column">
        <CTRL:communityfile id="CTRLCommunityFile" runat="server" width="900px" tristateselection="false" folderselectable="false" />
    </li>
    <li class="column" runat="server" id="HCselectedItems">
        <asp:Literal ID="LTselectedItems" runat="server">These elements have already been selected</asp:Literal>
        <br /><br />
        <asp:CheckBoxList ID="CBLselectedItems" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical">
    
        </asp:CheckBoxList>
    </li>
</ul>