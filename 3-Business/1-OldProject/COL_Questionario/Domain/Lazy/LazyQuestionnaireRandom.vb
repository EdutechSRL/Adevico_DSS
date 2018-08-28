<Serializable()>
Public Class LazyQuestionnaireRandom

#Region "Private"
    Private _Id As Integer
    Private _IdFather As Integer
    Private _CreatedOn As DateTime
    Private _DeletedOn As DateTime?
    Private _IdPerson As Integer
    Private _IdUser As Integer
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
    Public Overridable Property IdFather As Integer
        Get
            Return _IdFather
        End Get
        Set(value As Integer)
            _IdFather = value
        End Set
    End Property
    Public Overridable Property CreatedOn As DateTime
        Get
            Return _CreatedOn
        End Get
        Set(value As DateTime)
            _CreatedOn = value
        End Set
    End Property
    Public Overridable Property DeletedOn As DateTime?
        Get
            Return _DeletedOn
        End Get
        Set(value As DateTime?)
            _DeletedOn = value
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
    Public Overridable Property IdUser As Integer
        Get
            Return _IdUser
        End Get
        Set(value As Integer)
            _IdUser = value
        End Set
    End Property
#End Region

    Sub New()

    End Sub

End Class