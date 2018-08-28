<Serializable(), CLSCompliant(True)> Public Class PersonType
	Inherits DomainObject

#Region "Private Fields"
	Private _ID As Integer
	Private _Description As String
	Private _MaxPersonalCommunity As Integer
#End Region

#Region "Public Property"
	Public Property ID() As Integer
		Get
			Return _ID
		End Get
		Set(ByVal value As Integer)
			_ID = value
		End Set
	End Property
	Public Property Description() As String
		Get
			Return _Description
		End Get
		Set(ByVal value As String)
			_Description = value
		End Set
	End Property
	Public Property MaxPersonalCommunity() As Integer
		Get
			Return _MaxPersonalCommunity
		End Get
		Set(ByVal value As Integer)
			_MaxPersonalCommunity = value
		End Set
	End Property
#End Region

	Sub New()
		MyBase.New()
		Me._MaxPersonalCommunity = 0
	End Sub
	Sub New(ByVal iID As Integer)
		MyBase.New()
		Me._ID = 0
		Me._MaxPersonalCommunity = 0
	End Sub
	Sub New(ByVal iID As Integer, ByVal iDescription As String)
		MyBase.New()
		Me._ID = 0
		Me._Description = iDescription
		Me._MaxPersonalCommunity = 0
	End Sub
	Sub New(ByVal iID As Integer, ByVal iDescription As String, ByVal iMaxPersonalCommunity As Integer)
		MyBase.New()
		Me._ID = 0
		Me._Description = iDescription
		Me._MaxPersonalCommunity = iMaxPersonalCommunity
	End Sub
End Class