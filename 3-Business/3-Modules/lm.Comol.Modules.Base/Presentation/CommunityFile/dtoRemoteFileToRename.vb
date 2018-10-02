Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoRemoteFileToRename
        Public Id As System.Guid
        Public DisplayName As String
        Public isFile As Boolean
        Public Status As ItemRepositoryStatus
        Public FolderId As Long
        Public SavedFilePath As String
        Public SavedName As String
        Public SavedExtension As String
    End Class
End Namespace

