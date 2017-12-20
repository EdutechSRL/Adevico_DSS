Imports lm.Comol.UI.Presentation

Partial Public Class UCpager
    Inherits System.Web.UI.UserControl
    Implements iPager


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property ButtonCount() As Integer Implements lm.Comol.UI.Presentation.iPager.ButtonCount
        Get
            Return Me.ViewState("ButtonCount")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ButtonCount") = value
        End Set
    End Property
    Public Property ItemsForPage() As Integer Implements lm.Comol.UI.Presentation.iPager.ItemsForPage
        Get
            Return Me.ViewState("ItemsForPage")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ItemsForPage") = value
        End Set
    End Property
    Public Property PageCount() As Integer Implements lm.Comol.UI.Presentation.iPager.PageCount
        Get
            Return Me.ViewState("PageCount")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("PageCount") = value
        End Set
    End Property
    Public Property PageSize() As Integer Implements lm.Comol.UI.Presentation.iPager.PageSize
        Get
            Return Me.ViewState("PageSize")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("PageSize") = value
        End Set
    End Property
    Public Property StartIndex() As Integer Implements lm.Comol.UI.Presentation.iPager.StartIndex
        Get
            Return Me.ViewState("StartIndex")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("StartIndex") = value
        End Set
    End Property
    Public Property CurrentPage() As Integer Implements lm.Comol.UI.Presentation.iPager.CurrentPage
        Get
            Return Me.ViewState("CurrentPage")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentPage") = value
        End Set
    End Property

    Private Sub RPTpager_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTpager.ItemCommand
        Dim NewPage As Integer = e.CommandArgument

    End Sub

    Private Sub GotoPage(ByVal Index As Integer)

    End Sub

    Private Function CreatePageLink(ByVal Index As Integer) As IList(Of Pager)
        Dim oLista As New List(Of Pager)

        If Index = 1 AndAlso Me.PageCount <= Me.ButtonCount Then
            For PageIndex As Integer = 1 To Me.PageCount
                oLista.Add(New Pager(PageIndex, PageIndex.ToString))
            Next
        ElseIf Index = 1 Then
            For PageIndex As Integer = 1 To Me.ButtonCount - 1
                oLista.Add(New Pager(PageIndex, PageIndex.ToString))
            Next
            oLista.Add(New Pager(Me.ButtonCount, "..."))
        ElseIf Index = Me.PageCount Then
            For PageIndex As Integer = (1 + Me.PageCount - Me.ButtonCount) To Me.PageCount
                oLista.Add(New Pager(PageIndex, PageIndex.ToString))
            Next
            oLista(0).PageString = "..."
        Else

        End If

        Return oLista
    End Function

    Private Class Pager
        Private _PageNumber As Integer
        Private _PageString As String
        Public Property PageNumber() As Integer
            Get
                Return _PageNumber
            End Get
            Set(ByVal value As Integer)
                _PageNumber = value
            End Set
        End Property
        Public Property PageString() As String
            Get
                Return _PageString
            End Get
            Set(ByVal value As String)
                _PageString = value
            End Set
        End Property
        Sub New()

        End Sub
        Sub New(ByVal Number As Integer)
            Me._PageNumber = Number
        End Sub
        Sub New(ByVal Number As Integer, ByVal PageString As String)
            Me._PageNumber = Number
            Me._PageString = PageString
        End Sub
    End Class
End Class