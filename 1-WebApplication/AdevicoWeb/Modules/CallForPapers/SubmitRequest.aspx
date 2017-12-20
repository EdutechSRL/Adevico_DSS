<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="SubmitRequest.aspx.vb" Inherits="Comunita_OnLine.SubmitRequest" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputField.ascx" TagName="CTRLInputField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/Modules/Common/Editor/UC_TextAreaEditorHeader.ascx" TagPrefix="CTRL" TagName="UC_TextAreaEditorHeader" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" />
    <script type="text/javascript">
        $(function () {
            $(".fieldobject.checkboxlist").each(function () {
                if ($(this).find(".extraoption").size() > 0) {
                    var $extraoption = $(this).find(".extraoption");
                    var $textoption = $(this).find(".textoption");

                    $extraoption.next("label").after($textoption);
                    if ($extraoption) {
                        if ($extraoption.is(":checked")) {
                            $textoption.find("input").attr("disabled", false);
                            $textoption.removeClass("disabled");
                        } else {
                            $textoption.find("input").attr("disabled", true);
                            $textoption.addClass("disabled");
                        }
                    }
                }
            });
            $(".fieldobject.radiobuttonlist").each(function () {
                if ($(this).find(".extraoption").size() > 0) {
                    var $extraoption = $(this).find(".extraoption input[type='radio']");
                    var $textoption = $(this).find(".textoption");

                    $extraoption.next("label").after($textoption);
                    if ($(this).find("input[type='radio']")) {
                        if ($(this).is(":checked")) {
                            $textoption.find("input").attr("disabled", false);
                            $textoption.removeClass("disabled");
                        } else {
                            $textoption.find("input").attr("disabled", true);
                            $textoption.addClass("disabled");
                        }
                    }
                }
            });
            $(".fieldobject.radiobuttonlist input[type='radio']").change(function () {

                if ($(this).parents("span.extraoption").first().size() > 0) {
                    var $textoption = $(this).parents(".radiobuttonlist").first().find(".textoption");
                    $textoption.find("input").attr("disabled", false);
                    $textoption.removeClass("disabled");
                } else {
                    var $textoption = $(this).parents(".radiobuttonlist").first().find(".textoption");
                    $textoption.find("input").attr("disabled", true);
                    $textoption.addClass("disabled");
                }
            });

            $(".fieldobject.checkboxlist input[type='checkbox']").change(function () {

                if ($(this).is(".extraoption")) {
                    var ischecked = $(this).is(":checked");
                    var $textoption = $(this).parents(".checkboxlist").first().find(".textoption");
                    $textoption.find("input").attr("disabled", !ischecked);
                    $textoption.toggleClass("disabled");
                }
            });
        });

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

            $(".fieldobject.disclaimer.custom").checkboxList({
                listSelector: "span.inputcheckboxlist",
                errorSelector: "self",
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

    <%--    function updateProgress() {
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
        }--%>

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
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVpreview" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <br /><br />
            <CTRL:Messages ID="CTRLerrorMessages"  runat="server" />
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWstartMessage" runat="server">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPbackTolist" runat="server" Text="Lista bandi" CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <CTRL:Messages ID="CTRLstartMessages"  runat="server" Visible="false" />
            <div class="view">
                <div class="cfpdescription ">
                    <div class="textwrapper">
                        <div class="renderedtext"><asp:Literal ID="LTstartMessage" runat="server"></asp:Literal></div>
                    </div>
                    <div class="cfpdetails" runat="server" id="DVexpiration">
                        <span class="expiration">
                            <asp:Label ID="LBtimeValidity_t" runat="server">Validità</asp:Label>
                            <asp:Label ID="LBstartDate" CssClass="startdate" runat="server"></asp:Label>&nbsp;-&nbsp;
                            <asp:Label ID="LBendDate" CssClass="enddate" runat="server"></asp:Label>
                        </span>
                    </div>
                </div>
                <fieldset class="section partecipants" runat="server" id="FLDsubmittersSelector" visible="false">
                    <legend><asp:Literal id="LTsubmitterTypesTitle" runat="server"></asp:Literal></legend>
                    <div class="sectiondescription">
		                <div class="messages">
			                <div class="message info">
                                <asp:Literal ID="LTsubmittersSelectorMessage" runat="server"></asp:Literal>
			                </div>
		                </div>
	                </div>
                    <div class="fieldrow">
                        <asp:RadioButtonList ID="RBLsubmitters" runat="server" CssClass="rbldl" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:RadioButtonList>
                    </div>
                </fieldset>
                <div class="DivEpButton big" runat="server" id="DVstartSubmission">
                    <asp:Button ID="BTNstartCompile" runat="server" />
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWcall" runat="server">
            
            <div class="view">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu"></asp:HyperLink>
                    <asp:Button ID="BTNvirtualDeleteSubmission" runat="server" Text="Cancella domanda" Visible="false"/>
                    <asp:Button ID="BTNgoToFirstStepTop" runat="server" Text="Salva" Visible="false"/>
                    <asp:Button ID="BTNsubmitRequestTop" runat="server" Text="Sottometti definitivamente" Visible="false"/>
                </div>
                <CTRL:Messages ID="CTRLviewMessages"  runat="server" Visible="false" />
                <asp:Repeater id="RPTattachments" runat="server">
                    <HeaderTemplate>
                        <fieldset class="section collapsable attachments">
                            <legend><asp:Literal id="LTattachmentsTitle" runat="server"></asp:Literal></legend>
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
                            <legend><asp:Literal id="LTsectionTitle" runat="server"></asp:Literal></legend>
                            <div class="sectiondescription">
                                <asp:Literal ID="LTsectionDescription" runat="server"></asp:Literal>
                            </div>
                            <asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>" OnItemCreated="RPTfields_ItemCreated" OnItemDataBound="RPTfields_ItemDataBound">
                                <ItemTemplate>
                                    <CTRL:CTRLInputField ID="CTRLinputField" runat="server" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </ItemTemplate>   
                </asp:Repeater>
<%--                <telerik:radprogressmanager id="RPBmanager" runat="server" registerforsubmit="false" />
                <telerik:radprogressarea id="PRAfileUpload" runat="server" displaycancelbutton="false" skin="Default" />--%>
                <div class="DivEpButton big">
                    <asp:Button ID="BTNgoToFirstStep" runat="server" />
                    <asp:Button ID="BTNsubmitRequest" runat="server" />
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>