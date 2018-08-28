<Serializable()>
Public Class dtoUserQuest
    Public Id As Integer
    Public Score As Integer
    Public MaxAttempts As Integer
    Public Attempts As Integer
    Public MinScore As Integer
    Public MaxScore As Integer
    Public Status As QuizStatus
    Public Name As String
    Public IdLastAttempt As Integer
    Public DisplayScoreToUser As Boolean
    Public DisplayAttemptScoreToUser As Boolean
    Public DisplayResultsStatus As Boolean
    Public DisplayAvailableAttempts As Boolean
    Public DisplayCurrentAttempts As Boolean

    Public PreviousScore As Integer
    Public Type As QuestionnaireType

    Public Sub New()

    End Sub
    Public Sub New(item As LazyQuestionnaire, translatedName As String)
        Id = item.Id
        MaxAttempts = item.MaxAttempts
        MinScore = item.MinScore
        DisplayScoreToUser = item.DisplayScoreToUser
        DisplayAttemptScoreToUser = item.DisplayAttemptScoreToUser
        DisplayResultsStatus = item.DisplayResultsStatus
        DisplayAvailableAttempts = item.DisplayAvailableAttempts
        DisplayCurrentAttempts = item.DisplayCurrentAttempts
        Type = DirectCast(item.IdType, QuestionnaireType)
        MaxScore = item.EvaluationScale
        If String.IsNullOrWhiteSpace(translatedName) Then
            Name = ""
        Else
            Name = translatedName
        End If

        Status = QuizStatus.ToCompile
        'MaxScore = item.
        'Status = QuizStatus.ToCompile

        'Public Status As QuizStatus
        'Public Name As String
        'Public IdLastAttempt As Integer

    End Sub
End Class