Public Class Test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LT_test.Text = Session("Track")
    End Sub

End Class