<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_DettagliBookmark.ascx.vb" Inherits="Comunita_OnLine.UC_DettagliBookmark" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Table Runat="server" ID="TBLinfo" CellPadding="5" CellSpacing="0" Width=450px>
	<asp:TableRow ID="TBRnome">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBnomeLink_t" Runat="server" CssClass="Bookmark_TitoloBold">Nome:</asp:Label>
		</asp:TableCell>
		<asp:TableCell >
			<asp:Label ID="LBnomeLink" Runat="server" CssClass="Bookmark_Testo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow> 
	<asp:TableRow ID="TBRlink">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBlink_t" Runat="server" CssClass="Bookmark_TitoloBold">Link:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBlink" Runat="server" CssClass="Bookmark_Testo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRdescrizione">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBdescrizione_t" Runat="server" CssClass="Bookmark_TitoloBold">Descrizione:</asp:Label>
		</asp:TableCell>
		<asp:TableCell >
			<asp:Label ID="LBdescrizione" Runat="server" CssClass="Bookmark_TestoInfo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRimportato">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBtipoLink_t" Runat="server" CssClass="Bookmark_TitoloBold">Tipo Link:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBtipoLink" Runat="server" CssClass="Bookmark_Testo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRcreatoil">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBcreato_t" Runat="server" CssClass="Bookmark_TitoloBold">Creato il:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBcreato" Runat="server" CssClass="Bookmark_Testo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRmodificatoil">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBmodificato_t" Runat="server" CssClass="Bookmark_TitoloBold">Ultima modifica:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBmodificato" Runat="server" CssClass="Bookmark_Testo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="TBRautore">
		<asp:TableCell Width=100px>
			<asp:Label ID="LBautore_t" Runat="server" CssClass="Bookmark_TitoloBold">Autore:</asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="LBautore" Runat="server" CssClass="Bookmark_Testo"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>