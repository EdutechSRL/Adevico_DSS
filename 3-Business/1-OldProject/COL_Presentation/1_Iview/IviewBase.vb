Public Interface IviewBase
    Inherits IViewCommon

    ReadOnly Property History() As HistoryElement
    'ReadOnly Property Logo(ByVal OrganizzazioneID As Integer) As LinkElement


    Property UtentiConnessi() As Integer
    Property PostItSistema() As COL_PostIt
    Property ShowPostItSistema() As Boolean
    Property RiepilogoPostIt() As Integer

    Sub CambiaComunitaFromHistory(ByVal ComunitaID As Integer)
    Sub CambiaComunita(ByVal OrganizzazioneID As Integer, ByVal ComunitaID As Integer, ByVal TipoComunitaID As Integer, ByVal RuoloID As Integer, ByVal iListaServizi As ServiziCorrenti, ByVal iHistory As HistoryElement)

    Function ViewGetPostBackEventReference(ByVal argomenti As String) As String

End Interface