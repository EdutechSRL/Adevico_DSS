<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="PublicRequestsCollector.aspx.vb" Inherits="Comunita_OnLine.PublicRequestsCollector" %>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" EnableScripts="false"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="viewbuttons clearfix">
    <asp:HyperLink ID="HYPtoLoginPage" runat="server" Text="List calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
</div>
<asp:MultiView ID="MLVresults" runat="server">
    <asp:View ID="VIWlist" runat="server">
        <div class="pager" runat="server" id="DVpagerTop"  visible="false">
            <asp:literal ID="LTpageTop" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
        </div>
        <div class="list">
            <asp:Repeater id="RPTcalls" runat="server">
                <HeaderTemplate>
                    <ul class="cfps externallist">
                </HeaderTemplate>
                <ItemTemplate>
                     <li class="cfp clearfix" id="call_<%#Container.DataItem.Id %>">
                        <a name="<%#Container.DataItem.Id %>"></a>
                        <div class="externalleft icons">
                            <asp:Label ID="LBlocked" CssClass="icon locked" runat="server" Visible="false">&nbsp;</asp:Label>
                        </div>
                        <div class="cfpcontainer">
                            <div class="cfpheader clearfix">
                                <span class="left">
                                    <span class="titlecont">
                                        <span class="title">
                                            <asp:Literal ID="LTname" runat="server"></asp:Literal>
                                            <asp:Literal ID="LTedition" runat="server" Visible="false"></asp:Literal>
                                        </span>
                                        <span class="icons">
                                            <asp:Label ID="LBnewItem" CssClass="icon hasnew" runat="server" Visible="false">&nbsp;</asp:Label>
                                            <asp:Label ID="LBexpiringItem" CssClass="icon expiring" runat="server" Visible="false">&nbsp;</asp:Label>
                                        </span>
                                    </span>
                                </span>
                                <span class="right">
                                    <!-- icone -->
                                </span>
                            </div>
                            <div class="cfpdesc clearfix">
                                <div class="description left">
                                    <div class="intro">
                                        <asp:literal ID="LTintroCall" runat="server" Text="<%#Container.DataItem.Call.Summary %>"></asp:literal>
                                    </div>
                               </div>
                            </div>
                            <div class="cfpfooter clearfix"> 	
                                <div class="left">
			                        <div class="expiredate">
                                        <asp:Label ID="LBendDateTime_t" class="Label" runat="server">Scadenza:</asp:Label>
			                           	<ul>
			                           		<li><asp:Label ID="LBendDate" runat="server" class="cfpdetails"></asp:Label></li>
			                           	</ul>
			                       </div>
		                       	</div>
                                <div class="right">
                                    <span class="item first">
                                        <asp:HyperLink ID="HTPcallInfo" runat="server"></asp:HyperLink>
			                        </span>
			                        <span class="item last">
                                        <asp:HyperLink ID="HTPsubmitPublicCall" runat="server"></asp:HyperLink>
			                        </span >
		                       </div>
		                    </div>
                            <div class="clearer"></div>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
           
        </div>
        <div class="pager" runat="server" id="DVpagerBottom" visible="false">
            <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
        </div>
    </asp:View>
    <asp:View ID="VIWnoItems" runat="server">
         <br /><br /><br />
         <asp:Label ID="LBnoCalls" runat="server" Visible="false"></asp:Label>
         <br /><br /><br />
    </asp:View>
</asp:MultiView>

</asp:Content>