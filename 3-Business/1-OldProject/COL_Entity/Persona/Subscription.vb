Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints

<Serializable(), CLSCompliant(True)> Public Enum SubscriptionStatus
	None = 0
	NewSubscription = 1
	WaitingForActivation = 2
	Disabled = 4
    Responsible = 8 'boolean aggiunto a membershipContactInfo, serve anche qui?
    Active = 16
End Enum

<Serializable(), CLSCompliant(True)> Public Class Subscription
	Inherits DomainObject

#Region "Private Property"
	Private _ID As Integer
	Private _Person As New Person
	Private _Role As New Role
	Private _Community As New Community
	Private _SubscriptionData As Nullable(Of DateTime)
	Private _IsActivated As Boolean
	Private _IsEnabled As Boolean
	Private _IsResponsible As Boolean
	Private _SMSnotificationEnabled As Boolean
	Private _SkipCover As Boolean
	Private _AccessTime As Nullable(Of DateTime)
	Private _PreviousAccessTime As Nullable(Of DateTime)
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
	Public Property MemberRole() As Role
		Get
			Return _Role
		End Get
		Set(ByVal value As Role)
			_Role = value
		End Set
	End Property
	Public ReadOnly Property CommunitySubscriptedID() As Integer
		Get
			If _Community Is Nothing Then
				Return -1
			Else
				Return _Community.ID
			End If
		End Get
	End Property
	Public Property CommunitySubscripted() As Community
		Get
			Return _Community
		End Get
		Set(ByVal value As Community)
			_Community = value
		End Set

	End Property
	Public Property Member() As Person
		Get
			Return _Person
		End Get
		Set(ByVal value As Person)
			_Person = value
		End Set

	End Property
	<Nullable(True)> Public Property SubscriptionData() As Nullable(Of DateTime)
		Get
			Return _SubscriptionData
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_SubscriptionData = value
		End Set
	End Property
	Public Property IsEnabled() As Boolean
		Get
			Return _IsEnabled
		End Get
		Set(ByVal value As Boolean)
			_IsEnabled = value
		End Set
	End Property
	Public Property IsActivated() As Boolean
		Get
			Return _IsActivated
		End Get
		Set(ByVal value As Boolean)
			_IsActivated = value
		End Set
	End Property
	<Nullable(True)> Public Property AccessTime() As Nullable(Of DateTime)
		Get
			Return _AccessTime
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_AccessTime = value
		End Set
	End Property
	<Nullable(True)> Public Property PreviousAccessTime() As Nullable(Of DateTime)
		Get
			Return _PreviousAccessTime
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_PreviousAccessTime = value
		End Set
	End Property
	Public Property SkipCover() As Boolean
		Get
			Return _SkipCover
		End Get
		Set(ByVal value As Boolean)
			_SkipCover = value
		End Set
	End Property
	Public Property SMSnotificationEnabled() As Boolean
		Get
			Return _SMSnotificationEnabled
		End Get
		Set(ByVal value As Boolean)
			_SMSnotificationEnabled = value
		End Set
	End Property
	Public Property IsResponsible() As Boolean
		Get
			Return _IsResponsible
		End Get
		Set(ByVal value As Boolean)
			_IsResponsible = value
		End Set
	End Property
	Public ReadOnly Property Status() As SubscriptionStatus
		Get
			If Me._IsActivated = False And Me._IsEnabled = False Then
				Return SubscriptionStatus.WaitingForActivation
			ElseIf Not Me.IsEnabled Then
				Return SubscriptionStatus.Disabled
			End If
		End Get
	End Property
#End Region

#Region "New"
    Sub New()

    End Sub
	Sub New(ByVal iPerson As Person)
		Me._Person = iPerson
		'me._SkipCover=
		'Me._SMSnotificationEnabled = iPerson.ski
	End Sub
	Sub New(ByVal iPerson As Person, ByVal iCommunity As Community)
		Me._Person = iPerson
		Me._Community = iCommunity
		'me._SkipCover=
		'Me._SMSnotificationEnabled = iPerson.ski
	End Sub
	Sub New(ByVal iPerson As Person, ByVal iCommunity As Community, ByVal iRole As Role)
		Me._Person = iPerson
		Me._Community = iCommunity
		Me._Role = iRole
		'me._SkipCover=
		'Me._SMSnotificationEnabled = iPerson.ski
	End Sub
	Sub New(ByVal iPerson As Person, ByVal iCommunity As Community, ByVal iRole As Role, ByVal iIsActivated As Boolean, ByVal iIsEnabled As Boolean)
		Me._Person = iPerson
		Me._Community = iCommunity
		Me._Role = iRole
		Me._IsActivated = iIsActivated
		Me._IsEnabled = iIsEnabled
		'me._SkipCover=
		'Me._SMSnotificationEnabled = iPerson.ski
	End Sub
	Sub New(ByVal iPerson As Person, ByVal iCommunity As Community, ByVal iRole As Role, ByVal iIsActivated As Boolean, ByVal iIsEnabled As Boolean, ByVal iIsResponsible As Boolean)
		Me._Person = iPerson
		Me._Community = iCommunity
		Me._Role = iRole
		Me._IsActivated = iIsActivated
		Me._IsEnabled = iIsEnabled
		Me.IsResponsible = iIsResponsible
		'me._SkipCover=
		'Me._SMSnotificationEnabled = iPerson.ski
	End Sub
