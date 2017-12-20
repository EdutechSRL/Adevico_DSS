Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class AccessExternal
    Inherits PageBase
    Implements TK.Presentation.View.iViewTicketAccess


#Region "Context"
    'Definizion Presenter...
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketAccessPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketAccessPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketAccessPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne
    Private Enum PwdValidationError
        none
        empty
        invalidformat
        notEqual
    End Enum

#End Region

#Region "Implements"
    'Property della VIEW
    Public ReadOnly Property GetLanguageCode As String Implements TK.Presentation.View.iViewTicketAccess.GetLanguageCode
        Get
            Return MyBase.LinguaCode
        End Get
    End Property

    Private Property ShowLoginToken As Boolean
        Get
            Dim Show As Boolean = False
            Try
                Show = ViewStateOrDefault(Of Boolean)("ShowToken", False)
            Catch ex As Exception

            End Try
            Return Show
        End Get
        Set(value As Boolean)
            ViewState("ShowToken") = value
        End Set
    End Property

    Public ReadOnly Property Settings As TK.Domain.DTO.DTO_NotificationSettings Implements TK.Presentation.View.iViewTicketAccess.Settings
        Get

            Dim sets As New TK.Domain.DTO.DTO_NotificationSettings()
            With sets
                .BaseUrl = ApplicationUrlBase
                .CategoriesTemplate = ""
                .DateTimeFormat = TicketHelper.DateTimeFormat
                .AvailableCategoryTypes = Nothing
                .AvailableTicketStatus = Nothing
                .LangCode = Me.CTRLlang.GetLanguageCode() 'GetCurrentUser.LanguageCode    'Override nel service
                .SmtpConfig = PageUtility.CurrentSmtpConfig
            End With

            Return sets
        End Get

    End Property
    Public ReadOnly Property UrlToken As String Implements TK.Presentation.View.iViewTicketAccess.UrlToken
        Get
            Return Request.QueryString("Tk")
        End Get
    End Property

#End Region

