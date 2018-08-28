
Namespace Events
	<Serializable(), CLSCompliant(True)> Public Class EventItem
		Inherits DomainObject

#Region "Private Property"
		Private _ID As Integer
		Private _StartDate As DateTime
		Private _EndDate As DateTime
		Private _Place As String
		Private _Links As String
		Private _Title As String
		Private _Text As String
		Private _Note As String
		Private _ExternalPlannerID As String
		Private _EventItem As EventItem
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
		Public Property StartDate() As DateTime
			Get
				StartDate = _StartDate
			End Get
			Set(ByVal Value As DateTime)
				_StartDate = Value
			End Set
		End Property
		Public Property EndDate() As DateTime
			Get
				EndDate = _EndDate
			End Get
			Set(ByVal Value As DateTime)
				_EndDate = Value
			End Set
		End Property
		Public Property Title() As String
			Get
				Title = _Title
			End Get
			Set(ByVal Value As String)
				_Title = Value
			End Set
		End Property
		Public Property Text() As String
			Get
				Text = _Text
			End Get
			Set(ByVal Value As String)
				_Text = Value
			End Set
		End Property
		Public Property Place() As String
			Get
				Place = _Place
			End Get
			Set(ByVal Value As String)
				_Place = Value
			End Set
		End Property
		Public Property Links() As String
			Get
				Links = _Links
			End Get
			Set(ByVal Value As String)
				_Links = Value
			End Set
		End Property
		Public Property Note() As String
			Get
				Note = _Note
			End Get
			Set(ByVal Value As String)
				_Note = Value
			End Set
		End Property
		Public Property ExternalPlannerID() As String
			Get
				ExternalPlannerID = _ExternalPlannerID
			End Get
			Set(ByVal Value As String)
				_ExternalPlannerID = Value
			End Set
		End Property
		Public Property ReferTo() As EventItem
			Get
				Return _EventItem
			End Get
			Set(ByVal value As EventItem)
				_EventItem = value
			End Set
		End Property
#End Region

		Private Sub SetNullValue()
			Me._ExternalPlannerID = ""
			Me._Links = ""
			Me._Note = ""
			Me._Place = ""
			Me._Text = ""
			Me._Title = ""
		End Sub

		Public Sub New()
			Me.SetNullValue()
		End Sub
		Public Sub New(ByVal iID As Integer)
			Me.SetNullValue()
			Me._ID = iID
		End Sub

		Public Shared Function FindByID(ByVal item As EventItem, ByVal argument As Integer) As Boolean
			Return item.ID = argument
		End Function
		Public Shared Function FindByExternalPlannerID(ByVal item As EventItem, ByVal argument As String) As Boolean
			Return item.ExternalPlannerID = argument
		End Function
	End Class
End Namespace