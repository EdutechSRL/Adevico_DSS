<Serializable()>
Public Class QuestionnaireAnswer
    Public Id As Long
    Public IdQuestionnaire As Long
    Public IdUser As Integer
    Public IdInvitedUser As Long
    Public IdRandomQuestionnaire As Long
    Public Answers As List(Of QuestionAnswer)
    Public Questions As List(Of LazyAssociatedQuestion)
    Public Sub New()
        Answers = New List(Of QuestionAnswer)
        Questions = New List(Of LazyAssociatedQuestion)
    End Sub

End Class


<Serializable()> Public Class QuestionAnswer
    Public Id As Long
    Public IdQuestion As Long
    Public IdQuestionOption As Long
    Public QuestionType As Domanda.TipoDomanda
    Public OptionNumber As Integer
    Public OptionText As String
    Public Value As String
    Public Evaluation As String
End Class