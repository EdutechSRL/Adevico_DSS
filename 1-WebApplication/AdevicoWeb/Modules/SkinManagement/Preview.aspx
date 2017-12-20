<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master"
    CodeBehind="Preview.aspx.vb" Inherits="Comunita_OnLine.PreviewSkin" %>

<%@ MasterType VirtualPath="~/ExternalService.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../../Questionari/stile.css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <% Select Case Me.CurrentModule%>
    <% Case "SRVRFM", "SRVCFP"%>
    <link href="../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.checkboxList.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.textVal.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
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

            $("fieldset.section legend").click(function () {
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
    </script>
    <% End Select%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div runat="server" id="DVstandard">
        <br />
        <span class="Titolo_campo">Titolo campo:</span> &nbsp; <span class="Testo_Campo">Testo
            Campo</span>
        <br />
        <span class="Titolo_campo">Titolo campo:</span> &nbsp;
        <input type="text" class="Testo_Campo" value="Testo campo" />
        <br />
        <br />
        <span>Testo normale.<br />
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam accumsan ultrices
            malesuada. Quisque malesuada aliquet lectus in ultrices. Cras convallis sem sed
            dolor feugiat sed feugiat arcu congue. Aenean non nibh eget erat euismod accumsan.
            Vivamus scelerisque libero ac lorem ultricies a suscipit neque pellentesque. Aenean
            est massa, eleifend et sagittis eget, tincidunt sed mauris. Sed ut purus tortor,
            in venenatis est. Fusce non mi luctus magna ultrices varius eget vitae lectus. Morbi
            tincidunt erat eget nisl mollis luctus. Integer suscipit, nunc nec dictum vehicula,
            nibh odio venenatis est, id. </span>
        <br />
        <br />
        <a href="#">Link normale</a> &nbsp;&nbsp;&nbsp; <a href="#" class="Link_Menu">Link Menu</a>
        &nbsp;&nbsp;&nbsp;
        <input type="button" class="Link_Menu" value="Bottone" />
        <br />
        <br />
        <table cellspacing="0" cellpadding="0" border="1" style="width: 600px; border-collapse: collapse;">
            <thead>
                <tr class="ROW_header_Small_Center">
                    <td class="ROW_header_Small_Center">
                        Nome colonna
                    </td>
                    <td class="ROW_header_Small_Center">
                        Nome colonna
                    </td>
                    <td class="ROW_header_Small_Center">
                        Nome colonna
                    </td>
                </tr>
            </thead>
            <tbody>
                <tr class="ROW_Normal_Small">
                    <td class="ROW_Normal_Small">
                        Testo riga normale
                    </td>
                    <td class="ROW_Normal_Small">
                        &nbsp;
                    </td>
                    <td class="ROW_Normal_Small">
                        Testo riga normale
                    </td>
                </tr>
                <tr class="ROW_Alternate_Small">
                    <td class="ROW_Alternate_Small">
                        &nbsp;
                    </td>
                    <td class="ROW_Alternate_Small">
                        Testo riga normale
                    </td>
                    <td class="ROW_Alternate_Small">
                        &nbsp;
                    </td>
                </tr>
                <tr class="ROW_Normal_Small">
                    <td class="ROW_Normal_Small">
                        Testo riga normale
                    </td>
                    <td class="ROW_Normal_Small">
                        &nbsp;
                    </td>
                    <td class="ROW_Normal_Small">
                        Testo riga normale
                    </td>
                </tr>
                <tr class="ROW_Disattivate_Small">
                    <td class="ROW_Disattivate_Small">
                        Riga disattivata
                    </td>
                    <td class="ROW_Disattivate_Small">
                        <a href="#" class="ROW_ItemLink_Small">Testo link</a>
                    </td>
                    <td class="ROW_Disattivate_Small">
                        Riga disattivata
                    </td>
                </tr>
                <tr class="ROW_Disabilitate_Small">
                    <td class="ROW_Disabilitate_Small">
                        Riga disabilitata
                    </td>
                    <td class="ROW_Disabilitate_Small">
                        <a href="#" class="ROW_ItemLink_Small">Testo link</a>
                    </td>
                    <td class="ROW_Disabilitate_Small">
                        Riga disabilitata
                    </td>
                </tr>
                <tr class="ROW_Alternate_Small">
                    <td class="ROW_Alternate_Small">
                        Testo riga normale
                    </td>
                    <td class="ROW_Alternate_Small">
                        <a href="#" class="ROW_ItemLink_Small">Testo link</a>
                    </td>
                    <td class="ROW_Alternate_Small">
                        &nbsp;
                    </td>
                </tr>
                <tr class="ROW_Normal_Small">
                    <td class="ROW_Normal_Small">
                        &nbsp;
                    </td>
                    <td class="ROW_Normal_Small">
                        <a href="#" class="ROW_ItemLink_Small">Testo link</a>
                    </td>
                    <td class="ROW_Normal_Small">
                        Testo riga normale
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr class="ROW_Page_Small">
                    <td colspan="3" align="center">
                        Footer tabella
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    <div runat="server" id="DVmodules" visible="false">
        <% Select Case Me.CurrentModule%>
        <% Case "SRVQUST"%>
        <%--QUESTIONARI--%>
        <div>
            <span>Nome questionario</span>
            <br />
            <br />
            <table cellspacing="0" cellpadding="4" border="0" style="color: #333333; border-width: 1px;
                border-style: solid; width: 100%; border-collapse: collapse;">
                <tbody>
                    <tr>
                        <td style="background-color: #EFF3FB;">
                            <b>--</b>
                            <br />
                            <i></i>
                            <hr />
                            <table cellspacing="0" border="0" style="width: 100%; border-collapse: collapse;"
                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande">
                                <tbody>
                                    <tr>
                                        <td style="background-color: White;">
                                            <div style="text-align: right;">
                                                (Cod.### <span>- Diff. ###</span>)
                                            </div>
                                            <br />
                                            <span>1</span> Testo domanda 1 - Scelta multipla
                                            <br />
                                            <br />
                                            <table border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="Risposte">
                                                            <input type="radio" value="ctl01" name="RBL_multiple" id="Radio1"><span>Opzione 1
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Risposte">
                                                            <input type="radio" value="ctl02" name="RBL_multiple" id="Radio2"><span>Opzione 2</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Risposte">
                                                            <input type="radio" value="ctl03" name="RBL_multiple" id="Radio3"><span>Opzione 2</span>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: White;">
                                            <div style="text-align: right;">
                                                (Cod.### <span>- Diff. ###</span>)
                                            </div>
                                            <br />
                                            <span>2</span> Testo domanda 2 - testo libero
                                            <br />
                                            <br />
                                            <table border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="Risposte">
                                                            <span></span>
                                                        </td>
                                                        <td>
                                                            <textarea style="width: 500px;" cols="20" rows="3" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl02$ctl01"></textarea>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #EFF3FB;">
                                            <div style="text-align: right;">
                                                (Cod.### <span>- Diff. ###</span>)
                                            </div>
                                            <br />
                                            <span>3</span> Testo domanda 3 - Numerica
                                            <br />
                                            <br />
                                            <table border="0" id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_TBLNumerica_3">
                                                <tbody>
                                                    <tr>
                                                        <td class="Risposte">
                                                            <span>Testo uno:</span>
                                                        </td>
                                                        <td class="Risposte">
                                                            <input type="text" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl03$TXBTestoNumerica_0">
                                                            <span></span><span style="color: Red; display: none;" id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_COVOpzioneNumerica_0">
                                                                Inserire un numero. </span><span style="color: Red; display: none;" id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_CVisOverflowOpzioneNumerica_0">
                                                                    Il numero inserito è troppo grande.</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Risposte">
                                                            <span>Testo due:</span>
                                                        </td>
                                                        <td class="Risposte">
                                                            <input type="text" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl03$TXBTestoNumerica_1">
                                                            <span></span><span style="color: Red; display: none;">Inserire un numero. </span>
                                                            <span style="color: Red; display: none;">Il numero inserito è troppo grande.</span>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #EFF3FB;">
                                            <div style="text-align: right;">
                                                (Cod.### <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_LBDifficoltaTesto">
                                                    - Diff. ###</span>)
                                            </div>
                                            <br />
                                            <span>4</span> Testo domanda 4 - Rating
                                            <br />
                                            <br />
                                            <table cellspacing="0" cellpadding="10" border="0" style="border-width: 1px; border-style: solid;
                                                width: 810px; border-collapse: collapse;">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 0px;" class="CellaVuota">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <span>1</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <span>2</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <span>3</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <span>4</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <span>5</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="CellaDomanda">
                                                            <span>Elemento_1</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl06" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl06">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl07" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl07">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl08" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl08">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl09" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl09">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl10" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl10">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="CellaDomanda">
                                                            <span>Elemento_2</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl12" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl12">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl13" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl13">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl14" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl14">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl15" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl15">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl16" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl16">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="CellaDomanda">
                                                            <span>Elemento_3</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl18" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl18">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl19" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl19">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl20" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl20">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl21" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl21">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl22" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl22">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="CellaDomanda">
                                                            <span>Elemento_4</span>
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl24" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl24">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl25" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl25">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl26" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl26">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl27" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl27">
                                                        </td>
                                                        <td class="CellaRisposta">
                                                            <input type="radio" value="ctl28" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                                id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl28">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #EFF3FB;">
                                            <div style="text-align: right;">
                                                (Cod.### <span>- Diff. ###</span>)
                                            </div>
                                            <br />
                                            <span>5</span> Testo domanda 5 - Dropdown
                                            <br />
                                            <br />
                                            <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl01_UCDomandaDropDown_1_LBEtichettaDropDown">
                                            </span>
                                            <select id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl01_UCDomandaDropDown_1_DDLOpzioni"
                                                name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl01$UCDomandaDropDown_1$DDLOpzioni">
                                                <option value="1">Risposta 1</option>
                                                <option value="2">Risposta 2</option>
                                                <option value="3">Risposta 3</option>
                                                <option value="4">Risposta 4</option>
                                                <option value="5">Risposta 5</option>
                                                <option value="6">Risposta 6</option>
                                            </select>
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br />
                            <div class="NomePaginaFooter">
                                --
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <% Case "SRVCFP"%>
        <fieldset class="section collapsable cfpintro">
            <legend>Introduction</legend>
            <div class="cfpdescription">
                <p>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris faucibus mi mi.
                    Maecenas auctor ullamcorper tincidunt. Praesent placerat, velit in luctus dignissim,
                    eros purus cursus tellus, nec facilisis massa urna ac arcu. Nam quam nisl, tristique
                    quis rutrum id, vestibulum eu sem. Lorem ipsum dolor sit amet, consectetur adipiscing
                    elit. Donec a fringilla dolor. Nunc justo ipsum, dignissim eu faucibus vel, tincidunt
                    ullamcorper mi. Aliquam nec mauris lorem, eu suscipit nisl. Nulla et risus eu mauris
                    dictum faucibus id in nibh.
                </p>
                <p>
                    Suspendisse metus lacus, dignissim in convallis et, pellentesque ut ipsum. Aliquam
                    erat volutpat. Phasellus semper consectetur nibh, nec ullamcorper orci semper semper.
                    Phasellus tempus porttitor arcu, vel eleifend magna mollis porttitor. Donec velit
                    arcu, varius et commodo eget, lacinia sed felis. Pellentesque ipsum quam, tempus
                    at condimentum sit amet, dapibus cursus libero. Phasellus nec varius quam. Nulla
                    sed quam arcu. Donec volutpat dapibus mi vitae pulvinar. Nulla facilisi.
                </p>
            </div>
            <div class="cfpdetail">
                <span class="expiration error">Validity: <span class="startdate">01/01/2012</span>&nbsp;-&nbsp;<span
                    class="enddate">01/03/2012</span> </span><span class="winnerinfo">winner publication
                        at 01/04/2012 </span>
            </div>
        </fieldset>
        <fieldset class="section partecipants">
            <legend>Partecipant Type</legend>
            <div class="sectiondescription">
                <div class="messages">
                    <div class="message">
                        In modalit&agrave; l'anteprima la lista di selezioni tipo di partecipante &egrave;
                        disabilitata.<br />
                        Selezionare un tipo di partecipante dal menu a tendina superiore per visualizzarne
                        l'anteprima.
                    </div>
                </div>
            </div>
            <div class="fieldrow">
                <span id="Span1" class="rbldl">
                    <input id="Radio4" type="radio" name="ctl00$MainContent$rbl" value="&lt;dl&gt;&lt;dt&gt;Partecipante 1&lt;/dt&gt;&lt;dd&gt;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat, est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus, id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.&lt;/dd&gt;&lt;/dl&gt;"><label
                        for="MainContent_rbl_0"><dl>
                            <dt>Partecipante 1</dt><dd class="hidden">Lorem ipsum dolor sit amet, consectetur adipiscing
                                elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus
                                vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum
                                ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam
                                quis libero erat, et feugiat magna.</dd></dl>
                    </label>
                    <br>
                    <input id="Radio5" type="radio" name="ctl00$MainContent$rbl" value="&lt;dl&gt;&lt;dt&gt;Partecipante 2&lt;/dt&gt;&lt;dd&gt;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat, est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus, id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.&lt;/dd&gt;&lt;/dl&gt;"><label
                        for="MainContent_rbl_1"><dl>
                            <dt>Partecipante 2</dt><dd class="hidden">Nulla gravida, quam eu porttitor consequat,
                                est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet
                                neque. Aliquam scelerisque porttitor turpis quis commodo.
                            </dd>
                        </dl>
                    </label>
                    <br>
                    <input id="Radio6" type="radio" name="ctl00$MainContent$rbl" value="&lt;dl&gt;&lt;dt&gt;Partecipante 3&lt;/dt&gt;&lt;dd&gt;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat, est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus, id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.&lt;/dd&gt;&lt;/dl&gt;"><label
                        for="MainContent_rbl_2"><dl>
                            <dt>Partecipante 3</dt><dd class="hidden">Aliquam eleifend erat vestibulum purus interdum
                                ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed,
                                eleifend in velit.</dd></dl>
                    </label>
                </span>
            </div>
        </fieldset>
        <fieldset class="section collapsable attachments">
            <legend>Attached Files</legend>
            <div class="fieldobject">
                <div class="fieldrow">
                    <ul class="attachedfiles">
                        <li class="attachedfile"><span class="iteminfo"><span class="name"><span class="actionbuttons">
                            <span class="fileIco extdoc" title="">&nbsp;</span> </span>01 - Abstract bando.doc
                        </span><span class="itemdetail">(1.17 mb)</span> </span>
                            <div class="cfpdescription">
                                description
                            </div>
                        </li>
                        <li class="attachedfile"><span class="iteminfo"><span class="name"><span class="actionbuttons">
                            <span class="fileIco extdoc" title="">&nbsp;</span> </span>02 - Requisiti.doc </span>
                            <span class="itemdetail">(1.17 mb)</span> </span>
                            <div class="cfpdescription">
                                description
                            </div>
                        </li>
                        <li class="attachedfile"><span class="iteminfo"><span class="name"><span class="actionbuttons">
                            <span class="fileIco extdoc" title="">&nbsp;</span> </span>03 - Informativa Privacy.doc
                        </span><span class="itemdetail">(1.17 mb)</span> </span>
                            <div class="cfpdescription">
                                description
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </fieldset>
        <fieldset class="section collapsable">
            <legend>Section</legend>
            <div class="sectiondescription">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sagittis urna
                sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et vehicula elit.
                Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada ut, accumsan
                et tortor. In eget velit sed erat lobortis posuere. Suspendisse non lacus sed dolor
                tincidunt accumsan. Duis porttitor tincidunt sagittis.
            </div>
            <div class="fieldobject singleline">
                <div class="fieldrow fieldinput">
                    <label class="" for="">
                        Lorem<span class="required">*</span></label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <input class="inputtext " maxlength="10" type="text"><span class="inlinetooltip">Help</span>
                    <br />
                    <span class="fieldinfo "><span class="maxchar ">Caratteri disponibili: <span class="availableitems">
                    </span>/<span class="totalitems"></span> </span><span class="generic "></span></span>
                </div>
            </div>
            <div class="fieldobject multiline">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Label</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <textarea class="textarea" maxlength="10"></textarea><span class="inlinetooltip">Help</span>
                    <br />
                    <span class="fieldinfo "><span class="maxchar ">Caratteri disponibili: <span class="availableitems">
                    </span>/<span class="totalitems"></span> </span><span class="generic"></span></span>
                </div>
            </div>
            <div class="fieldobject disclaimer">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Label</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <div class="disclaimerwrapper">
                        <div class="disclaimertext">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam quis molestie purus.
                            Nulla facilisi. Curabitur lacinia, nibh sed vehicula vehicula, diam mi sollicitudin
                            felis, a commodo risus nulla nec diam. Quisque eu quam sodales arcu cursus fringilla
                            cursus sit amet est. Proin sit amet purus eget metus cursus ullamcorper. Aenean
                            ante mi, venenatis ac ornare nec, porta non erat. Nulla tempor, nibh accumsan elementum
                            imperdiet, erat magna scelerisque enim, non varius ligula elit id purus. Lorem ipsum
                            dolor sit amet, consectetur adipiscing elit. Maecenas ante elit, sollicitudin a
                            mollis id, sollicitudin quis erat. Nunc et sem libero, nec eleifend velit. Quisque
                            sagittis ullamcorper sodales. Donec justo lacus, vestibulum et ullamcorper a, volutpat
                            eget mauris. Vestibulum hendrerit lectus ac sapien accumsan in laoreet velit consectetur.
                            Ut condimentum interdum lectus sit amet bibendum. Aenean nec metus nisl, at mollis
                            diam. In turpis nisl, bibendum vitae eleifend sit amet, vehicula ac orci. Nam luctus
                            facilisis urna quis molestie. Lorem ipsum dolor sit amet, consectetur adipiscing
                            elit. Praesent vehicula vehicula nibh vitae bibendum. Integer commodo, nisl ac lobortis
                            bibendum, massa sapien aliquet quam, a elementum nisl mauris pellentesque magna.
                            Cras ac sapien ligula. Ut facilisis elit vitae nisl hendrerit blandit. Cum sociis
                            natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Suspendisse
                            convallis feugiat leo sit amet tincidunt. Praesent eget volutpat massa. Morbi ultricies,
                            erat at malesuada mollis, nisi tortor ullamcorper quam, vitae feugiat mi eros a
                            nulla. Curabitur id accumsan felis. Etiam dignissim mi eleifend turpis mattis id
                            porta erat pulvinar.</div>
                        <div class="disclaimerinput">
                            <label class="disclaimerlabel">
                                Accept</label><input type="radio" name="disclaimer"><label class="disclaimerlabel">Refuse</label><input
                                    type="radio" name="disclaimer" checked>
                        </div>
                    </div>
                </div>
            </div>
            <div class="fieldobject note">
                <div class="fieldrow fielddescription">
                    <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                        sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                        vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                        ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                        lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                </div>
            </div>
            <div class="fieldobject fileupload">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Label</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <input type="file" /><span class="inlinetooltip">Help</span>
                </div>
            </div>
        </fieldset>
        <fieldset class="section collapsable">
            <legend>Section</legend>
            <div class="sectiondescription">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sagittis urna
                sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et vehicula elit.
                Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada ut, accumsan
                et tortor. In eget velit sed erat lobortis posuere. Suspendisse non lacus sed dolor
                tincidunt accumsan. Duis porttitor tincidunt sagittis.
            </div>
            <div class="fieldobject date">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Label</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <input class="inputtext" type="text"><span class="inlinetooltip">Help</span>
                </div>
            </div>
            <div class="fieldobject time">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Label</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <input class="inputtext" type="text"><input class="inputtext" type="text"><span class="inlinetooltip">Help</span>
                </div>
            </div>
            <div class="fieldobject datetime">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Label</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <input class="inputtext" type="text"><input class="inputtext" type="text"><input
                        class="inputtext" type="text"><span class="inlinetooltip">Help</span>
                </div>
            </div>
            <div class="fieldobject radiobuttonlist">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Lorem</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <span id="Span2" class="inputradiobuttonlist">
                        <input id="Radio8" type="radio" name="rbnlistA" value="Option 1"><label for="ArbnlistA_0">Option
                            1</label><br>
                        <input id="Radio9" type="radio" name="rbnlistA" value="Option 2"><label for="ArbnlistA_1">Option
                            2</label><br>
                        <input id="Radio10" type="radio" name="rbnlistA" value="Option 3"><label for="ArbnlistA_2">Option
                            3</label><br>
                        <input id="Radio11" type="radio" name="rbnlistA" value="Option 4"><label for="ArbnlistA_3">Option
                            4</label><br>
                    </span><span class="inlinetooltip">Help</span>
                </div>
            </div>
            <div class="fieldobject checkboxlist">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Lorem Ipsum Dolor Sit</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <span id="Span3" class="checkboxlist min-1 max-4">
                        <input id="Checkbox1" type="checkbox" name="chblistA$0" value="Option 1"><label for="AchblistA_0">Option
                            1</label><br>
                        <input id="Checkbox2" type="checkbox" name="chblistA$1" value="Option 2"><label for="AchblistA_1">Option
                            2</label><br>
                        <input id="Checkbox3" type="checkbox" name="chblistA$2" value="Option 3"><label for="AchblistA_2">Option
                            3</label><br>
                        <input id="Checkbox4" type="checkbox" name="chblistA$3" value="Option 4"><label for="AchblistA_3">Option
                            4</label><br>
                    </span><span class="inlinetooltip">Help</span>
                    <br />
                    <span class="fieldinfo "><span class="maxchar ">Caratteri disponibili: <span class="availableitems">
                    </span>/<span class="totalitems"></span> </span><span class="minmax ">Min <span class="min">
                        1 risposta</span> / Max <span class="max">4 risposte</span> </span><span class="generic">
                            Bla bla bla bla bla bla </span></span>
                </div>
            </div>
            <div class="fieldobject dropdownlist">
                <div class="fieldrow fieldinput">
                    <label for="">
                        Lorem Ipsum Dolor Sit</label>
                    <div class="fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                    <select>
                        <option>Option 1</option>
                        <option>Option 2</option>
                        <option>Option 3</option>
                        <option>Option 4</option>
                    </select>
                    <span class="inlinetooltip">Help</span>
                </div>
            </div>
        </fieldset>
        <div class="CFPBoxes">
            <div class="CFPBox boxsave left">
                <a href="#" class="linkMenu" title="Save">Save</a>
                <div class="cfpdescription">
                    Salva la domanda e potrai modificarla in un secondo momento. La domanda non è sottomessa
                    (e quindi valida) fino a quando non verrà premuto sottometti definitivamente
                </div>
            </div>
            <div class="CFPBox boxsubmit right">
                <a href="#" class="linkMenu" title="Submit">Submit</a>
                <div class="cfpdescription">
                    Sottometti definitivamente la domanda affinchè venga valutata. Non sarà più possibile
                    modificare la domanda
                </div>
            </div>
        </div>
        <% Case "SRVRFM"%>
        <div class="view preview">
            <div class="cfpdescription">
                <div class="textwrapper">
                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris faucibus mi mi.
                        Maecenas auctor ullamcorper tincidunt. Praesent placerat, velit in luctus dignissim,
                        eros purus cursus tellus, nec facilisis massa urna ac arcu. Nam quam nisl, tristique
                        quis rutrum id, vestibulum eu sem. Lorem ipsum dolor sit amet, consectetur adipiscing
                        elit. Donec a fringilla dolor. Nunc justo ipsum, dignissim eu faucibus vel, tincidunt
                        ullamcorper mi. Aliquam nec mauris lorem, eu suscipit nisl. Nulla et risus eu mauris
                        dictum faucibus id in nibh.
                    </p>
                    <p>
                        Suspendisse metus lacus, dignissim in convallis et, pellentesque ut ipsum. Aliquam
                        erat volutpat. Phasellus semper consectetur nibh, nec ullamcorper orci semper semper.
                        Phasellus tempus porttitor arcu, vel eleifend magna mollis porttitor. Donec velit
                        arcu, varius et commodo eget, lacinia sed felis. Pellentesque ipsum quam, tempus
                        at condimentum sit amet, dapibus cursus libero. Phasellus nec varius quam. Nulla
                        sed quam arcu. Donec volutpat dapibus mi vitae pulvinar. Nulla facilisi.
                    </p>
                </div>
                <div class="cfpdetail">
                    <span class="expiration">Validity: <span class="startdate">01/01/2012</span>&nbsp;-&nbsp;<span
                        class="enddate">01/03/2012</span> </span><span class="winnerinfo">winner publication
                            at 01/04/2012 </span>
                </div>
            </div>
            <fieldset class="section partecipants">
                <legend>Partecipant Type</legend>
                <div class="sectiondescription">
                    Aenean at risus turpis, vel congue leo. Morbi elementum, odio et tempus interdum,
                    purus massa porttitor massa, nec luctus metus nibh non nibh. Aenean tortor elit,
                    rutrum in fringilla a, adipiscing a mi.
                </div>
                <div class="fieldrow">
                    <span id="MainContent_rbl" class="rbldl">
                        <input id="MainContent_rbl_0" type="radio" name="ctl00$MainContent$rbl" value="&lt;dl&gt;&lt;dt&gt;Partecipante 1&lt;/dt&gt;&lt;dd&gt;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat, est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus, id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.&lt;/dd&gt;&lt;/dl&gt;"><label
                            for="MainContent_rbl_0"><dl>
                                <dt>Partecipante 1</dt><dd class="hidden">Lorem ipsum dolor sit amet, consectetur adipiscing
                                    elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus
                                    vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum
                                    ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam
                                    quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat,
                                    est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet
                                    neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus,
                                    id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat
                                    sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus
                                    placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.</dd></dl>
                        </label>
                        <br>
                        <input id="MainContent_rbl_1" type="radio" name="ctl00$MainContent$rbl" value="&lt;dl&gt;&lt;dt&gt;Partecipante 2&lt;/dt&gt;&lt;dd&gt;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat, est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus, id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.&lt;/dd&gt;&lt;/dl&gt;"><label
                            for="MainContent_rbl_1"><dl>
                                <dt>Partecipante 2</dt><dd class="hidden">Lorem ipsum dolor sit amet, consectetur adipiscing
                                    elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus
                                    vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum
                                    ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam
                                    quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat,
                                    est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet
                                    neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus,
                                    id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat
                                    sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus
                                    placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.</dd></dl>
                        </label>
                        <br>
                        <input id="MainContent_rbl_2" type="radio" name="ctl00$MainContent$rbl" value="&lt;dl&gt;&lt;dt&gt;Partecipante 3&lt;/dt&gt;&lt;dd&gt;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat, est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus, id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.&lt;/dd&gt;&lt;/dl&gt;"><label
                            for="MainContent_rbl_2"><dl>
                                <dt>Partecipante 3</dt><dd class="hidden">Lorem ipsum dolor sit amet, consectetur adipiscing
                                    elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus
                                    vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum
                                    ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam
                                    quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat,
                                    est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet
                                    neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus,
                                    id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat
                                    sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus
                                    placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.</dd></dl>
                        </label>
                        <br>
                        <input id="MainContent_rbl_3" type="radio" name="ctl00$MainContent$rbl" value="&lt;dl&gt;&lt;dt&gt;Partecipante 4&lt;/dt&gt;&lt;dd&gt;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat, est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus, id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.&lt;/dd&gt;&lt;/dl&gt;"><label
                            for="MainContent_rbl_3"><dl>
                                <dt>Partecipante 4</dt><dd class="hidden">Lorem ipsum dolor sit amet, consectetur adipiscing
                                    elit. Cras non tristique nulla. Sed facilisis feugiat quam, sed laoreet nisl luctus
                                    vel. Sed convallis varius magna, feugiat dignissim ante bibendum id. Vestibulum
                                    ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam
                                    quis libero erat, et feugiat magna. Nulla gravida, quam eu porttitor consequat,
                                    est lacus tempor ligula, dapibus cursus lacus elit sit amet felis. Nam vel laoreet
                                    neque. Aliquam scelerisque porttitor turpis quis commodo. Fusce in turpis purus,
                                    id bibendum ante. Donec a tristique augue. Phasellus nec leo in lacus accumsan volutpat
                                    sed ut nulla. Aliquam eleifend erat vestibulum purus interdum ac tincidunt purus
                                    placerat. Morbi libero arcu, congue eleifend sollicitudin sed, eleifend in velit.</dd></dl>
                        </label>
                    </span>
                </div>
            </fieldset>
            <div class="DivEpButton big">
                <a href="#" class="linkMenu" title="Next">Next</a>
            </div>
        </div>
        <hr />
        <div class="view preview">
            <fieldset class="section collapsable attachments">
                <legend>Attached Files</legend>
                <div class="fieldobject">
                    <div class="fieldrow">
                        <ul class="attachedfiles">
                            <li class="attachedfile"><span class="iteminfo"><span class="name"><span class="actionbuttons">
                                <span class="fileIco extdoc" title="">&nbsp;</span> </span>01 - Abstract bando.doc
                            </span><span class="itemdetail">(1.17 mb)</span> </span>
                                <div class="cfpdescription">
                                    description
                                </div>
                            </li>
                            <li class="attachedfile"><span class="iteminfo"><span class="name"><span class="actionbuttons">
                                <span class="fileIco extdoc" title="">&nbsp;</span> </span>02 - Requisiti.doc </span>
                                <span class="itemdetail">(1.17 mb)</span> </span>
                                <div class="cfpdescription">
                                    description
                                </div>
                            </li>
                            <li class="attachedfile"><span class="iteminfo"><span class="name"><span class="actionbuttons">
                                <span class="fileIco extdoc" title="">&nbsp;</span> </span>03 - Informativa Privacy.doc
                            </span><span class="itemdetail">(1.17 mb)</span> </span>
                                <div class="cfpdescription">
                                    description
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </fieldset>
            <fieldset class="section collapsable">
                <legend>Section</legend>
                <div class="sectiondescription">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sagittis urna
                    sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et vehicula elit.
                    Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada ut, accumsan
                    et tortor. In eget velit sed erat lobortis posuere. Suspendisse non lacus sed dolor
                    tincidunt accumsan. Duis porttitor tincidunt sagittis.
                </div>
                <div class="fieldobject singleline">
                    <div class="fieldrow fieldinput">
                        <label class="" for="">
                            Lorem<span class="required">*</span></label>
                        <div class="fielddescription">
                            <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                                sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                                vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                                ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                                lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                        </div>
                        <input class="inputtext " maxlength="10" type="text"><span class="inlinetooltip">Help</span>
                        <br />
                        <span class="fieldinfo "><span class="maxchar ">Caratteri disponibili: <span class="availableitems">
                        </span>/<span class="totalitems"></span> </span><span class="generic "></span></span>
                    </div>
                </div>
                <div class="fieldobject multiline">
                    <div class="fieldrow fieldinput">
                        <label for="">
                            Label</label>
                        <div class="fieldrow fielddescription">
                            <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                                sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                                vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                                ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                                lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                        </div>
                        <textarea class="textarea" maxlength="10"></textarea><span class="inlinetooltip">Help</span>
                        <br />
                        <span class="fieldinfo "><span class="maxchar ">Caratteri disponibili: <span class="availableitems">
                        </span>/<span class="totalitems"></span> </span><span class="generic "></span></span>
                    </div>
                </div>
                <div class="fieldobject disclaimer">
                    <div class="fieldrow fieldinput">
                        <label for="">
                            Label</label>
                        <div class="fielddescription">
                            <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                                sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                                vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                                ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                                lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                        </div>
                        <div class="disclaimerwrapper">
                            <div class="disclaimertext">
                                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam quis molestie purus.
                                Nulla facilisi. Curabitur lacinia, nibh sed vehicula vehicula, diam mi sollicitudin
                                felis, a commodo risus nulla nec diam. Quisque eu quam sodales arcu cursus fringilla
                                cursus sit amet est. Proin sit amet purus eget metus cursus ullamcorper. Aenean
                                ante mi, venenatis ac ornare nec, porta non erat. Nulla tempor, nibh accumsan elementum
                                imperdiet, erat magna scelerisque enim, non varius ligula elit id purus. Lorem ipsum
                                dolor sit amet, consectetur adipiscing elit. Maecenas ante elit, sollicitudin a
                                mollis id, sollicitudin quis erat. Nunc et sem libero, nec eleifend velit. Quisque
                                sagittis ullamcorper sodales. Donec justo lacus, vestibulum et ullamcorper a, volutpat
                                eget mauris. Vestibulum hendrerit lectus ac sapien accumsan in laoreet velit consectetur.
                                Ut condimentum interdum lectus sit amet bibendum. Aenean nec metus nisl, at mollis
                                diam. In turpis nisl, bibendum vitae eleifend sit amet, vehicula ac orci. Nam luctus
                                facilisis urna quis molestie. Lorem ipsum dolor sit amet, consectetur adipiscing
                                elit. Praesent vehicula vehicula nibh vitae bibendum. Integer commodo, nisl ac lobortis
                                bibendum, massa sapien aliquet quam, a elementum nisl mauris pellentesque magna.
                                Cras ac sapien ligula. Ut facilisis elit vitae nisl hendrerit blandit. Cum sociis
                                natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Suspendisse
                                convallis feugiat leo sit amet tincidunt. Praesent eget volutpat massa. Morbi ultricies,
                                erat at malesuada mollis, nisi tortor ullamcorper quam, vitae feugiat mi eros a
                                nulla. Curabitur id accumsan felis. Etiam dignissim mi eleifend turpis mattis id
                                porta erat pulvinar.</div>
                            <div class="disclaimerinput">
                                <label class="disclaimerlabel">
                                    Accept</label><input type="radio" name="disclaimer"><label class="disclaimerlabel">Refuse</label><input
                                        type="radio" name="disclaimer" checked>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="fieldobject note">
                    <div class="fieldrow fielddescription">
                        <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                            sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                            vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                            ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                            lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                    </div>
                </div>
                <div class="fieldobject fileupload">
                    <div class="fieldrow fieldinput">
                        <label for="">
                            Label</label>
                        <div class="fielddescription">
                            <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                                sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                                vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                                ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                                lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                        </div>
                        <input type="file" /><span class="inlinetooltip">Help</span>
                    </div>
                </div>
            </fieldset>
            <fieldset class="section collapsable">
                <legend>Section</legend>
                <div class="sectiondescription">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sagittis urna
                    sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et vehicula elit.
                    Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada ut, accumsan
                    et tortor. In eget velit sed erat lobortis posuere. Suspendisse non lacus sed dolor
                    tincidunt accumsan. Duis porttitor tincidunt sagittis.
                </div>
                <div class="fieldobject radiobuttonlist">
                    <div class="fieldrow fieldinput">
                        <label for="">
                            Lorem</label>
                        <div class="fielddescription">
                            <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                                sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                                vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                                ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                                lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                        </div>
                        <span id="" class="inputradiobuttonlist">
                            <input id="ArbnlistA_0" type="radio" name="rbnlistA" value="Option 1"><label for="ArbnlistA_0">Option
                                1</label><br>
                            <input id="ArbnlistA_1" type="radio" name="rbnlistA" value="Option 2"><label for="ArbnlistA_1">Option
                                2</label><br>
                            <input id="ArbnlistA_2" type="radio" name="rbnlistA" value="Option 3"><label for="ArbnlistA_2">Option
                                3</label><br>
                            <input id="ArbnlistA_3" type="radio" name="rbnlistA" value="Option 4"><label for="ArbnlistA_3">Option
                                4</label><br>
                        </span><span class="inlinetooltip">Help</span>
                    </div>
                </div>
                <div class="fieldobject checkboxlist">
                    <div class="fieldrow fieldinput">
                        <label for="">
                            Lorem Ipsum Dolor Sit</label>
                        <div class="fielddescription">
                            <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                                sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                                vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                                ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                                lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                        </div>
                        <span id="" class="checkboxlist min-1 max-4">
                            <input id="AchblistA_0" type="checkbox" name="chblistA$0" value="Option 1"><label
                                for="AchblistA_0">Option 1</label><br>
                            <input id="AchblistA_1" type="checkbox" name="chblistA$1" value="Option 2"><label
                                for="AchblistA_1">Option 2</label><br>
                            <input id="AchblistA_2" type="checkbox" name="chblistA$2" value="Option 3"><label
                                for="AchblistA_2">Option 3</label><br>
                            <input id="AchblistA_3" type="checkbox" name="chblistA$3" value="Option 4"><label
                                for="AchblistA_3">Option 4</label><br>
                        </span><span class="inlinetooltip">Help</span>
                        <br />
                        <span class="fieldinfo "><span class="maxchar ">Caratteri disponibili: <span class="availableitems">
                        </span>/<span class="totalitems"></span> </span><span class="minmax ">Min <span class="min">
                            1 risposta</span> / Max <span class="max">4 risposte</span> </span><span class="generic">
                                Bla bla bla bla bla bla </span></span>
                    </div>
                </div>
                <div class="fieldobject dropdownlist">
                    <div class="fieldrow fieldinput">
                        <label for="">
                            Lorem Ipsum Dolor Sit</label>
                        <div class="fielddescription">
                            <span class="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas
                                sagittis urna sed ante iaculis et laoreet sapien placerat. Ut vitae metus mi, et
                                vehicula elit. Suspendisse eu euismod neque. Sed nisi massa, placerat at malesuada
                                ut, accumsan et tortor. In eget velit sed erat lobortis posuere. Suspendisse non
                                lacus sed dolor tincidunt accumsan. Duis porttitor tincidunt sagittis.</span>
                        </div>
                        <select>
                            <option>Option 1</option>
                            <option>Option 2</option>
                            <option>Option 3</option>
                            <option>Option 4</option>
                        </select>
                        <span class="inlinetooltip">Help</span>
                    </div>
                </div>
            </fieldset>
            <div class="DivEpButton big">
                <a href="#" class="linkMenu" title="Prev">Prev</a> <a href="#" class="linkMenu" title="Submit">
                    Submit</a>
            </div>
    </div>
    <% End Select%>
    </div>
</asp:Content>
