Imports Comol.Entity.Events
Imports Comol.Entity.ModuloEsse3
Imports Comol.Entity.ModuloEsse3.OffertaFormativa
Imports Comol.Entity.ModuloEsse3.Registro

Public Interface IviewRegistriEsse3
	Property OrderBy() As String
	Property Direction() As Comol.Entity.sortDirection
	Property GridPageSize() As Integer
	Property GridCurrentPage() As Integer
	Property GridMaxPage() As Integer
	Property EnableSelectAll() As Boolean
	Property EnableUnselectAll() As Boolean
	Property EnableInvertSelection() As Boolean
	ReadOnly Property Presenter() As PresenterRegistriEsse3
	ReadOnly Property CurrentLanguage() As Lingua
	ReadOnly Property CurrentTheacher() As Person
	ReadOnly Property FacoltaCorrente() As Facolta_Esse3
	ReadOnly Property AnnoAccademicoCorrente() As AccademicYear_Esse3
	ReadOnly Property CurrentRegistro() As Int64

	WriteOnly Property Lastupdate() As DateTime


	Sub UnloadImportData()
	Sub CaricaRegistri(ByVal oLista As List(Of RegistroEsse3))
	Sub CaricaRegistro(ByVal oRegistro As RegistroEsse3)
	Sub CaricaDettagliSemplificati(ByVal oDetails As List(Of DettaglioRegistro_Esse3))
	Sub CaricaDettagliEstesi(ByVal oDetails As List(Of DettaglioRegistro_Esse3))
	Sub CaricaFacolta(ByVal oLista As List(Of Facolta_Esse3))
	Sub CaricaAnniAccademici(ByVal oLista As List(Of AccademicYear_Esse3))
	Sub LoadEsse3DetailsForSelect(ByVal oDetails As List(Of DettaglioRegistro_Esse3))
	Sub LoadVerboseEsse3DetailsForSelect(ByVal oDetails As List(Of DettaglioRegistro_Esse3))
	Sub ShowImportResult(ByVal message As String)
	Function SelectedEventsToImport() As IList(Of Int64)
	Sub SelectEsse3Events(ByVal EventsList As IList(Of Int64))
	ReadOnly Property CommunityToImport() As Community
	Sub ShowConfirmImport()
	Sub ShowNoEventToImport()
	Sub ShowConflicts(ByVal oDetailsEvent As List(Of EventConflict))
	Function ConflictsSolution() As IList(Of EventConflict)
End Interface