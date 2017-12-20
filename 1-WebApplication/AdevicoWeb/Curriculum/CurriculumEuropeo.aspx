<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" Codebehind="CurriculumEuropeo.aspx.vb" Inherits="Comunita_OnLine.CurriculumEuropeo"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="DATI" TagName="CTRLdati" Src="./UC_datiCurriculum.ascx" %>
<%@ Register TagPrefix="LINGUA" TagName="CTRLlingua" Src="./UC_ConoscenzaLingua.ascx" %>
<%@ Register TagPrefix="LAVORO" TagName="CTRLlavoro" Src="./UC_EsperienzeLavorative.ascx" %>
<%@ Register TagPrefix="FORMAZIONE" TagName="CTRLformazione" Src="./UC_formazione.ascx" %>
<%@ Register TagPrefix="COMPETENZE" TagName="CTRLcompetenze" Src="./UC_Competenze.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">@import url(./../Jscript/Calendar/calendar-blue.css);</style>
    <script type=text/javascript src="./../Jscript/Calendar/calendar.js"></script>
    <script type=text/javascript src="./../Jscript/Calendar/calendar-setup.js"></script>
    <LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>

    <%=CalendarScript() %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align:right;" align="center">
        <asp:Panel ID="PNLmenu" Runat=server HorizontalAlign=Right Visible=False >
	        <asp:ImageButton ID="IMBword" Runat=server ImageUrl ="../images/ico/doc_G.gif" Visible=False  ></asp:ImageButton>
	        <asp:linkbutton id="LKBinserisciFormazione" Visible="False" Runat="server" Text="Inserisci Nuova Esperienza Formativa" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBinserisciLingua" Visible="False" Runat="server" Text="Aggiungi Nuova" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBinserisciEsperienza" Visible="False" Runat="server" Text="Aggiungi Nuova"  CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBaggiungiCurriculum" Visible="False" Runat="server" Text="Aggiungi Curriculum" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBmodificaCurriculum" Visible="False" Runat="server" Text="Salva modifiche" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBaggiungiCompetenza" Visible="False" Runat="server" Text="Aggiungi Nuova" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBmodificaCompetenza" Visible="False" Runat="server" Text="Salva modifiche" CssClass=Link_Menu></asp:linkbutton>
										
	        <asp:linkbutton id="LKBannullaFormazione" Visible="False" Runat="server" Text="Annulla" CausesValidation=False  CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBaggiungiFormazione" Visible="False" Runat="server" Text="Aggiungi Nuova" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBmodificaFormazione" Visible="False" Runat="server" Text="Salva modifiche" CssClass=Link_Menu></asp:linkbutton>
										
	        <asp:linkbutton id="LKBannullaLingua" Visible="False" Runat="server" Text="Annulla" CausesValidation=False  CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBaggiungiLingua" Visible="False" Runat="server" Text="Aggiungi Nuova" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBmodificaLingua" Visible="False" Runat="server" Text="Salva modifiche" CssClass=Link_Menu></asp:linkbutton>
										
	        <asp:linkbutton id="LKBannullaEsperienza" Visible="False" Runat="server" Text="Annulla" CausesValidation=False  CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBaggiungiEsperienza" Visible="False" Runat="server" Text="Aggiungi Nuova" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBmodificaEsperienza" Visible="False" Runat="server" Text="Salva modifiche" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBesportaPDF" Visible="True" Runat="server" Text="Esporta PDF" CssClass=Link_Menu></asp:linkbutton>
	        <asp:linkbutton id="LKBesportaRTF" Visible="True" Runat="server" Text="Esporta RTF" CssClass=Link_Menu></asp:linkbutton>
        </asp:Panel>
    </div>

    <div align="left" style="width: 900px;  padding-top:5px; ">
    	<asp:Panel ID="PNLpermessi" Runat="server" Visible="False">
			<table align="center">
				<tr>
					<td height="50">&nbsp;</td>
				</tr>
				<tr>
					<td align="center">
						<asp:Label id="LBNopermessi" Runat="server" CssClass="NoPermessi"></asp:Label>
					</td>
				</tr>
			</table>
		</asp:Panel>
		<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center">
			<asp:Table ID="TBLdati" Runat="server" Width=800px>
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
					<asp:TableCell ID="TableCell1" Runat="server">
						<DATI:CTRLdati id="CTRLdati" runat="server" Width="800px"></DATI:CTRLdati>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat="server" ID="TBRcompetenze" Visible="False">
					<asp:TableCell>
						<COMPETENZE:CTRLcompetenze id="CTRLcompetenze" runat="server" Width="800px"></COMPETENZE:CTRLcompetenze>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat="server" ID="TBRformazione" Visible="False">
					<asp:TableCell>
						<FORMAZIONE:CTRLformazione id="CTRLformazione" runat="server" Width="800px"></FORMAZIONE:CTRLformazione>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat="server" ID="TBRlingua" Visible="False">
					<asp:TableCell>
						<LINGUA:CTRLlingua id="CTRLlingua" runat="server" Width="800px"></LINGUA:CTRLlingua>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow Runat="server" ID="TBResperienze" Visible="False">
					<asp:TableCell>
						<LAVORO:CTRLlavoro id="CTRLlavoro" runat="server" Width="800px"></LAVORO:CTRLlavoro>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</asp:Panel>
    </div>

</asp:Content>


