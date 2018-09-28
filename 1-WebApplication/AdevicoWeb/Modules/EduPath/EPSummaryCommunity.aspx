<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EPSummaryCommunity.aspx.vb" Inherits="Comunita_OnLine.EPSummaryCommunity" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register Src="~/Modules/EduPath/UC/UC_OrderBy.ascx" TagName="OrderBy" TagPrefix="UC"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>edupath.css?v=201605041410lm" rel="Stylesheet" />
    <script src="../../Jscript/Modules/Common/jquery.treeTable.js" type="text/javascript"></script>
    <link href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" rel="Stylesheet" />
    <style>
    span.btnOrderBy
    {        
        display:inline-block;
        *display:inline;
        zoom:1;
        vertical-align: middle;        
    }
</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVsummary">
        <asp:View runat="server" ID="VIWsummary">
            <div class="epsummary epsummary-organization">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPback" runat="server" Text="**back" CssClass="linkMenu"></asp:HyperLink>
                </div>
                <div class="fieldobject filters">
                    <div class="fieldrow filterorganization" runat="server" id="DIVfilterOrganization">
                        <asp:Label runat="server" ID="LBfilterOrganizationTitle" CssClass="fieldlabel">**Organization</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLfilterOrganization">
                        </asp:DropDownList>
                    </div>
                    <div class="fieldrow filtercommunity">
                        <asp:Label runat="server" ID="LBfilterCommunityTitle" CssClass="fieldlabel">**Community</asp:Label>
                        <asp:TextBox runat="server" ID="TXBfilterCommunity" />
                    </div>
                    <div class="fieldrow filterupdate">
                        <asp:Button runat="server" CssClass="Link_Menu" ID="BTNupdate" Text="**Update" />
                    </div>
                </div>
                <div class="fieldobject details">                    
                    <div class="fieldrow">
                        <span class="fieldlabel">
                            <asp:Label runat="server" ID="LBpaths">**EduPaths</asp:Label>                            
                            <asp:Label runat="server" ID="LBpathsOrganization" Visible="false">**EduPaths in organization</asp:Label>
                            <asp:Label runat="server" ID="LBorgname" CssClass="name" Visible="false">**ABC</asp:Label>                            
                            :
                        </span> 
                        <span class="status">
                            <span class="statusitem">
                                <span class="locked">&nbsp;</span>
                                <asp:Label runat="server" ID="LBlocked">**0</asp:Label>                                
                                <asp:Label runat="server" ID="LBlockedTitle" CssClass="label">**Locked</asp:Label>                                
                                <span class="statusitem">
                                    <span class="bracket">(</span>
                                    <span class="draft">&nbsp;</span>
                                    <asp:Label runat="server" ID="LBdraft">**0</asp:Label>
                                    <asp:Label runat="server" ID="LBdraftTitle" CssClass="label">**draft</asp:Label>
                                    <span class="bracket">)</span>
                                </span>
                            </span>
                            <span class="statusitem separator">/</span>
                            <span class="statusitem">
                                <span class="unlocked">&nbsp;</span>
                                <asp:Label runat="server" ID="LBunlocked">**0</asp:Label>                                
                                <asp:Label runat="server" ID="LBunlockedTitle" CssClass="label">unlocked</asp:Label>                                
                            </span>
                        </span>
                    </div>
                </div>
                <div class="pager top" runat="server" id="DVpagerTop">
                    <asp:Literal ID="LTpageTop" runat="server">**Go to page: </asp:Literal>
                    <CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
                </div>
                <table class="table light epsummary">
                        <thead>
                            <tr>
                                <th class="name">                                    
                                    <asp:Label runat="server" ID="LBcommunityNameHeader">**Community Name</asp:Label>
                                    <UC:OrderBy runat="server" ID="OBcommunityname" OnOnOrderBy="OrderBy" Column="community" /> 
                                </th>
                                <th class="status">                                    
                                    <asp:Label runat="server" ID="LBpathStatusHeader">**Path Status</asp:Label>
                                </th>
                                <th class="actions">                                    
                                    <asp:Label runat="server" ID="LBactionsHeader">**Actions</asp:Label>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                <asp:Repeater runat="server" id="RPTcommunities">
                <ItemTemplate>
                            <tr>
                                <td class="name">                                    
                                    <asp:Label runat="server" ID="LBcommunityName">**Community Name</asp:Label>
                                </td>
                                <td class="status">
                                    <asp:Label runat="server" ID="LBpathStatus">**Path Status</asp:Label>
                                </td>
                                <td class="actions">
                                    <span class="icons">                                        
                                        <asp:HyperLink runat="server" ID="HYPstats" CssClass="icon stats"></asp:HyperLink>
                                    </span>
                                </td>
                            </tr>
                </ItemTemplate>
                </asp:Repeater>
                                </tbody>
            </table>
                <%--<table class="table light epsummary">
                    <thead>
                        <tr>
                            <th class="name">
                                *Community Name
                            </th>
                            <th class="status">
                                *Path Status
                            </th>
                            <th class="actions">
                                *Actions
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="name">
                                *Lorem ipsum dolor sit amet, consectetur adipiscing.
                            </td>
                            <td class="status">
                                *10 locked (1 draft) / 5 unlocked
                            </td>
                            <td class="actions">
                                <span class="icons"><span class="icon stats"></span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="name">
                                *Lorem ipsum dolor sit amet, consectetur adipiscing.
                            </td>
                            <td class="status">
                                *10 locked (1 draft) / 5 unlocked
                            </td>
                            <td class="actions">
                                <span class="icons"><span class="icon stats"></span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="name">
                                *Lorem ipsum dolor sit amet, consectetur adipiscing.
                            </td>
                            <td class="status">
                                *10 locked (1 draft) / 5 unlocked
                            </td>
                            <td class="actions">
                                <span class="icons"><span class="icon stats"></span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="name">
                                *Lorem ipsum dolor sit amet, consectetur adipiscing.
                            </td>
                            <td class="status">
                                *10 locked (1 draft) / 5 unlocked
                            </td>
                            <td class="actions">
                                <span class="icons"><span class="icon stats"></span></span>
                            </td>
                        </tr>
                    </tbody>
                </table>--%>
                <div class="pager bottom" runat="server" id="DVpagerBottom">
                    <asp:Literal ID="LTpageBottom" runat="server">**Go to page: </asp:Literal>
                    <CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWerror">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
