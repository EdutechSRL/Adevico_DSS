Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel


Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewInvolvingProjects
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property Pager() As lm.Comol.Core.DomainModel.PagerBase

        ReadOnly Property ModulePersmission() As ModuleTaskList
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        ReadOnly Property PreLoadedPageSize() As Integer
        Property CurrentPageSize() As Integer

        ReadOnly Property CurrentPageIndex() As Integer

        ReadOnly Property PreLoadedView() As ViewModeType

        Property CurrentSorting() As lm.Comol.Modules.TaskList.Domain.Sorting
        ReadOnly Property PortalName As String
        Sub LoadSorts(ByVal oList As List(Of Sorting))

        ReadOnly Property PreLoadedSorting As Sorting

        ReadOnly Property PreLoadedCommunityFilter() As TaskFilter
        Property CurrentCommunityFilter() As TaskFilter
        Property CurrentOrderBy() As TasksPageOrderBy
        ReadOnly Property PreLoadedOrderBy() As TasksPageOrderBy

        Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting) ', ByVal ViewMode As ViewModeType

        Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal TaskType As TaskManagedType, ByVal SortBy As Sorting) ', ByVal OrderBy As ProjectOrderBy)
        Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting)

        Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As TasksPageOrderBy, ByVal SortBy As Sorting)

        Sub LoadFilters(ByVal oList As List(Of TaskFilter))
        Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType))
        Sub LoadInvolvingProjects(ByVal oList As System.Collections.Generic.List(Of dtoInvolvingProjectsWithRolesWithHeader))

        Sub ShowDeletedParentError()

        'Function CanUpdate(ByVal dtoPermission As lm.Modules.TaskList.DomainModel.Enum.TaskPermissionEnum)
        Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As IViewReallocateUsers.ModeType)
    End Interface

End Namespace

