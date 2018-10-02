Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.TaskList.Domain


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewTaskDetails
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentTaskListContext() As TaskListContext
        ReadOnly Property TaskID() As Long
        'Property TaskName() As String
        'Property TaskDescription() As String
        'Property TaskCompleteness() As Integer


        Sub DisplayTask(ByVal oTask As Object)
        Sub SetEditing(ByVal Permission As Integer)
        WriteOnly Property UpdateEnabled() As Boolean
        WriteOnly Property PreviousPage() As String
        Sub UpdateTask()

    End Interface
End Namespace
