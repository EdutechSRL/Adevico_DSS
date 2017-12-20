<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" Codebehind="AdminG_WizardTipoComunita.aspx.vb" Inherits="Comunita_OnLine.AdminG_WizardTipoComunita"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>


<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <style type="text/css">
		td{
		    font-size: 11px;
		}
	</style>
		
	<script type="text/javascript" language=javascript>
		function SelectFromNameAndAssocia(Nome, value) {
		    var HIDcheckbox;
		    //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
		    HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
		    for (i = 0; i < document.forms[0].length; i++) {
		        e = document.forms[0].elements[i];
		        if (e.type == 'checkbox' && e.name == Nome) {//"CBXassocia"
		            if (e.checked == true) {
		                if (HIDcheckbox.value == "")
		                    HIDcheckbox.value = ',' + value + ','
		                else {
		                    pos1 = HIDcheckbox.value.indexOf(',' + value + ',')
		                    if (pos1 == -1)
		                        HIDcheckbox.value = HIDcheckbox.value + value + ','
		                }
		            }
		            else {
		                valore = HIDcheckbox.value;
		                pos1 = HIDcheckbox.value.indexOf(',' + value + ',')
		                if (pos1 != -1) {
		                    stringa = ',' + value + ','
		                    HIDcheckbox.value = HIDcheckbox.value.substring(0, pos1)
		                    HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1 + value.length + 1, valore.length)
		                }
		            }
		        }
		    }

		    if (HIDcheckbox.value == ",")
		        HIDcheckbox.value = "";
		}
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<input type=hidden id="HDNazione" value="gestioneTipo" runat=server NAME="HDNazione"/>
	<asp:Table ID="TBLprincipale" Runat=server CellPadding=0 GridLines=None Width=900px CellSpacing=0>
