Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class ProfileDisabled
    Inherits PageBase
    Implements IViewProfileDisabled

#Region "Context"
    Private _Presenter As ProfileDisabledPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As ProfileDisabledPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfileDisabledPresenter(Me.CurrentContext, Me)
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
    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewProfileDisabled.isSystemOutOfOrder
        Get
            Return Not AccessoSistema
        End Get
    End Property
    Public Property IdProfile As Integer Implements IViewProfileDisabled.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProvider As Long Implements IViewProfileDisabled.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProvider") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedIdProfile As Integer Implements IViewProfileDisabled.PreloadedIdProfile
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return CInt(0)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedIdProvider As Long Implements IViewProfileDisabled.PreloadedIdProvider
        Get
            If IsNumeric(Request.QueryString("IdProvider")) Then
                Return CLng(Request.QueryString("IdProvider"))
            Else
                Return CLng(0)
            End If
        End Get
    End Property
    Public WriteOnly Property AllowInternalAuthentication As Boolean Implements IViewProfileDisabled.AllowInternalAuthentication
        Set(value As Boolean)
            Me.LTaccess.Visible = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
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
            .setLiteral(LTtitleProfileDisabled)
            .setLiteral(LTaccess)

            Dim url As String = BaseUrl
            If SystemSettings.Login.isSSLloginRequired AndAlso Me.Request.Url.AbsoluteUri.StartsWith("http://") Then
                url = PageUtility.SecureApplicationUrlBase
            End If
            If LTaccess.Text.Contains("{0}") Then
                LTaccess.Text = String.Format(LTaccess.Text, url & RootObject.InternalLogin(True))
            End If
            .setLiteral(LTsupportInfo)
            If LTsupportInfo.Text.Contains("{0}") Then
                Dim oMailLocalized As MailLocalized = Me.LocalizedMail
                LTsupportInfo.Text = String.Format(LTsupportInfo.Text, oMailLocalized.SystemSender.Address, oMailLocalized.SystemSender.Address)
            End If
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
    Public Sub GotoAuthenticationSelctorPage() Implements IViewProfileDisabled.GotoAuthenticationSelctorPage
        Me.PageUtility.RedirectToUrl(SystemSettings.Presenter.DefaultLogonPage)
    End Sub
    Public Sub GotoInternalLogin() Implements IViewProfileDisabled.GotoInternalLogin
        Me.PageUtility.RedirectToUrl(RootObject.InternalLogin(False), SystemSettings.Login.isSSLloginRequired)
    End Sub
    Public Sub AllowExternalWebAuthentication(url As String) Implements IViewProfileDisabled.AllowExternalWebAuthentication
        Resource.setLiteral(LTexternalWebLogon)
        If (LTexternalWebLogon.Text.Contains("{0}")) Then
            LTexternalWebLogon.Text = String.Format(LTexternalWebLogon.Text, url)
            LTexternalWebLogon.Visible = True
        Else
            LTexternalWebLogon.Visible = False
        End If
    End Sub
#End Region

   

    Public Sub DisplayDisabledAccount(name As String) Implements IViewProfileDisabled.DisplayDisabledAccount
        Me.LTprofileDisabled.Text = String.Format(Me.Resource.getValue("DisplayDisabledAccount"), name)
    End Sub

    Public Sub DisplayNotActivatedAccount(name As String) Implements IViewProfileDisabled.DisplayNotActivatedAccount
        Me.LTprofileDisabled.Text = String.Format(Me.Resource.getValue("DisplayNotActivatedAccount"), name)
    End Sub

    Public Sub GotoRemoteUrl(url As String) Implements IViewProfileDisabled.GotoRemoteUrl
        Response.Redirect(url)
    End Sub

   
End Class