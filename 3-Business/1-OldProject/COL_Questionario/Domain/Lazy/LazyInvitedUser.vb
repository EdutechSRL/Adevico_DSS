<Serializable()>
Public Class LazyInvitedUser

#Region "Private"
    Private _IdQuestionnnaire As Integer
    Private _IdPerson As Integer
    Private _Id As Integer
    Private _Mail As String
    Private _Description As String
    Private _MailSent As Integer
    Private _Password As String
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
    Public Overridable Property IdQuestionnnaire As Integer
        Get
            Return _IdQuestionnnaire
        End Get
        Set(value As Integer)
            _IdQuestionnnaire = value
        End Set
    End Property
    Public Overridable Property Mail As String
        Get
            Return _Mail
        End Get
        Set(value As String)
            _Mail = value
        End Set
    End Property
    Public Overridable Property Description As String
        Get
            Return _Description
        End Get
        Set(value As String)
            _Description = value
        End Set
    End Property
    Public Overridable Property MailSent As Integer
        Get
            Return _MailSent
        End Get
        Set(value As Integer)
            _MailSent = value
        End Set
    End Property
    Public Overridable Property Password As String
        Get
            Return _Password
        End Get
        Set(value As String)
            _Password = value
        End Set
    End Property

#End Region

    Sub New()

    End Sub

End Class