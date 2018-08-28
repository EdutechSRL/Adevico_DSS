Namespace Forum
	Public Class ForumRole
		Inherits DomainObject

#Region "Private Properties"
		Private _ID As Int64
		Private _Name As String
		Private _Description As String
#End Region

#Region "Public Properties"
		Public Property ID() As Int64
			Get
				ID = _ID
			End Get
			Set(ByVal Value As Int64)
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
		Public Property Description() As String
			Get
				Description = _Description
			End Get
			Set(ByVal Value As String)
				_Description = Value
			End Set
		End Property
#End Region

		Sub New()
			MyBase.New()
		End Sub

		Public Shared Function FindByID(ByVal item As ForumRole, ByVal argument As Int64)
			Return item.ID = argument
		End Function
	End Class
End Namespace
