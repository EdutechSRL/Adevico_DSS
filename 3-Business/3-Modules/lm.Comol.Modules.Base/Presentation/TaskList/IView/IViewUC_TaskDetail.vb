Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewUC_TaskDetail
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentTaskID() As Long
        Property CurrentViewType() As viewDetailType
        Property CurrentTaskAssignmentID() As Long


        Property TaskPermission() As TaskPermissionEnum
        Property isTaskChild() As Boolean

        Sub InitAddTask(ByVal TaskDetail As dtoTaskDetail, ByVal isProject As Boolean, ByVal oListOfStatus As List(Of TaskStatus), ByVal oPriorityList As List(Of TaskPriority), ByVal oListOfCategory As List(Of TaskCategory))
        Sub InitViewUpdate(ByVal TaskDetail As dtoTaskDetail, ByVal oListOfStatus As List(Of TaskStatus), ByVal oPriorityList As List(Of TaskPriority), ByVal oList As List(Of TaskCategory), ByVal MainPageType As ViewModeType, ByVal DetailType As IViewTaskDetail.viewDetailType)
        Sub InitViewRead(ByVal TaskDetail As dtoTaskDetail, ByVal MainPageType As ViewModeType, ByVal DetailType As IViewTaskDetail.viewDetailType)


        Sub SetBackUrl(ByVal BackUrl As String)

        Enum viewDetailType
            Read
            Update
            AddProject
            AddTask
        End Enum


    End Interface
End Namespace