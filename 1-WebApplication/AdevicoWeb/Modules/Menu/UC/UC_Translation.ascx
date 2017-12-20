<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Translation.ascx.vb" Inherits="Comunita_OnLine.UC_ItemTranslation" %>
<div style="clear:both;" class="Translation">
    <asp:Repeater ID="RPTtranslations" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <div style="clear:both;">
                <div class="Menu_TransLangName">
                    <%#Container.DataItem.LanguageName%>
                </div>
                <div class="Menu_Translation">
                     <asp:Label ID="LBitemName" runat="server" CssClass="Titolo_campo">Name:</asp:Label>
                     <asp:TextBox ID="TXBname" runat="server" CssClass="Testo_campo"  Columns="50" MaxLength="150" Text="<%#Container.DataItem.Name%>"></asp:TextBox>
                     <br />
                     <asp:Label ID="LBitemTooltip" runat="server" CssClass="Titolo_campo">ToolTip:</asp:Label>
                     <asp:TextBox ID="TXBtooltip" runat="server" CssClass="Testo_campo" Columns="50" MaxLength="150" Text="<%#Container.DataItem.ToolTip%>"></asp:TextBox>
                     <asp:Literal ID="LTid" runat="server" Visible="false" Text="<%#Container.DataItem.Id%>"></asp:Literal>
                     <asp:Literal ID="LTidLanguage" runat="server" Visible="false" Text="<%#Container.DataItem.IdLanguage%>"></asp:Literal>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
</div>