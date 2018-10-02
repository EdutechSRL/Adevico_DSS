Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    Public Interface IviewWorkBookItemFileList
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property WorkBookCommunityID() As Integer
        Property WorkBookModuleID() As Integer
        Property ItemID() As System.Guid
        Property AutoUpdate() As Boolean
        Property AllowOnlyVisibleFiles() As Boolean
        Property ShowManagementButtons() As Boolean
        Sub LoadFiles(ByVal oList As List(Of dtoWorkBookFile))
        Sub RequireUpdate()
        Property ItemPermissions() As WorkBookItemPermission
        Property CommunityRepositoryPermissions() As ModuleCommunityRepository

    End Interface
End Namespace