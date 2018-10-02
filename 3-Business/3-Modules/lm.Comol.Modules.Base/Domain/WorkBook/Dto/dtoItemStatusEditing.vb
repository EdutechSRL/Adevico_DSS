Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoItemStatusEditing
        Public ItemId As System.Guid
        Public StatusId As Integer
        Public Editing As EditingPermission
    End Class
End Namespace