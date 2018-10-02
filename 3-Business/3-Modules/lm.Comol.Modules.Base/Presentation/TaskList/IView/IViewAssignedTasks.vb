Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel


Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewAssignedTasks
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property ModulePersmission() As ModuleTaskList
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedView() As ViewModeType
        ReadOnly Property PreLoadedOrderBy() As TasksPageOrderBy
        ReadOnly Property PreLoadedCommunityFilter() As TaskFilter
        ReadOnly Property CurrentPageIndex() As Integer
        Property CurrentPageSize() As Integer
        Property CurrentCommunityFilter() As TaskFilter
        Property CurrentOrderBy() As TasksPageOrderBy
        Property CurrentSorting() As Sorting
        ReadOnly Property PortalName As String
        'Property PreLoadedSorting() As Sorting

        ReadOnly Property PreLoadedSorting As Sorting


        Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting) ', ByVal ViewMode As ViewModeType
        Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal TaskType As TaskManagedType, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting)

        Sub LoadAssignedTasksByCommunity(ByVal oList As List(Of dtoAssignedTasksWithCommunityHeader))
        Sub LoadAssignedTasksByProject(ByVal oList As List(Of dtoAssignedTasksWithProjectHeader))

        Sub LoadFilters(ByVal oList As List(Of TaskFilter))
        Sub LoadSorts(ByVal oList As List(Of Sorting))
        Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType))
        Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As lm.Comol.Modules.Base.Presentation.TaskList.IViewReallocateUsers.ModeType)

        Sub ShowDeletedParentError()
    End Interface
End Namespace


