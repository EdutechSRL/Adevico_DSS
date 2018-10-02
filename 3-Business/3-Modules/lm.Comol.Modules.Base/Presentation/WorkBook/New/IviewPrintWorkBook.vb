Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewPrintWorkBook
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreloadedWorkBookID() As System.Guid
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)
        Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository
        ReadOnly Property DisplayOrderAscending() As Boolean
        Sub LoadItems(ByVal oList As List(Of dtoWorkBookItem))
        Sub NoPermissionToViewItems()
        Sub NoWorkBookWithThisID()
        Sub NoWorkBookItemWithThisID()
    End Interface
End Namespace