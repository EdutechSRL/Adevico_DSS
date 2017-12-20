Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class SystemOutOfOrder
    Inherits PageBase
    Implements IViewSystemOutOfOrder


#Region "Context"
    Private _Presenter As SystemOutOfOrderPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As SystemOutOfOrderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SystemOutOfOrderPresenter(Me.CurrentContext, Me)
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

    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewSystemOutOfOrder.isSystemOutOfOrder
        Get
            Return Not AccessoSistema
        End Get
    End Property
#End Region

    'Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
    '    If (ViewState(Key) Is Nothing) Then
    '        ViewState(Key) = DefaultValue
    '        Return DefaultValue
    '    Else
    '        Return ViewState(Key)
    '    End If
    'End Function

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
            .setLiteral(LTtitleSystemOutOforder)
            .setLiteral(LToutOfOrderInfo)
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
    Public Sub GotoAuthenticationSelctorPage() Implements IViewSystemOutOfOrder.GotoAuthenticationSelctorPage
        Me.PageUtility.RedirectToUrl(RootObject.InternalShibbolethAuthenticationPage(False))
    End Sub
    Public Sub GotoInternalLogin() Implements IViewSystemOutOfOrder.GotoInternalLogin
        Me.PageUtility.RedirectToUrl(RootObject.InternalLogin(False), SystemSettings.Login.isSSLloginRequired)
    End Sub
    Public Sub AllowExternalWebAuthentication(url As String) Implements IViewSystemOutOfOrder.AllowExternalWebAuthentication

    End Sub
#End Region

   
End Class