#Region "Inherits"
    'Property del PageBase

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

    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'HideErrors()
        Me.Master.Page_Title = Resource.getValue("Page.Title.External")
    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        Me.Master.BindSkin()

        If (Me.CurrentPresenter.InitView()) Then
            HideErrors()

            Me.TXBloginToken.Text = ""
            ShowView(TK.Domain.Enums.AccessView.login)
            SetLoginView(TK.Domain.Enums.LoginStatus.normal)
            SetTitleDescription()
        End If

        'Me.CurrentPresenter.InitView()
        



    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AccessExternal", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()


        With Resource

            .setLiteral(LTpageTitle_t)
            '.setLiteral(LTcontentTitle_t)

            .setLabel(LBloginmail_t)
            .setLabel(LBlogincode_t)

            .setLabel(LBregmail_t)
            .setLabel(LBregname_t)
            .setLabel(LBregSname_t)

            .setLabel(LBregPwd)
            .setLabel(LBregPwd2)

            .setLabel(LBrecMail_t)


            .setLabel(LBrecMail_t)
            .setLabel(LBrecMail_t)
            .setLabel(LBrecMail_t)

            .setLabel(LBcaptcha_t)

            .setButton(BTNenter, True, False, False, True)
            .setButton(BTNcreate, True, False, False, True)
            .setButton(BTNrecover, True, False, False, True)
            .setButton(BTNvalidate, True, False, False, True)
            '.setButton(BTNchange, True, False, False, True)

            .setLinkButton(LNBtoLogin, True, True)
            .setLinkButton(LNBtoCreate, True, True)
            .setLinkButton(LNBtoRecover, True, True)

            Me.RCPcaptcha.CaptchaAudioLinkButtonText = .getValue("Captcha.AudioLinkButtonText")
            '
            'LBLchangePwdOLD_t
            'LBLchangePwdNEW1_t
            'LBLchangePwdNEW2_t

            Me.Master.Page_Title = .getValue("Page.Title.External")

            Me.SetMandatory()

            .setLiteral(LTnoAccess)

        End With

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    'Sub e function della View

    Public Sub ShowView(currentView As TK.Domain.Enums.AccessView) Implements TK.Presentation.View.iViewTicketAccess.ShowView

        PNLconfirm.Visible = True

        Select Case currentView
            'Case TK.Domain.Enums.AccessView.login
            Case TK.Domain.Enums.AccessView.register
                Me.MLVviews.SetActiveView(Me.VIWRegister)
                ShowButton(False, True, False, False, True)
                ShowNavigation(True, False, True)

                LTViewTitle.Text = Me.Resource.getValue("StepTitle.register")

            Case TK.Domain.Enums.AccessView.recover
                Me.MLVviews.SetActiveView(Me.VIWRecover)
                ShowButton(False, False, True, False, True)
                ShowNavigation(True, True, False)

                LTViewTitle.Text = Me.Resource.getValue("StepTitle.recover")

            Case TK.Domain.Enums.AccessView.recoverRequestSended
                Me.MLVviews.SetActiveView(Me.VIWRecoverRequestSended)
                ShowButton(False, False, False, False, False)
                ShowNavigation(True, False, False)

                LTViewTitle.Text = Me.Resource.getValue("StepTitle.recover")

                LTrecovered.Text = Me.Resource.getValue("StepDescription.recovered")

            Case TK.Domain.Enums.AccessView.registered
                Me.MLVviews.SetActiveView(Me.VIWregistered)
                ShowButton(False, False, False, False, False)
                ShowNavigation(True, False, False)

                LTViewTitle.Text = Me.Resource.getValue("StepTitle.register")

            Case Else
                Me.MLVviews.SetActiveView(Me.VIWlogin)
                ShowButton(True, False, False, False, False)
                ShowNavigation(False, True, True)

                LTViewTitle.Text = Me.Resource.getValue("StepTitle.login")

        End Select

        SetTitleDescription()

    End Sub

    Public Sub RedirectToCreate() Implements TK.Presentation.View.iViewTicketAccess.RedirectToCreate
        HideErrors()
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.ExternalAdd())
    End Sub

    Public Sub RedirectToList() Implements TK.Presentation.View.iViewTicketAccess.RedirectToList
        HideErrors()
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.ExternalList())
    End Sub

    Public Sub RedirectToTicket(TicketID As Long) Implements TK.Presentation.View.iViewTicketAccess.RedirectToTicket
        HideErrors()
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.ExternalEdit(TicketID))
    End Sub

    Public Sub ShowAccessError([Error] As TK.Domain.Enums.ExternalUserValidateError) Implements TK.Presentation.View.iViewTicketAccess.ShowAccessError
        Select Case [Error]
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.ExternalUserValidateError.invalidMail
                Me.SetHint(LTloginmailHint, Resource.getValue("Error.InvalidMail"))
                SetDivError(Me.DIVloginmail, True)
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.ExternalUserValidateError.invalidCode
                Me.SetHint(LTlogincodeHint, Resource.getValue("Error.InvaliCode"))
                SetDivError(Me.DIVlogincode, True)
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.ExternalUserValidateError.TokenEmpty

                Me.SetHint(LTtokenHint, "")
                Me.SetLoginView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.LoginStatus.token)
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.ExternalUserValidateError.TokenExpired
                Me.SetHint(LTtokenHint, Resource.getValue("Error.ExpiredToken"))
                SetDivError(Me.DIVlogintoken, True)
                Me.TXBloginToken.Text = Me.UrlToken
                Me.SetLoginView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.LoginStatus.token)
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.ExternalUserValidateError.TokenInvalid
                Me.SetHint(LTtokenHint, "")
                Me.SetLoginView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.LoginStatus.token)
        End Select
        'Me.LITerror.Text = [Error].ToString()
    End Sub

    Public Sub ShowRecoverError([Error] As TK.Domain.Enums.AccessRecoverError) Implements TK.Presentation.View.iViewTicketAccess.ShowRecoverError

        SetDivError(Me.DIVrecMail, True)
        SetHint(LTrecMailHint, Resource.getValue("RecError." & [Error].ToString))

        If [Error] = TK.Domain.Enums.AccessRecoverError.MailFormat _
            OrElse [Error] = TK.Domain.Enums.AccessRecoverError.MailNotChecked _
            OrElse [Error] = TK.Domain.Enums.AccessRecoverError.MailNotFound Then
            SetDivError(Me.DIVrecMail, True)
            SetHint(LTrecMailHint, Resource.getValue("RecError." & [Error].ToString))
        ElseIf [Error] = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.AccessRecoverError.InternalError Then
            LTrecovered.Text = Me.Resource.getValue("StepDescription.NotRecovered")
        End If

    End Sub

    Public Sub ShowRegistered(Result As TK.Domain.DTO.DTO_ExtUserAddResult) Implements TK.Presentation.View.iViewTicketAccess.ShowRegistered

        If Result.Errors = TK.Domain.Enums.ExternalUserCreateError.none Then
            Me.ShowView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.AccessView.registered)
            Me.LTreged.Text = Resource.getValue("LTreged.text").Replace("{displayname}", Result.User.SName & " " & Result.User.Name).Replace("{mail}", Result.User.Mail)
            'TEST:
            Dim User As TK.Domain.DTO.DTO_User = GetCurrentUser()
            If Not IsNothing(User) Then
                Result.Note &= "<hr>Test Value:"
                Result.Note &= "<br/>Session: " & User.Name & " " & User.SName

            End If
            Me.LTreged.Text &= Result.Note

            ShowLoginToken = True
            Me.TXBloginmail.Text = Me.TXBregMail.Text

        Else

            Select Case Result.Errors
                Case TK.Domain.Enums.ExternalUserCreateError.TicketMail
                    Me.SetHint(LTregmailHint, Resource.getValue("Error.MailInTicket"))
                Case TK.Domain.Enums.ExternalUserCreateError.internalMail
                    Me.SetHint(LTregmailHint, Resource.getValue("Error.MailInTicket"))
                Case TK.Domain.Enums.ExternalUserCreateError.invalidMail
                    Me.SetHint(LTregmailHint, Resource.getValue("Error.InvalidMailFormat"))
                Case TK.Domain.Enums.ExternalUserCreateError.internalError
                    Me.SetHint(LTregmailHint, Resource.getValue("Error.Internal"))
            End Select
            SetDivError(Me.DIVregmail, True)

        End If
    End Sub

    Public Sub DisplayLogin(LoginStat As TK.Domain.Enums.LoginStatus) Implements TK.Presentation.View.iViewTicketAccess.DisplayLogin
        SetLoginView(LoginStat)
    End Sub

    Public Sub ShowValidationError(Result As TK.Domain.Enums.TokenValidationResult) Implements TK.Presentation.View.iViewTicketAccess.ShowValidationError
        Me.LTlogincodeHint.Text = Result.ToString()
    End Sub

    Public Sub SendAction( _
                     Action As TK.ModuleTicket.ActionType, _
                     idCommunity As Integer, _
                     Type As TK.ModuleTicket.InteractionType, _
                    Optional Objects As System.Collections.Generic.IList(Of System.Collections.Generic.KeyValuePair(Of Integer, String)) = Nothing) _
                 Implements TK.Presentation.View.iViewTicketAccess.SendAction

        Dim oList As List(Of WS_Actions.ObjectAction) = Nothing

        If Not IsNothing(Objects) Then
            oList = (From kvp As KeyValuePair(Of Integer, String) In Objects
                    Select Me.PageUtility.CreateObjectAction(kvp.Key, kvp.Value)).ToList()
        End If

        Me.PageUtility.AddActionToModule(idCommunity, Me.CurrentModuleID, Action, oList, Type)

    End Sub
    Public Sub ShowServiceDisabled() Implements TK.Presentation.View.iViewTicketAccess.ShowServiceDisabled

        Me.PNLcontent.Visible = False
        Me.PNLserviceDisabled.Visible = True

    End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"

    Private Sub ShowButton(ByVal Enter As Boolean, ByVal ConfirmReg As Boolean, ByVal Recover As Boolean, ByVal ConfirmToken As Boolean, ByVal ShowCaptcha As Boolean)
        If Enter Or ConfirmReg Or Recover Or ConfirmToken Then
            Me.PNLconfirm.Visible = True
            Me.BTNcreate.Visible = ConfirmReg
            Me.BTNenter.Visible = Enter
            Me.BTNrecover.Visible = Recover
            Me.BTNvalidate.Visible = ConfirmToken
            Me.PNLcatcha.Visible = ShowCaptcha
        Else
            Me.PNLconfirm.Visible = False
        End If
    End Sub

    Private Sub ShowNavigation(ByVal ToLogin As Boolean, ByVal ToRegistration As Boolean, ByVal ToRecover As Boolean)
        If ToLogin Or ToRegistration Or ToRecover Then
            Me.PNLnavigation.Visible = True
            Me.LNBtoLogin.Visible = ToLogin
            Me.LNBtoCreate.Visible = ToRegistration
            Me.LNBtoRecover.Visible = ToRecover

            LNBtoLogin.CssClass = "first accesscommands"
            LNBtoRecover.CssClass = "last accesscommands"
            Me.LNBtoCreate.CssClass = "accesscommands"
            If Not ToLogin Then
                Me.LNBtoCreate.CssClass = "first " & Me.LNBtoCreate.CssClass
            End If

            If Not ToRecover Then
                Me.LNBtoCreate.CssClass = "last " & Me.LNBtoCreate.CssClass
            End If
        Else
            Me.PNLnavigation.Visible = False
        End If
    End Sub

    Private Sub HideErrors()
        SetDivError(Me.DIVloginmail, False)
        SetDivError(Me.DIVlogincode, False)

        SetHint(LTloginmailHint, "")
        SetHint(LTlogincodeHint, "")

        SetDivError(DIVregmail, False)
        SetDivError(DIVregname, False)
        SetDivError(DIVregSname, False)

        SetDivError(DIVregPassword, False)
        SetDivError(DIVregPassword2, False)

        SetDivError(DIVcaptcha, False)

        SetHint(LTregmailHint, "")
        SetHint(LTregnameHint, "")
        SetHint(LTregSnameHint, "")

        SetHint(LTrecMailHint, "")

        SetHint(LTregPwd, "")
        SetHint(LTregPwd2, "")

        SetHint(LTcaptcha, "")
    End Sub

    Private Sub SetHint(ByVal oLiteral As Literal, Optional ByVal ErrText As String = "", Optional ByVal Value As String = "")
        Dim Txt As String = ""
        If Not String.IsNullOrEmpty(Value) Then
            Txt = Resource.getValue(oLiteral.ID & "." & Value)
        Else
            Txt = Resource.getValue(oLiteral.ID & ".text")
        End If

        oLiteral.Text = Me.LThintTemplate.Text.Replace("{HintText}", Txt).Replace("{ErrorText}", ErrText)

    End Sub
    ''' <summary>
    ''' Controlla che la password sia adatta.
    ''' Al momento controllo SOLAMENTE se è stata inserita e se è uguale alla pwd di controllo.
    ''' SUCCESSIVAMENTE potrà:
    '''   - essere spostata nel presenter,
    '''   - controllare lunghezza o latri parametri (caratteri necessari, etc...)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckPwd() As PwdValidationError
        If String.IsNullOrEmpty(Me.TXBregPwd.Text) Then
            Return PwdValidationError.empty
        ElseIf Me.TXBregPwd.Text <> Me.TXBregPwd2.Text Then
            Return PwdValidationError.notEqual
        End If

        Return PwdValidationError.none

    End Function

    Private Sub SetDivError(ByVal DIV As System.Web.UI.HtmlControls.HtmlGenericControl, ByVal HasError As Boolean)
        If HasError Then
            If Not DIV.Attributes("class").Contains(" error") Then
                DIV.Attributes("class") &= " error"
            End If
        Else
            DIV.Attributes("class") = DIV.Attributes("class").Replace(" error", "")
        End If
    End Sub

    Public Sub SetCurrentUser(User As TK.Domain.DTO.DTO_User) Implements TK.Presentation.View.iViewTicketAccess.SetCurrentUser

        Session(TicketHelper.SessionExtUser) = User

    End Sub

    Private Sub SetTitleDescription()
        Dim ViewValue As String = ""
        Select Case MLVviews.ActiveViewIndex
            Case 1
                ViewValue = "register.init"
                LTmandatory_t.Visible = True
            Case 2
                ViewValue = "register.sended"
                LTmandatory_t.Visible = False
            Case 3
                ViewValue = "recover.init"
                LTmandatory_t.Visible = True
            Case 4
                ViewValue = "" '"recover.sended"
                LTmandatory_t.Visible = False
            Case Else
                ViewValue = "login"
                LTmandatory_t.Visible = True
        End Select

        If Not String.IsNullOrEmpty(ViewValue) Then
            Resource.setLiteral(LTviewDescription, ViewValue)
        Else
            LTviewDescription.Text = ""
        End If


    End Sub

    Private Sub SetLoginView(Status As TK.Domain.Enums.LoginStatus)
        Select Case Status
            Case TK.Domain.Enums.LoginStatus.normal
                Me.PLHlogin.Visible = True
                Me.PLHtoken.Visible = False
                ShowButton(True, False, False, False, False)
            Case TK.Domain.Enums.LoginStatus.registration
                Me.PLHlogin.Visible = True
                Me.PLHtoken.Visible = True
                ShowButton(True, False, False, False, False)
            Case TK.Domain.Enums.LoginStatus.token
                Me.PLHlogin.Visible = False
                Me.PLHtoken.Visible = True
                ShowButton(False, False, False, True, False)
        End Select
    End Sub

    Private Function GetCurrentUser() As TK.Domain.DTO.DTO_User

        Dim Usr As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User

        Try
            Usr = DirectCast(Session(TicketHelper.SessionExtUser), lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User)
        Catch ex As Exception

        End Try

        Return Usr
    End Function

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)

    Private Sub LNBtoLogin_Click(sender As Object, e As System.EventArgs) Handles LNBtoLogin.Click
        Me.ShowView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.AccessView.login)

        If Me.ShowLoginToken AndAlso String.IsNullOrEmpty(Me.UrlToken) Then
            Me.SetLoginView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.LoginStatus.registration)
        End If

    End Sub

    Private Sub LNBtoCreate_Click(sender As Object, e As System.EventArgs) Handles LNBtoCreate.Click

        Me.ShowView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.AccessView.register)

    End Sub

    Private Sub LNBtoRecover_Click(sender As Object, e As System.EventArgs) Handles LNBtoRecover.Click
        Me.ShowView(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.AccessView.recover)
    End Sub

    Private Sub BTNenter_Click(sender As Object, e As System.EventArgs) Handles BTNenter.Click

        Dim HasError As Boolean = False

        If String.IsNullOrEmpty(TXBloginmail.Text) Then
            Me.SetHint(LTloginmailHint, Resource.getValue("Error.RequiredField"))
            SetDivError(Me.DIVloginmail, True)
            HasError = True
        End If

        If String.IsNullOrEmpty(TXBlogincode.Text) Then
            Me.SetHint(LTlogincodeHint, Resource.getValue("Error.RequiredField"))
            SetDivError(Me.DIVlogincode, True)
            HasError = True
        End If

        If Not HasError Then
            CurrentPresenter.Enter(Me.TXBloginmail.Text, Me.TXBlogincode.Text, Me.TXBloginToken.Text)
        End If
    End Sub

    Private Sub BTNcreate_Click(sender As Object, e As System.EventArgs) Handles BTNcreate.Click
        Dim HasError As Boolean = False
        HideErrors()

        If String.IsNullOrEmpty(TXBregMail.Text) Then
            HasError = True
        End If

        If String.IsNullOrEmpty(TXBregname.Text) Then
            HasError = True
        End If

        If String.IsNullOrEmpty(TXBregSname.Text) Then
            HasError = True
        End If

        Dim pwderr As PwdValidationError = Me.CheckPwd()

        If pwderr = PwdValidationError.empty Then
            HasError = True
        ElseIf pwderr = PwdValidationError.invalidformat Then
            Me.SetHint(LTregPwd, Resource.getValue("Error.PasswordFormat"))
            SetDivError(Me.DIVregPassword, True)
            HasError = True
        ElseIf pwderr = PwdValidationError.notEqual Then
            Me.SetHint(LTregPwd, Resource.getValue("Error.PasswordNotEqual"))
            SetDivError(Me.DIVregPassword, True)
            Me.SetHint(LTregPwd2, Resource.getValue("Error.PasswordNotEqual"))
            SetDivError(Me.DIVregPassword2, True)
            HasError = True
        End If

        Me.RCPcaptcha.Validate()

        If Not Me.RCPcaptcha.IsValid Then
            Me.SetHint(LTcaptcha, Resource.getValue("Error.Captcha"))
            SetDivError(Me.DIVcaptcha, True)
            HasError = True
        End If

        If Not HasError Then
            Me.CurrentPresenter.Register(TXBregMail.Text, Me.TXBregname.Text, Me.TXBregSname.Text, Me.GetLanguageCode, Me.TXBregPwd.Text)
        End If
    End Sub

    Private Sub BTNrecover_Click(sender As Object, e As System.EventArgs) Handles BTNrecover.Click
        Dim HasError As Boolean = False
        HideErrors()

        Me.RCPcaptcha.Validate()

        If Not Me.RCPcaptcha.IsValid Then
            Me.SetHint(LTcaptcha, Resource.getValue("Error.Captcha"))
            SetDivError(Me.DIVcaptcha, True)
        End If

        Me.CurrentPresenter.Recover(Me.TXBrecMail.Text)

    End Sub


    Private Sub CTRLlang_UpdateInternationalization(oLingua As Comol.Entity.Lingua) Handles CTRLlang.UpdateInternationalization
        MyBase.UpdateLanguage(oLingua)

        SetTitleDescription()

    End Sub

    Private Sub BTNvalidate_Click(sender As Object, e As System.EventArgs) Handles BTNvalidate.Click
        Dim usr As TK.Domain.DTO.DTO_User = GetCurrentUser()

        Me.CurrentPresenter.TokenValidate(Me.TXBloginToken.Text, usr.UserId)
    End Sub

    Private Sub SetMandatory()
        LTmandatory_t.Text = Resource.getValue("Mandatory.Description")

        If String.IsNullOrEmpty(LTmandatory_t.Text) Then
            LTmandatory_t.Text = "Marked fields {0} are mandatory"
        End If

        Dim mt As String = LTmandatoryTemplate.Text

        Me.LTmandatory_t.Text = LTmandatory_t.Text.Replace("{0}", mt)

        LBloginmail_t.Text &= mt
        LBlogincode_t.Text &= mt
        LBregmail_t.Text &= mt
        LBregname_t.Text &= mt
        LBregSname_t.Text &= mt
        LBregPwd.Text &= mt
        LBregPwd2.Text &= mt
        LBcaptcha_t.Text &= mt
        LBrecMail_t.Text &= mt

    End Sub

#End Region


    'Public Shared Function CssVersion() As String
    '    Return TicketBase.CssVersion()
    'End Function

    Private _CssVerison As String = ""

    Public Function CssVersion() As String

        If String.IsNullOrEmpty(_CssVerison) Then
            Dim tkVerUC As UC_TicketCssVersion = LoadControl(BaseUrl & "/Modules/Ticket/UC/UC_TicketCssVersion.ascx")
            _CssVerison = TkVerUC.GetVersionString()
        End If

        Return _CssVerison
        'Return "?v=201507080935lm"
    End Function

End Class