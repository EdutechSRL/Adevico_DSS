Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewUC_AssignedUser
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentTaskID() As Long
        ' Property IsManageMode() As Boolean
        Property TaskPermission() As TaskPermissionEnum
        Property CurrentView() As viewMode
        Property CanDeleteManager() As Boolean


        'paginazione
        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        ReadOnly Property CurrentPageSize() As Integer
        'fine paginazione


        Sub InitAssignedPersons(ByVal ListOfTaskAssignment As List(Of dtoTaskAssignment))


        Enum viewMode
            None
            Manage
            SelectActiveResource
            SelectDeletedResource
            ViewActiveUser
            ViewAllResource
        End Enum

    End Interface
End Namespace