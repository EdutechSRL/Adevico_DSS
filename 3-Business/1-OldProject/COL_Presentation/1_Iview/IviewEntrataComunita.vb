Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports System.Collections
Imports System.Collections.Generic

Public Interface IviewEntrataComunita
	Inherits IviewGeneric

	Property Ordinamento() As FiltroOrdinamento
	ReadOnly Property Servizio() As Services_ElencaComunita

	' filtri applicati
	Property FiltroOrganizzazione() As FilterElement
    Property FiltroReferente() As FilterElement
	Property FiltroStatus() As FilterElement
    Property FiltroTipoComunita() As FilterElement
	Property FiltroLettera() As FiltroComunita
	ReadOnly Property FiltroQuickSearch() As QuickSearchElement

	' elementi filtri selezionati
	Property Selezione_Organizzazioneata() As FilterElement
    Property Selezione_TipoComunita() As FilterElement
    Property Selezione_Referente() As FilterElement
	Property Selezione_Status() As FilterElement

	' elementi filtri 
	WriteOnly Property ElencoOrganizzazioni() As List(Of COL_BusinessLogic_v2.IscrizioneComunita)
    WriteOnly Property ElencoResponsabili() As List(Of COL_Persona)
	WriteOnly Property ElencoStatus() As List(Of FilterElement)
	WriteOnly Property ElencoTipoComunita() As List(Of COL_Tipo_Comunita)
	Property AutomaticFilterUpdate() As Boolean
	



	Sub CaricaLista(ByVal items As List(Of PlainComunita))
	Sub DettagliComunita(ByVal ComunitaID As Integer)
	Sub EntrataComunita(ByVal ComunitaID As Integer, ByVal Percorso As String)

End Interface