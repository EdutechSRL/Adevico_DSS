Public Interface IviewLsMail_Lista
	Inherits IViewCommon

#Region "Proprietà della lista attuale"
    Property NomeLista() As String
    Property IdLista() As Integer
    Property IdProprietario() As Integer
#End Region
#Region "Nuovo indirizzo: da inserire"
    Property Ind_Id() As Integer
    Property Ind_PrsnId() As Integer
    Property Ind_Titolo() As String
    Property Ind_Nome() As String
    Property Ind_Cognome() As String
    Property Ind_Mail() As String
    Property Ind_Struttura() As String
#End Region

    ''' <summary>
    ''' Direzione di ordinamento
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property SortDir() As String

    ''' <summary>
    ''' Bind dell'elenco degli indirizzi
    ''' </summary>
    ''' <param name="Items"></param>
    ''' <remarks></remarks>
    Sub SetLista(ByVal Items As IList)

#Region "Visualizzazioni e messaggi"
    Sub ShowElenco()
    Sub ShowNoItem()

    Sub ShowModifica()

    Sub HideMessage()
    Sub ShowNuovo()
    Sub ShowUpdateOk()
    Sub ShowUpdateError()
    Sub ShowAddUserOk()
    Sub ShowAddUserError()
    Sub ShowDelOk()
    Sub ShowDelError()

    Sub ShowMessageToPage(ByVal errorMessage As String)
#End Region

End Interface
