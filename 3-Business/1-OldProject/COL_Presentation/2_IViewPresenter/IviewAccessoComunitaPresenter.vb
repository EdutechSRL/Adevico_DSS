Public Interface IviewAccessoComunitaPresenter

	ReadOnly Property View() As IviewEntrataComunita
	Sub Initialize()
	Sub MostraDettagliComunita(ByVal ComunitaID As Integer)
	Sub AccediComunita(ByVal ComunitaID As Integer, ByVal Percorso As String)
	Sub CancellaIscrizione(ByVal ComunitaID As Integer, ByVal Percorso As String)
	Sub CaricaPeriodo(Optional ByVal OrganizzazioneID As Integer = -1)
	Sub CaricaOrganizzazione()
	Sub CaricaAnnoAccademico()
	Sub CaricaCorsiDiStudio()
	Sub CaricaStatusComunita()
	Sub CaricaTipoComunita()
	Sub CaricaResponsabile()

End Interface
