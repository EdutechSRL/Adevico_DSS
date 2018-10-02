Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    Public Class AssignedTasksPresenter
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
        Public Overloads ReadOnly Property View() As IViewAssignedTasks
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
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewAssignedTasks)
            MyBase.New(oContext, view)
            Me.CurrentManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
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

            If Not Me.UserContext.isAnonymous Then
                Me.LoadTaskTabs()
                Me.LoadFilters()
                If Me.View.PreLoadedOrderBy = lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy.None Then
                    Me.View.CurrentOrderBy = lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy.Community
                Else
                    Me.View.CurrentOrderBy = Me.View.PreLoadedOrderBy
                End If

                Me.LoadSorts()
                If Me.View.PreLoadedSorting = lm.Comol.Modules.TaskList.Domain.Sorting.None Then
                    Me.View.CurrentSorting = lm.Comol.Modules.TaskList.Domain.Sorting.DeadlineOrder
                Else
                    Me.View.CurrentSorting = Me.View.PreLoadedSorting
                End If


                Me.LoadAssignedTasks()
                Me.View.SetNavigationUrlToAssignedTask(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, TasksPageOrderBy.Community, Sorting.DeadlineOrder)
                Me.View.SetNavigationUrlToAddProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy, Sorting.DeadlineOrder)

                Me.View.SetNavigationUrlToAdministration(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, ProjectOrderBy.AllActive, Sorting.DeadlineOrder)

                If Me.UserContext.CurrentCommunityID = 0 Then
                    Me.View.SetNavigationUrlToProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, TasksPageOrderBy.AllActive, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToManage(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, TasksPageOrderBy.AllActive, TaskManagedType.Projects, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToAdministration(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, ProjectOrderBy.AllActive, Sorting.DeadlineOrder)

                Else
                    Me.View.SetNavigationUrlToProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, TasksPageOrderBy.AllActive, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToManage(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, TasksPageOrderBy.AllActive, TaskManagedType.Projects, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToAdministration(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, ProjectOrderBy.AllActive, Sorting.DeadlineOrder)
                End If
            End If
        End Sub

        Public Sub LoadAssignedTasks()

            Dim oUserID As Integer
            oUserID = Me.AppContext.UserContext.CurrentUserID

            Dim oPager As New PagerBase
            Dim ItemCount As Integer = 0
            'Task assegnati alla persona per:  la comunità corrente, tutte le comunità, i task personali della comunità corrente, i task personali di portale.
            ItemCount = Me.CurrentManager.GetAssignedTasksCount(oUserID, Me.View.CurrentCommunityFilter, Me.UserContext.CurrentCommunityID)
            oPager.Count = ItemCount - 1
            oPager.PageSize = Me.View.CurrentPageSize
            If ItemCount > 0 Then
                oPager.PageIndex = Me.View.CurrentPageIndex
            End If

            If Me.View.CurrentOrderBy = lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy.Community Then
                Dim ListOfDtoAssignedTaskByCommunity As List(Of dtoAssignedTasksWithCommunityHeader) = New List(Of dtoAssignedTasksWithCommunityHeader)
                ListOfDtoAssignedTaskByCommunity = Me.CurrentManager.GetPagedAssignedTasksByCommunity(oUserID, oPager.PageSize, oPager.Skip, Me.View.CurrentCommunityFilter, Me.UserContext.CurrentCommunityID, Me.View.CurrentSorting, View.portalname)
                Me.View.LoadAssignedTasksByCommunity(ListOfDtoAssignedTaskByCommunity)

            Else
                Dim ListOfDtoAssignedTaskByProject As New List(Of dtoAssignedTasksWithProjectHeader)
                ListOfDtoAssignedTaskByProject = Me.CurrentManager.GetPagedAssignedTasksByProject(oUserID, oPager.PageSize, oPager.Skip, Me.View.CurrentCommunityFilter, Me.UserContext.CurrentCommunityID, Me.View.CurrentSorting, View.portalname)
                Me.View.LoadAssignedTasksByProject(ListOfDtoAssignedTaskByProject)
            End If

            Me.View.Pager = oPager
            Me.View.NavigationUrl(oPager.PageSize, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy, Me.View.CurrentSorting) ',Me.View.CurrentViewModeType
        End Sub

        'Carica una lista di String corrispondenti ai termini di ricerca dei Tasks assegnati 
        Public Sub LoadFilters()
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

        Public Sub LoadTaskTabs()
            Dim oList As New List(Of ViewModeType)
            oList.Add(lm.Comol.Modules.TaskList.Domain.ViewModeType.TodayTasks)
            oList.Add(lm.Comol.Modules.TaskList.Domain.ViewModeType.InvolvingProjects)
            oList.Add(lm.Comol.Modules.TaskList.Domain.ViewModeType.TasksManagement)

            If (From c In Me.View.CommunitiesPermission Where c.Permissions.Administration).Count > 0 Then
                oList.Add(lm.Comol.Modules.TaskList.Domain.ViewModeType.TaskAdmin)
            End If

            Me.View.LoadTaskTabs(oList)

        End Sub

        Public Sub Delete(ByVal TaskID As Long)
            Me.CurrentManager.DeleteTask(TaskID)
            Me.LoadAssignedTasks()
        End Sub

        Public Sub VirtualDelete(ByVal TaskId As Long)
            If Not Me.CurrentManager.GetIfTaskIsDeleted(TaskId) Then
                Dim ParentID As Long = Me.CurrentManager.GetParentNameAndID(TaskId).ID
                If ParentID <> TaskId Then
                    Dim BrothersNumber As Integer = Me.CurrentManager.GetNumberOfChildren(ParentID, False)
                    If BrothersNumber = 1 Then
                        Me.View.GoToReallocateResource(TaskId, IViewReallocateUsers.ModeType.VirtualDelete)
                    Else
                        Me.CurrentManager.DeleteVirtualTask(TaskId, Me.AppContext.UserContext.CurrentUser)
                        Me.LoadAssignedTasks()
                    End If
                Else
                    Me.CurrentManager.DeleteVirtualTask(TaskId, Me.AppContext.UserContext.CurrentUser)
                    Me.LoadAssignedTasks()
                End If
            Else
                Me.LoadAssignedTasks()
            End If
        End Sub

        Public Sub Undelete(ByVal TaskID As Long)
            If Me.CurrentManager.GetIfTaskIsDeleted(TaskID) Then
                Dim dtoParentSimple As dtoTaskSimple = Me.CurrentManager.GetParentNameAndID(TaskID)
                If Not dtoParentSimple.isDeleted Then
                    Dim ActiveResourceNumberOfParent As Integer = Me.CurrentManager.GetTaskAssignmentCount(dtoParentSimple.ID, TaskRole.Resource, False)
                    If ActiveResourceNumberOfParent > 0 Then
                        Me.View.GoToReallocateResource(TaskID, IViewReallocateUsers.ModeType.Undelete)
                    Else
                        Dim DeletedResourceNumberOfParent As Integer = Me.CurrentManager.GetTaskAssignmentCount(dtoParentSimple.ID, TaskRole.Resource, False)
                        If DeletedResourceNumberOfParent > 0 Then
                            Me.CurrentManager.DeleteResourcesTaskAssignments(dtoParentSimple.ID)
                        Else
                            Me.CurrentManager.UnDeleteTask(TaskID, Me.AppContext.UserContext.CurrentUser)
                            Me.LoadAssignedTasks()
                        End If
                    End If
                Else
                    'errore da mostrare
                    Me.View.ShowDeletedParentError()
                    Me.LoadAssignedTasks()
                End If
            Else
                Me.LoadAssignedTasks()
            End If
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
