
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class TaskDetailUCPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager


#Region "Standard"

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewUC_TaskDetail
            Get
                Return MyBase.View
            End Get
        End Property
        Public Property CurrentTaskManager() As TaskManager
            Get
                Return _BaseTaskManager
            End Get
            Set(ByVal value As TaskManager)
                _BaseTaskManager = value
            End Set
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUC_TaskDetail)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Private Function LoadTaskPriorities()
            Dim ListOfPriorities As New List(Of TaskPriority)
            ListOfPriorities.Add(TaskPriority.high)
            ListOfPriorities.Add(TaskPriority.normal)
            ListOfPriorities.Add(TaskPriority.low)
            Return ListOfPriorities
        End Function

        Private Function LoadTaskStatus()
            Dim oListOfStatus As New List(Of TaskStatus)
            oListOfStatus.Add(TaskStatus.completed)
            oListOfStatus.Add(TaskStatus.notStarted)
            oListOfStatus.Add(TaskStatus.started)
            oListOfStatus.Add(TaskStatus.suspended)
            Return oListOfStatus
        End Function


        Public Sub InitView(ByVal TypeView As IViewUC_TaskDetail.viewDetailType, ByVal TaskDetailWithPermission As dtoTaskDetailWithPermission, ByVal BackUrl As String, ByVal MainPage As ViewModeType, ByVal DetailType As IViewTaskDetail.viewDetailType)
            Me.View.CurrentViewType = TypeView
            Me.View.TaskPermission = TaskDetailWithPermission.Permission
            Me.View.SetBackUrl(BackUrl)

            Me.View.CurrentTaskID = TaskDetailWithPermission.dtoTaskDetail.TaskID
            Select Case Me.View.CurrentViewType
                Case IViewUC_TaskDetail.viewDetailType.AddProject
                    Me.View.InitAddTask(TaskDetailWithPermission.dtoTaskDetail, True, LoadTaskStatus, LoadTaskPriorities, Me.CurrentTaskManager.GetAllTaskCategories(False))

                Case IViewUC_TaskDetail.viewDetailType.AddTask
                    Me.View.InitAddTask(TaskDetailWithPermission.dtoTaskDetail, False, LoadTaskStatus, LoadTaskPriorities, Me.CurrentTaskManager.GetAllTaskCategories(False))

                Case Else
                    If Not ((TaskPermissionEnum.None And Me.View.TaskPermission) > TaskPermissionEnum.None) Then
                        Me.View.CurrentTaskAssignmentID = TaskDetailWithPermission.dtoTaskDetail.TaskAssignmentID
                        Select Case Me.View.CurrentViewType
                            Case IViewUC_TaskDetail.viewDetailType.Read
                                Me.View.InitViewRead(TaskDetailWithPermission.dtoTaskDetail, MainPage, DetailType)

                            Case IViewUC_TaskDetail.viewDetailType.Update

                                If Me.CurrentTaskManager.GetNumberOfChildren(Me.View.CurrentTaskID, False) = 0 Then
                                    Me.View.isTaskChild = True
                                Else
                                    Me.View.isTaskChild = False
                                End If
                                Dim ListOfCategories As List(Of TaskCategory)
                                Dim ListOfTaskStatus As List(Of TaskStatus)
                                Dim ListOfPriorities As List(Of TaskPriority)
                                ListOfPriorities = LoadTaskPriorities()
                                ListOfCategories = Me.CurrentTaskManager.GetAllTaskCategories(False)
                                ListOfTaskStatus = LoadTaskStatus()

                                Me.View.InitViewUpdate(TaskDetailWithPermission.dtoTaskDetail, ListOfTaskStatus, ListOfPriorities, ListOfCategories, MainPage, DetailType)
                        End Select
                    End If
            End Select
        End Sub




        Public Sub UpdatePersonalCompleteness(ByVal PersonalCompleteness As Integer)
            Dim oTA As TaskAssignment
            oTA = Me.CurrentTaskManager.GetTaskAssignment(Me.View.CurrentTaskAssignmentID)
            Me.CurrentTaskManager.UpdateTaskAssignmentCompleteness(oTA, PersonalCompleteness)

        End Sub

        Public Function GetTaskCategory(ByVal name As String)
            Dim oTaskCategory As TaskCategory
            oTaskCategory = Me.CurrentTaskManager.GetTaskCategory(name, False)
            Return oTaskCategory
        End Function

        'Public Function GetTaskStatus(ByVal name As String)
        '    Dim status As TaskStatus
        '    Select Case name
        '        Case TaskStatus.completed.ToString
        '            status = TaskStatus.completed

        '        Case TaskStatus.notStarted.ToString
        '            status = TaskStatus.notStarted

        '        Case TaskStatus.pending.ToString
        '            status = TaskStatus.pending

        '        Case TaskStatus.started.ToString
        '            status = TaskStatus.started

        '        Case TaskStatus.suspended.ToString
        '            status = TaskStatus.suspended
        '    End Select
        '    Return status
        'End Function

        'Public Function GetTaskPriority(ByVal Name As String)
        '    Dim priority As TaskPriority
        '    Select Case Name
        '        Case TaskPriority.critical.ToString
        '            priority = TaskPriority.critical

        '        Case TaskPriority.high.ToString
        '            priority = TaskPriority.high

        '        Case TaskPriority.normal.ToString
        '            priority = TaskPriority.normal

        '        Case TaskPriority.low.ToString
        '            priority = TaskPriority.low
        '    End Select
        '    Return priority
        'End Function


    End Class

End Namespace