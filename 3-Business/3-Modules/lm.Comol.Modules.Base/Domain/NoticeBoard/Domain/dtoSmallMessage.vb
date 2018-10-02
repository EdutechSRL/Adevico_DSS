Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoSmallMessage
        Inherits DomainObject(Of Long)

        Public Author As Person
        Public isDeleted As Boolean
        Public MessageDate As String
        Public Url As String

        Sub New()

        End Sub
        Sub New(ByVal oNoticeBoard As NoticeBoard)
            Id = oNoticeBoard.Id
            If Not oNoticeBoard.ModifiedOn.HasValue OrElse oNoticeBoard.ModifiedOn.Equals(New Date) Then
                MessageDate = FormatDateTime(oNoticeBoard.CreatedOn, DateFormat.ShortDate) & " " & FormatDateTime(oNoticeBoard.CreatedOn, DateFormat.ShortTime)
            Else
                MessageDate = FormatDateTime(oNoticeBoard.ModifiedOn, DateFormat.ShortDate) & " " & FormatDateTime(oNoticeBoard.ModifiedOn, DateFormat.ShortTime)
            End If

            isDeleted = oNoticeBoard.isDeleted
            If IsNothing(oNoticeBoard.ModifiedBy) Then
                Author = oNoticeBoard.CreatedBy
            Else
                Author = oNoticeBoard.ModifiedBy
            End If

        End Sub
    End Class
End Namespace