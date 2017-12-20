Public Class UC_AdvancedProgressBar
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeControl(ByVal bar As lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar, Optional ByVal totalitems As Long = 0)
        If IsNothing(bar) Then
            bar = New lm.Comol.Core.BaseModules.WebControls.Generic.AdvancedProgresBar(totalitems)
        End If
        InitializeControl(bar.Items)
    End Sub
    Private Sub InitializeControl(ByVal items As List(Of lm.Comol.Core.BaseModules.WebControls.Generic.ProgressBarItem))
        RPTcompleteness.DataSource = items
        RPTcompleteness.DataBind()
    End Sub


    Private Sub RPTcompleteness_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcompleteness.ItemDataBound
        Dim item As lm.Comol.Core.BaseModules.WebControls.Generic.ProgressBarItem = e.Item.DataItem
        Dim oControl As HtmlControl = e.Item.FindControl("SPNprogressItem")

        oControl.Attributes("class") = LTspnprogressItem.Text & GetItemCssClass(item.DisplayOrder) & " " & item.CssClass
        Dim oLiteral As Literal = e.Item.FindControl("LTprogresItemTitleStatus")
        oLiteral.Text = item.TranslatePercentage.Replace(",", ".")
        oControl.Attributes("title") = oLiteral.Text
        oLiteral = e.Item.FindControl("LTprogresItemStatusInfo")
        oLiteral.Text = item.TranslateValue
    End Sub

    Private Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
        Dim cssClass As String = ""
        Select Case d
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & d.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function
End Class