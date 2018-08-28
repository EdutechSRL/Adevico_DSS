<Serializable()>
Public Class QuestionAssociation

#Region "Private"
    Private _Id As Integer
    Private _IdQuestion As Integer
    Private _IdQuestionnaire As Integer
    Private _Number As Integer
    Private _Weight As Integer
    Private _Difficulty As Integer
    Private _Mandatory As Boolean
    Private _Evaluable As Boolean
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
    Public Overridable Property IdQuestion As Integer
        Get
            Return _IdQuestion
        End Get
        Set(value As Integer)
            _IdQuestion = value
        End Set
    End Property
    Public Overridable Property IdQuestionnaire As Integer
        Get
            Return _IdQuestionnaire
        End Get
        Set(value As Integer)
            _IdQuestionnaire = value
        End Set
    End Property
    Public Overridable Property Number As Integer
        Get
            Return _Number
        End Get
        Set(value As Integer)
            _Number = value
        End Set
    End Property
    Public Overridable Property Weight As Integer
        Get
            Return _Weight
        End Get
        Set(value As Integer)
            _Weight = value
        End Set
    End Property
    Public Overridable Property Difficulty As Integer
        Get
            Return _Difficulty
        End Get
        Set(value As Integer)
            _Difficulty = value
        End Set
    End Property
    Public Overridable Property Mandatory As Boolean
        Get
            Return _Mandatory
        End Get
        Set(value As Boolean)
            _Mandatory = value
        End Set
    End Property
    Public Overridable Property Evaluable As Boolean
        Get
            Return _Evaluable
        End Get
        Set(value As Boolean)
            _Evaluable = value
        End Set
    End Property
#End Region

    Sub New()

    End Sub

End Class