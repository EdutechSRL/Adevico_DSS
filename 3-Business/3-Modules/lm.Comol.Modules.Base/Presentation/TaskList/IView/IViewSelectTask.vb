Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewSelectTask
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentTaskID() As Long
        Property SelectedTaskID() As Long
        Property StartLevel() As Integer

        Function GetSelectedTaskID() As Long

        Sub LoadTask(ByVal ListOfTasks As IList(Of dtoSelectTask))



    End Interface
End Namespace