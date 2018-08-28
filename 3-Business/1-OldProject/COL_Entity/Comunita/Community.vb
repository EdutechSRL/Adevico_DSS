Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints


<Serializable(), CLSCompliant(True)> Public Class Community
	Inherits DomainObject
	Implements IEquatable(Of Community)


#Region "Private Property"
	Private _ID As Integer
	Private _MainFatherID As Integer
	Private _MainFather As Community
	Private _Name As String
	Private _CreatedAt As Nullable(Of DateTime)
	Private _CloseAt As Nullable(Of DateTime)
	Private _StartSubscription As Nullable(Of DateTime)
	Private _EndSubscription As Nullable(Of DateTime)
	Private _Type As New CommunityType
	Private _YearOfCreation As Integer
	Private _Organization As Organization
	Private _ConfirmSubscription As Boolean
	Private _isClosedByAdministration As Boolean
	'Private _isArchiviata As Boolean
	Private _MaxSubscription As Integer
	Private _OverMaxSubscription As Integer
	Private _AllowSubscription As Boolean
	Private _AllowUnsubscription As Boolean
	Private _FreeAccess As Boolean
	Private _AccessForPrinting As Boolean
	Private _Creator As Person
	Private _Responsible As Person
	Private _Description As String
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
	Public Property MainFatherID() As Integer
		Get
			MainFatherID = _MainFatherID
		End Get
		Set(ByVal Value As Integer)
			_MainFatherID = Value
		End Set
	End Property
	Public Property MainFather() As Community
		Get
			MainFather = _MainFather
		End Get
		Set(ByVal Value As Community)
			_MainFather = Value
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
	<Nullable(True)> Public Property CloseAt() As Nullable(Of DateTime)
		Get
			Return _CloseAt
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_CloseAt = value
		End Set
	End Property
	<Nullable(True)> Public Property StartSubscription() As Nullable(Of DateTime)
		Get
			Return _StartSubscription
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_StartSubscription = value
		End Set
	End Property
	<Nullable(True)> Public Property EndSubscription() As Nullable(Of DateTime)
		Get
			Return _EndSubscription
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_EndSubscription = value
		End Set
	End Property
	Public Property Type() As CommunityType
		Get
			Type = _Type
		End Get
		Set(ByVal Value As CommunityType)
			_Type = Value
		End Set
	End Property
	Public Property YearOfCreation() As Integer
		Get
			YearOfCreation = _YearOfCreation
		End Get
		Set(ByVal Value As Integer)
			_YearOfCreation = Value
		End Set
	End Property
	Public Property Organization() As Organization
		Get
			Organization = _Organization
		End Get
		Set(ByVal Value As Organization)
			_Organization = Value
		End Set
	End Property
	Public Property ConfirmSubscription() As Boolean
		Get
			ConfirmSubscription = _ConfirmSubscription
		End Get
		Set(ByVal Value As Boolean)
			_ConfirmSubscription = Value
		End Set
	End Property
	Public Property isClosedByAdministration() As Boolean
		Get
			isClosedByAdministration = _isClosedByAdministration
		End Get
		Set(ByVal Value As Boolean)
			_isClosedByAdministration = Value
		End Set
	End Property
	'Public Property isArchiviata() As Boolean
	'	Get
	'		isArchiviata = _isArchiviata
	'	End Get
	'	Set(ByVal Value As Boolean)
	'		_isArchiviata = Value
	'	End Set
	'End Property
	Public Property MaxSubscription() As Integer
		Get
			_MaxSubscription = _MaxSubscription
		End Get
		Set(ByVal Value As Integer)
			_MaxSubscription = Value
		End Set
	End Property
	Public Property OverMaxSubscription() As Integer
		Get
			OverMaxSubscription = _OverMaxSubscription
		End Get
		Set(ByVal Value As Integer)
			_OverMaxSubscription = Value
		End Set
	End Property
	Public Property AllowSubscription() As Boolean
		Get
			AllowSubscription = _AllowSubscription
		End Get
		Set(ByVal Value As Boolean)
			_AllowSubscription = Value
		End Set
	End Property
	Public Property AllowUnsubscription() As Boolean
		Get
			AllowUnsubscription = _AllowUnsubscription
		End Get
		Set(ByVal Value As Boolean)
			_AllowUnsubscription = Value
		End Set
	End Property
	Public Property FreeAccess() As Boolean
		Get
			FreeAccess = _FreeAccess
		End Get
		Set(ByVal Value As Boolean)
			_FreeAccess = Value
		End Set
	End Property
	Public Property AccessForPrinting() As Boolean
		Get
			AccessForPrinting = _AccessForPrinting
		End Get
		Set(ByVal Value As Boolean)
			_AccessForPrinting = Value
		End Set
	End Property
	Public Property Creator() As Person
		Get
			Creator = _Creator
		End Get
		Set(ByVal Value As Person)
			_Creator = Value
		End Set
	End Property
	Public Property Responsible() As Person
		Get
			Responsible = _Responsible
		End Get
		Set(ByVal Value As Person)
			_Responsible = Value
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

	ReadOnly Property Status() As CommunityStatus
		Get
			If Me._isClosedByAdministration Then
				Return CommunityStatus.ClosedByAdministration
			ElseIf MyBase.IsArchive Then
				Return CommunityStatus.Archiviata
			Else
				Return CommunityStatus.OnlyActivated
			End If
		End Get
	End Property
