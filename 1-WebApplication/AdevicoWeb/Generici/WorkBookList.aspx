<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookList.aspx.vb" Inherits="Comunita_OnLine.WorkBookList" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right;" align="center">
        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Save status" ID="LNBsaveStatus"
            CausesValidation="false" Visible="false"></asp:LinkButton>
        <asp:HyperLink ID="HYPaddWorkBook" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Add workbook" Height="18px"></asp:HyperLink>
    </div>
    <div style="width: 900px; text-align: center; margin: 0,auto; padding-top: 5px; clear: both;"
        align="center">
        <telerik:RadTabStrip ID="TBSdiario" runat="server" Align="Justify" Width="100%" Height="20px"
            CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true">
            <Tabs>
                <telerik:RadTab Text="Personal" Value="1" />
                <telerik:RadTab Text="Community" Value="2" />
                <telerik:RadTab Text="WorkBook Management" Value="4" />
            </Tabs>
        </telerik:RadTabStrip>
    </div>
    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;">
        <asp:MultiView ID="MLVworkbooks" runat="server" ActiveViewIndex="1">
            <asp:View ID="VIWlist" runat="server">
                <div style="text-align: left; padding-top:4px;">
                    <div style="float: left; width: 350px; vertical-align: middle;">
                        <asp:Label ID="LBfilterBy" runat="server" CssClass="Titolo_campoSmall">Filter by</asp:Label>&nbsp;
                        <asp:DropDownList ID="DDLfilterBy" runat="server" CssClass="Testo_campo" AutoPostBack="true">
                            <asp:ListItem Value="1">All communities</asp:ListItem>
                            <asp:ListItem Value="2">Current community</asp:ListItem>
                            <asp:ListItem Value="4">Portal</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="float: left; width: 550px; vertical-align: middle;">
                        <asp:Label ID="LBorderBy" runat="server" CssClass="Titolo_campoSmall">Order by:</asp:Label>&nbsp;
                        <asp:RadioButtonList ID="RBLorderBy" runat="server" CssClass="Testo_campo" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" AutoPostBack="true">
                            <asp:ListItem Value="4" Selected="True">Community</asp:ListItem>
                            <asp:ListItem Value="6">Last change</asp:ListItem>
                            <asp:ListItem Value="3">Status</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div style="width: 900px; text-align: center; padding-top: 10px; margin: 0px auto;
                    clear: both;">
                    <asp:Repeater ID="RPTcommunities" runat="server">
                        <ItemTemplate>
                            <br />
                            <h3>
                                <b>
                                    <asp:Literal ID="LTcommunityName" runat="server"></asp:Literal>
                                    <%-- <asp:LinkButton ID="LNBentra" runat="server" CommandName="enter" CommandArgument='<%#Container.Dataitem.CommunityID %>'
                                        Text='<%#Container.Dataitem.CommunityName %>' Visible="false"></asp:LinkButton>--%>
                            </h3>
                            <asp:Repeater ID="RPTworkBooks" runat="server" DataSource='<%#Container.Dataitem.Items %>'
                                OnItemCommand="RPTworkBooks_ItemCommand">
                                <HeaderTemplate>
                                    <table class="table light" width="900px">
                                        <tr>
                                            <th style="width: 40px;">
                                                <asp:Label ID="LBaction" runat="server">E</asp:Label>
                                            </th>
                                            <th style="width: 40px;">
                                                <asp:Label ID="LBedit" runat="server">M</asp:Label>
                                            </th>
                                            <th style="width: 720px;">
                                                <asp:Label ID="LBworkbook" runat="server">Workbook</asp:Label>
                                            </th>
                                            <th style="width: 100px;">
                                                <asp:Label ID="LBlastEdit_t" runat="server"></asp:Label>
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class='<%# me.BackGroundItem(Container.DataItem)%>'>
                                        <td style="width: 40px; vertical-align: top; text-align: center; padding-top: 4px;">
                                            <div style="padding-top: 5px;">
                                                <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualdelete"
                                                    CausesValidation="false" CommandArgument='<%#Container.Dataitem.Id %>'></asp:LinkButton>
                                                <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                                                    Visible="false" CommandArgument='<%#Container.Dataitem.Id %>'></asp:LinkButton>
                                                <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="confirmDelete"
                                                    CommandArgument='<%#Container.Dataitem.Id %>'></asp:LinkButton>
                                            </div>
                                            <br />
                                        </td>
                                        <td style="width: 40px; vertical-align: top; text-align: center; padding-top: 4px;">
                                            <div style="padding-top: 5px;">
                                                <asp:HyperLink ID="HYPedit" runat="server" Target="_self"></asp:HyperLink>
                                            </div>
                                            <br />
                                        </td>
                                        <td style="width: 720px;">
                                            <div id="DIVtitolo" style="text-align: left; word-wrap:break-word;" runat="server">
                                                <asp:Label ID="LBtitle_t" runat="server" CssClass="Titolo_campoSmall">Title:</asp:Label>&nbsp;
                                                <span class="Testo_campoSmall">
                                                    <asp:HyperLink ID="HYPtitle" runat="server" Target="_self" CssClass="ROW_ItemLink_Small" Visible="false"></asp:HyperLink>
                                                    <asp:Literal ID="LTtitle" runat="server" />
                                                </span>
                                                <asp:Literal ID="LTworkbookID" runat="server" Visible="false" Text='<%#Container.Dataitem.Id %>'></asp:Literal>
                                            </div>
                                            <div style="padding-top: 5px; word-wrap: break-word;">
                                                <asp:Label ID="LBauthors_t" runat="server" CssClass="Titolo_campoSmall">Authors:</asp:Label>&nbsp;
                                                <asp:Label ID="LBauthors" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                            </div>
                                            <div id="DVcommunity" runat="server" style="clear: both; padding: 4px; word-wrap: break-word;">
                                                <asp:Label ID="LBcommunityOwner_t" runat="server" CssClass="Titolo_campoSmall">Community:</asp:Label>
                                                <asp:Label ID="LBcommunityOwner" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                            </div>
                                            <div id="DIVadminPanel" style="background-color:#808080; clear: both; " runat="server">
                                                <div style="float: left; padding-top: 6px; text-align: left; width: 360px;">
                                                    <asp:Label runat="server" ID="LBstatusItem_t" CssClass="Titolo_campoSmall">Status:</asp:Label>&nbsp;
                                                    <asp:DropDownList ID="DDLstatus" runat="server" CssClass="Testo_campoSmall">
                                                        <asp:ListItem Value="3">In attesa di verifica</asp:ListItem>
                                                        <asp:ListItem Value="2">Non approvato</asp:ListItem>
                                                        <asp:ListItem Value="1">Approvato</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="LBstatusItem" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                                </div>
                                                <div style="float: left; vertical-align: middle; text-align: left; padding-top: 5px;
                                                    width: 300px;">
                                                    <asp:Label ID="LBediting_t" runat="server" CssClass="Titolo_campoSmall" >Editing:</asp:Label>
                                                    <asp:DropDownList ID="DDLediting" runat="server" CssClass="Testo_campoSmall">
                                                        <asp:ListItem Text="Only workbook responsible" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="Only author" Value="11"></asp:ListItem>
                                                        <asp:ListItem Text="Only authors" Value="15"></asp:ListItem>
                                                        <asp:ListItem Text="Only workbooks administrators" Value="8"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="LBediting" runat="server" CssClass="Testo_campoSmall" ></asp:Label>
                                                    <asp:Label ID="LBdraft" runat="server"  CssClass="Titolo_campoSmall">**    DRAFT ITEM (VISIBLE ONLY FOR YOU)    **</asp:Label>
                                                </div>
                                            </div>
                                        </td>
                                        <td style="width: 100px;">
                                            <asp:Label ID="LBlastEdit" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
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
