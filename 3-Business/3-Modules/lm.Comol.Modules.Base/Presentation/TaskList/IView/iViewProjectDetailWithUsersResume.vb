
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain
Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface iViewProjectDetailWithUsersResume
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property TaskPermission As TaskPermissionEnum

        ReadOnly Property BackUrl As String

        ReadOnly Property DetailUrl As String

        ReadOnly Property CurrentProjectID As Long

        'Property ProjectName As String

        ReadOnly Property ViewModeType As Modules.TaskList.Domain.ViewModeType

        Sub InitView(ByVal ProjectID As Long, ByVal ProjectName As String)

        Sub ShowError(ByVal oMessage As String)

    End Interface
End Namespace