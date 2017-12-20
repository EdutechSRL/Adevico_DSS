<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ReviewSubmission.aspx.vb" Inherits="Comunita_OnLine.ReviewSubmission" %>

<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputField.ascx" TagName="CTRLInputField" TagPrefix="CTRL" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputRequiredFile.ascx" TagName="CTRLrequiredFile" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_RenderField.ascx" TagName="CTRLrenderField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_TextAreaEditorHeader.ascx" TagPrefix="CTRL" TagName="UC_TextAreaEditorHeader" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadContent" runat="server">
     <CTRL:Header ID="CTRLheader" runat="server"/>
    <script type="text/javascript">
        $(function () {
            $(".fieldinput input").add(".fieldinput textarea.textarea").focus(function () {
                $(this).siblings(".inlinetooltip").addClass("visible");
            });

            $(".fieldobject.date .fieldinput input")
                .add(".fieldobject.datetime .fieldinput input")
                .add(".fieldobject.time .fieldinput input").focus(function () {
                    $(this).parents(".fieldobject").find(".inlinetooltip").addClass("visible");
                });
            $(".fieldobject.date .fieldinput input")
                .add(".fieldobject.datetime .fieldinput input")
                .add(".fieldobject.time .fieldinput input").blur(function () {
                    $(this).parents(".fieldobject").find(".inlinetooltip").removeClass("visible");
                });

            $(".fieldinput input[type=file]").hover(function () {
                $(this).siblings(".inlinetooltip").addClass("visible");
            },
        function () {
            $(this).siblings(".inlinetooltip").removeClass("visible");
        });

            $(".fieldinput input").add(".fieldinput textarea.textarea").blur(function () {
                $(this).siblings(".inlinetooltip").removeClass("visible");
            });
            $("fieldset.section.collapsed").each(function () {
                var $fieldset = $(this);
                var $legend = $fieldset.children().filter("legend");
                var $children = $fieldset.children().not("legend");
                $children.toggle();
            });

            $("fieldset.section.collapsable legend").click(function () {
                var $legend = $(this);
                var $fieldset = $legend.parent();
                var $children = $fieldset.children().not("legend");
                $children.toggle();
                $fieldset.toggleClass("collapsed");
            });

            $(".fieldobject.checkboxlist").checkboxList({
                listSelector: "span.inputcheckboxlist",
                errorSelector: ".fieldrow.fieldinput label",
                checkOnStart: true,
                error: {
                    min: ".minmax .min",
                    max: ".minmax .max"
                }
            });

            //jquery.textVal.js

            $(".fieldobject.singleline .fieldrow.fieldinput").textVal({
                textSelector: "input.inputtext",
                charAvailable: ".fieldinfo .maxchar .availableitems",
                errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
                charMax: ".fieldinfo .maxchar .totalitems"

            });

            $(".fieldobject.multiline .fieldrow.fieldinput").textVal({
                textSelector: "textarea.textarea",
                charAvailable: ".fieldinfo .maxchar .availableitems",
                errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
                charMax: ".fieldinfo .maxchar .totalitems"
            });
        });
        $(document).ready(function () {
            $('#infoDelete').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 900,
                height: 600,
                minHeight: 400,
                minWidth: 600,
                zIndex: 99999,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }
            });
        });

        function updateProgress() {
            var empty = true;

            $("input[type=file]").each(function () {
                empty = empty || $(this).val() == "";
            });
            if (empty) {
                try {
                    $find("<%= PRAfileUpload.ClientID %>").hide();
                    getRadProgressManager().hideProgressAreas();
                }
                catch (ex) { }
            }
            else {
                getRadProgressManager().startProgressPolling();
            }
        }

        function showDialog(id) {
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

    </script>
     <asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
            <script language="javascript">
                $(function () {
                    showDialog("infoDelete");
                });
            </script>
        </asp:Literal>
    <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
    <CTRL:UC_TextAreaEditorHeader runat="server" id="UC_TextAreaEditorHeader" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVpreview" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBpreviewMessage" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWcall" runat="server">
            <div class="view reviewsubmission">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPrevisions" runat="server" Text="Lista revisione" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                    <asp:Button ID="BTNsaveRevisionTop" runat="server" Text="Salva" Visible="false"/>
                    <asp:Button ID="BTNsubmitRevisionTop" runat="server" Text="Sottometti definitivamente" Visible="false"/>
                </div>
                <div class="messages" id="DVmessages" runat="server" visible="false">
                    <div class="message info" id="DVtimeValidity" runat="server">
                        <span class="revisionalert clearfix">
                            <span class="revisionneeded">
                                <asp:Literal ID="LTrevisionAction_t" runat="server">E' stata richiesta una revisione</asp:Literal>
                                <span class="revisionapplicant">
                                    <asp:Literal ID="LTrevisionActionBy_t" runat="server">da</asp:Literal>
                                    <asp:label ID="LBrevisionActionBy" runat="server" CssClass="name"></asp:label>
                                </span>
                            </span>
                            <asp:Label ID="LBrevisionMessage" runat="server" cssclass="revisionmsg clearfix"></asp:Label>
                            <span class="revisionconfirm">
                                <span class="revisionmessage">
                                    <asp:Literal ID="LTrevisionStatus" runat="server"></asp:Literal>
                                    <span class="revisionapplicant">
                                        <asp:Literal ID="LTrevisionActionManagedBy_t" runat="server">da</asp:Literal>
                                        <asp:label ID="LBrevisionActionManagedBy" runat="server" CssClass="name"></asp:label>
                                    </span>
                                </span>
                                <span class="revisiondate" runat="server" id="DVrevisionDate">
                                    <asp:Literal ID="LTrevisionActionDeadline_t" runat="server">entro il</asp:Literal>
                                    <asp:Label ID="LBrevisionActionDeadline" runat="server" cssclass="date"></asp:Label>
                                </span>
                                <span class="revisionmessage" runat="server" id="DVrevisionAction">
                                    <asp:Literal ID="LTrevisionActionSubmittedOn" runat="server"></asp:Literal>&nbsp;
                                    <asp:label ID="LBrevisionExpectedAction" runat="server" CssClass="name">(in attesa di verifica)</asp:label>
                                </span>
                            </span>
                        </span>
                    </div>
                    <div class="message alert" id="DVmessage" runat="server" visible="false">
                        <asp:Label ID="LBdisplayMessage" runat="server"></asp:Label>
                    </div>
                    <br />
                </div>
                <fieldset class="section collapsable collapsed cfpintro">
                    <legend>
                    <span class="switchsection handle">&nbsp;</span>
                    <span class="title">
                        <asp:Label id="LBcallDescriptionTitle" runat="server"></asp:Label>
                    </span>
                    </legend>
                    <div class="cfpdescription">
                        <div class="renderedtext"><asp:Literal id="LTcallDescription" runat="server"/></div>
                    </div>
                </fieldset>
                <asp:Repeater id="RPTattachments" runat="server">
                    <HeaderTemplate>
                        <fieldset class="section collapsable collapsed attachments">
                            <legend>
                            <span class="switchsection handle">&nbsp;</span>
                            <span class="title">
                                <asp:Literal id="LTattachmentsTitle" runat="server"></asp:Literal>
                            </span>
                            </legend>
                            <div class="fieldobject">
                                <div class="fieldrow">
                                    <ul class="attachedfiles">
                    </HeaderTemplate>
                    <ItemTemplate>
                                        <li class="attachedfile">
                                             <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false"  />
                                            <div class="cfpdescription" runat="server" id="DVdescription" visible="false">
                                                <asp:Label ID="LBattachmentDescription" runat="server"></asp:Label>
                                            </div>
                                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                                    </ul>
                                </div>
                            </div>
                         </fieldset>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="RPTsections" runat="server">
                    <ItemTemplate>
                        <fieldset class="section collapsable">
                            <legend>
                            <span class="switchsection handle">&nbsp;</span>
                            <span class="title">
                                <asp:Literal id="LTsectionTitle" runat="server"></asp:Literal>
                            </span>
                            </legend>
                            <div class="sectiondescription">
                                <asp:Literal ID="LTsectionDescription" runat="server"></asp:Literal>
                            </div>
                            <asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>" OnItemCreated="RPTfields_ItemCreated" OnItemDataBound="RPTfields_ItemDataBound">
                                <ItemTemplate>
                                    <CTRL:CTRLInputField ID="CTRLinputField" ReviewMode="true" runat="server" Visible="false" />
                                    <CTRL:CTRLrenderField ID="CTRLrenderField" runat="server" Visible="false"/>
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </ItemTemplate>   
                </asp:Repeater>
                <telerik:radprogressmanager id="RPBmanager" runat="server" registerforsubmit="false" />
                <telerik:radprogressarea id="PRAfileUpload" runat="server" displaycancelbutton="false" skin="Default" />
                <div class="CFPBoxes" id="DVbottomBox" runat="server" visible="false">
                    <div class="CFPBox boxsave left">
                        <asp:Button ID="BTNsaveRevisionBottom" runat="server" Text="Salva" />

                        <div class="cfpdescription">
                            <asp:Literal ID="LTsaveBottomRevisionExplanation" runat="server">
                            Salva la revisione e potrai modificarla in un secondo momento. La revisione non è
                            sottomessa (e quindi valida) fino a quando non verrà premuto sottometti definitivamente</asp:Literal>
                        </div>
                    </div>
                    <div class="CFPBox boxsubmit right">
                        <asp:Button ID="BTNsubmitRevisionBottom" runat="server" Text="Sottometti definitivamente" />
                        <div class="cfpdescription">
                            <asp:Literal ID="LTsaveAndSubmitBottomRevisionExplanation" runat="server">Sottometti definitivamente la revisione affinchè venga valutata. Non sarà più possibile
                            modificare la domanda</asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>