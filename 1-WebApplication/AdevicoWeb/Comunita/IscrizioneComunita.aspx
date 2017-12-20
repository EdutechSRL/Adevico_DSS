<%@ Page Language="vb" ValidateRequest="false" MasterPageFile="~/AjaxPortal.Master" AutoEventWireup="false" CodeBehind="IscrizioneComunita.aspx.vb"
    Inherits="Comunita_OnLine.IscrizioneComunita" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--
--%>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
			function AggiornaForm(){
				valore = document.forms[0].<%=me.DDLTipoRicerca.ClientID%>.options[document.forms[0].<%=me.DDLTipoRicerca.ClientID%>.selectedIndex].value
				if (valore==-9){
					document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore
					__doPostBack('DDLTipoRicerca','');
					return true;
					}
				else if (document.forms[0].<%=me.HDNselezionato.ClientID%>.value == -9){
					document.forms[0].<%=me.HDNselezionato.ClientID%>.value = valore
					__doPostBack('DDLTipoRicerca','');
					return true;
					}
				else 
					return false;
			}


			function SelectMe(Me){
			var HIDcheckbox,selezionati;
			//eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
            //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
            HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>'); 
				selezionati = 0
				for(i=0;i< document.forms[0].length; i++){ 
					e=document.forms[0].elements[i];
					if ( e.type=='checkbox' && e.name.indexOf("CBcorso") != -1 ){
						if (e.checked==true){
							selezionati++
							if (HIDcheckbox.value == ""){
								HIDcheckbox.value = ',' + e.value +','
							}	  
							else{
								pos1 = HIDcheckbox.value.indexOf(',' + e.value  +',')
								if (pos1==-1)
									HIDcheckbox.value = HIDcheckbox.value + e.value  +','
								}
						}
						else{
							valore = HIDcheckbox.value
							pos1 = HIDcheckbox.value.indexOf(',' + e.value  +',')
							if (pos1!=-1){
								stringa = ',' + e.value 
								HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
								HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+e.value.length+1,valore.length)
								}
						}
					}  
				}
				if (HIDcheckbox.value==",")
					HIDcheckbox.value = ""
			}
			
			function SelectAll( SelectAllBox ){
				var actVar = SelectAllBox.checked ;
				var TBcheckbox;
				//eval('HDNcomunitaSelezionate= this.document.forms[0].HDNcomunitaSelezionate')
                //eval('HDNcomunitaSelezionate=<%=Me.HDNcomunitaSelezionate.ClientID%>')
                HDNcomunitaSelezionate = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>'); 
				HDNcomunitaSelezionate.value = ""
				for(i=0;i< document.forms[0].length; i++){ 
					e=document.forms[0].elements[i];
					if ( e.type=='checkbox' && e.name.indexOf("CBcorso") != -1 ){
						e.checked= actVar ;
						if (e.checked==true)
							if (HDNcomunitaSelezionate.value == "")
								HDNcomunitaSelezionate.value = ',' + e.value+','
							else
								HDNcomunitaSelezionate.value = HDNcomunitaSelezionate.value + e.value +','
					}
				}
			}
			
			function HasComunitaSelezionate(conferma,Messaggio,MessaggioConferma){
				var HIDcheckbox,selezionati;
				//eval('HIDcheckbox= this.document.forms[0].HDNcomunitaSelezionate')
                //eval('HIDcheckbox=<%=Me.HDNcomunitaSelezionate.ClientID%>')
                HIDcheckbox = this.document.getElementById('<%=Me.HDNcomunitaSelezionate.ClientID%>'); 
				if (HIDcheckbox.value=="," || HIDcheckbox.value==""){
					alert(Messaggio)
					return false;
					}
				else{
					if (conferma==true){
						return confirm(MessaggioConferma);
						}
					else
						return true;
					}
			}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table width="900px" align="center">
