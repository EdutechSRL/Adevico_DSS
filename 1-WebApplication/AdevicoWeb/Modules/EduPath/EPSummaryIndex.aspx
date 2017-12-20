<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EPSummaryIndex.aspx.vb" Inherits="Comunita_OnLine.EPSummaryIndex" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVsummary">
        <asp:View runat="server" ID="VIWsummary">
            <div class="epsummary epsummary-index">
                <div class="description">
                    <asp:Label runat="server" ID="LBsummaryDescription">**Scelta tipologia di sommario</asp:Label>
                </div>
                <ul class="ballots">
                <asp:Repeater ID="RPTtypes" runat="server">
                    <ItemTemplate>
                        <li class="ballot">
                            <div class="ballotcontent">
                                <a class="" href="#" runat="server" id="AsummaryType">
                                    <asp:Label ID="LBsummaryTypeIcon" runat="server" CssClass="balloticon">&nbsp;</asp:Label>
                                    <asp:Label runat="server" CssClass="ballottext" ID="LBsummaryTypeText"></asp:Label>
                                </a>
                                
                                <div class="ballotdescription">
                                    <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                </ul>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>