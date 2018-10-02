Imports lm.Comol.Modules.Base.Presentation.TaskList
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IViewUC_QuickSelectionTaskUsers
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Sub LoadQuickSelUsers(ByVal olist As List(Of dtoUsers))

        Property CurrentTaskID() As Long
        Property PreLoadedRole() As TaskRole
        Property CurrentRole() As TaskRole
        Property isChild() As Boolean

        'ReadOnly Property Taskrole() As TaskRole

        Sub LoadRoles(ByVal list As List(Of TaskRole))


        Function GetSelectedUser(ByVal oRole As String) As Object
        Function GetQuickSelectionUsers() As List(Of Person)
        'Function GetQuickSelectionUsersID() As List(Of Integer)

        'Function GetSelectedUser()
        'Function GetIfManagerIsResource() As Boolean

        Sub NavigationUrl(ByVal taskRole As TaskRole, ByVal TaskID As Long)


    End Interface

End Namespace
