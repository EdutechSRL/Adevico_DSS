Imports COL_BusinessLogic_v2.Comunita

Public Interface IviewComunitaGestioneIScritti
    Inherits IviewListGeneric

#Region "Filtri"
    Property Ruolo() As Integer
    Property TipoRicerca() As Integer
    Property ValoreRicerca() As String
    Property VisualizzaIscrizione() As Integer
	Sub CaricaRuoli(ByVal items As GenericCollection(Of Role))

    Property oFiltroAbilitazione() As FiltroAbilitazione
    Property TipoRuoloId() As Integer
    Property oAnagrafica() As FiltroAnagrafica
    Property oFiltroRicercaAnagrafica() As FiltroRicercaAnagrafica

    Sub ShowModifica()
    Property Iscritto_Id() As Integer
    WriteOnly Property Iscritto_Anagrafica() As String
    Property Iscritto_IdRuolo() As Integer
    Property Iscritto_IsResponsabile() As Boolean

    Sub ShowGriglia()

#End Region

End Interface

