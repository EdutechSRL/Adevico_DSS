<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="Resources.aspx.vb" Inherits="Comunita_OnLine.ProjectResources" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/ProjectManagement/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ManageProjectResources" Src="~/Modules/ProjectManagement/UC/UC_ManageProjectResources.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AddExternalResource" Src="~/Modules/ProjectManagement/UC/UC_AddExternalResource.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RemoveResource" Src="~/Modules/ProjectManagement/UC/UC_ConfirmResourceToRemove.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProjectManagement/Css/ProjectManagement.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>
    <script language="javascript" type="text/javascript">
//         $(function () {
////             $(".ddbuttonlist.enabled").dropdownButtonList();


//             $("table.expandable").each(function () {
//                 var $table = $(this);
//                 var max = $table.data("max") - 1;
//                 $table.find("tbody > tr").removeClass("hidden");
//                 $table.find("tbody > tr:gt(" + max + ")").addClass("hidden");

//                 var $showextra = $table.find("tfoot .showextra:not(.first)");
//                 var $hideextra = $table.find("tfoot .hideextra:not(.last)");

//                 if ($showextra.size() > 0) {
//                     $showextra.html($showextra.html().replace("{0}", $table.find("tbody > tr").size()));
//                 }
//                 if ($hideextra.size() > 0) {
//                     $hideextra.html($hideextra.html().replace("{0}", max + 1));
//                 }

//                 if ($table.find("tbody > tr").size() <= max) {
//                     $table.find("tfoot").hide();
//                 } else {
//                     $table.find("tfoot").show();
//                 }
//             });

//             $("table.expandable tfoot .showextra").click(function () {
//                 var $table = $(this).parents("table.expandable").first();
//                 $table.removeClass("compressed");
//             });

//             $("table.expandable tfoot .hideextra").click(function () {
//                 var $table = $(this).parents("table.expandable").first();
//                 $table.addClass("compressed");
//             });

//             $(".expandlistwrapper:visible:not(.initialized)").each(function () {
//                 InitializeExpandList($(this));
//             });

//             function InitializeExpandList(el) {
//                 if (!el.is(".initialized")) {
//                     el.addClass("initialized");
//                     var $children = el.find("ul.expandlist");
//                     var $content = $children.children().wrapAll('<div class="overflow">');

//                     //$children.wrapInner('<div class="clearfix" />');
//                     var delta = 3;

//                     var $el = el.find("div.overflow");
//                     var HasOverflow = $children.height() + delta < $el.height();

//                     if (!HasOverflow) {
//                         el.addClass("disabled");
//                         el.removeClass("compressed");
//                     } else {
//                         el.removeClass("disabled");
//                     }
//                 }
//             }

//             $(".expandlistwrapper .command.expand").click(function () {
//                 $(this).parents(".expandlistwrapper").first().removeClass("compressed");
//                 return false;
//             });

//             $(".expandlistwrapper .command.compress").click(function () {
//                 $(this).parents(".expandlistwrapper").first().addClass("compressed");
//                 return false;
//             });
//         });

         $(document).ready(function () {
             $(".view-modal.view-users-internal").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 890,
                 height: 600,
                 minHeight: 300,
                 minWidth: 700,
                 title: '<%=InternalDialogTitleTranslation() %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });

             $(".view-modal.view-users-external").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 600,
                 height: 500,
                 minHeight: 300,
                 minWidth: 400,
                 title: '<%=ExternalDialogTitleTranslation() %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });
             $(".view-modal.view-removeresource").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 600,
                 height: 300,
                 minHeight: 200,
                 minWidth: 200,
                 title: '<%=RemoveResourceDialogTitleTranslation() %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });
             
         });
    </script>
    <CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBnoResources" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:Button ID="BTNsaveResourcesTop" runat="server" Text="*Save" Visible="false" />
                                <asp:HyperLink ID="HYPgoToProjectMapTop" class="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                            </div>
                            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content clearfix">
                                <div class="fieldobject toolbar clearfix">
                                    <div class="fieldrow right">
                                        <div class="ddbuttonlist enabled" id="DVaddResource" runat="server" visible="false"><!--
                                            --><asp:LinkButton ID="LNBaddUsersFromCommunity" runat="server" Text="*Add User" CssClass="linkMenu ddbutton" /><!--
                                            --><asp:LinkButton ID="LNBaddCustomUser" runat="server" Text="*Add Custom" CssClass="linkMenu ddbutton" /><!--
                                            --><asp:LinkButton ID="LNBaddCustomUsers" runat="server" Text="*Add Custom" CssClass="linkMenu ddbutton" /><!--
                                        --></div>
                                    </div>
                                </div>
                                <ul class="userselections">
                                    <li class="userselection usersummary userspool">
                                        <div class="userselectioncontent">
                                            <div class="title clearfix">
                                                <div class="left">
                                                    <asp:Label ID="LBresourceListTitle" runat="server" CssClass="title">*Users</asp:Label>
                                                </div>
                                                <div class="right">
                                                    <span class="icons">
                                                    </span>
                                                </div>
                                                <div class="clearer">
                                                </div>
                                            </div>
                                            <div class="footer">
                                                <!--Options-->
                                                <CTRL:ManageProjectResources id="CTRLmanageResources" runat="server" DisplayCommands="false"></CTRL:ManageProjectResources>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:Button ID="BTNsaveResourcesBottom" runat="server" Text="*Save" Visible="false" />
                                <asp:HyperLink ID="HYPgoToProjectMapBottom" class="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsBottom" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="view-modal view-users-internal" id="DVselectUsers" runat="server" visible="false">
                <CTRL:SelectUsers ID="CLTRselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
                    DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True" DefaultMaxPreviewItems="5"
                    ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false" />
            </div>
            <div class="view-modal view-users-external" id="DVaddExternalResources" runat="server" visible="false">
                <CTRL:AddExternalResource id="CTRLaddExternalResources" runat="server" Rows="10" DisplayDescription="true" RaiseCommandEvents="true"></CTRL:AddExternalResource>
            </div>
            <div class="view-modal view-removeresource" id="DVremoveResources" runat="server" visible="false">
                <CTRL:RemoveResource id="CTRLconfirmRemoveResource" runat="server" DisplayDescription="true" RaiseCommandEvents="true" DisplayCommands="true" AllowDelete="true" ></CTRL:RemoveResource>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>