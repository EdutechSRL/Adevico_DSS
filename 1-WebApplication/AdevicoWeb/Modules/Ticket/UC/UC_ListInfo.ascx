<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ListInfo.ascx.vb" Inherits="Comunita_OnLine.UC_ListInfo" %>

<asp:Repeater ID="RPTitem" runat="server">
    <ItemTemplate>
        <asp:literal id="LTstaticItem" runat="server"></asp:literal>
        <asp:linkbutton ID="LKBsendAction" runat="server"></asp:linkbutton>
    </ItemTemplate>
</asp:Repeater>


<asp:literal ID="LTmainCss" runat="server" Visible="false">item statusitem</asp:literal>
<asp:literal ID="LThideCss" runat="server" Visible="false">empty</asp:literal>

<asp:literal id="LTcontentTemplate" runat="server" Visible="false"><span class="item statusitem {hideCss}" {innerTitle}><span class="counter {itemCss}">{innerValue}</span><span class="label">{innerText}</span></span></asp:literal>