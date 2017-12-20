Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class InternalLogin
    Inherits PageBase
    Implements IViewInternalLogin

#Region "Context"
    Private _Presenter As InternalLoginPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As InternalLoginPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InternalLoginPresenter(Me.CurrentContext, Me)
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
    Public WriteOnly Property AllowRetrievePassword As Boolean Implements IViewInternalLogin.AllowRetrievePassword
        Set(value As Boolean)
            Me.LTretrievePassword.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowSubscription As Boolean Implements IViewInternalLogin.AllowSubscription
        Set(value As Boolean)
            Me.LTsubscription.Visible = value AndAlso PageUtility.SystemSettings.Presenter.AllowUserRegistration
        End Set
    End Property
    Public WriteOnly Property AllowAuthentication As Boolean Implements IViewLogin.AllowAuthentication
        Set(value As Boolean)
            Me.BTNlogin.Enabled = True
        End Set
    End Property
    Public WriteOnly Property AllowExternalWebAuthentication As Boolean Implements IViewInternalLogin.AllowExternalWebAuthentication
        Set(value As Boolean)
            Me.LTexternalWebLogon.Visible = value
        End Set
    End Property

    Public ReadOnly Property AllowAdminAccess As Boolean Implements IViewLogin.AllowAdminAccess
        Get
            Return (Request.QueryString("AdminAccess") = "true")
        End Get
    End Property
    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewLogin.isSystemOutOfOrder
        Get
            Return Not AccessoSistema
        End Get
    End Property
    Public Property LoggedUserId As Integer Implements IViewLogin.LoggedUserId
        Get
            Return ViewStateOrDefault("LoggedUserId", 0)
        End Get
        Set(value As Integer)
            ViewState("LoggedUserId") = value
        End Set
    End Property
    Public Function GetUrlToken(identifiers As List(Of String)) As dtoUrlToken Implements IViewInternalLogin.GetUrlToken
        For Each identifier As String In identifiers
            If ((From s As String In Request.QueryString.AllKeys Select s.ToLower).ToList.Contains(identifier.ToLower)) Then
                Dim dto As New dtoUrlToken
                dto.Identifier = identifier
                dto.Value = Request.QueryString(identifier)

                Return dto
            End If
        Next
        Return Nothing
    End Function
    Public ReadOnly Property HasUrlValues As Boolean Implements IViewInternalLogin.HasUrlValues
        Get
            Return (From k As String In Request.QueryString.AllKeys Where k <> "AdminAccess" Select k).Any
        End Get
    End Property
    Public ReadOnly Property SubscriptionActive As Boolean Implements IViewInternalLogin.SubscriptionActive
        Get
            Return SystemSettings.Login.SubscriptionActive
        End Get
    End Property
