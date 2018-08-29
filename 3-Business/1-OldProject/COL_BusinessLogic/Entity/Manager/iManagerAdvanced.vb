Namespace Comol.Manager
	Public Interface iManagerAdvanced
#Region "Proprietà"
		ReadOnly Property CurrentUser() As Person
		ReadOnly Property CurrentCommunity() As Community
		ReadOnly Property UseCache() As Boolean
		ReadOnly Property CurrentDB() As ConnectionDB
		ReadOnly Property Language() As Lingua
#End Region
	End Interface
End Namespace