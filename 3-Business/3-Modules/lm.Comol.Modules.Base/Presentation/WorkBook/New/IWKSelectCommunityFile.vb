Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWKSelectCommunityFile
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter
        ReadOnly Property PreloadedItemID() As System.Guid
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        WriteOnly Property AllowCommunityLink() As Boolean
        WriteOnly Property BackToManagement() As System.Guid

        Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer, ByVal ModuleID As Integer)

        Sub InitializeFileSelector(ByVal CommunityID As Integer, ByVal SelectedFiles As List(Of Long), ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)
        Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer)

        Sub NotifyAddCommunityFile(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As DateTime, ByVal CreatorName As String, ByVal Authors As List(Of Integer))
        Sub ReturnToFileManagement(ByVal ItemID As System.Guid)
    End Interface
End Namespace