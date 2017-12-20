Imports lm.Comol.Core.BaseModules.Tickets.Domain.Enums
Imports lm.Comol.Core.DomainModel.Helpers
Imports TK = lm.Comol.Core.BaseModules.Tickets


Public Class UC_TicketUserSettings
    Inherits BaseControl
    Implements TK.Presentation.View.iViewUcUserSettings

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.UcUserSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.UcUserSettingsPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.UcUserSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

    Public Event SessionTimeOut(CommunityId As Integer)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "BaseControl"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UserSettings", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLiteral(LTmyNotification_t)
            .setLiteral(LTmyNotificationDesc_t)
            .setLiteral(LTusrDeacription_t)
            .setLiteral(LTmanDescription_t)

            LBusrMailSet_t.Text = ""
            LBmanMailSet_t.Text = ""
            .setLabel(LBusrMailSet_t)
            .setLabel(LBmanMailSet_t)

            .setLinkButton(LKBgetUsrGlobalSettings, True, True, False, False)
            .setLinkButton(LKBgetManGlobalSettings, True, True, False, False)

            .setLinkButton(Me.LKBsaveSettings, True, True, False, False)

            '.setLabel(LBextraInfoUsr)
            '.setLabel(LBextraInfoMan)

            .setLiteral(LTtitleUser_t)
            .setLiteral(LTtitleManager_t)
        End With

        UCswitchUser.SetText(Me.Resource, True, True)
        UCswitchMan.SetText(Me.Resource, True, True)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region

#Region "UC Public"

    'TODO

