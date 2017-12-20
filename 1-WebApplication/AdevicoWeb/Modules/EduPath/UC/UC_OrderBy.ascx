<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_OrderBy.ascx.vb" Inherits="Comunita_OnLine.UC_OrderBy" %>
<span class="btnOrderBy">
    <asp:LinkButton runat="server" ID="LNBup" Visible="false"><% = Me.TextUp%></asp:LinkButton>
    <asp:LinkButton runat="server" ID="LNBdown" Visible="false"><% = Me.TextDown%></asp:LinkButton>
    <asp:LinkButton runat="server" ID="LNBupDown" Visible="true"><% = Me.TextDown%></asp:LinkButton>
</span>