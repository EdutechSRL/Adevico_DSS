Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewUC_TasksMap
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        Property CurrentTaskID() As Long
        'Property ListOfTasks() As List(Of dtoTaskMap)
        Property StartLevel() As Integer
        Property LastLevel() As Integer
        Property CanManage() As Boolean

        Property ViewToLoadUC() As ViewModeType

        Property ViewOnlyActiveTask() As Boolean

        Property CanAddSubTask() As Boolean

        Sub LoadDdlWBSlevel()
        Sub LoadTasks(ByVal ListOfTask As List(Of dtoTaskMap)) ', ByVal ViewToLoad As ViewModeType)
        Sub GoToMainPage()
        Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As IViewReallocateUsers.ModeType)
        Sub ReloadPage()

        Sub ShowErrorPopUp(ByVal ErrorName As ErrorType)

        Enum ErrorType
            ParentDeleted
        End Enum

    End Interface
End Namespace