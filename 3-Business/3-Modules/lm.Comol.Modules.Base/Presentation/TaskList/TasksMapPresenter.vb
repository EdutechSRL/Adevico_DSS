
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class TasksMapPresenter
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
        Public Overloads ReadOnly Property View() As IViewTaskMap
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewTaskMap)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region


        Public Sub InitView()
            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission) 'nn loggato allora errore

            ElseIf Me.View.MainPage = ViewModeType.TaskAdmin Then
                Me.View.CurrentProjectID = Me.CurrentTaskManager.GetProjectID(Me.View.CurrentTaskID)
                Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)

                Dim isAdministered As Boolean = (From c In Me.View.CommunitiesPermission Where c.ID = Me.View.CurrentCommunityID AndAlso c.Permissions.Administration = True).Count > 0
                If isAdministered = True Then
                    Dim dtoTask As dtoTaskDetail = Me.CurrentTaskManager.GetTaskDetail(Me.View.CurrentProjectID)
                    Me.View.SetTaskName(dtoTask.TaskName)

                    Dim CanSwichTaskWbsPosition As Boolean = isAdministered
                    Me.View.InitHyperlinkUrl(CanSwichTaskWbsPosition)
                    Select Case Me.View.CurrentMapType
                        Case IViewTaskMap.viewMapType.ClassicMap
                            Me.View.InitMap()
                        Case IViewTaskMap.viewMapType.SwichMap
                            If CanSwichTaskWbsPosition Then
                                Me.View.InitSwichMap()
                            Else
                                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                            End If
                    End Select
                Else
                    Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                End If

            Else

                Me.View.CurrentProjectID = Me.CurrentTaskManager.GetProjectID(Me.View.CurrentTaskID)
                Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionOverAllProject(Me.View.CurrentProjectID, Me.AppContext.UserContext.CurrentUserID)

                If ((Me.View.TaskPermission And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView) Then
                    Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                    ' Me.View.CurrentProjectID = Me.CurrentTaskManager.GetProjectID(Me.View.CurrentTaskID)
                    Dim dtoTask As dtoTaskDetail = Me.CurrentTaskManager.GetTaskDetail(Me.View.CurrentProjectID)
                    Me.View.SetTaskName(dtoTask.TaskName)
                    '  Dim CanSwichTaskWbsPosition As Boolean = Me.CurrentTaskManager.CanSwichTaskWBSPosition(Me.View.CurrentProjectID, Me.AppContext.UserContext.CurrentUserID)
                    Dim CanSwichTaskWbsPosition As Boolean = ((Me.View.TaskPermission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete)
                    Me.View.InitHyperlinkUrl(CanSwichTaskWbsPosition)
                    Select Case Me.View.CurrentMapType
                        Case IViewTaskMap.viewMapType.ClassicMap
                            Me.View.InitMap()
                        Case IViewTaskMap.viewMapType.SwichMap
                            If CanSwichTaskWbsPosition Then
                                Me.View.InitSwichMap()
                            Else
                                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                            End If
                    End Select

                Else
                    Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                End If
            End If

            'End If
        End Sub

        Public Function GetTaskDetailWithPermission()
            Return CurrentTaskManager.GetTaskDetailWithPermission(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
        End Function

        Public Sub UpdateTask(ByVal oTask As Task)
            Me.CurrentTaskManager.UpdateTaskDetail(Me.View.CurrentTaskID, oTask, Me.AppContext.UserContext.CurrentUser)
        End Sub

        Public Sub ReloadInfo()
            Select Case Me.View.CurrentMapType
                Case IViewTaskMap.viewMapType.ClassicMap
                    Me.View.InitMap()
                Case IViewTaskMap.viewMapType.SwichMap
                    Me.View.InitSwichMap()
            End Select
        End Sub

        Private Function CanUpdate()
            '   Select Case Me.View.TaskPermission
            If ((Me.View.TaskPermission And TaskPermissionEnum.AddFile) = TaskPermissionEnum.AddFile) Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.ManagementUser) = TaskPermissionEnum.ManagementUser Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete Then
                Return True
            ElseIf ((Me.View.TaskPermission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate) Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.TaskSetCategory) = TaskPermissionEnum.TaskSetCategory Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.TaskSetDeadline) = TaskPermissionEnum.TaskSetDeadline Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.TaskSetEndDate) = TaskPermissionEnum.TaskSetEndDate Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.TaskSetPriority) = TaskPermissionEnum.TaskSetPriority Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.TaskSetStartDate) = TaskPermissionEnum.TaskSetStartDate Then
                Return True
            ElseIf (Me.View.TaskPermission And TaskPermissionEnum.TaskSetStatus) = TaskPermissionEnum.TaskSetStatus Then
                Return True
            Else
                Return False
                '  End Select
            End If
        End Function



    End Class
End Namespace
