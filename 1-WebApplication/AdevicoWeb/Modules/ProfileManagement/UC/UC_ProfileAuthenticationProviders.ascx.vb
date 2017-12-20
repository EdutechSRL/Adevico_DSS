Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.BaseModules.ProviderManagement

Public Class UC_ProfileAuthenticationProviders
    Inherits BaseControl
    Implements IViewManageProfileAuthenticationProviders

    'Public Event UpdateContainer()
    Public Event CloseContainer()
    Public Event CloseContainerAndReload()
    Public Event DisplayAuthenticationsList()
    Public Event DisplayAuthenticationEdit()

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As ManageProfileAuthenticationProvidersPresenter
    Private ReadOnly Property CurrentPresenter() As ManageProfileAuthenticationProvidersPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ManageProfileAuthenticationProvidersPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property idProfile As Integer Implements IViewManageProfileAuthenticationProviders.idProfile
        Get
            Return ViewStateOrDefault("idProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("idProfile") = value
        End Set
    End Property
    Private Property AllowAddprovider As Boolean Implements IViewManageProfileAuthenticationProviders.AllowAddprovider
        Get
            Return ViewStateOrDefault("AllowAddprovider", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAddprovider") = value
            Me.BTNaddNewProvider.Visible = value
        End Set
    End Property
    Private Property AllowEdit As Boolean Implements IViewManageProfileAuthenticationProviders.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
        End Set
    End Property
    Private Property AllowRenewPassword As Boolean Implements IViewManageProfileAuthenticationProviders.AllowRenewPassword
        Get
            Return ViewStateOrDefault("AllowRenewPassword", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowRenewPassword") = value
        End Set
    End Property
    Private Property AllowSetPassword As Boolean Implements IViewManageProfileAuthenticationProviders.AllowSetPassword
        Get
            Return ViewStateOrDefault("AllowSetPassword", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSetPassword") = value
        End Set
    End Property

    Public Property isAjaxPanel As Boolean Implements IViewManageProfileAuthenticationProviders.isAjaxPanel
        Get
            Return ViewStateOrDefault("isAjaxPanel", False)
        End Get
        Set(value As Boolean)
            ViewState("isAjaxPanel") = value
        End Set
    End Property
    Public Property CurrentIdLoginInfo As Long Implements IViewManageProfileAuthenticationProviders.CurrentIdLoginInfo
        Get
            Return ViewStateOrDefault("CurrentIdLoginInfo", CLng(0))
        End Get
        Set(value As Long)
            ViewState("CurrentIdLoginInfo") = value
        End Set
    End Property
    Public Property idDefaultProvider As Long Implements IViewManageProfileAuthenticationProviders.idDefaultProvider
        Get
            Return ViewStateOrDefault("idDefaultProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("idDefaultProvider") = value
        End Set
    End Property
    Public Property CurrentIdProvider As Long Implements IViewManageProfileAuthenticationProviders.CurrentIdProvider
        Get
            Return ViewStateOrDefault("CurrentIdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("CurrentIdProvider") = value
        End Set
    End Property
    Public Property AvailableProvidersCount As Long Implements IViewManageProfileAuthenticationProviders.AvailableProvidersCount
        Get
            Return ViewStateOrDefault("AvailableProvidersCount", CLng(0))
        End Get
        Set(value As Long)
            ViewState("AvailableProvidersCount") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WizardInternalProfile", "Authentication")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBunknownUser)
            .setButton(BTNback, True, , , True)
            .setLabel(LBmessage)
            .setLabel(LBunknownUser)

            .setButton(BTNcloseModal, True)
            .setButton(BTNaddNewProvider, True)


            .setLabel(LBlogin_t)
            .setButton(BTNundoSaveLogin, True)
            '.setButton(BTNsetPassword, True)
            .setButton(BTNnewPassword, True)
            .setButton(BTNsaveLogin, True)
            .setLabel(LBexternalDuplicateLong)

            .setLabel(LBexternalDuplicateString)
            .setButton(BTNundoExternal, True)
            .setButton(BTNsaveExternal, True)
            .setLabel(LBloginDuplicate)
            LBaddExternalDuplicateLong.Text = LBexternalDuplicateLong.Text
            LBaddExternalDuplicateString.Text = LBexternalDuplicateString.Text
            LBaddLogin_t.Text = LBlogin_t.Text
            LBaddLoginDuplicate.Text = LBloginDuplicate.Text
            .setButton(BTNundoAdd, True)
            .setButton(BTNadd, True)

            .setLabel(LBpasswordEdit_t)
            .setLabel(LBpasswordEditConfirm_t)
            LBloginPasswordEdit_t.Text = LBlogin_t.Text
            .setButton(BTNundoSetPassword, True)
            .setButton(BTNsaveNewPassword, True)
        End With
    End Sub
#End Region

#Region "Implements"

    Public Sub InitializeControlForAdd(idProfile As Integer, isAjax As Boolean) Implements IViewManageProfileAuthenticationProviders.InitializeControlForAdd
        isAjaxPanel = isAjax
        LBprofileTitle.Visible = isAjax
        Me.LBaddExternalLong_t.Visible = False
        Me.LBaddExternalString_t.Visible = False
        Me.LBaddLoginDuplicate.Visible = False
        CurrentPresenter.InitView(idProfile, True)
    End Sub
    Public Sub InitializeControl(idProfile As Integer, isAjax As Boolean) Implements IViewManageProfileAuthenticationProviders.InitializeControl
        isAjaxPanel = isAjax
        DVmenu.Visible = isAjax
        LBprofileTitle.Visible = isAjax
        CurrentPresenter.InitView(idProfile, False)
    End Sub
    Public Sub LoadItems(providers As List(Of dtoUserProvider)) Implements IViewManageProfileAuthenticationProviders.LoadItems
        Me.RPTaccountInfo.DataSource = providers
        Me.RPTaccountInfo.DataBind()
        Me.MLVaccountInfo.SetActiveView(VIWinfo)
        If isAjaxPanel Then
            'RaiseEvent UpdateContainer()
        Else
            RaiseEvent DisplayAuthenticationsList()
        End If
    End Sub

#Region "Messages"
    Public Sub DisplayError(message As lm.Comol.Core.Authentication.ProfilerError) Implements IViewManageProfileAuthenticationProviders.DisplayError
        Me.MLVaccountInfo.SetActiveView(VIWmessage)
        Me.LBmessage.Text = Resource.getValue("ProfilerError." & message.ToString)
        'If isAjaxPanel Then
        '    RaiseEvent UpdateContainer()
        'End If
    End Sub
    Public Sub DisplayNoPermission() Implements IViewManageProfileAuthenticationProviders.DisplayNoPermission
        Me.MLVaccountInfo.SetActiveView(VIWmessage)
        Me.LBmessage.Text = Resource.getValue("DisplayNoPermission")
        'If isAjaxPanel Then
        '    RaiseEvent UpdateContainer()
        'End If
    End Sub
    Public Sub DisplayProfileUnknown() Implements IViewManageProfileAuthenticationProviders.DisplayProfileUnknown
        Me.MLVaccountInfo.SetActiveView(VIWunknownUser)
        'If isAjaxPanel Then
        '    RaiseEvent UpdateContainer()
        'End If
    End Sub
    Public Sub DisplayWorkinkSessionTimeout() Implements IViewManageProfileAuthenticationProviders.DisplayWorkinkSessionTimeout
        Me.MLVaccountInfo.SetActiveView(VIWmessage)
        Me.LBmessage.Text = Resource.getValue("DisplayWorkinkSessionTimeout")
        'If isAjaxPanel Then
        '    RaiseEvent UpdateContainer()
        'End If
    End Sub
#End Region

    Public Sub DisplayProfilerExternalError(message As lm.Comol.Core.Authentication.ProfilerError) Implements IViewManageProfileAuthenticationProviders.DisplayProfilerExternalError
        If MLVaccountInfo.GetActiveView Is VIWadd Then
            Me.LBaddExternalLong_t.Visible = True
            Me.LBaddExternalString_t.Visible = True
        Else
            Me.LBexternalDuplicateLong.Visible = True
            Me.LBexternalDuplicateString.Visible = True
        End If
    End Sub
    Private Sub DisplayProfilerInternalError(message As lm.Comol.Core.Authentication.ProfilerError) Implements IViewManageProfileAuthenticationProviders.DisplayProfilerInternalError
        If MLVaccountInfo.GetActiveView Is VIWadd Then
            Me.LBaddExternalLong_t.Visible = True
            Me.LBaddExternalString_t.Visible = True
        Else
            Me.LBexternalDuplicateLong.Visible = True
            Me.LBexternalDuplicateString.Visible = True

        End If
    End Sub
    Private Sub SetTitle(displayName As String) Implements IViewManageProfileAuthenticationProviders.SetTitle
        Me.LBprofileTitle.Text = String.Format(Resource.getValue("ProfileProvidersTitle"), displayName)
    End Sub

    Public Sub AddUserProviders(availableProviders As List(Of dtoBaseProvider)) Implements IViewManageProfileAuthenticationProviders.AddUserProviders
        Me.CurrentIdLoginInfo = 0
        Me.DDLauthentication.Items.Clear()
        Me.DDLauthentication.Items.AddRange((From p As dtoBaseProvider In availableProviders Select New ListItem(p.Translation.Name, p.IdProvider)).ToArray())
        Me.DDLauthentication.Enabled = (availableProviders.Count > 1)

        Me.SPNaddExternalLong.Visible = False
        Me.SPNaddExternalString.Visible = False
        Me.SPNaddInternal.Visible = False
        Me.MLVaccountInfo.SetActiveView(VIWadd)


        If Me.DDLauthentication.Items.Count > 0 Then
            Dim provider As dtoBaseProvider = (From p As dtoBaseProvider In availableProviders Where p.IdProvider = CLng(Me.DDLauthentication.SelectedValue) Select p).FirstOrDefault()
            LoadProviderToAdd(provider)
            'Else
            '    If isAjaxPanel Then
            '        RaiseEvent UpdateContainer()
            '    End If
        End If
    End Sub
    Private Sub EditInternalUserInfo(idLoginInfo As Long, login As String) Implements IViewManageProfileAuthenticationProviders.EditInternalUserInfo
        Me.CurrentIdLoginInfo = idLoginInfo
        Me.TXBlogin.Text = login
        Me.LBloginDuplicate.Visible = False
        Me.MLVaccountInfo.SetActiveView(VIWinternal)
        If isAjaxPanel Then
            'RaiseEvent UpdateContainer()
        Else
            RaiseEvent DisplayAuthenticationEdit()
        End If
    End Sub
    Private Sub EditInternalUserPassword(idLoginInfo As Long, login As String, username As String, usermail As String) Implements IViewManageProfileAuthenticationProviders.EditInternalUserPassword
        Me.Resource.setLabel(LBpasswordEditDescription)
        Me.LBpasswordEditDescription.Text = String.Format(LBpasswordEditDescription.Text, username, usermail)
        Me.CurrentIdLoginInfo = idLoginInfo
        Me.LBloginPasswordEdit.Text = login
        Me.LBloginDuplicate.Visible = False
        Me.MLVaccountInfo.SetActiveView(VIWinternalPassword)
        Me.TXBpasswordEdit.Text = ""
        Me.TXBpasswordEditConfirm.Text = ""
        If isAjaxPanel Then
            'RaiseEvent UpdateContainer()
        Else
            RaiseEvent DisplayAuthenticationEdit()
        End If
    End Sub
    Public Sub EditExternalUserInfo(idLoginInfo As Long, provider As dtoUserProvider, credentials As lm.Comol.Core.Authentication.dtoExternalCredentials) Implements IViewManageProfileAuthenticationProviders.EditExternalUserInfo
        Me.CurrentIdLoginInfo = idLoginInfo
        Dim allowLong As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(provider.IdentifierFields, lm.Comol.Core.Authentication.IdentifierField.longField)
        Dim allowString As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(provider.IdentifierFields, lm.Comol.Core.Authentication.IdentifierField.stringField)
        Me.SPNexternalLong.Visible = allowLong
        Me.SPNexternalString.Visible = allowString

        Me.RFVexternalLong.Visible = allowLong
        Me.RFVexternalString.Visible = allowString
        Me.RNVexternalLong.Visible = allowLong

        Me.LBexternalLong_t.Text = provider.Translation.FieldLong
        Me.LBexternalString_t.Text = provider.Translation.FieldString
        Me.TXBexternalLong.Text = credentials.IdentifierLong
        Me.TXBexternalString.Text = credentials.IdentifierString

        Me.MLVaccountInfo.SetActiveView(VIWexternal)
        LBexternalDuplicateLong.Visible = False
        LBexternalDuplicateString.Visible = False

        If isAjaxPanel Then
            'RaiseEvent UpdateContainer()
        Else
            RaiseEvent DisplayAuthenticationEdit()
        End If

    End Sub
    Private Function SendMail(user As lm.Comol.Core.Authentication.InternalLoginInfo, password As String) As Boolean Implements IViewManageProfileAuthenticationProviders.SendMail
        Dim sent As Boolean = False
        Try
            Dim mailTranslated As MailLocalized = PageUtility.LocalizedMail(user.Person.LanguageID)
            Dim mail As New COL_E_Mail(mailTranslated)

            mail.Mittente = mailTranslated.SystemSender
            mail.IndirizziTO.Add(New MailAddress(user.Person.Mail))
            'mail.Oggetto = mailTranslated.NotificationMessages(NotificationMessage.NotificationType.Login).Subject
            'mail.Body = mailTranslated.NotificationMessages(NotificationMessage.NotificationType.Login).Message


            mail.Oggetto = Me.Resource.getValue("newPasswordmailSubject")
            mail.Body = String.Format(Me.Resource.getValue("newPasswordmailAdministratorBody"), user.Person.SurnameAndName, user.Login, password, Me.PageUtility.ApplicationUrlBase)

            mail.Body = mail.Body & vbCrLf & mailTranslated.SystemFirmaNotifica
            mail.Body = Replace(mail.Body, "<br>", vbCrLf)

            mail.InviaMail()
            sent = (mail.Errore = Errori_Db.None)
        Catch ex As Exception

        End Try
        Return sent
    End Function
    Public Sub LoadProviderToAdd(provider As dtoBaseProvider) Implements IViewManageProfileAuthenticationProviders.LoadProviderToAdd
        Me.SPNaddExternalLong.Visible = False
        Me.SPNaddExternalString.Visible = False
        Me.SPNaddInternal.Visible = False
        If Not IsNothing(provider) Then
            If provider.Type = lm.Comol.Core.Authentication.AuthenticationProviderType.Internal Then
                Me.SPNaddInternal.Visible = True
                Me.TXBaddLogin.Text = ""
            Else
                Dim allowLong As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(provider.IdentifierFields, lm.Comol.Core.Authentication.IdentifierField.longField)
                Dim allowString As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(provider.IdentifierFields, lm.Comol.Core.Authentication.IdentifierField.stringField)
                Me.SPNaddExternalLong.Visible = allowLong
                Me.SPNaddExternalString.Visible = allowString

                Me.RFVaddExternalLong.Visible = allowLong
                Me.RFVaddExternalString.Visible = allowString
                Me.RNVaddExternalLong.Visible = allowLong

                Me.LBaddExternalLong_t.Text = provider.Translation.FieldLong
                Me.LBaddExternalString_t.Text = provider.Translation.FieldString

                LBaddExternalString_t.Visible = allowString
                RFVaddExternalLong.Visible = allowLong
                Me.TXBaddExternalLong.Text = ""
                Me.TXBaddExternalString.Text = ""
            End If
        End If
        'If isAjaxPanel Then
        '    RaiseEvent UpdateContainer()
        'End If
    End Sub

#End Region

    Private Sub RPTaccountInfo_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTaccountInfo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoUserProvider = DirectCast(e.Item.DataItem, dtoUserProvider)

            Dim oLiteral As Literal = e.Item.FindControl("LTidLoginInfo")
            oLiteral.Text = dto.Id

            oLiteral = e.Item.FindControl("LTtype")
            oLiteral.Text = dto.Type.ToString

            Dim oLabel As Label = e.Item.FindControl("LBproviderName")

            oLabel.Text = dto.Translation.Name
            oLabel = e.Item.FindControl("LBproviderDescription")
            oLabel.Text = dto.Translation.Description

            oLabel = e.Item.FindControl("LBproviderInfo")
            Dim buttonPwd As Button = e.Item.FindControl("BTNnewPassword")
            Dim buttonEdit As Button = e.Item.FindControl("BTNeditProvider")
            Dim buttonDefault As Button = e.Item.FindControl("BTNsetDefault")
            Dim buttonDelete As Button = e.Item.FindControl("BTNdeleteProvider")
            Dim buttonEnable As Button = e.Item.FindControl("BTNenable")
            Dim buttonSetPwd As Button = e.Item.FindControl("BTNsetPassword")

            buttonDefault.Visible = AllowEdit AndAlso (idDefaultProvider <> dto.IdProvider)
            buttonDelete.Visible = AllowEdit AndAlso (idDefaultProvider <> dto.IdProvider)


            buttonEnable.Visible = AllowEdit AndAlso ((idDefaultProvider <> dto.IdProvider) OrElse Not dto.isEnabled)
            buttonEnable.CommandName = IIf(dto.isEnabled, "disable", "enable")
            Me.Resource.setButton(buttonEnable, True)
            Me.Resource.setButton(buttonPwd, True)
            Me.Resource.setButton(buttonSetPwd, True)
            Me.Resource.setButton(buttonEdit, True)
            Me.Resource.setButton(buttonDefault, True)
            Me.Resource.setButton(buttonDelete, True, False, True)
            Me.Resource.setButtonByValue(buttonEnable, IIf(dto.isEnabled, "disable", "enable"), True)


            Dim oLBidentifyer_t As Label = e.Item.FindControl("LBidentifyer_t")
            Dim oLBidentifyer As Label = e.Item.FindControl("LBidentifyer")

            Me.Resource.setLabel(oLBidentifyer_t)
            If TypeOf dto Is dtoInternalUserProvider Then
                Dim internal As dtoInternalUserProvider = DirectCast(dto, dtoInternalUserProvider)
                oLabel.Visible = (internal.ResetType <> lm.Comol.Core.Authentication.EditType.none)
                If internal.ResetType <> lm.Comol.Core.Authentication.EditType.none AndAlso internal.ModifiedOn.HasValue Then
                    Resource.setLabel(oLabel)
                    oLabel.Text = String.Format(oLabel.Text, internal.ModifiedOn.Value.ToString("dd/MM/yy"), internal.ModifiedOn.Value.ToString("HH:mm"), Resource.getValue("EditType." & internal.ResetType.ToString), internal.ModifiedBy.SurnameAndName)
                End If

                oLabel = e.Item.FindControl("LBpasswordExpiresOn")

                oLabel.Visible = internal.PasswordExpiresOn.HasValue
                If internal.PasswordExpiresOn.HasValue Then

                    Resource.setLabel(oLabel)
                    oLabel.Text = String.Format(oLabel.Text, internal.PasswordExpiresOn.Value.ToString("dd/MM/yy"), internal.PasswordExpiresOn.Value.ToString("HH:mm"))
                End If
                buttonSetPwd.Visible = Me.AllowSetPassword
                buttonPwd.Visible = Me.AllowRenewPassword
                buttonEdit.Visible = AllowEdit

                oLBidentifyer.Text = internal.Login
            ElseIf TypeOf dto Is dtoExternalUserProvider Then
                Dim external As dtoExternalUserProvider = DirectCast(dto, dtoExternalUserProvider)
                buttonPwd.Visible = False
                buttonEdit.Visible = AllowEdit

                Dim allowLong As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(external.IdentifierFields, lm.Comol.Core.Authentication.IdentifierField.longField)
                Dim allowString As Boolean = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(external.IdentifierFields, lm.Comol.Core.Authentication.IdentifierField.stringField)

                If allowLong AndAlso allowString Then
                    oLBidentifyer_t.ToolTip = external.Translation.FieldString & "/" & external.Translation.FieldLong
                    oLBidentifyer.Text = external.IdExternalString & "/" & external.IdExternalLong
                ElseIf allowLong Then
                    oLBidentifyer_t.ToolTip = external.Translation.FieldLong
                    oLBidentifyer.Text = external.IdExternalLong
                ElseIf allowString Then
                    oLBidentifyer_t.ToolTip = external.Translation.FieldString
                    oLBidentifyer.Text = external.IdExternalString
                End If
            End If
        End If
    End Sub

    Private Sub RPTaccountInfo_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTaccountInfo.ItemCommand
        Dim IdProvider As Long = 0
        If IsNumeric(e.CommandArgument) Then
            IdProvider = CLng(e.CommandArgument)
        End If
        If e.CommandName = "newPassword" Then
            CurrentPresenter.RenewPassword(idProfile)
        ElseIf e.CommandName = "setPassword" Then
            CurrentPresenter.SetPassword(IdProvider)
        ElseIf e.CommandName = "edit" Then
            CurrentPresenter.EditUserProviderInfo(IdProvider)
        ElseIf e.CommandName = "default" Then
            CurrentPresenter.SetDefaultProvider(IdProvider)
        ElseIf e.CommandName = "delete" Then
            CurrentPresenter.DeleteLoginInfo(IdProvider)
        ElseIf e.CommandName = "disable" Then
            CurrentPresenter.ModifyAuthenticationUse(idProfile, True)
        ElseIf e.CommandName = "enable" Then
            CurrentPresenter.ModifyAuthenticationUse(idProfile, False)
        End If
    End Sub

    Private Sub DDLauthentication_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLauthentication.SelectedIndexChanged
        CurrentPresenter.SelectNewUserProvider(Me.DDLauthentication.SelectedValue)
    End Sub

#Region "undo Buttons"
    Private Sub BTNcloseModal_Click(sender As Object, e As System.EventArgs) Handles BTNcloseModal.Click
        'If idDefaultProvider = 0 Then
        '    RaiseEvent CloseContainer()
        'Else
        RaiseEvent CloseContainerAndReload()
        'End If
    End Sub
    Private Sub BTNback_Click(sender As Object, e As System.EventArgs) Handles BTNback.Click
        Me.CurrentPresenter.LoadAuthenticationItems()
    End Sub
    Private Sub BTNundoExternal_Click(sender As Object, e As System.EventArgs) Handles BTNundoExternal.Click
        Me.MLVaccountInfo.SetActiveView(Me.VIWinfo)
        If isAjaxPanel Then
            'RaiseEvent UpdateContainer()
        Else
            RaiseEvent DisplayAuthenticationsList()
        End If
    End Sub
    Private Sub BTNundoSaveLogin_Click(sender As Object, e As System.EventArgs) Handles BTNundoSaveLogin.Click
        Me.MLVaccountInfo.SetActiveView(Me.VIWinfo)
        If isAjaxPanel Then
            'RaiseEvent UpdateContainer()
        Else
            RaiseEvent DisplayAuthenticationsList()
        End If
    End Sub
    Private Sub BTNundoAdd_Click(sender As Object, e As System.EventArgs) Handles BTNundoAdd.Click
        If idDefaultProvider = 0 Then
            If isAjaxPanel Then
                RaiseEvent CloseContainerAndReload()
            Else
                RaiseEvent DisplayAuthenticationsList()
            End If
        Else
            Me.CurrentPresenter.LoadAuthenticationItems()
            If Not isAjaxPanel Then
                RaiseEvent DisplayAuthenticationsList()
            End If
        End If
    End Sub
    Private Sub BTNundoSetPassword_Click(sender As Object, e As System.EventArgs) Handles BTNundoSetPassword.Click
        Me.MLVaccountInfo.SetActiveView(Me.VIWinfo)
        If isAjaxPanel Then
            'RaiseEvent UpdateContainer()
        Else
            RaiseEvent DisplayAuthenticationsList()
        End If
    End Sub
#End Region

    Private Sub BTNaddNewProvider_Click(sender As Object, e As System.EventArgs) Handles BTNaddNewProvider.Click
        Me.LBaddExternalDuplicateLong.Visible = False
        Me.LBaddExternalDuplicateString.Visible = False
        Me.LBaddLoginDuplicate.Visible = False
        Me.CurrentPresenter.AddNewLoginInfo()
    End Sub

    Private Sub BTNadd_Click(sender As Object, e As System.EventArgs) Handles BTNadd.Click
        If Me.SPNaddInternal.Visible Then
            If Not String.IsNullOrEmpty(Me.TXBaddLogin.Text) Then
                Me.CurrentPresenter.SaveInternalProvider(Me.TXBaddLogin.Text)
            End If
        Else
            Dim identifierLong As Long = 0
            If Not String.IsNullOrEmpty(TXBaddExternalLong.Text) AndAlso IsNumeric(TXBaddExternalLong.Text) Then
                identifierLong = CLng(Me.TXBaddExternalLong.Text)
            End If
            Me.CurrentPresenter.SaveExternalProvider(Me.DDLauthentication.SelectedValue, New lm.Comol.Core.Authentication.dtoExternalCredentials() With {.IdentifierString = TXBaddExternalString.Text, .IdentifierLong = identifierLong})
        End If
    End Sub


    'Private Sub BTNsetPassword_Click(sender As Object, e As System.EventArgs) Handles BTNsetPassword.Click
    '    Me.CurrentPresenter.RenewPassword(idProfile)
    'End Sub
    Private Sub BTNnewPassword_Click(sender As Object, e As System.EventArgs) Handles BTNnewPassword.Click
        Me.CurrentPresenter.RenewPassword(idProfile)
    End Sub

    Private Sub BTNsaveExternal_Click(sender As Object, e As System.EventArgs) Handles BTNsaveExternal.Click
        Dim identifierLong As Long = 0
        If Not String.IsNullOrEmpty(TXBexternalLong.Text) AndAlso IsNumeric(TXBexternalLong.Text) Then
            identifierLong = CLng(Me.TXBexternalLong.Text)
        End If

        Me.CurrentPresenter.SaveExternalProvider(CurrentIdProvider, New lm.Comol.Core.Authentication.dtoExternalCredentials() With {.IdentifierString = TXBexternalString.Text, .IdentifierLong = identifierLong})
    End Sub

    Private Sub BTNsaveLogin_Click(sender As Object, e As System.EventArgs) Handles BTNsaveLogin.Click
        Me.CurrentPresenter.SaveInternalProvider(Me.TXBlogin.Text)
    End Sub
    Private Sub BTNsaveNewPassword_Click(sender As Object, e As System.EventArgs) Handles BTNsaveNewPassword.Click
        Me.CurrentPresenter.SaveInternalPassword(Me.TXBpasswordEdit.Text, True)
    End Sub
    'Public Sub LoadProfileInfoError(errors As List(Of ProfilerError))
    '    Me.LBloginDuplicate.Visible = errors.Contains(ProfilerError.loginduplicate)
    '    Me.LBmailDuplicate.Visible = errors.Contains(ProfilerError.mailDuplicate)
    '    Me.LBtaxCodeDuplicate.Visible = errors.Contains(ProfilerError.taxCodeDuplicate) AndAlso Me.SystemSettings.Presenter.DefaultTaxCodeRequired
    '    If idProfileType = lm.Comol.Core.DomainModel.UserTypeStandard.UniversityTeacher Then
    '        Me.LBmatriculaDuplicate.Visible = errors.Contains(ProfilerError.uniqueIDduplicate)
    '    End If
    '    Me.LBexternalDuplicateLong.Visible = errors.Contains(ProfilerError.externalUniqueIDduplicate)
    '    Me.LBexternalDuplicateString.Visible = errors.Contains(ProfilerError.externalUniqueIDduplicate)
    'End Sub


    Public Sub DisplayAddView()
        Me.LBaddExternalDuplicateLong.Visible = False
        Me.LBaddExternalDuplicateString.Visible = False
        Me.LBaddLoginDuplicate.Visible = False
        Me.CurrentPresenter.AddNewLoginInfo()
    End Sub
End Class