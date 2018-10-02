Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewTaskDetail
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Function RepositoryPermission(ByVal CommunityID As Integer) As CoreModuleRepository


        ReadOnly Property CurrentViewDetailType() As viewDetailType
        ReadOnly Property CurrentTaskID() As Long
        ReadOnly Property SortOfTask() As Sorting
        'dati x setBackUrl
        ReadOnly Property ViewToLoad() As ViewModeType
        ReadOnly Property Filter() As TaskFilter
        ReadOnly Property PageIndex() As Integer
        ReadOnly Property PageSize() As Integer
        ReadOnly Property TypeOfTask() As TaskManagedType
        ReadOnly Property OrderBy() As TasksPageOrderBy
        ReadOnly Property BackUrl() As String
        Property CurrentCommunityID() As Integer
        Property TaskPermission() As TaskPermissionEnum
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))
        ReadOnly Property CommunitiesPermissionCS() As IList(Of ModuleCommunityPermission(Of lm.Comol.Modules.TaskList.ModuleTasklist))

        Sub ShowError(ByVal ErrorString As String)

        Sub InitViewReadOnly(ByVal TaskDetailWithPermission As dtoTaskDetailWithPermission) ', ByVal task As Task) ', ByVal moduleTask As CoreItemPermission)
        Sub InitViewEditable(ByVal TaskDetailWithPermission As dtoTaskDetailWithPermission) ', ByVal task As Task) ', ByVal moduleTask As CoreItemPermission)

        'Gestione File MD

        'Property ItemID() As Long
        Sub LoadFilesToManage(ByVal ItemID As Long, ByVal oModuleTask As CoreItemPermission, ByVal files As IList(Of iCoreItemFileLink(Of Long)), ByVal urlToPublish As String)

        'void LoadFilesToManage(T ItemID, CoreItemPermission oPermission, IList<iCoreItemFileLink<T>> files, string urlToPublish);
        Enum viewDetailType
            None = -1
            Read = 1
            Editable = 2
            'Admin = 3
        End Enum


    End Interface
End Namespace