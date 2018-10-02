Namespace lm.Comol.Modules.Base.DomainModel
    Public Class RepositoryRootObject
        Public Shared Function DisplayActionControlNew() As String
            Return "Modules/Repository/UC/UC_ModuleRepositoryAction.ascx"
        End Function
        Public Shared Function DisplayActionControl() As String
            Return "Modules/Repository/UC/UC_ModuleToRepositoryAction.ascx"
        End Function
        Public Shared Function CreateActionControl() As String
            Return "Modules/Repository/UC/UC_ModuleToRepository.ascx"
        End Function
        Public Shared Function DisplayActionControlQuiz() As String
            Return "Modules/EduPath/UC/UC_ModuleToQuizAction.ascx"
        End Function
        Public Shared Function DownloadFile(ByVal FileID As Long, ByVal UserID As Long, ByVal LanguageCode As String) As String
            Return "File.repository?FileID=" & FileID.ToString & "&ForUserID=" & UserID.ToString & "&Language=" & LanguageCode
        End Function
        Public Shared Function DownloadFileFromModule(ByVal FileID As Long, ByVal UserID As Long, ByVal LanguageCode As String, ByVal ModuleID As Integer, ByVal CommunityID As Integer, ByVal LinkID As Long) As String
            Return "File.repository?FileID=" & FileID.ToString & "&ForUserID=" & UserID.ToString & "&Language=" & LanguageCode & "&LinkID=" & LinkID.ToString & "&ModuleID=" & ModuleID.ToString & "&CommunityID=" & CommunityID.ToString
        End Function

    End Class
End Namespace