Public Interface IviewAccessoComunitaPresenter

	ReadOnly Property View() As IviewEntrataComunita
	Sub Initialize()
	Sub MostraDettagliComunita(ByVal ComunitaID As Integer)
	Sub AccediComunita(ByVal ComunitaID As Integer, ByVal Percorso As String)
	Sub CancellaIscrizione(ByVal ComunitaID As Integer, ByVal Percorso As String)
    Sub CaricaOrganizzazione()
    Sub CaricaStatusComunita()
	Sub CaricaTipoComunita()
	Sub CaricaResponsabile()

End Interface
