Imports COL_BusinessLogic_v2.Comunita

Public Interface IviewIscrizioneCorsi
	Inherits IviewListGeneric

	Property FiltroOrganizzazioneCorrente() As FilterElement
	Property FiltroAnnoAccademicoCorrente() As FilterElement
	Property FiltroPeriodoCorrente() As FilterElement
	Property FiltroReferenteCorrente() As FilterElement


	Property OrganizzazioneCorrente() As FilterElement
	Property AnnoAccademicoCorrente() As FilterElement
	Property PeriodoCorrente() As FilterElement
	Property ReferenteCorrente() As FilterElement
	Property AutomaticFilterUpdate() As Boolean
	ReadOnly Property LetteraCorrente() As FiltroComunita
	ReadOnly Property QuickSearchCorrente() As QuickSearchElement


	Sub CaricaGriglia(ByVal items As GenericCollection(Of PlainCorso))
	Sub CaricaOrganizzazioni(ByVal items As GenericCollection(Of COL_Organizzazione))
	Sub CaricaPeriodi(ByVal items As GenericCollection(Of COL_Periodo))
	Sub CaricaAnnoAccademico(ByVal items As GenericCollection(Of AnnoAccademico))
    Sub CaricaReferenti(ByVal items As List(Of COL_Persona))
	Sub CaricaQuickSearch(ByVal items As GenericCollection(Of QuickSearchElement))
	Sub RefreshPageCounter(ByVal pageIndex As Integer, ByVal pageCount As Integer, ByVal RecordForPage As Integer, ByVal RecordCount As Integer)
	Sub RefreshRiepilogo()
End Interface