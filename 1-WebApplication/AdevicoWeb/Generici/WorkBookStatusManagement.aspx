<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookStatusManagement.aspx.vb" Inherits="Comunita_OnLine.WorkBookStatusManagement" %>

<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right;" align="center">
        <asp:HyperLink ID="HYPaddStatus" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Add status" Height="18px"></asp:HyperLink>
    </div>
    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;">
        <asp:MultiView ID="MLVworkbooks" runat="server" ActiveViewIndex="1">
            <asp:View ID="VIWlist" runat="server">
                <div style="width: 900px; text-align: left; padding-top: 10px; margin: 0px auto;
                    clear: both;">
                    <asp:Label ID="LBdefault_t" runat="server" CssClass="Titolo_campoSmall">Default status:</asp:Label>
                    <asp:Label ID="LBdefault" runat="server"></asp:Label>&nbsp;
                     <asp:Label ID="LBdefaultInfo" runat="server" CssClass="testo_campoSmall"></asp:Label>
                </div> 
                <div style="width: 900px; text-align: center; padding-top: 10px; margin: 0px auto;
                    clear: both;">
                    <asp:Repeater ID="RPTworkbookStatus" runat="server">
                        <HeaderTemplate>
                            <table width="900px" cellpadding="0" cellspacing="0" border="1">
                                <tr class="ROW_header_Small_Center">
                                    <td style="width: 40px;">
                                        <asp:Label ID="LBedit_t" runat="server">M</asp:Label>
                                    </td>
                                    <td style="width: 40px;">
                                        <asp:Label ID="LBaction_t" runat="server">E</asp:Label>
                                    </td>
                                    <td style="width: 720px;">
                                        <asp:Label ID="LBname_t" runat="server">Name</asp:Label>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Label ID="LBcount_t" runat="server"></asp:Label>
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 40px; vertical-align: top; text-align: center; padding-top: 4px;">
                                    <div style="padding-top: 5px;">
                                        <asp:HyperLink ID="HYPedit" runat="server" Target="_self"></asp:HyperLink>
                                    </div>
                                    <br />
                                </td>
                                <td style="width: 40px; vertical-align: top; text-align: center; padding-top: 4px;">
                                    <div style="padding-top: 5px;">
                                        <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="confirmDelete"
                                            CommandArgument='<%#Container.Dataitem.Id %>'></asp:LinkButton>
                                    </div>
                                    <br />
                                </td>
                                <td style="width: 720px; padding-left:10px;">
                                   <div><%#Container.DataItem.Name%></div>
                                   <div>
                                        <asp:Label ID="LBvisible_t" runat="server"></asp:Label>
                                        <asp:Label ID="LBvisibleTo" runat="server"></asp:Label>
                                   </div>
                                </td>
                                <td style="width: 100px;">
                                    <asp:Label ID="LBcount" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;">
                    <div style="text-align: left; width: 50%; float: left;">
                        <div style="text-align: left;" runat="server" id="DIVpageSize">
                            <asp:Label ID="LBpagesize" runat="server" CssClass="Titolo_campoSmall"></asp:Label>&nbsp;
                            <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="25" Selected="True">25</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="100">100</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="text-align: right; width: 50%; float: left;">
                        <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWerrors" runat="server">
                <div style="padding-top: 180px; padding-bottom: 180px;">
                    <asp:Label ID="LBerrors" runat="server"></asp:Label>
                </div>
                <div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
