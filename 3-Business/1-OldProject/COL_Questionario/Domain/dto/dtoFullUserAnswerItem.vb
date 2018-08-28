<Serializable()>
Public Class dtoFullUserAnswerItem
    Public Property Id As Integer
    Public Property AttemptNumber As Integer
    Public Property Answer As LazyUserResponse
    Sub New(item As LazyUserResponse)
        Id = item.Id
        Answer = item
    End Sub
End Class