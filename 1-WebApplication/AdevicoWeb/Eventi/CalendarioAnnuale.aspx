<%@ Page Language="vb" validateRequest="false" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="CalendarioAnnuale.aspx.vb" Inherits="Comunita_OnLine.CalendarioAnnuale"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CORPO2" TagName="CtrLLegenda" Src="Legenda.ascx" %>
<%--
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
		<script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
		<STYLE type="text/css">#cache { Z-INDEX: 10; VISIBILITY: visible; POSITION: absolute; TOP: 200px }
		</STYLE>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">

	<DIV id="cache" align="center">
		<TABLE align="center" style="BORDER-RIGHT: #ff0000 1px dashed; BORDER-TOP: #ff0000 1px dashed; BORDER-LEFT: #ff0000 1px dashed; BORDER-BOTTOM: #ff0000 1px dashed"
			cellSpacing="0" cellPadding="0" width="400" bgColor="#000000" border="0">
			<TR>
				<TD vAlign="middle" align="center">
					<TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#ffff00" border="0">
						<TR align="center">
							<TD vAlign="middle" align="center"><FONT color="#000000" size="3"><br/>
									<b>Attendere...caricamento in corso</b><br/>
									<br/>
								</FONT>
							</TD>
						</TR>
					</TABLE>
				</TD>
			</TR>
		</TABLE>
	</DIV>


	<script language="JavaScript" type="text/javascript">
//		var nava = (document.layers);
//		var dom = (document.getElementById);
//		var iex = (document.all);
//		if (nava) { cach = document.cache }
//		else if (dom) { cach = document.getElementById("cache").style }
//		else if (iex) { cach = cache.style }
	    largeur = screen.width;
	    cach = cache; //add
		cach.left = Math.round((largeur / 2) - 250);
		cach.visibility = "visible";


	</script>



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
					<asp:table ID="TBLmese" Runat="server" Width=780px>
						<asp:TableRow HorizontalAlign="Center">
							<asp:TableCell HorizontalAlign="Center">
								<table width="100%">
									<tr>
										<td align="left"><asp:LinkButton ID="LKBAnnoPrec" Font-Name="tahoma" Font-Size="12px" Runat="server"></asp:LinkButton></td>
										<td align="center"><asp:Label ID="LBLAnnoCorrente" Font-Name="tahoma" Font-Size="16px" Font-Bold="True" Runat="server"></asp:Label></td>
										<td align="right"><asp:LinkButton ID="LKBAnnoSuc" Font-Name="tahoma" Font-Size="12px" Runat="server"></asp:LinkButton></td>
									</tr>
								</table>
								<br/>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow Runat="server" ID="Tablerow1" HorizontalAlign="Center">
							<asp:TableCell>
								<table align="center" bgcolor="#e5ede7" border="1">
									<tr style="border-color:#ffffff">
										<asp:Repeater ID="RPTgiorniSettimana" Runat="server">
											<ItemTemplate>
												<td align="center" height="20px" runat="server" id="CellaTemp" width="20px" bordercolor="#e5c2a7" bgcolor="#ffffff"><font size="1"><%#Container.DataItem("giornoSettimana")%></font></td>
											</ItemTemplate>
										</asp:Repeater>
									</tr>
									<tr bgcolor="#e5ede7" bordercolor="#e5ede7">
										<td colspan="43">&nbsp;</td>
									</tr>
									<asp:Repeater ID="RPTMesi" Runat="server">
										<ItemTemplate>
											<tr valign="bottom" style="border-color:White">
												<td align="center" bordercolor="#e5c2a7" bgcolor="#ffffff" id="cellaSelectSett" runat="server"
													valign="bottom" width="30px">
													<asp:LinkButton ID="LKBMese" Runat=server CommandArgument = <%#Container.DataItem("PrimoGiorno")%>>
														<%#Container.DataItem("Mese")%>
													</asp:LinkButton>
												</td>
												<asp:Repeater ID="RPTgiorni" Runat="server" OnItemCreated="RPTgiorni_ItemCreated">
													<ItemTemplate>
														<td id="CellaColore" runat="server" align="center" valign="middle" height="15px" width="15px"
															bordercolor="#e5c2a7" bgcolor="#ffffff">
															<asp:LinkButton ID="LKB_temp" OnClick="ClickGiorno" CssClass="Eventi_LinkButtonAnnuale" Runat="server"
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
						<asp:TableRow BackColor="#dfeae2">
							<asp:TableCell HorizontalAlign="Center">
								<Table style="border-color:#ffffff" border="2" CellSpacing="4">
									<tr>
										<td align="center" height="25px">
											<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLVisualizzazione">passa alla visualizzazione</asp:Label>
											<br/>
											&nbsp;|&nbsp;<asp:linkbutton ID="LKBgoTOsettimanale" Runat="server" CssClass="Eventi_Label">settimanale</asp:linkbutton>
											&nbsp;|&nbsp;<asp:linkbutton ID="LKBgoTOmensile" Runat="server" CssClass="Eventi_Label">mensile</asp:linkbutton>&nbsp;|&nbsp;
										</td>
										<td align="center"height="25px">
											<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBFiltroComunita" Visible="False">FILTRO COMUNITA'</asp:Label><br/>
											<asp:radiobuttonlist id="RBLFiltroComunita" Runat="server" CssClass="Eventi_Label" Visible="False" RepeatLayout="Flow"
												Repeatdirection="Horizontal" Autopostback="true">
												<asp:ListItem Value="-1">tutte</asp:ListItem>
												<asp:ListItem Value="0" Selected="True">corrente</asp:ListItem>
											</asp:radiobuttonlist>
										</td>
										<td align="center" height="25px">
											<asp:Label Runat="server" CssClass="Eventi_Label" ID="LBLcambiaAnno">cambia anno:</asp:Label><br/>
											<asp:DropDownList ID="DDLVaiA_anni" Runat="server" AutoPostBack="True" CssClass="Eventi_Label"></asp:DropDownList>
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

    <script type="text/javascript">
        //windows.onload = cacheOff;

        <%= Me.BodyId() %>.onload = cacheOff(0);
        
        function cacheOff(num) {
            cache.style.display = "none";
        }
//    	function cacheOff(text) {
//		    alert(text);
//		    //cache.visibility = "hidden";
//		    //cache.style.display = "none"
//		}
//        <%= Me.BodyId() %>.onload = cacheOff("Fired")
    </script>
    <div style="visibility:hidden"
</asp:Content>


