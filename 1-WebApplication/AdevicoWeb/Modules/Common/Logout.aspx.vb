Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class Logout
    Inherits PageBase
    Implements IViewUserLogout

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As UserLogoutPresenter
    Public ReadOnly Property CurrentPresenter() As UserLogoutPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UserLogoutPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "View"
    Public Function IsShibbolethSessionActive(key As String) As Boolean Implements IViewUserLogout.IsShibbolethSessionActive
        Try
            Return Not String.IsNullOrEmpty(Request.ServerVariables(key).ToString)
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public ReadOnly Property UserAccessInfo As lm.Comol.Core.DomainModel.Helpers.dtoLoginCookie Implements IViewUserLogout.UserAccessInfo
        Get
            Return PageUtility.ReadLoginProviderCookie()
        End Get
    End Property

#End Region

#Region "inherits"
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

  

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(SystemSettings.Login.AlwaysDefaultPageForInternal)
    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub


    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub GoToDefaultPage() Implements IViewUserLogout.GoToDefaultPage
        LogoutActions()
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage, SystemSettings.Login.isSSLloginRequired)
    End Sub
    Public Sub LoadExternalProviderPage(url As String) Implements IViewUserLogout.LoadExternalProviderPage
        LogoutActions()
        If String.IsNullOrEmpty(url) Then
        Else
            Response.Redirect(url)
        End If
    End Sub
    Public Sub LoadInternalLoginPage() Implements IViewUserLogout.LoadInternalLoginPage
        LogoutActions()
        Dim url As String = PageUtility.GetDefaultLogoutPage()
        If String.IsNullOrEmpty(url) Then
            Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False), SystemSettings.Login.isSSLloginRequired)
        ElseIf url.StartsWith(BaseUrl) Then
            Me.PageUtility.RedirectToUrl(url, SystemSettings.Login.isSSLloginRequired)
        ElseIf url.StartsWith("http") Then
            Response.Redirect(url)
        End If
    End Sub
    'Public Sub LoadOldAuthenticationPage(idAuthenticationType As Integer) Implements IViewUserLogout.LoadOldAuthenticationPage
    '    Dim RemoteUrl As String = ""
    '    Select Case idAuthenticationType
    '        Case Main.TipoAutenticazione.IOP
    '            RemoteUrl = (From o In Me.SystemSettings.UrlProviders Where o.ComolID = idAuthenticationType Select o.RemoteLogin).FirstOrDefault

    '    End Select
    '    If RemoteUrl = "" Then
    '        RemoteUrl = Me.PageUtility.DefaultUrl
    '    End If
    '    LogoutActions()
    '    Me.Response.Redirect(RemoteUrl, True)
    'End Sub
    Private Sub LoadLogoutMessage(mode As lm.Comol.Core.Authentication.LogoutMode, type As lm.Comol.Core.Authentication.AuthenticationProviderType, destinationUrl As String) Implements IViewUserLogout.LoadLogoutMessage
        LogoutActions()
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.LogoutMessage(mode, type, Server.UrlEncode(destinationUrl)))
    End Sub
    Public Sub LoadLanguage(language As lm.Comol.Core.DomainModel.Language) Implements IViewUserLogout.LoadLanguage
        Dim oLingua As New Lingua(language.Id, language.Code) With {.Icona = language.Icon, .isDefault = language.isDefault}

        Me.OverloadLanguage(oLingua)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub
#End Region

#Region "Internal"
    Private Sub LogOutFunction() 'Optional ByVal UserId As Integer = -1)

        Try
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")

            If Not IsNothing(oPersona) Then
                'Pulizia allegati mail
                Dim path As String = PageUtility.PhysicalApplicationPath
                Environment.CurrentDirectory = path
                lm.Comol.Core.File.Delete.Directory(path & "Mail\" & oPersona.ID & "\", True)
                'Chiusura Chat
                lm.Comol.Modules.Standard.Chat.IM_SharedFunction.DiscardAllChats(oPersona.ID)
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub LogoutActions()
        PageUtility.ClearLogonProviderCookie()
        Me.LogOutFunction()
        Me.PageUtility.LogoutAction()
        Session.Clear()
        Session.Abandon()
    End Sub
#End Region
   


End Class