Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IviewUC_AddTaskAssignment
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        ReadOnly Property ModulePersmission() As lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList

        ReadOnly Property CommunitiesPermission() As IList(Of lm.Comol.Core.DomainModel.ModuleCommunityPermission(Of lm.Comol.Modules.Base.Presentation.TaskList.ModuleTaskList))

        Property CurrentTaskID() As Long
        Property isChild() As Boolean

        Property CurrentRole() As TaskRole

        Property CurrentCommunityID() As Integer

        Property ListOfCommunityID() As List(Of Integer)
        Function GetIfManagerIsResource() As Boolean
        Function GetSelectedUser()
        Sub InitUserSelection(ByVal ListOfPersonToHide As List(Of Integer))
    End Interface
End Namespace