<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditSettings.aspx.vb" Inherits="Comunita_OnLine.EditCallSettings" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="COL" TagName="Dialog" Src="~/Modules/EduPath/UC/UCDialog.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register Src="~/Modules/CallForPapers/Edit/UC/UC_PrintSettings.ascx" TagPrefix="COL" TagName="UC_PrintSettings" %>



<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" />
    <style type="text/css">
        .tableCell{
            display:table-cell !important;
        }
        label.inline.tableCell{
            display:table-cell !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#confirmStatus').dialog({
                appendTo: "form",
                closeOnEscape: false,
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 500,
                height: 350,
                minHeight: 200,
                //                minWidth: 700,
                zIndex: 1000,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }

            });

          



        });

        function showDialog(id) {            

            $('#' + id).dialog("open");
            return false;
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
    </script>
    <asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
        <script language="javascript">
            $(function () {
                showDialog("confirmStatus");
            });
        </script>
    </asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView id="MLVsettings" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <CTRL:Messages ID="CTRLemptyMessage"  runat="server"/>
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
                        <asp:button ID="BTNsaveSettingsTop" runat="server" Text="Save"/>
                    </div>
                     <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                </div>
                <div class="contentouter">
                    <div class="content">
                       <!-- @Start General Settings -->
                            <fieldset>
                                <div class="fieldobject">
                                    <div class="fieldrow fieldtitle">
                                        <asp:Label ID="LBtitle_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="TXBtitle">Title:</asp:Label>
                                        <asp:TextBox ID="TXBtitle" runat="server" Columns="60" CssClass="Testo_campo_obbligatorio"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtitle" runat="server" ControlToValidate="TXBtitle"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="fieldrow fieldedition">
                                        <asp:Label ID="LBedition_t" runat="server" AssociatedControlID="TXBedition" CssClass="Titolo_campo fieldlabel">Edition:</asp:Label>
                                        <asp:TextBox ID="TXBedition" runat="server" Columns="60" CssClass="Testo_Campo"></asp:TextBox>
                                    </div>
                                    <div class="fieldrow fieldstatus">
                                        <asp:Label ID="LBstatus" runat="server" AssociatedControlID="DDLstatus" CssClass="Titolo_campo fieldlabel">Status:</asp:Label>
                                        <asp:DropDownList ID="DDLstatus" runat="server" CssClass="Testo_Campo"></asp:DropDownList>
                                    </div>
                                    
                                    <div class="fieldrow fieldadvCommission" id="DVadvancedCommission" runat="server">
                                    
                                    <%--<div id="DVadvancedCommission" class="fieldrow fieldstatus" runat="server">--%>
                                        <asp:Label ID="LBcommissionType_t" runat="server" AssociatedControlID="DDLstatus" CssClass="Titolo_campo fieldlabel">Gestione commissioni:</asp:Label>
                                        <asp:RadioButtonList ID="RBLcommissionType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Classica" Selected="True" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Avanzata" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    <%--</div>--%>
                                        </div>
                                    <div class="fieldrow short">
                                        <asp:Label ID="LBsummary_t" runat="server" AssociatedControlID="CTRLeditorSummary" CssClass="Titolo_campo fieldlabel">Summary:</asp:Label>
                                        <CTRL:CTRLeditor id="CTRLeditorSummary" runat="server" ContainerCssClass="containerclass" 
                                            LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" EditorHeight="220px" MaxTextLength="30000"/>
                                    </div>
                                    <div class="fieldrow long">
                                        <asp:Label ID="LBdescription_t" runat="server" AssociatedControlID="CTRLeditorSummary" CssClass="Titolo_campo fieldlabel">Description:</asp:Label>

                                        <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass" 
                                            LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" EditorHeight="300px" MaxTextLength="100000"/>
                                    </div>
                                </div>
                            </fieldset>
                            <fieldset>
                            
                                <div class="fieldobject">
                                    <div class="fieldrow fielddaterange">
                                        <asp:Label ID="LBtimeValidity_t" runat="server" Visible="false">Time validity</asp:Label>
                                        <asp:Label ID="LBfromDate_t" runat="server" AssociatedControlID="RDPfromDay" CssClass="Titolo_campo alignr first">from</asp:Label>
                                        <telerik:RadDateTimePicker id="RDPfromDay" runat="server" ></telerik:RadDateTimePicker>
                                        <asp:Label ID="LBtoDate_t" runat="server" AssociatedControlID="RDPtoDay" CssClass="Titolo_campo alignr last">to:</asp:Label>
                                        <telerik:RadDateTimePicker id="RDPtoDay" runat="server" ></telerik:RadDateTimePicker>
                                        <span class="labelinput">
                                            <asp:CheckBox ID="CBXsubmissionClosed" runat="server" CssClass="testo_campoSmall" />
                                            <asp:Label ID="LBsubmissionClosed" runat="server" CssClass="inline" AssociatedControlID="CBXsubmissionClosed">Chiudi temporaneamente le sottomissioni<</asp:Label>
                                        </span>
                                     </div>
                                     <div class="fieldrow fieldendtime">
                                         <span class="description">
                                            <asp:Label ID="LBoverrideData" runat="server">Consenti di estendere la data di termine di:</asp:Label>
                                            <asp:DropDownList ID="DDLhours" CssClass="testo_campoSmall" runat="server">
                                                <asp:ListItem Value="0" Text="00"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="01"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="02"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="03"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="04"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="05"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="06"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="LBhours" runat="server" AssociatedControlID="DDLhours" CssClass="alignr first">ore</asp:Label>
                                            <asp:DropDownList ID="DDLminutes" CssClass="testo_campoSmall" runat="server">
                                                <asp:ListItem Value="0" Text="00"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="05"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                                <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                                <asp:ListItem Value="45" Text="45"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="LBminutes" runat="server" CssClass="alignr last" AssociatedControlID="DDLminutes">minuti</asp:Label>
                                        
                                        </span>
                                    </div>                    
                                     <div class="fieldrow fieldextratime">
                                         <asp:CheckBox ID="CBXallowExtensionDate" runat="server" CssClass="inputtext tableCell" />
                                         <asp:Label ID="LBallowExtensionDate" runat="server" AssociatedControlID="CBXallowExtensionDate" CssClass="inline tableCell">Utilizza la data di inizio della compilazione anziché quella di invio per l'ammissione del bando entro i termini indicati.</asp:Label>
                                    </div>
                                    <div class="fieldrow" id="DVacceptRefusePolicy" runat="server">
                                        <span class="description">
                                            <asp:Label ID="LBAcceptRefusePolicy_t" runat="server" AssociatedControlID="RBLacceptRefusePolicy">Fase accettazione domande:</asp:Label>
                                        </span>
                                        <asp:RadioButtonList ID="RBLacceptRefusePolicy" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" RepeatColumns="2">
                                            <asp:ListItem Selected="True" Value="None">Nessuna notifica</asp:ListItem>
                                            <asp:ListItem Value="Accept">Notifica accettazione domanda</asp:ListItem>
                                            <asp:ListItem Value="Refuse">Notifica rifiuto domanda</asp:ListItem>
                                            <asp:ListItem Value="All">Notifica accettazione/rifiuto domanda</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="fieldrow" id="DVrevision" runat="server" visible="false">
                                        <asp:Label ID="LBallowRevision_t" runat="server" AssociatedControlID="RBLrevisionType" CssClass="fieldlabel">Revisioni domande:</asp:Label>
                                        <asp:RadioButtonList ID="RBLrevisionType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Value="None">Nessuna</asp:ListItem>
                                            <asp:ListItem Value="OnlyManager">Solo al gestore/i</asp:ListItem>
                                            <asp:ListItem Value="ManagerSubmitter">Al/i gestore/i e al partecipante</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </fieldset>
                        
                            <fieldset id="FLDSsignMandatory" runat="server">
                                <div class="fieldobject">
                                     <div class="fieldobject">
                                        <div class="fieldrow">
                                             <asp:Label ID="LBsignMandatory" runat="server" AssociatedControlID="CBXsignMandatory" CssClass="Titolo_campo fieldlabel">*Richiedi controfirma:</asp:Label>
                                            <asp:CheckBox ID="CBXsignMandatory" runat="server" Checked="False" />
                                        </div>
                                    </div>
                                </div>
                            </fieldset>

                            <fieldset>
                                <div class="fieldobject">
                                    <COL:UC_PrintSettings runat="server" ID="CTRLprintSet" />
                                </div>
                            </fieldset>

                            <fieldset runat="server" id="FLDevaluation" visible="false">
                                <!--<legend class="inlinelegend"><input class="inputtext" type="checkbox" /><label for="">Attiva sistema di valutazione</label></legend>-->              
                                <div class="fieldobject">
                                    <div class="fieldrow">
                                        <asp:Label ID="LBendEvaluationOn" runat="server" AssociatedControlID="RDPendEvaluationOn"
                                        CssClass="Titolo_campo fieldlabel">Valutazioni concluse entro il:</asp:Label>
                                        <telerik:RadDateTimePicker id="RDPendEvaluationOn" runat="server" ></telerik:RadDateTimePicker>
                                    </div>
                                    <div class="fieldrow" id="DVevaluationMessage" runat="server" visible="false">
                                        <CTRL:Messages ID="CTRLevaluationMessage"  runat="server"/>
                                    </div>
                                    <div class="fieldrow">
                                        <asp:Label ID="LBevaluationResults_t" runat="server" AssociatedControlID="DDLevaluationType" CssClass="Titolo_campo fieldlabel">*Risultati valutazione:</asp:Label>
                                        <asp:DropDownList ID="DDLevaluationType" runat="server" CssClass="Testo_Campo">
                                            <asp:ListItem Value="0"></asp:ListItem>
                                            <asp:ListItem Value="1"></asp:ListItem>
                                            <asp:ListItem Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <!--<div class="fieldrow">
                                         <asp:Label ID="LBdisplayWinner" runat="server" AssociatedControlID="CBXdisplayWinner" CssClass="Titolo_campo fieldlabel">*Mostra vincitore:</asp:Label>
                                         <asp:CheckBox ID="CBXdisplayWinner" runat="server" CssClass="inputtext" />
                                    </div>
                                    <div class="fieldrow">
                                        <asp:Label ID="LBAwardDate" runat="server" AssociatedControlID="TXBawardDate" CssClass="Titolo_campo fieldlabel">*Risultati pubblici:</asp:Label>
                                        <asp:TextBox ID="TXBawardDate" runat="server" CssClass="Testo_Campo" Columns="60" />
                                    </div>-->
                                </div>
                            </fieldset>
                        <!-- @End General Settings -->
                    </div>
                </div>
                <div class="footer">
                    <div class="DivEpButton">
                        <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                        <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                        <asp:button ID="BTNsaveSettingsBottom" runat="server" Text="Save"/>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="confirmStatus" style="display:none;">
        <div id="DVconfirmStatus" runat="server" visible="false">
            <div class="columnsEdit">
                <div class="column left w20">
                    &nbsp;
                </div>
                <div class="column right w70">
                    <asp:Label ID="LBinfoConfirmStatus" runat="server" CssClass="testo_campoSmall"></asp:Label>
                </div>
            </div>
            <div class="columnsEdit" id="DVendDate" runat="server">
                <div class="column left w20">
                    <asp:Label ID="LBconfirmEndDate" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                </div>
                <div class="column right w70">
                    <telerik:RadDateTimePicker id="RDPconfirmEndDay" runat="server" ></telerik:RadDateTimePicker>
                </div>
            </div>
            <div class="columnsEdit">
                <div class="column left w20">
                </div>
                <div class="column right w70 DivEpButton">
                    <asp:Button ID="BTNundoConfirmStatus" runat="server" Text="Undo" 
                        CssClass="Link_Menu" />
                    <asp:Button ID="BTNconfirmStatus" runat="server" Text="Conferma" CssClass="Link_Menu" />
                </div>
            </div>
        </div>
    </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>