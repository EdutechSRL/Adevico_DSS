Namespace Forum
	Public Class Forum
		Inherits DomainObjectCommunity

#Region "Private Properties"
		Private _ID As Integer
		Private _Name As String
		Private _Description As String
		Private _DefaultRole As ForumRole
		Private _IsModerated As Boolean
		Private _NumberOfPost As Int64
		Private _NumberOfTopic As Int64
		Private _NumberOfVisits As Int64
		Private _Topics As List(Of ForumTopic)
#End Region

#Region "Public Properties"
		Public Property ID() As Integer
			Get
				Id = _ID
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
		Public Property Topics() As List(Of ForumTopic)
			Get
				Topics = _Topics
			End Get
			Set(ByVal Value As List(Of ForumTopic))
				_Topics = Value
			End Set
		End Property
		Public Property DefaultRole() As ForumRole
			Get
				DefaultRole = _DefaultRole
			End Get
			Set(ByVal Value As ForumRole)
				_DefaultRole = Value
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
		Public Property IsModerated() As Boolean
			Get
				Return _IsModerated
			End Get
			Set(ByVal Value As Boolean)
				_IsModerated = Value
			End Set
		End Property
		Public Property NumberOfPost() As Int64
			Get
				NumberOfPost = _NumberOfPost
			End Get
			Set(ByVal value As Int64)
				_NumberOfPost = value
			End Set
		End Property
		Public Property NumberOfTopic() As Int64
			Get
				NumberOfTopic = _NumberOfTopic
			End Get
			Set(ByVal value As Int64)
				_NumberOfTopic = value
			End Set
		End Property
		Public Property NumberOfVisits() As Int64
			Get
				NumberOfVisits = _NumberOfVisits
			End Get
			Set(ByVal value As Int64)
				_NumberOfVisits = value
			End Set
		End Property
#End Region

#Region "Metodi New"
		Sub New()
			Me._Topics = New List(Of ForumTopic)
		End Sub
#End Region


	End Class


	Public Enum StatusForumPost As Integer
		Approved = 1
		Waiting = 2
		Censored = 3
	End Enum
End Namespace