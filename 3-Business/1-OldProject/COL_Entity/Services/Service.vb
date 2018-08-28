<Serializable(), CLSCompliant(True)> Public Class ServiceBase
	'Inherits DomainObject
	Private _ID As Integer
	Private _Code As String
    Private _PermissionString As String
    Private _PermissionLong As Long
	Private _CommunityID As Integer

	Public Property ID() As Integer
		Get
			ID = _ID
		End Get
		Set(ByVal value As Integer)
			_ID = value
		End Set
	End Property
	Public Property Code() As String
		Get
			Code = _Code
		End Get
		Set(ByVal value As String)
			_Code = value
		End Set
	End Property
	Public Property CommunityID() As Integer
		Get
			CommunityID = _CommunityID
		End Get
		Set(ByVal value As Integer)
			_CommunityID = value
		End Set
	End Property
    Public ReadOnly Property PermissionLong() As Long
        Get
            PermissionLong = _PermissionLong
        End Get
    End Property
    Public Property PermissionString() As String
        Get
            PermissionString = _PermissionString
        End Get
        Set(ByVal value As String)
            _PermissionString = value
            _PermissionLong = Convert.ToInt64(New String(value.Reverse().ToArray()), 2)
        End Set
    End Property
	Public Sub New()
		_PermissionString = "00000000000000000000000000000000"
	End Sub
	Public Sub New(ByVal ID As Integer)
		_ID = ID
		_PermissionString = "00000000000000000000000000000000"
	End Sub
	Public Sub New(ByVal ID As Integer, ByVal Code As String)
		_ID = ID
		_Code = Code
		_PermissionString = "00000000000000000000000000000000"
	End Sub
	Public Sub New(ByVal ID As Integer, ByVal Code As String, ByVal Permessi As String)
		_ID = ID
		_Code = Code
		If Permessi = "" Then
			Permessi = "00000000000000000000000000000000"
		End If
		_PermissionString = Permessi
        _PermissionLong = Convert.ToInt64(New String(_PermissionString.Reverse().ToArray()), 2)
	End Sub
End Class