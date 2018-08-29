Public Class HASH_TipoMateriale
	Inherits Hashtable

	Public ReadOnly Property Name(ByVal TipoMaterialeID As Integer) As String
		Get
			Return DirectCast(MyBase.Item(TipoMaterialeID), String)
		End Get
	End Property

	Sub New()
		MyBase.New()
	End Sub
	'Sub New(ByVal LinguaID As Integer)
	'	Dim oLista As New List(Of COL_TipoMateriale)
	'	oLista = COL_TipoMateriale.ElencaForVisualizza(LinguaID)
	'	For Each oTipoMateriale As COL_TipoMateriale In oLista
	'		MyBase.Add(oTipoMateriale.Id, oTipoMateriale.Descrizione)
	'	Next
	'End Sub
End Class
