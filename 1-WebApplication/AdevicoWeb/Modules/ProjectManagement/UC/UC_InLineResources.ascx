<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InLineResources.ascx.vb" Inherits="Comunita_OnLine.UC_InLineResources" %>
<span class="resourceslist" runat="server" id="SPNresourceslist">
<asp:Repeater ID="RPTresources" runat="server">
    <HeaderTemplate>
    
    </HeaderTemplate>
    <ItemTemplate>
        <span class="resource" title="<%#Container.DataItem.LongName %>" data-id="<%#Container.DataItem.IdResource %>">
            <span class="initials"><%#Container.DataItem.ShortName%></span>
            <span class="complete"><%#Container.DataItem.LongName%></span>
        </span>
    </ItemTemplate>
</asp:Repeater>
</span>