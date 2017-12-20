Partial Public Class IndiceSecondario
	Inherits Base



	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Me.RedirectToLoginUrl("index.aspx")
	End Sub

	Public Overrides Sub BindDati()

	End Sub

	Public Overrides Sub SetCultureSettings()

	End Sub

	Public Overrides Sub SetInternazionalizzazione()

	End Sub
End Class