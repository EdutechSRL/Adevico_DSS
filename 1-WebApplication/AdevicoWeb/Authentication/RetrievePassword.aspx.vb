Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
Public Class RetrievePassword
    Inherits PageBase
    Implements IViewRetrievePassword


#Region "Context"
    Private _Presenter As RetrievePasswordPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As RetrievePasswordPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RetrievePasswordPresenter(Me.CurrentContext, Me)
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
    Public WriteOnly Property AllowSubscription As Boolean Implements IViewRetrievePassword.AllowSubscription
        Set(value As Boolean)
            Me.LTsubscription.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowExternalWebAuthentication As Boolean Implements IViewRetrievePassword.AllowExternalWebAuthentication
        Set(value As Boolean)
            Me.LTexternalWebLogon.Visible = value
        End Set
    End Property

    Public WriteOnly Property AllowBackFromRetrieve As Boolean Implements IViewRetrievePassword.AllowBackFromRetrieve
        Set(value As Boolean)
            Me.LTbackToLoginPage.Visible = value
        End Set
    End Property

    Public ReadOnly Property AllowAdminAccess As Boolean Implements IViewRetrievePassword.AllowAdminAccess
        Get
            Return (Request.QueryString("AdminAccess") = "true")
        End Get
    End Property
    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewRetrievePassword.isSystemOutOfOrder
        Get
            Return Not AccessoSistema
        End Get
    End Property
    Public ReadOnly Property SubscriptionActive As Boolean Implements IViewRetrievePassword.SubscriptionActive
        Get
            Return SystemSettings.Login.SubscriptionActive
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()

            Me.Master.ShowLanguage = False
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
            .setLiteral(LTbackToLoginPage)
            .setLiteral(LTshibbolethLogon)
            .setLiteral(LTexternalWebLogon)
            .setLiteral(LTsubscription)
            .setLiteral(LTbackToLoginPage)

            Dim url As String = BaseUrl
            If SystemSettings.Login.isSSLloginRequired AndAlso Me.Request.Url.AbsoluteUri.StartsWith("http://") Then
                url = PageUtility.SecureApplicationUrlBase
            End If
            ' sistemare link
            If (LTbackToLoginPage.Text.Contains("{0}")) Then
                LTbackToLoginPage.Text = String.Format(LTbackToLoginPage.Text, url & RootObject.InternalLogin(AllowAdminAccess))
            End If

            If (LTshibbolethLogon.Text.Contains("{0}")) Then
                LTshibbolethLogon.Text = String.Format(LTshibbolethLogon.Text, url & RootObject.ShibbolethLogin(AllowAdminAccess))
            End If

            '  <item name="LTexternalWebLogon.text">&lt;li&gt;&lt;a class="account-type" href="{0}"&gt;Accedi dal sito aziendale&lt;/a&gt;&lt;/li&gt;</item>
            If (LTsubscription.Text.Contains("{0}")) Then
                LTsubscription.Text = String.Format(LTsubscription.Text, url & lm.Comol.Core.BaseModules.ProfileManagement.RootObject.InternalProfileWizard)
            End If

            .setLiteral(LTtitleInternalLoginRetrieve)
            .setLiteral(LTmailInfo)
            .setTextBox(TXBmail)
            TXBmail.Attributes.Add("onfocus", "if (this.value=='" & Me.TXBmail.Text & "') this.value = ''")
            TXBmail.Attributes.Add("onblur", "if (this.value=='') this.value = '" & Me.TXBmail.Text & "'")

            .setButton(BTNretrievePassword, True, , , True)
            .setLiteral(LTretrieveError)
            .setLiteral(LTretrieveErrorSubscription)
            .setLiteral(LTretrieveErrorShibbolethAccount)
            LTretrieveErrorSubscription.Visible = SystemSettings.Presenter.AllowUserRegistration
            If (LTretrieveErrorSubscription.Text.Contains("{0}")) Then
                LTretrieveErrorSubscription.Text = String.Format(LTretrieveErrorSubscription.Text, url & lm.Comol.Core.BaseModules.ProfileManagement.RootObject.InternalProfileWizard)
            End If
            If (LTretrieveErrorShibbolethAccount.Text.Contains("{0}")) Then
                LTretrieveErrorShibbolethAccount.Text = String.Format(LTretrieveErrorShibbolethAccount.Text, url & RootObject.ShibbolethLogin(AllowAdminAccess))
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
    Public Sub DisplaySystemOutOfOrder() Implements IViewRetrievePassword.DisplaySystemOutOfOrder
        PageUtility.RedirectToUrl(RootObject.SystemOutOforder)
    End Sub
    Public Sub DisplayRetrievePassword() Implements IViewRetrievePassword.DisplayRetrievePassword
        Resource.setTextBox(TXBmail)
        Me.SPNmessages.Attributes.Add("class", "invisible")
    End Sub
    Public Sub DisplayRetrievePasswordError() Implements IViewRetrievePassword.DisplayRetrievePasswordError
        Me.SPNmessages.Attributes.Add("class", "")
        LTretrieveError.Text = Me.Resource.getValue("RetrievePasswordError")
    End Sub
    Public Sub DisplayRetrievePasswordUnknownLogin() Implements IViewRetrievePassword.DisplayRetrievePasswordUnknownLogin
        Me.SPNmessages.Attributes.Add("class", "")
        Me.Resource.setLiteral(LTretrieveError)
    End Sub

