<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityLastUpdates.aspx.vb" Inherits="Comunita_OnLine.CommunityLastUpdates"
    Theme="Materiale" EnableTheming="true" %>

<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("ul.tabs>li").filter(":not(.unselectable)").click(function() {
                $(this).siblings("li").removeClass("current");
                $(this).addClass("current");
            });

        });
    </script>

    <style type="text/css">
        div.RadTabStrip
        {
            border-bottom-style: solid;
            border-bottom-width: 1px;
            background-image: url('30x1_White.gif');
            background-position: left;
            background-repeat: repeat-x;
        }
        ul.rtsUL
        {
            border-bottom-style: none;
            background-position: 100%;
        }
    </style>
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <asp:HyperLink ID="HYPbackHistory" runat="server" EnableViewState="false" CssClass="Link_Menu"
            Visible="false" Text="Back" Height="18px"></asp:HyperLink>
    </div>
    <div id="Div3" style="width: 900px; text-align: left; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <div style="width: 900px; text-align: left;">
            <div style="float: left;">
                <telerik:RadTabStrip ID="TBSlastUpdates" runat="server" Height="20px" CausesValidation="False"
                    AutoPostBack="True" Skin="Outlook" SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Today And Yesterday" Value="1"></telerik:RadTab>
                        <telerik:RadTab Text="Last Week" Value="2"></telerik:RadTab>
                        <telerik:RadTab Text="Last Month" Value="3"></telerik:RadTab>
                        <telerik:RadTab Text="All" Value="4" ></telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </div>
            <div style="float: left; height: 20px; width: 450px; border-bottom-color: Gray; border-bottom-width: 1px;
                border-bottom-style: solid; display: none;">
                <%--<hr style=" display:inline block; vertical-align:bottom;"/>--%>
            </div>
        </div>
        <br /><br />
        <div class="news">
            <asp:Literal ID="LTdays" runat="server" EnableViewState="false"></asp:Literal>
            <div runat="server" id="DVfilters" visible="false">
                <br />
                <asp:Literal ID="LTfilter" runat="server">Filtra per:</asp:Literal>
                <asp:DropDownList ID="DDLservices" runat="server" AutoPostBack="true"></asp:DropDownList>
                <br />
                <asp:RadioButtonList ID="RBLview" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical" AutoPostBack="true" Visible="false">
                    <asp:ListItem Text="View by community" Value="1"></asp:ListItem>
                    <asp:ListItem Text="View by data" Value="2" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <br class="clear" />
            <div class="event">
                <asp:Repeater ID="RPTnewsData" runat="server">
                    <ItemTemplate>
                        <h3>
                            <asp:Literal ID="LTmoduleName" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.ModuleName %>'></asp:Literal>
                            <asp:LinkButton ID="LNBentra" runat="server" CommandName="enter" CommandArgument='<%#Container.Dataitem.ModuleID %>'
                                Text='<%#Container.Dataitem.ModuleName %>' Visible="false"></asp:LinkButton>
                        </h3>
                        <asp:Repeater ID="RPTmoduleNews" runat="server" DataSource='<%#Container.Dataitem.News %>'>
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:Literal ID="LTuniqueID" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.UniqueID %>' Visible="false"></asp:Literal>
                                    <asp:Literal ID="LTsentDate" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.SentDate %>'  Visible="false"></asp:Literal>
                                    <asp:Literal ID="LTmessage" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.Message %>'></asp:Literal>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="RPTallNews" runat="server" Visible="false">       
                   <ItemTemplate>
		                <h3>
                            <asp:Literal ID="LTname" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.Name %>'></asp:Literal>
                            <asp:LinkButton ID="LNBentra" runat="server" CommandName="enter" CommandArgument='<%#Container.Dataitem.ID %>' Text='<%#Container.Dataitem.Name %>' Visible="false"></asp:LinkButton>
                        </h3>
                        <asp:Repeater ID="RPTmultiples" runat="server" DataSource='<%#Container.Dataitem.Multiples %>'>
                            <ItemTemplate>
                                <h4>
                                    <asp:Literal ID="LTname" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.Name %>'></asp:Literal>
                                    <asp:LinkButton ID="LNBentra" runat="server" CommandName="enter" CommandArgument='<%#Container.Dataitem.ID %>' Text='<%#Container.Dataitem.Name %>' Visible="false"></asp:LinkButton>
                                </h4>
                                <asp:Repeater ID="RPTmultipleNews" runat="server" DataSource='<%#Container.Dataitem.News %>'>
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:Literal ID="LTuniqueID" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.UniqueID %>' Visible="false"></asp:Literal>
                                            <asp:Literal ID="LTsentDate" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.SentDate %>' Visible="false"></asp:Literal>
                                            <asp:Literal ID="LTmessage" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.Message %>'></asp:Literal>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="LTnonews" runat="server"></asp:Literal>
            </div>
            <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;">
                <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
            </div>
        </div>
    </div>
    <div id="Div2" style="width: 900px; text-align: left; padding-top: 5px; margin: 0px auto;
        clear: both; display: none;" runat="server" visible="false">
        <div style="padding-top: 5px;">
            <asp:Literal ID="LTsearch" runat="server" EnableViewState="false" Text="Name or Surname:"></asp:Literal>
            <asp:TextBox ID="TXBsearch" runat="server" Columns="50" MaxLength="60" SkinID="find"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="BTNsearch" runat="server" CssClass="Link_Menu" UseSubmitBehavior="true" />
        </div>
        <div style="text-align: right;" runat="server" id="DIVpageSize">
            <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50" Selected="True">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="150">150</asp:ListItem>
                <asp:ListItem Value="200">200</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
</asp:Content>
