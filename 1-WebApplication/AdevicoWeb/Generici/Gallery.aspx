<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="Gallery.aspx.vb" Inherits="Comunita_OnLine.Gallery"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_Header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="../jscript/generali.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
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
		<br/>
		<asp:Table id="TBLfiltroNew" Runat=server  Width="900px" CellPadding=0 CellSpacing=0>
			<asp:TableRow id="TBRchiudiFiltro" Height=22px>
				<asp:TableCell CssClass="Filtro_CellSelezionato" HorizontalAlign=Center Width=150px Height=22px VerticalAlign=Middle >
					<asp:LinkButton ID="LNBchiudiFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False>Chiudi Filtri</asp:LinkButton>
				</asp:TableCell>
				<asp:TableCell CssClass="Filtro_CellDeSelezionato" Width=750px Height=22px>&nbsp;
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow id="TBRapriFiltro" Visible=False Height=22px>
				<asp:TableCell ColumnSpan=1 CssClass="Filtro_CellApriFiltro" HorizontalAlign=Center Width=150px Height=22px>
					<asp:LinkButton ID="LNBapriFiltro" Runat=server CssClass="Filtro_Link" CausesValidation=False >Apri Filtri</asp:LinkButton>
				</asp:TableCell>
				<asp:TableCell CssClass="Filtro_Cellnull" Width=700px Height=22px>&nbsp;
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TBRfiltri">
				<asp:TableCell CssClass="Filtro_CellFiltri" ColumnSpan=2 Width=900px HorizontalAlign=center>
					<table cellspacing=0 border=0 align=left Width=900px>
						<tr>
							<td height=30px>&nbsp;</td>
							<td height=30px>
								<asp:Label ID="LBtipoRuolo_t" Runat=server CssClass="FiltroVoceSmall">Tipo Ruolo</asp:Label>&nbsp;
								<asp:dropdownlist id="DDLTipoRuolo" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true"></asp:dropdownlist>
							</td>
							<td height=30px width=20px>&nbsp;</td>
							<td height=30px>
								<asp:Label ID="LBtipoRicerca_t" Runat=server CssClass="FiltroVoceSmall">Tipo Ricerca</asp:Label>&nbsp;
								<asp:dropdownlist id="DDLTipoRicerca" CssClass="FiltroCampoSmall" Runat="server" AutoPostBack="false">
									<asp:ListItem  Selected="true" Value=-2>Nome</asp:ListItem>
									<asp:ListItem Value=-3>Cognome</asp:ListItem>
									<asp:ListItem  value = -4>Nome/Cognome</asp:ListItem>
								</asp:dropdownlist>
							</td>
							<td height=30px width=20px>&nbsp;</td>
							<td height=30px>
								<asp:Label ID="LBvalore_t" Runat=server CssClass="FiltroVoceSmall">Valore</asp:Label>&nbsp;
								<asp:textbox id="TXBValore" CssClass="FiltroCampoSmall" Runat="server" MaxLength ="100" Columns=50></asp:textbox>
							</td>
						</tr>
						<tr>
							<td height=30px>&nbsp;</td>
							<td colspan=4>
								<asp:RadioButtonList ID=RBLabilitazione Runat=server  RepeatDirection=Horizontal  CssClass="FiltroCampoSmall" AutoPostBack=True RepeatLayout=Flow >
									<asp:ListItem Selected =True Value=0 >Tutti</asp:ListItem>
									<asp:ListItem value=1>Abilitati</asp:ListItem>
									<asp:ListItem Value=7>Bloccati</asp:ListItem>
									<asp:ListItem value=4>In Attesa di Conferma</asp:ListItem>
								</asp:RadioButtonList>&nbsp;
							</td>
							<td align=right >
								<asp:button id="BTNCerca" CssClass="PulsanteFiltro" Runat="server" Text="Cerca"></asp:button>
							</td>
						</tr>
						<tr>
							<td colspan=6 height=15px>&nbsp;</td>
						</tr>
					</table>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell ColumnSpan=2>
					<table width=100%>
						<tr>
							<td>
								<table align=left >
									<tr>
										<td align="center"><asp:linkbutton id="LKBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" OnClick="FiltroLinkLettere_Click">Tutti</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBaltro" Runat="server" CssClass="lettera" CommandArgument="0" OnClick="FiltroLinkLettere_Click">Altro</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBa" Runat="server" CssClass="lettera" CommandArgument="1" OnClick="FiltroLinkLettere_Click">A</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBb" Runat="server" CssClass="lettera" CommandArgument="2" OnClick="FiltroLinkLettere_Click">B</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBc" Runat="server" CssClass="lettera" CommandArgument="3" OnClick="FiltroLinkLettere_Click">C</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBd" Runat="server" CssClass="lettera" CommandArgument="4" OnClick="FiltroLinkLettere_Click">D</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBe" Runat="server" CssClass="lettera" CommandArgument="5" OnClick="FiltroLinkLettere_Click">E</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBf" Runat="server" CssClass="lettera" CommandArgument="6" OnClick="FiltroLinkLettere_Click">F</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBg" Runat="server" CssClass="lettera" CommandArgument="7" OnClick="FiltroLinkLettere_Click">G</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBh" Runat="server" CssClass="lettera" CommandArgument="8" OnClick="FiltroLinkLettere_Click">H</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBi" Runat="server" CssClass="lettera" CommandArgument="9" OnClick="FiltroLinkLettere_Click">I</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBj" Runat="server" CssClass="lettera" CommandArgument="10" OnClick="FiltroLinkLettere_Click">J</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBk" Runat="server" CssClass="lettera" CommandArgument="11" OnClick="FiltroLinkLettere_Click">K</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBl" Runat="server" CssClass="lettera" CommandArgument="12" OnClick="FiltroLinkLettere_Click">L</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBm" Runat="server" CssClass="lettera" CommandArgument="13" OnClick="FiltroLinkLettere_Click">M</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBn" Runat="server" CssClass="lettera" CommandArgument="14" OnClick="FiltroLinkLettere_Click">N</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBo" Runat="server" CssClass="lettera" CommandArgument="15" OnClick="FiltroLinkLettere_Click">O</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBp" Runat="server" CssClass="lettera" CommandArgument="16" OnClick="FiltroLinkLettere_Click">P</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBq" Runat="server" CssClass="lettera" CommandArgument="17" OnClick="FiltroLinkLettere_Click">Q</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBr" Runat="server" CssClass="lettera" CommandArgument="18" OnClick="FiltroLinkLettere_Click">R</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBs" Runat="server" CssClass="lettera" CommandArgument="19" OnClick="FiltroLinkLettere_Click">S</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBt" Runat="server" CssClass="lettera" CommandArgument="20" OnClick="FiltroLinkLettere_Click">T</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBu" Runat="server" CssClass="lettera" CommandArgument="21" OnClick="FiltroLinkLettere_Click">U</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBv" Runat="server" CssClass="lettera" CommandArgument="22" OnClick="FiltroLinkLettere_Click">V</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBw" Runat="server" CssClass="lettera" CommandArgument="23" OnClick="FiltroLinkLettere_Click">W</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBx" Runat="server" CssClass="lettera" CommandArgument="24" OnClick="FiltroLinkLettere_Click">X</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBy" Runat="server" CssClass="lettera" CommandArgument="25" OnClick="FiltroLinkLettere_Click">Y</asp:linkbutton></td>
										<td align="center"><asp:linkbutton id="LKBz" Runat="server" CssClass="lettera" CommandArgument="26" OnClick="FiltroLinkLettere_Click">Z</asp:linkbutton></td>
									</tr>
								</table>
							</td>
							<td align=right>
								<asp:CheckBox ID="CBXwithFoto" Runat=server CssClass="FiltroCampoSmall" AutoPostBack=True Text="Con foto"></asp:CheckBox>&nbsp;
								<asp:Label ID="LBnumRecord_t" Runat=server CssClass="Filtro_TestoPaginazione">N° Record</asp:Label>
								<asp:dropdownlist id="DDLNumeroRecord" CssClass="Filtro_RecordPaginazione" Runat="server" AutoPostBack="true">
									<asp:ListItem Value="20" Selected="true"></asp:ListItem>
									<asp:ListItem Value="40"></asp:ListItem>
									<asp:ListItem Value="80"></asp:ListItem>
								</asp:dropdownlist>
							</td>
						</tr>
					</table>
										
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<table align=center border=0>
			<tr>
				<td align=center>
					<asp:Table Runat=server ID="TBLgallery" HorizontalAlign=Center Width="900px" BorderColor=Navy GridLines=None>
					</asp:Table>
										
					<asp:DataGrid 
						ID="DGgallery" 
						Runat="server"
						ShowHeader="False"
						PageSize="24"
						GridLines="None"
						ShowFooter="False"
						AllowPaging="True"
						AutoGenerateColumns="False"
						AllowCustomPaging="True"
						CssClass="DataGrid_Generica">
						<PagerStyle CssClass="ROW_Page_Small" Position=bottom Mode="NumericPages" Visible="true"
							HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom"></PagerStyle>
							<Columns>
								<asp:BoundColumn DataField="PRSN_ID" Visible=False ></asp:BoundColumn>
							</Columns>
					</asp:DataGrid>
				</td>
			</tr>
		</table>
	</asp:Panel>
</asp:Content>



<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head runat="server">
    <title>Comunità On Line - Gallery</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
    <LINK href="../Styles.css" type="text/css" rel="stylesheet"/>

  </head>
  <body >

     <form id="aspnetForm" method="post" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		<table class="contenitore" cellSpacing="0" cellPadding="0" align="center">
				<tr>
					<td colSpan="3"><HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER></td>
				</tr>
				<tr>
					<td class="RigaTitolo" align=left >
						<asp:Label ID="LBTitolo" Runat="server">Gallery utenti</asp:Label>
					</td>
				</tr>
				<tr>
					<td align="left" colSpan="3">

					</td>
				</tr>
				<tr>
					<td colSpan="3"></td>
				</tr>
			</table>
			<FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>
  </body>
</html>--%>
