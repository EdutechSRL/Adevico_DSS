<Serializable(), CLSCompliant(True)> Public Class GenericElement
	Private _Text As String
	Private _Value As Integer

	Public Property Text() As String
		Get
			Text = _Text
		End Get
		Set(ByVal value As String)
			_Text = value
		End Set
	End Property
	Public Property Value() As Integer
		Get
			Value = _Value
		End Get
		Set(ByVal value As Integer)
			_Value = value
		End Set
	End Property

	Public Sub New()

	End Sub

	Public Sub New(ByVal Value As Integer)
		_Text = String.Empty
		_Value = Value
	End Sub
	Public Sub New(ByVal Value As Integer, ByVal Text As String)
		_Text = Text
		_Value = Value
	End Sub

End Class
