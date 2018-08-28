Namespace Events
	<Serializable(), CLSCompliant(True)> Public Class EventType
		Inherits DomainObject

#Region "Private Property"
		Private _ID As Integer
		Private _Name As String
		Private _Image As String
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
		Public Property Image() As String
			Get
				Image = _Image
			End Get
			Set(ByVal Value As String)
				_Image = Value
			End Set
		End Property
#End Region

		Sub New()

		End Sub
		Sub New(ByVal iID As Integer)
			Me._ID = iID
		End Sub
		Sub New(ByVal iID As Integer, ByVal iName As String)
			Me._ID = iID
			Me._Name = iName
		End Sub
	End Class
End Namespace