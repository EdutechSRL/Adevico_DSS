Public Class Uc_CategoryTree
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Public Sub BindTree(ByVal Categories As IList(Of lm.Comol.Core.BaseModules.Tickets.Domain.Category), ByVal IsMain As Boolean, ByVal CurLanguageCode As String)

        If (IsMain) Then
            Me.UlMain.Attributes.Add("class", LTulMainCssClass.Text)
        End If

        Me.RPTcategories.DataSource = Categories.OrderBy(Function(x) x.Order).ToList()
        Me.RPTcategories.DataBind()
    End Sub

    Public ReadOnly Property SelectedCategoryId As Integer
        Get
            Return 0
        End Get
    End Property

    Private _CurLangCode As String = ""

    Private Property CurLangCode
        Get
            Return _CurLangCode
        End Get
        Set(value)
            _CurLangCode = value
        End Set
    End Property


    Private Sub RPTcategories_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcategories.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If Not IsNothing(e.Item.DataItem) Then
                Dim Category As lm.Comol.Core.BaseModules.Tickets.Domain.Category = e.Item.DataItem

                Dim LBL_Name As Label = e.Item.FindControl("LBname")
                LBL_Name.Text = Category.GetTranslatedName(CurLangCode)
                LBL_Name.ToolTip = Category.GetTranslatedDescription(CurLangCode)

                Dim PH_Children As PlaceHolder = e.Item.FindControl("PHchildren")
                If Not IsNothing(PH_Children) Then

                    PH_Children.Controls.Clear()

                    'Dim UC_Categories As New Uc_CategoryTree() ' = e.Item.FindControl("UC_Categories")
                    Dim CTRLcategories As Uc_CategoryTree = LoadControl("UC_CategoryTree.ascx")
                    'PH_Children.Controls.Add(UC_Categories)

                    If Not IsNothing(CTRLcategories) AndAlso Not IsNothing(Category.Children) AndAlso Category.Children.Count > 0 Then
                        CTRLcategories.BindTree(Category.Children, False, CurLangCode)
                    Else
                        Dim LiCategory As HtmlControl = e.Item.FindControl("LiCategory")
                        LiCategory.Attributes.Add("class", Me.LTliLeaftCssClass.Text)
                    End If

                    PH_Children.Controls.Add(CTRLcategories)
                    'PH_Children.DataBind()
                End If



            End If

        End If
    End Sub


    Public Property Visible As Boolean
        Get
            Return Me.RPTcategories.Visible
        End Get
        Set(value As Boolean)
            Me.RPTcategories.Visible = value
        End Set
    End Property
End Class