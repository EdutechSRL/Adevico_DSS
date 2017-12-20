<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CompactInternalFileUploader.ascx.vb"
    Inherits="Comunita_OnLine.UC_CompactInternalFileUploader" %>
    <input class="Testo_campo_obbligatorioSmall" id="TXBFile" type="file" size="60" name="TXBFile" runat="server" />
<asp:UpdatePanel ID="UDPtype" UpdateMode="Conditional" ChildrenAsTriggers="true"
    runat="server">
    <ContentTemplate>
        <asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall">Type:</asp:Label>
        <asp:RadioButtonList ID="RBLtype" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
            RepeatLayout="Flow" AutoPostBack="true">
        </asp:RadioButtonList>
        <asp:Label ID="LBplay" runat="server" CssClass="Titolo_campoSmall">(Play:</asp:Label>
                <asp:RadioButtonList ID="RBLplay" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="False" Selected="True">Only on platform</asp:ListItem>
                    <asp:ListItem Value="True">Allow also download</asp:ListItem>
                </asp:RadioButtonList>
        <asp:Literal ID="LTplayClosed" runat="server">):</asp:Literal>
    </ContentTemplate>
</asp:UpdatePanel>