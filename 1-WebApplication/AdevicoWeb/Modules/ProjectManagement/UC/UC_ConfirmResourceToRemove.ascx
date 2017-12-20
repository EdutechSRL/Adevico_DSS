<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ConfirmResourceToRemove.ascx.vb" Inherits="Comunita_OnLine.UC_ConfirmResourceToRemove" %>

<div class="fieldobject options">
    <div class="fieldrow" id="DVdescription" runat="server" visible="false">
        <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
        <asp:Label ID="LBdescriptionAssignedTasks" runat="server" CssClass="description" Visible="false"></asp:Label>
    </div>
    <asp:Repeater ID="RPTactions" runat="server">
        <ItemTemplate>
            <div class="fieldrow">
                <input type="radio" id="RDremoveaction" name="RDremoveaction" value="<%#CInt(Container.DataItem) %>" <%# SetChecked(Container.DataItem)%> />
                <label for="RDremoveaction"><%#Resource.getValue("name.RemoveAction." & Container.DataItem.ToString())%></label>
                <asp:Label ID="LBactionDescription" runat="server" cssclass="description"> </asp:Label>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div class="fieldobject clearfix" id="DVcommands" runat="server">
    <div class="fieldrow buttons right">
        <asp:Button ID="BTNapplyRemoveResource" runat="server" CssClass="linkMenu" />
        <asp:Button ID="BTNcancelRemoveResource" runat="server" CssClass="linkMenu" CausesValidation="false" />
    </div>
</div>