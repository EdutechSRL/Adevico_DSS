<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InfoCurriculum.aspx.vb" Inherits="Comunita_OnLine.InfoCurriculum"%>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="DATI" TagName="CTRLdati" Src="./UC_infodatiCurriculum.ascx" %>
<%@ Register TagPrefix="LINGUA" TagName="CTRLlingua" Src="./UC_infoConoscenzaLingua.ascx" %>
<%@ Register TagPrefix="LAVORO" TagName="CTRLlavoro" Src="./UC_infoEsperienzeLavorative.ascx" %>
<%@ Register TagPrefix="FORMAZIONE" TagName="CTRLformazione" Src="./UC_infoformazione.ascx" %>
<%@ Register TagPrefix="COMPETENZE" TagName="CTRLcompetenze" Src="./UC_infoCompetenze.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head runat="server">
		<title>Comunità On Line - Info Curriculum</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
	</head>
	<body id="popup">
		<form id="aspnetForm" method="post" runat="server"> 
		<asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<table class="contenitore" cellSpacing="0" cellPadding="0" align="center">
				<tr>
					<td align=right >
						<asp:LinkButton ID="LNBclose" Runat=server CausesValidation=False CssClass="Link_Menu">Chiudi (x)</asp:LinkButton>
					</td>
				</tr>
				<tr>
					<td align="left">
						<input type=hidden id="HDN_Studente_ID" runat=server/>
						<asp:Panel ID="PNLcurriculum" runat="server" Visible="False" HorizontalAlign=Left >
							<asp:Table ID="TBLdati" Runat="server" Width=100% HorizontalAlign=Left>
								<asp:TableRow>
									<asp:TableCell CssClass=RigaTab>
										 <telerik:radtabstrip id="TBSmenu" runat="server" align="Justify" Width="100%" Height="26px" SelectedIndex="0"
                                            causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                                            <tabs>
                                                <telerik:RadTab text="Dati" value="TABdati" runat="server"></telerik:RadTab>
                                                <telerik:RadTab text="Competenze" value="TABcompetenze" runat="server" ></telerik:RadTab>
                                                <telerik:RadTab text="Istruzione" value="TABformazione" runat="server" ></telerik:RadTab>
                                                <telerik:RadTab text="Lingue" value="TABlingua" runat="server" ></telerik:RadTab>
                                                <telerik:RadTab text="Esperienze Lavorative" value="TABesperienze" runat="server" ></telerik:RadTab>
                                            </tabs>
                                        </telerik:radtabstrip>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Runat="server" ID="TBRdati">
									<asp:TableCell >
										<DATI:CTRLdati id="CTRLdati" runat="server" ></DATI:CTRLdati>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Runat="server" ID="TBRcompetenze" Visible="False">
									<asp:TableCell>
										<COMPETENZE:CTRLcompetenze id="CTRLcompetenze" runat="server" Width="550px"></COMPETENZE:CTRLcompetenze>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Runat="server" ID="TBRformazione" Visible="False">
									<asp:TableCell>
										<FORMAZIONE:CTRLformazione id="CTRLformazione" runat="server" Width="550px"></FORMAZIONE:CTRLformazione>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Runat="server" ID="TBRlingua" Visible="False">
									<asp:TableCell>
										<LINGUA:CTRLlingua id="CTRLlingua" runat="server" Width="550px"></LINGUA:CTRLlingua>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Runat="server" ID="TBResperienze" Visible="False">
									<asp:TableCell>
										<LAVORO:CTRLlavoro id="CTRLlavoro" runat="server" Width="550px"></LAVORO:CTRLlavoro>
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
