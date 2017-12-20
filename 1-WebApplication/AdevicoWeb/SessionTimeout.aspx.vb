Public Partial Class SessionTimeout
	Inherits Base


	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Public Overrides Sub BindDati()

	End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_Login", "Root")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		Me.Resource.setLabel(Me.LBsessioneScaduta)
		Me.Resource.setButton(Me.BTNaccesso, True)
	End Sub

	Private Sub BTNaccesso_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaccesso.Click
		Me.RedirectToLoginUrl("index.aspx")
	End Sub
End Class