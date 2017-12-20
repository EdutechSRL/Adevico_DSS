<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_ChatHelp.ascx.vb" Inherits="Comunita_OnLine.UC_ChatHelp" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table1" class="chat_chiaro" cellSpacing="0" cellPadding="3" width="488" align="center"
	border="1">
	<tr>
		<td align="center">
			<asp:imagebutton id="IBPan1_Chat" runat="server" ImageUrl="../images/Chat/Chat_Hot.gif"></asp:imagebutton>
		</td>
		<td colSpan="2">
			<asp:label id="Lbl_IBChat" Runat="server" Font-Size="xx-small">
			Nasconde tutte le impostazioni per visualizzare solo i messaggi
		</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:imagebutton id="IBPan2_Stili" runat="server" ImageUrl="../images/Chat/Sty_HOT.gif"></asp:imagebutton></td>
		<td colSpan="2">
			<asp:label id="Label16" Font-Size="xx-small" Runat="server">Attiva o disattiva le 
				formattazioni del testo
			</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:imagebutton id="IBPan3_Smile" runat="server" ImageUrl="../images/Chat/Smi_Hot.gif"></asp:imagebutton>
		</td>
		<td colSpan="2">
			<asp:label id="Label15" Font-Size="xx-small" Runat="server">Apre una finestra dalla quale è possibile aggiungere le faccine
			</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:imagebutton id="IBPan4_Tools" runat="server" ImageUrl="../images/Chat/Stru_HOT.gif"></asp:imagebutton>
		</td>
		<td colSpan="2">
			<asp:label id="Lbl_IBStrum" Runat="server" Font-Size="xx-small">
				Visualizza gli strumenti per la chat
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">
			<SELECT class="Chat_Pulsante" id="Select" style="FONT-SIZE: xx-small" name="Select2">
				<OPTION>5"</OPTION>
				<OPTION selected="selected">10"</OPTION>
				<OPTION>15"</OPTION>
				<OPTION>20"</OPTION>
				<OPTION>30"</OPTION>
				<OPTION>1'</OPTION>
			</SELECT>
		</td>
		<td>
			<asp:label id="Label20" Font-Size="xx-small" Runat="server">
				Tempo di aggiornamento
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">
			<IMG alt="" src="./../images/Clear.gif"/>
		</td>
		<td>
			<asp:label id="Label10" Font-Size="xx-small" Runat="server">Cancella i messaggi correnti.
			</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:imagebutton id="IBPan5_File" runat="server" ImageUrl="../images/Chat/File_Hot.gif" Font-Size="xx-small"></asp:imagebutton>
		</td>
		<td colSpan="2">
			<asp:label id="Label25" Font-Size="xx-small" Runat="server">Apre la finestra per la gestione dei file
			</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:imagebutton id="IBPan6_Utenti" runat="server" ImageUrl="../images/Chat/Ut_Hot.gif"></asp:imagebutton>
		</td>
		<td colSpan="2">
			<asp:label id="Lbl_IBUtenti" Runat="server" Font-Size="xx-small">
				Visualizza la lista utenti
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">
			<SELECT class="Chat_Pulsante" id="Select1" style="FONT-SIZE: xx-small; WIDTH: 176px; HEIGHT: 86px"
				multiple size="5" name="Select1">
				<OPTION>A - Mario Rossi</OPTION>
				<OPTION>W - Gino Bianchi</OPTION>
				<OPTION>R - Luca Verdi</OPTION>
				<OPTION>O - Marco Neri</OPTION>
				<OPTION>W - B - Andrea Gialli</OPTION>
			</SELECT>
		</td>
		<td>
			<asp:label id="Lbl_ElencoUtenti" Runat="server" Font-Size="xx-small">
				Lista utenti
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">A -
		</td>
		<td>
			<asp:label id="Label26" Font-Size="xx-small" Runat="server">Amministratore della chat.</asp:label>
			<asp:label id="Label27" Font-Size="xx-small" Runat="server"> Può modificare i livelli degli altri utenti,visualizzare tutti i messaggi contenuti nel buffer e cancellare il buffer.
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">W -</td>
		<td>
			<asp:label id="Label28" Font-Size="xx-small" Runat="server">
			Utente generico (Writer).
			</asp:label>
			<asp:label id="Label29" Font-Size="xx-small" Runat="server"> 
			Può scrivere messaggi e leggere tutti i messaggi pubblici e privati (scritti o diretti a lui).
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">R -</td>
		<td>
			<asp:label id="Label30" Font-Size="xx-small" Runat="server">In sola lettura (Reader).</asp:label>
			<asp:label id="Label31" Font-Size="xx-small" Runat="server"> Può solo leggere i messaggi pubblici e quelli privati inviati a lui. Non può inviare messaggi.</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">O -</td>
		<td>
			<asp:label id="Label32" Font-Size="xx-small" Runat="server">Fuori dalla chat (OUT).</asp:label>
			<asp:label id="Label33" Font-Size="xx-small" Runat="server">L'utente non ha più accesso alla chat. Questo stato è 
			temporaneo e serve nel caso un utente non si comporti correttamente.</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">- B -</td>
		<td>
			<asp:label id="Label34" Font-Size="xx-small" Runat="server">
				Utente bloccato
			</asp:label>
			<asp:label id="Label35" Font-Size="xx-small" Runat="server">
				L'utente è stato bloccato. Non verranno più visualizzati i suoi messaggi. L'utente bloccatto non viasualizza più l'utente che ha effettuato il blocco nella lista utenti. Il blocco ha effettu su tutte le comunità. - Non è possibile bloccarsi o bloccare gli amministratori -
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">
			<IMG alt="" src="./../Images/lucchetto_closed.gif"/>
		</td>
		<td>
			<asp:label id="Label21" Font-Size="xx-small" Runat="server">
				Blocca un utente
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">
			<IMG alt="" src="./../Images/lucchetto_open.gif"/>
		</td>
		<td>
			<asp:label id="Label23" Font-Size="xx-small" Runat="server">
				Sblocca un utente bloccato
			</asp:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="center">
			<asp:imagebutton id="IMBaggiorna" runat="server" ImageUrl="./../Images/aggiorna.gif" AlternateText="Aggiorna"></asp:imagebutton>
		</td>
		<td>
			<asp:Label ID="Lbl_Refresh" Runat="server" Font-Size="xx-small">
				Aggiorna la lista utenti
			</asp:Label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:imagebutton id="IBPan7_Comunita" runat="server" ImageUrl="../images/Chat/Com_Hot.gif"></asp:imagebutton>
		</td>
		<td colSpan="2">
			<asp:label id="Lbl_IBCom" Runat="server" Font-Size="xx-small">			
			Lista comunita
	</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:image id="ImgLinkHelp" Runat="server" ImageUrl="../images/Chat/Hlp_HOT.gif"></asp:image>
		</td>
		<td colSpan="2">
			<asp:label id="Lbl_Help" Runat="server" Font-Size="xx-small">
				Visualizza questa schermata
			</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:button id="BtSendAll" Font-Size="XX-Small" runat="server" Text="Invia" CssClass="Chat_Pulsante"></asp:button>
		</td>
		<td colSpan="2">
			<asp:label id="Label4" Font-Size="xx-small" Runat="server">
				Invia un messaggio a tutti i partecipanti della chat.
			</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:button id="BtSendTo" Font-Size="XX-Small" runat="server" Text="Invia a..." CssClass="Chat_Pulsante"></asp:button>
		</td>
		<td colSpan="2">
			<asp:label id="Label6" Font-Size="xx-small" Runat="server">
				Invia un messaggio pubblico destinato agli utenti selezionati. Per selazionare più utenti premere CTRL.
			</asp:label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:button id="BtPrivate" Font-Size="XX-Small" runat="server" Text="Privato per..." CssClass="Chat_Pulsante"></asp:button>
		</td>
		<td colSpan="2">
			<asp:label id="Label8" Font-Size="xx-small" Runat="server">
				Invia un messaggio privato ai soli utenti  selezionati. Per selazionare più utenti premere CTRL.
			</asp:label>
		</td>
	</tr>
</TABLE>
