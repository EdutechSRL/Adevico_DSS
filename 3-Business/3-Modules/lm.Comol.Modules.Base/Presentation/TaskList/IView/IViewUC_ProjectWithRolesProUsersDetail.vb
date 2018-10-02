Imports lm.Comol.Modules.TaskList.Domain


Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewUC_ProjectWithRolesProUsersDetail
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentTaskID() As Long

        Property LastLevel() As Integer

        Property CanManage() As Boolean

        Property ViewToLoadUC() As ViewModeType

        Property CanAddSubTask() As Boolean

        Property StartLevel() As Integer

        Property ViewOnlyActiveTask() As Boolean

        Sub LoadTasks(ByVal ListOfTask As List(Of dtoQuickUserSelection))

        Sub ReloadPage()

        Sub GoToMainPage()

        Enum ErrorType
            ParentDeleted
        End Enum

        Sub ShowErrorPopUp(ByVal ErrorName As ErrorType)

        Sub GoToReallocateResource(ByVal TaskId As Long, ByVal modeType As IViewReallocateUsers.ModeType)


    End Interface
End Namespace

