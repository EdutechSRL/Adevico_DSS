<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="StatisticheAccessi.aspx.vb" Inherits="Comunita_OnLine.StatisticheAccessi" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" Namespace="Telerik.WebControls" Assembly="RadChart.Net2" %>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
	    <!--
	    #DivCal{
		    background-color: lightblue;
		    position: absolute;
		    left: 890px;
		    top: 230px;
            Z-INDEX: -1;
		    width: 200px;
		    VISIBILITY: hidden;
		    }
	    -->
    </style>
    <style type="text/css">
        @import url(./../Jscript/Calendar/calendar-blue.css);
    </style>
    <script type="text/javascript" src="./../Jscript/Calendar/calendar.js"></script>
    <script type="text/javascript" src="./../Jscript/Calendar/calendar-setup.js"></script>
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <style type="text/css">
        td
        {
            font-size: 11px;
        }
    </style>
    <%=CalendarScript() %>
    <%--    <script type="text/javascript" language="javascript">
    	function SubmitRicerca(event) {
    		if (document.all) {
    		    if (event.keyCode == 13) {
    		        event.returnValue = false;
    		        event.cancel = true;
    		        return false;
    		    }
    		}
    		else if (document.getElementById) {
    		    if (event.which == 13) {
    		        event.returnValue = false;
    		        event.cancel = true;
    		        return false;
    		    }
    		}
    		else if (document.layers) {
    		    if (event.which == 13) {
    		        event.returnValue = false;
    		        event.cancel = true;
    		        return false;
    		    }
    		}
    		else
    		    return true;
    	}
	</script>--%>
    <%--<body onkeydown="return SubmitRicerca(event);">--%>
    <%--<script type="text/javascript">
        < %= Me.BodyId() % >.onkeydown = return SubmitRicerca(event);
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table width="900px" align="center">
        <%--	    <tr>
		    <td class="RigaTitolo" align="left">
			    <asp:Label id="LBtitolo" Runat="server">Statistiche Accessi Comunità On Line -</asp:Label>
		    </td>
	    </tr>--%>
        <tr>
            <td align="center">
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False" HorizontalAlign="Center"
                    Width="900px">
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LBNopermessi" CssClass="messaggio" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center" Width="900px">
                    <asp:Table ID="TBLdati" runat="server" Width="900px" BorderColor="Navy">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <telerik:RadTabStrip ID="TBSmenu" runat="server" Align="Justify" Width="900px" Height="26px"
                                    SelectedIndex="0" CausesValidation="false" AutoPostBack="true" Skin="Outlook"
                                    EnableEmbeddedSkins="true">
                                    <Tabs>
                                        <telerik:RadTab Text="Giornaliera" Value="TABgiorno" runat="server" Width="50px">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Settimanale" Value="TABsettimanale" runat="server" Width="50px">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Mensile" Value="TABmensile" runat="server" Width="50px">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Annuale" Value="TABannuale" runat="server" Width="50px">
                                        </telerik:RadTab>
                                        <telerik:RadTab Text="Iscritti" Value="TABiscritti" runat="server" Width="50px">
                                        </telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBRFiltri" runat="server">
                            <asp:TableCell ColumnSpan="2">
                                <input type="hidden" id="HDNDataMin" runat="server" name="HDNDataMin" />
                                <input type="hidden" id="HDNDataMax" runat="server" name="HDNDataMax" />
                                <input type="hidden" id="HDNdataI" runat="server" name="HDNdataI" />
                                <table width="900" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td rowspan="3">
                                            <asp:Panel ID="PNLStat_Giorno" runat="server">
                                                <table width="150" border="0">
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Label ID="LBtotaleG" CssClass="FiltroVoceSmall" runat="server">Totale:</asp:Label>
                                                        </td>
                                                        <td rowspan="5" width="10">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LBtotaleG_giorno_t" runat="server" CssClass="FiltroVoceSmall"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <b>
                                                                <asp:Label ID="LBtotaleG_giorno" runat="server" CssClass="FiltroCampoSmall"></asp:Label></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LBtotaleG_settimana_t" runat="server" CssClass="FiltroVoceSmall"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <b>
                                                                <asp:Label ID="LBtotaleG_settimana" runat="server" CssClass="FiltroCampoSmall"></asp:Label></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LBtotaleG_mese_t" runat="server" CssClass="FiltroVoceSmall"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <b>
                                                                <asp:Label ID="LBtotaleG_mese" runat="server" CssClass="FiltroCampoSmall"></asp:Label></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LBtotaleG_anno_t" runat="server" CssClass="FiltroVoceSmall"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <b>
                                                                <asp:Label ID="LBtotaleG_anno" runat="server" CssClass="FiltroCampoSmall"></asp:Label></b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LBorganizzazione_t" runat="server" CssClass="FiltroVoceSmall">Organizzazione/Facoltà:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDLorgnIscritti" runat="server" CssClass="FiltroCampoSmall"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Label ID="LBL_Data_t" runat="server" CssClass="FiltroVoceSmall">Data:</asp:Label>
                                            &nbsp;
                                            <asp:Label ID="LBdataI" runat="server" CssClass="Testo_campoSmall" Text=""></asp:Label>
                                            <asp:DropDownList ID="DDLgiorno" runat="server" Visible="False">
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:ImageButton ID="IMBapriInizio" ImageUrl="../images/cal.gif" runat="server" CausesValidation="False">
                                            </asp:ImageButton>
                                            &nbsp;
                                            <asp:DropDownList ID="DDLmese" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:DropDownList ID="DDLanno" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LBLVisualizzazione_t" runat="server" CssClass="FiltroVoceSmall">Visualizza: </asp:Label>
                                            <asp:RadioButtonList ID="RBLvisualizzaIscrizioni" CssClass="FiltroCampoSmall" AutoPostBack="True"
                                                RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" Visible="False">
                                                <asp:ListItem Value="1">Andamento Iscrizioni</asp:ListItem>
                                                <asp:ListItem Value="0" Selected="True">Iscritti</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RadioButtonList ID="RBLvisualizzaAltro" CssClass="FiltroCampoSmall" AutoPostBack="True"
                                                RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                <asp:ListItem Value="1" Selected="True">Andamento</asp:ListItem>
                                                <asp:ListItem Value="0">Confronto</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LBLOpzioni" CssClass="FiltroVoceSmall" runat="server">Opzioni: </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="RBLtipoChart" runat="server" CssClass="FiltroCampoSmall"
                                                            RepeatDirection="Horizontal" AutoPostBack="True">
                                                            <asp:ListItem Selected="true" Value="0">Bar</asp:ListItem>
                                                            <asp:ListItem Value="1">StackedBar</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="CBmostraLabel" runat="server" AutoPostBack="True" Checked="true"
                                                            Text="Mostra Valori" CssClass="FiltroCampoSmall"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="CBautopostback" runat="server" AutoPostBack="True" Text="AutoAggiornamento"
                                                CssClass="FiltroCampoSmall" Checked="True"></asp:CheckBox>
                                            &nbsp;
                                            <asp:Button ID="BTNesegui" runat="server" Text="Esegui" CssClass="Pulsante" Visible="False">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <CTRL:RadChart ID="CTRLchart" SeriesOrientation="vertical" runat="server" Width="800px"
                                    Height="450px">
                                </CTRL:RadChart>
                                <asp:Label ID="LBnoRecord" CssClass="errore" runat="server" Visible="False"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="200px">
                                <asp:Panel ID="PNLRuoli" runat="server">
                                    <asp:Label ID="LBruoli_t" runat="server" CssClass="FiltroVoceSmall">Tipo Persona:</asp:Label><br />
                                    <asp:CheckBox ID="CBtutti" runat="server" Checked="True" Text="Tutti" AutoPostBack="True"
                                        CssClass="FiltroCampoSmall"></asp:CheckBox>
                                    <br />
                                    <asp:CheckBoxList ID="CBLtipoPersona" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
                                        CssClass="FiltroCampoSmall" Enabled="False" AutoPostBack="True" RepeatLayout="Table">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:Panel ID="PNLAnniConf" runat="server">
                                    <asp:Label ID="LBL_AnnoConf" runat="server" CssClass="FiltroVoceSmall">Anni: </asp:Label><br />
                                    <asp:CheckBoxList ID="CBL_AnnoConf" runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall">
                                        <asp:ListItem>2006</asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:Panel ID="PNLIscrittiIscrizioni" runat="server" Visible="False">
                                    <asp:CheckBox ID="CBtuttiIscritti" runat="server" Checked="True" Text="Tutti" AutoPostBack="True"
                                        CssClass="FiltroCampoSmall"></asp:CheckBox>
                                    <asp:CheckBoxList ID="CBLtipoPersonaIscritti" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
                                        CssClass="FiltroCampoSmall" Enabled="False" AutoPostBack="True" RepeatLayout="Table"
                                        Width="150">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBRData" runat="server">
                            <asp:TableCell ColumnSpan="2" BorderWidth="0">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:Label ID="LBL_TableData" runat="server" Style="overflow: scroll; display: block;
                                                width: 900px;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <br />
                                            <asp:LinkButton ID="LNK_DownloadData" runat="server" CssClass="LINK_MENU"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
