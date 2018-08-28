Namespace Forum
	Public Class ForumPost
		Inherits DomainObject

#Region "Private Property"
		Private _ID As Int64
		Private _Subject As String
		Private _Description As String
		Private _Forum As Forum
		Private _ForumTopic As ForumTopic
		Private _PostReferrer As ForumPost
		Private _Status As StatusForumPost
#End Region

#Region "Public Property"
		Public Property ID() As Int64
			Get
				ID = _ID
			End Get
			Set(ByVal Value As Int64)
				_ID = Value
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
		Public Property TopicOwner() As ForumTopic
			Get
				TopicOwner = _ForumTopic
			End Get
			Set(ByVal Value As ForumTopic)
				_ForumTopic = Value
			End Set
		End Property
		Public Property PostReferrer() As ForumPost
			Get
				PostReferrer = _PostReferrer
			End Get
			Set(ByVal Value As ForumPost)
				_PostReferrer = Value
			End Set
		End Property
		Public Property Subject() As String
			Get
				Subject = _Subject
			End Get
			Set(ByVal Value As String)
				_Subject = Value
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
		Public Property Status() As StatusForumPost
			Get
				Status = _Status
			End Get
			Set(ByVal Value As StatusForumPost)
				_Status = Value
			End Set
		End Property
#End Region

		Sub New()
			MyBase.New()
			Me._Status = StatusForumPost.Approved
		End Sub

	End Class
End Namespace