#End Region

    Public ReadOnly Property TranslatedPassword As String
        Get
            Return Me.Resource.getValue("TXBpassword.text")
        End Get
    End Property
    Public ReadOnly Property TranslatedLogin As String
        Get
            Return Me.Resource.getValue("TXBlogin.text")
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowLanguage = True
        If Page.IsPostBack = False Then
            If Not Request.IsSecureConnection AndAlso PageUtility.SystemSettings.Login.isSSLloginRequired Then
                Response.Redirect(Replace(Request.Url.ToString().ToLower, "http://", "https://"))
            Else
                CurrentPresenter.InitView()
                SetInfo()
            End If
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
            .setLiteral(LTretrievePassword)
            .setLiteral(LTexternalWebLogon)
            .setLiteral(LTsubscription)
            .setLiteral(LTtitleInternalLogin)

            .setTextBox(TXBlogin)
            TXBlogin.Attributes("placeholder") = TranslatedLogin

            'TXBlogin.Attributes.Add("onfocus", "if (this.value=='" & Me.TXBlogin.Text & "') this.value = ''")
            'TXBlogin.Attributes.Add("onblur", "if (this.value=='') this.value = '" & Me.TXBlogin.Text & "'")

            .setButton(BTNlogin, True, , , True)
            TXBpassword.Attributes("placeholder") = TranslatedPassword

            .setLiteral(LTloginError)
            .setLiteral(LTloginErrorRetrieve)

            Dim url As String = BaseUrl
            If SystemSettings.Login.isSSLloginRequired AndAlso Me.Request.Url.AbsoluteUri.StartsWith("http://") Then
                url = PageUtility.SecureApplicationUrlBase
            End If
            ' sistemare link
            If (LTretrievePassword.Text.Contains("{0}")) Then
                LTretrievePassword.Text = String.Format(LTretrievePassword.Text, url & RootObject.InternalRetrievePassword(AllowAdminAccess))
            End If


            '  <item name="LTexternalWebLogon.text">&lt;li&gt;&lt;a class="account-type" href="{0}"&gt;Accedi dal sito aziendale&lt;/a&gt;&lt;/li&gt;</item>
            If (LTsubscription.Text.Contains("{0}")) Then
                LTsubscription.Text = String.Format(LTsubscription.Text, url & lm.Comol.Core.BaseModules.ProfileManagement.RootObject.InternalProfileWizard)
            End If
            If (LTloginErrorRetrieve.Text.Contains("{0}")) Then
                LTloginErrorRetrieve.Text = String.Format(LTloginErrorRetrieve.Text, url & RootObject.InternalRetrievePassword(AllowAdminAccess))
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
#Region "Messaggi"
    Public Sub DisplaySystemOutOfOrder() Implements IViewLogin.DisplaySystemOutOfOrder
        PageUtility.RedirectToUrl(RootObject.SystemOutOforder)
    End Sub
    Public Sub DisplayLogonInput() Implements IViewInternalLogin.DisplayLogonInput
        Me.MLVlogon.SetActiveView(VIWdefault)
        Resource.setTextBox(TXBlogin)
        Resource.setTextBox(TXBpassword)
        Me.SPNmessages.Attributes.Add("class", "invisible")
    End Sub
    Public Sub DisplayInvalidCredentials() Implements IViewInternalLogin.DisplayInvalidCredentials
        Me.SPNmessages.Attributes.Add("class", "")
        Me.Resource.setLiteral(LTloginError)
    End Sub
    Public Sub DisplayAccountDisabled() Implements IViewLogin.DisplayAccountDisabled
        Me.SPNmessages.Attributes.Add("class", "")
        LTloginError.Text = Resource.getValue("DisplayAccountDisabled")
        If LTloginError.Text.Contains("{0}") Then
            Dim oMailLocalized As MailLocalized = Me.LocalizedMail
            LTloginError.Text = String.Format(LTloginError.Text, oMailLocalized.SystemSender.Address, oMailLocalized.SystemSender.Address)
        End If
    End Sub
    Public Sub DisplayAccountDisabled(url As String) Implements IViewInternalLogin.DisplayAccountDisabled
        Me.PageUtility.RedirectToUrl(url)
    End Sub

    Public Sub DisplayAuthenticationOutOfOrder() Implements IViewLogin.DisplayAuthenticationOutOfOrder
        '  Me.MLVlogon.SetActiveView(VIWsystemOutOfOrder)
    End Sub
