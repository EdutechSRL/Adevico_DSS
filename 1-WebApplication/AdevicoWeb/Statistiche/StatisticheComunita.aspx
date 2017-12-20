<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="StatisticheComunita.aspx.vb" Inherits="Comunita_OnLine.StatisticheComunita" %>
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

	<style type=text/css>@import url(./../Jscript/Calendar/calendar-blue.css);</style>

	<script type=text/javascript src="./../Jscript/Calendar/calendar.js"></script>
		
	<script type=text/javascript src="./../Jscript/Calendar/calendar-setup.js"></script>
	
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
	<style type="text/css">
		td{
		font-size: 11px;
		}
	</style>

    <%=CalendarScript() %>
    <%-- 
    <script type="text/javascript" language="javascript">
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
    </script>

    <script type="text/javascript">
        < %= Me.BodyId() % >.onkeydown = SubmitRicerca(event);
    </script>
    --%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table width="900px" align="center">
<%--		<tr>
			<td class="RigaTitolo" align="left">
				<asp:Label id="LBtitolo" Runat="server">Statistiche Accessi Comunità On Line -</asp:Label>
			</td>
		</tr>--%>
		<tr>
			<td align="center">
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign="Center" Width=900px>
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" CssClass="messaggio" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td vAlign="top" height="50">
								&nbsp;
							</td>
						</tr>
					</table>
				</asp:Panel>
									
                <asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" Width="900px">
	                <asp:table ID="TBLdati" Runat="server" Width="900px" BorderColor="Navy">
		                <asp:TableRow >
			                <asp:TableCell ColumnSpan="2">
				                <telerik:RadTabStrip ID="TBSmenu" runat="server" Align="Justify" Width="850px" Height="26px"
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
	                <asp:TableRow ID="TBRFiltri" Runat="server">
		                <asp:TableCell ColumnSpan="2">
			                <input type=hidden id="HDNDataMin" runat=server NAME="HDNDataMin"/>
			                <input type=hidden id="HDNDataMax" runat=server NAME="HDNDataMax"/>
			                <input type=hidden id="HDNdataI" runat=server NAME="HDNdataI"/>
			                <table width="900" border="0" cellpadding="0" cellspacing="0">
				                <tr>
					                <td rowspan="3">
						                <asp:Panel ID="PNLStat_Giorno" Runat="server">
							                <table width="150" border="0" cellpadding="0" cellspacing="5">
								                <tr><td colspan=3>
									                <asp:Label id="LBtotaleG" CssClass="FiltroVoceSmall" Runat="server">Totale:</asp:Label>
								                </td><td rowspan="5" width="10">
									                &nbsp;
								                </td></tr><tr><td>
									                <asp:Label ID="LBtotaleG_giorno_t" Runat=server CssClass="FiltroVoceSmall"></asp:Label>
								                </td><td align=right>
									                <b><asp:Label ID="LBtotaleG_giorno" Runat=server CssClass="FiltroCampoSmall"></asp:Label></b>
								                </td></tr><tr><td>
									                <asp:Label ID="LBtotaleG_settimana_t" Runat=server CssClass="FiltroVoceSmall"></asp:Label>
								                </td><td align=right>
									                <b><asp:Label ID="LBtotaleG_settimana" Runat=server CssClass="FiltroCampoSmall"></asp:Label></b>
								                </td></tr><tr><td>
									                <asp:Label ID="LBtotaleG_mese_t" Runat=server CssClass="FiltroVoceSmall"></asp:Label>
								                </td><td align=right>
									                <b><asp:Label ID="LBtotaleG_mese" Runat=server CssClass="FiltroCampoSmall"></asp:Label></b>
								                </td></tr><tr><td>
									                <asp:Label ID="LBtotaleG_anno_t" Runat=server CssClass="FiltroVoceSmall"></asp:Label>
								                </td><td align=right>
									                <b><asp:Label ID="LBtotaleG_anno" Runat=server CssClass="FiltroCampoSmall"></asp:Label></b>
								                </td></tr>		
							                </table>
						                </asp:Panel>
						                <asp:Panel ID="PNLStat_Settimana" Runat="server">
							                <asp:Label id="LBtotaleSett" CssClass="FiltroVoceSmall" Runat="server">Totale:</asp:Label>
						                </asp:Panel>
						                <asp:Panel ID="PNLStat_Mese" Runat="server">
							                <asp:Label id="LBtotaleMens" CssClass="FiltroVoceSmall" Runat="server">Totale:</asp:Label>
						                </asp:Panel>
						                <asp:Panel ID="PNLStat_Anno" Runat="server">
							                <asp:Label id="LBtotaleAnn" CssClass="FiltroVoceSmall" Runat="server">Totale:</asp:Label>
						                </asp:Panel>
						                &nbsp;
					                </td><td valign="top">
						                <table cellpadding="0" cellspacing="0"><tr>
						                <td>
							                <asp:Label ID="LBLOpzioni" CssClass="FiltroVoceSmall" Runat="server">Opzioni: </asp:Label>
						                </td><td>
							                <asp:RadioButtonList ID="RBLtipoChart" Runat=server CssClass="FiltroCampoSmall" RepeatDirection =Horizontal AutoPostBack=True >
								                <asp:ListItem Selected =true Value=0>Bar</asp:ListItem>
								                <asp:ListItem  Value=1>StackedBar</asp:ListItem>
							                </asp:RadioButtonList>
						                </td><td>
							                <asp:CheckBox ID="CBmostraLabel" Runat =server AutoPostBack =True  Checked =true Text ="Mostra Valori" CssClass="FiltroCampoSmall"></asp:CheckBox>
						                </td></tr></table>
					                </td><td align="right" valign="top" >
						                <asp:Label ID="LBL_Data_t" Runat="server" CssClass="FiltroVoceSmall">Data:</asp:Label>
						                &nbsp;
						                <asp:label id="LBdataI" Runat="server" CssClass="Testo_campoSmall" text=""></asp:label>
						                <asp:DropDownList ID="DDLgiorno" Runat="server" Visible=False></asp:DropDownList>
						                &nbsp;
						                <asp:ImageButton ID="IMBapriInizio" ImageUrl="../images/cal.gif" Runat="server" CausesValidation="False"></asp:ImageButton>
						                &nbsp;
						                <asp:DropDownList ID="DDLmese" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=true></asp:DropDownList>
						                &nbsp;
						                <asp:DropDownList ID="DDLanno" Runat="server" CssClass="FiltroCampoSmall" AutoPostBack=true></asp:DropDownList>
					                </td>
				                </tr>
				                <tr>
					                <td>&nbsp;</td>
					                <td align="right">
						                <asp:CheckBox ID=CBautopostback Runat=server AutoPostBack=True Text="AutoAggiornamento" CssClass="FiltroCampoSmall" Checked="True"></asp:CheckBox>
						                &nbsp;
						                <asp:button id="BTNesegui" Runat="server"  Text="Esegui" CssClass="Pulsante" Visible="False"></asp:button>
					                </td>
				                </tr>
			                </table>
		                </asp:TableCell>
	                </asp:TableRow>
	                <asp:TableRow>
		                <asp:TableCell>
			                <CTRL:radchart id="CTRLchart" seriesorientation="vertical" runat="server"  Width=800px Height=450px></CTRL:radchart>
			                <asp:Label id="LBnoRecord" CssClass="errore" Runat="server" Visible=False ></asp:Label>
		                </asp:TableCell>
		                <asp:TableCell Width="200px">
			                <asp:Panel ID="PNLRuoli" Runat="server">
				                <asp:Label ID="LBruoli_t" Runat=server CssClass="FiltroVoceSmall">Tipo Persona:</asp:Label><br/>
				                <asp:CheckBox ID=CBtutti Runat=server Checked =True Text="Tutti" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:CheckBox>
				                <br/>
				                <asp:CheckBoxList ID=CBLtipoPersona Runat=server RepeatColumns=1 RepeatDirection=Horizontal CssClass="FiltroCampoSmall" Enabled=False AutoPostBack=True RepeatLayout=Table></asp:CheckBoxList>
			                </asp:Panel>

			                <asp:Panel ID="PNLIscrittiIscrizioni" Runat="server" Visible="False" >			
				                <asp:CheckBox ID="CBtuttiIscritti" Runat=server Checked =True Text="Tutti" AutoPostBack=True CssClass="FiltroCampoSmall"></asp:CheckBox>	
				                <asp:CheckBoxList ID="CBLtipoPersonaIscritti" Runat=server RepeatColumns=1 RepeatDirection=Horizontal CssClass="FiltroCampoSmall" Enabled=False AutoPostBack=True RepeatLayout=Table></asp:CheckBoxList>	
			                </asp:Panel>
		                </asp:TableCell>
	                </asp:TableRow>
	                <asp:TableRow>
	                    <asp:TableCell Width="800px" HorizontalAlign="Right">
	                        <asp:LinkButton ID="LNK_DownloadData" Runat="server" CssClass="LINK_MENU"></asp:LinkButton>
	                    </asp:TableCell>
	                    <asp:TableCell>
	                        &nbsp;
	                    </asp:TableCell>
	                </asp:TableRow>
	                </asp:table>
	
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
<HTML>
	<head runat="server">
		<title>Comunità On Line - Statistiche Accessi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		

		</HEAD>
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