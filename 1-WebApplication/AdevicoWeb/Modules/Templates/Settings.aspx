<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Settings.aspx.vb" Inherits="Comunita_OnLine.NotificationSettings" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Templates/UC/UC_TemplateSelector.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />

<%--        <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>--%>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>

    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.persist-min.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("table.treetable").treeTable({ clickableNodeNames: true, persist: true, persistStoreName: "<%=PersistStoreName %>", treeColumn: 1 });
            $(".dialog").dialog({ autoOpen: false });
            $("span.icon.showdesc").click(function () {
                var html = $(this).parents("td").first().find("div.description").html();
                $(".dlgdescription").find("div.description").html(html);
                $(".dlgdescription").dialog("open");
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
     <asp:MultiView ID="MLVcontent" runat="server">
        <asp:View ID="VIWdefault" runat="server">
            <h3><asp:Literal ID="LTdefaultMessage" runat="server"></asp:Literal></h3>
        </asp:View>
        <asp:View ID="VIWcontent" runat="server">
            <div class="viewbuttons clearfix" id="DVsaveSettingsUp" runat="server">
                <asp:LinkButton ID="LNBsaveSettingsUp" runat="server" CssClass="Link_Menu" Text="*Save settings"></asp:LinkButton>
            </div>
            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
            <div class="tablewrapper">
                <table class="table fullwidth light treetable templatenotificationsettings">
                    <thead>
                        <tr>
                            <th class="warning">&nbsp;</th>
                            <th class="name"><asp:Literal ID="LTmoduleNameHeader_t" runat="server">*Name</asp:Literal></th>
                            <th class="template"><asp:Literal ID="LTtemplateNameHeader_t" runat="server">*Template</asp:Literal></th>
                        </tr>
                    </thead>
                <asp:Repeater ID="RPTmodules" runat="server">
                    <HeaderTemplate>
                    <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="srv-<%#Container.DataItem.Id %>" class="service">
                            <td class="warning">
                                <span class="icons">
                                    <asp:Label ID="LBmoduleErrors" runat="server" CssClass="icon showdesc " Visible="false">*error message</asp:Label>
                                </span>
                                <div class="invisible description">
                                    <asp:Literal ID="LTmoduleErrors" runat="server"></asp:Literal>
                                </div>
                            </td>
                            <td class="name">
                                <%#Container.DataItem.ModuleName %><asp:literal ID="LTmoduleCode" runat="server" Visible="false" Text='<%#Container.DataItem.ModuleCode %>'></asp:literal>
                            </td>
                            <td class="template">
                                <span class="info left">
                                    <span class="eventstat">
                                        <span class="eventsenabled"><%#Container.DataItem.EventsEnabled %></span>
                                        <asp:label ID="LBeventsEnabled" runat="server" CssClass="desc">*events enabled</asp:label>
                                        <span class="sep">/</span>
                                        <span class="eventstotal"><%#Container.DataItem.EventsTotal%></span>
                                        <asp:label ID="LBeventsTotal" runat="server" CssClass="desc">*total</asp:label>
                                    </span>
                                </span>
                            </td>
                        </tr>
                        <asp:Repeater ID="RPTmoduleEvents" runat="server" DataSource="<%#Container.DataItem.Events %>" OnItemCommand="RPTmoduleEvents_ItemCommand" OnItemDataBound="RPTmoduleEvents_ItemDataBound">
                            <ItemTemplate>
                                <tr id="srv-<%#Container.DataItem.IdModule %>-evnt-<%#Container.DataItem.IdEvent %>" class="event child-of-srv-<%#Container.DataItem.IdModule %>">
                                    <td class="warning">
                                        <span class="icons">
                                            <asp:Label ID="LBmoduleEventError" runat="server" CssClass="icon showdesc " Visible="false">*error message</asp:Label>
                                        </span>
                                        <div class="invisible description">
                                            <asp:Literal ID="LTmoduleEventError" runat="server"></asp:Literal>
                                        </div>
                                    </td>
                                    <td class="name">
                                        <%#Container.DataItem.Name%>
                                        <span class="icons">
                                            <asp:Label ID="LBmandatoryAction" runat="server" Text="" Visible='<%#Container.DataItem.IsMandatory %>' CssClass="icon mandatory"></asp:Label>
                                        </span>                                       
                                        <asp:literal ID="LTidEvent" runat="server" Visible="false" Text='<%#Container.DataItem.IdEvent %>'></asp:literal>
                                        <asp:literal ID="LTisMandatory" runat="server" Visible="false" Text='<%#Container.DataItem.IsMandatory %>'></asp:literal>
                                    </td>
                                    <td class="template">
                                        <span class="selector left">
                                            <CTRL:Selector id="CTRLselector" runat="server" OnTemplateSelected="TemplateSelected" RaiseSelectionEvent="true" ></CTRL:Selector>
                                        </span>
                                        <span class="icons right">
                                            
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td colspan="2">
                                <asp:Label ID="LBemptyNotificationSettingItems" runat="server">*No module actions</asp:Label>
                            </td>
                        </tr>
                    </tbody>
                    </FooterTemplate>
                </asp:Repeater>
                </table>
            </div>
            <div class="dialog dlgdescription" title="<%=DialogTitle() %>">
                <div class="fieldobject">
                    <div class="fieldrow">
                        <div class="description">description...</div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <asp:Literal id="LTiconsBaseCssClass" runat="server" Visible="false">icon showdesc </asp:Literal>
</asp:Content>