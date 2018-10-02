Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IViewCommunityFileDetail
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property MaxResult() As Integer
        ReadOnly Property Portalname() As String

        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        Sub LoadFolder(ByVal oFolder As dtoCommunityItemRepository, ByVal Communityname As String)
        Sub LoadFolderContent(ByVal oFile As dtoCommunityItemRepository, ByVal Communityname As String)
        Sub LoadNoDetails(ByVal ItemId As Long, ByVal CommunityId As Integer)


        Sub LoadAllowPermission(ByVal ForCommunity As Boolean, ByVal Roles As List(Of String), ByVal Persons As List(Of String), ByVal MorePerson As Boolean)
        Sub LoadDenyPermission(ByVal ForCommunity As Boolean, ByVal Roles As List(Of String), ByVal Persons As List(Of String), ByVal MorePerson As Boolean)
        Sub LoadNoDenyPermission()
    End Interface
End Namespace