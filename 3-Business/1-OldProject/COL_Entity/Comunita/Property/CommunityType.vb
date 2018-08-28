<Serializable(), CLSCompliant(True)> Public Class CommunityType
	Inherits DomainObject

#Region "Private Property"
	Private _ID As Integer
	Private _Name As String
	Private _Image As String
	Private _isVisible As Boolean
#End Region

#Region "Public Property"
	Public Property ID() As Integer
		Get
			ID = _ID
		End Get
		Set(ByVal Value As Integer)
			_ID = Value
		End Set
	End Property
	Public Property Name() As String
		Get
			Name = _Name
		End Get
		Set(ByVal Value As String)
			_Name = Value
		End Set
	End Property
	Public Property FileImage() As String
		Get
			FileImage = _Image
		End Get
		Set(ByVal Value As String)
			_Image = Value
		End Set
	End Property
	Public Property isVisibile() As Boolean
		Get
			isVisibile = _isVisible
		End Get
		Set(ByVal Value As Boolean)
			_isVisible = Value
		End Set
	End Property
#End Region

#Region "Metodi New"
	Sub New()
		Me._isVisible = 1
	End Sub
	Sub New(ByVal ID As Integer)
		Me._isVisible = True
		Me._ID = ID
	End Sub
	Sub New(ByVal ID As Integer, ByVal nome As String)
		Me._isVisible = True
		Me._ID = ID
		Me._Name = nome
	End Sub
	Sub New(ByVal ID As Integer, ByVal nome As String, ByVal icona As String)
		Me._isVisible = True
		Me._ID = ID
		Me._Name = nome
		Me._Image = icona
	End Sub
	Sub New(ByVal ID As Integer, ByVal nome As String, ByVal icona As String, ByVal Visibile As Boolean)
		Me._isVisible = 1
		Me._ID = ID
		Me._Name = nome
		Me.isVisibile = Visibile
	End Sub
#End Region

	Public Shared Function FindByVisibility(ByVal item As CommunityType, ByVal argument As Boolean) As Boolean
		Return (argument = item.isVisibile)
	End Function
	Public Shared Function FindByID(ByVal item As CommunityType, ByVal argument As Integer) As Boolean
		Return (argument = item.ID)
	End Function
End Class