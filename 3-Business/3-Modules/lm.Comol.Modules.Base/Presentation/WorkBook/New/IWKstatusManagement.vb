Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWKstatusManagement
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedPageIndex() As Integer
        ReadOnly Property DefaultPageSize() As Integer
        ReadOnly Property ModulePermission() As ModuleWorkBookManagement

        WriteOnly Property DefaultStatus() As String
        Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentPageSize() As Integer

        Property AllowCreateStatus() As Boolean
        Sub ShowNoStatusView()
        Sub ShowListView()
        Sub LoadStatus(ByVal oList As List(Of dtoWorkBookStatus))
        Sub NoPermissionToAccessPage()
        Sub NavigationUrl()

    End Interface
End Namespace