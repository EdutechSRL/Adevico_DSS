Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewTasksManagement
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        Property Pager() As lm.Comol.Core.DomainModel.PagerBase

        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedView() As ViewModeType
        ReadOnly Property PreLoadedOrderBy() As ProjectOrderBy
        ReadOnly Property PreLoadedCommunityFilter() As TaskFilter
        ReadOnly Property CurrentPageIndex() As Integer
        Property CurrentPageSize() As Integer
        Property CurrentCommunityFilter() As TaskFilter
        Property CurrentOrderBy() As ProjectOrderBy


        ReadOnly Property ModulePersmission() As ModuleTaskList
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        ReadOnly Property PreLoadedTaskTypeSelected() As TaskManagedType
        Property CurrentTaskTypeSelected() As TaskManagedType

        Sub LoadSorts(ByVal oList As List(Of Sorting))
        ReadOnly Property PreLoadedSorting As Sorting
        ReadOnly Property PortalName As String
        Property CurrentSorting() As lm.Comol.Modules.TaskList.Domain.Sorting



        Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal Type As TaskManagedType, ByVal SortBy As Sorting) ', ByVal ViewMode As ViewModeType
        Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal Type As TaskManagedType, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting)

        Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal Tasktype As TaskManagedType)

        Sub LoadFilters(ByVal oList As List(Of TaskFilter))
        Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType))

        Sub ShowDeletedParentError()

        Sub LoadManagedTasks(ByVal oList As System.Collections.Generic.List(Of dtoAssignedTasksWithCommunityHeader))
        Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As IViewReallocateUsers.ModeType)
    End Interface

End Namespace
