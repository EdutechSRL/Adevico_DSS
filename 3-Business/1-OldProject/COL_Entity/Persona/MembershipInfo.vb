<Serializable(), CLSCompliant(True)> Public Class MembershipInfo
	Inherits DomainObject
	Private _MemberID As Integer
    Private _MemberRole As Role
    Private _Status As SubscriptionStatus
    Private _isResponsible As Boolean

	Public Property MemberID() As Integer
		Get
			Return _MemberID
		End Get
		Set(ByVal value As Integer)
			_MemberID = value
		End Set
	End Property
    Public Property MemberRole() As Role
        Get
            Return _MemberRole
        End Get
        Set(ByVal value As Role)
            _MemberRole = value
        End Set
    End Property
	Public Property Status() As SubscriptionStatus
		Get
			Return _Status
		End Get
		Set(ByVal value As SubscriptionStatus)
			_Status = value
		End Set
	End Property

    Public Property isResponsible()
        Get
            Return _isResponsible
        End Get
        Set(ByVal value)
            _isResponsible = value
        End Set
    End Property

	Sub New()

	End Sub
    Sub New(ByVal pMemberID As Integer, ByVal pRole As Role, ByVal pStatus As SubscriptionStatus)
        _MemberID = pMemberID
        _MemberRole = pRole
        _Status = pStatus
    End Sub
End Class