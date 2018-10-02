Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
    <CLSCompliant(True)> Public Interface IWKstatusEdit
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property PreLoadedStatusID() As Integer
        ReadOnly Property ModulePermission() As ModuleWorkBookManagement
        Property AllowSaveStatus() As Boolean
        Sub ShowNoStatusView()
        Sub ShowEditView()
        Sub LoadStatus(ByVal oStatus As dtoWorkBookStatus, ByVal oList As List(Of dtoWorkBookStatusTranslation))
        Sub NoPermissionToAccessPage()
        Sub LoadStatusList()

    End Interface
End Namespace