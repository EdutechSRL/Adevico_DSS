<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardModificaComunita.aspx.vb" Inherits="Comunita_OnLine.WizardModificaComunita" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdatiComunita" Src="./UC_WizardComunita/UC_Fase1DatiComunita.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLresponsabile" Src="./UC_WizardComunita/UC_Fase2Responsabile.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLvisualizzaPadri" Src="./UC_WizardModificaComunita/UC_Fase3VisualizzaComunitaPadri.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaggiungiPadri" Src="./UC_WizardModificaComunita/UC_Fase3AggiungiComunitaPadri.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLservizi" Src="./UC_WizardModificaComunita/UC_Fase4modificaServizi.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLfinale" Src="./UC_WizardModificaComunita/UC_Fase5sceltaDefault.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtags" Src="~/Modules/Tag/UC/UC_TagsSelectorForCommunityHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <CTRL:CTRLtags id="CTRLtagsHeader" runat="server"></CTRL:CTRLtags>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <input type="hidden" id="HDN_ComunitaAttualeID" runat="server" name="HDN_ComunitaAttualeID" />
    <input type="hidden" id="HDNazione" value="gestioneTipo" runat="server" name="HDNazione" />
    <asp:Table ID="TBLprincipale" runat="server" CellPadding="0" GridLines="None" Width="900px"
        CellSpacing="0">
        <%--		<asp:TableRow>
			<asp:TableCell HorizontalAlign=Left CssClass="RigaTitolo">
				<asp:Label ID="LBTitolo" Runat="server">Creazione comunità</asp:Label>
			</asp:TableCell>
		</asp:TableRow>--%>
        <asp:TableRow ID="TBRmenu">
            <asp:TableCell HorizontalAlign="Right">
                &nbsp;<asp:LinkButton ID="LNBindietro" runat="server" Text="Torna all'elenco" CssClass="Link_Menu"
                    CausesValidation="False"></asp:LinkButton>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell CssClass="top">
                <asp:Panel ID="PNLpermessi" runat="server" Visible="False">
                    <br />
                    <table align="center">
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="50">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLnavigazione2" runat="server" HorizontalAlign="Right" Width="100%"
                    BorderWidth="1">
                    <table cellspacing="0" cellpadding="0" border="0" align="right">
                        <tr>
                            <td>
                                <asp:Button ID="BTNelenco2" runat="server" Text="Torno all'elenco" CssClass="PulsanteFiltro"
                                    CausesValidation="False"></asp:Button>
                                <asp:Button ID="BTNsalva2" runat="server" Text="Salva" CssClass="PulsanteFiltro"
                                    CausesValidation="true" Visible="False"></asp:Button>
                                <asp:Button ID="BTNaggiungiPadre2" runat="server" Text="Aggiungi altra" CssClass="PulsanteFiltro"
                                    CausesValidation="true" Visible="False"></asp:Button>
                            </td>
                            <td width="35">
                                &nbsp;
                            </td>
                            <td width="100" nowrap="nowrap">
                                <asp:Button ID="BTNindietro2" runat="server" CssClass="PulsanteFiltro" Text="< Indietro"
                                    CausesValidation="False"></asp:Button>
                            </td>
                            <td width="5">
                                &nbsp;
                            </td>
                            <td width="100" nowrap="nowrap">
                                <asp:Button ID="BTNavanti2" runat="server" CssClass="PulsanteFiltro" Text="Avanti >"
                                    CausesValidation="True"></asp:Button>
                            </td>
                            <td width="20">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center" Width="900px"
                    BorderWidth="1">
                    <asp:Table runat="server" ID="TBLinserimento" CellPadding="0" CellSpacing="0" Width="900px"
                        Height="450px">
                        <asp:TableRow>
                            <asp:TableCell>&nbsp;</asp:TableCell>
                            <asp:TableCell HorizontalAlign="left" CssClass="top">
                                <asp:Table HorizontalAlign="left" runat="server" ID="TBLdati" Width="800px" Visible="true"
                                    GridLines="none">
                                    <asp:TableRow HorizontalAlign="center">
                                        <asp:TableCell Width="20px">&nbsp;</asp:TableCell>
                                        <asp:TableCell>
                                            <CTRL:CTRLdatiComunita ID="CTRLdati" runat="server"></CTRL:CTRLdatiComunita>
                                        </asp:TableCell>
                                        <asp:TableCell Width="15px">&nbsp;</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table HorizontalAlign="center" runat="server" ID="TBLresponsabile" Width="800px"
                                    Visible="False" GridLines="none">
                                    <asp:TableRow>
                                        <asp:TableCell Width="20px">&nbsp;</asp:TableCell>
                                        <asp:TableCell>
                                            <CTRL:CTRLresponsabile ID="CTRLresponsabile" runat="server"></CTRL:CTRLresponsabile>
                                        </asp:TableCell>
                                        <asp:TableCell Width="15px">&nbsp;</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table HorizontalAlign="center" runat="server" ID="TBLpadri" Width="800px" Visible="False"
                                    GridLines="none">
                                    <asp:TableRow>
                                        <asp:TableCell Width="20px">&nbsp;</asp:TableCell>
                                        <asp:TableCell>
                                            <br />
                                            <CTRL:CTRLvisualizzaPadri ID="CTRLvisualizzaPadri" runat="server"></CTRL:CTRLvisualizzaPadri>
                                        </asp:TableCell>
                                        <asp:TableCell Width="15px">&nbsp;</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table HorizontalAlign="center" runat="server" ID="TBLaggiungiPadri" Width="800px"
                                    Visible="False" GridLines="none">
                                    <asp:TableRow>
                                        <asp:TableCell Width="20px">&nbsp;</asp:TableCell>
                                        <asp:TableCell>
                                            <CTRL:CTRLaggiungiPadri ID="CTRLaggiungiPadri" runat="server"></CTRL:CTRLaggiungiPadri>
                                        </asp:TableCell>
                                        <asp:TableCell Width="15px">&nbsp;</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table HorizontalAlign="center" runat="server" ID="TBLservizi" Width="800px"
                                    Visible="False" GridLines="none">
                                    <asp:TableRow>
                                        <asp:TableCell Width="20px">&nbsp;</asp:TableCell>
                                        <asp:TableCell>
                                            <CTRL:CTRLservizi ID="CTRLservizi" runat="server"></CTRL:CTRLservizi>
                                        </asp:TableCell>
                                        <asp:TableCell Width="15px">&nbsp;</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table HorizontalAlign="center" runat="server" ID="TBLfinale" Width="800px" Visible="False"
                                    GridLines="none">
                                    <asp:TableRow>
                                        <asp:TableCell Width="20px">&nbsp;</asp:TableCell>
                                        <asp:TableCell>
                                            <br />
                                            <br />
                                            <br />
                                            <CTRL:CTRLfinale ID="CTRLfinale" runat="server"></CTRL:CTRLfinale>
                                        </asp:TableCell>
                                        <asp:TableCell Width="15px">&nbsp;</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                            <asp:TableCell Width="5px">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Panel ID="PNLnavigazione" runat="server" HorizontalAlign="Right" Width="100%"
                    BorderWidth="1">
                    <table cellspacing="0" cellpadding="0" border="0" align="right">
                        <tr>
                            <td>
                                <asp:Button ID="BTNelenco" runat="server" Text="Annulla" CssClass="PulsanteFiltro"
                                    CausesValidation="False"></asp:Button>
                                <asp:Button ID="BTNsalva" runat="server" Text="Salva" CssClass="PulsanteFiltro" CausesValidation="true"
                                    Visible="False"></asp:Button>
                                <asp:Button ID="BTNaggiungiPadre" runat="server" Text="Aggiungi altra" CssClass="PulsanteFiltro"
                                    CausesValidation="true" Visible="False"></asp:Button>
                            </td>
                            <td width="35">
                                &nbsp;
                            </td>
                            <td width="100" nowrap="nowrap">
                                <asp:Button ID="BTNindietro" runat="server" CssClass="PulsanteFiltro" Text="< Indietro"
                                    CausesValidation="False"></asp:Button>
                            </td>
                            <td width="5">
                                &nbsp;
                            </td>
                            <td width="100" nowrap="nowrap">
                                <asp:Button ID="BTNavanti" runat="server" CssClass="PulsanteFiltro" Text="Avanti >"
                                    CausesValidation="True"></asp:Button>
                            </td>
                            <td width="20">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<%--

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
	<META HTTP-EQUIV="Pragma" CONTENT="no-cache"/>
    <title>Comunità On Line - Wizard modifica dati comunità</title>
	<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
	
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
  </head>
   <script language="javascript" type="text/javascript">
       function SubmitRicerca(event) {
        var numCharCode;
     var elTarget;
     var strType;

		  // get event if not passed
     if (!e) var e = event;
   // get character code of key pressed
    if (e.keyCode) numCharCode = e.keyCode;
     else if (e.which) numCharCode = e.which;
    // get target
      if (e.target) elTarget = e.target;
      else if (e.srcElement) elTarget = e.srcElement;
                                                 
      // if form input field
      if ( elTarget.tagName.toLowerCase() == 'input' )
         {
   
         // get type
         strType = elTarget.getAttribute('type').toLowerCase();
   
         // based on type
         switch ( strType )
            {
            case 'checkbox' :
            case 'radio' :
            case 'text' :
   
               // if this is a return
               if ( numCharCode == 13 )
                  {
                  // cancel event to prevent form submission
                  return false;
                  }
   
               break;
               
            }
   
         }
   
      // process default action
      return true;
//				 if (document.all){
//					if (event.keyCode == 13){
//						event.returnValue=false;
//						event.cancel = true;
//						return false;
//						}
//					}
//				else if (document.getElementById){
//					if (event.which == 13){
//						event.returnValue=false;
//						event.cancel = true;
//						return false;
//						}
//					}
//				else if(document.layers){
//					if(event.which == 13){
//						event.returnValue=false;
//						event.cancel = true;
//							return false;
//						}
//					}
//				else
//					return true;
			}
		</script>
<body onkeydown="return SubmitRicerca(event);">
    <form id="aspnetForm" method="post" runat="server" defaultfocus="LBTitolo">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
	<table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
				<tr>
				    <td colspan="3" >
				    <div>
				        <HEADER:CtrLHEADER id="Intestazione" runat="server" ShowNews="false"></HEADER:CtrLHEADER>	
				    </div>
				    </td>
			    </tr>
			<tr class="contenitore">
				<td colSpan="3">

				</td>
			</tr>
		</table>
		<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>
  </body>
</html>--%>