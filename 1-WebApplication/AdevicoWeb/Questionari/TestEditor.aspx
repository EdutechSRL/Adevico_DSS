<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="TestEditor.aspx.vb" Inherits="Comunita_OnLine.QuestTestEditor" %>
    <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVeditor" runat="server">
        <asp:View ID="VIWempty" runat="server">
        </asp:View>
        <asp:View ID="VIWtest" runat="server">
            Domanda:
            <CTRL:CTRLeditor id="CTRLeditorQ" runat="server" ContainerCssClass="containerclass"
             LoaderCssClass="loadercssclass" Width="800px" AutoInitialize="true"
              CurrentType="telerik" Toolbar="advanced" EnabledTags="img,latex,youtube,emoticons,wiki"
             ></CTRL:CTRLeditor>

             <script type="text/javascript">
          //<![CDATA[
//                 Telerik.Web.UI.Editor.CommandList["InsertLatex"] = function (commandName, editor, args) {
//                     //var elem = editor.getSelectedElement(); //returns the selected element.
//                     var content = editor.getSelection().getText();

//                     content = content.replace("[latex]", "").replace("[/latex]", "");

//                     var myCallbackFunction = function (sender, args) {
//                         editor.pasteHtml(String.format("{0}{1}{2}", args.openTag, args.text, args.closeTag));
//                     }

//                     editor.showExternalDialog("../Modules/Common/Editor/Telerik/InsertLatex.aspx",
//            content,
//            550,
//            400,
//            myCallbackFunction,
//            null,
//            "Insert Latex",
//            true,
//            Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
//            false,
//            true);
//                 };

//                 Telerik.Web.UI.Editor.CommandList["InsertYoutube"] = function (commandName, editor, args) {

//                     //var elem = editor.getSelectedElement(); //returns the selected element.
//                     var content = editor.getSelection().getText();

//                     content = content.replace("[youtube]", "").replace("[/youtube]", "");

//                     var myCallbackFunction = function (sender, args) {
//                         editor.pasteHtml(String.format("{0}{1}{2}", args.openTag, args.text, args.closeTag));
//                     }