#End Region
    Public Sub SetExternalWebLogonUrl(url As String) Implements IViewRetrievePassword.SetExternalWebLogonUrl
        If (LTexternalWebLogon.Text.Contains("{0}")) Then
            LTexternalWebLogon.Text = String.Format(LTexternalWebLogon.Text, url)
        End If
    End Sub

    Public Sub GotoInternalLogin() Implements IViewRetrievePassword.GotoInternalLogin
        PageUtility.RedirectToUrl(RootObject.InternalLogin(AllowAdminAccess), SystemSettings.Login.isSSLloginRequired)
    End Sub
#End Region

    Private Sub BTNretrievePassword_Click(sender As Object, e As System.EventArgs) Handles BTNretrievePassword.Click
        CurrentPresenter.RetrievePassword(Me.TXBmail.Text)
    End Sub

    Public Sub SendMail(user As InternalLoginInfo, password As String, l As lm.Comol.Core.DomainModel.Language) Implements IViewRetrievePassword.SendMail
        Dim oUserResource As ResourceManager = Nothing
        Dim mailTranslated As MailLocalized = Nothing
        If PageUtility.IsoLanguageCodeChanged OrElse IsNothing(l) Then
            oUserResource = Resource
            mailTranslated = PageUtility.LocalizedMail(PageUtility.LinguaID)
        Else

            oUserResource = New ResourceManager

            oUserResource.UserLanguages = l.Code
            oUserResource.ResourcesName = "pg_ISAuthenticationPage"
            oUserResource.Folder_Level1 = "Authentication"
            oUserResource.setCulture()


            mailTranslated = PageUtility.LocalizedMail(l.Id)
        End If



        Dim mail As New COL_E_Mail(mailTranslated)

        mail.Mittente = mailTranslated.SystemSender
        mail.IndirizziTO.Add(New MailAddress(user.Person.Mail))


        mail.Oggetto = oUserResource.getValue("newPasswordmailSubject")
        mail.Body = String.Format(oUserResource.getValue("newPasswordmailBody"), user.Person.SurnameAndName, user.Login, password, Me.PageUtility.ApplicationUrlBase)

        mail.Body = mail.Body & vbCrLf & mailTranslated.SystemFirmaNotifica
        mail.Body = Replace(mail.Body, "<br>", vbCrLf)

        mail.InviaMail()
    End Sub
End Class