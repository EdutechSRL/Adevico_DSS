Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Public Class dtoError
    Public BaseUrl As String
    Public CommunityID As Integer
    Public UserID As Integer
    Public FolderID As Long
    Public FileID As Long
    Public FileName As String
    Public Extension As String
    Public ForUserID As Integer
    Public Settings As NotificationErrorSettings
    Public FileSettings As FileSettings
    Public ErrorType As ItemRepositoryStatus
    Public Language As String
    Public IsOnModal As Boolean
End Class