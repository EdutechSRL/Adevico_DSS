<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DatiCover.ascx.vb" Inherits="Comunita_OnLine.UC_DatiCover" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtesto" Src="./UC_Testo_Stile.ascx" %>

<style>

hr {
    margin:0em;
	clear:both;
	visibility:hidden;
} 
</style>
<div class="CoverTable">
    <div>
	   <div class="CoverTitleCellEdit">
		  <asp:Label ID="LBattivata" Runat=server CssClass="Titolo_CampoSmall">Attivata:</asp:Label>
	   </div>
	    <div class="CoverDatiCellEdit">
		  <asp:CheckBox id="CBXattivata" Runat=server Text="Si" CssClass="Testo_campoSmall"></asp:CheckBox>
	   </div>
	   <hr />
    </div>
    <div>
	  <div class="CoverTitleCellEdit">
		  <asp:Label ID="LBtitolo" Runat=server CssClass="Titolo_CampoSmall">Titolo:</asp:Label>
	   </div>
	    <div class="CoverDatiCellEdit">
		  <CTRL:CTRLtesto ID="CTRLtitolo" MaxLength="300" TextRows="3" TextColumns="65" isRequired="true" runat="server"/>
	   </div>
	   <hr />
    </div>
    <div>
	   <div class="CoverTitleCellEdit">
		  <asp:Label ID="LBanno" Runat=server CssClass="Titolo_CampoSmall">Anno:</asp:Label>
	   </div>
	   <div class="CoverDatiCellEdit">
		   <CTRL:CTRLtesto ID="CTRLanno" MaxLength="200" TextRows="1" TextColumns="65" isRequired="false" runat="server"/>
	   </div>
	   <hr />
    </div>
     <div>
	   <div class="CoverTitleCellEdit">
		  <asp:Label ID="LBcommento" Runat=server CssClass="Titolo_CampoSmall">Commento:</asp:Label>
	   </div>
	   <div class="CoverDatiCellEdit">
		  <CTRL:CTRLtesto ID="CTRLcommento" MaxLength="5000" TextRows="3" TextColumns="65" isRequired="false" runat="server"/>
	   </div>
	   <hr />
    </div>
    <div>
	   <div class="CoverTitleCellEdit">
		  <asp:Label ID="LBpaginaDefault_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Servizio default</asp:Label>
	   </div>
	    <div class="CoverDatiCellEdit">
		  <asp:DropDownList ID="DDLpagineDefault" Runat=server cssClass="Testo_campoSmall"></asp:DropDownList>
		  <asp:Label ID="LBpaginaDefault" Runat=server CssClass="Titolo_campoSmall">&nbsp;Servizio visualizzato di default al click dell'utente sul logo della comunità.</asp:Label>
	   </div>
	   <hr />
    </div>
    <div>
	  <div class="CoverTitleCellEdit">
		  <asp:label ID="LBlogo_t" Runat=server CssClass="Titolo_campoSmall">&nbsp;Logo:</asp:Label>
	   </div>
	   <div class="CoverDatiCellEdit">
		  <div>
			 <div>
				<asp:Label ID="LBavviso" Runat=server CssClass ="avviso_normal">L'immagine del Logo verrà visulizzata con un'altezza
				max di 300px. Per migliorare la velocità di navigazione si consigliano file jpeg.</asp:Label>
			 </div>
			 <div>
			    <asp:Image ID="IMGlogo" Runat=server  BorderStyle=None></asp:Image>
			 </div>
			 <div id="DVnewLogo" runat="server">
				<input type=file id="FILElogo" runat=server Class="Testo_campoSmall" size=65 NAME="FILElogo"/>
			 </div>
			 <div id="DVerroreLogo" runat="server">
				<asp:Label ID="LBerrore" Runat=server CssClass ="avviso_normal" Visible=true ></asp:Label>
				&nbsp;&nbsp;
				<asp:Button ID="BTNeliminaLogo" Runat=server CausesValidation=False CssClass="Pulsante_Menu" Text="Elimina"></asp:Button>
			 </div>
		  </div>
	   </div>
	   <hr />
    </div>
     <div>
	 <div class="CoverTitleCellEdit">
		  <asp:label ID="LBdidascalia" Runat=server CssClass="Titolo_campoSmall">&nbsp;Didascalia:</asp:Label>
	   </div>
	  <div class="CoverDatiCellEdit">
		   <CTRL:CTRLtesto ID="CTRLdidascalia" MaxLength="200" TextRows="2" TextColumns="65" isRequired="false" runat="server" ShowStile="false"/>
	   </div>
	   <hr />
     </div>
</div>