Public Class UC_DialogComment
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeControl(title As String, submitterName As String, evaluatorName As String, comment As String)
        InitializeControl("", title, submitterName, evaluatorName, comment, "")
    End Sub
    Public Sub InitializeControl(title As String, submitterName As String, evaluatorName As String, comment As String, criterion As String)
        InitializeControl("", title, submitterName, evaluatorName, comment, criterion)
    End Sub
    Public Sub InitializeControl(cssClass As String, title As String, submitterName As String, evaluatorName As String, comment As String, criterion As String)
        LBsubmitter.Text = submitterName
        LBevaluator.Text = evaluatorName

        If Not String.IsNullOrEmpty(cssClass) Then
            DVcomment.Attributes.Add("class", DVcomment.Attributes("class") & " " & cssClass)
        End If
        If Not String.IsNullOrEmpty(title) Then
            DVcomment.Attributes.Add("title", title)
        End If
        If Not String.IsNullOrEmpty(criterion) Then
            LBcriterion.Text = criterion
        End If
        LTcommment.Text = comment
    End Sub
End Class