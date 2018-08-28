Public Class Uc_StarBind
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property RadRating As Telerik.Web.UI.RadRating
        Get
            Return Me.RD_Star
        End Get
        Set(value As Telerik.Web.UI.RadRating)
            RD_Star = value
        End Set
    End Property

End Class