<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileCommunities.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProfileCommunities" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../../UC/UC_PagerControl.ascx" %>
<asp:MultiView ID="MLVinfo" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWdefault" runat="server">
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <div class="InfoContent">
            <%--style="width: 100%; text-align: right; padding-top: 5px; clear: both; height: 22px;"--%>
            <div class="ComListContainer">
                <div style="text-align:right;">
                    <asp:RadioButtonList ID="RBLstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
                <div class="ComList">
                    <asp:Repeater ID="RPTsubscriptions" runat="server">
                        <HeaderTemplate>
                        <table class="ComTable" cellpadding="0" cellspacing="0" border="1px" width="700px"><thead><tr class="ROW_header_Small_Center">
                        <%--<ul class="Com_List">
                            <li class="Header">--%>
                                <td style="width: 420px;">
                                    <asp:Label ID="LBname_t" runat="server"></asp:Label>
                                </td>
                                <td style="width: 140px;">
                                    <asp:Label ID="LBsubscription_t" runat="server"></asp:Label>
                                </td>
                                <td style="width: 140px;">
                                    <asp:Label ID="LBlastVisit_t" runat="server"></asp:Label>
                                </td>
                            <%--</li>--%>
                            </tr></thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="ROW_Normal_Small"><td>
                                <asp:Label ID="LBname" runat="server" CssClass="Col1"></asp:Label>
                            </td><td>
                                <asp:Label ID="LBsubscription" runat="server" CssClass="Col2"></asp:Label>
                            </td><td>
                                <asp:Label ID="LBlastVisit" runat="server" CssClass="Col3"></asp:Label>
                            </td></tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <%--style="text-align: left; width: 50%; float: left;"--%>
                
                <div class="ComPage" runat="server" id="DIVpageSize" visible="false">
                    <asp:Label ID="LBpagesize" runat="server" CssClass="Titolo_campoSmall"></asp:Label>&nbsp;
                    <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="15">15</asp:ListItem>
                        <asp:ListItem Value="25" Selected="True">25</asp:ListItem>
                        <asp:ListItem Value="50">50</asp:ListItem>
                        <asp:ListItem Value="100">100</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <%--style="text-align: right; width: 50%; float: left;"--%>
                <div class="ComPager">
                    <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false"
                        Visible="false"></CTRL:GridPager>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>