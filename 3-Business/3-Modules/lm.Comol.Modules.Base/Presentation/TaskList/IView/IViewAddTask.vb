Imports lm.Comol.Modules.TaskList.Domain
Imports COL_BusinessLogic_v2.UCServices



Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewAddTask
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property BackUrl() As String
        ReadOnly Property CurrentTaskID() As Long

        Property TaskPermission() As TaskPermissionEnum
        Property CurrentStep() As ViewStep
        Property ParentID() As Long
        Property CurrentTaskIsChild() As Boolean
        Property AddAnotherChildClicked() As Boolean
        Property CurrentCommunityID() As Integer
        Property dtoParentProperty() As dtoTaskDetailWithPermission


        Property SessionUniqueKey() As System.Guid
        Sub ClearUniqueKey()
        Property TasksToAdd() As List(Of Task)
        Property dtoReallocateTAToAdd() As List(Of dtoReallocateTA)
        Property dtoParentReallocateTA() As List(Of dtoReallocateTA)


        Function GetVirtualTaskAssignmentToAdd() As List(Of dtoReallocateTA)
        Function GetUserFromResumeWithModify() As List(Of dtoReallocateTA)
        Function GetTaskChild() As Task
        Function GetParentID() As Long
        Sub ShowError(ByVal ErrorString As String)
        Sub InitSelectParentMap()
        Sub InitSetTaskProperty(ByVal ParentName As String, ByVal dtoTaskDetailToView As dtoTaskDetailWithPermission, ByVal ViewDetailType As IViewUC_TaskDetail.viewDetailType)
        Sub InitResumeTasksWithModifyResources(ByVal ParentName As String, ByVal ListOfdtoOfTaskWithResource As List(Of dtoReallocateTAWithHeader), ByVal ShowManagerError As Boolean)
        Sub InitFinalResumeTasks(ByVal ParentName As String, ByVal ListOfdtoOfTaskWithResource As List(Of dtoReallocateTAWithHeader))
        Sub InitAddVirtualTaskAssignment(ByVal CurrentChildID As Long, ByVal TaskName As String, ByVal CurrentCommunityID As Integer, ByVal ListOfVirtualAssignment As List(Of dtoUserWithRole))
        ' Function GetFinalResource() As dtoReallocateTA da fare quando tia finisce la sua function!!!




        Sub InitButton()
        Sub GoBackPage(ByVal Action As Services_TaskList.ActionType)
        Sub GoBackPage(ByVal Action As COL_BusinessLogic_v2.UCServices.Services_TaskList.ActionType, ByVal ListOfTaskID As List(Of Long))
        Enum ViewStep
            SelectParent = 0
            SetProperty = 1
            UserManagement = 2
            AddUser = 3
            FinalResumeTasks = 4
            ChangeParent = 5
            UpdateChildProperties = 6
        End Enum

        Enum PreviusPage
            DeatailReadOnly
            DetailUpdate
            TaskMap
        End Enum

    End Interface
End Namespace