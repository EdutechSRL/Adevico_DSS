Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports COL_Questionario.Business

Public Class InvitedUsersPresenter
    Inherits lm.Comol.Core.DomainModel.Common.DomainPresenter
#Region "Initialize"
    Private _ModuleID As Integer
    Private _Service As ServiceQuestionnaire
    'private int ModuleID
    '{
    '    get
    '    {
    '        if (_ModuleID <= 0)
    '        {
    '            _ModuleID = this.Service.ServiceModuleID();
    '        }
    '        return _ModuleID;
    '    }
    '}
    Public Overridable Property CurrentManager() As BaseModuleManager
        Get
            Return m_CurrentManager
        End Get
        Set(value As BaseModuleManager)
            m_CurrentManager = value
        End Set
    End Property
    Private m_CurrentManager As BaseModuleManager
    Protected Overridable ReadOnly Property View() As IViewInvitedUsers
        Get
            Return DirectCast(MyBase.View, IViewInvitedUsers)
        End Get
    End Property
    Private ReadOnly Property Service() As ServiceQuestionnaire
        Get
            If _Service Is Nothing Then
                _Service = New ServiceQuestionnaire(AppContext)
            End If
            Return _Service
        End Get
    End Property
    Public Sub New(oContext As iApplicationContext)
        MyBase.New(oContext)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
    Public Sub New(oContext As iApplicationContext, view As IViewInvitedUsers)
        MyBase.New(oContext, view)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
