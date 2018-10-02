Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewAddProject
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        'dati x setBackUrl
        ReadOnly Property ViewToLoad() As ViewModeType
        ReadOnly Property Filter() As TaskFilter
        ReadOnly Property PageIndex() As Integer
        ReadOnly Property PageSize() As Integer
        ReadOnly Property OrderBy() As TasksPageOrderBy
        ReadOnly Property TypeOfTask() As TaskManagedType
        'end x setrBackUrl

        ReadOnly Property ModulePermission() As ModuleTaskList

        Property UrlTaskID As Long

        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        ReadOnly Property BackUrl() As String

        Property isPortal() As Boolean
        Property isPersonal() As Boolean
        Property CurrentCommunityID() As Integer

        Sub ShowError(ByVal ErrorString As String)

        '  Sub SetUrl()

        Property CurrentStep() As StepType
        Property dtoProject() As dtoTaskDetail
        Property SessionUniqueKey() As System.Guid
        Sub ClearUniqueKey()
        Function GetProject() As Task

        Sub InitSelectProjectType(ByVal oList As List(Of IViewAddProject.viewTaskListType))

        Sub InitCommunitySelection(ByVal oListCommunitiesID As System.Collections.Generic.List(Of Integer))
        Sub InitSetProjectProperty(ByVal TaskDetailWithPermission As dtoTaskDetailWithPermission, ByVal ViewDetailType As IViewUC_TaskDetail.viewDetailType, ByVal BackUrl As String)
        Sub LoadNoPermissionToCreate(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub GoBackPage(ByVal Action As Services_TaskList.ActionType)


        Enum StepType
            SelectType = 0
            SelectCommunity = 1
            SetProperty = 2
        End Enum


        Enum viewTaskListType
            None = 0
            Personal = 1
            PersonalCommunity = 2
            Community = 3
        End Enum

    End Interface
End Namespace