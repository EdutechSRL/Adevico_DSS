Public Class UC_SimplePager
    Inherits System.Web.UI.UserControl

    Public Event GoToPage(ByVal page As Integer)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="totalPage">Pagine totali</param>
    ''' <param name="currentPage">Pagina corrente ad INDICE 0</param>
    ''' <remarks></remarks>
    Public Sub InitUc(ByVal totalPage As Integer, ByVal currentPage As Integer)

        _currentPage = currentPage

        If (totalPage = 0) Then
            Me.RPTpages.Visible = False
        Else
            Dim values As IList(Of Integer) = New List(Of Integer)()

            For i As Integer = 1 To totalPage
                values.Add(i)
            Next

            Me.RPTpages.DataSource = values
            Me.RPTpages.DataBind()

        End If

    End Sub

    Private _currentPage As Integer = 0

    Private Sub RPTpages_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTpages.ItemCommand

        Dim page As Integer = 0
        Try
            page = System.Convert.ToInt32(e.CommandArgument) - 1
        Catch ex As Exception

        End Try

        If (page < 0) Then
            page = 0
        End If

        RaiseEvent GoToPage(page)

    End Sub
    
    Private Sub RPTpages_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTpages.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim val As Integer = e.Item.DataItem
            
            Dim LNBpage As LinkButton = e.Item.FindControl("LNBpage")
            If Not IsNothing(LNBpage) Then
                LNBpage.Text = val.ToString()
                LNBpage.CommandName = "gotoPage"
                LNBpage.CommandArgument = val

                If (val = _currentPage + 1) Then
                    LNBpage.Enabled = False
                    LNBpage.CssClass = "PagerSpan"
                End If

            End If
        End If
    End Sub


End Class