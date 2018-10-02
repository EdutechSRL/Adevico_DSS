Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class AssignedUserUCPresenter
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
        Public Overloads ReadOnly Property View() As IViewUC_AssignedUser
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUC_AssignedUser)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Sub VirtualDeleteTaskAssignment(ByVal TaskAssignmentID As Long)
            Dim InterestedTaskAssignment As TaskAssignment
            InterestedTaskAssignment = Me.CurrentTaskManager.GetTaskAssignment(TaskAssignmentID)
            If Not IsNothing(InterestedTaskAssignment) Then
                Me.CurrentTaskManager.DeleteVirtualTaskAssignment(InterestedTaskAssignment, Me.AppContext.UserContext.CurrentUser)
            End If

        End Sub

        Sub UnDeleteTaskAssignment(ByVal TaskAssignmentID As Long)
            Me.CurrentTaskManager.UnDeleteTaskAssignment(TaskAssignmentID)

        End Sub

        Sub DeletePermanentlyTaskAssignment(ByVal TaskAssignmentID As Long)
            Me.CurrentTaskManager.DeleteTaskAssignment(TaskAssignmentID)
            InitAssignedPersons()
        End Sub

        Public Sub InitView(ByVal CurrentTaskID As Long, ByVal TaskPermission As TaskPermissionEnum, ByVal ViewMode As IViewUC_AssignedUser.viewMode)
            Me.View.CurrentView = ViewMode
            Me.View.CurrentTaskID = CurrentTaskID
            Me.View.TaskPermission = TaskPermission
            SetStartPager()
            InitAssignedPersons()
        End Sub


        Public Sub InitAssignedPersons()
            Dim ListOfDtoTaskAssignment As New List(Of dtoTaskAssignment)

            Select Case Me.View.CurrentView
                Case IViewUC_AssignedUser.viewMode.Manage
                    ListOfDtoTaskAssignment = Me.CurrentTaskManager.GetPagedTaskAssignment(Me.View.CurrentTaskID, False, Me.View.Pager.PageSize, Me.View.Pager.PageIndex)
                    Me.View.CanDeleteManager = Me.CurrentTaskManager.CanDeleteManagers(Me.View.CurrentTaskID)

                Case IViewUC_AssignedUser.viewMode.SelectActiveResource
                    ListOfDtoTaskAssignment = Me.CurrentTaskManager.GetPagedTaskAssignment(Me.View.CurrentTaskID, TaskRole.Resource, False, Me.View.Pager.PageSize, Me.View.Pager.PageIndex)

                Case IViewUC_AssignedUser.viewMode.SelectDeletedResource
                    ListOfDtoTaskAssignment = Me.CurrentTaskManager.GetPagedTaskAssignment(Me.View.CurrentTaskID, TaskRole.Resource, True, Me.View.Pager.PageSize, Me.View.Pager.PageIndex)

                Case IViewUC_AssignedUser.viewMode.ViewActiveUser
                    ListOfDtoTaskAssignment = Me.CurrentTaskManager.GetPagedTaskAssignment(Me.View.CurrentTaskID, True, Me.View.Pager.PageSize, Me.View.Pager.PageIndex)

                Case IViewUC_AssignedUser.viewMode.ViewAllResource
                    ListOfDtoTaskAssignment = Me.CurrentTaskManager.GetPagedTaskAssignment(Me.View.CurrentTaskID, TaskRole.Resource, Me.View.Pager.PageSize, Me.View.Pager.PageIndex, False)
            End Select


            Me.View.InitAssignedPersons(ListOfDtoTaskAssignment)
        End Sub

        Private Sub SetStartPager()
            Dim oPager As New lm.Comol.Core.DomainModel.PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count = Me.CurrentTaskManager.GetTaskAssignmentCount(Me.View.CurrentTaskID)
            oPager.Count -= 1
            Me.View.Pager = oPager
        End Sub

    End Class
End Namespace