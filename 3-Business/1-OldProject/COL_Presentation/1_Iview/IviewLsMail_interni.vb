Public Interface IviewLsMail_interni
	Inherits IViewCommon

    ''' <summary>
    ''' Per il bind della lista
    ''' </summary>
    ''' <param name="Items"></param>
    ''' <remarks></remarks>
    Sub SetLista(ByVal Items As IList)

    ''' <summary>
    ''' ID della comunità selezionata
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property SelectedComunitaId() As Integer

    ''' <summary>
    ''' Id dell'indirizzo in cui aggiungere gli iscritti
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property AddressId() As Integer

    ''' <summary>
    ''' Mostra la griglia
    ''' </summary>
    ''' <remarks></remarks>
    Sub Showgriglia()

    ''' <summary>
    '''  Nessun elemento trovato
    ''' </summary>
    ''' <remarks></remarks>
    Sub ShowNoItem()

    ''' <summary>
    '''  Parametri insufficenti per effettuare la ricerca
    ''' </summary>
    ''' <remarks></remarks>
    Sub ShowNoParameter()

    ''' <summary>
    ''' Direzione ordinamento
    ''' </summary>
    ''' <remarks> 
    ''' "asc" - ascendente
    ''' "desc" - discendente
    ''' </remarks>
    Property OrderDir() As String

    ''' <summary>
    '''  Precedente campo per l'ordinamento
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    '''   Se il nuovo OrderExpr della gridview è uguale a questo, inverto la direzione di ordinamento
    ''' </remarks>
    Property OldOrderExpr() As String

    ''' <summary>
    '''  Salva la selezione corrente
    ''' </summary>
    ''' <remarks>
    '''  Viene inoltre eseguita OGNI volta che viene effettuato un bind sulla griglia, per mantenere i dati selezionati o deselezionati
    ''' </remarks>
    Sub SaveCurrent()

    ''' <summary>
    ''' Visualizza un messaggio nella pagina
    ''' </summary>
    ''' <param name="errorMessage">Errore visualizzato</param>
    ''' <remarks></remarks>
    Sub ShowMessageToPage(ByVal errorMessage As String)

End Interface