Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    Public Class ProjectWithRolesProUsersDetailUCPresenter

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
        Public Overloads ReadOnly Property View() As IViewUC_ProjectWithRolesProUsersDetail
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUC_ProjectWithRolesProUsersDetail)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView(ByVal CurrentTaskID As Long, ByVal PermissionOverProject As TaskPermissionEnum, ByVal ViewToLoad As ViewModeType) ', ByVal BackUrl As String, ByVal SwichTaskUrl As String, ByVal MainPage As ViewModeType, ByVal DetailType As IViewTaskDetail.viewDetailType)
            Me.View.CurrentTaskID = CurrentTaskID
            Dim Permission As TaskPermissionEnum
            Me.View.ViewToLoadUC = ViewToLoad

            If ViewToLoad = ViewModeType.TaskAdmin Then

                Permission = Me.CurrentTaskManager.GetRolePermissions(TaskRole.ProjectOwner)

            Else

                Permission = PermissionOverProject 'Me.CurrentTaskManager.GetPermissionsOverTask(CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)

            End If

            Me.View.CanManage = Me.CanManage(Permission)
            Me.View.ViewOnlyActiveTask = Not ((Permission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete)
            Dim ListOfTasks As List(Of dtoQuickUserSelection) = Me.GetQuickSelectionMap()

            If (ListOfTasks.Count > 0) Then
                Me.View.StartLevel = 1
                Me.View.LastLevel = Me.CurrentTaskManager.GetMaxLevel(ListOfTasks)
                'Me.View.LoadDdlWBSlevel()
                Me.View.CanAddSubTask = False
                Me.View.LoadTasks(ListOfTasks) ', ViewToLoad)
            Else
                Me.View.GoToMainPage()
            End If
        End Sub

        Public Function CanManage(ByVal TaskPermission As TaskPermissionEnum)
            Dim ManagerPermission As TaskPermissionEnum = Me.CurrentTaskManager.GetRolePermissions(TaskRole.Manager)
            If TaskPermission >= ManagerPermission Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GetQuickSelectionMap()
            Return Me.CurrentTaskManager.GetQuickSelectionMap(Me.CurrentTaskManager.GetTask(Me.View.CurrentTaskID), Me.AppContext.UserContext.CurrentUserID, Me.View.ViewOnlyActiveTask)
        End Function

        Public Sub VirtualDelete(ByVal TaskId As Long)
            If Not Me.CurrentTaskManager.GetIfTaskIsDeleted(TaskId) Then
                Dim ParentID As Long = Me.CurrentTaskManager.GetParentNameAndID(TaskId).ID
                If ParentID <> TaskId Then
                    Dim BrothersNumber As Integer = Me.CurrentTaskManager.GetNumberOfChildren(ParentID, False)
                    If BrothersNumber = 1 Then
                        Me.View.GoToReallocateResource(TaskId, IViewReallocateUsers.ModeType.VirtualDelete)
                    Else
                        Me.CurrentTaskManager.DeleteVirtualTask(TaskId, Me.AppContext.UserContext.CurrentUser)
                        Me.Reload()
                    End If
                Else
                    Me.CurrentTaskManager.DeleteVirtualTask(TaskId, Me.AppContext.UserContext.CurrentUser)
                    Me.Reload()
                End If
            End If
        End Sub

        Public Sub Undelete(ByVal TaskID As Long)
            If Me.CurrentTaskManager.GetIfTaskIsDeleted(TaskID) Then
                Dim dtoParentSimple As dtoTaskSimple = Me.CurrentTaskManager.GetParentNameAndID(TaskID)
                If Not dtoParentSimple.isDeleted Then
                    Dim ActiveResourceNumberOfParent As Integer = Me.CurrentTaskManager.GetTaskAssignmentCount(dtoParentSimple.ID, TaskRole.Resource, False)
                    If ActiveResourceNumberOfParent > 0 Then
                        Me.View.GoToReallocateResource(TaskID, IViewReallocateUsers.ModeType.Undelete)
                    Else
                        Dim DeletedResourceNumberOfParent As Integer = Me.CurrentTaskManager.GetTaskAssignmentCount(dtoParentSimple.ID, TaskRole.Resource, False)
                        If DeletedResourceNumberOfParent > 0 Then
                            Me.CurrentTaskManager.DeleteResourcesTaskAssignments(dtoParentSimple.ID)
                        Else
                            Me.CurrentTaskManager.UnDeleteTask(TaskID, Me.AppContext.UserContext.CurrentUser)
                            Me.Reload()
                        End If
                    End If
                ElseIf TaskID = dtoParentSimple.ID Then
                    Me.CurrentTaskManager.UnDeleteTask(TaskID, Me.AppContext.UserContext.CurrentUser)
                    Me.Reload()
                Else
                    Me.View.ShowErrorPopUp(IViewUC_ProjectWithRolesProUsersDetail.ErrorType.ParentDeleted)
                End If
            End If
        End Sub

        Public Sub Delete(ByVal TaskID As Long)
            Me.CurrentTaskManager.DeleteTask(TaskID)
            Dim PermissionOverProject As TaskPermissionEnum = Me.CurrentTaskManager.GetPermissionOverAllProject(Me.View.CurrentTaskID, Me.AppContext.UserContext.CurrentUserID)
            If (PermissionOverProject And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView Then
                Me.View.ReloadPage()
            Else
                Me.View.GoToMainPage()
            End If
        End Sub

        Public Function AdminPermissions()
            Dim TP As TaskPermissionEnum
            TP = Me.CurrentTaskManager.GetRolePermissions(TaskRole.ProjectOwner)
            Return TP
        End Function


        Public Sub Reload()
            Dim ListOfTasks As List(Of dtoQuickUserSelection) = Me.GetQuickSelectionMap()
            Me.View.LastLevel = Me.CurrentTaskManager.GetMaxLevel(ListOfTasks)

            If ListOfTasks.Count > 0 Then
                ''Me.View.LoadDdlWBSlevel()
                Me.View.CanAddSubTask = False
                Me.View.LoadTasks(ListOfTasks)
            Else
                Me.View.GoToMainPage()
            End If
        End Sub
    End Class

End Namespace

