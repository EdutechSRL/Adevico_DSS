Public Class UCtoolTip
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UCtoolTip", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LTdisplayAction)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(ByVal action As String, ByVal innerText As String)
        InitializeControl(action, innerText, False, "")
    End Sub
    Public Sub InitializeControl(ByVal action As String, ByVal innerText As String, ByVal showIcon As Boolean)
        InitializeControl(action, innerText, showIcon, "")
    End Sub
    Public Sub InitializeControl(ByVal action As String, ByVal innerText As String, ByVal showIcon As Boolean, ByVal cssClass As String)
        displayAction(action, showIcon, cssClass)
        LTitem.Text = innerText
    End Sub
    Public Sub InitializeControl(ByVal action As String, ByVal innerText As List(Of String))
        InitializeControl(action, innerText, False, "")
    End Sub
    Public Sub InitializeControl(ByVal action As String, ByVal innerText As List(Of String), ByVal showIcon As Boolean)
        InitializeControl(action, innerText, showIcon, "")
    End Sub
    Public Sub InitializeControl(ByVal action As String, ByVal innerText As List(Of String), ByVal showIcon As Boolean, ByVal cssClass As String)
        DisplayAction(action, showIcon, cssClass)
        LTitem.Text = ""
        If innerText.Count = 1 Then
            LTitem.Text = innerText(0)
        ElseIf innerText.Count > 1 Then
            LTitem.Text = "<ul>"
            For Each value As String In innerText
                LTitem.Text &= "<li>" & value & "</li>"
            Next
            LTitem.Text &= "</ul>"
        End If
    End Sub
    Private Sub DisplayAction(ByVal displayAction As String, ByVal showIcon As Boolean, ByVal cssClass As String)
        Dim display As String = IIf(showIcon, "<span class=""icon{0}"">{1}</span>", "{0}")

        LTdisplayAction.Text = IIf(showIcon, String.Format(display, IIf(String.IsNullOrEmpty(cssClass), "", " " & cssClass), displayAction), displayAction)
    End Sub
End Class