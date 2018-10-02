Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewWorkBookItemErrors
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreloadedItemID() As System.Guid
        ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission)

        ReadOnly Property ItemInternalFiles() As List(Of String)
        ReadOnly Property ItemCommunityFiles() As List(Of String)

        Sub SetBackToWorkbook(ByVal WorkBookID As System.Guid, ByVal ItemID As System.Guid)
        Sub SetBackToWorkbookItem(ByVal ItemID As System.Guid)
        Sub SetBackToFileManagement(ByVal ItemID As System.Guid)

    End Interface
End Namespace