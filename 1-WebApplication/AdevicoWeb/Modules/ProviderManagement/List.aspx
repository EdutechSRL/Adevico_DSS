<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master"
    CodeBehind="List.aspx.vb" Inherits="Comunita_OnLine.ListAuthenticationProvider" %>

<%@ Register TagPrefix="CTRL" TagName="InfoProvider" Src="UC/UC_InfoProvider.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   <link href="../../Graphics/Modules/ProviderManagement/Css/ProviderManagement.css" type="text/css" rel="stylesheet" />
    <script language="Javascript" type="text/javascript">
        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }     
        $(document).ready(function () {
            $(".view-modal.view-providerinfo").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 750,
                height: 550,
                minHeight: 400,
                minWidth: 200,
                title: '<%=TranslateModalView("ProviderInfo")%>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
         });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPaddProvider" runat="server" CssClass="Link_Menu" Text="Add provider" Height="18px" CausesValidation="false"></asp:HyperLink>
    </div>
    <asp:MultiView ID="MLVprofiles" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWlist" runat="server">
            <div id="DVprovidersTable">
                <div class="tablewrapper">
                    <asp:Repeater ID="RPTproviders" runat="server">
                        <HeaderTemplate>
                            <table class="table light fullwidth" cellpadding="0" cellspacing="0">
                                <tr>
                                    <th style="width: 460px;">
                                        <asp:Label ID="LBproviderName_t" runat="server">Name</asp:Label>
                                    </th>
                                    <th style="width: 220px;">
                                        <asp:Label ID="LBproviderType_t" runat="server">Type</asp:Label>
                                    </th>
                                    <th style="width: 80px;">
                                        <asp:Label ID="LBproviderIsEnabled_t" runat="server">Enabled</asp:Label>
                                    </th>
                                    <th style="width: 80px;">
                                        <asp:Label ID="LBproviderUsedBy_t" runat="server">Used by</asp:Label>
                                    </th>
                                        <th style="width: 60px;">
                                        <asp:Label ID="LBactions_t" runat="server"></asp:Label>
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class='<%#me.BackGroundItem(Container.DataItem.Provider.Deleted,Container.itemType) %>'>
                                <td style="width: 540px; padding-left: 5px;">
                                    <a name="<%# Container.DataItem.ID.tostring() %>"></a>
                                    <asp:Label ID="LBproviderName" runat="server" CssClass="itemname"></asp:Label>
                                    <div id="DVsettings" runat="server" visible="false" class="settings">
                                        [<asp:HyperLink ID="HYPadvancedSettings" runat="server" CssClass="ROW_ItemLink_Small"></asp:HyperLink>]
                                    </div>
                                </td>
                                <td style="width: 220px; text-align: center;">
                                    <asp:Label ID="LBproviderType" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="LBproviderIsEnabled" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="LBproviderUsedBy" runat="server"></asp:Label>
                                </td>
                                <td style="width: 60px; text-align:right;" class="actions">
                                    <span class="icons">
                                        <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CommandArgument='<%#Container.DataItem.Id %>'
                                        CssClass="icon delete needconfirm" Visible="false">&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBvirtualUnDelete" runat="server" CommandName="undelete" CommandArgument='<%#Container.DataItem.Id %>'
                                            CssClass="icon recover" Visible="false">&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualDelete"
                                            CommandArgument='<%#Container.DataItem.Id %>' CssClass="icon virtualdelete needconfirm"
                                            Visible="false">&nbsp;</asp:LinkButton>
                                        <asp:HyperLink ID="HYPedit" runat="server" CssClass="icon edit">&nbsp;</asp:HyperLink>
                                        <asp:LinkButton ID="LNBproviderInfo" runat="server" Visible="false" CommandName="infoProvider"
                                        CommandArgument='<%#Container.DataItem.Id %>' CssClass="icon view" OnClientClick="return onUpdating();">&nbsp;</asp:LinkButton>
                                    </span>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div class="fieldobject clearfix"> 
                    <div class="fieldrow left">
                        <div id="DVpageSize" runat="server">
                            <asp:Label ID="LBpagesize" runat="server" CssClass="Titolo_campoSmall"></asp:Label>&nbsp;
                            <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="25" Selected="True">25</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="100">100</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="fieldrow right">
                        <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false"
                            Visible="false"></CTRL:GridPager>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="view-modal view-providerinfo" id="DVproviderInfo" runat="server" visible="false">
        <div class="fieldobject fielddescription">
            <div class="fieldrow">
                <asp:Label ID="LBproviderDisplayInfoDescription" runat="server" CssClass="description">*Dettagli sistema di autenticazione:</asp:Label>
            </div>
        </div>
        <CTRL:InfoProvider ID="CTRLinfoProvider" runat="server" />
        <div class="fieldobject clearfix">
            <div class="fieldrow right">
                <asp:Button ID="BTNcloseProviderDisplayInfo" runat="server" CssClass="Link_Menu" />
            </div>
        </div>
    </div>
</asp:Content>