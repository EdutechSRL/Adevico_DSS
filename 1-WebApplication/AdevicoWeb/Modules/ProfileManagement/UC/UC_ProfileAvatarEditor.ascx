<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileAvatarEditor.ascx.vb" Inherits="Comunita_OnLine.UC_ProfileAvatarEditor" %>

<span class="Fieldrow">
    <asp:Label ID="LBavatarInfo" runat="server" CssClass="Testo_Campo">
		ATTENZIONE! Dimensioni consigliate: Altezza 125 larghezza 100 pixel.
		Se tale rapporto non viene rispettato l'immagine verrà ridimensionata o tagliata.
    </asp:Label>
</span>
<span class="Fieldrow">&nbsp;</span>
<span class="Fieldrow" id="SPNerrors" runat="server" visible="false">
    <asp:Label ID="LBavatarErrors" runat="server" CssClass="Testo_Campo">

    </asp:Label>
</span>
<span class="Fieldrow">
    <asp:Label ID="LBavatarInsert_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="INFavatar">Select file: </asp:Label>
    <input type="file" id="INFavatar" runat="server" size="60" />
</span>
<span class="Fieldrow">&nbsp;</span>
<span class="Fieldrow">
    <asp:Button ID="BTNcloseMailWindowFromEditor" runat="server" Text="Chiudi" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
    <asp:Button ID="BTNupload" runat="server" Text="Upload" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
</span>