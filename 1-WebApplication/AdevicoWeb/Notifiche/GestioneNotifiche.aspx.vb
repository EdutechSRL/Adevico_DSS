Public Partial Class GestioneNotifiche
	Inherits PageBase

	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return True
		End Get
	End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	End Sub

	Public Overrides Sub BindDati()
		Dim oServizio As New UCServices.Services_Mail
		Me.CTRLsorgenteComunita.ServizioCode = UCServices.Services_Mail.Codex
		Me.CTRLsorgenteComunita.SetupControl()
		Me.CTRLsorgenteComunita.Visible = True
	End Sub

	Public Overrides Sub BindNoPermessi()

	End Sub

	Public Overrides Function HasPermessi() As Boolean

	End Function

	Public Overrides Sub RegistraAccessoPagina()

	End Sub

	Public Overrides Sub SetCultureSettings()

	End Sub

	Public Overrides Sub SetInternazionalizzazione()

	End Sub

	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

	End Sub

	Private Sub CTRLsorgenteComunita_AggiornaDati(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLsorgenteComunita.AggiornaDati
		If CTRLsorgenteComunita.ComunitaID <> -1 Then
			LBLcomunita.Text = CTRLsorgenteComunita.ComunitaID
		Else
			LBLcomunita.Text = ""
		End If
	End Sub
	
End Class