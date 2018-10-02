Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoNoticeBoard
        Inherits DomainObject(Of Long)


        Public Style As TextStyle
        Public Message As String
        Public isDeleted As Boolean
        Public MessageDate As DateTime

        Sub New()

        End Sub
    End Class
End Namespace