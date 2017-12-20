<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CTRLgridTop.ascx.vb" Inherits="Comunita_OnLine.CTRLgridTop" %>

<div class="parteSopra">
  <div class="pagingSopra" style="border-left: 1px #333333 solid;">
    <asp:Label runat="server" ID="LBpageRecord"></asp:Label>
  </div>
  <div class="pagingSopra">
    <asp:ImageButton CssClass="elementoGiu01" runat="server" ID="IMBfirst" ImageUrl="img/primo.gif" />&nbsp;
    <asp:ImageButton CssClass="elementoGiu01" runat="server" ID="IMBprev" ImageUrl="img/indietro.gif" />&nbsp;&nbsp;
    <asp:linkbutton id="LNB1" Runat="server" CssClass="selected" CommandArgument="1">1</asp:linkbutton>
    <asp:linkbutton id="LNB2" Runat="server" CommandArgument="2">2</asp:linkbutton>
    <asp:linkbutton id="LNB3" Runat="server" CommandArgument="3">3</asp:linkbutton>
    <asp:linkbutton id="LNB4" Runat="server" CommandArgument="4">4</asp:linkbutton>
    <asp:linkbutton id="LNB5" Runat="server" CommandArgument="5">5</asp:linkbutton>
    <asp:linkbutton id="LNB6" Runat="server" CommandArgument="6">6</asp:linkbutton>
    <asp:linkbutton id="LNB7" Runat="server" CommandArgument="7">7</asp:linkbutton>
    <asp:linkbutton id="LNB8" Runat="server" CommandArgument="8">8</asp:linkbutton>
    <asp:linkbutton id="LNB9" Runat="server" CommandArgument="9">9</asp:linkbutton>
    <asp:ImageButton runat="server" ID="IMBnext" CssClass="elementoGiu01" ImageUrl="img/avanti.gif" />&nbsp;
    <asp:ImageButton runat="server" ID="IMBlast" CssClass="elementoGiu01" ImageUrl="img/ultimo.gif"/>
  </div>
  <div class="sfondoPagingSopra">
    &nbsp;
  </div>
  <div class="cercaVeloce">
	<asp:linkbutton id="LNBtutti" Runat="server" CssClass="lettera" CommandArgument="-1" >Tutti</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBa" Runat="server" CssClass="lettera" CommandArgument="1" >A</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBb" Runat="server" CssClass="lettera" CommandArgument="2" >B</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBc" Runat="server" CssClass="lettera" CommandArgument="3" >C</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBd" Runat="server" CssClass="lettera" CommandArgument="4" >D</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBe" Runat="server" CssClass="lettera" CommandArgument="5" >E</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBf" Runat="server" CssClass="lettera" CommandArgument="6" >F</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBg" Runat="server" CssClass="lettera" CommandArgument="7" >G</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBh" Runat="server" CssClass="lettera" CommandArgument="8" >H</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBi" Runat="server" CssClass="lettera" CommandArgument="9" >I</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBj" Runat="server" CssClass="lettera" CommandArgument="10" >J</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBk" Runat="server" CssClass="lettera" CommandArgument="11" >K</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBl" Runat="server" CssClass="lettera" CommandArgument="12" >L</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBm" Runat="server" CssClass="lettera" CommandArgument="13" >M</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBn" Runat="server" CssClass="lettera" CommandArgument="14" >N</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBo" Runat="server" CssClass="lettera" CommandArgument="15" >O</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBp" Runat="server" CssClass="lettera" CommandArgument="16" >P</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBq" Runat="server" CssClass="lettera" CommandArgument="17" >Q</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBr" Runat="server" CssClass="lettera" CommandArgument="18" >R</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBs" Runat="server" CssClass="lettera" CommandArgument="19" >S</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBt" Runat="server" CssClass="lettera" CommandArgument="20" >T</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBu" Runat="server" CssClass="lettera" CommandArgument="21" >U</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBv" Runat="server" CssClass="lettera" CommandArgument="22" >V</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBw" Runat="server" CssClass="lettera" CommandArgument="23" >W</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBx" Runat="server" CssClass="lettera" CommandArgument="24" >X</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBy" Runat="server" CssClass="lettera" CommandArgument="25" >Y</asp:linkbutton>
	&nbsp;&nbsp;<asp:linkbutton id="LNBz" Runat="server" CssClass="lettera" CommandArgument="26" >Z</asp:linkbutton>
    <br />
    <asp:Label ID="LBcerca" runat="server">Cerca una parola</asp:Label>
    <asp:TextBox runat="server" ID="TXBcercaveloce"></asp:TextBox>&nbsp;<asp:Label ID="LBwhere" runat="server">nel</asp:Label>&nbsp;<asp:DropDownList runat="server" ID="DDLsearch">
    <asp:ListItem Text="Scegli il campo" Selected="True"></asp:ListItem>
    </asp:DropDownList>&nbsp;<asp:Button Text="CERCA" ID="BTNcerca" runat="server" />
  </div>
  <div class="sfondoCercaVeloce">
    &nbsp;
  </div>
</div>