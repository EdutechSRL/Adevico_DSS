Public Class UC_TextArea
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearTools()

        'If Not Page.IsPostBack Then
        '    LBLwarningLenght.Visible = False
        'End If
    End Sub

    Public Property Text As String
        Get
            'Return Server.HtmlEncode(Me.RDEtelerik.Text)
            'If (MaxLenght > 0) Then
            '    Dim insertedText As String = Me.RDEtelerik.Text

            '    If (insertedText.Length > MaxLenght) Then
            '        HasLenghError = True
            '        Return Me.RDEtelerik.Text.Substring(0, MaxLenght)
            '    Else
            '        HasLenghError = False
            '        Return Me.RDEtelerik.Text
            '    End If
            'Else
            '    HasLenghError = False
            '    Return Me.RDEtelerik.Text
            'End If


            Return Me.RDEtelerik.Text
        End Get
        Set(value As String)
            Me.RDEtelerik.Content = value.Replace(ControlChars.Lf, "<br/>" & ControlChars.Lf)
            'Me.RDEtelerik.Content = Server.HtmlEncode(Me.RDEtelerik.Text).Replace(ControlChars.Lf, "<br/>" & ControlChars.Lf)
        End Set
    End Property


    Public Property Enabled As Boolean
        Get
            Return Me.RDEtelerik.Enabled
        End Get
        Set(value As Boolean)
            Me.RDEtelerik.Enabled = value
        End Set
    End Property

    Public Property MaxLenght As Integer
        Get
            Return ViewStateOrDefault("MaxEditorLenght", 0)

        End Get
        Set(value As Integer)
            ViewState("MaxEditorLenght") = value
        End Set
    End Property

    Public Sub ClearTools()
        For Each group As Telerik.Web.UI.EditorToolGroup In Me.RDEtelerik.Tools

            For i As Integer = 0 To group.Tools.Count() - 1
                group.Tools.RemoveAt(i)
            Next
        Next
    End Sub

    'Public Property HasLenghError() As Boolean
    '    Get
    '        Return ViewStateOrDefault("HasLenghError", False)
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("HasLenghError") = value
    '    End Set
    'End Property

    Public ReadOnly Property HasLenghError() As Boolean
        Get
            If (MaxLenght <= 0) Then
                Return False
            End If

            Return Me.RDEtelerik.Text.Length > MaxLenght
        End Get

    End Property

    '


    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
End Class