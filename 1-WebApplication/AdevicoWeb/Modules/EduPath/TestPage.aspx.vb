Public Class TestPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub OB1_OnOrderBy(e As OrderByEventArgs) Handles OB1.OnOrderBy
        OB2.Status = OrderByStatus.None
        OB3.Status = OrderByStatus.None
    End Sub

    Private Sub OB2_OnOrderBy(e As OrderByEventArgs) Handles OB2.OnOrderBy
        OB1.Status = OrderByStatus.None
        OB3.Status = OrderByStatus.None
    End Sub

    Private Sub OB3_OnOrderBy(e As OrderByEventArgs) Handles OB3.OnOrderBy
        OB1.Status = OrderByStatus.None
        OB2.Status = OrderByStatus.None        
    End Sub
End Class