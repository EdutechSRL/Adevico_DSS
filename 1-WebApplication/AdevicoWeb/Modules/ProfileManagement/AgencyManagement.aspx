<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="AgencyManagement.aspx.vb" Inherits="Comunita_OnLine.AgencyManagement" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>

<asp:Content ID="Content5" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadContent" runat="server">
        <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201604071200lm" rel="Stylesheet" type="text/css" />
    <script language="Javascript" type="text/javascript">
        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }     
    </script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPimportAgency" runat="server" CssClass="Link_Menu" Text="Import"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPaddAgency" runat="server" CssClass="Link_Menu" Text="Add agency"
            Height="18px" CausesValidation="false"></asp:HyperLink>
    </div>
    <div>
        <asp:MultiView ID="MLVagencies" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWlist" runat="server">
                <div class="AgencyManagementFilters">
                    <div class="fieldobject filters container_12 clearfix">
                        <div class="fieldrow grid_8 alpha namefilter">
                            <asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLsearchBy">Find by:</asp:Label>&nbsp;
                            <asp:DropDownList ID="DDLsearchBy" runat="server" CssClass="fieldinput" Width="130px">

                            </asp:DropDownList>
                            <asp:TextBox ID="TXBvalue" runat="server" MaxLength="300" CssClass="fieldinput" Columns="30"></asp:TextBox>
                        </div>
                        <div class="fieldrow grid_4 omega availabilityfilter">
                            <asp:Label ID="LBagencyAvailability_t" runat="server" CssClass="fieldlabel">Fra</asp:Label>
                            <asp:DropDownList ID="DDLagencyAvailability" runat="server" CssClass="fieldinput" >

                            </asp:DropDownList>
                        </div>
                        <div class="clear"></div>
                        <div class="abcupdate clearfix">
                            <div class="fieldrow left abcfilters" runat="server" id="DVletters"
                                visible="false">
                                <CTRL:AlphabetSelector ID="CTRLalphabetSelector" runat="server" RaiseSelectionEvent="true">
                                </CTRL:AlphabetSelector>
                            </div>
                            <div class="fieldrow right updatefilter">
                                <asp:Button ID="BTNcerca" runat="server" CssClass="linkMenu" Text="*Update" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="tablewrapper">
                    <asp:Repeater ID="RPTagencies" runat="server">
                        <HeaderTemplate>
                            <table class="table light fullwidth">
                                <tr>
                                     <th style="width: 40%;">
                                        <asp:Label ID="LBname_t" runat="server">Name</asp:Label>
                                    </th>
                                    <th style="width: 17%;">
                                        <asp:Label ID="LBexternalCode_t" runat="server">External code</asp:Label>
                                    </th>
                                    <th style="width: 17%;">
                                        <asp:Label ID="LBtaxCode_t" runat="server">Tax code</asp:Label>
                                    </th>
                                    <th style="width: 10%;">
                                        <asp:Label ID="LBnationalCode_t" runat="server">National code</asp:Label>
                                    </th>
                                    <th style="width: 4%;">
                                        <asp:Label ID="LBemployeesNumber_t" runat="server">Employees</asp:Label>
                                    </th>
                                    <th class="actions" style="width: 60px;">
                                        <asp:Label ID="LBagencyActions_t" runat="server">I</asp:Label>
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class='<%#me.BackGroundItem(Container.itemtype,Container.DataItem.Agency.Deleted) %>'>
                                <td style="width: 40%; padding-left: 5px;">
                                    <%#Container.DataItem.Agency.Name%>
                                    <asp:Repeater ID="RPTorganizations" runat="server" DataSource="<%#Container.DataItem.Agency.Organizations%>">
                                        <HeaderTemplate>
                                            <ul>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li> <%#Container.DataItem.Name%></li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                                <td style="width: 17%; padding-left: 5px;">
                                    &nbsp;<%#Container.DataItem.Agency.ExternalCode%>&nbsp;
                                </td>
                                 <td style="width: 17%; padding-left: 5px;">
                                    &nbsp;<%#Container.DataItem.Agency.TaxCode%>&nbsp;
                                </td>
                                <td style="width: 17%; padding-left: 5px;">
                                    <div style="text-align: center;">
                                        &nbsp;<%#Container.DataItem.Agency.NationalCode%>&nbsp;
                                    </div>
                                </td>
                                <td style="width: 2%; text-align: center;">
                                    <div style="text-align: center;">
                                        <%#Container.DataItem.UsedBy%>
                                    </div>
                                </td>
                                <td class="actions" style="width: 60px;">
                                    <span class="icons">
                                        <asp:HyperLink ID="HYPdelete" runat="server" CssClass="icon delete " Visible="false">D</asp:HyperLink>
                                        <asp:LinkButton ID="LNBvirtualDelete" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                        <asp:LinkButton ID="LNBrecover" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                        <asp:HyperLink ID="HYPedit" runat="server" CssClass="icon edit ">E</asp:HyperLink>
                                        <asp:HyperLink ID="HYPinfo" runat="server" CssClass="icon view newWindow" Target="_blank">I</asp:HyperLink>
                                        <asp:Literal ID="LTactions" runat="server" Visible="false">&nbsp;</asp:Literal>
                                        
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
                        <div runat="server" id="DIVpageSize" visible="false">
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
            </asp:View>
            <asp:View ID="VIWerrors" runat="server">
                <div style="padding-top: 180px; padding-bottom: 180px;">
                    <asp:Label ID="LBerrors" runat="server"></asp:Label>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>