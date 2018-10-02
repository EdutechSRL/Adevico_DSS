Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoWorkBookStatus
        Public isDefault As Boolean
        Public ID As Integer
        Public Name As String
        Public AvailableForPermission As EditingPermission
        Public ItemsCount As Long
        Public WorkbookCount As Long


        Public Sub New()

        End Sub
    End Class
End Namespace