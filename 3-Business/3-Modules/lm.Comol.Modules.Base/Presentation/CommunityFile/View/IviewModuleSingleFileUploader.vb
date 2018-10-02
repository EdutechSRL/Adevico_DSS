Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewModuleSingleFileUploader
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView



        Property RepositoryCommunityID() As Integer
        Property AllowDownload() As Boolean
        Property ItemType() As RepositoryItemType
        Property AllowAnonymousUpload As Boolean
        Sub InitializeControl(ByVal idCommunity As Integer, allowAnonymousUpload As Boolean)
        Function AddModuleInternalFile(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile)
        Function UploadAndLinkInternalFile(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer) As ModuleActionLink
    End Interface
End Namespace