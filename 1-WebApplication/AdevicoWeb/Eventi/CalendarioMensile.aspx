<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="CalendarioMensile.aspx.vb" Inherits="Comunita_OnLine.CalendarioMensile"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CORPO2" TagName="CtrLLegenda" Src="Legenda.ascx" %>
<%--
--%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--		<tr>
			<td Class="RigaTitolo" align=left>
				<asp:label id="LBtitolo" Runat="server">Calendario Annuale</asp:label>
			</td>
		</tr>--%>
		<tr>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>
				<asp:panel id="PNLpermessi" Runat="server" Visible="False">
					<table align="center">
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
						<tr>
							<td align="center">
								<asp:Label id="LBnopermessi" CssClass="messaggio" ForeColor="Blue" Runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td height="50">&nbsp;</td>
						</tr>
					</table>
				</asp:panel>
				<asp:panel id="PNLcontenuto" Runat="server" HorizontalAlign="Center">
					<asp:table ID="TBLmese" Runat="server">
						<asp:TableRow HorizontalAlign="Center">
							<asp:TableCell HorizontalAlign="Center">
								<table width="100%">
									<tr>
										<td align="left"><asp:LinkButton ID="LKBmesePrec" Font-Name="tahoma" Font-Size="12px" Runat="server"></asp:LinkButton></td>
										<td align="center"><asp:Label ID="LBLMeseCorrente" Font-Name="tahoma" Font-Size="16px" Font-Bold="True" Runat="server"></asp:Label></td>
										<td align="right"><asp:LinkButton ID="LKBmeseSuc" Font-Name="tahoma" Font-Size="12px" Runat="server"></asp:LinkButton></td>
									</tr>
								</table>
								<br/>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow Runat="server" ID="Tablerow1" HorizontalAlign="Center">
							<asp:TableCell>
								<table align="center" bgcolor="#e5ede7" border="1">
									<tr bordercolor="#ffffff">
										<asp:Repeater ID="RPTgiorniSettimana" Runat="server">
											<ItemTemplate>
												<td align="center" id="CellaTemp" runat="server" bordercolor="#e5ede7" bgcolor="#ffffff">&nbsp;<%#Container.DataItem("giornoSettimana")%></td>
											</ItemTemplate>
										</asp:Repeater>
									</tr>
									<tr bgcolor="#e5ede7" bordercolor="#e5ede7">
										<td colspan="8">&nbsp;</td>
									</tr>
									<asp:Repeater ID="RPTsettimane" Runat="server">
										<ItemTemplate>
											<tr valign="bottom" bordercolor="White">
												<td align="center" id="cellaSelectSett" runat="server" valign="bottom" width="30px"
													bordercolor="#ffffff" bgcolor="#e5ede7">
													<asp:LinkButton ID="LKBSettimana" Runat=server CssClass="Eventi_Label" CommandArgument = <%#Container.DataItem("PrimoGiorno")%>> >> </asp:LinkButton>
												</td>
												<asp:Repeater ID="RPTgiorni" Runat="server" OnItemCreated="RPTgiorni_ItemCreated">
													<ItemTemplate>
														<td id="CellaColore" runat="server" class="Eventi_cellaMensile" bordercolor="#e5c2a7"
															bgcolor="#ffffff">
															<asp:LinkButton ID="LKB_temp" OnClick="ClickGiorno" CssClass="Eventi_Linkbutton" Runat="server"
															Visible="False" CommandArgument=<%#Container.DataItem("id")%>>
																<%#Container.DataItem("giorno")%>
															</asp:LinkButton>
														</td>
													</ItemTemplate>
												</asp:Repeater>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</table>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow BackColor="#dfeae2" BorderColor="#ffffff" BorderWidth="1">
							<asp:TableCell HorizontalAlign="Right" BackColor="#dfeae2" BorderColor="#ffffff">
								<Table BorderColor="#ffffff" border="2" CellSpacing="4">
									<tr>
										<td align="center" height="25px">
											<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLVisualizzazione">passa alla visualizzazione</asp:Label>
											<br/>
											&nbsp;|&nbsp;<asp:linkbutton ID="LKBgoTOsettimanale" Runat="server" CssClass="Eventi_Label">settimanale</asp:linkbutton>
											&nbsp;|&nbsp;<asp:linkbutton ID="LKBgoTOannuale" Runat="server" CssClass="Eventi_Label">annuale</asp:linkbutton>&nbsp;|&nbsp;
										</td>
										<td height="25px">
											<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBFiltroComunita" Visible="False">FILTRO COMUNITA'</asp:Label><br/>
											<asp:radiobuttonlist id="RBLFiltroComunita" Runat="server" CssClass="Eventi_Label" Visible="False" RepeatLayout="Flow"
												Repeatdirection="Horizontal" Autopostback="true">
												<asp:ListItem Value="-1">tutte</asp:ListItem>
												<asp:ListItem Value="0" Selected="True">corrente</asp:ListItem>
											</asp:radiobuttonlist>
										</td>
										<td height="25px">
											<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLcambiaMese">cambia mese</asp:Label>
											<br/>
											<asp:DropDownList ID="DDLVaiA_mesi" Runat="server" CssClass="Eventi_Label"></asp:DropDownList>
											&nbsp;&nbsp;<asp:DropDownList ID="DDLVaiA_anni" Runat="server" CssClass="Eventi_Label"></asp:DropDownList>
											&nbsp;&nbsp;<asp:Button ID="BTNVai" Runat="server" CssClass="PulsantePiccolo" Text="visualizza"></asp:Button>
										</td>
										<td height="25px">
											<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLEventiVisual">eventi da visualizzare:</asp:Label>
											<br/>
											<asp:CheckBox ID="CBEventiTutti" AutoPostBack="True" Checked="True" Runat="server" CssClass="Eventi_Label"
												Text="tutti"></asp:CheckBox>
											&nbsp;&nbsp;&nbsp;<asp:Button ID="BTNfiltroEventi" Runat="server" Text="eventi..." CssClass="PulsantePiccolo"></asp:Button>
										</td>
									</tr>
									<tr>
										<td colspan="3">
											<asp:CheckBoxList ID="CBXLFiltroEventi" Runat="server" CssClass="Eventi_Label" RepeatDirection="Horizontal"
												RepeatLayout="Flow" Visible="False"></asp:CheckBoxList>
										</td>
										<td>
											<asp:Button ID="BTNApplicaFiltroEventi" Runat="server" Text="applica" CssClass="PulsantePiccolo"
												Visible="False"></asp:Button>
										</td>
									</tr>
								</Table>
							</asp:TableCell>
						</asp:TableRow>
					</asp:table>
				</asp:panel>
			</td>
		</tr>
	</table>
</asp:Content>

