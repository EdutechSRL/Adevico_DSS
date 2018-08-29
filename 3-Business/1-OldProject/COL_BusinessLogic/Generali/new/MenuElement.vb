Public Class MenuElement
	Private _Text As String
	Private _Url As String
	Private _UniqueID As Integer
	Private _PadreID As Integer

	Public Property ID() As Integer
		Get
			ID = _UniqueID
		End Get
		Set(ByVal value As Integer)
			_UniqueID = value
		End Set
	End Property
	Public Property Text() As String
		Get
			Text = _Text
		End Get
		Set(ByVal value As String)
			_Text = value
		End Set
	End Property
	Public Property Url() As String
		Get
			Url = _Url
		End Get
		Set(ByVal value As String)
			_Url = value
		End Set
	End Property
	Public Property PadreID() As Integer
		Get
			PadreID = _PadreID
		End Get
		Set(ByVal value As Integer)
			_PadreID = value
		End Set
	End Property

	Public Sub New()
		_Text = String.Empty
		_Url = String.Empty
		_UniqueID = 0
		_PadreID = 0
	End Sub
	Public Sub New(ByVal Testo As String)
		_Text = Testo
		_Url = String.Empty
		_UniqueID = 0
		_PadreID = 0
	End Sub
	Public Sub New(ByVal Testo As String, ByVal Url As String)
		_Text = Testo
		_Url = Url
		_UniqueID = 0
		_PadreID = 0
	End Sub
	Public Sub New(ByVal ID As Integer, ByVal Testo As String, ByVal Url As String)
		_Text = Testo
		_Url = Url
		_PadreID = 0
		_UniqueID = ID
	End Sub
	Public Sub New(ByVal ID As Integer, ByVal PadreID As Integer, ByVal Testo As String, ByVal Url As String)
		_Text = Testo
		_Url = Url
		_PadreID = PadreID
		_UniqueID = ID
	End Sub
End Class