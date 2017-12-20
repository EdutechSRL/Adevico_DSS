<Serializable()> _
Public Class GenericNews
	Private _TestoScorrevole As String
	Private _Descrizione As String
	Private _LinguaID As Integer
	Private _isModificato As Boolean

	Public Property TestoScorrevole() As String
		Get
			TestoScorrevole = _TestoScorrevole
		End Get
		Set(ByVal value As String)
			If _TestoScorrevole <> value Then
				_isModificato = True
			End If
			_TestoScorrevole = value
		End Set
	End Property
	Public Property Descrizione() As String
		Get
			Descrizione = _Descrizione
		End Get
		Set(ByVal value As String)
			If _Descrizione <> value Then
				_isModificato = True
			End If
			_Descrizione = value
		End Set
	End Property
	Public Property LinguaID() As Integer
		Get
			LinguaID = _LinguaID
		End Get
		Set(ByVal value As Integer)
			_LinguaID = value
		End Set
	End Property
	Public ReadOnly Property isModificato() As Integer
		Get
			isModificato = _isModificato
		End Get
	End Property

	Public Sub New()

	End Sub

	Public Sub New(ByVal LinguaID As Integer, ByVal TestoBreve As String, ByVal Descrizione As String)
		_TestoScorrevole = TestoBreve
		_Descrizione = Descrizione
		_LinguaID = LinguaID
		_isModificato = False
	End Sub

	Public Shared Function FindByLingua(ByVal item As GenericNews, ByVal value As Integer) As Boolean
		Return (item.LinguaID = value)
	End Function

End Class