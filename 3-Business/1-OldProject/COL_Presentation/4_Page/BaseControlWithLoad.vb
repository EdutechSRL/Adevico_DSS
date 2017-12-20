Public MustInherit Class BaseControlWithLoad
    Inherits BaseControlSession

	Public MustOverride ReadOnly Property AlwaysBind() As Boolean
    'Public MustOverride ReadOnly Property VerifyAuthentication() As Boolean

	Protected Overridable Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not VerifyAuthentication AndAlso Not IsSessioneScaduta() Then
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
        End If
        If AlwaysBind Then
            Me.BindDati()
        ElseIf Page.IsPostBack = False Then
            Me.BindDati()
        End If
        'End If
	End Sub
End Class