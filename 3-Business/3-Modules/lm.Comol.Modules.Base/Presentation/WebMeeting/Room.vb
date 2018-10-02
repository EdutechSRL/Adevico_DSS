Public Class Room
    Public id As Integer
    Public name As String
    Public description As String
    Public isPublic As Boolean
    Public Type As Int16
    Public ConnectedUsers As List(Of WMUser)
    Public hasWhiteboard As Boolean
    Public onLineUsers As Int16
    Public maxLogged As Int16
    Public startTime As DateTime
    Public isDemoRoom As Boolean
    Public demoTime As Int16

End Class