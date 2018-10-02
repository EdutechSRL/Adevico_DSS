Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewGenerateGanttXML
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView
        ReadOnly Property ProjectID() As Long

        Sub GenerateGanttXML(ByVal Project As ProjectForGanttXML)

    End Interface
End Namespace