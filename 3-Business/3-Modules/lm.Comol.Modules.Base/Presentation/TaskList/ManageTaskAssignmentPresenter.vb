Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class ManageTaskAssignmentPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager

#Region "Standard"

        Public Overloads Property CurrentManager() As TaskManager
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As TaskManager)
                _CurrentManager = value
            End Set
        End Property

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IViewManageTaskAssignment
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

        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewManageTaskAssignment)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub

#End Region

#Region "PERMESSI"
        Private _Permission As ModuleTaskList
        Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        Private ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As ModuleTaskList
            Get
                If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
                    _Permission = Me.View.ModulePersmission
                    Return _Permission
                ElseIf CommunityID > 0 Then
                    _Permission = (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
                    If IsNothing(_Permission) Then
                        _Permission = New ModuleTaskList
                    End If
                    Return _Permission
                Else
                    Return _Permission
                End If
                Return _Permission
            End Get
        End Property

        Private ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))
            Get
                If IsNothing(_CommunitiesPermission) Then
                    _CommunitiesPermission = Me.View.CommunitiesPermission()
                End If
                Return _CommunitiesPermission
            End Get
        End Property

#End Region


        Public Sub InitView()

            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission) 'nn loggato allora errore
            Else
                Select Case Me.View.ViewType
                    Case IViewManageTaskAssignment.viewAssignmentType.AddTaskAssignment

                        If Me.View.ViewModeType = ViewModeType.TaskAdmin Then
                            Me.View.TaskPermission = Me.CurrentTaskManager.GetRolePermissions(TaskRole.ProjectOwner)
                        Else
                            Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
                        End If
                        If ((Me.View.TaskPermission And TaskPermissionEnum.ManagementUser) = TaskPermissionEnum.ManagementUser) Then
                            Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                            Me.InitUserSelection()
                        Else
                            Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                        End If

                    Case IViewManageTaskAssignment.viewAssignmentType.ViewTaskAssignment
                        ''da fare

                    Case IViewManageTaskAssignment.viewAssignmentType.AddQuickTaskAssignment

                        'If Me.View.ViewModeType = ViewModeType.TaskAdmin Then
                        '    Me.View.TaskPermission = Me.CurrentTaskManager.GetRolePermissions(TaskRole.ProjectOwner)
                        'Else
                        '    Me.View.TaskPermission = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
                        'End If
                        'If ((Me.View.TaskPermission And TaskPermissionEnum.ManagementUser) = TaskPermissionEnum.ManagementUser) Then
                        '    Me.View.CurrentCommunityID = Me.CurrentTaskManager.GetCommunityID(Me.View.CurrentTaskID)
                        Me.InitUserSelection()
                        'Else
                        '    Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                        'End If

                    Case Else
                        Me.View.ShowError(My.Resources.ModuleBaseResource.NotPermission)
                End Select

            End If
        End Sub


        Private Sub InitUserSelection()
            Dim listOfPersonIDToHide As New List(Of Integer)
            Dim listtemp As New List(Of Integer)

            Dim oTaskDetail As dtoTaskDetail
            oTaskDetail = Me.CurrentTaskManager.GetTaskDetail(Me.View.CurrentTaskID)

            Me.View.SetTaskName(oTaskDetail.TaskWBS, oTaskDetail.TaskName)
            Me.View.InitUserSelection()

        End Sub

    End Class
End Namespace