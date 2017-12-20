<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GenericFieldsMatcher.ascx.vb" Inherits="Comunita_OnLine.UC_GenericFieldsMatcher" %>
    <asp:MultiView ID="MLVcontrolData" runat="server">
    <asp:View ID="VIWempty" runat="server">
        <span class="Fieldrow">
            <br /><br /><br /><br />
            <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
            <br /><br /><br /><br />
        </span>
    </asp:View>
    <asp:View ID="VIWmatchColumns" runat="server">
        <br />
        <span class="Fieldrow">
            <asp:Label ID="LBinfo" runat="server" CssClass="Titolo_Campo full"></asp:Label>
        </span>
        <asp:Repeater ID="RPTmatcher" runat="server">
            <HeaderTemplate>
                <table width="100%" border="1px" cellspacing="0">
                    <tr class="ROW_header_Small_Center">
                        <th><asp:Label ID="LBsourceColumn" runat="server"></asp:Label></th>
                        <th><asp:Label ID="LBdestinationColumn" runat="server"></asp:Label></th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="LTcolumnNumber" runat="server" Visible="false" Text="<%#Container.Dataitem.Number %>"></asp:Literal>
                            <asp:Label ID="LBsourceName" runat="server" CssClass="Titolo_Campo" AssociatedControlID="DDLdestination" Text="<%#Container.Dataitem.SourceColumn %>"></asp:Label>
                        </td>

                        <td class="Field">
                            <asp:DropDownList ID="DDLdestination" runat="server" CssClass="Testo_Campo">
                        
                            </asp:DropDownList>
                            <asp:Label ID="LBerror" runat="server" Text="*" Visible="false"></asp:Label>
                        </td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        </asp:View>
</asp:MultiView>