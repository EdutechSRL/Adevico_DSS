<%@ Control Language="vb" AutoEventWireup="false" EnableTheming="true" CodeBehind="CTRLgridUnder.ascx.vb" Inherits="Comunita_OnLine.CTRLgridUnder" %>

<div class="parteSotto">
  <div class="legenda">
    <asp:Panel runat="server" ID="pnl1" CssClass="fLeft"><div class="fLeft elementoGiu04" style="height: 1em; width: 1em; border: 1px #ffffff solid; background-color: #99adc2;"></div>&nbsp;Comunità bloccata&nbsp;&nbsp;</asp:Panel>
    <asp:Panel runat="server" ID="Panel1" CssClass="fLeft"><div class="fLeft elementoGiu04" style="height: 1em; width: 1em; border: 1px #ffffff solid; background-color: #e1b999;"></div>&nbsp;Comunità archiviata&nbsp;&nbsp;</asp:Panel>
    <asp:Panel runat="server" ID="Panel2" CssClass="fLeft"><div class="fLeft elementoGiu04" style="height: 1em; width: 1em; border: 1px #ffffff solid; background-color: #999999;"></div>&nbsp;In attesa di conferma&nbsp;&nbsp;</asp:Panel>
    <asp:Panel runat="server" ID="Panel3" CssClass="fLeft"><div class="fLeft elementoGiu04" style="height: 1em; width: 1em; border: 1px #ffffff solid; background-color: #c2adc2;"></div>&nbsp;Accesso alla comunità bloccato</asp:Panel>
    <asp:Panel runat="server" ID="Panel4" CssClass="fRight"><asp:checkbox runat="server" ID="CB1" Checked="true" />Mostra comunità archiviate</asp:Panel>
  </div>
  <div class="numPagineSotto">
    <span style="font-size: x-small;"><asp:Literal ID="LTnumeroRecord" runat="server">n. record</asp:Literal>&nbsp;&nbsp;</span>
    <asp:DropDownList runat="server" ID="DDLNumPagine" AutoPostBack="True">
    <asp:ListItem Text=" 10" Value=10 ></asp:ListItem>
    <asp:ListItem Text=" 20" value=20 Selected="True"></asp:ListItem>
    <asp:ListItem Text=" 30" value=30 ></asp:ListItem>
    <asp:ListItem Text=" 50" value=50 ></asp:ListItem>
    <asp:ListItem Text="100" value=100 ></asp:ListItem>
    </asp:DropDownList>
  </div>
  <div class="sfondoNumPagineSotto">
    &nbsp;
  </div>
  <div class="pagingSotto">
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
  <div class="sfondoPagingSotto">
    &nbsp;
  &nbsp;</div>
</div>