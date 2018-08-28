Namespace Events
	<Serializable(), CLSCompliant(True)> Public Class CommunityEvent
		Inherits DomainObjectCommunity

#Region "Private Property"
		Private _ID As Integer
		Private _FatherEventID As Integer
		Private _Name As String
		Private _IsMacro As Boolean
		Private _Repeat As Integer
		Private _DateToEndRepeat As DateTime
		Private _Note As String
		Private _Place As String
		Private _Links As String
		Private _Type As New EventType
		Private _AccademicYear As AcademicYear
		Private _IsPerpetuo As Integer
		Private _Items As New List(Of EventItem)
		Private _ExternalPlannerID As String
		Private _Event As CommunityEvent
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
		Public Property FatherEventID() As Integer
			Get
				FatherEventID = _FatherEventID
			End Get
			Set(ByVal Value As Integer)
				_FatherEventID = Value
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
		Public Property IsMacro() As Boolean
			Get
				IsMacro = _IsMacro
			End Get
			Set(ByVal Value As Boolean)
				_IsMacro = Value
			End Set
		End Property
		Public Property Repeat() As Boolean
			Get
				Repeat = _Repeat
			End Get
			Set(ByVal Value As Boolean)
				_Repeat = Value
			End Set
		End Property
		'Public Property DateToEndRepeat() As DateTime
		'	Get
		'		DateToEndRepeat = _DateToEndRepeat
		'	End Get
		'	Set(ByVal Value As DateTime)
		'		_DateToEndRepeat = Value
		'	End Set
		'End Property
		Public Property Note() As String
			Get
				Note = _Note
			End Get
			Set(ByVal Value As String)
				_Note = Value
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
		Public Property Type() As EventType
			Get
				Type = _Type
			End Get
			Set(ByVal Value As EventType)
				_Type = Value
			End Set
		End Property
		Public Property Year() As AcademicYear
			Get
				Year = _AccademicYear
			End Get
			Set(ByVal Value As AcademicYear)
				_AccademicYear = Value
			End Set
		End Property
		Public Property IsPerpetuo() As Boolean
			Get
				IsPerpetuo = _IsPerpetuo
			End Get
			Set(ByVal Value As Boolean)
				_IsPerpetuo = Value
			End Set
		End Property
		Public Property Items() As List(Of EventItem)
			Get
				Items = _Items
			End Get
			Set(ByVal Value As List(Of EventItem))
				_Items = Value
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
		Public Property ReferTo() As CommunityEvent
			Get
				Return _Event
			End Get
			Set(ByVal value As CommunityEvent)
				_Event = value
			End Set
		End Property
#End Region

		Private Sub SetNullValue()
			Me._ExternalPlannerID = ""
			Me._FatherEventID = 0
			Me._IsMacro = False
			Me._IsPerpetuo = False
			Me._Links = ""
			Me._Name = ""
			Me._Note = ""
			Me._Place = ""
			Me._Repeat = False
		End Sub
		Public Sub New()

		End Sub
		Public Sub New(ByVal iID As Integer)
			Me._ID = iID
		End Sub

		Public Shared Function FindByCommunity(ByVal Item As CommunityEvent, ByVal argument As Integer) As Boolean
			Return Item.CommunityOwner.ID = argument
		End Function
		Public Shared Function FindByType(ByVal Item As CommunityEvent, ByVal argument As Integer) As Boolean
			Return Item.Type.ID = argument
		End Function

	End Class
End Namespace