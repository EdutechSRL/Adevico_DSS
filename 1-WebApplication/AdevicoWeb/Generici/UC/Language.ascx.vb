Public Partial Class Language
    Inherits System.Web.UI.UserControl

    Public Event LinguaChanged()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim NewLinguaId As Integer = 0
		try
			If Not IsNothing(Session("NewLanguageId")) Then
				NewLinguaId = Session("NewLanguageId")
				Session("NewLanguageId") = Nothing
			Else
				Exit Sub
			End If
		Catch	ex As Exception

		End Try
       
        If Not NewLinguaId = 0 Then
            Me.ChangeLinguaDefault(NewLinguaId)
        End If
    End Sub
#Region "Gestione Pulsanti"
    Protected Sub Imb_Ita_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imb_Ita.Click
        Me.ChangeLinguaDefault(1)
    End Sub
    Protected Sub Imb_Eng_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imb_Eng.Click
        Me.ChangeLinguaDefault(2)
    End Sub
#End Region

#Region "Metodi privati per cambio lingua"

    Private Sub ChangeLinguaDefault(ByVal LinguaId As Integer)
        Dim oPersona As COL_Persona
        Try
            oPersona = Session("objPersona")

			if not isnothing(oPersona)
				Dim oLingua As New Lingua
				oLingua = ManagerLingua.GetByID(LinguaId)

				oPersona.SalvaImpostazioneLingua(LinguaId)
				oPersona.EstraiTutto(LinguaId)
				oPersona.Lingua = oLingua

				Me.SetupLingua(LinguaId)
			End If
		Catch ex As Exception
			Exit Sub
		End Try
		Response.Redirect(Request.Url.ToString)
	End Sub

	Private Sub SetupLingua(ByVal IdLingua As Integer)
		Dim oLingua As New Lingua
		oLingua = ManagerLingua.GetByID(IdLingua)
		Session("LinguaID") = oLingua.Id
		Session("LinguaCode") = oLingua.Codice
		Me.SetCookies(Session("LinguaID"), Session("LinguaCode"))
		'SetCulture(Session("LinguaCode"))
		'Me.SetupInternazionalizzazione()
		'Me.CTRLlogin.SetupInternazionalizzazione(Session("LinguaCode"))
		RaiseEvent LinguaChanged()
	End Sub

    Private Sub SetCookies(ByVal LinguaID As Integer, ByVal LinguaCode As String)
        Dim oBrowser As System.Web.HttpBrowserCapabilities
        oBrowser = Request.Browser

        If oBrowser.Cookies Then
            Dim oCookie_ID As New System.Web.HttpCookie("LinguaID", LinguaID.ToString)
            Dim oCookie_Code As New System.Web.HttpCookie("LinguaCode", LinguaCode)

            oCookie_ID.Expires = Now.AddYears(1)
            oCookie_Code.Expires = Now.AddYears(1)

            Me.Response.Cookies.Add(oCookie_ID)
            Me.Response.Cookies.Add(oCookie_Code)
        End If
    End Sub

#End Region

    'Private Sub SetTitle()

    '    Me.Imb_Eng.Attributes.Add("onmouseout", "window.status='';return true;")
    '    Me.Imb_Eng.Attributes.Add("onfocus", "window.status='Set language as English';return true;")
    '    Me.Imb_Eng.Attributes.Add("onmouseover", "window.status='Set language as English';return true;")
    '    Me.Imb_Ita.Attributes.Add("onmouseout", "window.status='';return true;")
    '    Me.Imb_Ita.Attributes.Add("onfocus", "window.status='Imposta Italiano come lingua';return true;")
    '    Me.Imb_Ita.Attributes.Add("onmouseover", "window.status='Imposta Italiano come lingua';return true;")
    'End Sub

    Public WriteOnly Property ShowMe() As Boolean
        'Get
        '    Return Me.Imb_Ita.Visible
        'End Get
        Set(ByVal value As Boolean)
            Me.Imb_Ita.Visible = value
            Me.Imb_Ita.Enabled = value

            Me.Imb_Eng.Visible = value
            Me.Imb_Eng.Enabled = value
        End Set
    End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.Imb_Ita.Enabled = True
        Me.Imb_Eng.Enabled = True

        If Session("LinguaCode") = "it-IT" Then

            Me.Imb_Eng.Attributes.Add("onmouseout", "window.status='';return true;")
            Me.Imb_Eng.Attributes.Add("onfocus", "window.status='Set language as English';return true;")
            Me.Imb_Eng.Attributes.Add("onmouseover", "window.status='Set language as English';return true;")

            Me.Imb_Ita.Attributes.Add("onmouseout", "window.status='';return true;")
            Me.Imb_Ita.Attributes.Add("onfocus", "window.status='Lingua attuale: Italiano';return true;")
            Me.Imb_Ita.Attributes.Add("onmouseover", "window.status='Lingua attuale: Italiano';return true;")

            Me.Imb_Ita.Enabled = False

        ElseIf Session("LinguaCode") = "en-US" Then

            Me.Imb_Eng.Attributes.Add("onmouseout", "window.status='';return true;")
            Me.Imb_Eng.Attributes.Add("onfocus", "window.status='Current language: English';return true;")
            Me.Imb_Eng.Attributes.Add("onmouseover", "window.status='Current language: English';return true;")

            Me.Imb_Ita.Attributes.Add("onmouseout", "window.status='';return true;")
            Me.Imb_Ita.Attributes.Add("onfocus", "window.status='Imposta Italiano come lingua';return true;")
            Me.Imb_Ita.Attributes.Add("onmouseover", "window.status='Imposta Italiano come lingua';return true;")

            Me.Imb_Eng.Enabled = False
        End If
    End Sub
End Class