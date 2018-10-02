Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewModuleMultipleFileUploader
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        Property AllowAnonymousUpload As Boolean
        Property UploadAsUser As Int32

        Property MaxFileInputsCount() As Integer
        Property RepositoryCommunityID() As Integer

        Sub InitializeControl(ByVal CommunityID As Integer)
        Function AddModuleInternalFiles(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActivityID As Integer, ByVal ObjectTypeID As Integer) As MultipleUploadResult(Of dtoModuleUploadedFile)
        Function UploadAndLinkInternalFile(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ObjectTypeID As Integer) As IList(Of ModuleActionLink)
    End Interface
End Namespace