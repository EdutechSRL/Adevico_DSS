<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_OrderBy.ascx.vb" Inherits="Comunita_OnLine.UC_RepositoryOrderBy" %>
<asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server"></asp:View>
    <asp:View ID="VIWcontent" runat="server">
        <div class="groupedselector <%=ContainerCssClass %>">
            <asp:Label ID="LBorderBySelectorDescription" runat="server" CssClass="description">*Sort by: </asp:Label>
            <div class="selectorgroup">
                <asp:Label ID="LBorderBySelected" runat="server" CssClass="selectorlabel"></asp:Label>
                <span class="selectoricon">&nbsp;</span>
            </div>
            <div class="selectormenu">
                <div class="selectorinner">
                    <div class="selectoritems">
                        <asp:Repeater ID="RPTorderBy" runat="server">
                            <ItemTemplate>
                                <div class="selectoritem" id="DVitemOrderBy" runat="server">
                                    <asp:LinkButton ID="LNBorderItemsBy" runat="server" CssClass="selectorlabel" CausesValidation="false"> <span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:LinkButton> 
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView><asp:Literal ID="LTcssClassActive" runat="server" Visible="false">active</asp:Literal><asp:Literal ID="LTcssClassOrderBy" runat="server" Visible="false">selectoritem</asp:Literal>