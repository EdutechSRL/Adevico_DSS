<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PagerControl.ascx.vb" Inherits="Comunita_OnLine.UC_PagerControl" %>
    <asp:ImageButton CssClass="elementoGiu01" runat="server" ID="IMBfirst" ImageUrl="~/Style/Pagers/Img/primo.gif" CommandName="-4" />
    <asp:Literal ID="LTseparator_First" runat="server" EnableViewState="false">&nbsp;</asp:Literal>
    <asp:ImageButton CssClass="elementoGiu01" runat="server" ID="IMBprev" ImageUrl="~/Style/Pagers/Img/indietro.gif" CommandName="-3" />
    <asp:Literal ID="LTseparator_Previous" runat="server" EnableViewState="false">&nbsp;&nbsp;</asp:Literal>
    <span id="spanbutton" runat="server">
      <asp:LinkButton ID="LNB1" CommandArgument="0" runat="server" CssClass="ROW_PagerLink">1</asp:LinkButton>
      <asp:LinkButton ID="LNB2" CommandArgument="1" runat="server" CssClass="ROW_PagerLink">2</asp:LinkButton>
      <asp:LinkButton ID="LNB3" CommandArgument="2" runat="server" CssClass="ROW_PagerLink">3</asp:LinkButton>
      <asp:LinkButton ID="LNB4" CommandArgument="3" runat="server" CssClass="ROW_PagerLink">4</asp:LinkButton>
      <asp:LinkButton ID="LNB5" CommandArgument="4" runat="server" CssClass="ROW_PagerLink">5</asp:LinkButton>
      <asp:LinkButton ID="LNB6" CommandArgument="5" runat="server" CssClass="ROW_PagerLink">6</asp:LinkButton>
      <asp:LinkButton ID="LNB7" CommandArgument="6" runat="server" CssClass="ROW_PagerLink">7</asp:LinkButton>
      <asp:LinkButton ID="LNB8" CommandArgument="7" runat="server" CssClass="ROW_PagerLink">8</asp:LinkButton>
      <asp:LinkButton ID="LNB9" CommandArgument="8" runat="server" CssClass="ROW_PagerLink">9</asp:LinkButton>      
    </span>
     <span id="spannavigate" runat="server">
        <asp:HyperLink ID="HYP1" runat="server" CssClass="ROW_PagerLink">1</asp:HyperLink>
      <asp:HyperLink ID="HYP2" runat="server" CssClass="ROW_PagerLink">2</asp:HyperLink>
      <asp:HyperLink ID="HYP3" runat="server" CssClass="ROW_PagerLink">3</asp:HyperLink>
      <asp:HyperLink ID="HYP4" runat="server" CssClass="ROW_PagerLink">4</asp:HyperLink>
      <asp:HyperLink ID="HYP5" runat="server" CssClass="ROW_PagerLink">5</asp:HyperLink>
      <asp:HyperLink ID="HYP6" runat="server" CssClass="ROW_PagerLink">6</asp:HyperLink>
      <asp:HyperLink ID="HYP7" runat="server" CssClass="ROW_PagerLink">7</asp:HyperLink>
      <asp:HyperLink ID="HYP8" runat="server" CssClass="ROW_PagerLink">8</asp:HyperLink>
      <asp:HyperLink ID="HYP9" runat="server" CssClass="ROW_PagerLink">9</asp:HyperLink>
    </span>
    <asp:Literal ID="LTseparator_Next" runat="server" EnableViewState="false">&nbsp;&nbsp;</asp:Literal>
    <asp:ImageButton runat="server" ID="IMBnext" CssClass="elementoGiu01" ImageUrl="~/Style/Pagers/Img/avanti.gif" CommandName="-2" />
    <asp:Literal ID="LTseparator_Last" runat="server" EnableViewState="false">&nbsp;</asp:Literal>
    <asp:ImageButton runat="server" ID="IMBlast" CssClass="elementoGiu01" ImageUrl="~/Style/Pagers/Img/ultimo.gif" CommandName="-1" />