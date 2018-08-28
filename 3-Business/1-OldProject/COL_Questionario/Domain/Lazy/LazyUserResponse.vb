<Serializable()>
Public Class LazyUserResponse

#Region "Private"
    Private _IdQuestionnnaire As Long
    'Private _IsDeleted As Boolean
    Private _IdPerson As Integer
    Private _QuestionsSkipped As Integer?
    Private _Id As Integer
    'Private _Mark As Short
    Private _QuestionsCount As Integer?
    Private _CorrectAnswers As Integer?
    Private _WrongAnswers As Integer?
    Private _UngradedAnswers As Integer?
    Private _CompletedOn As DateTime?
    Private _ModifiedOn As DateTime?
    Private _StartedOn As DateTime?
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
    Public Overridable Property CompletedOn As DateTime?
        Get
            Return _CompletedOn
        End Get
        Set(value As DateTime?)
            _CompletedOn = value
        End Set
    End Property
    Public Overridable Property StartedOn As DateTime?
        Get
            Return _StartedOn
        End Get
        Set(value As DateTime?)
            _StartedOn = value
        End Set
    End Property
    Public Overridable Property ModifiedOn As DateTime?
        Get
            Return _ModifiedOn
        End Get
        Set(value As DateTime?)
            _ModifiedOn = value
        End Set
    End Property
    'Public Overridable Property IsDeleted As Boolean
    '    Get
    '        Return _IsDeleted
    '    End Get
    '    Set(value As Boolean)
    '        _IsDeleted = value
    '    End Set
    'End Property
    Public Overridable Property IdPerson As Integer
        Get
            Return _IdPerson
        End Get
        Set(value As Integer)
            _IdPerson = value
        End Set
    End Property
    Public Overridable Property QuestionsSkipped As Integer?
        Get
            Return _QuestionsSkipped
        End Get
        Set(value As Integer?)
            _QuestionsSkipped = value
        End Set
    End Property
    Public Overridable Property QuestionsCount As Integer?
        Get
            Return _QuestionsCount
        End Get
        Set(value As Integer?)
            _QuestionsCount = value
        End Set
    End Property
    Public Overridable Property CorrectAnswers As Integer?
        Get
            Return _CorrectAnswers
        End Get
        Set(value As Integer?)
            _CorrectAnswers = value
        End Set
    End Property
    Public Overridable Property WrongAnswers As Integer?
        Get
            Return _WrongAnswers
        End Get
        Set(value As Integer?)
            _WrongAnswers = value
        End Set
    End Property
    Public Overridable Property UngradedAnswers As Integer?
        Get
            Return _UngradedAnswers
        End Get
        Set(value As Integer?)
            _UngradedAnswers = value
        End Set
    End Property
   

   
    Private _IdRandomQuestionnaire As Integer?
    Private _IdInvitedUser As Integer?
    Private _LastIdQuestion As Integer?
    Private _IsDeleted As Boolean
    Private _SemiCorrectAnswers As Integer?
    Private _Score As Decimal?
    Private _RelativeScore As Decimal?
    Public Overridable Property IdRandomQuestionnaire As Integer?
        Get
            Return _IdRandomQuestionnaire
        End Get
        Set(value As Integer?)
            _IdRandomQuestionnaire = value
        End Set
    End Property
    Public Overridable Property IdInvitedUser As Integer?
        Get
            Return _IdInvitedUser
        End Get
        Set(value As Integer?)
            _IdInvitedUser = value
        End Set
    End Property
    Public Overridable Property LastIdQuestion As Integer?
        Get
            Return _LastIdQuestion
        End Get
        Set(value As Integer?)
            _LastIdQuestion = value
        End Set
    End Property
    Public Overridable Property IsDeleted As Boolean
        Get
            Return _LastIdQuestion
        End Get
        Set(value As Boolean)
            _LastIdQuestion = value
        End Set
    End Property
    Public Overridable Property SemiCorrectAnswers As Integer?
        Get
            Return _SemiCorrectAnswers
        End Get
        Set(value As Integer?)
            _SemiCorrectAnswers = value
        End Set
    End Property
    Public Overridable Property Score As Decimal?
        Get
            Return _Score
        End Get
        Set(value As Decimal?)
            _Score = value
        End Set
    End Property
    Public Overridable Property RelativeScore As Decimal?
        Get
            Return _RelativeScore
        End Get
        Set(value As Decimal?)
            _RelativeScore = value
        End Set
    End Property

#End Region

    Sub New()

    End Sub

End Class