
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" Codebehind="AdminG_ManagementServizi.aspx.vb" Inherits="Comunita_OnLine.AdminG_ManagementServizi"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>


<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content1">
	<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	<table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td class="RigaTitoloAdmin" align="left">
				<asp:Label ID="LBtitolo" Runat="server">Gestione Servizi</asp:Label>
			</td>
		</tr>--%>
		<tr>
			<td align=right >
				<asp:Panel Runat=server ID="PNLinserisci" HorizontalAlign=right>
					<asp:linkbutton id="LNBmenu" Runat="server" text="Gestione Menu" CausesValidation=False CssClass=Link_Menu></asp:linkbutton>&nbsp;
					<asp:linkbutton id="LNBinserisci" Runat="server" text="Inserisci" CausesValidation=False CssClass=Link_Menu></asp:linkbutton>
				</asp:Panel>
			</td>
		</tr>
		<tr>
			<td align="center" valign="top">
				<br/>
				<asp:Panel ID="PNLpermessi" Runat="server" Visible="False" HorizontalAlign=Center>
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label>
							</td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:Panel>
					<asp:Panel ID="PNLcontenuto" Runat=server HorizontalAlign=Center>
						<asp:datagrid 
							id="DGServizi" Runat="server" 
							ShowHeader="true" AllowSorting="true" 
							CssClass="table light fullwidth"
							AutoGenerateColumns="False"  UseAccessibleHeader="true"
							DataKeyField="SRVZ_ID" AllowPaging="false">
							<Columns>
                                <asp:TemplateColumn HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IMBinfo" runat="server" CausesValidation="false" ImageUrl="../images/proprieta.gif"  Visible=True></asp:ImageButton>
													<asp:ImageButton ID="IMBmodifica" runat="server" Commandname ="modifica" CausesValidation=False ImageUrl="../images/m.gif" Visible=True></asp:ImageButton>
													<asp:ImageButton ID="IMBelimina" runat="server" CommandName="elimina" CausesValidation=False ImageUrl="../images/x_d.gif" Visible=True></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Name">
									<ItemTemplate>
                                        <div>
                                            <asp:Label ID="LBnome" Runat=server><%# DataBinder.Eval(Container.DataItem, "SRVZ_nome") %></asp:Label>
                                        </div>
                                        <div id="DVdescription" runat="server">
                                            <asp:Label ID="LBdescrizione" Runat=server><%# DataBinder.Eval(Container.DataItem, "SRVZ_Descrizione") %></asp:Label>
                                        </div>
                                        <div>
                                            <asp:LinkButton ID="LNBattiva" Runat="server" CausesValidation="False" CommandName=attiva>Attiva</asp:LinkButton>
													<asp:LinkButton ID="LNBdisattiva" Runat="server" CausesValidation="False" CommandName=disattiva>Disattiva</asp:LinkButton>
													&nbsp;|&nbsp;
													<asp:LinkButton ID="LNBassociaPermessi" Runat="server" CausesValidation="False" CommandName=permessi>Associa Permessi</asp:LinkButton>
													&nbsp;|&nbsp;
													<asp:LinkButton ID="LNBassociaTipiComunita" Runat="server" CausesValidation="False" CommandName=tipocomunita>Associa tipi comunità</asp:LinkButton>
													&nbsp;|&nbsp;
													<asp:LinkButton ID="LNBdefinisciRuoli" Runat="server" CausesValidation="False" CommandName=ruoli>Definisci ruoli/permessi</asp:LinkButton>
                                        </div>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="SRVZ_nonDisattivabile" Visible=False ></asp:BoundColumn>
								<asp:BoundColumn DataField="SRVZ_Attivato" HeaderText="" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="SRVZ_ID" HeaderText="" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="SRVZ_Associato" HeaderText="" Visible="False"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
						<br/>
						<asp:Label ID="LBnoRecord" Runat=server CssClass="avviso11"></asp:Label>
						<input type=hidden id="HIDcheckbox" runat=server name="HIDcheckbox"/>

					</asp:Panel>
				</td>
			</tr>
		</table>	
</asp:Content>



<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
	<title>Amministrazione Servizi - Comunità On-Line</title>
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	<meta name=vs_defaultClientScript content="JavaScript"/>
	<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
	<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>

</head>
<body >
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
		<table cellSpacing="0" cellPadding="0" width="900px" align="center" border="0">
			<tr>
				<td colSpan="3">
					<HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
				</td>
			</tr>
			<tr>
				<td colSpan="3">

					</td>
				</tr>
			</table>
			<FOOTER:CTRLFOOTER id="CTRLFooter" runat="server"></FOOTER:CTRLFOOTER>
		</form>
	</body>
</html>--%>
