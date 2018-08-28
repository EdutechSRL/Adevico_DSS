<Serializable(), CLSCompliant(True)> Public Class Organization
	Inherits Community

#Region "Private property"
	Private _isFaculty As Boolean
	Private _LogoPath As String
	Private _Address As PostalAddress
	Private _Contact As Contact
	Private _OrganizationID As Integer
#End Region

#Region "Private property"
	Public Property OrganizationID() As Integer
		Get
			OrganizationID = _OrganizationID
		End Get
		Set(ByVal Value As Integer)
			_OrganizationID = Value
		End Set
	End Property
	Public Property isFaculty() As Boolean
		Get
			isFaculty = _isFaculty
		End Get
		Set(ByVal Value As Boolean)
			_isFaculty = Value
		End Set
	End Property
	Public Property LogoPath() As String
		Get
			LogoPath = _LogoPath
		End Get
		Set(ByVal Value As String)
			_LogoPath = Value
		End Set
	End Property
	Public Property Address() As PostalAddress
		Get
			Address = _Address
		End Get
		Set(ByVal Value As PostalAddress)
			_Address = Value
		End Set
	End Property
	Public Property VirtualContact() As Contact
		Get
			VirtualContact = _Contact
		End Get
		Set(ByVal Value As Contact)
			_Contact = Value
		End Set
	End Property
#End Region

	Private Sub SetNullValue(ByVal isLazy As Boolean)
		If Not isLazy Then
			_Contact = New Contact
			_Address = New PostalAddress
		End If
	End Sub

	Sub New(Optional ByVal isLazy As Boolean = True)
		MyBase.New()
		SetNullValue(isLazy)
	End Sub
	Sub New(ByVal iOrganizationID As Integer, Optional ByVal isLazy As Boolean = True)
		MyBase.New()
		SetNullValue(isLazy)
		Me._OrganizationID = iOrganizationID
	End Sub
	Sub New(ByVal iOrganizationID As Integer, ByVal iName As String, Optional ByVal isLazy As Boolean = True)
		MyBase.New()
		SetNullValue(isLazy)
		Me._OrganizationID = iOrganizationID
		MyBase.Name = iName
	End Sub
	Sub New(ByVal iID As Integer, ByVal iType As CommunityType, ByVal iOrganizationID As Integer, Optional ByVal isLazy As Boolean = True)
		MyBase.New()
		MyBase.ID = iID
		MyBase.Type = iType
		SetNullValue(isLazy)
		Me._OrganizationID = iOrganizationID
	End Sub
	Sub New(ByVal iID As Integer, ByVal iType As CommunityType, ByVal iOrganizationID As Integer, ByVal iName As String, Optional ByVal isLazy As Boolean = True)
		MyBase.New()
		MyBase.ID = iID
		MyBase.Type = iType
		SetNullValue(isLazy)
		Me._OrganizationID = iOrganizationID
		MyBase.Name = iName
	End Sub

	Public Shared Function FindByOrganizationID(ByVal item As Organization, ByVal argument As Integer) As Boolean
		Return item.OrganizationID = argument
	End Function

End Class