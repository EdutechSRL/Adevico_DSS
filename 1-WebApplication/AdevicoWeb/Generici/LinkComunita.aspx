<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="LinkComunita.aspx.vb" Inherits="Comunita_OnLine.LinkComunita"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="radTree" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLcreaLink" Src="./UC_Link/UC_CreaLink.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdettagliLink" Src="./UC_Link/UC_DettagliLink.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLesportaLink" Src="./UC_Link/UC_EsportaLink.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLimportaLink" Src="./UC_Link/UC_ImportaLink.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLgestioneLink" Src="./UC_Link/UC_GestioneLink.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="../jscript/generali.js" type="text/javascript"></script>
    
    <style type="text/css">
		td{
		font-size: 11px;
		}
	</style>

    <script language="javascript" type="text/javascript">
        function ProcessClientDrop(sourceNode, destNode) {
            return confirm('Confermare ?');
        }

//        function ExpandAll() {
//            var i;
//            for (i = 0; i < RDTraccoltaLink.AllNodes.length; i++) {
//                var node = RDTraccoltaLink.AllNodes[i];
//                if (node.Nodes.length > 0) {
//                    node.Expand();
//                }
//            }
//        }

//        function CollapseAll() {
//            var i;
//            for (i = 0; i < RDTraccoltaLink.AllNodes.length; i++) {
//                var node = RDTraccoltaLink.AllNodes[i];
//                if (node.Nodes.length > 0) {
//                    node.Collapse();
//                }
//            }
//        }

      	function ExpandAll() {
      	    var i;
      	    //for (i = 0; i < RDTraccoltaLink.AllNodes.length; i++) {
      	    for (i = 0; i < <%=RDTraccoltaLink.ClientId%>.AllNodes.length; i++) {
      	        var node = <%=RDTraccoltaLink.ClientId%>.AllNodes[i];
                //var node = RDTraccoltaLink.AllNodes[i];
      	        if (node.Nodes.length > 0) {
      	            node.Expand();
      	        }
      	    }
      	}

      	function CollapseAll() {
      	    var i;
      	    //for (i = 0; i < RDTraccoltaLink.AllNodes.length; i++) {
            for (i = 0; i < <%=RDTraccoltaLink.ClientId%>.AllNodes.length; i++) {
                //var node = RDTraccoltaLink.AllNodes[i];
      	        var node = <%=RDTraccoltaLink.ClientId%>.AllNodes[i];
      	        if (node.Nodes.length > 0) {
      	            node.Collapse();
      	        }
      	    }
      	}


        function UpdateAllChildren(nodes, checked) {
            var i;
            for (i = 0; i < nodes.length; i++) {
                if (checked)
                    nodes[i].Check()
                else
                    nodes[i].UnCheck()
                if (nodes[i].Nodes.length > 0)
                    UpdateAllChildren(nodes[i].Nodes, checked);
            }
        }

        function CheckChildNodes(node) {
            if (!node.Checked && node.Parent != null) {
                node.Parent.UnCheck();
            }
            UpdateAllChildren(node.Nodes, node.Checked);
        }
				
			
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">

		<table class="contenitore" cellSpacing="0" cellPadding="0" align="center">
