Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface ImoduleToRepositoryAction
        Inherits iModuleActionView

        Property IsInline As Boolean
        Property ServiceCode As String
        Property ServiceID As Integer
        Sub DisplayNoAction()
        Sub DisplayAction()

        Sub ActionForDownload(ByVal IdLink As Long, ByVal IdCommunity As Integer, ByVal item As BaseCommunityFile)
        Sub ActionForPlay(ByVal IdLink As Long, ByVal IdCommunity As Integer, ByVal item As BaseCommunityFile)
        Sub ActionForPlayInternal(ByVal IdLink As Long, ByVal IdCommunity As Integer, ByVal item As BaseCommunityFile, ByVal idAction As Integer)
        Sub ActionForUpload(ByVal descriptionOnly As Boolean, ByVal IdLink As Long, ByVal idFolder As Long, ByVal FolderName As String, ByVal IdCommunity As Integer)
        Sub ActionForCreateFolder(ByVal descriptionOnly As Boolean, ByVal IdLink As Long, ByVal idFolder As Long, ByVal FolderName As String, ByVal IdCommunity As Integer)

        Sub DisplayItemAction(ByVal fileName As String, ByVal extension As String, ByVal size As Long, ByVal type As Repository.RepositoryItemType)

        ReadOnly Property GetUrlForDownload(ByVal IdLink As Long, ByVal Idfile As Long, ByVal IdCommunity As Integer) As String
        ReadOnly Property GetUrlForPlay(ByVal IdLink As Long, ByVal Idfile As Long, ByVal UniqueID As System.Guid, ByVal CommunityID As Integer, ByVal type As Repository.RepositoryItemType) As String
        ReadOnly Property GetUrlForPlayInternal(ByVal IdLink As Long, ByVal Idfile As Long, ByVal UniqueID As System.Guid, ByVal CommunityID As Integer, ByVal ServiceActionID As Integer, ByVal type As Repository.RepositoryItemType) As String
        ReadOnly Property GetUrlForUpload(ByVal IdLink As Long, ByVal FolderID As Long, ByVal IdCommunity As String) As String
        ReadOnly Property GetUrlForCreateFolder(ByVal IdLink As Long, ByVal FolderID As Long, ByVal IdCommunity As Integer) As String
        ReadOnly Property GetUrlForSettings(ByVal Idfile As Long, ByVal type As Repository.RepositoryItemType) As String
        ReadOnly Property GetUrlForPersonalStatistics(ByVal Idfile As Long, ByVal type As Repository.RepositoryItemType) As String
        ReadOnly Property GetUrlForAdvancedStatistics(ByVal Idfile As Long, ByVal type As Repository.RepositoryItemType) As String
    End Interface
End Namespace