#End Region

    Public Sub InitView()
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout()
        Else
            Dim idQuest As Integer = View.PreloadedIdQuestionnnaire
            Dim idSessionQuest As Integer = View.SessionIdQuestionnnaire
            Dim quest As Questionario = Nothing
            Dim hasPermission As Boolean = View.HasPermission
            If idQuest = 0 Then
                idQuest = idSessionQuest
            ElseIf idSessionQuest <> idQuest Then
                quest = View.LoadQuestionnaire(idQuest)
                Dim group As QuestionarioGruppo = DALQuestionarioGruppo.readGruppoBYID(quest.idGruppo)
                If quest.ownerType = COL_BusinessLogic_v2.OwnerType_enum.None Then

                End If
            End If
            If hasPermission AndAlso idQuest > 0 Then
                View.CurrentIdQuestionnnaire = idQuest
            Else
                View.DisplayNoPermission()
            End If
        End If
    End Sub

    Function LoadTemplates() As Dictionary(Of Integer, String)
        Return Service.LoadTemplateNames(UserContext.CurrentUserID)
    End Function
    Sub LoadTemplate()
        Dim template As LazyTemplate = Service.GetTemplate(View.SelectedIdTemplate)
        If IsNothing(template) Then
            template = CreateTemplate()
        End If
        BaseLoadTemplate(template)
    End Sub

    Sub NewTemplate()
        Dim template As LazyTemplate = CreateTemplate()
        BaseLoadTemplate(template)
    End Sub
    Private Function CreateTemplate() As LazyTemplate
        Dim template As New LazyTemplate
        With template
            .Body = ""
            .IdLanguage = UserContext.Language.Id
            .Link = ""
            .Name = "--"
            .Subject = ""
            .TemplateType = 5
            .IdPerson = UserContext.CurrentUserID
            .MailSettings.CopyToSender = False
            .MailSettings.NotifyToSender = False
            .MailSettings.IsBodyHtml = True
            .MailSettings.NotifyToSender = False
            .MailSettings.SenderType = lm.Comol.Core.MailCommons.Domain.SenderUserType.LoggedUser
            .MailSettings.PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration
        End With
        Return template
    End Function
    Private Sub BaseLoadTemplate(template As LazyTemplate)
        Dim senderEdit As Boolean, subjectEdit As Boolean
        Dim userType As Integer = UserContext.UserTypeID
        senderEdit = (userType = UserTypeStandard.SysAdmin OrElse userType = UserTypeStandard.Administrator)
        subjectEdit = (userType = UserTypeStandard.SysAdmin OrElse userType = UserTypeStandard.Administrator OrElse userType = UserTypeStandard.Administrative)
        Dim dto As New lm.Comol.Core.Mail.dtoMailContent
        dto.Settings = Template.MailSettings
        dto.Body = Template.Body
        dto.Subject = Template.Subject
        View.LoadTemplate(Template.Name, dto, senderEdit, subjectEdit)
    End Sub
    Public Sub DeleteTemplate(idTemplate As Integer)
        Service.DeleteTemplate(idTemplate)
        View.LoadTemplates(Service.LoadTemplateNames(UserContext.CurrentUserID))
        LoadTemplate()
    End Sub

    Public Sub SaveTemplate(template As LazyTemplate)
        If template.Id = 0 Then
            template.IdPerson = UserContext.CurrentUserID
            template.IdLanguage = UserContext.Language.Id
        End If
        If template.Id > 0 OrElse template.IdPerson <> 0 Then
            Dim created As LazyTemplate = Service.SaveTemplate(template)
            If IsNothing(created) Then
            Else
                View.LoadTemplates(Service.LoadTemplateNames(UserContext.CurrentUserID))
                View.SelectedIdTemplate = created.Id
            End If
        End If

    End Sub

    Public Sub SendMail(content As lm.Comol.Core.Mail.dtoMailContent, invitedUsers As List(Of UtenteInvitato), sendToSender As Boolean, beginMessage As String)
        Dim mailService As New lm.Comol.Core.Mail.MailService(View.CurrentSmtpConfig, content.Settings)
        Dim notSentMail As String = ""
        Dim sentMail As String = ""
        Dim currentUser As lm.Comol.Core.DomainModel.Person = CurrentManager.GetPerson(UserContext.CurrentUserID)
        Dim idUserLanguage As Integer = 0
        Dim idPersonLanguage As Integer = 0
        If Not IsNothing(currentUser) Then
            idUserLanguage = currentUser.LanguageID
        End If
        Dim language As lm.Comol.Core.DomainModel.Language = CurrentManager.GetDefaultLanguage()
        Dim message As lm.Comol.Core.Mail.dtoMailMessage
        For Each user As UtenteInvitato In invitedUsers
            message = New lm.Comol.Core.Mail.dtoMailMessage(View.AnalyzeContent(content.Subject, user), View.AnalyzeContent(content.Body, user))
            message.FromUser = New System.Net.Mail.MailAddress(currentUser.Mail, currentUser.SurnameAndName)
            message.To.Add(New System.Net.Mail.MailAddress(user.Mail))

            idPersonLanguage = idUserLanguage
            If user.PersonaID > 0 Then
                Dim iUsers As lm.Comol.Core.DomainModel.Person = CurrentManager.GetPerson(user.PersonaID)
                If Not IsNothing(iUsers) Then
                    idPersonLanguage = iUsers.LanguageID
                End If
            End If
            If mailService.SendMail(idPersonLanguage, language, message) = lm.Comol.Core.Mail.MailException.MailSent Then
                Service.UpdateMailSent(user.ID)
                sentMail &= user.Cognome & " " & user.Nome & " (" & user.Mail & ")"
            Else
                notSentMail &= user.Cognome & " " & user.Nome & " (" & user.Mail & ")"
            End If
        Next
        If sendToSender Then
            message = New lm.Comol.Core.Mail.dtoMailMessage(content.Subject, content.Body)
            message.FromUser = New System.Net.Mail.MailAddress(currentUser.Mail, currentUser.SurnameAndName)
            message.To.Add(New System.Net.Mail.MailAddress(currentUser.Mail))
            message.Body = beginMessage & message.Body
            If Not String.IsNullOrEmpty(sentMail) Then
                message.Body &= vbCrLf & vbCrLf & View.MSGSendedMail & vbCrLf & sentMail

            End If
            If Not String.IsNullOrEmpty(notSentMail) Then
                message.Body &= vbCrLf & vbCrLf & View.MSGUnSendedMail & vbCrLf & notSentMail
            End If
            mailService.SendMail(idUserLanguage, language, message)
        End If
    End Sub
End Class