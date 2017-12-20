<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainStatisticheServizi.aspx.vb" Inherits="Comunita_OnLine.MainStatisticheServizi"  MasterPageFile="~/AjaxPortal.Master"%>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
        <div id="DVcontenitore" align="center">
   	        <div id="DVtitle" style="width: 900px; text-align:left;" class="RigaTitolo" align="center">
		        &nbsp;<asp:Label ID="LBTitolo" Runat="server">Scorm - Statistiche</asp:Label><br />
	        </div>
	        <div id="DVmenu" style="width: 900px; text-align:right;" align="center">
	            <asp:linkbutton id="LNB_GENERIC" Runat="server" Text="Tasto generico" CausesValidation="false" CssClass="LINK_MENU" Visible="false"></asp:linkbutton>

		    </div>
		    <br />
		    <div id="DVfiltri" align="left" style="width: 900px;">
		        <asp:Label ID="Lbl_Service_t" runat="server" CssClass="Titolo_campoSmall">Servizio</asp:Label>:
		        &nbsp;
		        <asp:DropDownList ID="DDL_Service" runat="server" CssClass="Testo_campoSmall" AutoPostBack="true">
		            <asp:ListItem Value=2>File</asp:ListItem>
		            <asp:ListItem Selected=True Value=1>Scorm</asp:ListItem>
		        </asp:DropDownList>
<%--		        &nbsp;&nbsp;&nbsp;
		        <asp:Label ID="Lbl_NomeComunita_t" runat="server" CssClass="Titolo_campoSmall">Comunità</asp:Label>:
		        &nbsp;
		        <asp:DropDownList ID="DDL_Comunita" runat="server" CssClass="Testo_campoSmall">
		            <asp:ListItem>Corso controllo qualità</asp:ListItem>
		            <asp:ListItem>Management Elle3</asp:ListItem>
		            <asp:ListItem>Altro...</asp:ListItem>
		            <asp:ListItem>+ Tutte +</asp:ListItem>
		        </asp:DropDownList>
		        <asp:Label ID="Lbl_NomeComunita" runat="server" CssClass="Testo_campoSmall">Corso controllo qualità</asp:Label>--%>
		        &nbsp;
		        <asp:LinkButton ID="Lnb_Cerca" Runat="server" Text="Tasto generico" CausesValidation="false" CssClass="LINK_MENU" Visible="false"></asp:LinkButton>
		    </div>
		    
		    
		    <div style="width: 900px; text-align:center; margin:0,auto; padding-top:5px; clear:both;" align="center">
                <telerik:RadTabStrip ID="TBSstat" runat="server" Align="Justify" Width="100%" Height="20px"
	                CausesValidation="false" AutoPostBack="true"  Skin="Outlook" EnableEmbeddedSkins="true">
	                <Tabs>
		                <telerik:RadTab Text="Personali" Value="1"></telerik:RadTab>
		                <telerik:RadTab Text="Nella comunità corrente" Value="0"></telerik:RadTab>
	                </Tabs>
                </telerik:RadTabStrip>
            </div>	
            
            <div align="left" style="width: 900px;  padding-top:5px; ">
		        <asp:PlaceHolder ID="PHStat" runat="server"></asp:PlaceHolder>
		    </div>
</div>
</asp:Content>