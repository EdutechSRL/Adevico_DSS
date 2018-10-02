Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewTaskAdministration
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        Property Pager() As lm.Comol.Core.DomainModel.PagerBase

        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedView() As ViewModeType
        ReadOnly Property PreLoadedOrderBy() As ProjectOrderBy
        ReadOnly Property PreLoadedCommunityFilter() As Integer
        ReadOnly Property CurrentPageIndex() As Integer
        Property CurrentPageSize() As Integer
        Property CurrentCommunityFilter() As Integer
        Property CurrentOrderBy() As ProjectOrderBy
        Property CurrentSorting() As lm.Comol.Modules.TaskList.Domain.Sorting
        'Property ListAdminCommunity() As List(Of Integer)

        'ReadOnly Property ModulePersmission() As ModuleTaskList
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        Sub LoadSorts(ByVal oList As List(Of Sorting))
        ReadOnly Property PreLoadedSorting As Sorting

        'ReadOnly Property PreLoadedTaskTypeSelected() As TaskManagedType
        ' Property CurrentTaskTypeSelected() As TaskManagedType

        Sub NavigationUrl(ByVal PageSize As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal SortBy As Sorting) ', ByVal ViewMode As ViewModeType

        Sub SetNavigationUrlToAssignedTask(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToManage(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy, ByVal Type As TaskManagedType, ByVal SortBy As Sorting)
        Sub SetNavigationUrlToAdministration(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As lm.Comol.Modules.TaskList.Domain.TaskFilter, ByVal OrderBy As lm.Comol.Modules.TaskList.Domain.TasksPageOrderBy, ByVal SortBy As Sorting)

        Sub SetNavigationUrlToAddProject(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As TaskFilter, ByVal OrderBy As ProjectOrderBy)

        Sub LoadFilters(ByVal oList As List(Of lm.Comol.Modules.TaskList.Domain.dtoCommunityForDDL))
        Sub LoadTaskTabs(ByVal oList As List(Of ViewModeType))

        Sub ShowDeletedParentError()

        Sub GoToReallocateResource(ByVal TaskID As Long, ByVal ReallocateType As IViewReallocateUsers.ModeType)

        Sub LoadAdministredTask(ByVal ListOfDtoAdministeredTasks As List(Of dtoAdminProjectsWithCommunityHeader))

    End Interface

End Namespace
