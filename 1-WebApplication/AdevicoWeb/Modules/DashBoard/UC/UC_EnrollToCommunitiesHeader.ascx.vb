Public Class UC_EnrollToCommunitiesHeader
    Inherits DBbaseControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        '    CTRLfiltersHeader.SetTransacionIdContainer(Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID & "_" & PageUtility.CurrentContext.UserContext.WorkSessionID.ToString)
        'End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter))
        CTRLfiltersHeader.SetDefaultFilters(filters)
    End Sub
    Public Sub SetTransacionIdContainer(value As String)
        CTRLfiltersHeader.SetTransacionIdContainer(value)
    End Sub
#End Region

End Class