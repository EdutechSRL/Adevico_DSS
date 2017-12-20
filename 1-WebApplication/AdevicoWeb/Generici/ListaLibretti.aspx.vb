Partial Public Class ListaLibretti
	Inherits PageBase



#Region "Proprietà"
	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return False
		End Get
	End Property
#End Region


	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub



	Public Overrides Sub BindDati()

	End Sub

	Public Overrides Sub BindNoPermessi()
		Me.MLVlibretti.SetActiveView(Me.VIWnoLibretti)
	End Sub

	Public Overrides Function HasPermessi() As Boolean
		Return True
	End Function

	Public Overrides Sub RegistraAccessoPagina()

	End Sub

	Public Overrides Sub SetCultureSettings()

	End Sub

	Public Overrides Sub SetInternazionalizzazione()

	End Sub

	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

	End Sub


End Class