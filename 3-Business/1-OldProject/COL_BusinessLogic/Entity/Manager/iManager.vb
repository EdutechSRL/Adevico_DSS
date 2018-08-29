Namespace Comol.Manager
	Public Interface iManager
#Region "Proprietà"
		ReadOnly Property CurrentUser() As COL_Persona
		ReadOnly Property CurrentCommunity() As COL_Comunita
		ReadOnly Property UseCache() As Boolean
		ReadOnly Property CurrentDB() As ConnectionDB
#End Region
	End Interface
End Namespace