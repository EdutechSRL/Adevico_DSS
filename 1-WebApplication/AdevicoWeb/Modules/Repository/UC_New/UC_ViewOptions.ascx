<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ViewOptions.ascx.vb" Inherits="Comunita_OnLine.UC_ViewOptions" %>
<span class="commands">
    <asp:HyperLink ID="HYPpresetTypeSimple" CssClass="command simple" Text="*Simple" runat="server" Visible="false"></asp:HyperLink>
    <asp:HyperLink ID="HYPpresetTypeStandard" CssClass="command standard" Text="*Standard" runat="server" Visible="false"></asp:HyperLink>
    <asp:HyperLink ID="HYPpresetTypeAdvanced" CssClass="command advanced" Text="*Advanced" runat="server" Visible="false"></asp:HyperLink>
    <div class="command groupedselector nolabel noactive custom" id="DIVcustomSettings" runat="server">
        <div class="selectorgroup">
            <span class="selectorlabel">&nbsp;</span>
            <span class="selectoricon actions">&nbsp;</span>
        </div>
        <div class="selectormenu">
            <div class="selectorinner">
                <div class="selectoritems">
                    <asp:Repeater ID="RPTviewOptions" runat="server">
                        <ItemTemplate>
                            <asp:MultiView ID="MLVviewOption" runat="server" ActiveViewIndex="0">
                                <asp:View ID="VIWoption" runat="server">
                                    <div class="selectoritem<%#GetItemCssClass(Container.DataItem) %>">
                                        <a class="command switch <%#GetItemTypeCssClass(Container.DataItem)%>">
                                            <span class="on">x</span><span class="off">o</span>
                                            <span class="text"><%#Container.DataItem.DisplayName %></span>
                                        </a>
                                    </div>
                                    <hr class="separator" id="HRseparator" runat="server" visible="false"/>
                                </asp:View>
                                <asp:View ID="VIWoptionNarrowWideView" runat="server">
                                    <div class="selectoritem first">
                                        <a class="command wide">
                                            <asp:Label ID="LBwideView" runat="server" CssClass="text">*Wide view</asp:Label>
                                        </a>
                                        <a class="command narrow">
                                            <asp:Label ID="LBnarrowView" runat="server" CssClass="text">*Narrow view</asp:Label>
                                        </a>
                                    </div>
                                    <hr class="separator">
                                </asp:View>
                            </asp:MultiView>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</span>
<asp:Literal ID="LTtemplateCustomCssClass" runat="server" Visible="false">command groupedselector nolabel noactive custom</asp:Literal>
<asp:Literal ID="LTtemplateActiveCssClass" runat="server" Visible="false">active</asp:Literal>