//                     editor.showExternalDialog("../Modules/Common/Editor/Telerik/InsertYoutube.aspx",
//            content,
//            550,
//            500,
//            myCallbackFunction,
//            null,
//            "Insert Youtube",
//            true,
//            Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
//            false,
//            true);
//                 }; 
          //]]>
     </script>


            Telerik
            <telerik:radeditor ID="RDEtelerik" runat="server" Visible="true" NewLineMode="P"
     AutoResizeHeight="false"  EnableResize="true"
    ></telerik:radeditor>
      <br /><br />
           <%-- Opzione 1:
            <CTRL:CTRLeditor id="CTRLeditorA" runat="server" CurrentType="telerik"></CTRL:CTRLeditor>
             Opzione 2:
            <CTRL:CTRLeditor id="CTRLeditorB" runat="server" FontNames="Verdana,Tahoma" FontSizes="2,3,4" CurrentType="telerik" Visible="false"></CTRL:CTRLeditor>
             Opzione 3:
            <CTRL:CTRLeditor id="CTRLeditorC" runat="server" FontNames="Verdana" FontSizes="2" CurrentType="telerik" Visible="false"></CTRL:CTRLeditor>--%>
            <asp:Button ID="BTNrenderQuestion" runat="server" Text="Render" />
            <table id="ctl00_CPHservice_DLPagine" cellspacing="0" cellpadding="4" border="0"
                style="color: #333333; border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;">
                <tr>
                    <td style="background-color: #EFF3FB;">
                        <table id="ctl00_CPHservice_DLPagine_ctl00_DLDomande" cellspacing="0" border="0"
                            style="width: 100%; border-collapse: collapse;">
                            <tr>
                                <td style="background-color: White;">
                                    <a name='1'></a>
                                    <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl00_DIVDomanda" class="ContenitoreDomanda0">
                                        <div class="TestoDomanda">
      
                                            <div runat="server" id="DIVCode">
                                                (Cod. [Identificativo domanda])
                                                <asp:Label runat="server" ID="LBDifficoltaTesto" Text="Media"></asp:Label>)
                                            </div>
                                            <br />
                                            <span class="question" title="Domanda iniziaziale (TEST RENDER)">
                                                <asp:Label ID="Label1" runat="server" Text="[1]" Visible="true"></asp:Label>
                                                    (*)
                                                <span class="name"><asp:Literal ID="LTrenderQuestion" runat="server"></asp:Literal></span>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="Risposte">
                                        <table id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl00_TBLDomandaSingola_0" border="0">
                                            <tr>
                                                <td class="Risposte">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl00_ctl01" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl00$RBGOpzione"
                                                        value="ctl01" /><span>Si </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Risposte">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl00_ctl03" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl00$RBGOpzione"
                                                        value="ctl03" /><span>No </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                         <%--   <tr>
                                <td style="background-color: #EFF3FB;">
                                    <a name='2'></a>
                                    <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl01_DIVDomanda" class="ContenitoreDomanda0">
                                        <div class="TestoDomanda">
                                            <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl01_DIVCode" style="text-align: right;">
                                                (Cod.750 <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl01_LBDifficoltaTesto">
                                                    - Diff. Med</span>)
                                            </div>
                                            <br />
                                            <span class="question" title="ATTENZIONE: e' obbligatorio rispondere a questa domanda">
                                                <span>2</span> <span class="mandatory">(*)</span> <span class="name">Nazione di appartenenza:</span>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="Risposte">
                                        <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl01_UCDomandaDropDown_1_LBEtichettaDropDown">
                                        </span>
                                        <select name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl01$UCDomandaDropDown_1$DDLOpzioni"
                                            id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl01_UCDomandaDropDown_1_DDLOpzioni">
                                            <option value="1">Italia</option>
                                            <option value="2">Francia</option>
                                            <option value="3">Germania</option>
                                            <option value="4">Olanda</option>
                                            <option value="5">Turchia</option>
                                            <option value="6">Extra Ue</option>
                                        </select>
                                        <br />
                                        <br>
                                        <span>Testo da viTesto da visualizzare con le informazioni base relative alla domanda
                                            Testo da visualizzare con le inTesto da visualizzare con le informazioni base relative
                                            alla domanda Testo da visualizzare con le inTesto da visualizzare con le informazioni
                                            base relative alla domanda Testo da visualizzare con le inTesto da visualizzare
                                            con le informazioni base relative alla domanda Testo da visualizzare con le inTesto
                                            da visualizzare con le informazioni base relative alla domanda Testo da visualizzare
                                            con le inTesto da visualizzare con le informazioni base relative alla domanda Testo
                                            da visualizzare con le inTesto da visualizzare con le informazioni base relative
                                            alla domanda Testo da visualizzare con le insualizzare con le informazioni base
                                            relative alla domanda Testo da visualizzare con le in</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: White;">
                                    <a name='3'></a>
                                    <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl02_DIVDomanda" class="ContenitoreDomanda0">
                                        <div class="TestoDomanda">
                                            <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl02_DIVCode" style="text-align: right;">
                                                (Cod.752 <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl02_LBDifficoltaTesto">
                                                    - Diff. Med</span>)
                                            </div>
                                            <br />
                                            <span class="question" title="ATTENZIONE: e' obbligatorio rispondere a questa domanda">
                                                <span>3</span> <span class="mandatory">(*)</span> <span class="name">dimmi il nome !
                                                </span></span>
                                        </div>
                                    </div>
                                    <div class="Risposte">
                                        <table id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl02_TBLTestoLibero_2" border="0">
                                            <tr>
                                                <td class="Risposte">
                                                    <span></span>
                                                </td>
                                                <td>
                                                    <textarea name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl02$ctl02" rows="3" cols="20"
                                                        style="width: 500px;"></textarea>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #EFF3FB;">
                                    <a name='4'></a>
                                    <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_DIVDomanda" class="ContenitoreDomanda0">
                                        <div class="TestoDomanda">
                                            <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_DIVCode" style="text-align: right;">
                                                (Cod.753 <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_LBDifficoltaTesto">
                                                    - Diff. Med</span>)
                                            </div>
                                            <br />
                                            <span class="question" title="ATTENZIONE: e' obbligatorio rispondere a questa domanda">
                                                <span>4</span> <span class="mandatory">(*)</span> <span class="name">Inserisci le temperature
                                                    del laboratorio </span></span>
                                        </div>
                                    </div>
                                    <div class="Risposte">
                                        <table id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_TBLNumerica_3" border="0">
                                            <tr>
                                                <td class="Risposte">
                                                    <span>Caldo:</span>
                                                </td>
                                                <td class="Risposte">
                                                    <input name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl03$TXBTestoNumerica_0"
                                                        type="text" id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_TXBTestoNumerica_0" /><span></span><span
                                                            id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_COVOpzioneNumerica_0" style="color: Red;
                                                            display: none;">Inserire un numero. </span><span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_CVisOverflowOpzioneNumerica_0"
                                                                style="color: Red; display: none;">Il numero inserito è troppo grande.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Risposte">
                                                    <span>Freddo:</span>
                                                </td>
                                                <td class="Risposte">
                                                    <input name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl03$TXBTestoNumerica_1"
                                                        type="text" id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_TXBTestoNumerica_1" /><span></span><span
                                                            id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_COVOpzioneNumerica_1" style="color: Red;
                                                            display: none;">Inserire un numero. </span><span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl03_CVisOverflowOpzioneNumerica_1"
                                                                style="color: Red; display: none;">Il numero inserito è troppo grande.</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: White;">
                                    <a name='5'></a>
                                    <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl04_DIVDomanda" class="ContenitoreDomanda0">
                                        <div class="TestoDomanda">
                                            <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl04_DIVCode" style="text-align: right;">
                                                (Cod.754 <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl04_LBDifficoltaTesto">
                                                    - Diff. Med</span>)
                                            </div>
                                            <br />
                                            <span class="question" title="ATTENZIONE: e' obbligatorio rispondere a questa domanda">
                                                <span>5</span> <span class="mandatory">(*)</span> <span class="name">Vacanza:<br>
                                                    <br>
                                                    <ul>
                                                        <li>Elenchi normali</li><li>Elenchi numerati</li><li>Formattazione testi</li><li>Inserimento
                                                            di [222] formule in formato latex: \sqrt[2]2\approx 1,4 =&gt;
                                                            <br>
                                                        </li>
                                                        <li><a href="" onclick="javascript:return LatexPopup(this,'http://localhost/Comol_Elle3/SmartTagsRender/LatexPopUp.aspx');"
                                                            class='latexhref' title='%5csqrt%5b2%5d%7b2%7d%5capprox+1%7b%2c%7d4'>
                                                            <img class='lateximg' src='http://localhost/Comol_Elle3/SmartTagsRender/LatexImage.aspx?%5csqrt%5b2%5d%7b2%7d%5capprox+1%7b%2c%7d4\dpi{160}'
                                                                alt='\sqrt[2]{2}\approx 1{,}4' title='\sqrt[2]{2}\approx 1{,}4' /></a></li><li><a
                                                                    href="" onclick="javascript:return LatexPopup(this,'http://localhost/Comol_Elle3/SmartTagsRender/LatexPopUp.aspx');"
                                                                    class='latexhref' title='%5csqrt%5b2%5d%7b2%7d'>
                                                                    <img class='lateximg' src='http://localhost/Comol_Elle3/SmartTagsRender/LatexImage.aspx?%5csqrt%5b2%5d%7b2%7d\dpi{160}'
                                                                        alt='\sqrt[2]{2}' title='\sqrt[2]{2}' /></a></li><li><a href="" onclick="javascript:return LatexPopup(this,'http://localhost/Comol_Elle3/SmartTagsRender/LatexPopUp.aspx');"
                                                                            class='latexhref' title='%5csqrt%5b4%5d%7b2%7d'>
                                                                            <img class='lateximg' src='http://localhost/Comol_Elle3/SmartTagsRender/LatexImage.aspx?%5csqrt%5b4%5d%7b2%7d\dpi{160}'
                                                                                alt='\sqrt[4]{2}' title='\sqrt[4]{2}' /></a></li><li><a href="" onclick="javascript:return LatexPopup(this,'http://localhost/Comol_Elle3/SmartTagsRender/LatexPopUp.aspx');"
                                                                                    class='latexhref' title='%5csqrt%5b3%5d%7bxy%7d'>
                                                                                    <img class='lateximg' src='http://localhost/Comol_Elle3/SmartTagsRender/LatexImage.aspx?%5csqrt%5b3%5d%7bxy%7d\dpi{160}'
                                                                                        alt='\sqrt[3]{xy}' title='\sqrt[3]{xy}' /></a></li><li>Inserimento video Youtube, materiale
                                                                                            SlideShare, immagini, link, etc...<span><a href="" onclick="javascript:return LatexPopup(this,'http://localhost/Comol_Elle3/SmartTagsRender/LatexPopUp.aspx');"
                                                                                                class='latexhref' title='%5csqrt%5b2%5d'><img class='lateximg' src='http://localhost/Comol_Elle3/SmartTagsRender/LatexImage.aspx?%5csqrt%5b2%5d\dpi{160}'
                                                                                                    alt='\sqrt[2]' title='\sqrt[2]' /></a></span></li></ul>
                                                    <div>
                                                        <br>
                                                    </div>
                                                </span></span>
                                        </div>
                                    </div>
                                    <div class="Risposte">
                                        <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl04_UCDomandaDropDown_4_LBEtichettaDropDown">
                                        </span>
                                        <select name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl04$UCDomandaDropDown_4$DDLOpzioni"
                                            id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl04_UCDomandaDropDown_4_DDLOpzioni">
                                            <option value="1">Al mare</option>
                                            <option value="2">Montagna</option>
                                            <option value="3">Lago</option>
                                        </select>
                                        <br />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #EFF3FB;">
                                    <a name='6'></a>
                                    <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_DIVDomanda" class="ContenitoreDomanda0">
                                        <div class="TestoDomanda">
                                            <div id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_DIVCode" style="text-align: right;">
                                                (Cod.755 <span id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_LBDifficoltaTesto">
                                                    - Diff. Med</span>)
                                            </div>
                                            <br />
                                            <span class="question" title="ATTENZIONE: e' obbligatorio rispondere a questa domanda">
                                                <span>6</span> <span class="mandatory">(*)</span> <span class="name">Dai un bel voto
                                                    !!</span> </span>
                                        </div>
                                    </div>
                                    <div class="Risposte">
                                        <table id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_TBLRadiobutton_5" cellspacing="0"
                                            cellpadding="10" border="0" style="border-width: 1px; border-style: solid; width: 810px;
                                            border-collapse: collapse;">
                                            <tr>
                                                <td class="CellaVuota" style="width: 0px;">
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
                                                    <span>0123456789 0123456789 012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
                                                        0123456789 012345678901234567890123456789012345678901234567890123456789 0123456789
                                                        0123456789 </span>
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl07" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                        value="ctl07" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl08" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                        value="ctl08" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl09" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                        value="ctl09" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl10" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                        value="ctl10" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl11" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_1"
                                                        value="ctl11" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="CellaDomanda">
                                                    <span>Ordine </span>
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl13" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                        value="ctl13" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl14" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                        value="ctl14" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl15" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                        value="ctl15" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl16" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                        value="ctl16" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl17" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_2"
                                                        value="ctl17" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="CellaDomanda">
                                                    <span>Pulizia </span>
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl19" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                        value="ctl19" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl20" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                        value="ctl20" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl21" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                        value="ctl21" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl22" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                        value="ctl22" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl23" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_3"
                                                        value="ctl23" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="CellaDomanda">
                                                    <span>Temperatura </span>
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl25" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                        value="ctl25" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl26" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                        value="ctl26" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl27" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                        value="ctl27" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl28" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                        value="ctl28" />
                                                </td>
                                                <td class="CellaRisposta">
                                                    <input id="ctl00_CPHservice_DLPagine_ctl00_DLDomande_ctl05_ctl29" type="radio" name="ctl00$CPHservice$DLPagine$ctl00$DLDomande$ctl05$RBGOpzione_4"
                                                        value="ctl29" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>--%>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
