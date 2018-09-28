Public Class UC_ProjectManagementHeader
    Inherits System.Web.UI.UserControl
#Region "Internal"
    Public WriteOnly Property LoadProgressBarHeader As Boolean
        Set(value As Boolean)
            CTRLadvancedProgressBarHeader.Visible = value
        End Set
    End Property
    Public WriteOnly Property LoadTaskHeader As Boolean
        Set(value As Boolean)
            CTRLdialogHeader.Visible = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class