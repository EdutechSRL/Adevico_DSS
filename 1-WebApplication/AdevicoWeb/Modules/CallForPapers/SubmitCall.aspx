<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="SubmitCall.aspx.vb" Inherits="Comunita_OnLine.SubmitCall" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputField.ascx" TagName="CTRLInputField" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputRequiredFile.ascx" TagName="CTRLrequiredFile" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_PrintDraft.ascx" TagPrefix="CTRL" TagName="PrintDraft" %>
<%@ Register Src="~/Modules/Common/Editor/UC_TextAreaEditorHeader.ascx" TagPrefix="CTRL" TagName="UC_TextAreaEditorHeader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" />
    <style>
		th {
		  text-align: left;
		}
		.table {
		  margin-bottom: 20px;
		}
		.table > thead > tr > th,
		.table > tbody > tr > th,
		.table > tfoot > tr > th,
		.table > thead > tr > td,
		.table > tbody > tr > td,
		.table > tfoot > tr > td {
		  padding: 8px;
		  line-height: 1.42857143;
		  vertical-align: top;
		  border-top: 1px solid #ddd;
		}
		.table > thead > tr > th {
		  vertical-align: bottom;
		  border-bottom: 2px solid #ddd;
		}
        table td input, 
        table td textarea {
            border: 0 none !important;
            border-bottom: solid 1px #ddd !important;
			width:100px;
        }
        .tableReport tr td,
        .tableReport tr th
        {
            border: 1px solid #ddd;
        }		
        fieldset{
            min-width:0 !important;
        }

        /*span.totalHidden {
            display: none;
        }*/

        /* Rad Upload: blocco interfaccia in fase di caricamento...*/
        
        /*  Pannello Upload Telerik */
        /*.RadUploadProgressArea.RadUploadProgressArea_Default,*/
        /*  Overlay salvataggio */
        .adv-overlay {
            top: 200px;
            left: 2%;
            right: 2%;
            bottom: 2%;
            background-color: #fff;
            width: auto;
            height: auto;
            background-color: RGBA(255,255,255,0.9);
            position: fixed;
            z-index: 111;
            border: solid 1px #868686;
            min-width: 320px;
            padding: 30px;
        }

        div.multiline span.warning
        {
            color: #c20 !important;
            background-color: #fee !important;
        }
	</style>
    <script type="text/javascript">    
    function CheckUpload() {
       //finds all file uploads that are currently in progress
        var uploadingRows = $(".RadAsyncUpload").find(".ruUploadProgress");
        //iterates and checks if there are any file uploads that are successfully completed or failed and if yes - pop-up an alert box and prevent page submitting 
        for (var i = 0; i < uploadingRows.length; i++) {
            if (!$(uploadingRows[i]).hasClass("ruUploadCancelled") && !$(uploadingRows[i]).hasClass("ruUploadFailure") && !$(uploadingRows[i]).hasClass("ruUploadSuccess")) {
                alert("Attendere il completamento degli upload prima di salvare.");
                return false;
            }
        }

        return true;
    }
    
         $(function () {

            /*Start: Blocco interfaccia in fase di salvataggio.*/
            var flagBtnOneClick = true;
            jQuery(".just-one-click, #<%=BTNsaveSubmissionTop.ClientID%>, #<%=BTNsubmitSubmissionTop.ClientID%>," + 
                    "#<%=BTNsaveSubmissionBottom.ClientID%>, #<%=BTNsubmitSubmissionBottom.ClientID%>").click(function(ev){

                if(!CheckUpload() || !flagBtnOneClick){
                    ev.preventDefault();
                    ev.stopPropagation();
                    return;
                }

                flagBtnOneClick = false;
                jQuery("body").append('<div class="adv-overlay" style="top:2%;">'
                                +'<h3 style="text-align:center;margin:top:13%;font-size:50px;color:#444;">Sto salvando...</h3></div>');
            
                /* per riabilitare dopo 2 sec scommentare qui
                setTimeOut(function(){
                    flagBtnOneClick = true;
                    jQuery("adv-overlay").remove();
                }, 2000);*/
            });

            jQuery(".just-one-click-delay").click(function(ev){

                if(!CheckUpload() || !flagBtnOneClick){
                    ev.preventDefault();
                    ev.stopPropagation();
                    return;
                }

                flagBtnOneClick = false;
                jQuery("body").append('<div class="adv-overlay" style="top:2%;">'
                                +'<h3 style="text-align:center;margin:top:13%;font-size:50px;color:#444;">Sto salvando bis...</h3></div>');
            
                /* Riabilito dopo 5 secondi...*/
                setTimeout(function(){
                    flagBtnOneClick = true;
                    jQuery(".adv-overlay").remove();
                }, 5000);
            });
            

            /*End*/

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
                         } 
                         //else {
                         //    $textoption.find("input").attr("disabled", true);
                         //    $textoption.addClass("disabled");
                         //}
                     }
                 }
             });
             $(".fieldobject.radiobuttonlist input[type='radio']").change(function () {
                
                 if ($(this).parents("span.extraoption").first().size()>0) {
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
                errorSelector: "self",
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
    <script type="text/javascript" src="../../Jscript/Modules/CallForPapers/TableReport.js"></script>
    

     <asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
            <script language="javascript" type="text/javascript">
                $(function () {
                    showDialog("infoDelete");
                });
            </script>
            <script>



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
        <asp:View ID="VIWcall" runat="server">
            <div class="view">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu"></asp:HyperLink>
                    <CTRL:PrintDraft runat="server" ID="CTRL_PrintEmptyDraft" />
                    <asp:Button ID="BTNvirtualDeleteSubmission" runat="server" Text="Cancella domanda" Visible="false"/>
                    <asp:Button ID="BTNsaveSubmissionTop" runat="server" Text="Salva" Visible="false" CssClass="TKlockOnUpload"/>
                    <asp:Button ID="BTNsubmitSubmissionTop" runat="server" Text="Sottometti definitivamente" Visible="false" CssClass="TKlockOnUpload"/>
                    <%--<asp:linkbutton id="LBKsubmitSubmissionTop" runat="server" CssClass="linkMenu" Text="Sottometti definitivamente"></asp:linkbutton>--%>
                </div>
                <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                <fieldset class="section collapsable cfpintro">
                    <legend>
                    <span class="switchsection handle">&nbsp;</span>
                    <span class="title">
                    <asp:Label id="LBcallDescriptionTitle" runat="server"></asp:Label>
                    </span>
                    </legend>
                    <div class="cfpdescription">
                        <div class="renderedtext"><asp:Literal id="LTcallDescription" runat="server"/></div>
                    </div>
                    <div class="cfpdetails">
                        <span class="expiration">
                            <asp:Label ID="LBtimeValidity_t" runat="server">Validità</asp:Label>
                            <asp:Label ID="LBstartDate" CssClass="startdate" runat="server"></asp:Label>&nbsp;-&nbsp;
                            <asp:Label ID="LBendDate" CssClass="enddate" runat="server"></asp:Label>
                        </span>
                        <asp:Label runat="server" ID="LBwinnerinfo" class="winnerinfo" Visible="false"></asp:Label>
                    </div>
                </fieldset>
                <fieldset class="section partecipants" runat="server" id="FLDsubmitters">
                    <legend>
                    <span class="switchsection handle">&nbsp;</span>
                    <span class="title">
                    <asp:Literal id="LTsubmitterTypesTitle" runat="server"></asp:Literal>
                    </span>
                    </legend>
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
                <asp:Repeater id="RPTattachments" runat="server">
                    <HeaderTemplate>
                        <fieldset class="section collapsable attachments">
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
                                    <CTRL:CTRLInputField ID="CTRLinputField" runat="server" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </ItemTemplate>   
                </asp:Repeater>
                <asp:Repeater ID="RPTrequiredFiles" runat="server">
                    <HeaderTemplate>
                        <fieldset class="section collapsable">
                            <legend>
                            <span class="switchsection handle">&nbsp;</span>
                            <span class="title">
                            <asp:Literal id="LTrequiredFilesTitle" runat="server"></asp:Literal>
                            </span>
                            </legend>
                            <div class="sectiondescription">
                                <asp:Literal ID="LTrequiredFilesDescription" runat="server"></asp:Literal>
                            </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <CTRL:CTRLrequiredFile ID="CTRLrequiredFile" runat="server" RemoveFile="OnRemoveFile" />
                    </ItemTemplate>   
                    <FooterTemplate>
                       
                        </fieldset>
                    </FooterTemplate>
                </asp:Repeater>
                <telerik:radprogressmanager id="RPBmanager" runat="server" registerforsubmit="false" />
                <telerik:radprogressarea id="PRAfileUpload" runat="server" displaycancelbutton="false" skin="Default" />
                <div class="CFPBoxes" id="DVbottomBox" runat="server" visible="false">
                    <div class="CFPBox boxsave left">
                        <asp:Button ID="BTNsaveSubmissionBottom" runat="server" Text="Salva" CssClass="TKlockOnUpload"/>
                        <CTRL:PrintDraft runat="server" ID="CTRL_SavePrintDraft" ButtonCssClass="linkMenu just-one-click-delay"/>
                        <div class="cfpdescription">
                            <asp:Literal ID="LTsaveBottomExplanation" runat="server">
                            Salva la domanda e potrai modificarla in un secondo momento. La domanda non è
                            sottomessa (e quindi valida) fino a quando non verrà premuto sottometti definitivamente</asp:Literal>
                        </div>
                    </div>
                    <div class="CFPBox boxsubmit right">
                        <asp:Button ID="BTNsubmitSubmissionBottom" runat="server" Text="Sottometti definitivamente" CssClass="TKlockOnUpload" />
                        <%--<asp:linkbutton id="LBKsubmitSubmissionBottom" runat="server" CssClass="linkMenu" Text="Sottometti definitivamente"></asp:linkbutton>--%>
                        <div class="cfpdescription">
                            <asp:Literal ID="LTsaveAndSubmitBottomExplanation" runat="server">Sottometti definitivamente la domanda affinchè venga valutata. Non sarà più possibile
                            modificare la domanda</asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>


</asp:Content>