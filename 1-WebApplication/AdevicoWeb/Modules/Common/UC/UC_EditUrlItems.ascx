<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditUrlItems.ascx.vb" Inherits="Comunita_OnLine.UC_EditUrlItems" %>
<div class="dialog <%=EditingCssClass %>">
    <div class="fieldobject intro" id="DVdescription" runat="server">
        <div class="fieldrow">
            <div class="description">
                <asp:Literal ID="LTdescriptionEditingUrls" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="fieldobject attachmentinput">
         <asp:Repeater id="RPTitems" runat="server">
            <ItemTemplate>
                <div class="fieldrow <%#GetCssClass(Container.ItemIndex ) %>">
                    <asp:Literal ID="LTidItem" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                    <asp:Label ID="LBurl_t" runat="server" AssociatedControlID="TXBurl">*Url:</asp:Label>
                    <asp:TextBox ID="TXBurl" runat="server" MaxLength="2000" CssClass="inputtext inputurl" Text='<%#Container.DataItem.Address %>' ></asp:TextBox>
                    <asp:Label ID="LBurlName_t" runat="server" AssociatedControlID="TXBurlName">Display name:</asp:Label>
                    <asp:TextBox ID="TXBurlName" runat="server" MaxLength="2000" CssClass="inputtext inputurl" Text='<%#Container.DataItem.Name %>'></asp:TextBox>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject commands" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:Button id="BTNsaveUrlSettings" runat="server" Text="*Save" />
            <asp:LinkButton id="LNBcloseUrlSettingsWindow" runat="server" CssClass="linkMenu close" Text="*Close"></asp:LinkButton>
        </div>
    </div>
</div>