function selectInit(calendar,date){
	if (calendar.dateClicked){
		dataI = date
		dataIniziale = dataI.split("/")
		
		dataIMin = document.getElementById("<%=me.HDNDataMin.clientID%>").value
		dataInizialeMin = dataIMin.split("/")
		
		dataIMax = document.getElementById("<%=me.HDNDataMax.clientID%>").value
		dataInizialeMax = dataIMax.split("/")
		
		var dataInizio = new Date()
		dataInizio = Date.parse(dataIniziale[1] + '/' + dataIniziale[0] + '/' + dataIniziale[2])
		var dataInizioMin = new Date()
		dataInizioMin = Date.parse(dataInizialeMin[1] + '/' + dataInizialeMin[0] + '/' + dataInizialeMin[2])
		var dataInizioMax = new Date()
		dataInizioMax = Date.parse(dataInizialeMax[1] + '/' + dataInizialeMax[0] + '/' + dataInizialeMax[2])
		
		if (dataInizio<dataInizioMin){
			document.getElementById("<%=me.HDNdataI.clientID%>").value = document.getElementById("<%=me.HDNdataMin.clientID%>").value
			document.getElementById("<%=me.LBdataI.clientID%>").innerHTML = document.getElementById("<%=me.HDNdataMin.clientID%>").value
			
		} else if (dataInizio>dataInizioMax) {
			document.getElementById("<%=me.HDNdataI.clientID%>").value = document.getElementById("<%=me.HDNdataMax.clientID%>").value
			document.getElementById("<%=me.LBdataI.clientID%>").innerHTML = document.getElementById("<%=me.HDNdataMax.clientID%>").value
			
		} else {
			document.getElementById("<%=me.HDNdataI.clientID%>").value = date
			document.getElementById("<%=me.LBdataI.clientID%>").innerHTML = date

		}
		
		<% if CBautopostback.checked then %>
		__doPostBack('IMBapriInizio','');
		<%else%>
		calendar.callCloseHandler();
		<% end if %>
	}
}
	
