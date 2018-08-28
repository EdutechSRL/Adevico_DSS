<Serializable()>
Public Class LazyExternalInvitedUser : Inherits LazyInvitedUser


#Region "Private"
    Private _Name As String
    Private _Surname As String
#End Region

#Region "Public"
    Public Overridable Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property
    Public Overridable Property Surname As String
        Get
            Return _Surname
        End Get
        Set(value As String)
            _Surname = value
        End Set
    End Property

#End Region

    Sub New()

    End Sub
End Class