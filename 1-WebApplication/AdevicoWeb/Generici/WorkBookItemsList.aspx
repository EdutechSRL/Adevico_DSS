<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookItemsList.aspx.vb" Inherits="Comunita_OnLine.WorkBookItemsList" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;">
        <div style="height: 20px; float: left; text-align: left; width: 420px">
            <asp:Label ID="LBorderby_t" runat="server" CssClass="Titolo_CampoSmall">Order by:&nbsp;</asp:Label>
            <asp:RadioButtonList ID="RBLorderby" RepeatDirection="Horizontal" CssClass="FiltroCampoSmall"
                RepeatLayout="Flow" runat="server" AutoPostBack="True">
                <asp:ListItem Value="0" Selected="True">From First to Last</asp:ListItem>
                <asp:ListItem Value="1">From Last to First</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div style="height: 20px; float: left; text-align: right; width: 480px;">
            <asp:HyperLink ID="HYPgoToWorkbooksList" runat="server" CssClass="Link_Menu"
                Visible="false" Text="Go to workbooks" Height="18px"></asp:HyperLink>
            <asp:HyperLink ID="HYPprintItems" runat="server" CssClass="Link_Menu"
                Visible="false" Text="Print Items" Height="18px" Target="_blank"></asp:HyperLink>
            <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Delete items" ID="LNBdeleteItems"
                CausesValidation="false" Visible="false"></asp:LinkButton>
            <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Save status" ID="LNBsaveStatus"
                CausesValidation="false" Visible="false"></asp:LinkButton>
            <asp:HyperLink ID="HYPaddItem" runat="server" CssClass="Link_Menu"
                Visible="false" Text="Add item" Height="18px"></asp:HyperLink>
            <asp:HyperLink ID="HYPbackToItemsList" runat="server" CssClass="Link_Menu"
                Visible="false" Text="Back to items" Height="18px"></asp:HyperLink>
        </div>
    </div>
    <asp:MultiView ID="MLVitems" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWitems" runat="server">
            <br />
            <div style="width: 900px; text-align: center; border: 1px; border-color: Black; border-style: solid;
                clear: both;">
                <div style="width: 900px; text-align: center; height: 24px;" class="DiarioLezioni_DGheader">
                    <asp:Label ID="LBtitle" runat="server">Items</asp:Label>
                </div>
                <table width="900px" cellpadding="0" cellspacing="0" border="0">
                    <asp:Repeater ID="RPTitemsDetails" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td scope="row" width="900px" align="left" class="DiarioLezioni_HeaderRigaLezione_Bold"
                                    valign="middle">
                                    <div style="clear: both; width: 900px;">
                                        <div style="float: left; padding-left: 5px; width: 22px; text-align: left; height: 20px;
                                            vertical-align: middle;">
                                            <input type="checkbox" value='<%# Container.DataItem.Item.ID.tostring()%>'
                                                id="CBXselected" runat="server" />
                                            <asp:Literal ID="LTitemID" runat="server" Visible="false" Text='<%# Container.DataItem.Item.ID.tostring()%>'></asp:Literal>
                                        </div>
                                        <div style="float: left; padding-left: 10px; padding-top: 2px; width: 582px; text-align: left;
                                            height: 20px; vertical-align: middle;">
                                            <a name="<%# Container.DataItem.Item.ID.tostring() %>">&nbsp;</a><asp:Literal ID="LTitemHeader"
                                                runat="server" ></asp:Literal>
                                        </div>
                                        <div style="float: left; padding-right: 5px; padding-top: 2px; text-align: right;
                                            width: 180px; height: 20px; vertical-align: middle;">
                                            <asp:HyperLink ID="HYPitemFiles" runat="server" Target="_self" CssClass="DiarioLezioni_HeaderLink">Gestione materiale</asp:HyperLink>
                                        </div>
                                        <div style="float: left; padding-right: 5px; padding-top: 2px; text-align: right;
                                            width: 80px; height: 20px; vertical-align: middle;">
                                            <asp:HyperLink ID="HYPedit" runat="server" Target="_self" CssClass="DiarioLezioni_HeaderLink">Gestione materiale</asp:HyperLink>
                                            <asp:ImageButton runat="server" ID="IMGundelete" CommandName="undelete" CommandArgument='<%# Container.DataItem.Item.ID%>'
                                                ImageAlign="AbsMiddle"></asp:ImageButton>
                                            <asp:ImageButton runat="server" ID="IMGvirtualDelete" CommandName="virtualdelete"
                                                CommandArgument='<%# Container.DataItem.Item.ID%>' ImageAlign="AbsMiddle"></asp:ImageButton>
                                            <asp:ImageButton visdible="false" runat="server" ID="IMGdelete" CommandName="confirmdelete"
                                                CommandArgument='<%# Container.DataItem.Item.ID%>' ImageAlign="AbsMiddle"></asp:ImageButton>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td width="900px" class='<%# me.BackGroundItem(Container.DataItem.Item)%>'>
                                    <div id="DIVtitolo" style="padding: 5px; text-align: left; width: 890px; word-wrap: break-word;"
                                        runat="server">
                                        <asp:Label runat="server" ID="LBtitolo_t" CssClass="Titolo_campoSmall">Titolo:</asp:Label>
                                        <span class="Testo_campoSmall">
                                            <%# Container.DataItem.Item.Title%></span>
                                    </div>
                                    <div id="DIVtext" style="padding: 0px 5px 5px 5px; text-align: left;" runat="server">
                                        <asp:Label runat="server" ID="LBprogramma_t" CssClass="Titolo_campoSmall">Programma:</asp:Label>
                                        <span class="Testo_campoSmall Editor">
                                            <%# Container.DataItem.Item.Body%></span>
                                    </div>
                                    <div id="DIVnote" style="padding: 0px 5px 5px 5px; text-align: left;" runat="server">
                                        <asp:Label runat="server" ID="LBnote_t" CssClass="Titolo_campoSmall">Note personali:</asp:Label>
                                        <span class="Testo_campoSmall">
                                            <asp:Literal ID="LTnote" runat="server" ></asp:Literal></span>
                                    </div>
                                    <div id="DIVmateriale" style="padding: 0px 5px 5px 5px; text-align: left;" runat="server">
                                        <div style="float: left; vertical-align: text-top; text-align: left;">
                                            <asp:Label runat="server" ID="LBmateriale_t" CssClass="Titolo_campoSmall">Materiale:</asp:Label>&nbsp;
                                        </div>
                                        <div style="float: left; vertical-align: text-top; text-align: left; width: 770px;">
                                                <asp:Repeater ID="RPTitemFiles" runat="server">
                                                    <HeaderTemplate>
                                                        <ul>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <li style="display:inline; width:770px; vertical-align: text-top;">
                                                            <asp:Label ID="LBnomeFile" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                                            <asp:HyperLink runat="server" ID="HYPfile" CssClass="ROW_ItemLink_Small" Visible="False"
                                                                Target="_blank"></asp:HyperLink>
                                                            <asp:Label ID="LBdimensione" runat="server" CssClass="Testo_campoSmall"></asp:Label>&nbsp;
                                                            <asp:HyperLink runat="server" ID="HYPdownload" Target="_blank" CssClass="ROW_ItemLink_Small"
                                                                Visible="False">Download</asp:HyperLink>
                                                            <asp:ImageButton ID="IMBcontenutoAttivo" runat="server" Visible="false" CausesValidation="false"
                                                                CommandName="prova" />
                                                            <asp:HyperLink runat="server" ID="HYPcontenutoAttivo" CssClass="ROW_ItemLink_Small"
                                                                Visible="false"></asp:HyperLink>
                                                                <br />
                                                        </li>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                      </ul>
                                                    </FooterTemplate>
                                                </asp:Repeater>                                          
                                        </div>
                                    </div>
                                    <div id="DIVitemSeparator" style="padding: 0px 5px 5px 5px; text-align: left; clear: both;"
                                        runat="server">
                                        &nbsp;&nbsp;&nbsp;
                                    </div>
                                    <div id="DIVadminPanel" style="padding: 0px 5px 5px 5px; text-align: left; clear: both;
                                        background-color: Beige; height: 25px; vertical-align: middle;" runat="server">
                                        <div style="float: left; padding-top: 6px; text-align: left; width:60px;">
                                            <asp:Label runat="server" ID="LBstatusItem_t" CssClass="Titolo_campoSmall">Status:</asp:Label>&nbsp;
                                        </div>
                                        <div style="float: left; vertical-align: middle; text-align: left; padding-top: 5px; width:470px;">
                                            <asp:DropDownList ID="DDLstatus" runat="server" CssClass="Testo_campoSmall">
                                                <asp:ListItem Value="3">In attesa di verifica</asp:ListItem>
                                                <asp:ListItem Value="2">Non approvato</asp:ListItem>
                                                <asp:ListItem Value="1">Approvato</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="LBstatusItem" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                        </div>
                                        <div style="float: left; vertical-align: middle; text-align: left; padding-top: 5px; width:355px;">
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
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </asp:View>
        <asp:View ID="VIWerrors" runat="server">
            <asp:Label ID="LBerrors" runat="server" ></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
