Public Partial Class Scheda_Informativa
	Inherits PageBase

    Private _OldPageUtility As OLDpageUtility

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property


    Private ReadOnly Property Utility() As OLDpageUtility
        Get
            If IsNothing(_OldPageUtility) Then
                _OldPageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _OldPageUtility
        End Get
    End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property

	Public Overrides Sub BindDati()
		If Request.QueryString("CMNT_ID") Is Nothing Then
			Me.CTRLDettagli.SetupDettagliComunita(Me.ComunitaLavoroID)
		Else
			Me.CTRLDettagli.SetupDettagliComunita(Request.QueryString("CMNT_ID"))
			Me.BTNindietro.Visible = True
		End If
	End Sub

	Public Overrides Sub BindNoPermessi()

	End Sub

	Public Overrides Function HasPermessi() As Boolean
		Return True
	End Function

	Public Overrides Sub RegistraAccessoPagina()

	End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_DettagliComunita", "UC")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With MyBase.Resource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
			.setButton(Me.BTNindietro, True)
		End With
	End Sub

	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

	End Sub

	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return True
		End Get
	End Property

	Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click
        Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
	End Sub
End Class