Public Class UC_OrderBy
    Inherits System.Web.UI.UserControl

    Private _cssclassUp As String = "ui-icon ui-icon-triangle-1-n"
    Public Property CssClassUp As String
        Get
            Return _cssclassUp
        End Get
        Set(value As String)
            _cssclassUp = value
        End Set
    End Property

    Private _cssclassDown As String = "ui-icon ui-icon-triangle-1-s"
    Public Property CssClassDown As String
        Get
            Return _cssclassDown
        End Get
        Set(value As String)
            _cssclassDown = value
        End Set
    End Property

    Private _cssclassUpDown As String = "ui-icon ui-icon-triangle-2-n-s"
    Public Property CssClassUpDown As String
        Get
            Return _cssclassUpDown
        End Get
        Set(value As String)
            _cssclassUpDown = value
        End Set
    End Property

    Private _textUp As String = "&nbsp;" '"&uarr;"
    Public Property TextUp As String
        Get
            Return _textUp
        End Get
        Set(value As String)
            _textUp = value
        End Set
    End Property

    Private _textDown As String = "&nbsp;" '"&darr;"
    Public Property TextDown As String
        Get
            Return _textDown
        End Get
        Set(value As String)
            _textDown = value
        End Set
    End Property

    Private _textUpDown As String = "&nbsp;" '"&uarr;&darr;"
    Public Property TextUpDown As String
        Get
            Return _textUpDown
        End Get
        Set(value As String)
            _textUpDown = value
        End Set
    End Property

    Private _column As String = Me.ID
    Public Property Column As String
        Get
            Return _column
        End Get
        Set(value As String)
            _column = value
        End Set
    End Property

    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function

    Private _status As OrderByStatus = OrderByStatus.None
    Public Property Status As OrderByStatus
        Get
            _status = ViewStateOrDefault("status", _status)
            Return _status
        End Get
        Set(value As OrderByStatus)
            _status = value
            ViewState("status") = _status
            Select Case _status
                Case OrderByStatus.None
                    LNBup.Visible = False
                    LNBdown.Visible = False
                    LNBupDown.Visible = True
                Case OrderByStatus.Ascending
                    LNBup.Visible = True
                    LNBdown.Visible = False
                    LNBupDown.Visible = False
                Case OrderByStatus.Descending
                    LNBup.Visible = False
                    LNBdown.Visible = True
                    LNBupDown.Visible = False
                Case OrderByStatus.Hidden
                    LNBup.Visible = False
                    LNBdown.Visible = False
                    LNBupDown.Visible = False
            End Select
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LNBup.CssClass = CssClassUp
        LNBdown.CssClass = CssClassDown
        LNBupDown.CssClass = CssClassUpDown
    End Sub

    Public Event OnOrderBy(e As OrderByEventArgs)
    Private Sub LNBupDown_Click(sender As Object, e As System.EventArgs) Handles LNBupDown.Click
        BTNClick(sender)
    End Sub

    Private Sub LNBdown_Click(sender As Object, e As System.EventArgs) Handles LNBdown.Click
        BTNClick(sender)
    End Sub

    Private Sub LNBup_Click(sender As Object, e As System.EventArgs) Handles LNBup.Click
        BTNClick(sender)
    End Sub

    Private Sub BTNClick(ByVal sender As Object)
        Select Case Status
            Case OrderByStatus.None
                Status = OrderByStatus.Ascending
            Case OrderByStatus.Ascending
                Status = OrderByStatus.Descending
            Case OrderByStatus.Descending
                Status = OrderByStatus.Ascending
            Case Else
                Status = OrderByStatus.Ascending
        End Select

        RaiseEvent OnOrderBy(New OrderByEventArgs() With {.Status = Status, .Column = Column, .BtnSender = sender})
    End Sub

    
End Class

Public Enum OrderByStatus
    Ascending
    Descending
    None
    Hidden
End Enum

Public Class OrderByEventArgs
    Inherits EventArgs

    Private _btnsender As Object
    Public Property BtnSender As Object
        Get
            Return _btnsender
        End Get
        Set(value As Object)
            _btnsender = value
        End Set
    End Property

    Private _column As String = ""
    Public Property Column As String
        Get
            Return _column
        End Get
        Set(value As String)
            _column = value
        End Set
    End Property

    Private _status As OrderByStatus = OrderByStatus.None
    Public Property Status As OrderByStatus
        Get
            Return _status
        End Get
        Set(value As OrderByStatus)
            _status = value
        End Set
    End Property

    Public ReadOnly Property Ascending As Boolean
        Get
            Return Status = OrderByStatus.Ascending
        End Get
    End Property

    Public ReadOnly Property Descending As Boolean
        Get
            Return Status = OrderByStatus.Descending
        End Get
    End Property

End Class