Calendar.setup({
	ifFormat : "%d/%m/%Y",
	inputField : "<%=me.HDNdataI.clientID%>",
	displayArea: "<%=me.LBdataI.clientID%>",
	button : "<%=me.IMBapriInizio.clientID%>",
	firstDay : 1,
	onSelect : selectInit
	}
	);
	
    </script>
</asp:Content>
<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
	<head runat="server">
		<title>Comunità On Line - Statistiche Accessi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		

		</head>
		<%
		try
			Select Case Session("LinguaCode")
                Case "it-IT"
                    response.write("<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-it.js" & """" &"></script>")
                Case "en-US"
                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
                Case "de-DE"
                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-de.js" & """" &"></script>")
                Case "fr-FR"
                   response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-fr.js" & """" &"></script>")
                Case "es-ES"
                    response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-es.js" & """" &"></script>")
                Case Else
                  response.write("<script type=text/javascript src=" & """" &  "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
            End Select
		catch ex as exception
			response.write("<script type=text/javascript src=" & """" & "./../Jscript/Calendar/lang/calendar-en.js" & """" &"></script>")
		end try%>

	<body onkeydown="return SubmitRicerca(event);">
		 <form id="Form2" method="post" runat="server">
		 <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table cellSpacing="0" cellPadding="0" width="780" align="center" border="0">
				<tr>
					<td colSpan="3">
						<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
					</td>
				</tr>
				<tr>
					<td colSpan="3" align="center">
						
					</td>
				</tr>
			</table>
		<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>

		</form>
	</body>
</HTML>--%>