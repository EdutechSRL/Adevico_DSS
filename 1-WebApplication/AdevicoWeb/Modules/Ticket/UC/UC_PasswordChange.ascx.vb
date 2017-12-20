Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_PasswordChange
    Inherits BaseControl
    Implements TK.Presentation.View.iViewUcPassword

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.UcPasswordPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.UcPasswordPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.UcPasswordPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

    Public Event ActionMessage(ByVal Messages As List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CTRLactionMsg.Visible = False
    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UserSettings", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBoldPwd_t)
            .setLabel(LBnewPwd_t)
            .setLabel(LBrenewPwd_t)
            .setLinkButton(LNBchangePwd, True, True, False, True)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property



    Private Sub LNBchangePwd_Click(sender As Object, e As System.EventArgs) Handles LNBchangePwd.Click

        Dim HasFields As Boolean = True

        If String.IsNullOrEmpty(Me.TXBoldPwd.Text) Then
            HasFields = False
        End If

        If String.IsNullOrEmpty(Me.TXBnewPwd.Text) Then
            HasFields = False
        End If

        If String.IsNullOrEmpty(Me.TXBrenewPwd.Text) Then
            HasFields = False
        End If

        If Not HasFields Then
            ShowResponse(TK.Domain.Enums.ExternalUserPasswordErrors.VoidField)
            Return
        End If

        If Not TXBnewPwd.Text = TXBrenewPwd.Text Then
            ShowResponse(TK.Domain.Enums.ExternalUserPasswordErrors.PasswordNotMatch)
            Return
        End If


        CurrentPresenter.ChangePassword()
    End Sub

    Public Sub DisplaySessionTimeout(CommunityId As Integer) Implements TK.Presentation.View.iViewBase.DisplaySessionTimeout

    End Sub

    Public Sub SendUserActions(ModuleId As Integer, Action As TK.ModuleTicket.ActionType, idCommunity As Integer, Type As TK.ModuleTicket.InteractionType, Optional Objects As System.Collections.Generic.IList(Of System.Collections.Generic.KeyValuePair(Of Integer, String)) = Nothing) Implements TK.Presentation.View.iViewBase.SendUserActions

    End Sub

    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewBase.ViewCommunityId
        Get
            Return -1
        End Get
        Set(value As Integer)

        End Set
    End Property

    Public ReadOnly Property CurrentUser As TK.Domain.DTO.DTO_User Implements TK.Presentation.View.iViewUcPassword.CurrentUser
        Get
            Dim Usr As New TK.Domain.DTO.DTO_User

            Try
                Usr = DirectCast(Session(TicketHelper.SessionExtUser), TK.Domain.DTO.DTO_User)
            Catch ex As Exception

            End Try

            Return Usr
        End Get
    End Property


    Public ReadOnly Property NotificationSettings As TK.Domain.DTO.DTO_NotificationSettings Implements TK.Presentation.View.iViewUcPassword.NotificationSettings
        Get
            Dim sets As New TK.Domain.DTO.DTO_NotificationSettings()
            With sets
                .BaseUrl = ApplicationUrlBase
                .CategoriesTemplate = ""
                .DateTimeFormat = TicketHelper.DateTimeFormat
                .AvailableCategoryTypes = Nothing
                .AvailableTicketStatus = Nothing
                .LangCode = CurrentUser.LanguageCode    'Override nel service
                .SmtpConfig = PageUtility.CurrentSmtpConfig
            End With

            Return sets
        End Get
    End Property


#Region "From PAGEBASE"
    Private ReadOnly Property ApplicationUrlBase
        Get
            Dim Redirect As String = "http"

            If RequireSSL Then  'AndAlso Not WithoutSSLfromConfig => andalso not false = and true
                Redirect &= "s://" & Me.Request.Url.Host & Me.BaseUrl
            Else
                Redirect &= "://" & Me.Request.Url.Host & Me.BaseUrl
            End If
            ApplicationUrlBase = Redirect
        End Get
    End Property

    Public ReadOnly Property RequireSSL() As Boolean
        Get
            Dim RichiediSSL As Boolean = False
            Try
                RequireSSL = SystemSettings.Login.isSSLrequired
            Catch ex As Exception
                RequireSSL = False
            End Try
        End Get
    End Property
#End Region

    Public ReadOnly Property Password As String Implements TK.Presentation.View.iViewUcPassword.Password
        Get
            Return TXBoldPwd.Text
        End Get
    End Property

    Public ReadOnly Property RetypePassword As String Implements TK.Presentation.View.iViewUcPassword.RetypedPassword
        Get
            Return TXBrenewPwd.Text
        End Get
    End Property


    Public ReadOnly Property NewPassword As String Implements TK.Presentation.View.iViewUcPassword.NewPassword
        Get
            Return TXBnewPwd.Text
        End Get
    End Property

    Public Sub ShowResponse([error] As TK.Domain.Enums.ExternalUserPasswordErrors) Implements TK.Presentation.View.iViewUcPassword.ShowResponse

        Dim DTOmsgs As New List(Of lm.Comol.Core.DomainModel.Helpers.dtoMessage)
        Dim DTOmsg As New lm.Comol.Core.DomainModel.Helpers.dtoMessage

        DTOmsg.Text = Resource.getValue("PasswordError." & [error].ToString())

        Select Case [error]
            Case TK.Domain.Enums.ExternalUserPasswordErrors.GenericError
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case TK.Domain.Enums.ExternalUserPasswordErrors.InvalidPassword
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case TK.Domain.Enums.ExternalUserPasswordErrors.MailSendError
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case TK.Domain.Enums.ExternalUserPasswordErrors.none
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case TK.Domain.Enums.ExternalUserPasswordErrors.PasswordNotMatch
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case TK.Domain.Enums.ExternalUserPasswordErrors.SessionTimeout
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.info
            Case TK.Domain.Enums.ExternalUserPasswordErrors.UserNotFound
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case TK.Domain.Enums.ExternalUserPasswordErrors.VoidField
                DTOmsg.Type = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End Select

        DTOmsgs.Add(DTOmsg)

        If (ShowMessages) Then
            Me.CTRLactionMsg.Visible = True
            Me.CTRLactionMsg.InitializeControl(DTOmsgs)
        End If

        If (SendActionMessage) Then
            RaiseEvent ActionMessage(DTOmsgs)
        End If
    End Sub

    Public Property ShowMessages As Boolean
        Get
            Try
                Return System.Convert.ToBoolean(Me.ViewState("ShowMessages"))
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.CTRLactionMsg.Visible = value
            Me.ViewState("ShowMessages") = value
        End Set
    End Property

    Public Property SendActionMessage As Boolean
        Get
            Try
                Return System.Convert.ToBoolean(Me.ViewState("SendActionMessage"))
            Catch ex As Exception
                Return True
            End Try
        End Get
        Set(value As Boolean)
            Me.ViewState("SendActionMessage") = value
        End Set
    End Property

    Public Sub ShowNoAccess() Implements TK.Presentation.View.iViewBase.ShowNoAccess

    End Sub

    Public Sub SendNotification(Action As lm.Comol.Core.Notification.Domain.NotificationAction, CurrentPersonId As Integer) Implements TK.Presentation.View.iViewBase.SendNotification

        Dim service As SrvNotifications.iNotificationsManagerService = Nothing

        Try
            service = New SrvNotifications.iNotificationsManagerServiceClient()


            service.NotifyActionToModule(SystemSettings.NotificationErrorService.ComolUniqueID, Action, CurrentPersonId, PageUtility.CurrentContext.UserContext.IpAddress, PageUtility.CurrentContext.UserContext.ProxyIpAddress)


        Catch ex As Exception
            If Not IsNothing(service) Then
                Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
                iService.Abort()
                iService = Nothing
            End If
        Finally
            If Not IsNothing(service) Then
                Dim iService As SrvNotifications.iNotificationsManagerServiceClient = DirectCast(service, SrvNotifications.iNotificationsManagerServiceClient)
                iService.Close()
                iService = Nothing
            End If
        End Try

    End Sub

    'Public Sub SendNotifications(Actions As IList(Of lm.Comol.Core.Notification.Domain.NotificationAction), CurrentPersonId As Integer) Implements TK.Presentation.View.iViewBase.SendNotifications

    'End Sub
End Class