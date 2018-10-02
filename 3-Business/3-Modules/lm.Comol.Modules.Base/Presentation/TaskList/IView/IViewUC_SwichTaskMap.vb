Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewUC_SwichTaskMap
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        Property CurrentTaskID() As Long

        Property StartLevel() As Integer
        Property LastLevel() As Integer

        Sub LoadTasks(ByVal ListOfTasks As List(Of dtoSwichTask))
        Sub LoadDdlWBSlevel()

    End Interface
End Namespace