Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Public Class dtoInternalError
    Public BaseUrl As String
    Public CommunityId As Integer
    Public UserID As Integer
    Public FileID As System.Guid
    Public FileName As String
    Public Extension As String
    Public ForUserID As Integer
    Public Settings As NotificationErrorSettings
    Public FileSettings As FileSettings
    Public ErrorType As ItemRepositoryStatus
    Public Language As String
End Class