#End Region

	Public Shared Function FindByOrganization(ByVal item As Subscription, ByVal argument As Integer) As Boolean
		If TypeOf item.CommunitySubscripted Is Organization Then
			Return DirectCast(item.CommunitySubscripted, Organization).OrganizationID = argument
		ElseIf item.CommunitySubscripted.Organization Is Nothing Then
			Return False
		Else
			Return item.CommunitySubscripted.Organization.OrganizationID = argument
		End If
	End Function
	Public Shared Function FindByType(ByVal item As Subscription, ByVal argument As Integer) As Boolean
		If argument < 0 Then
			Return True
		Else
			Return item.CommunitySubscripted.Type.ID = argument
		End If
	End Function
	
	
	Public Shared Function FindByCommunityStatus(ByVal item As Subscription, ByVal argument As CommunityStatus) As Boolean
		Return Community.FindByCommunityStatus(item.CommunitySubscripted, argument)
	End Function

	Public Shared Function FindByNameStartWith(ByVal item As Subscription, ByVal argument As String) As Boolean
		Return Community.FindByNameStartWith(item.CommunitySubscripted, argument)
	End Function
	Public Shared Function FindByNameContains(ByVal item As Subscription, ByVal argument As String) As Boolean
		Return Community.FindByNameContains(item.CommunitySubscripted, argument)
	End Function
	Public Shared Function FindByCreatedBeforeData(ByVal item As Subscription, ByVal argument As DateTime) As Boolean
		Return Community.FindByCreatedBeforeData(item.CommunitySubscripted, argument)
	End Function
	Public Shared Function FindByCreatedAfterData(ByVal item As Subscription, ByVal argument As DateTime) As Boolean
		Return Community.FindByCreatedAfterData(item.CommunitySubscripted, argument)
	End Function
	Public Shared Function FindBySubscriptionBeforeData(ByVal item As Subscription, ByVal argument As DateTime) As Boolean
		Return Community.FindBySubscriptionBeforeData(item.CommunitySubscripted, argument)
	End Function
	Public Shared Function FindBySubscriptionAfterData(ByVal item As Subscription, ByVal argument As DateTime) As Boolean
		Return Community.FindBySubscriptionAfterData(item.CommunitySubscripted, argument)
	End Function
	Public Shared Function FindByResponsible(ByVal item As Subscription, ByVal argument As Integer) As Boolean
		Return Community.FindByResponsible(item.CommunitySubscripted, argument)
	End Function
	Public Shared Function FindBySurnameResponsible(ByVal item As Subscription, ByVal argument As String) As Boolean
		Return Community.FindBySurnameResponsible(item.CommunitySubscripted, argument)
	End Function
End Class

'<Serializable()> Public Class KeyComunitaPersona
'	Private _comunita As Comunita
'	Private _persona As Persona

'	Public Property Comunita() As Comunita
'		Get
'			Return _comunita
'		End Get
'		Set(ByVal value As Comunita)
'			_comunita = value
'		End Set
'	End Property

'	Public Property Persona() As Persona
'		Get
'			Return _persona
'		End Get
'		Set(ByVal value As Persona)
'			_persona = value
'		End Set
'	End Property

'	Public Overrides Function Equals(ByVal obj As Object) As Boolean
'		Dim objKey As KeyComunitaPersona
'		objKey = CType(obj, KeyComunitaPersona)
'		If objKey Is Nothing Then
'			Return False
'		End If

'		Return (Me.Comunita.ID = objKey.Comunita.ID) And (Me.Persona.ID = objKey.Persona.ID)
'	End Function

'	Public Overrides Function GetHashCode() As Integer
'		Return Comunita.ID.GetHashCode ^ Persona.ID.GetHashCode
'	End Function


'End Class