<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditCallAvailability.aspx.vb" Inherits="Comunita_OnLine.EditCallAvailability"  %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelectorHeader" Src="~/Modules/Common/UC/UC_ModalCommunitySelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>

<%@ Register Src="~/Modules/SkinManagement/UC/UC_ModuleSkins.ascx" TagName="CTRLmoduleSkin" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersFromCall" Src="~/Modules/CallForPapers/UC/UC_SelectUsersFromCall.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
     <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>

     <script language="javascript" type="text/javascript">
         $(function () {
             $("fieldset.expandable").blockableFieldset({
                 blockedClass: "disabled"
             });
             //            $(".dialog.dlgselectroles").dialog({
             //                width: 700,
             //                height: 450,
             //                modal: true
             //            });
             $("table .headercheckbox input[type='checkbox']").change(function () {
                 var $checkbox = $(this);
                 var $table = $checkbox.parents("div.tablewrapper table");
                 var ischecked = $checkbox.is(":checked");
                 var $rows = $table.children("tbody").children("tr").find(".submittercheckbox input[type='checkbox']").attr("checked", ischecked);
             });

             $("table .submittercheckbox input[type='checkbox']").change(function () {
                 var $checkbox = $(this);
                 var $table = $checkbox.parents("div.tablewrapper table");
                 var checked = $table.find(".submittercheckbox input[type='checkbox']:checked").size();
                 var total = $table.find(".submittercheckbox input[type='checkbox']").size();

                 if (total != checked) {
                     $table.find(".headercheckbox input[type='checkbox']").attr("checked", false);
                 } else {
                     $table.find(".headercheckbox input[type='checkbox']").attr("checked", true);
                 }

             });

             $(".table th input[type='checkbox']").change(function () {
                 var $this = $(this);
                 $(this).parents("table").first().find("td input[type='checkbox']").prop("checked", $this.is(":checked"));

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

             $(".addcfp").hover(function () {
                 $("ul.navlist li.nav.cfp").switchClass("", "highlighted", 100);
             }, function () {
                 $("ul.navlist li.nav.cfp").switchClass("highlighted", "", 500);
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

             $(".expandlistwrapper:visible:not(.initialized)").each(function () {
                 InitializeExpandList($(this));
             });

             function InitializeExpandList(el) {
                 if (!el.is(".initialized")) {
                     el.addClass("initialized");
                     var $children = el.find("ul.expandlist");
                     var $content = $children.children().wrapAll('<div class="overflow">');

                     //$children.wrapInner('<div class="clearfix" />');
                     var delta = 3;

                     var $el = el.find("div.overflow");
                     var HasOverflow = $children.height() + delta < $el.height();

                     if (!HasOverflow) {
                         el.addClass("disabled");
                         el.removeClass("compressed");
                     } else {
                         el.removeClass("disabled");
                     }
                 }
             }

             $(".expandlistwrapper .command.expand").click(function () {
                 $(this).parents(".expandlistwrapper").first().removeClass("compressed");
                 return false;
             });

             $(".expandlistwrapper .command.compress").click(function () {
                 $(this).parents(".expandlistwrapper").first().addClass("compressed");
                 return false;
             });

         });
        $(document).ready(function () {
            $(".view-modal.view-users").dialog({
                appendTo:"form",
                closeOnEscape: false,
                modal: true,
                width: 890,
                height: 450,
                minHeight: 300,
                minWidth: 700,
                title: '<%=DialogTitleTranslation(2) %>',
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
                title: '<%=DialogTitleTranslation(3) %>',
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
                title: '<%=DialogTitleTranslation(1) %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });

            $(".view-modal.view-call").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 850,
                height: 650,
                minHeight: 500,
                minWidth: 750,
                title: '<%=DialogTitleTranslation(4) %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
         });
    </script>
    <CTRL:CommunitySelectorHeader id="CTRLcommunitySelectorHeader"  runat="server"  LoadCss="true" LoadScripts="true" Width="940" Height="650" MinHeight="500" MinWidth="750" SelectionMode="Multiple"></CTRL:CommunitySelectorHeader>
    <CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Literal id="LTdataMax" runat="server" Visible="false">5</asp:Literal>
    <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnocalls" runat="server"></asp:Label>
            <br /><br /><br /><br />
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
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallTop" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveAvailabilityTop" runat="server" Text="Save"/>
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
                                                        <li class="nav cfp" id="LIaddUserFromCall" runat="server" visible="false"><strong><asp:literal ID="LTaddUserFromCall" runat="server" Text="*Aggiungi da Bando"></asp:literal></strong> <span><asp:literal ID="LTaddUserFromCallDescription" runat="server" Text=""></asp:literal></span></li>
                                                        <li class="nav profile" id="LIaddProfile" runat="server" visible="false"><strong><asp:literal ID="LTaddProfile" runat="server" Text="*Aggiungi Profilo"></asp:literal></strong> <span><asp:literal ID="LTaddProfileDescription" runat="server" Text=""></asp:literal></span></li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="footer" style="text-align: center">
                                                <asp:Button ID="BTNaddCommunity" runat="server" Text="*Aggiungi Comunità" CssClass="Link_Menu addcommunity" Visible="false" />
                                                <asp:Button ID="BTNaddUserFromCommunity" runat="server" Text="*Aggiungi Utente" CssClass="Link_Menu adduser" Visible="false" />
                                                <asp:Button ID="BTNaddUserFromCall" runat="server" Text="*Aggiungi da Bando" CssClass="Link_Menu addcfp" Visible="false" />
                                                <asp:Button ID="BTNaddProfile" runat="server" Text="*Aggiungi Profilo" CssClass="Link_Menu addprofile" Visible="false" />
                                            </div>
                                        </div>
                                    </li>
                                    <li class="userselection publicaccess">
                                        <a name="#public"></a>
                                        <div class="userselectioncontent">
                                            <div class="title clearfix">
                                                <div class="left">
                                                    <asp:Label ID="LBisPublic_t" CssClass="fieldlabel" runat="server" AssociatedControlID="CBXisPublic">Public:</asp:Label>
                                                    <asp:CheckBox ID="CBXisPublic" CssClass="input" runat="server" />
                                                    <asp:Label ID="LBisPublicInfo" CssClass="inline" runat="server" AssociatedControlID="CBXisPublic"></asp:Label>
                                                </div>
                                                <div class="right"></div>
                                                <div class="clearer"></div>
                                            </div>
                                            <div class="footer">
                                                <div class="fieldobject">
                                                    <div class="fieldrow" id="DVpublicAccess" runat="server">
                                                        <asp:Label ID="LBpublicUrlInfo_t" CssClass="Titolo_Campo fieldlabel" runat="server">Url:</asp:Label>
                                                        <ul class="urllist">
                                                            <li class="urlgroup" runat="server" id="LIpublicListUrl">
                                                                 <div class="description">
                                                                    <asp:Literal ID="LTpublicListUrlMessage" runat="server"></asp:Literal>
                                                                 </div>
                                                                 <div class="url clearfix">
                                                                     <span class="left"><asp:Literal ID="LTpublicListUrl" runat="server"></asp:Literal></span>
                                                                 </div>
                                                             </li>
                                                             <li class="urlgroup">
                                                                 <div class="description">
                                                                    <asp:Literal ID="LTpublicCallDirectUrlMessage" runat="server"></asp:Literal>
                                                                 </div>
                                                                 <div class="url clearfix">
                                                                     <span class="left"><asp:Literal ID="LTpublicCallDirectUrl" runat="server"></asp:Literal></span>
                                                                 </div>
                                                             </li>
                                                        </ul>
                                                    </div> 
                                                    <div class="fieldrow" id="DVmoduleSkin" runat="server" visible="false">
                                                        <ctrl:CTRLmoduleSkin id="CTRLmoduleSkin" runat="server"></ctrl:CTRLmoduleSkin>
                                                    </div>
                                                </div>
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
                                                            <asp:Label ID="LBdisplayName" runat="server" CssClass="communityname" Text='<%#Container.DataItem.DisplayName %>'></asp:Label>
                                                            <asp:literal ID="LTidAssignment" runat="server" Text='<%#Container.DataItem.Id %>' Visible="false"></asp:literal>
                                                        </div>
                                                        <div class="right">
                                                            <span class="icons">  
                                                                <asp:Button ID="BTNeditCallCommunityAssignment" runat="server" CssClass="icon edit" CommandName="edititem" />
                                                                <asp:Button ID="BTNdeleteCallCommunityAssignment" runat="server" CssClass="icon delete needconfirm" CommandName="deleteitem" Visible="false"/>
                                                            </span>
                                                        </div>
                                                        <div class="clearer"></div>
                                                    </div>
                                                    <div class="footer">
                                                        <div class="fieldobject">
                                                            <div class="fieldrow">
                                                                <asp:Label ID="LBselectedItems_t" runat="server" class="fieldlabel" AssociatedControlID="RPTselectedItems">*</asp:Label>
                                                                <asp:Label ID="LBallSelectedItemsInfo" runat="server" Visible="false"></asp:Label>
                                                                <div class="inlinewrapper">
                                                                    <div class="communityroles expandlistwrapper compressed">
                                                                        <ul class="communityroles expandlist" id="ULitems" runat="server">
                                                                    <asp:Repeater ID="RPTselectedItems" runat="server" Visible="false">
                                                                        <ItemTemplate>
                                                                                <li class="communityrole expanditem"><%#Container.DataItem.DisplayName %></li>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                    </ul>
                                                                    <div class="commands">
                                                                        <span class="command expand"><asp:Label ID="LBshowAllSelectedItems" runat="server">...</asp:Label></span>
                                                                        <span class="command compress"><asp:Label ID="LBhideAllSelectedItems" runat="server">^</asp:Label></span>
                                                                    </div>
                                                                    <div class="clear"></div>
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
                                            <li class="userselection usersummary">
                                                <div class="userselectioncontent">
                                                    <div class="title clearfix">
                                                    <div class="left">
                                                        <asp:Label ID="LBuserListTitle" runat="server" CssClass="fieldlabel">*Users</asp:Label>
                                                    </div>
                                                    <div class="right">
                                                        <span class="icons">
                                                            <!--<span class="icon addcriteria adduser"></span>
                                                            <span class="icon edit unavailable"></span>
                                                            <span class="icon delete unavailable"></span>-->
                                                        </span>
                                                    </div>
                                                    <div class="clearer"></div>
                                                </div>
                                                    <div class="footer">
                                                        
                                                        <table class="table light users fullwidth expandable compressed" data-max="5">
                                                            <thead>
                                                                <tr>
                                                                    <th class="check">
                                                                        <input type="checkbox" class="checkall" />
                                                                    </th>
                                                                    <th class="username"><asp:Literal ID="LTusername_t" runat="server"></asp:Literal></th>
                                                                    <th class="details"><asp:Literal ID="LTusermail_t" runat="server"></asp:Literal></th>
                                                                </tr>
                                                            </thead>
                                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="check">
                                                                    <span class="submittercheckbox">
                                                                        <input type="checkbox" runat="server" id="CBXuser" />
                                                                        <asp:Literal ID="LTidUser" runat="server" Visible="false" Text='<%#Container.DataItem.IdPerson %>'></asp:Literal>
                                                                    </span>
                                                                </td>
                                                                <td class="username"><asp:Literal ID="LTuserName" runat="server"></asp:Literal></td>
                                                                <td class="details"><asp:Literal ID="LTuserMail" runat="server"></asp:Literal></td>
                                                            </tr>                                                   
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                            </tbody>
                                                            <tfoot>
                                                                <tr id="TRfooter" runat="server">
                                                                    <td colspan="3">
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
                                    <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                    <asp:button ID="BTNsaveAvailabilityBottom" runat="server" Text="Save"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="view-modal view-profiletypes" id="DVselectPersonTypes" runat="server" visible="false">
                <div class="fieldobject fielddescription">
                    <div class="fieldrow">
                        <asp:Label ID="LBpersonTypesDescription" runat="server" CssClass="description">*Seleziona a quali profili rendere accessibile il bando:</asp:Label>
                    </div>
                </div>
                <div class="fieldobject">
                    <div class="fieldrow">
                        <asp:Label ID="LBselectAllpersonTypes_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXselectAllpersonTypes"></asp:Label>
                        <asp:CheckBox ID="CBXselectAllpersonTypes" runat="server" AutoPostBack="true" />
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
                <div class="fieldobject fielddescription">
                    <div class="fieldrow">
                        <asp:Label ID="LBselectRolesDescription" runat="server" CssClass="description">*Seleziona a quali ruoli della comunità rendere accessibile il bando:</asp:Label>
                    </div>
                </div>
                <div class="fieldobject">
                    <div class="fieldrow">
                        <asp:Label ID="LBcommunityName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcommunityName"></asp:Label>
                        <asp:Label ID="LBcommunityName" runat="server" CssClass="communityname" AssociatedControlID=""></asp:Label>
                    </div>
                </div>
                <div class="fieldobject">
                    <div class="fieldrow">
                        <asp:Label ID="LBselectAllroles_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXselectAllRoles"></asp:Label>
                        <asp:CheckBox ID="CBXselectAllRoles" runat="server" AutoPostBack="true" />
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
            <CTRL:CommunitySelector id="CTRLaddCommunity" runat="server" visible="false"  SelectionMode="Multiple"></CTRL:CommunitySelector>
            <div class="view-modal view-call" id="DVselectFromCall" runat="server" visible="false">
                <CTRL:SelectUsersFromCall ID="CTRLselectUsersFromCall" runat="server" RaiseEvents="True"/>
            </div>
            <div class="view-modal view-users" id="DVselectUsers" runat="server" visible="false">
                 <CTRL:SelectUsers ID="CLTRselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
                  DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True" DefaultMaxPreviewItems="5" 
                  ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false"/>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>