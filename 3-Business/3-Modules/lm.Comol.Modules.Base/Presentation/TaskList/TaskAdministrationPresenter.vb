Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Modules.Base.DomainModel
Imports COL_BusinessLogic_v2.Comol.Manager
Imports COL_BusinessLogic_v2.UCServices


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class TaskAdministrationPresenter
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

        Public Overloads ReadOnly Property View() As IViewTaskAdministration
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

        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewTaskAdministration)
            MyBase.New(oContext, view)
            Me.CurrentManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub

#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Me.LoadTaskTabs()
                Me.LoadFilters()

                If Me.View.PreLoadedOrderBy = ProjectOrderBy.None Then
                    Me.View.CurrentOrderBy = ProjectOrderBy.AllActive
                Else
                    Me.View.CurrentOrderBy = Me.View.PreLoadedOrderBy
                End If

                Me.LoadSorts()
                If Me.View.PreLoadedSorting = lm.Comol.Modules.TaskList.Domain.Sorting.None Then
                    Me.View.CurrentSorting = lm.Comol.Modules.TaskList.Domain.Sorting.DeadlineOrder
                Else
                    Me.View.CurrentSorting = Me.View.PreLoadedSorting
                End If

                Me.LoadAdministratedTasks()

                Me.View.SetNavigationUrlToManage(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, TasksPageOrderBy.AllActive, TaskManagedType.Projects, Sorting.DeadlineOrder)
                Me.View.SetNavigationUrlToAddProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy)

                If Me.UserContext.CurrentCommunityID = 0 Then
                    Me.View.SetNavigationUrlToAssignedTask(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, ProjectOrderBy.Community, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.AllCommunities, TasksPageOrderBy.AllActive, Sorting.DeadlineOrder)

                Else
                    Me.View.SetNavigationUrlToAssignedTask(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, ProjectOrderBy.Community, Sorting.DeadlineOrder)
                    Me.View.SetNavigationUrlToProject(Me.View.CurrentPageSize, Me.View.CurrentPageIndex, TaskFilter.CurrentCommunity, TasksPageOrderBy.AllActive, Sorting.DeadlineOrder)

                End If
            End If
        End Sub

        'Private _CommunitiesPermission As IList(Of CommunityDiaryCommunityPermission)
        'Public ReadOnly Property CommunitiesPermission() As IList(Of CommunityDiaryCommunityPermission) 'Implements ICDitemManagementFile.CommunitiesPermission
        '    Get
        '        If IsNothing(_CommunitiesPermission) Then
        '            _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.UserContext.CurrentUser.Id, Services_DiarioLezioni.Codex) _
        '                                      Select New CommunityDiaryCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleCommunityDiary(New Services_DiarioLezioni(sb.PermissionString))}).ToList
        '        End If
        '        Return _CommunitiesPermission
        '    End Get
        'End Proper

        Public Sub LoadAdministratedTasks()
            Dim oUserID As Integer
            oUserID = Me.AppContext.UserContext.CurrentUserID

            Dim oCommunityIDlist As List(Of Integer) = Me.GetPublicAdministeredCommunityIDs(Me.View.CommunitiesPermission)
            
            Dim oPager As New PagerBase
            Dim ItemCount As Integer = 0
            Dim CountList As List(Of Integer) = New List(Of Integer)

            If Me.View.CurrentCommunityFilter = -1 Then
                ItemCount = Me.CurrentManager.GetAdministeredProjectsCount(oCommunityIDlist, Me.View.CurrentOrderBy)
            Else
                CountList.Add(Me.View.CurrentCommunityFilter)
                ItemCount = Me.CurrentManager.GetAdministeredProjectsCount(CountList, Me.View.CurrentOrderBy)
            End If

            'ItemCount = Me.CurrentManager.GetAdministratedTasksCount() '(oCommunityIDlist, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy)
            oPager.Count = ItemCount - 1
            oPager.PageSize = Me.View.CurrentPageSize
            If ItemCount > 0 Then
                oPager.PageIndex = Me.View.CurrentPageIndex
            End If

            ' = New List(Of dtoAssignedTasksWithCommunityHeader)
            Dim ListOfDtoAdministeredTasks As List(Of dtoAdminProjectsWithCommunityHeader) = New List(Of dtoAdminProjectsWithCommunityHeader)
            Dim oList As List(Of Integer) = New List(Of Integer)

            If Me.View.CurrentCommunityFilter = -1 Then
                ListOfDtoAdministeredTasks = Me.CurrentManager.GetAdministeredProjects(oCommunityIDlist, oUserID, oPager.PageSize, oPager.Skip, Me.View.CurrentOrderBy, Me.View.CurrentSorting)
            Else
                'Dim oList As List(Of Integer) = New List(Of Integer)
                oList.Add(Me.View.CurrentCommunityFilter)
                ListOfDtoAdministeredTasks = Me.CurrentManager.GetAdministeredProjects(oList, oUserID, oPager.PageSize, oPager.Skip, Me.View.CurrentOrderBy, Me.View.CurrentSorting)
            End If

            Me.View.LoadAdministredTask(ListOfDtoAdministeredTasks)

            Me.View.Pager = oPager
            Me.View.NavigationUrl(oPager.PageSize, Me.View.CurrentCommunityFilter, Me.View.CurrentOrderBy, Me.View.CurrentSorting) ',Me.View.CurrentViewModeType

                   End Sub

        Private Sub LoadFilters()

            Dim oList As New List(Of dtoCommunityForDDL)
            'Dim oCommunityIDlist As List(Of Integer) = (From s In Me.View.CommunitiesPermission Where (s.Permissions.Administration And s.ID <> 0) Select s.ID).ToList()
            Dim oCommunityIDlist As List(Of Integer) = Me.GetPublicAdministeredCommunityIDs(Me.View.CommunitiesPermission)
            For Each Item In oCommunityIDlist
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(Item)
                Dim oDtoCommunity As New dtoCommunityForDDL(oCommunity)
                oList.Add(oDtoCommunity)
            Next

            Me.View.LoadFilters(oList)

        End Sub

        Public Sub LoadTaskTabs()
            Dim oList As New List(Of ViewModeType)
            oList.Add(ViewModeType.TodayTasks)
            oList.Add(ViewModeType.InvolvingProjects)
            oList.Add(ViewModeType.TasksManagement)
            oList.Add(ViewModeType.TaskAdmin)

            Me.View.LoadTaskTabs(oList)
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


        Public Function GetPublicAdministeredCommunityIDs(ByVal p As List(Of ModuleCommunityPermission(Of ModuleTaskList)))
            Dim oList As List(Of Int32) = (From s In p Where (s.Permissions.Administration And s.ID <> 0) Select s.ID).ToList()
            Return oList
        End Function


        Public Sub VirtualDelete(ByVal TaskId As Long)
            If Not Me.CurrentManager.GetIfTaskIsDeleted(TaskId) Then
                Dim ParentID As Long = Me.CurrentManager.GetParentNameAndID(TaskId).ID
                If ParentID <> TaskId Then
                    Dim BrothersNumber As Integer = Me.CurrentManager.GetNumberOfChildren(ParentID, False)
                    If BrothersNumber = 1 Then
                        Me.View.GoToReallocateResource(TaskId, IViewReallocateUsers.ModeType.VirtualDelete)

                    Else
                        Me.CurrentManager.DeleteVirtualTask(TaskId, Me.AppContext.UserContext.CurrentUser)
                        Me.LoadAdministratedTasks()
                    End If
                Else
                    Me.CurrentManager.DeleteVirtualTask(TaskId, Me.AppContext.UserContext.CurrentUser)
                    Me.LoadAdministratedTasks()
                End If
            Else
                Me.LoadAdministratedTasks()
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
                            Me.LoadAdministratedTasks()
                        End If
                    End If
                ElseIf TaskID = dtoParentSimple.ID Then
                    Me.CurrentManager.UnDeleteTask(TaskID, Me.AppContext.UserContext.CurrentUser)
                    Me.LoadAdministratedTasks()
                Else
                    Me.View.ShowDeletedParentError()
                    Me.LoadAdministratedTasks()
                End If
            Else
                Me.LoadAdministratedTasks()
            End If
        End Sub

        Public Sub Delete(ByVal TaskID As Long)
            Me.CurrentManager.DeleteTask(TaskID)
            Me.LoadAdministratedTasks()
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
