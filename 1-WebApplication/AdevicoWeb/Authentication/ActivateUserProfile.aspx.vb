Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Authentication
Imports lm.Comol.Core.BaseModules.AuthenticationManagement
Imports lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation

Public Class ActivateUserProfile
    Inherits PageBase
    Implements IViewActivateUserProfile

#Region "Context"
    Private _Presenter As ActivateUserProfilePresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As ActivateUserProfilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ActivateUserProfilePresenter(Me.CurrentContext, Me)
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
    Public ReadOnly Property PreloadedIdUser As Integer Implements IViewActivateUserProfile.PreloadedIdUser
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadedUrlIdentifier As System.Guid Implements IViewActivateUserProfile.PreloadedUrlIdentifier
        Get
            Try
                Return New System.Guid(Request.QueryString("Identifier"))
            Catch ex As Exception

            End Try
            Return System.Guid.Empty
        End Get
    End Property
    Public WriteOnly Property AllowExternalWebAuthentication As Boolean Implements IViewActivateUserProfile.AllowExternalWebAuthentication
        Set(value As Boolean)
            Me.LTexternalWebLogon.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowInternalAuthentication As Boolean Implements IViewActivateUserProfile.AllowInternalAuthentication
        Set(value As Boolean)
            Me.LTinternalLoginPage.Visible = value
        End Set
    End Property

    Public ReadOnly Property AllowAdminAccess As Boolean Implements IViewActivateUserProfile.AllowAdminAccess
        Get
            Return (Request.QueryString("AdminAccess") = "true")
        End Get
    End Property
    Public ReadOnly Property isSystemOutOfOrder As Boolean Implements IViewActivateUserProfile.isSystemOutOfOrder
        Get
            Return Not AccessoSistema
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()

            Me.Master.ShowLanguage = True
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
            .setLiteral(LTinternalLoginPage)

            .setLiteral(LTexternalWebLogon)

            ' sistemare link
            If (LTinternalLoginPage.Text.Contains("{0}")) Then
                LTinternalLoginPage.Text = String.Format(LTinternalLoginPage.Text, BaseUrl & RootObject.InternalLogin(AllowAdminAccess))
            End If


            .setLiteral(LTtitleActivateUserProfile)
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
    Public Sub DisplaySystemOutOfOrder() Implements IViewActivateUserProfile.DisplaySystemOutOfOrder
        PageUtility.RedirectToUrl(RootObject.SystemOutOforder)
    End Sub
    Public Sub DisplayActivationInfo() Implements IViewActivateUserProfile.DisplayActivationInfo
        Me.LBmessage.Text = Resource.getValue("DisplayActivationInfo")
    End Sub

    Public Sub DisplayAlreadyActivatedInfo() Implements IViewActivateUserProfile.DisplayAlreadyActivatedInfo
        Me.LBmessage.Text = Resource.getValue("DisplayAlreadyActivatedInfo")
    End Sub

    Public Sub DisplayUnknownUser() Implements IViewActivateUserProfile.DisplayUnknownUser
        Me.LBmessage.Text = Resource.getValue("DisplayUnknownUser")
    End Sub
#End Region

    Public Sub SetExternalWebLogonUrl(url As String) Implements IViewActivateUserProfile.SetExternalWebLogonUrl
        If (LTexternalWebLogon.Text.Contains("{0}")) Then
            LTexternalWebLogon.Text = String.Format(LTexternalWebLogon.Text, url)
        End If
    End Sub

    Public Sub ReloadLanguageSettings(idlanguage As Integer, code As String) Implements IViewActivateUserProfile.ReloadLanguageSettings
        Dim language As New Lingua(idlanguage, code)
        Me.OverloadLanguage(language)
        Me.SetCultureSettings()
        Me.SetInternazionalizzazione()
    End Sub

    Public Sub SendActivationMail(person As lm.Comol.Core.DomainModel.Person) Implements IViewActivateUserProfile.SendActivationMail
        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(Session("LinguaCode"))

        Dim oUtility As New OLDpageUtility(Me.Context)
        Dim oMail As New COL_E_Mail(oUtility.LocalizedMail)
        Dim BodyAdmin As String
        Dim activatedOn As DateTime = DateTime.Now
        Dim sender As New MailAddress(oResourceConfig.getValue("systemMail"), oResourceConfig.getValue("systemMailSender"))
        oMail.Mittente = sender
        oMail.IndirizziTO.Add(New MailAddress(person.Mail, person.Name & " " & person.Surname))
        oMail.IndirizziCCN.Add(sender)
        oMail.Oggetto = Me.Resource.getValue("accountActivationMailSubject")
        oMail.Body = String.Format(Me.Resource.getValue("accountActivationMailBody"), person.Name, person.Surname, FormatDateTime(activatedOn, DateFormat.LongDate), FormatDateTime(activatedOn, DateFormat.ShortTime))

        oMail.Body = oMail.Body & vbCrLf & vbCrLf & vbCrLf & oUtility.LocalizedMail.SystemFirmaNotifica
        oMail.Body = Replace(oMail.Body, "(*)", "")
        oMail.Body = Replace(oMail.Body, "*", "")
        oMail.Body = Replace(oMail.Body, "&nbsp;", "")
        oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
        oMail.InviaMail()
    End Sub

#End Region

End Class