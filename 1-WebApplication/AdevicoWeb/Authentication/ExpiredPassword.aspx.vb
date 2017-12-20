Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class ExpiredPassword
    Inherits PageBase
    Implements IViewLogonExpiredPassword

#Region "Context"
    Private _Presenter As LogonExpiredPasswordPresenter

    Public ReadOnly Property CurrentPresenter() As LogonExpiredPasswordPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New LogonExpiredPasswordPresenter(Me.PageUtility.CurrentContext, Me)
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
    Public Property PreloggedUserId As Integer Implements IViewLogonExpiredPassword.PreloggedUserId
        Get
            Return Me.PageUtility.PreloggedUserId
        End Get
        Set(value As Integer)
            Me.PageUtility.PreloggedUserId = value
        End Set
    End Property
    Public Property PreloggedProviderId As Long Implements IViewLogonExpiredPassword.PreloggedProviderId
        Get
            Return Me.PageUtility.PreloggedProviderId
        End Get
        Set(value As Long)
            Me.PageUtility.PreloggedProviderId = value
        End Set
    End Property
    Public Property LoggedUserId As Integer Implements IViewLogonExpiredPassword.LoggedUserId
        Get
            Return ViewStateOrDefault("LoggedUserId", CInt(0))
        End Get
        Set(value As Integer)
            Me.ViewState("LoggedUserId") = value
        End Set
    End Property
    Public Property LoggedProviderId As Long Implements IViewLogonExpiredPassword.LoggedProviderId
        Get
            Return ViewStateOrDefault("LoggedProviderId", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("LoggedProviderId") = value
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
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTtitleMustEditPassword)
            .setLabel(LBpasswordConfirm_t)
            .setLabel(LBpasswordNew_t)
            .setLabel(LBpasswordOld_t)
            .setButton(BTNsaveNewPassword, True)
            .setCompareValidator(CMVpassword)
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
    Private Sub DisplayPasswordNotChanged() Implements IViewLogonExpiredPassword.DisplayPasswordNotChanged
        LBmessage.Text = Resource.getValue("PasswordNotChanged")
    End Sub
    Private Sub DisplayInvalidPassword() Implements IViewLogonExpiredPassword.DisplayInvalidPassword
        LBmessage.Text = Resource.getValue("InvalidPassword")
    End Sub
    Private Sub DisplayMustChangePassword(type As lm.Comol.Core.Authentication.EditType) Implements IViewLogonExpiredPassword.DisplayMustChangePassword
        LTtitleMustEditPassword.Text = Resource.getValue("LTtitleMustEditPassword." & type.ToString)
        LBmessage.Text = Resource.getValue("PasswordInfo")
    End Sub
    Private Sub DisplayPasswordExpiredOn(expiredOn As Date) Implements IViewLogonExpiredPassword.DisplayPasswordExpiredOn
        LTtitleMustEditPassword.Text = String.Format(Resource.getValue("LTtitleMustEditPassword.expiredOn"), FormatDateTime(expiredOn, DateFormat.ShortDate))
    End Sub

    Private Sub DisplaySamePasswordException() Implements IViewLogonExpiredPassword.DisplaySamePasswordException
        LBmessage.Text = Resource.getValue("DisplaySamePasswordException")
    End Sub

    Private Sub GotoRemoteLogonPage(url As String) Implements IViewLogonExpiredPassword.GotoRemoteLogonPage
        Response.Redirect(url)
    End Sub
    Private Sub GotoInternalAuthenticationPage() Implements IViewLogonExpiredPassword.GotoInternalAuthenticationPage
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalLogin(False))
    End Sub
    Private Sub GotoInternalShibbolethAuthenticationPage() Implements IViewLogonExpiredPassword.GotoInternalShibbolethAuthenticationPage
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.InternalShibbolethAuthenticationPage(False))
    End Sub
    Private Sub GotoShibbolethAuthenticationPage() Implements IViewLogonExpiredPassword.GotoShibbolethAuthenticationPage
        PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.AuthenticationManagement.RootObject.ShibbolethLogin(False))
    End Sub
    Private Sub LogonUser(user As Person, idDefaultCommunity As Integer, idProvider As Long, providerUrl As String, internalPage As Boolean, idDefaultORganization As Integer) Implements IViewLogonExpiredPassword.LogonUser
        Me.PageUtility.LogonUser(user, idDefaultCommunity, idProvider, IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl, idDefaultORganization)
    End Sub
    Private Sub DisplayPrivacyPolicy(userId As Integer, idProvider As Long, providerUrl As String, internalPage As Boolean) Implements IViewLogonExpiredPassword.DisplayPrivacyPolicy
        Me.PageUtility.PreloggedUserId = userId
        Me.PageUtility.PreloggedProviderId = idProvider
        Me.PageUtility.PreloggedProviderUrl = IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.PolicyManagement.RootObject.AcceptLogonPolicy)
    End Sub
#End Region

    Private Sub BTNsaveNewPassword_Click(sender As Object, e As System.EventArgs) Handles BTNsaveNewPassword.Click
        Me.CurrentPresenter.EditPassword(Me.TXBoldPpassword.Text, Me.TXBconfirmPassword.Text)
    End Sub

   
End Class