<%--        <tr>
            <td class="RigaTitolo" align="left">
                <asp:Label ID="LBtitolo" runat="server">Iscrizione alle comunità</asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td align="right">
                <table align="right">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <asp:Panel ID="PNLmenuDefault" runat="server" HorizontalAlign="right" Visible="true">
                                &nbsp;<asp:LinkButton ID="LNBiscriviMultipli" Enabled="False" runat="server" CssClass="LINK_MENU"
                                    Text="Iscrivi ai selzionati"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuDettagli" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBannullaDettagli" runat="server" Text="Torna all'elenco" CausesValidation="false"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBiscriviDettagli" runat="server" CssClass="LINK_MENU" CausesValidation="True"
                                    Text="Iscrivi"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuConferma" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBannullaConferma" runat="server" Text="Torna all'elenco" CausesValidation="false"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBiscriviConferma" runat="server" CssClass="LINK_MENU" CausesValidation="True"
                                    Text="Iscrivi"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuIscritto" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBelencoIscritte" runat="server" Text="Torna alle iscritte"
                                    CausesValidation="false" CssClass="LINK_MENU"></asp:LinkButton>
                                <asp:LinkButton ID="LNBiscriviAltre" runat="server" CssClass="LINK_MENU" CausesValidation="True"
                                    Text="Altra iscrizione"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="PNLmenuAccesso" runat="server" HorizontalAlign="Right" Visible="False">
                                <asp:LinkButton ID="LNBannulla" runat="server" Text="Torna all'elenco" CausesValidation="false"
                                    CssClass="LINK_MENU"></asp:LinkButton>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <input type="hidden" runat="server" id="HDNselezionato" name="HDNselezionato" />
                <input type="hidden" runat="server" id="HDNcomunitaSelezionate" name="HDNcomunitaSelezionate" />
                <input type="hidden" runat="server" id="HDN_filtroFacolta" name="HDN_filtroFacolta" />
                <input type="hidden" runat="server" id="HDN_filtroTipoRicerca" name="HDN_filtroTipoRicerca" />
                <input type="hidden" runat="server" id="HDN_filtroValore" name="HDN_filtroValore" />
                <input type="hidden" runat="server" id="HDN_filtroResponsabileID" name="HDN_filtroResponsabileID" />
                <input type="hidden" runat="server" id="HDN_filtroLaureaID" name="HDN_filtroLaureaID" />
                <input type="hidden" runat="server" id="HDN_filtroTipoCdl" name="HDN_filtroTipoCdl" />
                <input type="hidden" runat="server" id="HDN_filtroAnno" name="HDN_filtroAnno" />
                <input type="hidden" runat="server" id="HDN_filtroPeriodo" name="HDN_filtroPeriodo" />
                <input type="hidden" runat="server" id="HDN_filtroTipoComunitaID" name="HDN_filtroTipoComunitaID" />
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False" HorizontalAlign="Center">
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
                                <asp:LinkButton ID="LNBnascondi" runat="server">Indietro</asp:LinkButton>&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLconferma" runat="server" Visible="False" HorizontalAlign="Center">
                    <table align="center">
                        <tr>
                            <td height="50" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="LBconferma" CssClass="messaggioIscrizione" runat="server">Conferma l'iscrizione alla comunità #nomeComunita# - #nomeResponsabile#</asp:Label>
                                <asp:Label ID="LBconfermaMultipla" CssClass="messaggioIscrizione" runat="server"
                                    Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="50" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center">
                    <asp:Table ID="TBLfiltroNew" runat="server" Width="900px" CellPadding="0" CellSpacing="0"
                        HorizontalAlign="Center">
                        <asp:TableRow ID="TBRchiudiFiltro" Height="22px">
                            <asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign="Center" Width="150px"
                                Height="22px" VerticalAlign="Middle">
                                <asp:LinkButton ID="LNBchiudiFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Chiudi Filtri</asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell CssClass="Filtro_CellDeSelezionato" Width="750px" Height="22px">&nbsp;
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBRapriFiltro" Visible="False" Height="22px">
                            <asp:TableCell ColumnSpan="1" CssClass="Filtro_CellApriFiltro" HorizontalAlign="Center"
                                Width="150px" Height="22px">
                                <asp:LinkButton ID="LNBapriFiltro" runat="server" CssClass="Filtro_Link" CausesValidation="False">Apri Filtri</asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell CssClass="Filtro_Cellnull" Width="750px" Height="22px">&nbsp;
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TBRfiltri">
                            <asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan="2" Width="900px" HorizontalAlign="center">
                                <asp:Table runat="server" ID="TBLfiltro" CellPadding="1" CellSpacing="0" Width="900px"
                                    HorizontalAlign="center">
                                    <asp:TableRow>
                                        <asp:TableCell CssClass="FiltroVoceSmall" ColumnSpan="4">
                                            <table cellspacing="0" border="0" align="left">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LBtipoComunita_c" runat="server" CssClass="FiltroVoceSmall">Tipo Comunità</asp:Label>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDLTipo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="LBtipoRicerca_c" runat="server" CssClass="FiltroVoceSmall">Tipo Ricerca</asp:Label>&nbsp;
                                                        <asp:DropDownList ID="DDLTipoRicerca" runat="server" CssClass="FiltroCampoSmall">
                                                            <asp:ListItem Value="-2" Selected="true">Nome inizia per</asp:ListItem>
                                                            <asp:ListItem Value="-7">Nome contiene</asp:ListItem>
                                                            <asp:ListItem Value="-9">Del responsabile</asp:ListItem>
                                                            <asp:ListItem Value="-3">Creata dopo il</asp:ListItem>
                                                            <asp:ListItem Value="-4">Creata prima del</asp:ListItem>
                                                            <asp:ListItem Value="-5">Data iscrizione dopo il</asp:ListItem>
                                                            <asp:ListItem Value="-6">Data fine iscrizione prima del</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="LBvalore_c" runat="server" CssClass="FiltroVoceSmall" Visible="true">Valore</asp:Label>&nbsp;
                                                        <asp:TextBox ID="TXBValore" runat="server" CssClass="FiltroCampoSmall" MaxLength="100"
                                                            Columns="30"></asp:TextBox>
                                                        <asp:DropDownList ID="DDLresponsabile" runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall"
                                                            Visible="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow runat="server" ID="TBRorgnCorsi">
                                        <asp:TableCell ID="TBCorganizzazione0" ColumnSpan="5">
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:Table ID="TBLcorsi" CellPadding="2" CellSpacing="2" BorderStyle="None" runat="server"
                                                            Visible="False">
                                                            <asp:TableRow>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBannoAccademico_c" runat="server" CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:DropDownList ID="DDLannoAccademico" runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall">
                                                                    </asp:DropDownList>
                                                                </asp:TableCell>
                                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBperiodo_c" runat="server" CssClass="FiltroVoceSmall">Periodo:&nbsp;</asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:DropDownList ID="DDLperiodo" runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall">
                                                                    </asp:DropDownList>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                        <asp:Table ID="TBLcorsiDiStudio" CellPadding="2" CellSpacing="2" BorderStyle="None"
                                                            runat="server" Visible="False">
                                                            <asp:TableRow>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="LBtipoCorsoDiStudi_t" runat="server" CssClass="FiltroVoceSmall">A.A.:&nbsp;</asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:DropDownList ID="DDLtipoCorsoDiStudi" runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall">
                                                                    </asp:DropDownList>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                        <asp:Label ID="LBnoCorsi" runat="server">&nbsp;</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow Height="30px">
                                        <asp:TableCell Height="30px">&nbsp;</asp:TableCell>
                                        <asp:TableCell HorizontalAlign="right" Height="30px" ColumnSpan="4">
                                            <asp:CheckBox ID="CBXautoUpdate" runat="server" AutoPostBack="True" CssClass="FiltroCampoSmall"
                                                Text="Aggiornamento automatico"></asp:CheckBox>&nbsp;&nbsp;
                                            <asp:Button ID="BTNCerca" runat="server" CssClass="PulsanteFiltro" Text="Cerca">
                                            </asp:Button>&nbsp;
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell ColumnSpan="5" Height="10px" CssClass="nosize0">
													&nbsp;
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow Visible="true">
                            <asp:TableCell ColumnSpan="2" Width="900px" HorizontalAlign="Center">
                                <table cellpadding="0" cellspacing="0" align="center" width="900px" border="0">
                                    <tr>
                                        <td>
                                            <table align="left" width="500px">
                                                <tr>
                                                    <td align="center" nowrap="nowrap">
                                                        <asp:LinkButton ID="LKBtutti" runat="server" CssClass="lettera" CommandArgument="-1"
                                                            OnClick="FiltroLink_Click">Tutti</asp:LinkButton>
                                                    </td>
                                                    <td align="center" nowrap="nowrap">
                                                        <asp:LinkButton ID="LKBaltro" runat="server" CssClass="lettera" CommandArgument="0"
                                                            OnClick="FiltroLink_Click">Altro</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBa" runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLink_Click">A</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBb" runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLink_Click">B</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBc" runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLink_Click">C</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBd" runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLink_Click">D</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBe" runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLink_Click">E</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBf" runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLink_Click">F</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBg" runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLink_Click">G</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBh" runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLink_Click">H</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBi" runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLink_Click">I</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBj" runat="server" CssClass="lettera" CommandArgument="10"
                                                            OnClick="FiltroLink_Click">J</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBk" runat="server" CssClass="lettera" CommandArgument="11"
                                                            OnClick="FiltroLink_Click">K</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBl" runat="server" CssClass="lettera" CommandArgument="12"
                                                            OnClick="FiltroLink_Click">L</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBm" runat="server" CssClass="lettera" CommandArgument="13"
                                                            OnClick="FiltroLink_Click">M</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBn" runat="server" CssClass="lettera" CommandArgument="14"
                                                            OnClick="FiltroLink_Click">N</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBo" runat="server" CssClass="lettera" CommandArgument="15"
                                                            OnClick="FiltroLink_Click">O</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBp" runat="server" CssClass="lettera" CommandArgument="16"
                                                            OnClick="FiltroLink_Click">P</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBq" runat="server" CssClass="lettera" CommandArgument="17"
                                                            OnClick="FiltroLink_Click">Q</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBr" runat="server" CssClass="lettera" CommandArgument="18"
                                                            OnClick="FiltroLink_Click">R</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBs" runat="server" CssClass="lettera" CommandArgument="19"
                                                            OnClick="FiltroLink_Click">S</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBt" runat="server" CssClass="lettera" CommandArgument="20"
                                                            OnClick="FiltroLink_Click">T</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBu" runat="server" CssClass="lettera" CommandArgument="21"
                                                            OnClick="FiltroLink_Click">U</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBv" runat="server" CssClass="lettera" CommandArgument="22"
                                                            OnClick="FiltroLink_Click">V</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBw" runat="server" CssClass="lettera" CommandArgument="23"
                                                            OnClick="FiltroLink_Click">W</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBx" runat="server" CssClass="lettera" CommandArgument="24"
                                                            OnClick="FiltroLink_Click">X</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBy" runat="server" CssClass="lettera" CommandArgument="25"
                                                            OnClick="FiltroLink_Click">Y</asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="LKBz" runat="server" CssClass="lettera" CommandArgument="26"
                                                            OnClick="FiltroLink_Click">Z</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <asp:CheckBox CssClass="FiltroCampoSmall" ID="CBXmostraPadre" runat="server" Checked="False"
                                                Text=" Mostra comunità di appartenenza." TextAlign="Right" AutoPostBack="True">
                                            </asp:CheckBox>
                                            <asp:Label ID="LBnumeroRecord_1" runat="server" CssClass="Filtro_TestoPaginazione">N° Record</asp:Label>
                                            <asp:DropDownList ID="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" runat="server"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="15"></asp:ListItem>
                                                <asp:ListItem Value="30" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="45"></asp:ListItem>
                                                <asp:ListItem Value="50"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                <asp:DataGrid ID="DGComunita" runat="server" ShowFooter="false" OnPageIndexChanged="DGComunita_pageindexchanged"
                                    PageSize="30" AutoGenerateColumns="False" AllowPaging="true" DataKeyField="CMNT_ID"
                                    AllowSorting="true" CssClass="DataGrid_Generica">
                                    <AlternatingItemStyle CssClass="ROW_Alternate_Small"></AlternatingItemStyle>
                                    <HeaderStyle CssClass="ROW_header_Small_Center"></HeaderStyle>
                                    <ItemStyle CssClass="ROW_Normal_Small" Height="22px"></ItemStyle>
                                    <PagerStyle CssClass="ROW_Page_Small" Position="TopAndBottom" Mode="NumericPages"
                                        Visible="true" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
                                    </PagerStyle>
                                    <Columns>
                                        <asp:TemplateColumn runat="server" HeaderText="Tipo" ItemStyle-Width="50px" SortExpression="TPCM_Descrizione"
                                            ItemStyle-CssClass="ROW_TD_Small_center">
                                            <ItemTemplate>
                                                <img runat="server" src='<%# DataBinder.Eval(Container.DataItem, "TPCM_icona") %>'
                                                    alt='<%# DataBinder.Eval(Container.DataItem, "TPCM_descrizione") %>' id="Img2" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Nome" SortExpression="CMNT_Nome" HeaderStyle-CssClass="ROW_Header_Small_center"
                                            ItemStyle-CssClass="ROW_TD_Small_center">
                                            <ItemTemplate>
                                                <asp:Table ID="TBLnome" runat="server" HorizontalAlign="Left">
                                                    <asp:TableRow ID="TBRnome" runat="server">
                                                        <asp:TableCell Width="5px">&nbsp;</asp:TableCell>
                                                        <asp:TableCell ID="TBCchiusa" runat="server" CssClass="top">
                                                            <asp:Image ID="IMGisChiusa" runat="server" Visible="true" BorderStyle="None"></asp:Image>
                                                        </asp:TableCell>
                                                        <asp:TableCell ID="TBCnome" runat="server">
                                                            <asp:Label ID="LBcomunitaNome" runat="server">
																		<%# DataBinder.Eval(Container.DataItem, "CMNT_Nome") %>
                                                            </asp:Label>
                                                            (<b><asp:LinkButton ID="LNBiscrivi" runat="server" CommandName="Iscrivi" CausesValidation="False">Iscrivi</asp:LinkButton></b>
                                                            |
                                                            <asp:LinkButton ID="LNBdettagli" runat="server" CommandName="dettagli" CausesValidation="False">Dettagli</asp:LinkButton>
                                                            )
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="AnnoAccademico" HeaderText="A.A." Visible="false" SortExpression="CMNT_Anno"
                                            ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Periodo" HeaderText="Periodo" Visible="false" SortExpression="CMNT_PRDO_descrizione"
                                            ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="Mostra" CommandName="dettagli" HeaderText="Dettagli" ItemStyle-Width="60"
                                            Visible="False" HeaderStyle-CssClass="ROW_Header_Small_center" ItemStyle-CssClass="ROW_TD_Small">
                                        </asp:ButtonColumn>
                                        <asp:BoundColumn DataField="AnagraficaResponsabile" HeaderText="Responsabile" ItemStyle-Width="150"
                                            SortExpression="CMNT_Responsabile" ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="Iscritti" DataField="Iscritti" HeaderStyle-CssClass="ROW_Header_Small_center"
                                            ItemStyle-CssClass="ROW_TD_Small"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_ID" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Anno" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_dataInizioIscrizione" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_dataFineIscrizione" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="ALCM_Path" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Responsabile" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="TPCM_icona" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_EstesoNoSpan" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Iscritti" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_MaxIscritti" Visible="False" ItemStyle-CssClass="ROW_TD_Small_Center">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ALCM_isChiusaForPadre" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_TPCM_id" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_CanSubscribe" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_CanUnsubscribe" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_MaxIscrittiOverList" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Nome" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Archiviata" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CMNT_Bloccata" Visible="false"></asp:BoundColumn>
                                        <asp:TemplateColumn ItemStyle-Width="20px" ItemStyle-CssClass="ROW_TD_Small_center">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="SelectAll2" onclick="SelectAll(this);" runat="server"
                                                    name="SelectAll" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input runat="server" type="checkbox" id="CBcorso" name="CBcorso" onclick="SelectMe(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                                <br />
                                <br />
                                <asp:Label ID="LBmsgDG" runat="server" CssClass="avviso_normal" Visible="False"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
                <input type="hidden" runat="server" id="HDNcmnt_ID" name="HDNcmnt_ID" />
                <input type="hidden" runat="server" id="HDNcmnt_Path" name="HDNcmnt_Path" />
                <input type="hidden" runat="server" id="HDNisChiusaForPadre" name="HDNisChiusaForPadre" />
                <input type="hidden" id="HDNisChiusa" runat="server" name="HDNisChiusa" />
                <asp:Panel ID="PNLdettagli" runat="server" HorizontalAlign="Center" Visible="false">
                    <table width="500" align="center" border="0">
                        <tr>
                            <td align="center" colspan="2">
                                <fieldset>
                                    <legend class="tableLegend">
                                        <asp:Label ID="LBlegenda" runat="server" CssClass="tableLegend">Dettagli comunità</asp:Label>
                                    </legend>
                                    <DETTAGLI:CTRLDettagli ID="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLmessaggi" runat="server" Visible="False">
                    <table cellspacing="0" cellpadding="0" align="center" border="0">
                        <tr>
                            <td height="30">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Label ID="LBMessaggi" CssClass="avviso_normal" runat="server"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLiscrizioneAvvenuta" runat="server" Visible="False" HorizontalAlign="Center">
                    <table cellspacing="0" cellpadding="0" align="left" border="0">
                        <tr>
                            <td height="30">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LBiscrizione" runat="server" CssClass="avvisoIscrizione"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td height="30">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>




<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
<head runat="server">
    <title>Comunità on-Line - Iscrizione</title>

    

    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="./../Styles.css" type="text/css" rel="stylesheet" />
</head>



<body onkeydown="return SubmitRicerca(event);">
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server">
    </asp:ScriptManager>
    <table id="table1" cellspacing="0" cellpadding="0" width="780" border="0" align="center">
        <tr>
            <td colspan="3">
                <HEADER:CtrLHeader ID="Intestazione" runat="server"></HEADER:CtrLHeader>
            </td>
        </tr>
        <tr>
            <td colspan="3">

            </td>
        </tr>
    </table>
    <FOOTER:CtrLFooter ID="CtrLFooter" runat="server"></FOOTER:CtrLFooter>
    </form>
</body>
</html>--%>
