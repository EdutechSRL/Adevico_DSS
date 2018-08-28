
<Serializable(), CLSCompliant(True)> Public Class MemberContact
    Inherits DomainObject
    Implements IComparable

#Region "Private property"
    Private _ID As Integer
    Private _Name As String
    Private _Surname As String
    Private _Mail As String
    Private _MembershipInfo As New List(Of MembershipInfo)
    'Private _Selected As Boolean
    Private _RegistrationCode As String
    Private _Login As String
#End Region

#Region "Public property"

    Public Property Id() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
    Public Property Surname() As String
        Get
            Return _Surname
        End Get
        Set(ByVal value As String)
            _Surname = value
        End Set
    End Property
    Public Property Mail() As String
        Get
            Return _Mail
        End Get
        Set(ByVal value As String)
            _Mail = value
        End Set
    End Property
    Public Property MembershipInfo() As List(Of MembershipInfo)
        Get
            Return _MembershipInfo
        End Get
        Set(ByVal value As List(Of MembershipInfo))
            _MembershipInfo = value
        End Set
    End Property
    'Public Property Selected() As Boolean
    '    Get
    '        Return _Selected
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _Selected = value
    '    End Set
    'End Property
    Public ReadOnly Property FullName()
        Get
            Return Surname & " " & Name
        End Get
    End Property
    Public Property RegistrationCode() As String
        Get
            Return _RegistrationCode
        End Get
        Set(ByVal value As String)
            _RegistrationCode = value
        End Set
    End Property
    Public Property Login() As String
        Get
            Return _Login
        End Get
        Set(ByVal value As String)
            _Login = value
        End Set
    End Property

    Public ReadOnly Property Roles() As String
        Get
            Dim concatRoles As String = String.Empty
            For Each oMembershipInfo As MembershipInfo In _MembershipInfo
                concatRoles &= oMembershipInfo.MemberRole.ID & ", "
            Next
            Return concatRoles.TrimEnd(",")
        End Get
    End Property

    Public Shared Function FindByID(ByVal item As MemberContact, ByVal argument As Integer) As Boolean
        Return item.Id = argument
    End Function
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Dim objContatto As MemberContact
        objContatto = CType(obj, MemberContact)
        Me.Id.CompareTo(objContatto.Id)
    End Function

#End Region

End Class



