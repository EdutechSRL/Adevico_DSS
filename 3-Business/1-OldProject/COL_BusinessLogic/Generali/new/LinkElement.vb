Public Class LinkElement
	Private _Text As String
	Private _Url As String
	Private _ToolTip As String
	Private _UniqueID As Integer
	Private _ImageUrl As String

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
	Public Property ToolTip() As String
		Get
			ToolTip = _ToolTip
		End Get
		Set(ByVal value As String)
			_ToolTip = value
		End Set
	End Property
	Public Property ImageUrl() As String
		Get
			ImageUrl = _ImageUrl
		End Get
		Set(ByVal value As String)
			_ImageUrl = value
		End Set
	End Property

	Public Sub New()
		_Text = String.Empty
		_Url = String.Empty
		_ToolTip = String.Empty
		_ImageUrl = String.Empty
		_UniqueID = 0
	End Sub

	Public Sub New(ByVal Testo As String)
		_Text = Testo
		_Url = String.Empty
		_ToolTip = String.Empty
		_ImageUrl = String.Empty
		_UniqueID = 0
	End Sub
	Public Sub New(ByVal Testo As String, ByVal Url As String)
		_Text = Testo
		_Url = Url
		_ToolTip = String.Empty
		_ImageUrl = String.Empty
		_UniqueID = 0
	End Sub
	Public Sub New(ByVal Testo As String, ByVal Url As String, ByVal ToolTip As String)
		_Text = Testo
		_Url = Url
		_ToolTip = ToolTip
		_ImageUrl = String.Empty
		_UniqueID = 0
	End Sub
	Public Sub New(ByVal Testo As String, ByVal Url As String, ByVal ToolTip As String, ByVal ImageUrl As String)
		_Text = Testo
		_Url = Url
		_ToolTip = ToolTip
		_ImageUrl = ImageUrl
		_UniqueID = 0
	End Sub
End Class
