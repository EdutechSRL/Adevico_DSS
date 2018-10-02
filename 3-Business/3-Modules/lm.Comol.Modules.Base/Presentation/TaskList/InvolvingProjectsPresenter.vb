Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Domain


Namespace lm.Comol.Modules.Base.Presentation.TaskList


    Public Class InvolvingProjectsPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager

#Region "Standard"
        Public Overloads Property CurrentManager() As TaskManager
            Get
                Return _BaseTaskManager
            End Get
            Set(ByVal value As TaskManager)
                _BaseTaskManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewInvolvingProjects
            Get
                Return MyBase.View
            End Get
        End Property
        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New TaskManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewInvolvingProjects)
            MyBase.New(oContext, view)
            Me.CurrentManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region


        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then

                Me.LoadTaskTabs()
                Me.LoadFilters()

                If Me.View.PreLoadedOrderBy = TasksPageOrderBy.None Then
                    Me.View.CurrentOrderBy = TasksPageOrderBy.AllActive
                Else
                    Me.View.CurrentOrderBy = Me.View.PreLoadedOrderBy
                End If

                Me.LoadSorts()
                If Me.View.PreLoadedSorting = lm.Comol.Modules.TaskList.Domain.Sorting.None Then
                    Me.View.CurrentSorting = lm.Comol.Modules.TaskList.Domain.Sorting.DeadlineOrder
                Else
                    Me.View.CurrentSorting = Me.View.PreLoadedSorting
                End If

                Me.LoadInvolvingProjects()

                Me.View.SetNavigationUrlToProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, ProjectOrderBy.AllActive, Sorting.DeadlineOrder)
                Me.View.SetNavigationUrlToAddProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy, Me.View.CurrentSorting)
                Me.View.SetNavigationUrlToAdministration(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, ProjectOrderBy.AllActive, Sorting.DeadlineOrder)

                If Me.UserContext.CurrentCommunityID = 0 Then
                    Me.View.SetNavigationUrlToAssignedTask(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, TasksPageOrderBy.Community, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToManage(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, TasksPageOrderBy.AllActive, TaskManagedType.Projects, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToAdministration(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, ProjectOrderBy.AllActive, Sorting.DeadlineOrder)
                Else
                    Me.View.SetNavigationUrlToAssignedTask(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, TasksPageOrderBy.Community, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToManage(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, TasksPageOrderBy.AllActive, TaskManagedType.Projects, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToAdministration(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, ProjectOrderBy.AllActive, Sorting.DeadlineOrder)
                End If
                'Else

            End If
        End Sub

        Public Sub LoadInvolvingProjects()
            Dim oUserID As Integer
            oUserID = Me.AppContext.UserContext.CurrentUserID

            Dim oPager As New PagerBase
            Dim ItemCount As Integer = 0
            ItemCount = Me.CurrentManager.GetInvolvingProjectsCount(oUserID, Me.View.CurrentCommunityFilter, Me.UserContext.CurrentCommunityID, Me.View.CurrentOrderBy)
            oPager.Count = ItemCount - 1
            oPager.PageSize = Me.View.CurrentPageSize
            If ItemCount > 0 Then
                oPager.PageIndex = Me.View.CurrentPageIndex
            End If

            Dim ListOfInvolvingProjects As List(Of dtoInvolvingProjectsWithRolesWithHeader) = New List(Of dtoInvolvingProjectsWithRolesWithHeader)
            ListOfInvolvingProjects = Me.CurrentManager.GetPagedInvolvingProjects(oUserID, Me.UserContext.CurrentCommunityID, oPager.PageSize, oPager.Skip, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy, Me.View.CurrentSorting, View.portalName)
            Me.View.LoadInvolvingProjects(ListOfInvolvingProjects)

            Me.View.Pager = oPager
            Me.View.NavigationUrl(oPager.PageSize, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy, Me.View.CurrentSorting)

        End Sub

        Public Sub LoadTaskTabs()
            Dim oList As New List(Of ViewModeType)
            oList.Add(ViewModeType.TodayTasks)
            oList.Add(ViewModeType.InvolvingProjects)
            oList.Add(ViewModeType.TasksManagement)
            oList.Add(ViewModeType.TaskAdmin)

            Me.View.LoadTaskTabs(oList)
        End Sub

        Private Sub LoadFilters()
            Dim oList As New List(Of TaskFilter)
            If Me.UserContext.CurrentCommunityID = 0 Then
                oList.Add(TaskFilter.AllCommunities)
                oList.Add(TaskFilter.PortalPersonal)
                oList.Add(TaskFilter.AllCommunitiesPersonal)
            Else
                oList.Add(TaskFilter.AllCommunities)
                oList.Add(TaskFilter.CurrentCommunity)
                oList.Add(TaskFilter.CommunityPersonal)
                oList.Add(TaskFilter.PortalPersonal)
                oList.Add(TaskFilter.AllCommunitiesPersonal)
            End If
            Me.View.LoadFilters(oList)

            If (oList.Contains(Me.View.PreLoadedCommunityFilter)) Then
                Me.View.CurrentCommunityFilter = Me.View.PreLoadedCommunityFilter
            ElseIf Me.UserContext.CurrentCommunityID > 0 Then
                Me.View.CurrentCommunityFilter = TaskFilter.CurrentCommunity
            Else
                Me.View.CurrentCommunityFilter = TaskFilter.AllCommunities
            End If
        End Sub

        Public Sub LoadSorts()
            Dim oList As New List(Of Sorting)
            oList.Add(Sorting.DeadlineOrder)
            oList.Add(Sorting.AlphabeticalOrder)
            Me.View.LoadSorts(oList)
            If (oList.Contains(Me.View.PreLoadedSorting)) Then
                Me.View.CurrentSorting = Me.View.PreLoadedSorting
            Else
                Me.View.CurrentSorting = Sorting.DeadlineOrder
            End If
        End Sub
        Public Sub Delete(ByVal TaskID As Long)
            Me.CurrentManager.DeleteTask(TaskID)
            Me.LoadInvolvingProjects()
        End Sub
        Public Sub VirtualDelete(ByVal TaskId As Long)
            If Not Me.CurrentManager.GetIfTaskIsDeleted(TaskId) Then
                Me.CurrentManager.DeleteVirtualTask(TaskId, Me.AppContext.UserContext.CurrentUser)
            End If
            Me.LoadInvolvingProjects()
        End Sub

        Public Sub Undelete(ByVal TaskID As Long)
            If Me.CurrentManager.GetIfTaskIsDeleted(TaskID) Then
                Me.CurrentManager.UnDeleteTask(TaskID, Me.AppContext.UserContext.CurrentUser)
            End If
            Me.LoadInvolvingProjects()
        End Sub
        Public Function CanUpdate(ByVal dtoPermission As TaskPermissionEnum)

            If ((dtoPermission And TaskPermissionEnum.AddFile) = TaskPermissionEnum.AddFile) Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.ManagementUser) = TaskPermissionEnum.ManagementUser Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.ProjectDelete) = TaskPermissionEnum.ProjectDelete Then
                Return True
            ElseIf ((dtoPermission And TaskPermissionEnum.TaskCreate) = TaskPermissionEnum.TaskCreate) Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.TaskDelete) = TaskPermissionEnum.TaskDelete Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.TaskSetCategory) = TaskPermissionEnum.TaskSetCategory Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.TaskSetDeadline) = TaskPermissionEnum.TaskSetDeadline Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.TaskSetEndDate) = TaskPermissionEnum.TaskSetEndDate Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.TaskSetPriority) = TaskPermissionEnum.TaskSetPriority Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.TaskSetStartDate) = TaskPermissionEnum.TaskSetStartDate Then
                Return True
            ElseIf (dtoPermission And TaskPermissionEnum.TaskSetStatus) = TaskPermissionEnum.TaskSetStatus Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace

