<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DisplayRepositoryItem.ascx.vb" Inherits="Comunita_OnLine.UC_DisplayRepositoryItem" %>
<asp:MultiView ID="MLVdisplayMode" runat="server">
    <asp:View ID="VIWinline" runat="server">
        <asp:Literal ID="LTitem" runat="server"></asp:Literal>
    </asp:View>
     <asp:View ID="VIWtable" runat="server">
    
    </asp:View>
</asp:MultiView>