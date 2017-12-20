Public Interface IviewListaLibretti
	Property OrderBy() As String
	Property Direction() As Comol.Entity.sortDirection
	Property GridPageSize() As Integer
	Property GridCurrentPage() As Integer
	Property GridMaxPage() As Integer
	Property EnableSelectAll() As Boolean
	Property EnableUnselectAll() As Boolean
	Property EnableInvertSelection() As Boolean
	'	ReadOnly Property Presenter() As PresenterLibrettiElettronici
	ReadOnly Property CurrentUserID() As Integer
	ReadOnly Property CurrentCommunityID() As Integer


	Sub NoLibrettiElettronici()
End Interface