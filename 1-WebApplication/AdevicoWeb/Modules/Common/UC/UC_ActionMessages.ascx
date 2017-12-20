<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ActionMessages.ascx.vb" Inherits="Comunita_OnLine.UC_ActionMessages" %>
<%@ Register TagPrefix="CTRL" TagName="Message" Src="~/Modules/Common/UC/UC_ActionMessage.ascx" %>
<asp:Repeater ID="RPTmessages" runat="server">
    <HeaderTemplate>
        <div class="messages">
    </HeaderTemplate>
    <ItemTemplate>
        <CTRL:Message runat="Server" id="CTRLmessage" MessageItemCommand="MessageItemCommand"></CTRL:Message>
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>