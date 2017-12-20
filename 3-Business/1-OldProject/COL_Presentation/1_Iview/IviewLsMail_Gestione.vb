Public Interface IviewLsMail_Gestione
	Inherits IViewCommon

    ''' <summary>
    ''' Nome della lista per la sua modifica
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property NewName() As String

    ''' <summary>
    ''' Direzione ordinamento
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property OrderDir() As String

    ''' <summary>
    ''' Bind della lista con tutte le "liste di distribuzione"
    ''' </summary>
    ''' <param name="items"></param>
    ''' <remarks></remarks>
    Sub SetLista(ByVal items As IList)

    ''' <summary>
    ''' Nasconde il messaggio di errore
    ''' </summary>
    ''' <remarks></remarks>
    Sub HideMessage()

#Region "Visualizzazione vari messaggi..."
    'Implementati - OK
    Sub ShowErrorInsert()
    Sub ShowInsertOk()
    Sub ShowEliminaOk()
    Sub ShowErrorElimina()
    Sub ShowLista()
    Sub ShowNoData()
#End Region
	Sub ShowAlertMessage(ByVal message As String)
End Interface
