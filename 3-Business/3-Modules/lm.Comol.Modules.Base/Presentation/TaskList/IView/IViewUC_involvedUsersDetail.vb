Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList


    <CLSCompliant(True)> Public Interface IViewUC_involvedUsersDetail
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentTaskID As Long

        Sub InitView(ByVal oList As List(Of dtoUsersOnQuickSelection))


    End Interface
End Namespace