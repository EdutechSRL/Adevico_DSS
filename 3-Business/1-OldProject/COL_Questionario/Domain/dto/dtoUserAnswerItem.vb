<Serializable()>
Public Class dtoUserAnswerItem
    Public Property Id As Integer
    Public Property DisplayName As String
    Public Property AttemptNumber As Integer
    Public Property IdQuestionnnaire As Integer
    Public Property CompletedOn As DateTime?
    Public Property IdPerson As Integer
    Public Property QuestionsSkipped As Integer?
    Public Property QuestionsCount As Integer?
    Public Property CorrectAnswers As Integer?
    Public Property WrongAnswers As Integer?
    Public Property UngradedAnswers As Integer?
    Public Property IdRandomQuestionnaire As Integer?
    Public Property IdInvitedUser As Integer?
    Public Property LastIdQuestion As Integer?
    Public Property IsDeleted As Boolean
    Public Property SemiCorrectAnswers As Integer?
    Public Property Score As Decimal?
    Public Property RelativeScore As Decimal?


    Sub New()

    End Sub

    Sub New(answer As LazyUserResponse)
        Id = answer.Id
        IdQuestionnnaire = answer.IdQuestionnnaire
        CompletedOn = answer.CompletedOn
        QuestionsSkipped = answer.QuestionsSkipped
        QuestionsCount = answer.QuestionsCount
        CorrectAnswers = answer.CorrectAnswers
        WrongAnswers = answer.WrongAnswers
        UngradedAnswers = answer.UngradedAnswers
        IdRandomQuestionnaire = answer.IdRandomQuestionnaire
        IdInvitedUser = answer.IdInvitedUser
        IdPerson = answer.IdPerson
        LastIdQuestion = answer.LastIdQuestion
        IsDeleted = answer.IsDeleted
        SemiCorrectAnswers = answer.SemiCorrectAnswers
        Score = answer.Score
        RelativeScore = answer.RelativeScore
    End Sub
End Class

<Serializable()>
Public Class dtoUserAnswerBaseItem
    Public Property Id As Integer
    Public Property AttemptNumber As Integer
    Public Property IdQuestionnnaire As Integer
    Public Property IdPerson As Integer
    Public Property IdRandomQuestionnaire As Integer?
    Public Property IdInvitedUser As Integer?


    Sub New()

    End Sub

    Sub New(answer As LazyUserResponse)
        Id = answer.Id
        IdQuestionnnaire = answer.IdQuestionnnaire
        IdRandomQuestionnaire = answer.IdRandomQuestionnaire
        IdInvitedUser = answer.IdInvitedUser
        IdPerson = answer.IdPerson
    End Sub
End Class