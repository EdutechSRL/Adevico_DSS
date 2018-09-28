<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="UserPathSummary.aspx.vb" Inherits="Comunita_OnLine.UserPathSummary" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/jquery.anchor.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>
    <link href="../../Content/jquery.treeTable/jquery.treeTable.css" type="text/css" rel="Stylesheet" />
    <script src="../../Scripts/jquery.treeTable.js" type="text/javascript"></script>
    <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>edupath.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript">
        $(function () {
            //$(".treetable").treeTable({ clickableNodeNames: true });

            $(".clearIt").click(function () {
                // $(".clearMe input").val("");
                $(this).parents(".fieldrow").find("input").val("");
            });
            //$("tr").anchor();
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Label runat="server" ID="LBservice">**Path Summary</asp:Label>
    -
    <asp:Label runat="server" ID="LBserviceUserName"></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVpathsummary">
        <asp:View runat="server" ID="VIWpathsummary">
            <div class="DivEpButton">

                <asp:HyperLink ID="HYPlistEduPath" runat="server" Text="**list edu path" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPback" runat="server" Text="**back" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
            </div>
            <div class="pathsummary userpathsummary">
                <div class="fieldobject details">
                    <div class="fieldrow" runat="server" id="DIVothercommunity">
                        <asp:Label runat="server" ID="LBcommunitynameTitle" CssClass="fieldlabel">**Community</asp:Label>
                        <asp:Label runat="server" ID="LBcommunityname">**Community</asp:Label>
                    </div>
                    <div class="fieldrow">
                        <asp:Label runat="server" ID="LBstatusTitle" CssClass="fieldlabel">**Status</asp:Label>

                        <span class="status">
                            <span class="statusitem">
                                <asp:Label CssClass="gray" runat="server" ID="LBnotstarted">**0</asp:Label>
                                <asp:Label CssClass="label" runat="server" ID="LBnotstartedlabel">**non iniziati</asp:Label>
                            </span>
                            <span class="statusitem">
                                <asp:Label CssClass="yellow" runat="server" ID="LBstarted">**0</asp:Label>
                                <asp:Label CssClass="label" runat="server" ID="LBstartedlabel">**iniziati</asp:Label>
                            </span>
                            <span class="statusitem">
                                <asp:Label CssClass="green" runat="server" ID="LBcompleted">**0</asp:Label>
                                <asp:Label CssClass="label" runat="server" ID="LBcompletedlabel">**completati</asp:Label>
                            </span>
                        </span>
                    </div>
                </div>
                <%--<div class="fieldobject filters">
                <div class="fieldrow">
                    
                    <asp:Label runat="server" ID="LBuserfilterTitle" CssClass="fieldlabel">**User</asp:Label>                                        
                    <asp:TextBox runat="server" ID="TXBuserFilter"></asp:TextBox><span class="icons">&nbsp;<span class="icon delete clearIt">&nbsp;</span></span>
                </div>
                <div class="fieldrow">                    
                    <asp:Label runat="server" ID="LBcommunityRoleTitle" CssClass="fieldlabel">**Community Role</asp:Label>
                    <asp:DropDownList runat="server" ID="DDLroleFilter" />
                </div>
                <div class="fieldrow">                    
                    <asp:Button runat="server" ID="BTNupdateFilter" Text="**Update Filter" CssClass="linkMenu" />
                </div>
            </div>--%>
                <asp:Repeater runat="server" ID="RPTusers">
                    <HeaderTemplate>
                        <table class="treetable table light">
                            <thead>
                                <tr>
                                    <th class="name">
                                        <asp:Label runat="server" ID="LBpathheader">**Path name</asp:Label></th>
                                    <th class="status">
                                        <asp:Label runat="server" ID="LBstatusheader">**Status</asp:Label></th>
                                    <th class="completion">
                                        <asp:Label runat="server" ID="LBcompletionheader">**Completion</asp:Label></th>
                                    <th class="startdate">
                                        <asp:Label runat="server" ID="LBstartdateheader">**StartDate</asp:Label></th>
                                    <th class="enddate">
                                        <asp:Label runat="server" ID="LBenddateheader">**EndDate</asp:Label></th>
                                    <th class="deadline">
                                        <asp:Label runat="server" ID="LBdeadlineheader">**Deadline</asp:Label></th>
                                    <th class="actions">
                                        <asp:Label runat="server" ID="LBactionheader">**Actions</asp:Label></th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>

                        <%--<tr class="user <%#Me.Zero(Container.Dataitem.Total) %> expanded <%#Me.ExpandMe(Container.Dataitem.Person.Id) %>" id="user-<%# Container.Dataitem.Person.Id %>">                        
                        <td class="name"><a name="u<%#Container.Dataitem.Person.Id %>"></a><asp:Label runat="server" ID="LBusername">**Username</asp:Label></td>
                        <td class="status">
                          <asp:label CssClass="gray" runat="server" ID="LBnotstarted">**0</asp:label>
                          <asp:label CssClass="yellow" runat="server" ID="LBstarted">**0</asp:label>
                          <asp:Label CssClass="green" runat="server" ID="LBcompleted">**0</asp:Label>                          
                        </td>
                        <td class="completion">&nbsp;</td>
                        <td class="startdate">&nbsp;</td>
                        <td class="enddate">&nbsp;</td>
                        <td class="deadline">&nbsp;</td>
                        <td class="actions">
                        </td>
                    </tr> --%>
                        <asp:Repeater runat="server" ID="RPTpaths">
                            <ItemTemplate>
                                <tr class="path child-of-user-<%# Container.Dataitem.IdPerson %>">
                                    <td class="name"><a name="u<%# Container.Dataitem.IdPerson %>p<%# Container.Dataitem.IdPath %>"></a>
                                        <asp:Label runat="server" ID="LBpathname">**PathName</asp:Label></td>
                                    <td class="status">
                                        <span class="big <%# Me.Status(Container.Dataitem.Status) %>" title="<%# Me.StatusTitle(Container.Dataitem.Status) %>">&nbsp;</span>
                                    </td>
                                    <td class="completion">
                                        <asp:Label runat="server" ID="LBcompletion">**</asp:Label></td>
                                    <td class="startdate">
                                        <asp:Label runat="server" ID="LBstartdate">**---</asp:Label></td>
                                    <td class="enddate">
                                        <asp:Label runat="server" ID="LBenddate">**---</asp:Label></td>
                                    <td class="deadline">
                                        <asp:Label runat="server" ID="LBdeadline">**---</asp:Label></td>
                                    <td class="actions">
                                        <span class="icons">
                                            <%--<span class="icon stats"></span>--%>
                                            <asp:HyperLink runat="server" ID="HYPstats" CssClass="icon stats"></asp:HyperLink>
                                            <asp:HyperLink runat="server" ID="HYPcertificates" CssClass="icon certificate"></asp:HyperLink>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                    </table>
                    </FooterTemplate>
                </asp:Repeater>

            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWerror">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <%--<asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />--%>
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
