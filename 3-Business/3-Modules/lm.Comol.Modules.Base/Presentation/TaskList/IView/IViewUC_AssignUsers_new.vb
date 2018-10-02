Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewUC_AssignUsers_new
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Sub InitAssignedPersons(ByVal ListOfDtoAssignUsers As List(Of dtoAssignUsers))

        Property TaskPermission() As TaskPermissionEnum
        Property CurrentView() As viewMode
        Property CurrentTaskID() As Long
        Property CanDeleteManager() As Boolean
        Property AreThereResources() As Boolean

        Property CurrentPageView() As lm.Comol.Modules.TaskList.Domain.ViewModeType ' ViewModeType

        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        ReadOnly Property CurrentPageSize() As Integer
        'Property CurrentPageSkip() As Integer


        Enum viewMode
            None
            Edit
            Read
        End Enum

    End Interface
End Namespace

