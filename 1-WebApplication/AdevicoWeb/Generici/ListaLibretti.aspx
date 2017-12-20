<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ListaLibretti.aspx.vb" Inherits="Comunita_OnLine.ListaLibretti" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CTRLFOOTER" Src="../UC/UC_Footer.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    function ChangeCheckBoxState(id, checkState) {
        var cb = document.getElementById(id);
        if (cb != null)
            cb.checked = checkState;
    }

    function ReverseCheckBoxState(id) {
        var cb = document.getElementById(id);
        if (cb != null)
            cb.checked = !cb.checked;
    }

    function ChangeAllCheckBoxStates(checkState) {
        // Toggles through all of the checkboxes defined in the CheckBoxIDs array
        // and updates their value to the checkState input parameter
        if (CheckBoxIDs != null) {
            for (var i = 0; i < CheckBoxIDs.length; i++)
                ChangeCheckBoxState(CheckBoxIDs[i], checkState);
        }
    }

    function ReverseAllCheckBoxStates() {
        // Toggles through all of the checkboxes defined in the CheckBoxIDs array
        // and updates their value to the checkState input parameter
        if (CheckBoxIDs != null) {
            for (var i = 0; i < CheckBoxIDs.length; i++)
                ReverseCheckBoxState(CheckBoxIDs[i]);
        }
    }
   
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	   	  <div id="DVtitle" style="width: 900px; text-align:left;" class="RigaTitolo" align="center">
			 <asp:Label ID="LBtitoloServizio" Runat="server">Libretto elettronico - Servizio sperimentale</asp:Label>
		  </div>
	   	  <div align="center" style="width: 900px;  padding-top:5px; ">
		  <asp:MultiView ID="MLVlibretti" runat="server" ActiveViewIndex=0>
			 <asp:View ID="VIWselezione" runat="server" >
				<div align="right" style="width: 900px; text-align:left;  padding-bottom:10px; ">
				      <asp:Button ID="BTNshowForUser" runat="server" Text="Mostra personali" />
				      <asp:Button ID="BTNshowForCommunity" runat="server" Text="Mostra per comunità" />
				</div>
				<br />
				<div>
				    <table>
			 		   <tr>
						  <td>
						  
						  </td>
					   </tr>
				    </table>
				    <asp:GridView ID="GDVregistri" runat="server" AllowPaging="True" AllowSorting="True" SkinID="mostraVideocast" DataKeyNames="ID" UseAccessibleHeader="true" PageSize="15">
					   <Columns>
						  <asp:BoundField DataField="ID" Visible="false" />
						  <asp:CommandField SelectText="Visualizza" ShowSelectButton="True" HeaderText="Opzioni" AccessibleHeaderText="Visualizza il dettaglio del registro"  />
						  <asp:TemplateField HeaderText="Corso" SortExpression="AttivitaDidattica.CompleteName">
							 <ItemTemplate>
								<%#Container.DataItem.AttivitaDidattica.CompleteName%>
							 </ItemTemplate>
						  </asp:TemplateField>
						  <asp:TemplateField HeaderText="Creato il" SortExpression="DataInserimento" AccessibleHeaderText="Ordina per data creazione" >
							 <ItemTemplate>
								<asp:Label ID="LBcreatoIl" runat="server"></asp:Label>
							 </ItemTemplate>
						  </asp:TemplateField>
						  <asp:TemplateField HeaderText="Modificato il" SortExpression="DataModifica">
							  <ItemTemplate>
								<asp:Label ID="LBmodificatoIl" runat="server"></asp:Label>
							 </ItemTemplate>
						  </asp:TemplateField>
						  <asp:TemplateField HeaderText="status" SortExpression="Stato">
							 <ItemTemplate>
								<asp:Label ID="LBstatus" runat="server"></asp:Label>
							 </ItemTemplate>
						  </asp:TemplateField>
					   </Columns>
				    </asp:GridView>
				</div>
			 </asp:View>
			 <asp:View ID="VIWregistro" runat="server">
				<div align="center" style=" text-align:right; width: 900px">
				    <div align="left" style="text-align:left; float:left;">
					   <asp:Literal id="LTlastUpdateDetails" runat="server"></asp:Literal>
				    </div>
				    <div align="right" style="text-align:right; clear:right">
					   <asp:Button ID="BTNreturnToList" runat="server" Text="Torna all'elenco" />
					   <asp:Button ID="BTNshowDetails" runat="server" Text="Mostra dettagli" />
					   <asp:Button ID="BTNhideDetails" runat="server" Text="Nascondi dettagli" />
					   <asp:Button ID="BTNimport" runat="server" Text="Importa in comunità" /> 
				    </div>
				</div>
				<br />
				<div align="center" style=" text-align:center; width: 800px">
				    <div align="left" style=" text-align:left; height:20px;">
					   <asp:Label id="LBcorso_t" runat="server" AssociatedControlID="LBcorso" CssClass="Titolo_campoSmall"></asp:Label>&nbsp;<asp:Label id="LBcorso" runat="server"></asp:Label>
				    </div>
				    <div align="left" style=" text-align:left;">
					   <div align="left" style="text-align:left; float:left; position:relative ">
						  <asp:Label id="LBdocenti_t" runat="server" AssociatedControlID="BLSdocenti" CssClass="Titolo_campoSmall"></asp:Label>
					   </div>
					   <div align="left" style="text-align:left; float:left; margin-top:-1em; padding-top:1em  ">
						  <asp:BulletedList id="BLSdocenti" runat="server"></asp:BulletedList>
					   </div>
				    </div>
				    <div align="left" style=" text-align:left; padding-bottom: 5px; clear:left; ">
					   <asp:Label id="LBcreate_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="LBcreate">Creato il:</asp:Label>
					   &nbsp;<asp:Label id="LBcreate" runat="server"></asp:Label>
					   &nbsp;&nbsp;
					    <asp:Label id="LBmodifyAt_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="LBmodifyAt">Modificato il:</asp:Label>
					   &nbsp;<asp:Label id="LBmodifyAt" runat="server"></asp:Label>
				    </div>
				    <div align="left" style=" text-align:left; padding-bottom: 5px; clear:left;  ">
					   <div align="left" style="text-align:left; float:left; position:relative">
						  <asp:Label id="LBactivitySummary_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="LBactivitySummary">Stato lavorazione:</asp:Label>
					   </div>
					   <div align="left" style="text-align:left; float:left; margin-top:-1em; padding-top:1em">
						  <asp:Label id="LBactivitySummary" runat="server"></asp:Label>
					   </div>
				    </div>
				    <div align="left" style=" text-align:left; padding-bottom: 5px; clear:left; " >
					   <asp:Label id="LBstatus_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="LBstatus">Stato Registro:</asp:Label>
					   &nbsp;<asp:Label id="LBstatus" runat="server"></asp:Label>
				    </div>
				    <div align="center" style=" text-align:center;  ">
					    <div style="width: 700px; text-align:left;" align="center">
						  <asp:MultiView ID="MLVregistroDettagli" runat="server" ActiveViewIndex="0">
							 <asp:View ID="VIWdettagliSemplificati" runat="server">
	   							<asp:GridView ID="GDVdettagli" runat="server" SkinID="griglia700" DataKeyNames="ID" AutoGenerateColumns="false">
								   <Columns>
									    <asp:BoundField DataField="ID" Visible="False" />
									    <asp:BoundField DataField="DataSvolgimento" DataFormatString="{0:d}" HtmlEncode="False" HeaderText="Data"/>
									    <asp:BoundField DataField="OreAccademiche" HeaderText="Ore"/>
									    <asp:BoundField DataField="Titolo" HeaderText="Titolo"/>
									    <asp:TemplateField HeaderText="Tipo Attivita">
										   <ItemTemplate>
											  <%#Container.DataItem.Tipo.Name%>
										   </ItemTemplate>
									    </asp:TemplateField>
								   </Columns>
							    </asp:GridView>
		  					 </asp:View>
							 <asp:View ID="VIWdettagliEstesi" runat="server">
								<div style=" border-style:solid; width: 700px; border-width:1px; ">
								    <table cellSpacing="0" cellPadding="0" width="700px" align="center">
									   <asp:Repeater ID="RPTlezioni" runat="server">
										  <ItemTemplate>
											 <tr>
												<td scope="row" width="700px" align="left" style=" background-color:#00008b; border-color:#00008b; height: 18px; padding-left:5px; color:White; font-size: x-small;">
												    <asp:Label Runat=server id="LBdataLezione"></asp:Label>
												</td>	
											 </tr>
											 <tr>
												<td width="700px" style=" background-color:#ffffff; border-right-color:Black; border-left-color: Black; ">
												    <div id="DIVtitolo" runat="server" style=" padding: 5px;">
													    <div align="left" style="text-align:left; float:left;width:100px">
														  <asp:Label Runat=server ID="LBtitolo_t" CssClass="Titolo_campoSmall">Titolo:</asp:Label>
													    </div>
													    <div align="left" style="text-align:left;">
														  <asp:Label Runat=server ID="LBtitolo" CssClass="Testo_campoSmall"><%#Container.DataItem.Titolo%></asp:Label>
													    </div>
												    </div>
												    <div id="DIVprogramma" runat="server" style=" padding: 0px 5px 5px 5px;">
													   <div align="left" style="text-align:left; float:left;width:100px">
														  <asp:Label Runat=server ID="LBprogramma_t" CssClass="Titolo_campoSmall">Programma:</asp:Label>
													   </div>
													   <div align="left" style="text-align:left;">
														  <asp:Label Runat=server ID="LBprogramma" CssClass="Testo_campoSmall"><%#Container.DataItem.Descrizione%></asp:Label>
													   </div>
												    </div>
												    <div id="DIVcompresenze" runat="server" style=" padding: 0px 5px 5px 5px;">
													   <div align="left" style="text-align:left; float:left;width:100px">
														  <asp:Label Runat=server ID="LBcompresenze_t" CssClass="Titolo_campoSmall">Compresenze:</asp:Label>
													   </div>
													   <div align="left" style="text-align:left;">
														  <asp:Label Runat=server ID="LBcompresenze" CssClass="Testo_campoSmall"><%#Container.DataItem.Descrizione%></asp:Label>
													   </div>
												    </div>
												    <div id="DIVgruppo" runat="server" style=" padding: 0px 5px 5px 5px;">
													   <div align="left" style="text-align:left; float:left;width:100px">
														  <asp:Label Runat=server ID="LBgruppi_t" CssClass="Titolo_campoSmall">Gruppi:</asp:Label>
													   </div>
													   <div align="left" style="text-align:left;">
														  <asp:Label Runat=server ID="LBgruppi" CssClass="Testo_campoSmall"><%#Container.DataItem.Descrizione%></asp:Label>
													   </div>
												    </div>
												</td>
											 </tr>
										  </ItemTemplate>
									   </asp:Repeater>
			 					    </table>
								</div>
							 </asp:View>
							 <asp:View ID="VIWnoDetails" runat="server">
							 </asp:View>
						  </asp:MultiView>
					   </div>
				    </div>
				</div>
			 </asp:View>
			 <asp:View id="VIWnoLibretti" runat="server">
				<div align="center" style="width: 700px; text-align:left; padding-top: 100px; padding-bottom: 200px;">
				    <asp:Label id="LBnoLibretti" runat="server"></asp:Label>
				</div>
			 </asp:View>
			 <asp:View ID="VIWpermessi" runat="server">
				<div id="DVpermessi" align="center" style=" padding-top: 200px; padding-bottom: 200px;">
				    <asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio">Errore nell'accesso alla pagina</asp:Label>
				</div>
			 </asp:View>
		  </asp:MultiView>
	  </div>
</asp:Content>


<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title id="Title1" runat="server">Comunità on Line - Libretto elettronico per certificazione attività</title>
    <LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
</head>

<body>
     <form id="aspnetForm" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
	   <div id="DVcontenitore" align="center">
		  <div id="DVheader" style="width: 900px; text-align:left;" align="center">
			 <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
		  </div>

		  <div id="DVfooter" align="center" style="clear: both; width: 900px;">
		  <FOOTER:CTRLFOOTER id="Piede" runat="server"></FOOTER:CTRLFOOTER>
	   </div>
	   </div>
    </form>
</body>
</html>--%>