<%--		<asp:TableRow>
			<asp:TableCell HorizontalAlign=Left CssClass=RigaTitoloAdmin>
				<asp:Label ID="LBTitolo" Runat="server">Gestione Tipo Comunità</asp:Label>
			</asp:TableCell>
		</asp:TableRow>--%>
		<asp:TableRow ID="TBRmenu">
			<asp:TableCell HorizontalAlign=Right>
				&nbsp;<asp:linkbutton id="LNBindietro" Runat="server" Text="Torna all'elenco" CssClass=Link_Menu CausesValidation=False></asp:linkbutton>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass=top>
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<br/>
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" Width=900px BorderWidth=1>
					<asp:Table Runat=server id="TBLinserimento" CellPadding=0 CellSpacing=0 Width=900px Height=450px>
						<asp:TableRow>
							<asp:TableCell>&nbsp;</asp:TableCell>
							<asp:TableCell HorizontalAlign=left CssClass=top>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLdati" Width=800px Visible=true >
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell width=120px>
											<asp:Label ID="LBtipoComunita_t" Runat=server CssClass="Titolo_CampoSmall">Nome:</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:TextBox ID="TXBtipoComunita" Runat=server CssClass="Testo_campo_obbligatorioSmall" MaxLength=100 Columns=60></asp:TextBox>
											<asp:requiredfieldvalidator id="RFVnome" runat="server" CssClass="Validatori" ControlToValidate="TXBtipoComunita" Display="Dynamic" EnableClientScript=True>*</asp:requiredfieldvalidator>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass=top ColumnSpan=2>
											<table border=1 align=left bgcolor="#FFFBF7" style="border-color:#CCCCCC" cellpadding=0 cellspacing=0>
												<tr>
													<td>
														<table border=0 align=left bgcolor="#FFFBF7" cellpadding=0 cellspacing=0>
															<asp:Repeater id="RPTnome" Runat="server">
																<HeaderTemplate>
																	<tr>
																		<td colspan=2 height=20px>
																			<asp:Label ID="LBlinguaNome_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Traduzioni(°):</asp:Label>
																		</td>
																	</tr>
																</HeaderTemplate>
																<ItemTemplate>
																	<tr>
																		<td align=right width=120px height=22px>
																			<asp:Label id="LBlinguaID" Text='<%# Databinder.eval(Container.DataItem, "ID")%>' runat="server" Visible=false />
																			<asp:Label id="LBlingua_Nome" Text='<%# Databinder.eval(Container.DataItem, "Nome")%>' runat="server" Visible=true CssClass=Repeater_VoceLingua/>&nbsp;
																		</td>
																		<td align=left height=22px>
																			<asp:TextBox ID="TXBtermine" Runat="server" CssClass="Testo_campoSmall" MaxLength="100" Columns="60"> </asp:TextBox>&nbsp;&nbsp;
																		</td>
																	</tr>
																</ItemTemplate>
																<FooterTemplate>
																	<tr><td colspan=2 class=nosize0>&nbsp;</td></tr>
																</FooterTemplate>
															</asp:Repeater>
														</table>
													</td>
												</tr>
											</table>								
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass=top width=120px >
											<asp:Label ID="LBtipoSottoComunita_t" Runat=server CssClass="Titolo_CampoSmall">Sotto comunità:</asp:Label>
										</asp:TableCell>
										<asp:TableCell CssClass=top>
											<asp:CheckBoxList ID=CHLtipoSottoComunita Runat=server CssClass="Testo_campoSmall" RepeatDirection=Horizontal RepeatColumns=3 RepeatLayout=Table></asp:CheckBoxList>
										</asp:TableCell>				
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLcategoriaFile" Width=800px Visible=False>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass=top width=120px Wrap=False >
											<asp:Label ID="LBcategoriaFile_t" Runat=server CssClass="Titolo_CampoSmall">Categorie file Associati:</asp:Label>
										</asp:TableCell>
										<asp:TableCell  CssClass="top">
											<asp:CheckBoxList ID="CBLcategoriaFile" Runat=server CssClass="Testo_campoSmall" RepeatDirection=Vertical RepeatColumns=4 RepeatLayout=Table >
											</asp:CheckBoxList>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLtipoRuolo" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass=top width=120px Wrap=False >
											<asp:Label ID="LBruoloDefault_t" Runat=server CssClass="Titolo_CampoSmall">Ruolo default:</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:DropDownList ID="DDLruoloDefault" Runat=server CssClass="Testo_campoSmall"></asp:DropDownList>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass=top width=120px Wrap=False >
											<asp:Label ID="LBtipiRuolo_t" Runat=server CssClass="Titolo_CampoSmall">Tipo Ruolo Associati:</asp:Label>
										</asp:TableCell>
										<asp:TableCell  CssClass="top">
											<table cellpadding=0 cellspacing=0>
												<tr>
													<td>
														<asp:CheckBoxList ID="CBLtipoRuolo" Runat=server CssClass="Testo_campoSmall" DataValueField ="TPRL_ID" RepeatLayout=Table RepeatColumns=4 RepeatDirection=Horizontal>
														</asp:CheckBoxList>
													</td>
												</tr>
											</table>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLruoliAllways" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell Height=40px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell>
											<asp:Label ID="LBtipiRuoloAll_t" Runat=server CssClass="Titolo_CampoSmall">Selezionare i Ruoli che si intendono rendere sempre disponibili anche nella definizione dei profili personali.</asp:Label>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell  CssClass="top">
											<table cellpadding=0 cellspacing=0>
												<tr>
													<td>
														<asp:CheckBoxList ID="CBLtipoRuoloAll" Runat=server CssClass="Testo_campoSmall" DataValueField ="TPRL_ID" RepeatLayout=Table RepeatColumns=4 RepeatDirection=Horizontal>
														</asp:CheckBoxList>
													</td>
												</tr>
											</table>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=left Runat=server ID="TBLmodelli" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass=top width=150px Wrap=False >
											<asp:Label ID="LBmodelloDefault_t" Runat=server CssClass="Titolo_CampoSmall">Modello default:</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:DropDownList ID="DDLmodelloDefault" Runat=server CssClass="Testo_campoSmall"></asp:DropDownList>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass=top width=150px Wrap=False >
											<asp:Label ID="LBmodelli_t" Runat=server CssClass="Titolo_CampoSmall">Modelli Comunità Associati:</asp:Label>
										</asp:TableCell>
										<asp:TableCell  CssClass="top">
											<table>
												<tr>
													<td>
														<asp:RadioButtonList id="RBLmodelli" Runat="server"  DataValueField="MDCM_ID" DataTextField="vuoto"></asp:RadioButtonList>
													</td>
													<td>
														<asp:CheckBoxList ID="CBLmodelli" Runat=server CssClass="Testo_campoSmall" RepeatDirection=Vertical>
														</asp:CheckBoxList>
													</td>
												</tr>
											</table>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table HorizontalAlign=center Runat=server ID="TBLverificaFinale" Width=800px Visible=False >
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2>
											<asp:Label ID="LBverificaFinale" Runat=server CssClass="info_blackMedium"></asp:Label>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 Height=10px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell width=120px >
											<asp:Label ID="LBicona_t" Runat=server CssClass="Titolo_CampoSmall">Icona:</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<INPUT id="TXBFile" type="file" runat="server" NAME="TXBFile" Class="Testo_campoSmall" size=40/>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
													
													
								<asp:Table ID="TBLservizio" Runat=server Width=750px Visible=False GridLines=none>
									<asp:TableRow Visible=False >
										<asp:TableCell  CssClass="top" Width=150px Wrap=False >
											<asp:Label ID="LBserviziAttivi_t" Runat=server CssClass="Titolo_CampoSmall">Servizi:</asp:Label>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell>
											<asp:Table HorizontalAlign=left Runat=server ID="TBLtipoComunita" GridLines=none >
												<asp:TableRow>
													<asp:TableCell>
														<asp:Label ID="LBorganizzazione_t" Runat=server CssClass="Titolo_campoSmall">Organizzazione:</asp:Label>
													</asp:TableCell>
													<asp:TableCell>
														<asp:DropDownList ID="DDLorganizzazione" AutoPostBack=True CssClass="Testo_campoSmall" Runat=server ></asp:DropDownList>
													</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan=3 CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
												</asp:TableRow>
												<asp:TableRow>
													<asp:TableCell ColumnSpan=2>
														<table>
															<asp:Repeater id="RPTservizio" Runat="server" >
															<HeaderTemplate>
																<tr>
																	<td class="Header_Repeater" width=200px align=left nowrap="nowrap" >
																		<asp:Label ID="LBservizio_t" Runat=server CssClass="titolo_campoSmall">Servizio:&nbsp;&nbsp;</asp:Label>
																	</td>
																	<td align=center class="Header_Repeater" width=70px>
																		<asp:Label ID="LBassociato_t" Runat=server CssClass="titolo_campoSmall">Associa</asp:Label>
																	</td>
																	<td align=center class="Header_Repeater" width=130px>
																		<asp:Label ID="LBattiva_t" Runat=server CssClass="titolo_campoSmall">Attiva di Default</asp:Label>
																	</td>
																</tr>
															</HeaderTemplate>
															<ItemTemplate>
																<tr>
																	<asp:Label id="LBsrvz_ID" Text='<%# Databinder.eval(Container.DataItem, "SRVZ_ID")%>' runat="server" Visible=false />
																	<asp:Label id="LBLKST_id" Text='<%# Databinder.eval(Container.DataItem, "LKST_id")%>' runat="server" Visible=false />
																	<td align=left width=200px nowrap="nowrap" >
																		<asp:Label id="LBservizio" Text='<%# Databinder.eval(Container.DataItem, "SRVZ_Nome")%>' runat="server" Visible=true CssClass=ROW_TD_Small/>
																	</td>
																	<td align=center width=70px>
																		<asp:CheckBox ID="CBXservizioAssociato" Runat=server Text="Si" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckAssociato") %> ></asp:CheckBox>
																	</td>
																	<td align=center width=130px>
																		<asp:CheckBox ID="CBXservizioAttivato" Runat=server Text="Si" CssClass="ROW_TD_Small" Checked=<%# DataBinder.Eval(Container.DataItem, "oCheckDefault") %> ></asp:CheckBox>
																	</td>
																</tr>
															</ItemTemplate>
														</asp:Repeater>
														</table>
													</asp:TableCell>
												</asp:TableRow>
											</asp:Table>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table ID="TBLpermessi" Runat=server Visible=False HorizontalAlign=Left>
									<asp:TableRow>
										<asp:TableCell>
											<asp:Label ID="LBorganizzazionePermessi_t" Runat=server CssClass="Titolo_campoSmall">Organizzazione:</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:DropDownList ID="DDLorganizzazionePermessi" AutoPostBack=True CssClass="Testo_campoSmall" Runat=server ></asp:DropDownList>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell>
											<asp:Label ID="LBserviziPermessi_t" Runat=server CssClass="Titolo_campoSmall">Organizzazione:</asp:Label>
										</asp:TableCell>
										<asp:TableCell>
											<asp:DropDownList ID="DDLserviziPermessi" AutoPostBack=True CssClass="Testo_campoSmall" Runat=server ></asp:DropDownList>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2 CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan=2>
											<asp:Table Runat=server HorizontalAlign=left ID="TBLpermessiRuoli" GridLines=Both CellSpacing=0 CellPadding=2>
																															
											</asp:Table>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
							</asp:TableCell>
							<asp:TableCell Width=5px>&nbsp;</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:Panel>
				<asp:Panel ID="PNLnavigazione" Runat=server HorizontalAlign=Right Width=900px BorderWidth=1>
					<table cellSpacing=0 cellPadding=0 border=0 align=right>
						<tr>
							<td align=right>&nbsp;</td>
							<td nowrap="nowrap" >
								<asp:Button ID="BTNelenco" Runat="server" Text="Annulla" CssClass="PulsanteFiltro" CausesValidation=False></asp:Button>
								<asp:linkbutton id="LNBaddRuolo" Runat="server" Text="Aggiungi ruolo" CssClass=Link_Menu Visible=False CausesValidation=False></asp:linkbutton>
							</td>
							<td width=35>&nbsp;</td>
							<td>
								<asp:Button id=BTNindietro Runat="server" CssClass="pulsante" Text="< Indietro" CausesValidation="False"></asp:Button>
							</td>
							<td width=5>&nbsp;</td>
							<td>
								<asp:Button id=BTNavanti Runat="server" CssClass="pulsante" Text="Avanti >" CausesValidation="True"></asp:Button>
							</td>
							<td width=35>&nbsp;</td>
							<td>
								<asp:Button id=BTNconferma runat="server" CssClass="pulsante" Text="Fine"></asp:Button>
							</td>
							<td width=20>&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
				<asp:Panel ID="PNLnavigazioneFinale" Runat=server HorizontalAlign=Right Width=900px BorderWidth=1 Visible=False >
					<table cellSpacing=0 cellPadding=0 border=0 align=right>
						<tr>
							<td align=right>&nbsp;</td>
							<td nowrap="nowrap">
								<asp:linkbutton id="LNBmanagement" Runat="server" Text="Torna al management" CssClass=Link_Menu CausesValidation=False></asp:linkbutton>
							</td>
							<td width=35>&nbsp;</td>
							<td>
								<asp:LinkButton ID="LNBindetroServizi" Runat=server CssClass=Link_Menu Visible=False CausesValidation=False>Torna ai servizi</asp:LinkButton>
							</td>
							<td width=5>&nbsp;</td>
							<td>
								<asp:LinkButton ID="LNBdefault" Runat=server CssClass=Link_Menu Visible=False>Setta default</asp:LinkButton>
								<asp:LinkButton ID="LNBtipocomunitaForAll" Runat=server CssClass=Link_Menu Visible=False>Replica su tutte</asp:LinkButton>
								<asp:LinkButton ID="LNBsalvaServizi" Runat=server CssClass=Link_Menu Visible=true>Salva impostazioni</asp:LinkButton>
								<asp:LinkButton ID="LNBsalvaPermessi" Runat=server CssClass=Link_Menu Visible=false>Salva impostazioni</asp:LinkButton>
								&nbsp;
								<asp:LinkButton ID="LNBgoToPermessi" Runat=server CssClass=Link_Menu Visible=False CausesValidation=false>Definisci i permessi</asp:LinkButton>
							</td>
							<td width=20>&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<input type=hidden id="HDNtpcm_id" runat=server NAME="HDNtpcm_id"/>
	<input type=hidden id="HIDcheckbox" runat=server NAME="HIDcheckbox"/>
</asp:Content>




<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Wizard Creazione Tipo Comunità</title>
		

		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>

	</HEAD>
	<body onkeydown="return SubmitRicerca(event);">
		<form id="aspnetForm" method="post" runat="server">
		
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table class="contenitore" align="center">
				<tr class="contenitore">
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr class="contenitore">
					<td colSpan="3">

					</td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
		</form>
	</body>
</HTML>--%>