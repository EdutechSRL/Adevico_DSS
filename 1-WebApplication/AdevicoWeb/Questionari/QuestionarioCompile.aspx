<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionarioCompile.aspx.vb" Inherits="Comunita_OnLine.QuestionarioCompile"
    ValidateRequest="false" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
     <script type="text/javascript">
         $(function () {
             $('.dialog.dlgconfirmsubmit').dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 autoOpen: false,
                 draggable: true,
                 modal: true,
                 title: "",
                 width: 600,
                 height: 280,
                 minHeight: 200,
                 //                minWidth: 700,
                 zIndex: 1000,
                 open: function (type, data) {
                     //                $(this).dialog('option', 'width', 700);
                     //                $(this).dialog('option', 'height', 600);
                     //$(this).parent().appendTo("form");
                     $(".ui-dialog-titlebar-close", this.parentNode).hide();
                 }

             });
             $(".opendlgconfirmsubmit").click(function () {
                 $(".dialog.dlgconfirmsubmit").dialog("open");
                 return false;
             });

             $(".closedlgconfirmsubmit").click(function () {
                 $(".dialog.dlgconfirmsubmit").dialog("close");
                 return false;
             });

             $('.dialog.dlgconfirmexit').dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 autoOpen: false,
                 draggable: true,
                 modal: true,
                 title: "",
                 width: 600,
                 height: 280,
                 minHeight: 200,
                 //                minWidth: 700,
                 zIndex: 1000,
                 open: function (type, data) {
                     //                $(this).dialog('option', 'width', 700);
                     //                $(this).dialog('option', 'height', 600);
                     //$(this).parent().appendTo("form");
                     $(".ui-dialog-titlebar-close", this.parentNode).hide();
                 }

             });
             $(".opendlgconfirmexit").click(function () {
                 $(".dialog.dlgconfirmexit").dialog("open");
                 return false;
             });

             $(".closedlgconfirmexit").click(function () {
                 $(".dialog.dlgconfirmexit").dialog("close");
                 return false;
             });

             $('.dialog.dlgundo').dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 autoOpen: false,
                 draggable: true,
                 modal: true,
                 title: "",
                 width: 600,
                 height: 280,
                 minHeight: 200,
                 //                minWidth: 700,
                 zIndex: 1000,
                 open: function (type, data) {
                     //                $(this).dialog('option', 'width', 700);
                     //                $(this).dialog('option', 'height', 600);
                     //$(this).parent().appendTo("form");
                     $(".ui-dialog-titlebar-close", this.parentNode).hide();
                 }

             });
            
             $(".opendlgundo").click(function () {
                 $(".dialog.dlgundo").dialog("open");
                 return false;
             });
             $(".closedlgundo").click(function () {
                 $(".dialog.dlgundo").dialog("close");
                 return false;
             });
         });
    </script>     
    <style type="text/css">
        body .answer.renderedtext {
            font-size: 13px;
        }
        body .CellaDomanda {
            font-size: 13px;
        }
        body .question {
            font-size: 13px;
        }
    </style> 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="Server">
    <%--<script src="/Elle3/Jscript/jquery-1.4.1.min.js"></script>--%>
    <style>
        .paneltimer
        {          
            display:none; 
            width:100%;            
            position:fixed;
            bottom:0;            
            left:0;
            z-index:9999;      
            text-align:center;      
        }
        
        .paneltempo
        {            
            background:yellow;
            font-weight:bold;         
            padding:40px 20px;
            font-size:20px;            
            
        }
    </style>

    <script type="text/javascript">


        //        jQuery.fn.outerHTML = function () {
        //            return $('<div>').append(this.eq(0).clone()).html();
        //        };

        //        $(document).ready(function () {

        //            var pass = "Password  ";
        //            if ($.browser.msie) {
        //                pass = "Password ";
        //            }
        //            var Cpass = "Conferma Password  ";
        //            if ($.browser.msie) {
        //                Cpass = "Conferma Password ";
        //            }
        //            setPassField(pass);
        //            setPassField(Cpass);
        //        });


        //        function setPassField(value) {

        //            var spanPassword = $("span").filter(function () {
        //                return $(this).text() == value;
        //            });

        //            var tablePassword = spanPassword.parents("table").first();

        //            tablePassword.find("input[type='checkbox']").attr("checked", "true").hide();

        //            var name = spanPassword.parents("td.Risposte").first().children("input:last").attr("name");

        //            if ($.browser.msie) {
        //                var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");

        //                var html = oldInput.outerHTML();

        //                html = html.replace("<INPUT", '<INPUT type="password"');

        //                var newInput = $(html);
        //                var myName = oldInput.attr("name")
        //                //newInput.attr("type", "password");

        //                newInput.attr("name", "new");

        //                newInput.insertBefore(oldInput);

        //                oldInput.remove();

        //                newInput.attr("name", myName);

        //            } else {
        //                var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");
        //                var newInput = oldInput.clone();
        //                var myName = oldInput.attr("name")
        //                //oldInput.attr("type", "password");
        //                newInput.attr("type", "password");
        //                newInput.attr("name", "new");
        //                newInput.insertBefore(oldInput);
        //                oldInput.remove();
        //                newInput.attr("name", myName);
        //            }

        //        }

        var secs;
        var timerID = null;
        var timerRunning = false;
        var delay = 1000;
        var tempo;
        var HIDtempo;
        var isStart = true;
        var tempoBlu = 300;
        var tempoRosso = 60;

        window.onload = checkTimer;

        function setValue(value) {
            starter = document.getElementById('<%=me.HIDstarter.clientId %>');
            starter.value = value;
        }

        function InitializeTimer() {
            // Set the length of the timer, in seconds
            secs = 60;
            StopTheClock();
            Ticks();
        }

        function StopTheClock() {
            if (timerRunning) {
                clearTimeout(timerID);
            }
            timerRunning = false;
        }

        function Ticks() {
            el = document.getElementById("<%=me.DIVpanelTempo.clientId %>");
            starter = document.getElementById("<%=me.HIDstarter.clientId %>");
            HIDtempo = document.getElementById("<%=me.HIDtempoRimanente.clientId %>");

            if (starter.value == "1") {
                if (isStart) {                                        
                    //el.style.backgroundColor = "white";
                    secs = HIDtempo.value - 0.5;
                    isStart = false;
                }
                if (secs < tempoBlu && secs > tempoRosso) {
                    el.style.backgroundColor = "yellow";
                    el.style.color = "red";
                }
                else {
                    if (secs < tempoRosso && secs > 0) {
                        //                        if ( el.style.backgroundColor=="white" )
                        {
                            el.style.backgroundColor = "red";
                            el.style.color = "yellow";
                        }
                        //                        else
                        //                            {
                        //                                el.style.backgroundColor="white";
                        //                            } 
                    }
                    else {
                        if (secs < 0) {
                            secs = 0;
                        }
                    }
                }
                minSec = new Date(0, 0, 0, 0, 0, secs);
                el.innerHTML = "<%=me.HIDmessaggio.value %>".replace("{secondi}", minSec.toLocaleTimeString());
            }


            self.status = secs;
            secs = secs - 0.5;
            timerRunning = true;
            timerID = self.setTimeout("Ticks()", delay);
        }



        function checkTimer() {
            InitializeTimer();
            HIDtempo = document.getElementById('<%=me.HIDtempoRimanente.clientId %>');
            tempo = HIDtempo.value

            Ticks();
        }
    </script>

       
    <div>
         <CTRL:Messages runat="server" ID="CTRLmessage" Visible="false" />
        <input id="HIDmessaggio" runat="server" type="hidden" />
        <div>
            <div>
                <div id="DIVpanelTimer" class="paneltimer" runat="server">
                    <div id="DIVpanelTempo" class="paneltempo" runat="server" style="display:none;">
                        <asp:Label ID="LBTempoRimanente" Visible="false" runat="server" Font-Size="Medium"
                            Font-Bold="true" />
                    </div>
                    <asp:UpdatePanel ID="UPTempo" runat="server">
                        <ContentTemplate>
                            <input id="HIDtempoRimanente" runat="server" value="0" type="hidden" />
                            <input id="HIDstarter" runat="server" value="0" type="hidden" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TMDurata" EventName="Tick" />
                            <asp:AsyncPostBackTrigger ControlID="TMSessione" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <asp:Timer ID="TMDurata" runat="server" Enabled="false">
            </asp:Timer>
            <asp:Timer ID="TMSessione" runat="server" Enabled="false">
            </asp:Timer>
            <div>
                <div>
                    <asp:MultiView runat="server" ID="MLVquestionari">
                        <%--attenzione!! non spostare VIWdati, oppure correggere tutti i riferimenti a MLVquestionari.activeViewIndex nel vb--%>
                        <asp:View ID="VIWdati" runat="server">
                            <asp:Panel ID="PNLmenu" runat="server" Width="99%" HorizontalAlign="right" Visible="true">
                                <asp:LinkButton ID="LNBdescrizione" runat="server" CssClass="Link_Menu" Visible="false"
                                    Style="margin-right: auto;"> </asp:LinkButton>
                                <asp:LinkButton ID="LNBannulla" Visible="true" runat="server" CssClass="Link_Menu"
                                    Style="margin-right: auto;"> </asp:LinkButton>
                                <asp:LinkButton ID="LNBTornaHome" Visible="true" runat="server" CssClass="Link_Menu"
                                    Style="margin-right: auto;"> </asp:LinkButton>
                                <asp:LinkButton ID="LNBFinito" Visible="false" runat="server" CssClass="Link_Menu"
                                    Style="margin-right: auto;"> </asp:LinkButton>
                                <asp:LinkButton ID="LNBsalvaEsci" Visible="false" runat="server" CssClass="Link_Menu"
                                    Style="margin-right: auto;"> </asp:LinkButton>
                                <asp:LinkButton ID="LNBsalvaContinua" runat="server" CssClass="Link_Menu" Visible="false"
                                    Style="margin-right: auto;"> </asp:LinkButton>
                            </asp:Panel>
                            <div style="width: 100%; margin-top: 20px; margin-bottom: 20px; text-align: left;">
                                <h2><asp:Label ID="LBname" runat="server"></asp:Label></h2>
                            </div>
                            <asp:label ID="LBisMandatoryInfoTop" runat="server"></asp:label>
                            <asp:Label ID="LBTroppeRispostePagina" Visible="false" runat="server" CssClass="Errore"> </asp:Label>
                            <asp:Label runat="server" ID="LBnoRisposta" CssClass="Errore" Visible="false"></asp:Label>
                            <asp:PlaceHolder runat="server" ID="PHucValutazione"></asp:PlaceHolder>
                            <asp:Panel ID="PNLElenco" runat="server" CssClass="elenco" Width="99%">
                                <br />
                                <asp:DataList ID="DLPagine" ShowFooter="true" runat="server" DataKeyField="id" CellPadding="4"
                                    ForeColor="#333333" BorderWidth="1" Width="100%">
                                    <ItemTemplate>
                                        <div class="NomePagina" id="DVpageName" runat="server">
                                            <h3>
                                                <%#Eval("nomePagina")%>
                                            </h3>
                                            <br />
                                        </div>
                                        <div class="TestoDomanda" id="DVpageDescription" runat="server">
                                            <i>
                                                <%#Eval("descrizione")%>
                                            </i>
                                        </div>
                                        
                                        <asp:DataList ID="DLDomande" runat="server" DataKeyField="id" OnItemDataBound="loadDomandeOpzioni"
                                            Width="100%">
                                            <ItemTemplate>
                                                <a name='<%#Eval("numero")%>'></a>
                                                <div class="ContenitoreDomanda0" runat="server" id="DIVDomanda">
                                                    <div class="TestoDomanda">
                                                        
                                                        <div style='<%# me.displayDifficulty %>' runat="server" id="DIVCode">
                                                            (Cod.<%#Eval("id")%>
                                                            <asp:Label runat="server" ID="LBDifficoltaTesto" Text='<%#Eval("difficoltaTesto")%>'></asp:Label>)
                                                        </div>
                                                        <br />
                                                        <span class="question" title="<%#MandatoryToolTip(Container.Dataitem)%>">
                                                            <asp:Label runat="server" Text='<%#Eval("numero")%>' Visible='<%# me.showDifficulty %>'></asp:Label>
                                                             <%#MandatoryDisplay(Container.Dataitem)%>
                                                            <span class="name"><%#me.SmartTagsAvailable.TagAll(Eval("testo"))%></span>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="Risposte">
                                                    <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                                </div>
                                                <asp:Label runat="server" ID="LBsuggerimentoOpzione" Font-Italic="true" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="LBSuggerimento" Text='<%#Eval("suggerimento")%>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <AlternatingItemStyle BackColor="#EFF3FB" />
                                            <ItemStyle BackColor="WHITE" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        </asp:DataList>
                                        <br />
                                        <div class="NomePaginaFooter" runat="server" id="DIVNomePaginaFooter">
                                            <br />
                                            <%#Eval("nomePagina")%>
                                        </div>
                                    </ItemTemplate>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <AlternatingItemStyle BackColor="#507CD1" />
                                    <ItemStyle BackColor="#EFF3FB" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                </asp:DataList>
                            </asp:Panel>
                            <br />
                            <div style="width: 100%; margin-top: 10px; margin-bottom: 20px;" runat="server" id="DIVNumeriPagina">
                                <asp:Label ID="LBpagina" runat="server"></asp:Label>
                                <asp:ImageButton ID="IMBprima" ImageUrl="img/indietro.gif" runat="server" Visible="False">
                                </asp:ImageButton>
                                &nbsp; &nbsp;
                                <asp:PlaceHolder ID="PHnumeroPagina" runat="server"></asp:PlaceHolder>
                                &nbsp; &nbsp;
                                <asp:ImageButton ID="IMBdopo" runat="server" ImageUrl="img/avanti.gif" Visible="False">
                                </asp:ImageButton>
                                <asp:Button runat="server" ID="BTNDopo" EnableViewState="False" Visible="false" />
                                <br />
                            </div>
                            <asp:label ID="LBisMandatoryInfoBottom" runat="server"></asp:label>
                            <div runat="server" id="DIVSalvaQuestionario">
                                <br />
                                <asp:Label runat="server" ID="LBAvvisoSalva" Visible="true"></asp:Label>
                                <asp:Literal ID="LTsaveAndExit" runat="server"><br /></asp:Literal>
                                <asp:Button runat="server" ID="BTNSalvaEEsci"  Visible="false" />
                                <asp:Button runat="server" ID="BTNSalvaContinua" />
                                <br />
                            </div>
                            <asp:Label runat="server" ID="LBAvvisoFine" Visible="true"></asp:Label>
                            <br />
                            <br />
                            <asp:Button runat="server" ID="BTNFine" Visible="true" />
                        </asp:View>
                        <asp:View ID="VIWdescrizione" runat="server">
                            <asp:Panel ID="PNLIndietro" runat="server" Width="100%" HorizontalAlign="right" Visible="false">
                                <asp:LinkButton ID="LNBindietro" Visible="true" runat="server" CssClass="Link_Menu"> </asp:LinkButton>&nbsp;</asp:Panel>
                            <br />
                            <br />
                            <asp:Label ID="LBnomeVIWDescrizione" runat="server" Text='<%#Eval("nome")%>'></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="LBdescrizioneVIWDescrizione" runat="server"> </asp:Label>
                            <br />
                            <br />
                            <br />
                            <asp:Label ID="LBTempoRimanenteVIWDescrizione" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="LBdurata" runat="server" Visible="false"></asp:Label>
                            <br />
                            <br />
                            <p align="center">
                                <asp:Button ID="BTNinizia" runat="server" Text="" ForeColor="Black"></asp:Button>
                                <asp:Button ID="BTNIniziaFacile" Visible="false" runat="server" Text="" ForeColor="Black">
                                </asp:Button>
                                <asp:Button ID="BTNIniziaMedio" Visible="false" runat="server" Text="" ForeColor="Black">
                                </asp:Button>
                                <asp:Button ID="BTNIniziaDifficile" Visible="false" runat="server" Text="" ForeColor="Black">
                                </asp:Button>
                                <asp:Button ID="BTNIniziaMisto" Visible="false" runat="server" Text="" ForeColor="Black">
                                </asp:Button>
                            </p>
                        </asp:View>
                        <asp:View runat="server" ID="VIWmessaggi">
                            <asp:Panel ID="PNLTornaLista" runat="server" Width="100%" HorizontalAlign="right">
                                <asp:LinkButton ID="LNBTornaLista" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
                                <asp:HyperLink id="HYPnewAttempt" runat="server" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                            </asp:Panel>
                            <br />
                            <CTRL:Messages runat="server" ID="CTRLerrorMessages" Visible="false" />
                            <br /><br /><br />
                            <asp:Label ID="LBConferma" runat="server" Visible="false"></asp:Label>
                            <br />
                            <asp:Button ID="BTNRestartAutoEval" runat="server" Visible="false" />
                            <asp:Button runat="server" ID="BTNSalvaAutovalutazione" Visible="false" />
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </div>
    </div>
    <div class="dialog dlgconfirmsubmit" runat="server" id="DVconfirmSubmit" visible="true">                
        <div class="fieldobject">
            <div class="fieldrow title">
                <div class="description">
                    <asp:Label ID="LBconfirmOptions" runat="server">*</asp:Label>
                </div>                        
            </div>
            <div class="fieldrow commandoptions clearfix">
                <div class="commandoption left">
                    <asp:Button Text="* Annulla" runat="server" CssClass="commandbutton editoption1 closedlgconfirmsubmit" ID="BTNundoOption" />
                    <asp:Label ID="LBundoOption" runat="server" CssClass="commanddescription" >* Annulla </asp:Label>
                </div>
                <div class="commandoption right">
                    <asp:Button Text="* Conferma " runat="server" CssClass="commandbutton editoption2" ID="BTNconfirmOption" />
                    <asp:Label ID="LBconfirmOption" runat="server" CssClass="commanddescription" >*</asp:Label>
                </div>
            </div>
        </div>
        <input type="hidden" id="HDNcurrentTime" runat="server" />
    </div>
    <div class="dialog dlgconfirmexit" runat="server" id="DVconfirmExit" visible="false">                
        <div class="fieldobject">
            <div class="fieldrow title">
                <div class="description">
                    <asp:Label ID="LBconfirmExitOptions" runat="server">*</asp:Label>
                </div>                        
            </div>
            <div class="fieldrow commandoptions clearfix">
                <div class="commandoption left">
                    <asp:Button Text="* Annulla" runat="server" CssClass="commandbutton editoption1 closedlgconfirmexit" ID="BTNundoExitOption" />
                    <asp:Label ID="LBundoExitOption" runat="server" CssClass="commanddescription" >* Annulla </asp:Label>
                </div>
                <div class="commandoption right">
                    <asp:Button Text="* Conferma " runat="server" CssClass="commandbutton editoption2" ID="BTNconfirmExitOption" />
                    <asp:Label ID="LBconfirmExitOption" runat="server" CssClass="commanddescription" >*</asp:Label>
                </div>
            </div>
        </div>
    </div> 
    <div class="dialog dlgundo" runat="server" id="DVundoExit" visible="false">                
        <div class="fieldobject">
            <div class="fieldrow title">
                <div class="description">
                    <asp:Label ID="LBundoActionMessage" runat="server">*</asp:Label>
                </div>                        
            </div>
            <div class="fieldrow commandoptions clearfix">
                <div class="commandoption left">
                    <asp:Button Text="* Annulla" runat="server" CssClass="commandbutton editoption1 closedlgundo" ID="BTNundoLeaveQuestionnaireOption" />
                    <asp:Label ID="LBundoLeaveQuestionnaireOption" runat="server" CssClass="commanddescription" >* Annulla </asp:Label>
                </div>
                <div class="commandoption right">
                    <asp:Button Text="* Conferma " runat="server" CssClass="commandbutton editoption2" ID="BTNconfirmLeaveQuestionnaireOption" />
                    <asp:Label ID="LBconfirmLeaveQuestionnaireOption" runat="server" CssClass="commanddescription" >*</asp:Label>
                </div>
            </div>
        </div>
    </div> 
    <asp:Literal ID="LTopenUndoDialogCssClass" runat="server" Visible="false">opendlgundo</asp:Literal><asp:Literal ID="LTcloseDialogCssClass" runat="server" Visible="false">closedlgconfirmsubmit</asp:Literal><asp:Literal ID="LTdlgconfirmsubmit" runat="server" Visible="false">dlgconfirmsubmit</asp:Literal><asp:Literal ID="LTconfirmDialogCssClass" runat="server" Visible="false">opendlgconfirmsubmit</asp:Literal><asp:Literal ID="LTconfirmExitDialogCssClass" Visible="false" runat="server">opendlgconfirmexit</asp:Literal>
</asp:Content>