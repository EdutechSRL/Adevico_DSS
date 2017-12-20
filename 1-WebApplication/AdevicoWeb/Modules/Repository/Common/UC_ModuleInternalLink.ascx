<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleInternalLink.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleInternalLink" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Repository/UC_New/UC_SelectRepositoryItemsTreeMode.ascx" %>

<ul class="columns">
    <li class="column">
        <CTRL:Selector id="CTRLrepositorySelector" runat="server" width="900px" RemoveEmptyFolders="true" TreeSelect="singleselect"  />
    </li>
    <li class="column" runat="server" id="LIselectedItems">
        <div class="description">
            <asp:Literal ID="LTlinkedItemsToOtherModule" runat="server">These elements have already been selected</asp:Literal>
        </div>
        <div class="selecteditems">
            <asp:CheckBoxList ID="CBLselectedItems" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical"/>
        </div>
    </li>
</ul>