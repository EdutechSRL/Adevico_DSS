<Serializable()>
Public Class QuestionnaireStatisticsFilter
    Public Users As List(Of QuestionnaireFilterByUser)
    Public Responses As List(Of QuestionnaireFilterByResponse)

    Public Sub New()
        Users = New List(Of QuestionnaireFilterByUser)
        Responses = New List(Of QuestionnaireFilterByResponse)
    End Sub

    Public Shared Function GetDefault() As QuestionnaireStatisticsFilter
        Dim item As New QuestionnaireStatisticsFilter
        item.Users.Add(QuestionnaireFilterByUser.currentCommunity)
        item.Users.Add(QuestionnaireFilterByUser.allUsers)
        item.Users.Add(QuestionnaireFilterByUser.externalUsers)
        item.Users.Add(QuestionnaireFilterByUser.invitedUsers)

        item.Responses.Add(QuestionnaireFilterByResponse.compiled)
        item.Responses.Add(QuestionnaireFilterByResponse.notCompleted)
        item.Responses.Add(QuestionnaireFilterByResponse.notStarted)
        item.Responses.Add(QuestionnaireFilterByResponse.invited)
        Return item
    End Function

End Class

<Serializable()>
Public Enum QuestionnaireFilterByUser
    none = 0
    currentCommunity = 1
    allUsers = 2
    externalUsers = 3
    invitedUsers = 4
End Enum


<Serializable()>
Public Enum QuestionnaireFilterByResponse
    none = 0
    compiled = 1
    notCompleted = 2
    notStarted = 3
    invited = 4
End Enum

