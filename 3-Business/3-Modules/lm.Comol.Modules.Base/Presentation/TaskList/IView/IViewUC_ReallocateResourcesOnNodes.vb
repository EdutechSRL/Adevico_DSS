Imports lm.Comol.Modules.TaskList.Domain

Namespace lm.Comol.Modules.Base.Presentation.TaskList
    <CLSCompliant(True)> Public Interface IViewUC_ReallocateResourcesOnNodes
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property CurrentEditMode() As EditType
        Sub LoadResources(ByVal oList As List(Of dtoReallocateTAWithHeader))
        Function GetUserList() As List(Of dtoReallocateTA)


        Enum EditType
            None = -1
            Read = 1
            Edit = 2
            EditNoButton = 3
        End Enum

    End Interface
End Namespace