#Region "Inizializzazione"

    ''' <summary>
    ''' Inizializza il controllo sull'utente corrente
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Bind()
        Me.CurrentPresenter.Initilize()
    End Sub

    ''' <summary>
    ''' Inizializza il controllo per una persona specifica (diversa dall'utente corrente)
    ''' </summary>
    ''' <param name="PersonId">ID Person</param>
    ''' <remarks></remarks>
    Public Sub BindPerson(ByVal PersonId As Integer)
        Me.CurrentPresenter.InitializePerson(PersonId)
    End Sub

    ''' <summary>
    ''' Inizializza il controllo su uno specifico Ticket User (diverso dall'utente corrente)
    ''' </summary>
    ''' <param name="UserId"></param>
    ''' <remarks></remarks>
    Public Sub BindUser(ByVal UserId As Int64)
        CurrentUserID = UserId
        Me.CurrentPresenter.InitializeUser(UserId)
    End Sub

#End Region

    ''Salva le impostazioni correnti
    Public Sub Save()
        Me.CurrentPresenter.Save()
    End Sub


#Region "Eventi"

    Public Event guiMessagesShow(ByVal Messages As IList(Of dtoMessage))
    Public Event guiMessageHide()

    'ByVal errorType As TK.Domain.Enums.ViewSettingsUserError


#End Region




#End Region


    'Public Sub test()

    '    Dim messages As New List(Of dtoMessage)


    '    Dim msg As New dtoMessage()
    '    msg.Type = MessageType.alert
    '    msg.Text = msg.Type.ToString()
    '    messages.Add(msg)

    '    msg = New dtoMessage()
    '    msg.Type = MessageType.error
    '    msg.Text = msg.Type.ToString()
    '    messages.Add(msg)

    '    msg = New dtoMessage()
    '    msg.Type = MessageType.info
    '    msg.Text = msg.Type.ToString()
    '    messages.Add(msg)

    '    msg = New dtoMessage()
    '    msg.Type = MessageType.none
    '    msg.Text = msg.Type.ToString()
    '    messages.Add(msg)

    '    msg = New dtoMessage()
    '    msg.Type = MessageType.success
    '    msg.Text = msg.Type.ToString()
    '    messages.Add(msg)

    '    RaiseEvent SendUserMessages(messages)

    'End Sub

#Region "View Implementation"
    Public Sub DisplaySessionTimeout(CommunityId As Integer) Implements TK.Presentation.View.iViewBase.DisplaySessionTimeout
        RaiseEvent SessionTimeOut(CommunityId)
    End Sub

    Public Sub SendNotification(Action As lm.Comol.Core.Notification.Domain.NotificationAction, CurrentPersonId As Integer) Implements TK.Presentation.View.iViewBase.SendNotification

    End Sub

    Public Sub SendUserActions(ModuleId As Integer, Action As TK.ModuleTicket.ActionType, idCommunity As Integer, Type As TK.ModuleTicket.InteractionType, Optional Objects As IList(Of KeyValuePair(Of Integer, String)) = Nothing) Implements TK.Presentation.View.iViewBase.SendUserActions

    End Sub

    Public Sub ShowNoAccess() Implements TK.Presentation.View.iViewBase.ShowNoAccess

    End Sub

    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewBase.ViewCommunityId

    Public Property settings As TK.Domain.DTO.DTO_UserSettings Implements TK.Presentation.View.iViewUcUserSettings.Settings
        Get

            Dim sets As New TK.Domain.DTO.DTO_UserSettings

            Dim userSet As TK.Domain.Enums.MailSettings = UCmailUsrSett.GetSettings()
            If System.Convert.ToInt64(userSet < 0) Then
                userSet = TK.Domain.Enums.MailSettings.none
                'sets.isUserNotificationOn = False
            End If

            Dim manSet As TK.Domain.Enums.MailSettings = UCmailManSett.GetSettings()
            If System.Convert.ToInt64(userSet < 0) Then
                manSet = TK.Domain.Enums.MailSettings.none
                'sets.isManagerNotificationOn = False
            End If

            sets.Settings = manSet Or userSet

            sets.isUserNotificationOn = Me.UCswitchUser.Status
            sets.isManagerNotificationOn = Me.UCswitchMan.Status

            Return sets

        End Get
        Set(value As TK.Domain.DTO.DTO_UserSettings)
            With value
                Me.UCmailUsrSett.BindSettings(False, False, value.Settings, ViewSettingsUser.Owner, True, True, value.isBehalfer)

                Me.PNLmanager.Visible = .isManager

                If .isManager Then
                    Me.UCmailManSett.BindSettings(False, False, value.Settings, ViewSettingsUser.Manager, True, True, False)
                Else
                    Me.UCmailManSett.BindSettings(False, False, TK.Domain.Enums.MailSettings.none, ViewSettingsUser.Manager, True, True, False)
                End If

                Me.UCswitchUser.Status = .isUserNotificationOn
                Me.UCswitchMan.Status = .isManagerNotificationOn

                Me.DVextraInfoUsr.Visible = Not .isUserSysNotificationOn
                If (DVextraInfoUsr.Visible) Then
                    Resource.setLabel(LBextraInfoUsr)
                End If

                Me.DVextraInfoMan.Visible = Not .isManagerSysNotificationOn
                If (DVextraInfoMan.Visible) Then
                    Resource.setLabel(LBextraInfoMan)
                End If



            End With

        End Set
    End Property

    Public Sub ShowError(setError As TK.Domain.Enums.ViewSettingsUserError) Implements TK.Presentation.View.iViewUcUserSettings.ShowError

        Dim Messages As IList(Of dtoMessage) = New List(Of dtoMessage)

        Select Case setError
            Case ViewSettingsUserError.none
                RaiseEvent guiMessageHide()

            Case ViewSettingsUserError.usernotfound
                Dim msg As New dtoMessage()
                msg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
                msg.Text = Resource.getValue(String.Format("SettingError.{0}", setError.ToString()))
                Messages.Add(msg)

            Case ViewSettingsUserError.internalError
                Dim msg As New dtoMessage()
                msg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.error
                msg.Text = Resource.getValue(String.Format("SettingError.{0}", setError.ToString()))
                Messages.Add(msg)

            Case ViewSettingsUserError.success
                Dim msg As New dtoMessage()
                msg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success
                msg.Text = Resource.getValue(String.Format("SettingError.{0}", setError.ToString()))
                Messages.Add(msg)

        End Select

        If Not IsNothing(Messages) AndAlso Messages.Any() Then
            RaiseEvent guiMessagesShow(Messages)
        End If

    End Sub



#End Region

    'Private Sub LKBtest_Click(sender As Object, e As EventArgs) Handles LKBtest.Click



    '    LKBtest.Text = String.Format("Test: {0}", System.Convert.ToInt64(Me.Settings.Settings).ToString())

    '    Dim settings As New TK.Domain.DTO.DTO_UserSettings()
    '    settings.isBehalfer = True
    '    settings.isManager = True
    '    settings.isManagerNotificationOn = True
    '    settings.isNotificationOn = True
    '    settings.isUserNotificationOn = True

    '    settings.Settings = TK.Domain.Enums.MailSettings.NewTicketUsr Or TK.Domain.Enums.MailSettings.NewTicketManager

    '    Me.Settings = settings


    'End Sub

    Public Property ShowSave As Boolean
        Get
            Return Me.LKBsaveSettings.Visible
        End Get
        Set(value As Boolean)
            Me.LKBsaveSettings.Visible = value
        End Set
    End Property



    Private Sub LKBsaveSettings_Click(sender As Object, e As EventArgs) Handles LKBsaveSettings.Click
        Me.CurrentPresenter.SaveUser(CurrentUserID)
    End Sub


    Private Property CurrentUserID As Int64
        Get
            Dim cUsrID = 0

            Try
                cUsrID = ViewStateOrDefault("CurrentTkUserID", 0)
            Catch ex As Exception

            End Try

            Return cUsrID
        End Get
        Set(value As Int64)
            ViewState("CurrentTkUserID") = value
        End Set
    End Property

    'Private Sub LKBgetUsrGlobalSettings_Click(sender As Object, e As EventArgs) Handles LKBgetUsrGlobalSettings.Click
    '    
    'End Sub

    'Private Sub LKBgetManGlobalSettings_Click(sender As Object, e As EventArgs) Handles LKBgetManGlobalSettings.Click
    '    
    'End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If String.IsNullOrEmpty(LBmanMailSet_t.Text) Then
            LBmanMailSet_t.Visible = False
        End If

        If String.IsNullOrEmpty(LBusrMailSet_t.Text) Then
            LBusrMailSet_t.Visible = False
        End If

    End Sub

    Private Sub UCswitchMan_StatusChange(Status As Boolean) Handles UCswitchMan.StatusChange
        Me.Save()
    End Sub
    
    Private Sub UCswitchUser_StatusChange(Status As Boolean) Handles UCswitchUser.StatusChange
        Me.Save()
    End Sub


End Class