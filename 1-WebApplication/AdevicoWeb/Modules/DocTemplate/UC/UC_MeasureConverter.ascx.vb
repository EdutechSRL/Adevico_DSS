Public Class UC_MeasureConverter
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            UC_MeasureOut.SetMeasure()
            UC_MeasureIn.SetMeasure()
        End If
    End Sub

#Region "Internal"
    Private Sub BindMeasure()
        UC_MeasureOut.Px = UC_MeasureIn.Px
        UC_MeasureOut.SetMeasure(UC_MeasureOut.GetCurrentMeasure())
    End Sub
#End Region

#Region "Handler"
    Private Sub UC_MeasureIn_SelectedMeasureChange() Handles UC_MeasureIn.SelectedMeasureChange
        BindMeasure()
    End Sub
    Private Sub UC_MeasureOut_SelectedMeasureChange() Handles UC_MeasureOut.SelectedMeasureChange
        BindMeasure()
    End Sub
    Private Sub LKB_Calculate_Click(sender As Object, e As System.EventArgs) Handles LKB_Calculate.Click
        BindMeasure()
    End Sub
#End Region

End Class