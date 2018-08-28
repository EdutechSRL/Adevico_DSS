Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.Mail

Public Interface IViewInvitedUsers
    Inherits lm.Comol.Core.DomainModel.Common.iDomainView

    ReadOnly Property SessionIdQuestionnnaire As Integer
    ReadOnly Property PreloadedIdQuestionnnaire As Integer
    Property CurrentIdQuestionnnaire As Integer
    ReadOnly Property MailContent As lm.Comol.Core.Mail.dtoMailContent
    ReadOnly Property CurrentSmtpConfig As lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig
    ReadOnly Property MSGSendedMail As String
    ReadOnly Property MSGUnSendedMail As String

    Function LoadQuestionnaire(ByVal id As Integer) As Questionario
    Function AnalyzeContent(ByVal content As String, user As UtenteInvitato) As String

    Function HasPermission() As Boolean

    Property SelectedIdTemplate As Integer
    Sub LoadTemplatesList()
    Sub LoadTemplates(items As Dictionary(Of Integer, String))
    Sub LoadTemplate(ByVal name As String, ByVal dto As dtoMailContent, senderEdit As Boolean, subjectEdit As Boolean)

    Sub DisplaySessionTimeout()
    Sub DisplayNoPermission()
End Interface
