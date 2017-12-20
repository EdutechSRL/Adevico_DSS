Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_CategoryDDLItem
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub RPTitems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitems.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim item As TK.Domain.DTO.DTO_CategoryTree = e.Item.DataItem
            'Dim PHItem As PlaceHolder = e.Item.FindControl("PHItem")
            Dim PNLitem As Panel = e.Item.FindControl("PNLitem")
            If Not IsNothing(item) Then ' AndAlso Not IsNothing(PHItem) Then

                Dim CtrlItem As UC_CategoryDDLItem = LoadControl("UC_CategoryDDLItem.ascx")
                CtrlItem.BindItem(item, PreSelectedId)


                If Not String.IsNullOrEmpty(CtrlItem.PreSelectedName) Then
                    PreSelectedName = CtrlItem.PreSelectedName
                End If


                'e.Item.Controls.Add(CtrlItem)
                PNLitem.Controls.Add(CtrlItem)

            End If

        End If

    End Sub

    Public Function BindItem(ByVal item As TK.Domain.DTO.DTO_CategoryTree, ByVal SelectedId As Int64) As String

        PreSelectedId = SelectedId

        LTcatItem.Text = LTcatItem.Text.Replace("{CatId}", item.Id).Replace("{CatName}", item.Name).Replace("{CatDesc}", item.Description)

        If (item.Id = SelectedId) Then
            LTcatItem.Text = LTcatItem.Text.Replace("{CatSel}", " activeselected")
            PreSelectedName = item.Name
        Else
            LTcatItem.Text = LTcatItem.Text.Replace("{CatSel}", "")
        End If

        If (item.IsSelectable) Then
            LTcatItem.Text = LTcatItem.Text.Replace("{CatEn}", "")
        Else
            LTcatItem.Text = LTcatItem.Text.Replace("{CatEn}", " disabled")
        End If

        If Not IsNothing(item.Children) AndAlso item.Children.Count > 0 Then
            Me.RPTitems.DataSource = item.Children.OrderBy(Function(x) x.Order).ToList()
            Me.RPTitems.DataBind()
        End If

        Return PreSelectedName

    End Function

    Private PreSelectedId As Int64 = 0

    Public PreSelectedName As String = ""
End Class