Public Class HASH_CategoriaFile
	Inherits Hashtable

	Public ReadOnly Property Name(ByVal CategoriaID As Integer) As String
		Get
			Return DirectCast(MyBase.Item(CategoriaID), String)
		End Get
	End Property

	Sub New()
		MyBase.New()
	End Sub
	'Sub New(ByVal LinguaID As Integer)
	'	Dim oLista As New List(Of COL_CategoriaFile)
	'	oLista = COL_CategoriaFile.ElencaForVisualizza(LinguaID, True)
	'	For Each oCategoria As COL_CategoriaFile In oLista
	'		MyBase.Add(oCategoria.ID, oCategoria.Nome)
	'	Next
	'End Sub
End Class