<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Chat_FileHelp.aspx.vb" Inherits="Comunita_OnLine.Chat_FileHelp"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head runat="server">
		<title>Chat_FileHelp</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>
	</HEAD>
	<body >
		<form id="aspnetForm" method="post" runat="server">
			<TABLE id="Table1" style="FONT-SIZE: xx-small" cellSpacing="0" cellPadding="3" width="370"
				align="center" border="1">
				<TR>
					<TD class="Chat_Pulsante" style="WIDTH: 105px" align="center" width="105">
						<asp:Label ID="LblTasto" Runat="server">Tasto</asp:Label>
					</TD>
					<TD class="Chat_Pulsante" align="center" width="390">
						<asp:Label ID="LblFunzione" Runat="server">Funzione</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD class="chat_scuro" style="WIDTH: 105px" align="center">
					    <INPUT class="Chat_Pulsante" id="Button1" type="button"	size="20" value="File Info" name="Button1" />
					</TD>
					<TD>
						<asp:Label ID="LblPuls1" Runat="server">
				Visualizza le informazioni relative al file: nome, dimensione, da chi è stato 
					caricato, data di immissione.</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD class="chat_scuro" style="WIDTH: 105px" align="center">
						<INPUT class="Chat_Pulsante" id="Button2" type="button" value="Scarica" name="Button2"/>
					</TD>
					<TD>
						<asp:Label ID="LblPuls2" Runat="server">
						Visualizza un elenco di link da cui scaricare i file ricercati.
					</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD class="chat_scuro" style="WIDTH: 105px" align="center">
						<INPUT class="Chat_Pulsante" id="Button3" type="button"	value="Rimuovi" name="Button3"/>
					</TD>
					<TD>
						<asp:Label ID="LblPuls3" Runat="server">
						Elimina i file selezionati. Una volta elimito un file non può più essere recuperato. Dopo un certo periodo di tempo il file viene eliminato in automatico.
					</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD class="chat_scuro" style="WIDTH: 105px" align="center">
						<INPUT class="Chat_Pulsante" id="Button4" type="button" value="Aiuto" name="Button3"/>
					</TD>
					<TD>
						<asp:Label ID="LblPuls4" Runat="server">
					Apre questa schermata.
				</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD class="chat_scuro" style="WIDTH: 105px" align="center"><INPUT class="Chat_Pulsante" id="Button5" type="button" value="Carica file" name="Button3"/>
					</TD>
					<TD>
						<asp:Label ID="LblPuls5" Runat="server">
						Carica il file selezionato nel server.<br/>
						Per motivi di sicurezza l'estensione dei file *.exe viene modificata in *.ex_ e 
						l'estensione *.com in *.co_
					</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD class="chat_scuro" style="WIDTH: 105px" align="center">
						<INPUT class="Chat_Pulsante" id="PostedFile" type="file" size="1" name="InputFileName"/>
					</TD>
					<TD>
						<asp:Label ID="LblPuls6" Runat="server">
						Selezione del file da caricare.
					</asp:Label>
					</TD>
				</TR>
				<tr>
					<td class="chat_scuro" align="center"><IMG alt="Off" src="./../images/OFF.gif"/>
					</td>
					<td>
						<asp:Label ID="LblPuls7" Runat="server">
						Indica che non è possibile caricare un file. E' possibile che un altro utente 
						stia caricando un file o si sia raggiunto il limite massimo di file da 
						caricare.
					</asp:Label>
					</td>
				</tr>
				<tr>
					<td class="chat_scuro" align="center"><IMG src="./../images/ON.gif" alt=""/>
					</td>
					<td>
						<asp:Label ID="LblPuls8" Runat="server">
						Indica che è possibile caricare un nuovo file.
					</asp:Label>
					</td>
				</tr>
				<tr>
					<td class="chat_scuro" align="center"><IMG src="./../images/aggiorna.gif" alt=""/>
					</td>
					<td>
						<asp:Label ID="LblPuls9" Runat="server">
						Aggiorna l'elenco dei file disponibili.
					</asp:Label>
					</td>
				</tr>
				<tr>
					<td><SELECT id="Select1" style="WIDTH: 125px; HEIGHT: 57px" size="3" name="Select1">
							<OPTION>/File1.ext</OPTION>
							<OPTION>/File2.ext</OPTION>
							<OPTION>/File3.ext</OPTION>
							<OPTION>/File4.ext</OPTION>
						</SELECT>
					</td>
					<td>
						<asp:Label id="LblPuls10" Runat="server">
						Elenco dei file disponibili.
					</asp:Label>
					</td>
				</tr>
				<tr>
					<td class="chat_chiaro" colSpan="2">
						<asp:Label ID="LblNote" Runat="server">
							<p><b>NOTE</b></p>
							<P>Gli sviluppatori ed i curatori del sito non si ritengono responsabili dei 
								contenuti dei file inseriti dagli utenti.<br/>
								Non si ritengono inoltre responsabili per la modifica o la cancellazione degli 
								stessi.<br/>
								Viene comunque tenuta traccia del nome dei file, del nome dell'utente e 
								dell'ora in cui il file è stato caricato. I file inoltre sono visibili da tutti 
								i partecipanti alla chat. Si consiglia quindi di cifrare documenti riservati.<br/>
								Si ricorda inoltre che è buona norma comprimere i file in modo da occupare meno 
								spazio e ridurre i tempi di upload e download.<br/>
								Inoltre i file con estensione <b>*.com</b> e <b>*.exe</b> vengono rinominati 
								rispettivamente in <b>*.co_</b> e <b>*.ex_</b> Per eseguire questi file occorre 
								scaricarli e rinominare l'estensione. Si consiglia di controllarli con un 
								antivirus prima di eseguire tale operazione.
							</P>
						</asp:Label>
					</td>
				</tr>
			</TABLE>
			<br/>
			<table cellSpacing="0" cellPadding="0" align="center" border="0" width="370" ID="Table2">
				<tr>
					<td><A href=""></A>
					</td>
					<td align="right">&nbsp;</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
