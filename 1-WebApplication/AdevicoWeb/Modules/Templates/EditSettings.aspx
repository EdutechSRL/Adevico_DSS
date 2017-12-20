<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditSettings.aspx.vb" Inherits="Comunita_OnLine.EditTemplateSettings" MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Templates/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Common/UC/UC_MailSettings.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>

     <script type="text/javascript">
         $(function () {
             $(".ddbuttonlist.enabled").dropdownButtonList();

             $(".btnswitch.disabled").click(function () {
                 return false;
             });

             $(".btnswitch:not(.disabled)").click(function () {
                 var $parent = $(this).parents(".btnswitchgroup").first();
                 $parent.find(".active").removeClass("active");
                 $(this).addClass("active");
                 return false;
             });
             $(".chzn-select").chosen();

             if ($.browser.msie && ($.browser.version === "7.0")) {

                 var j = 500;

                 $(".chzn-select").each(function () {

                     $(this).parent().css({ 'position': 'relative', 'z-index': j });
                     j = j - 1;

                 });

             }

             $(".selectall").click(function () {
                 var dis = $(this).parents(".choseselect").find(".chzn-select").attr("disabled");
                 if (dis != "disabled") {
                     $(this).parents(".choseselect").find(".chzn-select option:not(:disabled)").attr("selected", true);
                     $(this).parents(".choseselect").find(".chzn-select").trigger("liszt:updated");
                     $(this).parents(".choseselect").find(".chzn-select").trigger("change");
                 }
             });
             $(".selectnone").click(function () {
                 var dis = $(this).parents(".choseselect").find(".chzn-select").attr("disabled");
                 if (dis != "disabled") {
                     //$(this).parents(".choseselect").find(".chzn-select option").attr("selected", false);
                     $(this).parents(".choseselect").find(".chzn-select").val("");
                     $(this).parents(".choseselect").find(".chzn-select").trigger("liszt:updated");
                     $(this).parents(".choseselect").find(".chzn-select").trigger("change");
                 }
             });

             $(".fieldrow.eventswitch input[type='checkbox']").change(function () {
                 var checked = $(this).is(":checked");
                 var $parent = $(this).parents(".eventoptions");
                 if (checked) {
                     $parent.find(".fieldrow.events").addClass("enabled").removeClass("disabled");
                     $parent.find(".fieldrow.service").addClass("enabled").removeClass("disabled");
                 } else {
                     $parent.find(".fieldrow.events").removeClass("enabled").addClass("disabled");
                     $parent.find(".fieldrow.service").removeClass("enabled").addClass("disabled");
                 }
             });
         });

         $(document).ready(function () {
             $(".view-modal.view-modules").dialog({
                 appendTo: "form",
                 closeOnEscape: true,
                 modal: true,
                 width: 890,
                 height: 450,
                 minHeight: 300,
                 minWidth: 700,
                 title: '<%=DialogTitleTranslation() %>',
                 open: function (type, data) {
                    // $(this).parent().appendTo("form");
                     //$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });
         });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnoTemplate" runat="server"></asp:Label>
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
                                <asp:button ID="BTNeditDraftSettingsTop" runat="server" Text="Save" Visible="false"/>
                                <asp:button ID="BTNeditSettingsTop" runat="server" Text="Save" Visible="false"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                        </div>
                        <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
                            <asp:View id="VIWcontent" runat="server">
                            <div class="contentouter">
                                <div class="content">
                                    <div class="fieldobject pagedescription">
                                        <div class="fieldrow fielddescription">
                                            <asp:Label ID="LBeditTemplateDescription" runat="server" CssClass="description"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="fieldobject selectorwrapper templatename">
                                        <div class="fieldrow templatename">
                                            <asp:Label ID="LBtemplateName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBtemplateName">*Name:</asp:Label>
                                            <asp:Label ID="LBtemplateLanguage_t" runat="server" CssClass="templatelanguage" AssociatedControlID="TXBtemplateName"></asp:Label>
                                            <asp:TextBox ID="TXBtemplateName" runat="server" CssClass="fieldinput"></asp:TextBox>
                                            <asp:Label ID="LBtemplateVersion" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBtemplateName" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="fieldobject selectorwrapper serviceselector" ID="DVmoduleSelector" runat="server" visible="false">
                                        <div class="fieldrow">
                                            <div class="choseselect clearfix">
                                                <div class="left">
                                                    <asp:Label ID="LBmoduleName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBtemplateName">*Module:</asp:Label>
                                                    <select runat="server" id="SLBmodules" class="services chzn-select" multiple tabindex="2">
                                                                        
                                                    </select>
                                                </div>
                                                <div class="right">
                                                    <asp:Button ID="BTNapplyModules" runat="server" CssClass="Link_Menu" Text="Apply" />
                                                </div>
                                                <div class="right">
                                                    <span class="icons">
										                <span class="icon selectall" title="All" runat="server" id="SPNmoduleSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNmoduleSelectNone">&nbsp;</span>
									                </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="fieldobject clearfix selectorwrapper mediumselector " ID="DVnotificationSelector" runat="server" visible="false">
                                        <div class="fieldrow left">
                                            <asp:Label ID="LBnotificationSettings_t" CssClass="title" runat="server">*Medium Settings:</asp:Label>
                                        </div>
                                        <div class="fieldrow right">
                                            <asp:Label ID="LBnotificationType_t" CssClass="label" runat="server">*Add messagge type</asp:Label>
                                            <div class="ddbuttonlist enabled" id="DVnotificationType" runat="server"><!--
                                                --><asp:Repeater ID="RPTnotificationType" runat="server">
                                                    <ItemTemplate><!--
                                                    --><asp:linkbutton id="LNBnotificationType" runat="server" CssClass="linkMenu" CausesValidation="false" CommandName="add"></asp:linkbutton><!--    
                                                    --></ItemTemplate>
                                                </asp:Repeater><!--
                                            --></div>
                                        </div>
                                    </div>
                                    <asp:Repeater ID="RPTnotificationSettings" runat="server">
                                        <HeaderTemplate>
                                            <ul class="templatemedia clearfix">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li class="templatemedium sectioncontent <%#Container.DataItem.Channel.ToString() %>channel">
                                                <div class="templatemediumcontent">
                                                    <div class="title clearfix">
                                                        <div class="left">
                                                            <asp:Label id="LBsettingsTitle" runat="server" CssClass="fieldlabel">*Settings</asp:Label>
                                                            <asp:Literal ID="LTtemporaryID" runat="server" Visible="false" Text='<%#Container.DataItem.TemporaryId%>'></asp:Literal>
                                                        </div>
                                                        <div class="right">
                                                            <span class="icons">
                                                                <asp:Button ID="BTNdeleteSettings" runat="server" Text="D" CssClass="img_btn icon delete needconfirm" CommandArgument="<%#Container.DataItem.Settings.Id %>" CommandName="virtualdelete"/>
                                                            </span>
                                                        </div>
                                                        <div class="clearer"></div>
                                                    </div>
                                                    <div class="footer">
                                                        <div class="fieldobject fieldoptions">
                                                            <CTRL:MailSettings id="CTRLmailSettings" runat="server" visible="false" RaiseUpdateEvent="false" AllowSignatureFromTemplate="true" />
                                                        </div>
                                                       <%--<div class="fieldobject fieldoptions eventoptions" id="DVnotificationSettings" runat="server">
                                	                        <div class="fieldrow fielddescription">
                                                                <asp:Label id="LBitemDescription" runat="server" CssClass="description"></asp:Label>
                                	                        </div>
                                                           
                                                            <div class="fieldrow eventswitch">
                                                                <asp:Label id="LBnotificationMode_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXnotificationMode">*Enable Events</asp:Label>
                                                                <asp:CheckBox ID="CBXnotificationMode" runat="server" CssClass="fieldinput eventswitch"/>
                                                            </div>
                                                            <div class="fieldrow service" id="DVservice" runat="server">
                                                                <asp:Label id="LBmodules_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLmodules">*Module:</asp:Label>
                                                                <asp:DropDownList ID="DDLmodules" runat="server" CssClass="Testo_Campo" AutoPostBack="true" OnSelectedIndexChanged="DDLmodules_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="fieldrow events" id="DVevents" runat="server">
                                                                <asp:Label id="LBmodulesAction_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLactions">*Module Event:</asp:Label>
                                                                <asp:DropDownList ID="DDLactions" runat="server" CssClass="Testo_Campo"></asp:DropDownList>
                                                            </div>
                                                        </div> --%>
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <div class="fieldrow no-data-message" id="DVnoNotifications" runat="server" visible="false">
                                        <asp:Label ID="LBnoNotifications" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            </asp:View>
                            <asp:View ID="VIWunknown" runat="server">
                                <div class="contentouter">
                                    <div class="content">
                                    </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>

                       
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNeditDraftSettingsBottom" runat="server" Text="Save" Visible="false"/>
                                <asp:button ID="BTNeditSettingsBottom" runat="server" Text="Save" Visible="false"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="view-modal view-modules" id="DVmodulesConfirm" runat="server" visible="false">
        <div class="messages">
            <div class="message alert">
                <asp:Label ID="LBconfirmModulesContent" runat="server"></asp:Label>

                 <div class="fieldobject clearfix" id="DVremovedPlaceHolders">
                    <div class="fieldrow">
                        <asp:Repeater ID="RPTremovedPlaceHolders" runat="server">
                            <HeaderTemplate>
                                <ul class="modules placeholderslist">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <span class="modulename"><asp:literal ID="LTModuleName" runat="server"></asp:literal></span>
                                <asp:Repeater ID="RPTplaceHolders" runat="server" DataSource="<%#Container.DataItem.Value %>">
                                    <HeaderTemplate><ul class="placeholderslist"></HeaderTemplate>
                                    <ItemTemplate>
                                        <li runat="server" id="LIplaceHolder">
                                            <span class="placeholder"><%#Container.DataItem %></span>
                                            <span class="separator">, </span>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate></ul></FooterTemplate>
                                </asp:Repeater>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
        <div class="fieldobject clearfix">
            <div class="fieldrow description">
                <asp:Label ID="LBconfirmModulesContentForApply" runat="server"></asp:Label>
            </div>
        </div>
        <div class="fieldobject clearfix">
            <div class="fieldrow">
                <asp:button ID="BTNundoApplyModuleContentEdit" runat="server" Text="*Undo"/>
                <asp:button ID="BTNremovePlaceHoldersFromContent" runat="server" Text="*Remove from content"/>
                <asp:button ID="BTNnoActionOnPlaceHolders" runat="server" Text="*Leave them in content"/>
            </div>
        </div>
    </div>
</asp:Content>