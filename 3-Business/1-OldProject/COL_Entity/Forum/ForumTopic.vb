Namespace Forum
	Public Class ForumTopic
		Inherits DomainObjectCommunity

#Region "Private Properties"
		Private _ID As Int64
		Private _NumberOfPost As Int64
		Private _NumberOfVisits As Int64
		Private _Isvisible As Boolean
		Private _Posts As List(Of ForumPost)
		Private _StartPost As ForumPost
		Private _Forum As Forum
		Private _Status As StatusForumPost
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
		Public Property NumberOfPost() As Int64
			Get
				NumberOfPost = _NumberOfPost
			End Get
			Set(ByVal value As Int64)
				_NumberOfPost = value
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
		Public Property Subject() As String
			Get
				Subject = _StartPost.Subject
			End Get
			Set(ByVal Value As String)
				_StartPost.Subject = Value
			End Set
		End Property
		Public Property Description() As String
			Get
				Description = _StartPost.Description
			End Get
			Set(ByVal Value As String)
				_StartPost.Description = Value
			End Set
		End Property
		Public Property Posts() As List(Of ForumPost)
			Get
				Posts = _Posts
			End Get
			Set(ByVal Value As List(Of ForumPost))
				_Posts = Value
			End Set
		End Property
		Public Property ForumOwner() As Forum
			Get
				ForumOwner = _Forum
			End Get
			Set(ByVal Value As Forum)
				_Forum = Value
			End Set
		End Property
		Public Property Status() As StatusForumPost
			Get
				Status = _StartPost.Status
			End Get
			Set(ByVal Value As StatusForumPost)
				_StartPost.Status = Value
			End Set
		End Property
#End Region

#Region "Metodi New"
		Sub New()
			MyBase.New()
			Me._Posts = New List(Of ForumPost)
			Me._StartPost = New ForumPost
		End Sub
#End Region

	End Class
End Namespace