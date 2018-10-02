Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Repository
Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewSingleFileUploader
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        Property FolderID() As Long
        Property RepositoryCommunityID() As Integer
        WriteOnly Property CommunityName() As String
        WriteOnly Property FilePath() As String
        WriteOnly Property AllowFolderChange() As Boolean
        Property FolderName() As String
        Property Description() As String
        Property ItemType() As RepositoryItemType
        Property VisibleToDonwloaders() As Boolean
        Property DownlodableByCommunity() As Boolean
        Property AllowDownload() As Boolean
        ReadOnly Property Portalname() As String
        ReadOnly Property BaseFolder() As String
        ReadOnly Property ForCreate() As ItemRepositoryToCreate
        Property AllowUpload As Boolean
        Sub InitializeControl(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ForCreate As ItemRepositoryToCreate, ByVal oPermission As ModuleCommunityRepository)
        Sub LoadFolderSelector(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)
    End Interface
End Namespace