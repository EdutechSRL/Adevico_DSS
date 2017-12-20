<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SwitchClientSide.ascx.vb" Inherits="Comunita_OnLine.UC_ClientSideSwitch" %>
<div class="coveredradio enabled" data-disable="<%=DataDisable%>">
    <asp:radiobuttonlist ID="RBLswitch" class="wclist" runat="server" RepeatLayout="Flow">
        <asp:ListItem Value="true" data-value="on"></asp:ListItem>
        <asp:ListItem Value="false" data-value="off"></asp:ListItem>
    </asp:radiobuttonlist>
    <span class="btnswitchgroup small"><asp:HyperLink ID="HYPswitchOn" class="btnswitch on first" href="" runat="server"></asp:HyperLink><!----><asp:HyperLink ID="HYPswitchOff" class="btnswitch off last" href="" runat="server"></asp:HyperLink><!----></span>
</div>