<%--            <tr>
				<td class="RigaTitolo" align="left" colSpan="2">
					<asp:Label ID="LBTitolo" Runat="server">Raccolta Link</asp:Label>
				</td>
			</tr>--%>
			<tr>
				<td colSpan="2" align=right width=900px>
					<asp:Panel ID="PNLmenu" Runat="server" HorizontalAlign=Right width=900px>
						<table width=100% cellpadding=0 cellspacing=0 border=0>
							<tr>
								<td>
									<asp:LinkButton ID="LNBaggiorna" Runat=server CssClass="Link_Menu"></asp:LinkButton>
									<asp:LinkButton ID="LNBespandi" Runat=server CssClass="Link_Menu">Espandi albero</asp:LinkButton>
									<asp:LinkButton ID="LNBcomprimi" Runat=server CssClass="Link_Menu">Comprimi albero</asp:LinkButton>
								</td>
								<td align=right >
									<asp:LinkButton ID="LNBnuovoFolderLink" Runat=server CssClass="Link_Menu">Crea cartella</asp:LinkButton>
									<asp:LinkButton ID="LNBnuovoLink" Runat=server CssClass="Link_Menu">Crea Link</asp:LinkButton>							
								</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:Panel ID="PNLmenugestione" Runat=server Visible=False HorizontalAlign=Right width=900px>
						<asp:LinkButton ID="LNBindietro" Runat=server CssClass="Link_Menu" CausesValidation=False >Indietro</asp:LinkButton>
						<asp:LinkButton ID="LNBinserisci" Runat=server CssClass="Link_Menu">Salva Dati</asp:LinkButton>
						<asp:LinkButton ID="LNBesporta" Runat=server Visible=False CssClass="Link_Menu">Esporta Link selezionati</asp:LinkButton>
						<asp:LinkButton ID="LNBimporta" Runat=server Visible=False CssClass="Link_Menu">Importa Link selezionati</asp:LinkButton>
					</asp:Panel>
					<asp:Panel ID="PNLmanagement" Runat=server Visible=False HorizontalAlign=Right width=900px>
						<table align=right width=900px cellpadding=0 cellspacing=0 border=0>
							<tr>
								<td valign=middle >
									<asp:Label ID="LBfolder_t" Runat=server CssClass="FiltroVoceSmall10">Cartella:</asp:Label>
									<asp:LinkButton ID="LNBdettagliDir" Runat=server CausesValidation=False CssClass="Link_Menu" Visible=False>Dettagli</asp:LinkButton>
									<asp:LinkButton ID="LNBmodificaDir" Runat=server CssClass="Link_Menu" Visible=False >Modifica</asp:LinkButton>
									<asp:LinkButton ID="LNBeliminaDir" Runat=server CausesValidation=False Visible=False CssClass="Link_Menu">Cancella</asp:LinkButton>
									<asp:LinkButton ID="LNBeliminaContenuto" Runat=server CausesValidation=False CssClass="Link_Menu">Cancella contenuto</asp:LinkButton>		
								</td>
								<td align=right >
									&nbsp;
									<asp:LinkButton ID="LNBcreaDirectory" Runat=server CausesValidation=False CssClass="Link_Menu">Crea Cartella Link</asp:LinkButton>
									<asp:LinkButton ID="LNBcreaLink" Runat=server CausesValidation=False CssClass="Link_Menu">Crea Link</asp:LinkButton>								
								</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:Panel ID="PNLmenuLink" Runat=server Visible=False HorizontalAlign=Right width=900px>
						<asp:LinkButton ID="LNBdettagli" Runat=server CausesValidation=False CssClass="Link_Menu" Visible=False>Dettagli</asp:LinkButton>
						<asp:LinkButton ID="LNBmodifica" Runat=server CssClass="Link_Menu">Modifica</asp:LinkButton>
						<asp:LinkButton ID="LNBeliminaLink" Runat=server CausesValidation=False CssClass="Link_Menu">Cancella Link</asp:LinkButton>
					</asp:Panel>
					<asp:Panel ID="PNLconfermaCancella"  Runat=server Visible=False HorizontalAlign=Right width=900px>
						<asp:LinkButton ID="LNBannullaElimina" Runat=server CausesValidation=False CssClass="Link_Menu">Indietro</asp:LinkButton>
						<asp:LinkButton ID="LNBelimina" Runat=server CausesValidation=False CssClass="Link_Menu" >Elimina SOLO dalla comunità</asp:LinkButton>
						<asp:LinkButton ID="LNBreplicaElimina" Runat=server CausesValidation=False CssClass="Link_Menu">Replica sui personali</asp:LinkButton>
					</asp:Panel>
				</td>
			</tr>
			<tr>
				<td align="left" colSpan="3">
					<br/>
					<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign="Center">
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
					</asp:Panel>
					<asp:panel id="PNLcontenuto" Visible="true" Runat="server" HorizontalAlign=Center>
						<table width=900px align=center >
							<tr>
								<td>
                                     <telerik:radtabstrip id="TBSmenu" runat="server" align="Justify" Width="650px" Height="26px" SelectedIndex="0"
                                      causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                                        <tabs>
                                            <telerik:RadTab text="Raccolta Personale" value="TABlista" runat="server"></telerik:RadTab>
                                            <telerik:RadTab text="Gestione Personali" value="TABgestione" runat="server" ></telerik:RadTab>
                                            <telerik:RadTab text="Importa" value="TABimporta" runat="server" ></telerik:RadTab>
                                            <telerik:RadTab text="Esporta" value="TABesporta" runat="server" ></telerik:RadTab>
                                        </tabs>
                                    </telerik:radtabstrip>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Table ID="TBLlistaLink" Runat=server HorizontalAlign=left GridLines=none  >
										<asp:TableRow>
											<asp:TableCell CssClass="top">
												<radTree:RadTreeView id="RDTraccoltaLink" runat="server" align="left" causesValidation="False" autopostback=true
												CssFile="./../styles.css" XPStyle=true PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js" skin="BookMarkList" ImagesBaseDir="~/RadControls/TreeView/Skins/BookMarkList/">
											</radTree:RadTreeView>
											</asp:TableCell>
											<asp:TableCell Width=40px>&nbsp;</asp:TableCell>
											<asp:TableCell CssClass="top">
												<CTRL:CTRLdettagliLink id="CTRLdettagliLink" runat="server" visible=false></CTRL:CTRLdettagliLink>
												&nbsp;
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<asp:Table ID="TBLgestione" Runat=server Visible=false  HorizontalAlign=Center  BorderColor=000099 GridLines=Both CellSpacing=0>
										<asp:TableRow>
											<asp:TableCell HorizontalAlign=Center >
												<CTRL:CTRLgestioneLink id="CTRLgestioneLink" runat="server"></CTRL:CTRLgestioneLink>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<asp:Table  ID="TBLimporta" Runat=server Visible=false  HorizontalAlign=Center >
										<asp:TableRow>
											<asp:TableCell>
												<CTRL:CTRLimportaLink id="CTRLimportaLink" runat="server"></CTRL:CTRLimportaLink>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<asp:Table ID="TBLesporta" Runat=server Visible=false  HorizontalAlign=Center>
										<asp:TableRow  >
											<asp:TableCell>
												<CTRL:CTRLesportaLink id="CTRLesportaLink" runat="server"></CTRL:CTRLesportaLink>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
								</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:Table ID="TBLcreaLink" Runat=server Visible=false  HorizontalAlign=Center>
						<asp:TableRow>
							<asp:TableCell HorizontalAlign=Center >
								<CTRL:CTRLcreaLink id="CTRLcreaLink" runat="server"></CTRL:CTRLcreaLink>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</td>
			</tr>
		</table>
</asp:Content>



<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>Link</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
	  <LINK href="../Styles.css" type="text/css" rel="stylesheet"/>    

  </head>

  <body >

     <form id="aspnetForm" method="post" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
     		<tr>
				<td colSpan="2"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
			</tr>
		<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>
  </body>
</html>--%>