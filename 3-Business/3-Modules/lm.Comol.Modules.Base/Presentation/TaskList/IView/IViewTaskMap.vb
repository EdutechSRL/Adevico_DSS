Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewTaskMap
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        ReadOnly Property DetailType() As IViewTaskDetail.viewDetailType

        '    ReadOnly Property CurrentViewDetailType() As viewDetailType
        ReadOnly Property CurrentTaskID() As Long
        Property CurrentProjectID() As Long


        'dati x andare alla main page
        ReadOnly Property MainPage() As ViewModeType
        ReadOnly Property Filter() As TaskFilter
        ReadOnly Property PageIndex() As Integer
        ReadOnly Property PageSize() As Integer
        ReadOnly Property TypeOfTask() As TaskManagedType
        ReadOnly Property OrderBy() As TasksPageOrderBy
        'end x main page

        ReadOnly Property CurrentMapType() As viewMapType

        'ReadOnly Property ModulePersmission() As ModuleTaskList

        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        Property IsAdminmode As Boolean

        Property TaskPermission() As TaskPermissionEnum
        Property CurrentCommunityID() As Integer
        Sub ShowError(ByVal ErrorString As String)

        Sub InitMap()
        Sub InitSwichMap()
        Sub InitHyperlinkUrl(ByVal CanSwichTask As Boolean)
        Sub SetTaskName(ByVal TaskName As String)

        Enum viewDetailTabType
            None = -1
            Map = 1
            Detail = 2
        End Enum

        Enum viewMapType
            ClassicMap = 1
            SwichMap = 2
            None = -1
        End Enum




    End Interface
End Namespace

