<Serializable()>
Public Class dtoWorkingLibrary
    Public Property Library As Questionario
    Public Property IdLibrary As Integer
    Public Property UniqueIdentifier As Guid
    Public Property LoadedOn As DateTime
    Public Property TimeValidity As TimeSpan
    Public Sub New()

    End Sub

    Public Sub New(validity As TimeSpan)
        TimeValidity = validity
        LoadedOn = DateTime.Now

    End Sub

    Public Function IsValid() As Boolean
        Return (DateTime.Now - LoadedOn) <= TimeValidity
    End Function
End Class