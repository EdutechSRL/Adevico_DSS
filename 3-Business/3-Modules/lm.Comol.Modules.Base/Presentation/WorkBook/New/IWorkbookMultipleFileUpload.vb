Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWorkbookMultipleFileUpload
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter
        ReadOnly Property PreloadedItemID() As System.Guid
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)

        WriteOnly Property AllowUpload() As Boolean
        Sub SetUrlToWorkbook(ByVal WorkBookID As System.Guid, ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub SetUrlToItem(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub SetUrlToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter)
        WriteOnly Property AllowCommunityUpload() As Boolean
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository


        Sub NoPermissionToAddFiles(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub ReturnToItemsList(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub ReturnToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter)
        Sub ReturnToFileManagementWithErrors(ByVal ItemID As System.Guid, ByVal NUinternalFile As List(Of BaseFile), ByVal NUcommunityFile As List(Of dtoUploadedFile), ByVal oView As WorkBookTypeFilter)
        Sub InitializeCommunityUploader(ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository)
        Sub AddInternalFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer)

        Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub NotifyAddInternalFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        '  Sub NotifyUnlinkCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        ' Sub NotifyRemoveInternalFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))

    End Interface
End Namespace