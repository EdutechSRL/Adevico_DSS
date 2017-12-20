Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class InvalidToken
    Inherits PageBase
    Implements IViewInvalidToken


#Region "Context"
    Private _Presenter As InvalidTokenPresenter

    Private ReadOnly Property CurrentPresenter() As InvalidTokenPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InvalidTokenPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadedIdPerson As Integer Implements IViewInvalidToken.PreloadedIdPerson
        Get
            If IsNumeric(Request.QueryString("idPerson")) Then
                Return CInt(Request.QueryString("idPerson"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdProvider As Integer Implements IViewInvalidToken.PreloadedIdProvider
        Get
            If IsNumeric(Request.QueryString("IdProvider")) Then
                Return CInt(Request.QueryString("IdProvider"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedToken As lm.Comol.Core.Authentication.UrlProviderResult Implements IViewInvalidToken.PreloadedToken
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.UrlProviderResult).GetByString(Request.QueryString("token"), lm.Comol.Core.Authentication.UrlProviderResult.ValidToken)
        End Get
    End Property
#End Region

    Private Sub InvalidToken_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Me.Master.ShowHelp = False
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowLanguage = False
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView(PreloadedIdPerson, PreloadedIdProvider, PreloadedToken)
        End If
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ISAuthenticationPage", "Authentication")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTtitleInvalidToken)
            .setLiteral(LTexternalWebLogon)
            .setLiteral(LTtokenSupportInfo)
            
        End With
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub GoToDefaultPage() Implements IViewInvalidToken.GoToDefaultPage
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage)
    End Sub
    Private Sub LoadLanguage(language As lm.Comol.Core.DomainModel.Language) Implements IViewInvalidToken.LoadLanguage
        Dim oLingua As New Lingua(language.Id, language.Code) With {.Icona = language.Icon, .isDefault = language.isDefault}

        Me.OverloadLanguage(oLingua)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
    Private Sub DisplayMessage(message As lm.Comol.Core.Authentication.UrlProviderResult) Implements IViewInvalidToken.DisplayMessage
        LTinvalidToken.Text = Resource.getValue("UrlProviderResult." & message.ToString())
    End Sub

    Private Sub DisplayMessage(username As String, message As lm.Comol.Core.Authentication.UrlProviderResult) Implements IViewInvalidToken.DisplayMessage
        LTinvalidToken.Text = String.Format(Resource.getValue("username.UrlProviderResult." & message.ToString()), username)
    End Sub

    Private Sub SetAutoLogonUrl(url As String) Implements IViewInvalidToken.SetAutoLogonUrl
        Me.LTexternalWebLogon.Visible = Not String.IsNullOrEmpty(url)
        If (LTexternalWebLogon.Text.Contains("{0}")) Then
            LTexternalWebLogon.Text = String.Format(LTexternalWebLogon.Text, url)
        End If
        If Not String.IsNullOrEmpty(url) Then
            Me.LTredirect.Text = String.Format("<script language=""javascript"" type=""text/javascript"">window.setTimeout('window.location=""{0}""; ', 5000);</script>", url)

            Me.Resource.setLiteral(Me.LTtokenUrl)
            If LTtokenUrl.Text.Contains("{0}") Then
                Me.LTtokenUrl.Text = String.Format(LTtokenUrl.Text, url)
            End If
        End If
    End Sub
#End Region
   
End Class