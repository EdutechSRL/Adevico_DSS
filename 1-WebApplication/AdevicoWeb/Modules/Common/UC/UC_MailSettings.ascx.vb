Imports lm.Comol.Core.BaseModules.MailSender.Presentation
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.Mail

Public Class UC_MailSettings
    Inherits BaseControl
    Implements IViewMailSettings

#Region "Context"
    Private _Presenter As MailSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As MailSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MailSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowEditSignature As Boolean Implements IViewMailSettings.AllowEditSignature
        Get
            Return ViewStateOrDefault("AllowEditSignature", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEditSignature") = value
            Me.DVsignature.Visible = value
        End Set
    End Property
    Public Property AllowNoSignature As Boolean Implements IViewMailSettings.AllowNoSignature
        Get
            Return ViewStateOrDefault("AllowNoSignature", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowNoSignature") = value
        End Set
    End Property
    Public Property AllowSignatureFromTemplate As Boolean Implements IViewMailSettings.AllowSignatureFromTemplate
        Get
            Return ViewStateOrDefault("AllowSignatureFromTemplate", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSignatureFromTemplate") = value
        End Set
    End Property
    Public Property AllowSignatureFromSkin As Boolean Implements IViewMailSettings.AllowSignatureFromSkin
        Get
            Return ViewStateOrDefault("AllowSignatureFromSkin", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSignatureFromSkin") = value
        End Set
    End Property
    Public Property AllowSignatureFromField As Boolean Implements IViewMailSettings.AllowSignatureFromField
        Get
            Return ViewStateOrDefault("AllowSignatureFromSkin", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSignatureFromSkin") = value
        End Set
    End Property
    Public Property AllowCopyToSender As Boolean Implements IViewMailSettings.AllowCopyToSender
        Get
            Return ViewStateOrDefault("AllowCopyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowCopyToSender") = value
            Me.CBcopyToSender.Enabled = value
            If Not value Then
                Me.CBcopyToSender.Checked = False
            End If
        End Set
    End Property
    Public Property AllowFormatSelection As Boolean Implements IViewMailSettings.AllowFormatSelection
        Get
            Return ViewStateOrDefault("AllowFormatSelection", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowFormatSelection") = value
            Me.RBhtml.Enabled = value
            Me.RBtext.Enabled = value
            DVmailType.Visible = value
        End Set
    End Property
    Public Property AllowNotifyToSender As Boolean Implements IViewMailSettings.AllowNotifyToSender
        Get
            Return ViewStateOrDefault("AllowNotifyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowNotifyToSender") = value
            Me.CBnotifyToSender.Enabled = value
            If Not value Then
                Me.CBnotifyToSender.Checked = False
            End If
        End Set
    End Property
    Private Property AllowSenderEdit As Boolean Implements IViewMailSettings.AllowSenderEdit
        Get
            Return ViewStateOrDefault("AllowSenderEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSenderEdit") = value
            Me.DVsender.Visible = value
        End Set
    End Property
    Private Property AllowSubjectEdit As Boolean Implements IViewMailSettings.AllowSubjectEdit
        Get
            Return ViewStateOrDefault("AllowSubjectEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSubjectEdit") = value
            Me.DVsubject.Visible = value
        End Set
    End Property
    Private Property CopyToSender As Boolean
        Get
            Return Me.CBcopyToSender.Checked
        End Get
        Set(value As Boolean)
            Me.CBcopyToSender.Checked = value
        End Set
    End Property
    Private Property NotifyToSender As Boolean
        Get
            Return Me.CBnotifyToSender.Checked
        End Get
        Set(value As Boolean)
            Me.CBnotifyToSender.Checked = value
        End Set
    End Property
    Public Property Settings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Implements IViewMailSettings.Settings
        Get
            Dim oDto As New lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings

            With oDto
                .IsBodyHtml = IsHTMLformat
                .CopyToSender = CopyToSender
                .NotifyToSender = NotifyToSender
                .SenderType = IIf(Me.RBsenderSystem.Checked, lm.Comol.Core.MailCommons.Domain.SenderUserType.System, lm.Comol.Core.MailCommons.Domain.SenderUserType.LoggedUser)
                .PrefixType = IIf(Me.RBsubjectSystem.Checked, lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration, lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.None)
                .Signature = GetSelectedSignature()
            End With
            Return oDto
        End Get
        Set(value As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings)
            With value
                IsHTMLformat = .IsBodyHtml
                CopyToSender = .CopyToSender
                NotifyToSender = .NotifyToSender
                Me.RBsenderSystem.Checked = (.SenderType = lm.Comol.Core.MailCommons.Domain.SenderUserType.System)
                Me.RBsenderUser.Checked = (.SenderType = lm.Comol.Core.MailCommons.Domain.SenderUserType.LoggedUser)
                Me.RBsubjectNone.Checked = (.PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.None)
                Me.RBsubjectSystem.Checked = (.PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration)
            End With
        End Set
    End Property
    Public Property DisplayCopyToSender As Boolean Implements IViewMailSettings.DisplayCopyToSender
        Get
            Return ViewStateOrDefault("DisplayCopyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCopyToSender") = value
            Me.DVcopyToSender.Visible = value
        End Set
    End Property
    Public Property DisplayNotifyToSender As Boolean Implements IViewMailSettings.DisplayNotifyToSender
        Get
            Return ViewStateOrDefault("DisplayNotifyToSender", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayNotifyToSender") = value
            Me.DVnotifyToSender.Visible = value
        End Set
    End Property

    Public Property IsInitialized As Boolean Implements IViewMailSettings.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
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

#Region "Internal"
    Private Property IsHTMLformat As Boolean
        Get
            Return Me.RBhtml.Checked
        End Get
        Set(value As Boolean)
            Me.RBhtml.Checked = value
            Me.RBtext.Checked = Not value
        End Set
    End Property
    Public Property RaiseUpdateEvent As Boolean
        Get
            Return ViewStateOrDefault("RaiseUpdateEvent", False)
        End Get
        Set(value As Boolean)
            ViewState("RaiseUpdateEvent") = value
            Me.RBtext.AutoPostBack = value
            Me.RBhtml.AutoPostBack = value
            Me.RBsubjectNone.AutoPostBack = value
            Me.RBsubjectSystem.AutoPostBack = value
        End Set
    End Property
    Public Event UpdateMailBody(ByVal isHtml As Boolean)
    Public Event UpdateMailSender(ByVal t As lm.Comol.Core.MailCommons.Domain.SenderUserType)
    Public Event UpdateMailSubject(ByVal s As lm.Comol.Core.MailCommons.Domain.SubjectPrefixType)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_MailEditor", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBmailType_t)
            .setLabel(LBmailbodyHTML)
            .setLabel(LBmailbodyTEXT)
            .setLabel(LBsender_t)

            .setLabel(LBsubjectType_t)
            .setLabel(LBsubjectSystem)
            .setLabel(LBsubjectNone)

            .setLabel(LBcopyToSender)
            .setLabel(LBnotifyToSender)
            .setLabel(LBsignatureType_t)
            .setLabel(LBmailUserSender)
            LBmailSystemSender.Text = String.Format(.getValue("System.Mail"), SystemSettings.Mail.SystemSender.Address)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Core.MailCommons.Domain.Signature))
                Dim oLabel As Label = Me.FindControl("LBsignature" & name)
                .setLabel(oLabel)
            Next
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(settings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings, senderEdit As Boolean, subjectEdit As Boolean, signatureEdit As Boolean, Optional ByVal editing As Boolean = True) Implements IViewMailSettings.InitializeControl
        Me.SetInternazionalizzazione()

        Me.CurrentPresenter.InitView(settings, senderEdit, subjectEdit, signatureEdit)
        Me.DVcopyToSender.Visible = AllowCopyToSender
        Me.DVnotifyToSender.Visible = AllowNotifyToSender
        If Not editing Then
            Me.RBhtml.Enabled = False
            Me.RBsenderSystem.Enabled = False
            Me.RBsenderUser.Enabled = False
            Me.RBsubjectNone.Enabled = False
            Me.RBsubjectSystem.Enabled = False
            Me.RBtext.Enabled = False
            Me.RBsignatureFromNoReplySettings.Enabled = False
            Me.RBsignatureFromConfigurationSettings.Enabled = False
            Me.RBsignatureFromField.Enabled = False
            Me.RBsignatureFromSkin.Enabled = False
            Me.RBsignatureFromTemplate.Enabled = False
            Me.RBsignatureNone.Enabled = False
        End If
    End Sub

    Private Sub SetUserMail(userMail As String) Implements IViewMailSettings.SetUserMail
        Me.LBmailUserSender.ToolTip = String.Format(Resource.getValue("LBmailUserSender.ToolTip"), userMail)
    End Sub
    Private Sub LoadSignatureTypes(types As List(Of lm.Comol.Core.MailCommons.Domain.Signature), defaultType As lm.Comol.Core.MailCommons.Domain.Signature) Implements IViewMailSettings.LoadSignatureTypes
        Dim oRadio As RadioButton = Nothing
        For Each t As lm.Comol.Core.MailCommons.Domain.Signature In types
            Dim oControl As HtmlGenericControl = Me.FindControl("SPNsignature" & t.ToString)
            oControl.Visible = True
        Next
        oRadio = Me.FindControl("RBsignature" & defaultType.ToString)
        oRadio.Checked = True
    End Sub
#End Region

#Region ""
    Private Function GetSelectedSignature() As lm.Comol.Core.MailCommons.Domain.Signature
        Dim t As lm.Comol.Core.MailCommons.Domain.Signature
        If Me.RBsignatureFromConfigurationSettings.Checked Then
            t = lm.Comol.Core.MailCommons.Domain.Signature.FromConfigurationSettings
        ElseIf Me.RBsignatureFromNoReplySettings.Checked Then
            t = lm.Comol.Core.MailCommons.Domain.Signature.FromNoReplySettings
        ElseIf RBsignatureFromField.Checked Then
            t = lm.Comol.Core.MailCommons.Domain.Signature.FromField
        ElseIf RBsignatureFromSkin.Checked Then
            t = lm.Comol.Core.MailCommons.Domain.Signature.FromSkin
        ElseIf RBsignatureFromTemplate.Checked Then
            t = lm.Comol.Core.MailCommons.Domain.Signature.FromTemplate
        ElseIf RBsignatureNone.Checked Then
            t = lm.Comol.Core.MailCommons.Domain.Signature.None
        End If
        Return t
    End Function
    Private Sub RBhtml_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBhtml.CheckedChanged
        If RaiseUpdateEvent Then
            RaiseEvent UpdateMailBody(True)
        End If
    End Sub
    Private Sub RBtext_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBtext.CheckedChanged
        If RaiseUpdateEvent Then
            RaiseEvent UpdateMailBody(False)
        End If
    End Sub
    Private Sub RBsenderSystem_CheckedChanged(sender As Object, e As System.EventArgs) Handles RBsenderSystem.CheckedChanged
        If RaiseUpdateEvent Then
            RaiseEvent UpdateMailSender(IIf(RBsenderSystem.Checked, lm.Comol.Core.MailCommons.Domain.SenderUserType.System, lm.Comol.Core.MailCommons.Domain.SenderUserType.LoggedUser))
        End If
    End Sub
    Private Sub RBsenderUser_CheckedChanged(sender As Object, e As System.EventArgs) Handles RBsenderUser.CheckedChanged
        If RaiseUpdateEvent Then
            RaiseEvent UpdateMailSender(IIf(RBsenderUser.Checked, lm.Comol.Core.MailCommons.Domain.SenderUserType.LoggedUser, lm.Comol.Core.MailCommons.Domain.SenderUserType.System))
        End If
    End Sub
    Private Sub RBsubjectSystem_CheckedChanged(sender As Object, e As System.EventArgs) Handles RBsubjectSystem.CheckedChanged
        If RaiseUpdateEvent Then
            RaiseEvent UpdateMailSubject(IIf(RBsubjectSystem.Checked, lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.None, lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration))
        End If
    End Sub
    Private Sub RBsubjectNone_CheckedChanged(sender As Object, e As System.EventArgs) Handles RBsubjectNone.CheckedChanged
        If RaiseUpdateEvent Then
            RaiseEvent UpdateMailSubject(IIf(RBsubjectNone.Checked, lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.None, lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration))
        End If
    End Sub
#End Region


   
End Class