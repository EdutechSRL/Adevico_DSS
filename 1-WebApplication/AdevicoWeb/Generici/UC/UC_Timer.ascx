<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Timer.ascx.vb" Inherits="Comunita_OnLine.UC_Timer" %>
<asp:UpdatePanel ID="UPTempo" runat="server">
     <Triggers>
        <asp:AsyncPostBackTrigger ControlID="TMSessione" EventName="Tick" />
    </Triggers>
</asp:UpdatePanel>
<asp:Timer ID="TMSessione" runat="server" Enabled="true">
</asp:Timer>