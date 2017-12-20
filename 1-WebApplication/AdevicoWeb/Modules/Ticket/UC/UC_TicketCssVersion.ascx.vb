Public Class UC_TicketCssVersion
    Inherits System.Web.UI.UserControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetVersionString() As String
        Return Me.LTticketCssVersion.Text
    End Function
End Class