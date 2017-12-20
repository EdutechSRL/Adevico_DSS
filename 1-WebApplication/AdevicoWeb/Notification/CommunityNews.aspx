<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CommunityNews.aspx.vb" Inherits="Comunita_OnLine.CommunityNews" Theme="Materiale" EnableTheming="true"  %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
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
    <div id="Div1" style="width:900px; text-align:right; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
        <asp:HyperLink id="HYPbackHistory" runat="server" EnableViewState="false" CssClass="Link_Menu" Visible="false" Text="Back" Height="18px"></asp:HyperLink>
        <asp:HyperLink id="HYPbackAllNewsRead" runat="server" EnableViewState="false" CssClass="Link_Menu" Visible="false" Text="Back and set all news read" Height="18px"></asp:HyperLink>
    </div>
    <div id="Div3" style="width:900px; text-align:left; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
        <div class="news">
             <h3><asp:Literal ID="LTcommunityName" runat="server" EnableViewState="false"></asp:Literal></h3>
             <h4 class="aligned"><b><asp:Literal ID="LTday" runat="server" EnableViewState="false"></asp:Literal></b></h4>    
             <div class="event">
                <asp:Repeater ID="RPTnewsData" runat="server">
                    <ItemTemplate>
                        <h4>
                            <b><asp:Literal ID="LTmoduleName" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.ModuleName %>'></asp:Literal></b>
                            <asp:LinkButton ID="LNBentra" runat="server" CommandName="enter" CommandArgument='<%#Container.Dataitem.ModuleID %>' Text='<%#Container.Dataitem.ModuleName %>'  Visible="false"></asp:LinkButton>
                        </h4>            
                        <asp:Repeater ID="RPTmoduleNews" runat="server" DataSource='<%#Container.Dataitem.News %>'>
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
                <asp:Repeater ID="RPTallNews" runat="server" Visible="false">       
                   <ItemTemplate>
		                <h3>
                            <asp:Literal ID="LTname" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.Name %>'></asp:Literal>
                        </h3>
                        <asp:Repeater ID="RPTmultiples" runat="server" DataSource='<%#Container.Dataitem.Multiples %>'>
                            <ItemTemplate>
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
             </div>
            <asp:Literal ID="LTnonews" runat="server"></asp:Literal>       
            <div style="width:900px; text-align:right; padding-top:5px; clear:both; height:22px;">
	           <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
            </div>
        </div>
    </div>
     <div id="Div2" style="width:900px; text-align:left; padding-top:5px; margin: 0px auto;  clear:both; display:none;" runat="server" visible="false">
        <div style="text-align:right;" runat="server" id="DIVpageSize">
            <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100" Selected="True">100</asp:ListItem>
                <asp:ListItem Value="150">150</asp:ListItem>
                <asp:ListItem Value="200">200</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>   
</asp:Content>