<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FontSettings.ascx.vb" Inherits="Comunita_OnLine.UC_FontSettings" %>

<style>
    .fontbold {
        font-weight: bold;
    }
    .fontitalic {
        font-style: italic;
    }
    .fontunderline label {
        text-decoration: underline;
    }

</style>



<asp:DropDownList runat="server" ID="DDLfontName">
    <asp:ListItem Value="TIMES" Text="Times" Selected="true"></asp:ListItem>
    <asp:ListItem Value="HELVETICA" Text="Helvetica"></asp:ListItem>
    <asp:ListItem Value="COURIER" Text="Courier"></asp:ListItem>
</asp:DropDownList>

<asp:DropDownList runat="server" ID="DDLfontSize">
    <asp:ListItem Value="8" Text="8"></asp:ListItem>
    <asp:ListItem Value="9" Text="9"></asp:ListItem>
    <asp:ListItem Value="10" Text="10"></asp:ListItem>
    <asp:ListItem Value="11" Text="11"></asp:ListItem>
    <asp:ListItem Value="12" Text="12" Selected="True"></asp:ListItem>
    <asp:ListItem Value="14" Text="14"></asp:ListItem>
    <asp:ListItem Value="16" Text="16"></asp:ListItem>
    <asp:ListItem Value="18" Text="18"></asp:ListItem>
    <asp:ListItem Value="20" Text="20"></asp:ListItem>
    <asp:ListItem Value="22" Text="22"></asp:ListItem>
    <asp:ListItem Value="24" Text="24"></asp:ListItem>
    <asp:ListItem Value="26" Text="26"></asp:ListItem>
    <asp:ListItem Value="28" Text="28"></asp:ListItem>
    <asp:ListItem Value="36" Text="36"></asp:ListItem>
    <asp:ListItem Value="48" Text="48"></asp:ListItem>
    <asp:ListItem Value="72" Text="72"></asp:ListItem>
</asp:DropDownList>

<asp:CheckBox runat="server" ID="CBXbold" Text="*Bold" CssClass="fontbold"/>
<asp:CheckBox runat="server" ID="CBXitalic" Text="*Italic" CssClass="fontitalic"/>
<asp:CheckBox runat="server" ID="CBXunderline" Text="*Underline" CssClass="fontunderline"/>



<%--
COURIER
HELVETICA
SYMBOL
TIMES
ZAPFDINGBATS
--%>
                