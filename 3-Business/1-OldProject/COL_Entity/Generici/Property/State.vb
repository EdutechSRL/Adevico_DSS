<Serializable(), CLSCompliant(True)> Public Class State
	Inherits DomainObject

#Region "Private Property"
	Private _ID As Integer
	Private _Name As String
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
#End Region

#Region "Metodi New"
	Sub New()
		MyBase.New()
	End Sub
	Sub New(ByVal iID As Integer, ByVal iName As String)
		MyBase.New()
		Me._ID = iID
		Me._Name = iName
	End Sub
#End Region

	Public Shared Function FindByID(ByVal item As State, ByVal argument As Integer) As Boolean
		Return item.ID = argument
	End Function
	Public Shared Function FindByName(ByVal item As State, ByVal argument As String) As Boolean
		Return item.Name = argument
	End Function

End Class