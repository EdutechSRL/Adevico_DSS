<Serializable()>
Public Class Question

#Region "Private"
    Private _Id As Integer
    Private _Layout As String
    Private _CreatedBy As Integer
    Private _CreatedOn As Date?
    Private _ModifiedBy As Integer
    Private _ModifiedOn As Date?
    Private _isFromLibrary As Boolean
    Private _IdType As Integer
    Private _Count As Integer
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
    Public Overridable Property Layout As String
        Get
            Return _Layout
        End Get
        Set(value As String)
            _Layout = value
        End Set
    End Property
    Public Overridable Property CreatedBy As Integer
        Get
            Return _CreatedBy
        End Get
        Set(value As Integer)
            _CreatedBy = value
        End Set
    End Property
    Public Overridable Property CreatedOn As Date?
        Get
            Return _CreatedOn
        End Get
        Set(value As Date?)
            _CreatedOn = value
        End Set
    End Property
    Public Overridable Property ModifiedBy As Integer
        Get
            Return _ModifiedBy
        End Get
        Set(value As Integer)
            _ModifiedBy = value
        End Set
    End Property
    Public Overridable Property ModifiedOn As Date?
        Get
            Return _ModifiedOn
        End Get
        Set(value As Date?)
            _ModifiedOn = value
        End Set
    End Property
    Public Overridable Property isFromLibrary As Boolean
        Get
            Return _isFromLibrary
        End Get
        Set(value As Boolean)
            _isFromLibrary = value
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
    Public Overridable Property Count As Integer
        Get
            Return _Count
        End Get
        Set(value As Integer)
            _Count = value
        End Set
    End Property
#End Region

    Sub New()

    End Sub

End Class