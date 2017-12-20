<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ListaDistribuzione.aspx.vb" Inherits="Comunita_OnLine.ListaDistribuzione" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register Src="UC_MailingList/UC_MailingList.ascx" TagName="UC_MailingList" TagPrefix="uc1" %>
<%--
<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="./../uc/footer2.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="./../dhtmlcentral.css" rel="STYLESHEET" type="text/css" />
    <link href="./../dhtmlcentral.css" rel="STYLESHEET" type="text/css" />
    
    <style type="text/css">
        /*FireFox*/
        .IndirizzoMail {
            display:block;
            float:left;
            min-width:415px;
        }
        /*Internet Explorer*/
        * html .IndirizzoMail {
            display:block;
            float:left;
            width:415px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
   <table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">

		<tr class="contenitore">
			<td colspan="3">
				<table cellspacing="0" cellpadding="0" width="900" border="0">
<%--					<tr>
						<td class="RigaTitolo" align="left">
							<asp:Label ID="LBTitolo" Runat="server">Servizio Mailing List</asp:Label>
								
						</td>
					</tr>--%>
					<tr>
						<td align="center">
						<br /><br />
							<asp:Panel ID="PNLpermessi" Runat="server" Visible="false" HorizontalAlign="Center">
								    <table align="center">
										<tr>
											<td height="50">&nbsp;</td>
										</tr><tr>
											<td align="center">
												<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio">Non si dispone dei permessi necessari a visualizzare la pagina.</asp:Label>
											</td>
										</tr><tr>
											<td height="50">&nbsp;</td>
										</tr>
									</table>
								</asp:Panel>
							<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" width="900px">
		                        <table cellpadding="5px" cellspacing="0px">
		                            <tr>
		                                <td style="width: 62px" align="center">
		                                    <asp:label id="LBfrom" runat="server" text="Da:" CssClass="titolo_campoSmall"></asp:label>
		                                </td><td>
		                                    <asp:textbox id="TXBFrom" runat="server" ReadOnly="true" Width="450" Columns="60" CssClass="Testo_Mail"></asp:textbox>
		                                </td>
		                            </tr><tr>
		                                <td class="BgLavender" style="width: 62px;" align="center">
		                                    <asp:label id="LBa" runat="server" text="A:" CssClass="titolo_campoSmall"></asp:label>  
		                                </td><td class="BgLavender" style="padding:5px">
		                                    <asp:Label ID="LBL_DestMail" CssClass="testo_campoSmall" runat="server">E' possibile selezionare...</asp:Label>
		                                    <br />
		                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Width="850px" Height="100px" BorderColor="darkBlue" BorderWidth="1px" BackColor="white">
		                                        <asp:CheckBoxList ID="CBL_Liste" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Width="100%" class="testo_campoSmall"></asp:CheckBoxList>
                                            </asp:Panel>
                                            <br />
                                                <asp:Label ID="LBL_NoListe" runat="server" class="testo_campoSmall">Nessuna lista presente.</asp:Label>
                                                
                                            <table cellpadding="0" cellspacing="0" width="100%">
		                                        <tr>
		                                            <td>
		                                                <asp:CheckBox ID="Cbx_ShowNominativi" runat="server" AutoPostBack="true" Text="Visualizza Destinatari" class="testo_campoSmall"/>
		                                                <asp:Label ID="LblErrore_Dest" CssClass="errore" Runat="server" Visible="false"></asp:Label>    
		                                            </td><td align="right">
		                                                <asp:Button ID="Btn_ModificaLista" runat="server" Text="Modifica lista" CssClass="PulsanteFiltro" CausesValidation="false" />         
		                                            </td>
		                                        </tr>
		                                    </table>
		                                    <asp:Label ID="LBL_Nominativi" runat="server" CssClass="titolo_campoSmall"></asp:Label>
                                                
		                                    <asp:Panel ID="PNL_Destinatari" runat="server" ScrollBars="Vertical" Width="850px" Height="100px" BorderColor="darkBlue" BorderWidth="1px" BackColor="white">
                                                <div style="display: block; width:830px;" class="TestoCampoSmall">
                                                    <asp:Repeater ID="RPT_Address" runat="server">
                                                        <ItemTemplate>
                                                            <div style="display: inline;">
                                                                <div class="IndirizzoMail">
                                                                    <div style="display:inline;" class="titolo_campoSmall">
                                                                        <%# DataBinder.Eval(Container.DataItem, "DisplayName") %>
                                                                    </div> - <div style="display:inline;" class="testo_campoSmall">
                                                                        <%# DataBinder.Eval(Container.DataItem, "Address") %>    
                                                                    </div>
                                                                </div> 
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </asp:Panel>
                                            <br />
                                            <asp:Button id="Btn_AggiornaDestinatari" runat="server" CssClass="PulsanteFiltro" Text="Aggiorna" CausesValidation="false"></asp:Button>
                                        </td>
		                            </tr>
		                                
		                            <tr>
		                                <td style="width: 62px" align="center">
		                                    <asp:label id="LBobj" runat="server" text="Oggetto:" CssClass="titolo_campoSmall"></asp:label>
		                                </td><td>
		                                    <%--<asp:Label id="LBerrore" Runat="server" CssClass="errore"></asp:Label>--%>
											<asp:textbox id="TXBObj" runat="server" MaxLength="100" Columns="97" Width="830" CssClass="Testo_campo_obbligatorio"></asp:textbox>
		                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		                                    <asp:RequiredFieldValidator ID="RFV_Subject" runat="server" ControlToValidate="TXBObj" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            &nbsp;
                                        </td>
		                            </tr><tr>
		                                <td style="width: 62px" align="center">
		                                    <asp:label id="LBattach" runat="server" text="Allegati:" CssClass="titolo_campoSmall"></asp:label>
		                                </td><td>
		                                    <input id="fileAllega" type="file" size="110" name="fileAllega" runat="server" class="PulsanteFiltro"/>
		                                        
                                            <asp:button id="BTallega" Runat="server" Text="Allega" CssClass="PulsanteFiltro" CausesValidation="False"></asp:button>
                                            <br />
                                            <asp:RegularExpressionValidator ID="REVvalid" runat="server" ControlToValidate="fileAllega" ErrorMessage="Errore" ValidationExpression="^\b" />
		                                </td>
		                            </tr><tr>
		                                <td style="width: 62px">&nbsp;</td>
		                                <td>
		                                    <input id="TBFrom_att" type="hidden" name="TBFrom_att" runat="server"/>
												<asp:table id="TBLattach" Runat="server" BorderWidth="2" Width="850px"></asp:table>
			                                    <asp:Button ID="BTN_DelAllAttach" runat="server" Text="Clear" CssClass="PulsanteFiltro" />														
												<input id="TXBnascosto" type="hidden" name="TXBnascosto" runat="server"/> 
			                                <br />
			                                <asp:Label ID="LBLErroreAttach" CssClass="errore" Runat="server" ></asp:Label>										
		                                </td>
		                            </tr><tr>
		                                <td valign="top" style="width: 62px" align="center">
		                                    <asp:label id="LBbody" runat="server" text="Testo:" CssClass="titolo_campoSmall"></asp:label>
		                                </td><td>
		                                    <asp:textbox id="TXBbody" runat="server" rows="15" textmode="multiline" Width="850px" Columns="88" CssClass="Testo_Mail"></asp:textbox>
		                                </td>
		                            </tr><tr>
		                                <td style="width: 62px">
		                                    &nbsp;
		                                </td><td>
		                                    <asp:CheckBox id="CBXcopiamittente" Runat="server" Text="Invia copia a mittente" Checked="true"></asp:CheckBox>
		                                    <asp:CheckBox id="CBXricezione" Runat="server" Text="Conferma Ricezione"></asp:CheckBox>     
		                                </td>
		                            </tr><tr>
		                                <td style="width: 62px">
		                                    &nbsp;
		                                </td><td align="right">
		                                    <input id="BTNinvia" class="PulsanteFiltro" type="button" value="Invia" name="invia" runat="server" width="80" onserverclick="SendMail" causesvalidation="true" />
		                                </td>
		                            </tr>
		                        </table>
		                            			
								<asp:Panel id="PNLrubrica" Runat="server" width="530px" Visible="False" >
			                        Controllo per gestione Liste -Personali-
			                    </asp:Panel>							
		                    </asp:Panel>
		                    <asp:panel id="PNLinviato" Runat="server" Visible="False" HorizontalAlign="Center">
		                        <br />       
		                        <asp:Label ID="Lbl_InvioOK_t" runat="server" CssClass="messaggio"></asp:Label>
		                        <br /> <br />      
		                        <asp:Button ID="BTN_invioOk_Back" runat="server" Text="Ok" CssClass="PulsanteFiltro" />
		                        <br /> 
		                    </asp:panel>
		                    <asp:panel id="PNLerrore" Runat="server" Visible="False" HorizontalAlign="Center">
		                        <br />       
		                        <asp:Label ID="Lbl_InvioErr_t" runat="server" CssClass="messaggio"></asp:Label>
		                        <br /> <br />       
		                        <asp:Button ID="BTN_invioErr_Back" runat="server" Text="Ok" CssClass="PulsanteFiltro" />     
		                        <br />  
		                    </asp:panel>
		                    <asp:Panel ID="PNLGestioneListe" runat="server">
                                <uc1:UC_MailingList ID="UC_MailingList1" runat="server" />
		                        
		                    </asp:Panel>
					    </td>
					</tr>
				</table>
	    	</td>
		</tr>
		<tr>
			<td colspan="3"></td>
		</tr>
	</table>
</asp:Content>

<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Comunità On Line - Gestione Liste Mail</title>
    
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	<meta name="vs_defaultClientScript" content="JavaScript"/>
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	//<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
	
    <link href="./../Styles.css" type="text/css" rel="stylesheet"/>

    

</head>

<body>
    <form id="aspnetForm" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>

    			<tr>
				    <td colspan="3" >
				        <div>
				            <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>	
				        </div>
				    </td>
			    </tr>




    </form>
</body>
</html>--%>