#End Region

#Region "Metodi New"
	Private Sub SetNullValue()
		Me._AccessForPrinting = True
		Me._AllowSubscription = True
		Me._AllowUnsubscription = True
		Me._ConfirmSubscription = False
		Me._EndSubscription = Nothing
		Me._FreeAccess = False
		MyBase.IsArchive = False
		Me._isClosedByAdministration = False
		Me._MainFather = Nothing
		Me._MainFatherID = 0
		Me._Creator = New Person
		Me._Responsible = New Person
		Me._Type = New CommunityType
	End Sub

	Public Sub New()
		Me.SetNullValue()
	End Sub
	Public Sub New(ByVal iCommunityID As Integer)
		Me.SetNullValue()
		Me._ID = iCommunityID
	End Sub
	Public Sub New(ByVal iCommunityID As Integer, ByVal iTypeID As Integer)
		Me.SetNullValue()
		Me._ID = iCommunityID
		Me._Type.ID = iTypeID
	End Sub
	Public Sub New(ByVal iCommunityID As Integer, ByVal iType As CommunityType)
		Me.SetNullValue()
		Me._ID = iCommunityID
		Me._Type = iType
	End Sub
	Public Sub New(ByVal iCommunityID As Integer, ByVal Name As String, ByVal iFatherID As Integer)
		Me.SetNullValue()
		Me._ID = iCommunityID
		Me._Name = Name
		Me._MainFatherID = iFatherID
		Me._MainFather = New Community(iFatherID)
	End Sub
	Public Sub New(ByVal iCommunityID As Integer, ByVal Name As String, ByVal iFather As Community)
		Me.SetNullValue()
		Me._ID = iCommunityID
		Me._Name = Name
		Me._MainFatherID = iFather.ID
		Me._MainFather = iFather
	End Sub
	Public Sub New(ByVal iCommunityID As Integer, ByVal Name As String, ByVal iOrganization As Organization, ByVal iType As CommunityType)
		Me.SetNullValue()
		Me._ID = iCommunityID
		Me._Name = Name
		Me._Organization = iOrganization
		Me._Type = iType
    End Sub
    Public Sub New(ByVal iCommunityID As Integer, ByVal Name As String, ByVal iFatherID As Integer, ByVal iOrganization As Organization)
        Me.SetNullValue()
        Me._ID = iCommunityID
        Me._Name = Name
        Me._Organization = iOrganization
        Me._MainFatherID = iFatherID
        Me._MainFather = New Community(iFatherID)
    End Sub
	Public Sub New(ByVal iCommunityID As Integer, ByVal Name As String, ByVal iFather As Community, ByVal iOrganization As Organization, ByVal iType As CommunityType)
		Me.SetNullValue()
		Me._ID = iCommunityID
		Me._Name = Name
		Me._Organization = iOrganization
		Me._Type = iType
		Me._MainFatherID = iFather.ID
		Me._MainFather = iFather
	End Sub
#End Region

	Public Shared Function FindByCommunityStatus(ByVal item As Community, ByVal argument As CommunityStatus) As Boolean
		Select Case argument
			Case CommunityStatus.All
				Return True
			Case CommunityStatus.Archiviata
				Return item.IsArchive And Not item.isClosedByAdministration
			Case CommunityStatus.ClosedByAdministration
				Return item.isClosedByAdministration
			Case CommunityStatus.None
				Return False
			Case CommunityStatus.OnlyActivated
				Return item.IsArchive = False And item.isClosedByAdministration = False
		End Select
	End Function

	Public Shared Function FindByNameStartWith(ByVal item As Community, ByVal argument As String) As Boolean
		Return item.Name.StartsWith(argument, StringComparison.OrdinalIgnoreCase)
	End Function
	Public Shared Function FindByNameContains(ByVal item As Community, ByVal argument As String) As Boolean
		Return LCase(item.Name).Contains(LCase(argument))
	End Function
	Public Shared Function FindByCreatedBeforeData(ByVal item As Community, ByVal argument As DateTime) As Boolean
		Return item.CreatedAt <= argument
	End Function
	Public Shared Function FindByCreatedAfterData(ByVal item As Community, ByVal argument As DateTime) As Boolean
		Return item.CreatedAt >= argument
	End Function
	Public Shared Function FindBySubscriptionBeforeData(ByVal item As Community, ByVal argument As DateTime) As Boolean
		Return item.StartSubscription <= argument
	End Function
	Public Shared Function FindBySubscriptionAfterData(ByVal item As Community, ByVal argument As DateTime) As Boolean
		Return item.StartSubscription >= argument
	End Function
	Public Shared Function FindByResponsible(ByVal item As Community, ByVal argument As Integer) As Boolean
		Return item.Responsible.ID = argument
	End Function
	Public Shared Function FindBySurnameResponsible(ByVal item As Community, ByVal argument As String) As Boolean
		Return item.Responsible.Surname.StartsWith(argument)
	End Function

	'Public Function isEqual(ByVal other As Community) As Boolean Implements System.IEquatable(Of Community).Equals
	'	If Me._ID = other.ID Then
	'		Return True
	'	Else
	'		Return False
	'	End If
	'End Function

	Public Overloads Function Equals(ByVal other As Community) As Boolean Implements System.IEquatable(Of Community).Equals
		Return Me.ID = other.ID
	End Function
End Class