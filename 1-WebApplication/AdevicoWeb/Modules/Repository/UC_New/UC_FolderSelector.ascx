<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FolderSelector.ascx.vb" Inherits="Comunita_OnLine.UC_FolderSelector" %>
<div class="dropdown value enabled"><!--
    --><input type="hidden" id="HDNselectedIdFolder" runat="server"/><!--
    --><asp:Button ID="BTNupdate" runat="server" cssclass="folderapply"/><!--
    --><asp:label id="LBselectFolderPath" runat="server" CssClass="ddselector">*Select path</asp:label><!--
    --><span class="selector">
        <span class="selectoricon">&nbsp;</span>
        <span class="listwrapper">
            <span class="arrow"></span>
            <ul class="items">
                <asp:Repeater ID="RPTchildren" runat="server">
                    <ItemTemplate>
                        <asp:MultiView ID="MLVnode" runat="server">
                            <asp:View ID="VIWtypeOpenItemNode" runat="server">
                                <li class="item">
                            </asp:View>
                            <asp:View ID="VIWtypeItem" runat="server">
                                <asp:Literal ID="LTidFolder" runat="server" Visible="false" Text ="<%#Container.DataItem.Id %>"></asp:Literal>
                                <asp:Literal ID="LTname" runat="server" Visible="false" Text ="<%#Container.DataItem.Name %>"></asp:Literal>
                                <asp:HyperLink ID="HYPfolder" runat="server" class="ddbutton" data-text="<%#Container.DataItem.Name %>" data-id="<%#Container.DataItem.Id %>" ToolTip="<%#Container.DataItem.Name %>">
                                    <span class="icon">&nbsp;</span>
                                    <span class="pathname"><%#Container.DataItem.Name %></span>
                                </asp:HyperLink>
                            </asp:View>
                            <asp:View ID="VIWtypeNoChildren" runat="server"></asp:View>
                            <asp:View ID="VIWtypeOpenChildren" runat="server">
                                    <ul class="items">
                            </asp:View>
                            <asp:View ID="VIWtypeCloseChildren" runat="server">
                                    </ul>
                            </asp:View>
                            <asp:View ID="VIWtypeCloseNode" runat="server">
                                </li>
                            </asp:View>
                        </asp:MultiView>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </span>
    </span>
</div><asp:Literal ID="LTitemCssClass" runat="server" Visible="false">ddbutton</asp:Literal><asp:Literal ID="LTselectedItemCssClass" runat="server" Visible="false">activeselected</asp:Literal><asp:Literal ID="LTunselectableItemCssClass" runat="server" Visible="false">unselectable</asp:Literal>

