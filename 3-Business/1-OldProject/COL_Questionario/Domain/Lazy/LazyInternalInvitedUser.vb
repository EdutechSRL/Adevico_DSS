<Serializable()>
Public Class LazyInternalInvitedUser : Inherits LazyInvitedUser


#Region "Private"
    Private _IdPerson As Integer
#End Region

#Region "Public"
    Public Overridable Property IdPerson As Integer
        Get
            Return _IdPerson
        End Get
        Set(value As Integer)
            _IdPerson = value
        End Set
    End Property
#End Region

    Sub New()

    End Sub
End Class