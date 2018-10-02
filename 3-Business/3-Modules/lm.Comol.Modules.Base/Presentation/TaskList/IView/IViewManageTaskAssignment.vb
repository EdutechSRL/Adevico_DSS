Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel


Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewManageTaskAssignment
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        ReadOnly Property ModulePersmission() As ModuleTaskList

        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        ReadOnly Property CurrentTaskID() As Long
        ReadOnly Property ViewType() As viewAssignmentType
        ReadOnly Property BackUrl() As String
        ReadOnly Property Url() As String
        ReadOnly Property ViewModeType As ViewModeType

        ' Property CurrentRole() As TaskRole
        Property TaskPermission() As TaskPermissionEnum
        Property CurrentCommunityID() As Integer

        'Property ListOfCommunityID() As List(Of Integer)
        Sub SetTaskName(ByVal WBS As String, ByVal Name As String)
        Sub InitUserSelection()
        Sub InitAssignedPersons()
        Sub ShowError(ByVal ErrorString As String)


        Enum viewAssignmentType
            None = 0
            AddTaskAssignment = 1
            ViewTaskAssignment = 2
            AddQuickTaskAssignment = 3
        End Enum


    End Interface
End Namespace