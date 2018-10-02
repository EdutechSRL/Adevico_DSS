Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWorkBookItemManagementFile
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter
        ReadOnly Property PreloadedItemID() As System.Guid
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        ReadOnly Property AllowPublish() As Boolean
        WriteOnly Property AllowCommunityUpload() As Boolean
        WriteOnly Property AllowCommunityLink() As Boolean

        WriteOnly Property AllowUpload() As Boolean
        WriteOnly Property BackToWorkbook() As System.Guid
        Sub SetBackToItemUrl(ByVal WorkBookID As System.Guid, ByVal ItemID As System.Guid)
        Sub SetMultipleUploadUrl(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub ReturnToItemsList(ByVal WorkBookID As System.Guid)
        Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer)

        Sub InitializeCommunityUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository)
        Sub AddInternalFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer)

        Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyAddInternalFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub ReturnToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub ReturnToFileManagementWithErrors(ByVal NUinternalFile As List(Of BaseFile), ByVal NUcommunityFile As List(Of dtoUploadedFile), ByVal oView As WorkBookTypeFilter)

        Sub LoadFilesToManage(ByVal ItemID As System.Guid, ByVal CommunityID As Integer, ByVal AllowManagement As Boolean, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository, ByVal AllowPublish As Boolean)
        Sub LoadEditingPermission(ByVal ItemID As Long, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal GotoPage As RepositoryPage, ByVal WorkBookItemID As Guid)
    End Interface
End Namespace