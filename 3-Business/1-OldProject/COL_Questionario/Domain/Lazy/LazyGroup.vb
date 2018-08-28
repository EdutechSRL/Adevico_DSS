<Serializable()>
Public Class LazyGroup

#Region "Private"
    Private _Id As Integer
    Private _IdPerson As Integer
    Private _isPublic As Boolean
    Private _isShared As Boolean
    Private _isDeleted As Boolean
    Private _IdType As Integer
    Private _IdCommunity As Integer
#End Region

#Region "Public"
    Public Overridable Property Id As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property
    Public Overridable Property IdPerson As Integer
        Get
            Return _IdPerson
        End Get
        Set(value As Integer)
            _IdPerson = value
        End Set
    End Property
    Public Overridable Property IsPublic As Boolean
        Get
            Return _isPublic
        End Get
        Set(value As Boolean)
            _isPublic = value
        End Set
    End Property
    Public Overridable Property IsShared As Boolean
        Get
            Return _isShared
        End Get
        Set(value As Boolean)
            _isShared = value
        End Set

    End Property
    Public Overridable Property IsDeleted As Boolean
        Get
            Return _isDeleted
        End Get
        Set(value As Boolean)
            _isDeleted = value
        End Set
    End Property
    Public Overridable Property IdType As Integer
        Get
            Return _IdType
        End Get
        Set(value As Integer)
            _IdType = value
        End Set
    End Property
    Public Property IdCommunity() As Integer
        Get
            Return _IdCommunity
        End Get
        Set(ByVal value As Integer)
            _IdCommunity = value
        End Set
    End Property

#End Region

    Sub New()

    End Sub

End Class