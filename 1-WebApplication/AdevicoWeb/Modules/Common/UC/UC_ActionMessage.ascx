<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ActionMessage.ascx.vb" Inherits="Comunita_OnLine.UC_ActionMessage" %>
<div class="message <%=CssClass%> <%=CssClassMessageType%>">
    <span class="icons"><span class="icon"> </span></span><asp:Literal ID="LTmessage" runat="server"></asp:Literal>
    <asp:LinkButton ID="LNBconfirm" runat="server" CausesValidation="false" CommandName="confirm" Visible="false" CssClass="linkMenu"></asp:LinkButton>
    <asp:LinkButton ID="LNBcancel" runat="server" CausesValidation="false" CommandName="cancel" Visible="false" CssClass="linkMenu"></asp:LinkButton>
</div>