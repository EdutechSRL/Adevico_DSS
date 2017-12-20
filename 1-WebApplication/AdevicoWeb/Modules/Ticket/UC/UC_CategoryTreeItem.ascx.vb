Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_CategoryTreeItem
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub BindCategoryItem(ByVal Cat As TK.Domain.DTO.DTO_CategoryTree)
        
        Dim Css As String = ""

        If Cat.IsDefault Then
            Css &= " default"
        End If

        If Cat.IsDeleted Then
            Css &= " deleted"
        End If

        Dim Text As String = Cat.Name
        Dim Icon As String = ""


        If Not String.IsNullOrEmpty(Cat.Icon) Then
            Icon = LTiconTemplate.Text.Replace("{icon}", Cat.Icon)
        End If

        LTitem.Text = LTitem.Text.Replace("{id}", Cat.Id).Replace("{css}", Css).Replace("{icon}", Icon).Replace("{text}", Text)

        If Not IsNothing(Cat.Children) OrElse Cat.Children.Count > 0 Then
            Me.RPTchildren.DataSource = Cat.Children.OrderBy(Function(x) x.Order).ToList()
            Me.RPTchildren.DataBind()
        End If

    End Sub

    Private Sub RPTchildren_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTchildren.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim cat As TK.Domain.DTO.DTO_CategoryTree = e.Item.DataItem
            Dim PHChildren As PlaceHolder = e.Item.FindControl("PHChildren")
            PHChildren.Controls.Clear()

            If Not IsNothing(cat) And Not IsNothing(PHChildren) Then

                Dim CtrlItem As UC_CategoryTreeItem = LoadControl("UC_CategoryTreeItem.ascx")
                CtrlItem.BindCategoryItem(cat)
                PHChildren.Controls.Add(CtrlItem)

            End If

        End If
    End Sub

End Class