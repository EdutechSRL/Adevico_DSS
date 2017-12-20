<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InLineProjectRoles.ascx.vb" Inherits="Comunita_OnLine.UC_InLineProjectRoles" %>
<span class="roleslist" runat="server" id="SPNroleslist">
<asp:Repeater ID="RPTroles" runat="server">
    <ItemTemplate>
        <span class="role" title="<%#Container.DataItem.LongName %>">
            <span class="initials"><%#Container.DataItem.ShortName%></span>
            <span class="complete"><%#Container.DataItem.LongName%></span>
        </span>
    </ItemTemplate>
</asp:Repeater>
</span>