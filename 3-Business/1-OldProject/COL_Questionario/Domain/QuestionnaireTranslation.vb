<Serializable()>
Public Class QuestionnaireTranslation

#Region "Private"
    Private _Id As Integer
    Private _IdQuestionnnaire As Integer
    Private _Name As String
    Private _Description As String
    Private _IsDeleted As Boolean
    Private _Url As String
    Private _IsDefault As Boolean
    Private _IdLanguage As Integer
    Private _IsBlocked As Boolean
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
    Public Overridable Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
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
    Public Overridable Property IsDeleted As Boolean
        Get
            Return _IsDeleted
        End Get
        Set(value As Boolean)
            _IsDeleted = value
        End Set
    End Property
    Public Overridable Property IsDefault As Boolean
        Get
            Return _IsDefault
        End Get
        Set(value As Boolean)
            _IsDefault = value
        End Set
    End Property
    Public Overridable Property IsBlocked As Boolean
        Get
            Return _IsBlocked
        End Get
        Set(value As Boolean)
            _IsBlocked = value
        End Set
    End Property
    Public Overridable Property IdLanguage As Integer
        Get
            Return _IdLanguage
        End Get
        Set(value As Integer)
            _IdLanguage = value
        End Set
    End Property
    Public Overridable Property Url As String
        Get
            Return _Url
        End Get
        Set(value As String)
            _Url = value
        End Set
    End Property
#End Region


    Sub New()

    End Sub

End Class