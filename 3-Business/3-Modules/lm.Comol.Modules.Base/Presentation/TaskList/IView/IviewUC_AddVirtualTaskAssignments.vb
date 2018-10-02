Imports lm.Comol.Core.DomainModel
Imports Comol.Entity
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IviewUC_AddVirtualTaskAssignments
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        ReadOnly Property ModulePersmission() As ModuleTaskList

        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleTaskList))

        Property CurrentTaskID() As Long
        Property ListOfAssignedPersonWithRole() As List(Of dtoUserWithRole)

        Property CurrentRole() As TaskRole

        Property CurrentCommunityID() As Integer

        Property ListOfCommunityID() As List(Of Integer)

        Function GetSelectedUser()
        Sub InitUserSelection(ByVal ListOfPersonToHide As List(Of Integer))
        Sub LoadRole(ByVal listOfRole As List(Of TaskRole))
        Function GetCurretnRole()
    End Interface
End Namespace