<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ManageReview.aspx.vb" Inherits="Comunita_OnLine.ManageReview" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_RenderField.ascx" TagName="CTRLrenderField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayFile" Src="~/Modules/Repository/UC/UC_ModuleRepositoryAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLreport" Src="~/Modules/CallForPapers/UC/UC_SubmissionExport.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"  EnableScripts="true"/>
    <script type="text/javascript">
        <% = me.CTRLreport.GetControlScript(HDNdownloadTokenValue.ClientID) %>
    </script>
    <script type="text/javascript">
        $(function () {
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

            $(".persist-area").semiFixed()



            $(".fieldobject.multiline .fieldrow.fieldinput").textVal({
                textSelector: "textarea.textarea",
                charAvailable: ".fieldinfo .maxchar .availableitems",
                errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
                charMax: ".fieldinfo .maxchar .totalitems"
            });
        });

      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
      <div class="contentwrapper edit clearfix">
        <div class="view compiled">
            <div class="persist-area">
                <div class="topbar persist-header" id="DVtopMenu" runat="server" visible="false">
                    <div class="innerwrapper clearfix">
                        <div class="left">
                            <ul class="sumbmissiondetails">
                                <li class="submitter">
                                    <asp:Literal ID="LTowner_t" runat="server"></asp:Literal>&nbsp;<asp:Label ID="LBowner"
                                        runat="server" /></li>
                                <li class="submittertype">
                                    <asp:Literal ID="LTsubmitterType_t" runat="server"></asp:Literal>&nbsp;<asp:Label
                                        ID="LBsubmitterType" runat="server" /></li>
                                <li class="status">
                                    <asp:Literal ID="LTsubmissionStatus_t" runat="server"></asp:Literal>&nbsp;<asp:Label
                                        ID="LBsubmissionStatus" runat="server"></asp:Label></li>
                                <li class="submissiondate" id="LIsubmissionInfo" runat="server" visible="false">
                                    <asp:Literal ID="LTsubmittedOn_t" runat="server"></asp:Literal>&nbsp;
                                    <asp:Label ID="LBsubmittedOnData" runat="server" CssClass="date" />&nbsp;<asp:Label
                                        ID="LBsubmittedOnTime" runat="server" CssClass="time" />
                                    <span class="submittedby" runat="server" id="SPNsubmittedBy">&nbsp;<asp:Literal ID="LTsubmittedBy_t"
                                        runat="server"></asp:Literal>&nbsp;
                                        <asp:Label ID="LBsubmittedBy" runat="server" />
                                    </span>
                                </li>
                            </ul>
                        </div>
                        <div class="right">
                            <asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu"
                                Visible="false"></asp:HyperLink>
                            <asp:HyperLink ID="HYPmanage" runat="server" Text="Gestione bandi" CssClass="Link_Menu"
                                Visible="false"></asp:HyperLink>
                            <span class="icons large">
                                <CTRL:CTRLreport ID="CTRLreport" runat="server" WebOnlyRender="True" isContainer="false" />
                            </span>
                        </div>
                    </div>
                    <div class="revisionsettings innerwrapper clearfix" runat="server" visible="false" id="DVrevision">
                        <div class="fieldobject multiline">
                            <div class="fieldrow fieldinput" id="DVdeadline" runat="server" visible="false">
                                <asp:Label ID="LBdeadline_t" runat="server" AssociatedControlID="RDPdeadline"  CssClass="fieldlabel">Entro il:</asp:Label>
                                <telerik:raddatetimepicker id="RDPdeadline" runat="server">
                                </telerik:raddatetimepicker>
                            </div>
                            <div class="fieldrow">
                                <asp:Label ID="LBrequestReason_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBrequestReason">Motivazione:</asp:Label>
                                <asp:Label ID="LBrequestReason" runat="server" CssClass="revisionmsg">Motivazione:</asp:Label>
                            </div>
                            <div class="fieldrow fieldinput" runat="server" ID="DVfeedback">
                                <asp:Label ID="LBfeedback_t" runat="server" AssociatedControlID="TXBfeedback" CssClass="fieldlabel">Commento:</asp:Label>
                                <asp:TextBox runat="server" ID="TXBfeedback" TextMode="multiline"
                                    CssClass="textarea"></asp:TextBox>
                                <asp:Label runat="server" ID="LBfeedbackHelp" CssClass="inlinetooltip"></asp:Label>
                                <br />
                                <span class="fieldinfo ">
                                    <span id="SPNmaxChar" class="maxchar" runat="server">
                                        <asp:Literal ID="LTmaxCharsrequest" runat="server"></asp:Literal>
                                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                                    </span>
                                </span>
                            </div>
                            <div class="fieldrow">
                                <ul class="sumbmissiondetails revisionrequest">
                                    <li class="status">
                                        <asp:Label ID="LBstatusInfo_t" runat="server" AssociatedControlID="LBstatusInfo_t">Stato attuale domanda:</asp:Label>
                                        <asp:Label ID="LBstatusInfo" runat="server"></asp:Label>
                                    </li>
                                    <li class="submissiondate">
                                         <asp:Literal ID="LTrevisionDate" runat="server"></asp:Literal>&nbsp;
                                         <asp:Label ID="LBrevisionDate" runat="server" CssClass="date" />&nbsp;<asp:Label
                                        ID="LBrevisionTime" runat="server" CssClass="time" />
                                        <span class="submittedby" runat="server" id="Span2">&nbsp;
                                            <asp:Literal ID="LTrevisionBy_t" runat="server"></asp:Literal>&nbsp;
                                            <asp:Label ID="LBrevisionBy" runat="server" />
                                        </span>
                                    </li>
                                </ul>
                            </div>
                            <div class="fieldrow fieldinput DivEpButton">
                                <asp:Button ID="BTNcancelManagerRequest" runat="server" CssClass="Link_Menu" Visible="false"/>
                                <asp:Button ID="BTNcancelUserRevisionRequest" runat="server" CssClass="Link_Menu" Visible="false"/>
                                <asp:Button ID="BTNrefuseUserRequest" runat="server" CssClass="Link_Menu"  Visible="false"/>
                                <asp:Button ID="BTNacceptUserRequest" runat="server" CssClass="Link_Menu" Visible="false"/>
                                <asp:Button ID="BTNrefuseRevision" runat="server" CssClass="Link_Menu"  Visible="false"/>
                                <asp:Button ID="BTNacceptRevision" runat="server" CssClass="Link_Menu" Visible="false"/>
                                <asp:Button ID="BTNsaveRevisionSettings" runat="server" CssClass="Link_Menu" Visible="false"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="DVmessages" class="messages" runat="server" visible="false">
                    <div class="message alert">
                        <asp:Label ID="LBdisplayMessage" runat="server"></asp:Label>
                    </div>
                </div>
                <asp:MultiView ID="MLVpreview" runat="server">
                    <asp:View ID="VIWempty" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="LBemptyMessage" runat="server"></asp:Label>
                        <br />
                        <br />
                        <br />
                        <br />
                    </asp:View>
                    <asp:View ID="VIWcall" runat="server">
                        <fieldset class="section collapsable cfpintro collapsed" runat="server" id="FLDcallInfo" visible="false">
                            <legend>
                                <span class="switchsection handle">&nbsp;</span>
                                <span class="title">
                                    <asp:Label ID="LBcallDescriptionTitle" runat="server"></asp:Label>
                                </span>
                                </legend>
                            <div class="cfpdescription">
                                <div class="renderedtext"><asp:Literal ID="LTcallDescription" runat="server" /></div>
                            </div>
                        </fieldset>
                        <asp:Repeater ID="RPTsections" runat="server">
                            <ItemTemplate>
                                <fieldset class="section collapsable">
                                    <legend>
                                        <span class="switchsection handle">&nbsp;</span>
                                        <span class="title">
                                            <asp:Literal ID="LTsectionTitle" runat="server"></asp:Literal>
                                        </span>
                                        </legend>
                                    <div class="sectiondescription">
                                        <asp:Literal ID="LTsectionDescription" runat="server"></asp:Literal>
                                    </div>
                                    <asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>"
                                        OnItemDataBound="RPTfields_ItemDataBound">
                                        <ItemTemplate>
                                            <CTRL:CTRLrenderField ID="CTRLrenderField" runat="server" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </fieldset>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
      <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>