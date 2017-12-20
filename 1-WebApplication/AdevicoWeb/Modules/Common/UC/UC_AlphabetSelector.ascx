<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AlphabetSelector.ascx.vb" Inherits="Comunita_OnLine.UC_AlphabetSelector" %>
<asp:MultiView ID="MLVselector" runat="server">
    <asp:View ID="VIWactive" runat="server">
<span class="btnswitchgroup"><!--
    --><asp:Repeater ID="RPTletters" runat="server"><ItemTemplate><asp:LinkButton ID="LNBletter" runat="server" CssClass="btnswitch"></asp:LinkButton></ItemTemplate></asp:Repeater><!--
    -->
</span>    
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    </asp:View>
</asp:MultiView>