Public Class UC_CommunitiesTreeHeader
    Inherits DBbaseControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

#Region "Internal"
    Public Sub SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter))
        CTRLfiltersHeader.SetDefaultFilters(filters)
    End Sub
    Public Sub SetTransacionIdContainer(value As String)
        CTRLfiltersHeader.SetTransacionIdContainer(value)
    End Sub
#End Region
End Class