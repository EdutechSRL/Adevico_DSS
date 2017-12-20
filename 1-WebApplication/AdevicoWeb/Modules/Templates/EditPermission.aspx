<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditPermission.aspx.vb" Inherits="Comunita_OnLine.EditTemplatePermission"%>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Templates/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelectorHeader" Src="~/Modules/Common/UC/UC_ModalCommunitySelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/CallForPapers/callforpapers.js"></script>

     <script language="javascript" type="text/javascript">
         $(function () {
//             $(".ddbuttonlist.enabled").dropdownButtonList();


//             $(".dialog.dialogimport").dialog({
//                 width: 700,
//                 modal: true
//             });

//             $(".dialog.dlgadduser").dialog({
//                 width: 700,
//                 height: 450,
//                 modal: true
//             });

//             $(".dialog.dlgaddcommunity").dialog({
//                 width: 700,
//                 height: 450,
//                 modal: true
//             });

//             $(".dialog.dlgselectroles").dialog({
//                 width: 700,
//                 height: 450,
//                 modal: true
//             });

//             $(".dialog.dlgaddprofile").dialog({
//                 width: 700,
//                 height: 450,
//                 modal: true
//             });

             /*$("table .headercheckbox input[type='checkbox']").change(function(){
             var $checkbox=$(this);
             var $table=$checkbox.parents("div.tablewrapper table");
             var ischecked=$checkbox.is(":checked");
             var $rows = $table.children("tbody").children("tr").find(".permissioncheckbox input[type='checkbox']:not(:disabled)").attr("checked",ischecked);
             });*/

             $("table .permissioncheckbox input[type='checkbox']").change(function () {
                 var $checkbox = $(this);
                 var $table = $checkbox.parents("div.tablewrapper table");
                 var checked = $table.find(".permissioncheckbox input[type='checkbox']:checked").size();
                 var total = $table.find(".permissioncheckbox input[type='checkbox']").size();

                 if (total != checked) {
                     $table.find(".headercheckbox input[type='checkbox']").attr("checked", false);
                 } else {
                     $table.find(".headercheckbox input[type='checkbox']").attr("checked", true);
                 }

             });

             $(".table th input[type='checkbox']").change(function () {
                 var $this = $(this);
                 $(this).parents("table").first().find("td.check input[type='checkbox']:not(:disabled)").prop("checked", $this.is(":checked"));

                 var $el;
                 var $elout;

                 var ultrafast = 1;
                 var fast = 200;
                 var slow = 3000;

                 if ($this.is(":checked")) {
                     $el = $this.siblings(".selectorpopup.checkall");
                     $elout = $this.siblings(".selectorpopup.checknone");
                 } else {
                     $el = $this.siblings(".selectorpopup.checknone");
                     $elout = $this.siblings(".selectorpopup.checkall");
                 }

                 if ($el.size() > 0) {
                     $el.fadeIn(fast).addClass("open");
                     var ovt = setTimeout(function () { $el.fadeOut(fast, function () { $el.removeClass("open"); }); clearTimeout(ovt); }, slow);
                 }
                 if ($elout.size() > 0) {
                     $elout.fadeOut(ultrafast, function () { $elout.removeClass("open"); });
                 }

             });

             $(".addprofile").hover(function () {
                 $("ul.navlist li.nav.profile").switchClass("", "highlighted", 100);
             }, function () {
                 $("ul.navlist li.nav.profile").switchClass("highlighted", "", 500);
             });
             $(".addcommunity").hover(function () {
                 $("ul.navlist li.nav.community").switchClass("", "highlighted", 100);
             }, function () {
                 $("ul.navlist li.nav.community").switchClass("highlighted", "", 500);
             });
             $(".adduser").hover(function () {
                 $("ul.navlist li.nav.user").switchClass("", "highlighted", 100);
             }, function () {
                 $("ul.navlist li.nav.user").switchClass("highlighted", "", 500);
             });

//             $(".addprofile").click(function () {
//                 $(".dlgaddprofile").dialog("open");
//                 return false;
//             });
//             $(".addcommunity").click(function () {
//                 $(".dlgaddcommunity").dialog("open");
//                 return false;
//             });
//             $(".adduser, span.adduser").click(function () {
//                 $(".dlgadduser").dialog("open");
//                 return false;
//             });
//             $(".selectroles").click(function () {
//                 $(".dlgselectroles").dialog("open");
//                 return false;
//             });

             $("table.expandable").each(function () {
                 var $table = $(this);
                 var max = $table.data("max") - 1;



                 $table.find("tbody > tr").removeClass("hidden");
                 $table.find("tbody > tr:gt(" + max + ")").addClass("hidden");

                 var $showextra = $table.find("tfoot .showextra:not(.first)");
                 var $hideextra = $table.find("tfoot .hideextra:not(.last)");

                 if ($showextra.size() > 0) {
                     $showextra.html($showextra.html().replace("{0}", $table.find("tbody > tr").size()));
                 }
                 if ($hideextra.size() > 0) {
                     $hideextra.html($hideextra.html().replace("{0}", max + 1));
                 }

                 if ($table.find("tbody > tr").size() <= max) {
                     $table.find("tfoot").hide();
                 } else {
                     $table.find("tfoot").show();
                 }

             });

             $("table.expandable tfoot .showextra").click(function () {
                 var $table = $(this).parents("table.expandable").first();
                 $table.removeClass("compressed");
             });

             $("table.expandable tfoot .hideextra").click(function () {
                 var $table = $(this).parents("table.expandable").first();
                 $table.addClass("compressed");
             });

             $("table.table th .selectall").click(function () {
                 var $table = $(this).parents("table.table").first();
                 var $th = $(this).parents("th").first();

                 var datacol = $th.data("col");
                 $table.find("td[data-col='" + datacol + "'] input[type='checkbox']:not(:disabled)").prop("checked", true);
             });

             /*$("table.table th .selectnone").click(function(){
             var $table =$(this).parents("table.table").first();
             var $th = $(this).parents("th").first();
             var thclass = $th.attr("class").trim().replace(new RegExp(" ", 'g'),".");
             $table.find("td."+thclass+" input[type='checkbox']:not(:disabled)").prop("checked",false);
             });*/

             $("table.table th .selectnone").click(function () {
                 var $table = $(this).parents("table.table").first();
                 var $th = $(this).parents("th").first();

                 var datacol = $th.data("col");
                 $table.find("td[data-col='" + datacol + "'] input[type='checkbox']:not(:disabled)").prop("checked", false);
             });

             $("table.table td .selectall").click(function () {
                 //var $table =$(this).parents("table.table").first();
                 var $tr = $(this).parents("tr").first();
                 $tr.find("td:not(.check) input[type='checkbox']").prop("checked", true);
             });

             $("table.table td .selectnone").click(function () {
                 //var $table =$(this).parents("table.table").first();
                 var $tr = $(this).parents("tr").first();
                 $tr.find("td:not(.check) input[type='checkbox']").prop("checked", false);
             });

             $(".editing-on").click(function () {
                 $(this).parents(".userselection").removeClass("default-state").addClass("editing");
                 return false;
             });
         });
         $(document).ready(function () {
             $(".view-modal.view-users").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 890,
                 height: 450,
                 minHeight: 300,
                 minWidth: 700,
                 title: '<%=DialogTitleTranslation(5) %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });

             $(".view-modal.view-profiletypes").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 700,
                 height: 400,
                 minHeight: 300,
                 minWidth: 500,
                 title: '<%=DialogTitleTranslation(4) %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });
             $(".view-modal.view-roles").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 700,
                 height: 400,
                 minHeight: 300,
                 minWidth: 500,
                 title: '<%=DialogTitleTranslation(3) %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });
         });
    </script>
    <CTRL:CommunitySelectorHeader id="CTRLcommunitySelectorHeader"  runat="server" LoadCss="true" LoadScripts="true" Width="940" Height="650" MinHeight="500" MinWidth="750" SelectionMode="Multiple"></CTRL:CommunitySelectorHeader>
    <CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Literal id="LTdataMax" runat="server" Visible="false">5</asp:Literal>
    <asp:MultiView id="MLVpermissions" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
             <asp:Label ID="LBnoTemplate" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWpermissions" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNsavePermissionsTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                 <!-- @Start Call Availability -->
                                <ul class="userselections">
                                    <li class="userselection toolbar" id="LIaddToolbar" runat="server" visible="false">
                                        <div class="userselectioncontent">
                                            <div class="title clearfix">
                                                <div class="description">
                                                    <p><asp:Literal ID="LTtoolDescription" runat="server">*In questa sezione è possibile gestire le modalità di accesso al bando.</asp:Literal></p>
                                                    <ul class="navlist">
                                                        <li class="nav community" id="LIaddCommunity" runat="server" visible="false"><strong><asp:literal ID="LTaddCommunity" runat="server" Text="*Aggiungi comunità"></asp:literal></strong> <span><asp:literal ID="LTaddCommunityDescription" runat="server" Text=""></asp:literal></span></li>
                                                        <li class="nav user" id="LIaddUserFromCommunity" runat="server" visible="false"><strong><asp:literal ID="LTaddUserFromCommunity" runat="server" Text="*Aggiungi Utente"></asp:literal></strong> <span><asp:literal ID="LTaddUserFromCommunityDescription" runat="server" Text=""></asp:literal></span></li>
                                                        <li class="nav profile" id="LIaddProfile" runat="server" visible="false"><strong><asp:literal ID="LTaddProfile" runat="server" Text="*Aggiungi Profilo"></asp:literal></strong> <span><asp:literal ID="LTaddProfileDescription" runat="server" Text=""></asp:literal></span></li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="footer" style="text-align: center">
                                                <asp:Button ID="BTNaddCommunity" runat="server" Text="*Aggiungi Comunità" CssClass="Link_Menu addcommunity" Visible="false" />
                                                <asp:Button ID="BTNaddUserFromCommunity" runat="server" Text="*Aggiungi Utente" CssClass="Link_Menu adduser" Visible="false" />
                                                <asp:Button ID="BTNaddProfile" runat="server" Text="*Aggiungi Profilo" CssClass="Link_Menu addprofile" Visible="false" />
                                            </div>
                                        </div>
                                    </li>
                                    <asp:Repeater id="RPTcommunityAssignments" runat="server">
                                        <ItemTemplate>
                                            <li class="userselection <%#CssAssignmentItem(Container.DataItem.IsDefault)%>">
                                                <a name="#assignment_<%#Container.DataItem.Id %>_"></a>
                                                <div class="userselectioncontent">
                                                    <div class="title clearfix">
                                                        <div class="left">
                                                            <asp:Label ID="LBdisplayName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBdisplayName"></asp:Label>
                                                            <asp:Label ID="LBdisplayName" runat="server" CssClass="communityname inputtext" Text='<%#Container.DataItem.DisplayName %>'></asp:Label>
                                                            <asp:literal ID="LTidAssignment" runat="server" Text='<%#Container.DataItem.Id %>' Visible="false"></asp:literal>
                                                        </div>
                                                        <div class="right">
                                                            <span class="icons">  
                                                                <asp:Button ID="BTNeditCommunityAssignment" runat="server" CssClass="icon edit selectroles" CommandName="edititem" />
                                                                <asp:Button ID="BTNdeleteCommunityAssignment" runat="server" CssClass="icon delete needconfirm" CommandName="deleteitem" Visible="false"/>
                                                            </span>
                                                        </div>
                                                        <div class="clearer"></div>
                                                    </div>
                                                    <div class="footer">
                                                        <div class="fieldobject permissions">
                                                            <div class="fieldrow" runat="server" id="DVmodulePermissionsTitle" visible="false">
                                                                <asp:Label ID="LBcommunityItemModulePermissions_t" runat="server" CssClass="fieldlabel">*Permissions</asp:Label>
                                                            </div>
                                                            <div class="fieldrow" runat="server" id="DVmodulePermissions" visible="false">
                                                                <label class="" for=""> 
                                                                    <asp:Literal ID="LTcommunityItemModulePermissionInfo" runat="server">/Rendi disponibile ai gestori del servizio</asp:Literal>
                                                                    <span class="icons"><asp:Label ID="LBinfoModuleRoles" cssclass="icon infoalt" runat="server"></asp:Label></span>
                                                                </label>
                                                                <asp:CheckBox ID="CBXuse" runat="server" /><asp:Label ID="LBpermissionUse" runat="server" AssociatedControlID="CBXuse" CssClass="alignr">*Use</asp:Label>
                                                                <asp:CheckBox ID="CBXedit" runat="server" /><asp:Label ID="LBpermissionEdit" runat="server" AssociatedControlID="CBXedit" CssClass="alignr">*Edit</asp:Label>
                                                                <asp:CheckBox ID="CBXclone" runat="server" /><asp:Label ID="LBpermissionClone" runat="server" AssociatedControlID="CBXclone" CssClass="alignr">*Clone</asp:Label>
                                                                <asp:CheckBox ID="CBXpermissions" runat="server" /><asp:Label ID="LBpermissionChange" runat="server" AssociatedControlID="CBXpermissions" CssClass="alignr">*Usa</asp:Label>
                                                            </div>
                                                            <div class="fieldrow">
                                                                <div class="tablewrapper" id="DVtable" runat="server">
                                                                    <table class="table light permissions fullwidth">
                                                                        <thead>
															                <tr>
																                <th class="roleuser">
                                                                                    <asp:Label ID="LBroleHeader_t" CssClass="text" runat="server">*Role/ProfileType</asp:Label>
																                </th>
																                <th class="permission read" data-col="read">
                                                                                    <asp:Label ID="LBpermissionUseHeader_t" CssClass="text" runat="server">*Use</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBuseSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBuseSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
																                <th class="permission write" data-col="write">
																	                <asp:Label ID="LBpermissionEditHeader_t" CssClass="text" runat="server">*Edit</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBeditSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBeditSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
																                <th class="permission clone" data-col="clone">
																	                <asp:Label ID="LBpermissionCloneHeader_t" CssClass="text" runat="server">*Clone</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBcloneSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBcloneSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
																                <th class="permission permissionedit" data-col="permissionedit">
																	                <asp:Label ID="LBpermissionChangeHeader_t" CssClass="text" runat="server">*Change permission</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBpermissionChangeSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBpermissionChangeSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                 <asp:Repeater ID="RPTselectedItems" runat="server" OnItemDataBound="RPTselectedItemss_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <tr class="role">
																            <td class="roleuser">
																	            <%#Container.DataItem.DisplayName %>
                                                                                <asp:Literal ID="LTidRole" runat="server" Visible="false"></asp:Literal>
                                                                                <asp:Literal ID="LTidCommunity" runat="server" Visible="false"></asp:Literal>
                                                                                <asp:Literal ID="LTidPersonType" runat="server" Visible="false"></asp:Literal>
                                                                                <asp:Literal ID="LTidAssignment" runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                                                                                <span class="right icons">
                                                                                    <asp:Label ID="LBroleSelectAll" runat="server" CssClass="icon selectall" Visible='<%# Container.DataItem.AllowEdit %>'>&nbsp;</asp:Label><asp:Label ID="LBroleSelectNone" runat="server" CssClass="icon selectnone" Visible='<%# Container.DataItem.AllowEdit %>'>&nbsp;</asp:Label>
																	            </span>
																            </td>
																            <td class="permission read" data-col="read">
																	            <asp:CheckBox ID="CBXuse" runat="server" Checked='<%#Container.DataItem.Use %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																            </td>
																            <td class="permission write" data-col="write">
																	            <asp:CheckBox ID="CBXedit" runat="server" Checked='<%#Container.DataItem.Edit %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																            </td>
																            <td class="permission clone" data-col="clone">
																	            <asp:CheckBox ID="CBXclone" runat="server" Checked='<%#Container.DataItem.Clone %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																            </td>
																            <td class="permission permissionedit" data-col="permissionedit">
																	            <asp:CheckBox ID="CBXpermissions" runat="server" Checked='<%#Container.DataItem.ChangePermission %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																            </td>
                                                                        </tr>
														            </ItemTemplate>
                                                                </asp:Repeater>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Repeater ID="RPTusersAssignments" runat="server">
                                        <HeaderTemplate>
                                            <li class="userselection usersummary default-state">
                                                <div class="userselectioncontent">
                                                    <div class="title clearfix">
                                                    <div class="left">
                                                        <asp:Label ID="LBuserListTitle" runat="server" CssClass="fieldlabel">*Users</asp:Label>
                                                    </div>
                                                    <div class="right">
                                                        <span class="actions">
                                                            <span class="action first">
                                                                <asp:LinkButton ID="LNBmanageUsers" Text="*Manage users" runat="server" CssClass="editing-on hideonedit" CommandName="manage"></asp:LinkButton>
												            </span>
												            <span class="action last">
                                                                <asp:LinkButton ID="LNBsaveUsers" Text="*Save" runat="server" CssClass="exit-editing showonedit" CommandName="save"></asp:LinkButton>
												            </span>
                                                        </span>
                                                    </div>
                                                    <div class="clearer"></div>
                                                </div>
                                                    <div class="footer">
                                                        <div class="fieldobject users">
											                <div class="fieldrow">
											                    <div class="tablewrapper">
                                                                    <table class="table light users fullwidth expandable compressed" data-max="5">
                                                                        <thead>
                                                                            <tr>
                                                                                <th class="check showonedit">
																                    <span class="headercheckbox">
																	                    <input type="checkbox" name="" id="hdr-chb"> 
                                                                                    </span>
                                                                                </th>
                                                                                <th class="username"><span class="text"><asp:Literal ID="LTusername_t" runat="server"></asp:Literal></span></th>
                                                                                <th class="permission read hideonedit">
                                                                                    <asp:Label ID="LBpermissionUseHeader_t" CssClass="text" runat="server">*Use</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBuseSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBuseSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
																                <th class="permission write hideonedit" data-col="write">
																	                <asp:Label ID="LBpermissionEditHeader_t" CssClass="text" runat="server">*Edit</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBeditSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBeditSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
																                <th class="permission clone hideonedit" data-col="clone">
																	                <asp:Label ID="LBpermissionCloneHeader_t" CssClass="text" runat="server">*Clone</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBcloneSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBcloneSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
																                <th class="permission permissionedit hideonedit" data-col="permissionedit">
																	                <asp:Label ID="LBpermissionChangeHeader_t" CssClass="text" runat="server">*Change permission</asp:Label>
																	                <span class="icons">
                                                                                        <asp:Label ID="LBpermissionChangeSelectAll" runat="server" CssClass="icon selectall">&nbsp;</asp:Label><asp:Label ID="LBpermissionChangeSelectNone" runat="server" CssClass="icon selectnone">&nbsp;</asp:Label>
																	                </span>
																                </th>
                                                                                
                                                                            </tr>
                                                                        </thead>
                                                                    <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="check showonedit">
                                                                    <span class="submittercheckbox">
                                                                        <input type="checkbox" runat="server" id="CBXuser" enabled='<%# Container.DataItem.AllowEdit %>'/>
                                                                        <asp:Literal ID="LTidUser" runat="server" Visible="false" Text='<%#Container.DataItem.IdPerson %>'></asp:Literal>
                                                                    </span>
                                                                </td>
                                                                <td class="username">
																	<%#Container.DataItem.DisplayName %>
                                                                    <asp:Literal ID="LTidPerson" runat="server" Visible="false" Text='<%#Container.DataItem.IdPerson %>'></asp:Literal>
                                                                    <asp:Literal ID="LTidAssignment" runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                                                                    <span class="right icons hideonedit">
                                                                        <asp:Label ID="LBuserSelectAll" runat="server" CssClass="icon selectall" Visible='<%# Container.DataItem.AllowEdit %>'>&nbsp;</asp:Label><asp:Label ID="LBuserSelectNone" runat="server" CssClass="icon selectnone" Visible='<%# Container.DataItem.AllowEdit %>'>&nbsp;</asp:Label>
																	</span>
																</td>
																<td class="permission read hideonedit" data-col="read">
																	<asp:CheckBox ID="CBXuse" runat="server" Checked='<%#Container.DataItem.Use %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																</td>
																<td class="permission write hideonedit" data-col="write">
																	<asp:CheckBox ID="CBXedit" runat="server" Checked='<%#Container.DataItem.Edit %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																</td>
																<td class="permission clone hideonedit" data-col="clone">
																	<asp:CheckBox ID="CBXclone" runat="server" Checked='<%#Container.DataItem.Clone %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																</td>
																<td class="permission permissionedit hideonedit" data-col="permissionedit">
																	<asp:CheckBox ID="CBXpermissions" runat="server" Checked='<%#Container.DataItem.ChangePermission %>' Enabled='<%# Container.DataItem.AllowEdit %>' />
																</td>
                                                            </tr>                                                   
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                            </tbody>
                                                            <tfoot>
                                                                <tr id="TRfooter" runat="server">
                                                                    <td colspan="6">
                                                                        <asp:Label ID="LBshowextraUserItems" CssClass="showextra" runat="server"></asp:Label>
                                                                        <asp:Label ID="LBhideextraUserItems" CssClass="hideextra" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </tfoot>
                                                        </table>
                                                    </div>
                                                </div>
                                            </li>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="footer">
                                <div class="DivEpButton">
                                    <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                    <asp:button ID="BTNsavePermissionsBottom" runat="server" Text="Save"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="view-modal view-profiletypes" id="DVselectPersonTypes" runat="server" visible="false">
                <div class="fieldobject fielddescription">
                    <div class="fieldrow">
                        <asp:Label ID="LBpersonTypesDescription" runat="server" CssClass="description">*Seleziona le tipologie di profilo per cui assegnare i permessi:</asp:Label>
                    </div>
                </div>
                <asp:CheckBoxList ID="CBLprofileTypes" runat="server" CssClass="radiobuttontable"  RepeatColumns="4" RepeatLayout="Table" RepeatDirection="Horizontal">
                </asp:CheckBoxList>
                <div class="fieldobject clearfix">
                    <div class="fieldrow right">
                        <asp:Button ID="BTNclosePersonTypesAssignments" runat="server" CssClass="Link_Menu" />
                        <asp:Button ID="BTNsavePersonTypesAssignments" runat="server" CssClass="Link_Menu" />
                    </div>
                </div>
            </div>
            <div class="view-modal view-roles" id="DVselectRoles" runat="server" visible="false">
                <div class="fieldobject">
                    <div class="fieldrow">&nbsp;</div>
                </div>
                <div class="fieldobject fielddescription">
                    <div class="fieldrow">
                        <asp:Label ID="LBselectRolesDescription" runat="server" CssClass="description">*Seleziona a quali ruoli della comunità personalizzare i permessi:</asp:Label>
                    </div>
                </div>
                <div class="fieldobject">
                    <div class="fieldrow">
                        <asp:Label ID="LBcommunityName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcommunityName"></asp:Label>
                        <asp:Label ID="LBcommunityName" runat="server" CssClass="communityname" AssociatedControlID=""></asp:Label>
                    </div>
                </div>
                <asp:CheckBoxList ID="CBLroles" runat="server" CssClass="radiobuttontable"  RepeatColumns="4" RepeatLayout="Table" RepeatDirection="Horizontal">
                </asp:CheckBoxList>
                <div class="fieldobject clearfix">
                    <div class="fieldrow right">
                        <asp:Button ID="BTNcloseRoleAssignments" runat="server" CssClass="Link_Menu" />
                        <asp:Button ID="BTNsaveRoleAssignments" runat="server" CssClass="Link_Menu" />
                    </div>
                </div>
            </div>
            <CTRL:CommunitySelector id="CTRLaddCommunity" runat="server" visible="false" SelectionMode="Multiple"></CTRL:CommunitySelector>
            <div class="view-modal view-users" id="DVselectUsers" runat="server" visible="false">
                 <CTRL:SelectUsers ID="CLTRselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
                  DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="false" DefaultMaxPreviewItems="10" 
                  ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false"/>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>