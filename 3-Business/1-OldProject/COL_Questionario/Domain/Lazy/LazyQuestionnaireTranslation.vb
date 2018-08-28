<Serializable()>
Public Class LazyQuestionnaireTranslation

#Region "Private"
    Private _Id As Integer
    Private _IdQuestionnnaire As Integer
    Private _IdLanguage As Integer
    Private _IsDeleted As Boolean
    Private _IsDefault As Boolean
    Private _Name As String
    Private _Description As String
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
   
    Public Overridable Property IdLanguage As Integer
        Get
            Return _IdLanguage
        End Get
        Set(value As Integer)
            _IdLanguage = value
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
#End Region


    Sub New()

    End Sub

End Class