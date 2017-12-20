Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_CategoryDDL
    Inherits BaseControl

    Private _SelectedName As String = ""
    Private Property _DDLdisplayText As String '= ""
        Get
            Return ViewStateOrDefault("_SelCateName", "")
        End Get
        Set(value As String)
            Me.ViewState("_SelCateName") = value
        End Set
    End Property

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Page.IsPostBack) Then

            'Me.LitSelect_t.Visible = Not Me.PNLddl.Visible

            Me.PreSelectedId = Me.SelectedId
            RPTSubCategories.DataSource = BindedCategories
            RPTSubCategories.DataBind()

            SetSelectedCategory()
        End If
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        LTSelect_t.Text = Me._DDLdisplayText
        LTreadOnly.Text = Me._DDLdisplayText
    End Sub

    Private Sub RPTSubCategories_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTSubCategories.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim item As TK.Domain.DTO.DTO_CategoryTree = e.Item.DataItem
            Dim CTRLitem As UC_CategoryDDLItem = e.Item.FindControl("CTRLitem")

            If Not IsNothing(item) AndAlso Not IsNothing(CTRLitem) Then
                Dim preselectName As String = CTRLitem.BindItem(item, Me.PreSelectedId)
                If Not String.IsNullOrEmpty(preselectName) Then
                    _SelectedName = preselectName
                End If
            End If

        End If

    End Sub

    Public ReadOnly Property SelectedId As Int64
        Get

            Dim Id As Int64 = 0
            Try
                Id = System.Convert.ToInt64(Me.HID_Value.Value.Remove(0, 4))
            Catch ex As Exception

            End Try

            Return Id
        End Get
    End Property

    Private _IsForFilters As Boolean = False

    ''' <summary>
    ''' Bind della DDL categorie
    ''' </summary>
    ''' <param name="Categories">Lista categorie da visualizzazione (tree)</param>
    ''' <param name="SelectedCatId">ID categoria selezionata</param>
    ''' <param name="VoidCategoryText">SE non ho selezioni il testo visualizzato (già internazionalizzato)</param>
    ''' <param name="SelectedCat">Eventuale categoria selezionata</param>
    ''' <remarks></remarks>
    Public Sub InitDDL(ByVal Categories As IList(Of TK.Domain.DTO.DTO_CategoryTree), _
                       ByVal SelectedCatId As Int64, _
                       ByVal VoidCategoryText As String)

        VoidCateText = VoidCategoryText

        If SelectedCatId > 0 Then
            PreSelectedId = SelectedCatId
            Me.HID_Value.Value = "ctg-" + SelectedCatId.ToString()
            '    'PreSelectedName = SelectedCat.Name
            '    'LBLSelectName.Text = SelectedCat.Name
            '    'LitSelect_t.Text = SelectedCat.Name
            'Else
            '    'LitSelect_t.Text = Resource.getValue("")
        End If

        If Not IsNothing(Categories) Then
            BindedCategories = Categories.OrderBy(Function(x) x.Order).ToList()
            RPTSubCategories.DataSource = BindedCategories 'Categories.OrderBy(Function(x) x.Order)
            RPTSubCategories.DataBind()
        End If



        'LitSelect_t
        If SelectedCatId > 0 AndAlso Not String.IsNullOrEmpty(Me._SelectedName) Then
            Me._DDLdisplayText = Me._SelectedName
        Else
            Me._DDLdisplayText = VoidCateText

        End If

        
    End Sub

    Private Property VoidCateText As String
        Get
            Return ViewStateOrDefault("VoidCateText", "- ALL -")
        End Get
        Set(value As String)
            ViewState("VoidCateText") = value
        End Set
    End Property
    'Private Property PreSelectedName As String
    '    Get
    '        Return Me.ViewStateOrDefault("PreSelName", "").ToString()
    '    End Get
    '    Set(value As String)
    '        Me.ViewState("PreSelName") = value
    '    End Set
    'End Property

    Private Property PreSelectedId As Int64
        Get
            Return System.Convert.ToInt64(Me.ViewStateOrDefault("PreSelId", 0))
        End Get
        Set(value As Int64)
            Me.ViewState("PreSelId") = value
        End Set
    End Property

    Private Property BindedCategories As IList(Of TK.Domain.DTO.DTO_CategoryTree)
        Get
            Return DirectCast(Me.ViewState("Categories"), IList(Of TK.Domain.DTO.DTO_CategoryTree))
        End Get
        Set(value As IList(Of TK.Domain.DTO.DTO_CategoryTree))
            Me.ViewState("Categories") = value
        End Set
    End Property



    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property


    Private Sub SetSelectedCategory()

        Dim CategoryName As String = VoidCateText

        If (Me.SelectedId > 0 AndAlso Me.BindedCategories.Any()) Then

            If (Me.SelectedId > 0) Then
                GetRecoursiveName(Me.BindedCategories, Me.SelectedId, CategoryName)
            End If
        End If

        'If Not String.IsNullOrEmpty(CategoryName) Then
        Me.LTSelect_t.Text = CategoryName
        LTreadOnly.Text = CategoryName
        'Else
        'Me.LTSelect_t.Text
        'LTreadOnly.Text = VoidCateText
        'End If

    End Sub

    Private Sub GetRecoursiveName(ByVal Categories As IList(Of TK.Domain.DTO.DTO_CategoryTree), ByVal ID As Int64, ByRef CurrentName As String)

        If Not String.IsNullOrEmpty(CurrentName) Then
            Exit Sub
        End If

        For Each ct As TK.Domain.DTO.DTO_CategoryTree In Categories
            If ct.Id = ID Then
                CurrentName = ct.Name
                Exit Sub
            End If

            If Not IsNothing(ct.Children) AndAlso ct.Children.Any() Then
                GetRecoursiveName(ct.Children, ID, CurrentName)
            End If
        Next

    End Sub

    Public Property IsReadOnly As Boolean
        Get
            Return Not Me.PNLddl.Visible
        End Get
        Set(value As Boolean)
            Me.PNLddl.Visible = Not value
            Me.LTreadOnly.Text = Me.LTSelect_t.Text
            Me.LTreadOnly.Visible = value
        End Set
    End Property

End Class