Imports COL_BusinessLogic_v2
Imports System.Globalization

Partial Public Class Index : Inherits PageBase
	Implements IviewPortale
	Private _Presenter As PresenterPortale

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

	Public Property CurrentLanguageID() As Integer Implements IviewPortale.CurrentLanguageID
		Get
			Return Me.LinguaID
		End Get
		Set(ByVal value As Integer)
            'Me.CambiaImpostazioniLingua(value)
            'Me.SetCultureSettings()
            'Me.SetInternazionalizzazione()
            'Me.CTRLlogin.ReloadCultureSettings() 'SetupInternazionalizzazione(Me.LinguaCode)
            'Me.NewLinguaID = Me.LinguaID
		End Set
	End Property
	Public ReadOnly Property CurrentPresenter() As PresenterPortale Implements IviewPortale.CurrentPresenter
		Get
			If IsNothing(_Presenter) Then
				_Presenter = New PresenterPortale(Me)
			End If
			Return _Presenter
		End Get
	End Property
	Public ReadOnly Property MailConfig() As MailLocalized Implements IviewPortale.MailConfig
		Get
			Return Me.LocalizedMail
		End Get
	End Property
	Public Overloads ReadOnly Property IstituzioneID() As Integer Implements IviewPortale.IstituzioneID
		Get
			Return MyBase.IstituzioneID
		End Get
	End Property

	Public ReadOnly Property DefaultSetting() As PresenterSettings Implements IviewPortale.DefaultSetting
		Get
			Return MyBase.SystemSettings.Presenter
		End Get
	End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.Page.IsPostBack Then
            Dim useSSL As Boolean = SystemSettings.Login.isSSLrequired
            If Not useSSL AndAlso SystemSettings.Login.isSSLloginRequired Then
                useSSL = SystemSettings.Presenter.DefaultStartPage.Contains(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False)) OrElse (Not String.IsNullOrEmpty(SystemSettings.Presenter.FullDefaultStartPage) AndAlso SystemSettings.Presenter.FullDefaultStartPage.StartsWith("https"))
            End If
            If useSSL Then
                Me.PageUtility.RedirectToSecureUrl(SystemSettings.Presenter.DefaultStartPage & Me.Request.Url.Query)
            Else
                Me.PageUtility.RedirectToDefault(Me.Request.Url.Query)
            End If
        End If
        'If Me.Page.IsPostBack = False Then
        '	Dim CurrentUrl As String = ""
        '	If Me.Request.Url.Query = "" Then
        '		CurrentUrl = Me.Request.Url.AbsoluteUri
        '	Else
        '		CurrentUrl = Me.Request.Url.AbsoluteUri.Substring(0, Me.Request.Url.AbsoluteUri.IndexOf("?"))
        '	End If
        '	If CurrentUrl.ToLower <> Me.DefaultUrl.ToLower Then
        '		Me.RedirectToDefault(Me.Request.Url.Query)
        '	End If
        'End If
    End Sub

	Public Overrides Sub BindDati()
		Session("OrgnIDtoSubscribe") = ""
		Session("TipoPersonaIDtoSubscribe") = ""
		Session("IstituzioneIDtoSubscribe") = ""
		Session("limbo") = True
		Session("objPersona") = ""
		Session("ORGN_id") = ""
		Session("Istituzione") = ""

		Session("IdRuolo") = ""
		Session("IdComunita") = 0
		Session("ArrPermessi") = ""
		Session("ArrComunita") = ""
		Session("RLPC_ID") = 0

		Session("CMNT_path_forAdmin") = ""
		Me.AmministrazioneComunitaID = -1
		Me.isModalitaAmministrazione = False
        Me.CurrentPresenter.Init()
	End Sub

	Public Overrides Function HasPermessi() As Boolean
		Return True
	End Function

	Public Overrides Sub SetCultureSettings()

	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With Me.Resource

		End With
	End Sub


#Region "Inusati"
	Public Overrides Sub RegistraAccessoPagina()

	End Sub
	Public Overrides Sub BindNoPermessi()

	End Sub
	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

	End Sub
	Public Sub ChangeLanguageSettings() Implements IviewPortale.ChangeLanguageSettings

	End Sub
#End Region

End Class