#End Region
    Private Sub DisplayPrivacyPolicy(userId As Integer, idProvider As Long, providerUrl As String, internalPage As Boolean) Implements IViewLogin.DisplayPrivacyPolicy
        Me.PageUtility.PreloggedUserId = userId
        Me.PageUtility.PreloggedProviderId = idProvider
        Me.PageUtility.PreloggedProviderUrl = IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.PolicyManagement.RootObject.AcceptLogonPolicy)
    End Sub

    Public Sub LogonUser(user As Person, idProvider As Long, providerUrl As String, internalPage As Boolean, idUserDefaultIdOrganization As Int32) Implements IViewLogin.LogonUser
        PageUtility.LogonUser(user, idProvider, IIf(internalPage, PageUtility.ApplicationUrlBase(False, True), "") & providerUrl, idUserDefaultIdOrganization)
    End Sub

    Public Sub GotoAuthenticationSelctorPage() Implements IViewLogin.GotoAuthenticationSelctorPage
        Me.PageUtility.RedirectToUrl(RootObject.InternalShibbolethAuthenticationPage(AllowAdminAccess))
    End Sub

    Public Sub GoToProfile(wizardUrl As String) Implements IViewLogin.GoToProfile
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.ProfileManagement.RootObject.InternalProfileWizard(AllowAdminAccess), SystemSettings.Login.isSSLloginRequired)
    End Sub
    Public Sub GoToProfile(idProvider As Long, urlToken As dtoUrlToken, wizardUrl As String) Implements IViewInternalLogin.GoToProfile
        Dim oRemotePost As New lm.Comol.Modules.Base.DomainModel.RemotePost
        oRemotePost.Url = Me.BaseUrl & wizardUrl

        oRemotePost.Add("Identifier", urlToken.Identifier)
        oRemotePost.Add("DecriptedValue", urlToken.DecriptedValue)

        oRemotePost.Post()
    End Sub
    Public Sub SetExternalWebLogonUrl(url As String) Implements IViewInternalLogin.SetExternalWebLogonUrl
        If (LTexternalWebLogon.Text.Contains("{0}")) Then
            LTexternalWebLogon.Text = String.Format(LTexternalWebLogon.Text, url)
            LTexternalWebLogon.Visible = True
        Else
            LTexternalWebLogon.Visible = False
        End If
    End Sub

    Private Sub GotoRemoteUrl(url As String) Implements IViewInternalLogin.GotoRemoteUrl
        Response.Redirect(url)
    End Sub
    Private Sub DisplayMustEditPassword(userId As Integer, idProvider As Long) Implements IViewInternalLogin.DisplayMustEditPassword
        Me.PageUtility.PreloggedUserId = userId
        Me.PageUtility.PreloggedProviderId = idProvider
        Me.PageUtility.RedirectToUrl(lm.Comol.Core.BaseModules.ProfileManagement.RootObject.MustChangePassword)
    End Sub
#End Region

    Private Sub BTNlogin_Click(sender As Object, e As System.EventArgs) Handles BTNlogin.Click
        CurrentPresenter.Authenticate(Me.TXBlogin.Text, Me.TXBpassword.Text)
    End Sub


#Region "Info"
    Private Sub SetInfo()
        Me.ClearInfo()
        If Not IsNothing(SystemSettings.SkinSettings.LoginInfos) OrElse SystemSettings.SkinSettings.LoginInfos.Count > 0 Then
            Me.OpenInfo()
            For Each Infostr As String In SystemSettings.SkinSettings.LoginInfos
                Me.AddInfo(Infostr)
            Next
            Me.CloseInfo()
        End If

    End Sub

    Private Sub AddInfo(ByVal Text As String)
        Me.LTinfo.Text &= "<span class=""info-text"">"
        Me.LTinfo.Text &= Text
        Me.LTinfo.Text &= "</span>"
    End Sub

    Private Sub OpenInfo()
        Me.LTinfo.Text &= "<div class=""box full"">"
    End Sub

    Private Sub CloseInfo()
        Me.LTinfo.Text &= "</div>"
    End Sub

    Private Sub ClearInfo()
        Me.LTinfo.Text = ""
    End Sub
#End Region

    Public Sub DisplayInvalidToken(url As String, idPerson As Integer, urlToken As lm.Comol.Core.Authentication.dtoUrlToken, status As UrlProviderResult) Implements IViewLogin.DisplayInvalidToken
        PageUtility.NotifyTokenError(idPerson, urlToken, status)
        PageUtility.RedirectToUrl(url, False)
    End Sub

End Class