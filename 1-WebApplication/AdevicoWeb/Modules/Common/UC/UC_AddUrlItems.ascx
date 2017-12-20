<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddUrlItems.ascx.vb" Inherits="Comunita_OnLine.UC_AddUrlItems" %>
<div class="fieldobject attachmentinput">
    <asp:Repeater id="RPTitems" runat="server">
        <ItemTemplate>
            <div class="fieldrow <%#GetCssClass(Container.DataItem) %>">
                <asp:Label ID="LBurl_t" runat="server" AssociatedControlID="TXBurl">*Url:</asp:Label>
                <asp:TextBox ID="TXBurl" runat="server" MaxLength="2000" CssClass="inputtext inputurl" ></asp:TextBox>
                <asp:Label ID="LBurlName_t" runat="server" AssociatedControlID="TXBurlName">Display name:</asp:Label>
                <asp:TextBox ID="TXBurlName" runat="server" MaxLength="2000" CssClass="inputtext inputurl" ></asp:TextBox>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>