<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OLD_UICompile.aspx.vb" Inherits="Comunita_OnLine.UICompile"
    ValidateRequest="false" %>
    <%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="~/UC/UC_PortalHeader.ascx" %>
<%--
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>--%>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"  "http://www.w3.org/TR/html4/strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head runat="server">
    <title>Compilazione Questionario</title>
    <link href="./../Styles.css?v=201604071200lm" type="text/css" rel="stylesheet" />
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
    <%--
    jQuery.fn.outerHTML = function () {
            return $('<div>').append(this.eq(0).clone()).html();
        };

        $(document).ready(function () {

            var pass = "Password  ";
              if ($.browser.msie) {
              pass = "Password ";
            }
            var Cpass = "Conferma Password  ";
            if ($.browser.msie) {
                Cpass = "Conferma Password ";
            }
            setPassField(pass);
            setPassField(Cpass);
        });
         

         function setPassField(value) {

            var spanPassword = $("span").filter(function () {
                return $(this).text() == value;
            });

            var tablePassword = spanPassword.parents("table").first();

            tablePassword.find("input[type='checkbox']").attr("checked", "true").hide();

            var name = spanPassword.parents("td.Risposte").first().children("input:last").attr("name");

            if ($.browser.msie) {
                var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");

                var html = oldInput.outerHTML();

                html = html.replace("<INPUT", '<INPUT type="password"');

                var newInput = $(html);
                var myName = oldInput.attr("name")
                //newInput.attr("type", "password");

                newInput.attr("name", "new");

                newInput.insertBefore(oldInput);

                oldInput.remove();

                newInput.attr("name", myName);
            } else {
                var oldInput = spanPassword.parents("td.Risposte").first().children("input:last");
                var newInput = oldInput.clone();
                var myName = oldInput.attr("name")
                //oldInput.attr("type", "password");
                newInput.attr("type", "password");
                newInput.attr("name", "new");
                newInput.insertBefore(oldInput);
                oldInput.remove();
                newInput.attr("name", myName);
            }

        }--%>
    <script type="text/javascript">
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
                    el.style.display = "block";
                    el.style.backgroundColor = "white";
                    secs = HIDtempo.value - 0.5;
                    isStart = false;
                }
                if (secs < tempoBlu && secs > tempoRosso) {
                    el.style.backgroundColor = "blue";
                    el.style.color = "white";
                }
                else {
                    if (secs < tempoRosso && secs > 0) {
                        //                        if ( el.style.backgroundColor=="white" )
                        {
                            el.style.backgroundColor = "red";
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
</head>
<body class="bodyUICompile">
    <asp:PlaceHolder runat="server" ID="PHHeader"></asp:PlaceHolder>
    <input id="HIDmessaggio" runat="server" type="hidden" />
    <form id="aspnetForm" runat="server">
    <div class="page">
        <asp:ScriptManager ID="SMTimer" runat="server" OnAsyncPostBackError="SMTimer_AsyncPostBackError">
        </asp:ScriptManager>
        <div class="upper">
            <div id="DIVpanelTimer" runat="server" style="top: 135px; position: fixed; margin-top: 5px;
                background-color: White;">
                <div id="DIVpanelTempo" runat="server" class="DIVpanelTempo">
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
        <asp:Timer ID="TMSessione" runat="server" Enabled="true">
        </asp:Timer>
        <div class="containerQuest" runat="server" id="DIVContainer">
            <div>
                <asp:MultiView runat="server" ID="MLVquestionari">
                    <%--attenzione!! non spostare VIWdati, oppure correggere tutti i riferimenti a MLVquestionari.activeViewIndex nel vb--%>
                    <asp:View ID="VIWdati" runat="server">
                        <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right" Visible="false">
                            <asp:LinkButton ID="LNBdescrizione" runat="server" CssClass="Link_Menu" Visible="true"></asp:LinkButton>&nbsp;
                        </asp:Panel>
                        <asp:label ID="LBisMandatoryInfoTop" runat="server"></asp:label>
                        <asp:Label ID="LBTroppeRispostePagina" Visible="false" runat="server" CssClass="Errore"></asp:Label>
                        <asp:Label runat="server" ID="LBnoRisposta" CssClass="Errore" Visible="false"></asp:Label>
                        <asp:PlaceHolder runat="server" ID="PHucValutazione"></asp:PlaceHolder>
                        <asp:Panel ID="PNLElenco" runat="server" CssClass="elenco">
                            <asp:DataList ID="DLPagine" ShowFooter="true" runat="server" DataKeyField="id" CellPadding="4"
                                ForeColor="#333333" BorderWidth="1" Width="100%">
                                <ItemTemplate>
                                    <div class="NomePagina" id="DVpageName" runat="server">
                                        <h3>
                                            <%#Eval("nomePagina")%>
                                        </h3>
                                    </div>
                                    <br />
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
                                                        <asp:Label ID="LBquestionNumber" runat="server" Text='<%#Eval("numero")%>' Visible='<%# me.showDifficulty %>'></asp:Label>
                                                         <%#MandatoryDisplay(Container.Dataitem)%>
                                                        <span class="name"><%#me.SmartTagsAvailable.TagAll(Eval("testo"))%></span>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="Risposte">
                                                <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                            </div>
                                            <br />
                                            </div>
                                            <asp:Label runat="server" ID="LBSuggerimento" Text='<%#Eval("suggerimento")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <AlternatingItemStyle BackColor="#EFF3FB" />
                                        <ItemStyle BackColor="WHITE" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    </asp:DataList>
                                    <br />
                                    <div class="NomePaginaFooter" id="DVpageNameBottom" runat="server">
                                        <%#Eval("nomePagina")%>
                                    </div>
                                </ItemTemplate>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <AlternatingItemStyle BackColor="#507CD1" />
                                <ItemStyle BackColor="#EFF3FB" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            </asp:DataList>
                            <%--inserire qui RPTrisposte dell'ucStatGen--%>
                        </asp:Panel>
                        <br />
                        <div style="width: 100%; margin-top: 10px; margin-bottom: 20px; text-align: center;">
                            <asp:Label ID="LBpagina" runat="server"></asp:Label>
                            <asp:ImageButton ID="IMBprima" ImageUrl="img/indietro.gif" runat="server" Visible="False">
                            </asp:ImageButton>
                            &nbsp; &nbsp;
                            <asp:PlaceHolder ID="PHnumeroPagina" runat="server"></asp:PlaceHolder>
                            &nbsp; &nbsp;
                            <asp:ImageButton ID="IMBdopo" runat="server" ImageUrl="img/avanti.gif" Visible="False">
                            </asp:ImageButton>
                        </div>
                        <asp:label ID="LBisMandatoryInfoBottom" runat="server"></asp:label>
                        <br />
                        <asp:Label runat="server" ID="LBAvvisoSalva" Visible="true"></asp:Label>
                        <br />
                        <asp:Button runat="server" ID="BTNSalvaEEsci" EnableViewState="False" Visible="false" />
                        <asp:Button runat="server" ID="BTNSalvaContinua" EnableViewState="False" Visible="true" />
                        <br />
                        <asp:Label runat="server" ID="LBAvvisoFine" Visible="true"></asp:Label>
                        <br />
                        <asp:Button runat="server" ID="BTNDopo" EnableViewState="False" Visible="false" />
                        <asp:Button runat="server" ID="BTNFine" EnableViewState="False" Visible="true" />
                        <br />
                        <br />
                    </asp:View>
                    <asp:View ID="VIWdescrizione" runat="server">
                        <asp:Panel ID="PNLIndietro" runat="server" Width="100%" HorizontalAlign="right" Visible="false">
                            <asp:LinkButton ID="LNBindietro" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:Label ID="LBnomeVIWDescrizione" runat="server" Text='<%#Eval("nome")%>'></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="LBdescrizioneVIWDescrizione" runat="server" Text='<%#Eval("descrizione")%>'></asp:Label>
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="LBTempoRimanenteVIWDescrizione" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="LBAvvisoRispostaNonEditabile" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="LBdurata" runat="server" Visible="false"></asp:Label>
                        <br />
                        <br />
                        <p align="center">
                            <asp:Label runat="server" ID="LBLErrorePassword" Style="display: none; color: Red;
                                font-weight: bold"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="LBLPassword" Text="Password: " Visible='<%#Eval("isPassword")%>'></asp:Label>
                            <asp:TextBox ID="TXBPassword" runat="server"></asp:TextBox>
                            <br />
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
                        <br />
                        <br />
                        <asp:Label ID="LBErrore" runat="server"></asp:Label>
                        <asp:Label ID="LBConferma" runat="server" Visible="false"></asp:Label>
                        <br />
                        <asp:Button ID="BTNRestartAutoEval" runat="server" Visible="false" />
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
