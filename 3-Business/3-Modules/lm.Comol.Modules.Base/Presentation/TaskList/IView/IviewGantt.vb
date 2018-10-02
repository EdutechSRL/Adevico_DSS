
Imports lm.Comol.Modules.TaskList.Domain


Namespace lm.Comol.Modules.Base.Presentation.TaskList

    <CLSCompliant(True)> Public Interface IviewGantt
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property TaskID() As Long
        Property ProjectID() As Long

        ReadOnly Property PreviusPageType() As PageType

        Sub ShowError(ByVal ErrorString As String)
        Sub ShowGantt(ByVal Title As String)

        Enum PageType
            SwichMap
            GeneralMap
            DetailRead
            DetailUpdate
            None
        End Enum

    End Interface
End Namespace