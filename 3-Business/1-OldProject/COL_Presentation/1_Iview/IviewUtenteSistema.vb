Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona.CL_Docente
Imports COL_BusinessLogic_v2.Comol.Authentication


Public Interface IviewUtenteSistema
	Property Mode() As Modalita
	Property Status() As Fase
	Property Organizzazione() As COL_Organizzazione
	Property Login() As String
	Property Password() As String
	Property ConfirmPassword() As String
	Property Nome() As String
	Property Cognome() As String

	Property DataNascita() As Date
	Property TipoPersona() As COL_TipoPersona
	Property TipoAutenticazione() As AuthenticationType
	Property Lingua() As lingua



	Sub PopolaOrganizzazioni(ByVal Lista As List(Of COL_Organizzazione))
	Sub PopolaTipiPersona(ByVal Lista As List(Of COL_TipoPersona))
	Sub PopolaProvincia(ByVal Lista As List(Of Provincia))
	Sub PopolaStato(ByVal Lista As List(Of Provincia))
	Sub PopolaLingua(ByVal Lista As List(Of Lingua))


	Enum Fase
		SceltaOrganizzazione
		CampiPrimari
		DettagliTipologiaPersona
		RecapitiFisici
		RecapitiElettronici
		ConfermaFinale
	End Enum
	Enum Modalita
		Iscrizione
		Modifica
		CambioTipologia
	End Enum
End Interface