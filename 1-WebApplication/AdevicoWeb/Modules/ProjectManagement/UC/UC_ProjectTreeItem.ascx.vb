Imports lm.Comol.Modules.Standard.ProjectManagement.Domain
Public Class UC_ProjectTreeItem
    Inherits System.Web.UI.UserControl
    Private _currentShortDatePattern As String

    Public Sub InitializeControl(ByVal task As dtoActivityTreeItem, currentShortDatePattern As String)
        _currentShortDatePattern = currentShortDatePattern
        LTitemContainer.Text = LTitemContainer.Text.Replace("{id}", task.Id)
        LBtaskName.Text = task.Name
        LBtaskLinks.Text = task.Predecessors
        LBtaskDuration.Text = task.Duration.ToString
        If task.EarlyStartDate.HasValue Then
            LBtaskStartDate.Text = task.EarlyStartDate.Value.ToString(currentShortDatePattern)
        End If
        If task.EarlyFinishDate.HasValue Then
            LBtaskEndDate.Text = task.EarlyFinishDate.Value.ToString(currentShortDatePattern)
        End If
        If Not IsNothing(task.Children) OrElse task.Children.Count > 0 Then
            Me.RPTchildren.DataSource = task.Children
            Me.RPTchildren.DataBind()
        End If
    End Sub
    Public Sub ReloadPlacheHoldersOnPostBack()
        For Each item As RepeaterItem In RPTchildren.Items
            Dim oPlaceHolder As PlaceHolder = item.FindControl("PLHchild")
            If Not oPlaceHolder.HasControls Then
                Dim oControl As UC_ProjectTreeItem = LoadControl("UC_ProjectTreeItem.ascx")
                oPlaceHolder.Controls.Add(oControl)
            End If

        Next
    End Sub
    Private Sub RPTchildren_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTchildren.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim task As dtoActivityTreeItem = e.Item.DataItem
            If Not IsNothing(task) Then
                Dim oPlaceHolder As PlaceHolder = e.Item.FindControl("PLHchild")

                Dim oControl As UC_ProjectTreeItem = LoadControl("UC_ProjectTreeItem.ascx")
                oControl.InitializeControl(task, _currentShortDatePattern)
                oPlaceHolder.Controls.Add(oControl)
            End If
        End If
    End Sub
End Class
