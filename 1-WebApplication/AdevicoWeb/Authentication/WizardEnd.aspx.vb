Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Public Class WizardUserProfileEnd
    Inherits PageBase
    Implements IViewUserProfileWizardEndPage

#Region "Context"
    Private _Presenter As ProfileWizardEndPagelPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As ProfileWizardEndPagelPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfileWizardEndPagelPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public WriteOnly Property AllowBackToStartPage As Boolean Implements IViewUserProfileWizardEndPage.AllowBackToStartPage
        Set(value As Boolean)
            LTstartPage.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowExternalWebAuthentication As Boolean Implements IViewUserProfileWizardEndPage.AllowExternalWebAuthentication
        Set(value As Boolean)
            LTexternalWebLogon.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowInternalAuthentication As Boolean Implements IViewUserProfileWizardEndPage.AllowInternalAuthentication
        Set(value As Boolean)
            LTbackToLoginPage.Visible = value
        End Set
    End Property
 
    Public ReadOnly Property PreloadedMessage As ProfileSubscriptionMessage Implements IViewUserProfileWizardEndPage.PreloadedMessage
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileSubscriptionMessage).GetByString(Request.QueryString("Message"), ProfileSubscriptionMessage.None)
        End Get
    End Property
    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewUserProfileWizardEndPage.isSystemOutOfOrder
        Get
            Return Not AccessoSistema
        End Get
    End Property
    Public Property CurrentMessage As ProfileSubscriptionMessage Implements IViewUserProfileWizardEndPage.CurrentMessage
        Get
            Return ViewStateOrDefault("CurrentMessage", ProfileSubscriptionMessage.None)
        End Get
        Set(value As ProfileSubscriptionMessage)
            ViewState("CurrentMessage") = value
        End Set
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        Me.Master.ShowLanguage = True
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTtitleWizardCompleted)
            .setLiteral(LTstartPage)
            .setLiteral(LTshibbolethLogon)
            .setLiteral(LTexternalWebLogon)
            .setLiteral(LTbackToLoginPage)
            LTmessage.Text = ""


            Dim url As String = BaseUrl
            If SystemSettings.Login.isSSLloginRequired AndAlso Me.Request.Url.AbsoluteUri.StartsWith("http://") Then
                url = PageUtility.SecureApplicationUrlBase
            End If
            If (LTbackToLoginPage.Text.Contains("{0}")) Then
                LTbackToLoginPage.Text = String.Format(LTbackToLoginPage.Text, url & lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False))
            End If

            If (LTshibbolethLogon.Text.Contains("{0}")) Then
                LTshibbolethLogon.Text = String.Format(LTshibbolethLogon.Text, url & lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.ShibbolethLogin(False))
            End If
            If (LTstartPage.Text.Contains("{0}")) Then
                LTstartPage.Text = String.Format(LTstartPage.Text, url & SystemSettings.Presenter.DefaultStartPage)
            End If
        End With
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region


#Region "Implements"
    Public Sub LoadMessage(message As ProfileSubscriptionMessage) Implements IViewUserProfileWizardEndPage.LoadMessage
        LTmessage.Text = Resource.getValue("ProfileSubscriptionMessage." & message.ToString)

        If message = ProfileSubscriptionMessage.MatriculaDuplicated Then
            LTmessage.Text = String.Format(LTmessage.Text, LocalizedMail.SystemSender)
        End If
    End Sub
    Public Sub DisplaySystemOutOfOrder() Implements IViewUserProfileWizardEndPage.DisplaySystemOutOfOrder
        Me.PageUtility.RedirectToUrl(Me.SystemSettings.Presenter.DefaultStartPage, SystemSettings.Login.isSSLloginRequired)
    End Sub
    Public Sub GotoExternalLoginPage(url As String) Implements IViewUserProfileWizardEndPage.GotoExternalLoginPage
        Me.Response.Redirect(url)
    End Sub
    Public Sub GotoInternalLogin() Implements IViewUserProfileWizardEndPage.GotoInternalLogin
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False), SystemSettings.Login.isSSLloginRequired)
    End Sub
    Public Sub SetExternalWebLogonUrl(url As String) Implements IViewUserProfileWizardEndPage.SetExternalWebLogonUrl
        If (LTexternalWebLogon.Text.Contains("{0}")) Then
            LTexternalWebLogon.Text = String.Format(LTexternalWebLogon.Text, url)


        End If
    End Sub
#End Region

   
End Class