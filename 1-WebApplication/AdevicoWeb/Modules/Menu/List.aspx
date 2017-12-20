<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="List.aspx.vb" Inherits="Comunita_OnLine.MenubarList" %>

<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="COL" TagName="Dialog" Src="~/Modules/EduPath/UC/UCDialog.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="Menu.Common.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right;" align="center" runat="server">
        <asp:HyperLink ID="HYPaddMenuBar" runat="server" CssClass="Link_Menu" Text="Add Menubar"
            Height="18px" CausesValidation="false"></asp:HyperLink>
    </div>
    <div>
        <telerik:RadTabStrip ID="TBSmenubarTypes" runat="server" Align="Justify" Width="100%"
            Height="20px" CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true"
            SelectedIndex="0">
            <Tabs>
                <telerik:RadTab Text="*Portale" Value="Portal">
                </telerik:RadTab>
                <telerik:RadTab Text="*Comunità" Value="GenericCommunity">
                </telerik:RadTab>
                <telerik:RadTab Text="*Amministrazione" Value="PortalAdministration">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
    </div>
    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;">
        <asp:Repeater ID="RPTmenubar" runat="server">
            <HeaderTemplate>
               <%-- <table class="DataGrid_Generica" cellspacing="0" border="1" style="border-collapse: collapse;"
                    rules="all">--%>
                <br />
                <table class="table light" cellpadding="0" cellspacing="0" width="900px">
                    <thead>
                        <tr class="ROW_header_Small_Center">
                            <th>
                                <asp:Label ID="LBname" runat="server">*Name</asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="LBmenubarType" runat="server">*Type</asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="LBstatus" runat="server">*Status</asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="LBinfo" runat="server">*Last edit</asp:Label>
                            </th>
                            <th>
                                &nbsp;
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="<%# me.BackGroundItem(Container.DataItem.MenuBar.Deleted, Container.ItemType)%>">
                    <td>
                        <%#Container.Dataitem.MenuBar.Name%>
                    </td>
                    <td>
                        <asp:Label ID="LBmenubarType" runat="server" CssClass="Testo_campo"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBstatus" runat="server" CssClass="Testo_campo"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBinfo" runat="server" CssClass="Testo_campo"></asp:Label>
                    </td>
                    <td style=" vertical-align:middle; text-align:right;">
                        <span class="icons">
                            <COL:DialogLinkButton runat="server" ID="LNBvirtualDelete" Visible="false" CausesValidation="false" CommandName="virtualDelete" CssClass="icon virtualdelete"></COL:DialogLinkButton>
                            <asp:LinkButton ID="LNBvirtualUndelete" runat="server" CssClass="icon recover" CommandName="virtualUnDelete" Visible="false">U</asp:LinkButton>
                            <COL:DialogLinkButton runat="server" ID="LNBphisicalDelete" Visible="false" CausesValidation="false" CommandName="phisicalDelete" CssClass="icon delete"></COL:DialogLinkButton>
                            <asp:HyperLink ID="HYPeditMenuBar" runat="server" CssClass="icon edit" Text="E" CausesValidation="false"></asp:HyperLink>
                            <COL:DialogLinkButton runat="server" ID="DLBedit" Visible="false" CausesValidation="false" CommandName="edit" CssClass="icon edit"></COL:DialogLinkButton>
                            <asp:LinkButton ID="LNBsetDefault" runat="server" CssClass="icon makedefault" CommandName="setDefault">D</asp:LinkButton>
                            <asp:Label ID="LBisDefault" runat="server" Visible="false" CssClass="icon default"></asp:Label>
                            <asp:HyperLink ID="HYPviewMenuBar" runat="server" CssClass="icon view" Text="v" CausesValidation="false"></asp:HyperLink>
                        </span>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Label ID="LBnoItems" runat="server"></asp:Label>
    </div>
    <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;" id="DVpaging" runat="server">
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
            <asp:Literal ID="LTpage" runat="server">Go to page: </asp:Literal><CTRL:GridPager
                ID="PGgrid" runat="server" EnableQueryString="false"></CTRL:GridPager>
        </div>
    </div>
    <COL:Dialog runat="server" ID="DLGvirtualDeleteMenuBar" DialogClass="mandatoryDial"
        ServerSideCancel="false"></COL:Dialog>
           <COL:Dialog runat="server" ID="DLGdeleteMenuBar" DialogClass="deleteMenubar"
        ServerSideCancel="false"></COL:Dialog>
    <COL:Dialog runat="server" ID="DLGeditActiveMenuBar" DialogClass="editActiveDial"
        ServerSideCancel="false"></COL:Dialog>
</asp:Content>
