Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewCommunityFileEdit
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreloadedItemID() As Long
        ReadOnly Property PreloadedFolderID() As Long
        ReadOnly Property PreloadedCommunityID() As Integer
        ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository
        ReadOnly Property PreLoadedPage() As RepositoryPage

        WriteOnly Property AskToApplyToAllSubItems() As Boolean
        Property RepositoryCommunityID() As Integer
        Property RepositoryItemID() As Long
        ReadOnly Property Portalname() As String
        ReadOnly Property BaseFolder() As String
        WriteOnly Property CommunityName() As String
        WriteOnly Property ItemPath() As String
        'Property FileName() As String
        'Property FolderName() As String
        Property Description() As String
        Property VisibleToDownloaders() As Boolean
        Property AllowDownload() As Boolean
        Property AllowUpload As Boolean
        Property DownlodableByCommunity() As Boolean
        Property RepositoryFolderID() As Long

        WriteOnly Property AllowBackToManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal View As IViewExploreCommunityRepository.ViewRepository) As Boolean
        WriteOnly Property AllowBackToDownloads(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal View As IViewExploreCommunityRepository.ViewRepository) As Boolean

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub NoPermissionToEdit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean)

      
        Sub InitializeView(ByVal isFile As Boolean, ByVal type As Repository.RepositoryItemType)
        Sub LoadFolderSelector(ByVal ExludeFolderID As Long, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)

        Sub ShowFolderDoesntExist(ByVal FolderName As String)
        Sub ShowFileNameExist(ByVal FolderParentName As String, ByVal FileName As String)
        Sub ShowFolderExist(ByVal FolderParentName As String, ByVal FileName As String)
        Sub ItemFolderChanged()
        Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)
        Sub LoadEditingPermission(ByVal ItemID As Long, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage)

        Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean)
        Sub SendActionCompleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean)
        'Sub NotifyItemChanges(ByVal CommunityID As Integer, ByVal OwnerID As Integer, ByVal ItemName As String, ByVal ItemID As Long, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal isVisible As Boolean)
        Sub NotifyItemChanges(ByVal CommunityID As Integer, ByVal OwnerID As Integer, ByVal ItemName As String, ByVal ItemID As Long, ByVal FatherID As Long, ByVal FatherName As String, ByVal uniqueID As System.Guid, ByVal isVisible As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType)
        Sub NotifyVisibilityChange(ByVal CommunityID As Integer, ByVal ItemName As String, ByVal ItemID As Long, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal uniqueID As System.Guid, ByVal isVisible As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType)
        Sub NotifyPermissionChanged(ByVal ToallCommunity As Boolean, ByVal OwnerID As Integer, ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal ItemName As String, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal uniqueID As System.Guid, ByVal isVisible As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType)

        Property ItemName() As String
        Property ItemExtension() As String
        Sub RenameItemError(ByVal folderParentName As String, ByVal iteName As String, ByVal isFile As Boolean)

    End Interface
End Namespace