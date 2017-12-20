Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class UC_ProfileMailEditor
    Inherits BaseControl
    Implements IViewMailEditor
   
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
    Private _Presenter As MailEditorPresenter
    Private ReadOnly Property CurrentPresenter() As MailEditorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MailEditorPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdProfile As Integer Implements IViewMailEditor.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IsInitialized As Boolean Implements IViewMailEditor.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property IdPendingRequest As Long Implements IViewMailEditor.IdPendingRequest
        Get
            Return ViewStateOrDefault("IdPendingRequest", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdPendingRequest") = value
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

#Region "Control"
    Public Event RefreshContainer()
    Public Event MailUpdated(mailAddress As String)
    Public Event CloseWindow()
    Public Property InAjaxPanel As Boolean
        Get
            Return ViewStateOrDefault("InAjaxPanel", False)
        End Get
        Set(value As Boolean)
            ViewState("InAjaxPanel") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBmailEditorInfo_t)
            .setLabel(LBmailEditorInfo)
            .setLabel(LBmail_t)
            .setLabel(LBmailConfirm_t)

            .setRegularExpressionValidator(REVmail)
            .setRegularExpressionValidator(REVmailConfirm)
            .setCompareValidator(CVmail)
            .setLabel(LBmailCodeInfo_t)
            .setLabel(LBmailCodeInfo)
            .setLabel(LBmailCode_t)
            .setButton(BTNsaveMail, True)
            .setButton(BTNinsertCode, True)
            .setButton(BTNnewMail, True)
            .setButton(BTNsendCode, True)
            .setButton(BTNbackToMailEditorStep, True)
            .setButton(BTNcloseMailWindowFromCode, True)
            .setButton(BTNcloseMailWindowFromEditor, True)
            .setButton(BTNcloseMailWindowFromErrors, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idUser As Integer) Implements IViewMailEditor.InitializeControl
        Me.IsInitialized = True
        Me.CurrentPresenter.InitView(idUser)
    End Sub
    Public Sub DisplayMailEditor() Implements IViewMailEditor.DisplayMailEditor
        Me.MLVmail.SetActiveView(VIWeditor)
        Me.TXBconfirmMail.Text = ""
        Me.TXBmail.Text = ""
    End Sub
    Public Sub DisplayWaitingCode(createdOn As Date, mailAddress As String) Implements IViewMailEditor.DisplayWaitingCode
        Me.MLVmail.SetActiveView(VIWcode)
        Resource.setLabel(LBmailCodeInfo)
        LBmailCodeInfo.Text = String.Format(LBmailCodeInfo.Text, mailAddress, FormatDateTime(createdOn, DateFormat.ShortDate), FormatDateTime(createdOn, DateFormat.ShortTime))
        TXBmailCode.Text = ""
    End Sub

    Public Sub DisplayProfileUnknown() Implements IViewMailEditor.DisplayProfileUnknown
        Me.MLVmail.SetActiveView(VIWerrors)
        Me.LBgenericMessage.Text = Resource.getValue("DisplayProfileUnknown")
        Me.BTNbackToMailEditorStep.Visible = False
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewMailEditor.DisplaySessionTimeout
        Me.MLVmail.SetActiveView(VIWerrors)
        Me.LBgenericMessage.Text = Resource.getValue("DisplaySessionTimeout")
        Me.BTNbackToMailEditorStep.Visible = False
    End Sub
    Public Sub DisplayActivationComplete(mail As String) Implements IViewMailEditor.DisplayActivationComplete
        Me.MLVmail.SetActiveView(VIWerrors)
        Me.LBgenericMessage.Text = Resource.getValue("DisplayActivationComplete")
        Me.BTNbackToMailEditorStep.Visible = False
        RaiseEvent MailUpdated(mail)
    End Sub

    Public Sub DisplayError(errorMessage As lm.Comol.Core.BaseModules.ProfileManagement.Presentation.ErrorMessages) Implements IViewMailEditor.DisplayError
        Me.MLVmail.SetActiveView(VIWerrors)
        Me.LBgenericMessage.Text = Resource.getValue("ErrorMessages." & errorMessage.ToString)
        Me.BTNbackToMailEditorStep.Visible = True
        Me.BTNbackToMailEditorStep.CommandName = errorMessage.ToString
        If InAjaxPanel Then
            RaiseEvent RefreshContainer()
        End If
    End Sub
#End Region

    Private Sub BTNinsertCode_Click(sender As Object, e As System.EventArgs) Handles BTNinsertCode.Click
        Me.CurrentPresenter.ActivateMail(Me.TXBmailCode.Text)
    End Sub
    Private Sub BTNnewMail_Click(sender As Object, e As System.EventArgs) Handles BTNnewMail.Click
        Me.DisplayMailEditor()
        If InAjaxPanel Then
            RaiseEvent RefreshContainer()
        End If
    End Sub
    Private Sub BTNsaveMail_Click(sender As Object, e As System.EventArgs) Handles BTNsaveMail.Click
        Dim pending As MailEditingPending = Me.CurrentPresenter.SavePendingChanges(Me.TXBmail.Text)
        If Not IsNothing(pending) Then
            Me.SendMailToUser(pending)
            Me.DisplayWaitingCode(pending.CreatedOn, pending.Mail)
            If InAjaxPanel Then
                RaiseEvent RefreshContainer()
            End If
        End If
    End Sub
    Private Sub BTNsendCode_Click(sender As Object, e As System.EventArgs) Handles BTNsendCode.Click
        Me.CurrentPresenter.SendNewCode()
    End Sub
    Private Sub BTNbackToMailEditorStep_Click(sender As Object, e As System.EventArgs) Handles BTNbackToMailEditorStep.Click
        Dim currentMessage As ErrorMessages = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ErrorMessages).GetByString(sender.CommandName, ErrorMessages.None)
        Select Case currentMessage
            Case ErrorMessages.AlreadyActivated
                Me.DisplayMailEditor()
            Case ErrorMessages.InvalidCode
                Me.MLVmail.SetActiveView(VIWcode)
            Case ErrorMessages.NoPendingRequest
                Me.DisplayMailEditor()
            Case ErrorMessages.UnsavedRequest
                Me.DisplayMailEditor()
            Case Else
                Me.DisplayMailEditor()
        End Select
        If InAjaxPanel Then
            RaiseEvent RefreshContainer()
        End If
    End Sub

#Region "Mail"
    Private Sub SendMailToUser(ByVal pendingRequest As MailEditingPending)
        Try
            Dim oResourceConfig As New ResourceManager
            oResourceConfig = GetResourceConfig(Session("LinguaCode"))

            Dim oUtility As New OLDpageUtility(Me.Context)
            Dim oMail As New COL_E_Mail(oUtility.LocalizedMail)

            oMail.Mittente = New MailAddress(oResourceConfig.getValue("systemMail"), oResourceConfig.getValue("systemMailSender"))
            oMail.Oggetto = Me.Resource.getValue("mail.Subject")
            oMail.Body = Me.Resource.getValue("mail.Body")

            oMail.Body = String.Format(oMail.Body, pendingRequest.Person.SurnameAndName, pendingRequest.Mail, _
                                       FormatDateTime(pendingRequest.CreatedOn, DateFormat.ShortDate), FormatDateTime(pendingRequest.CreatedOn, DateFormat.ShortTime), _
                                      oUtility.ApplicationUrlBase & RootObject.ActivateUserMail(pendingRequest.UrlIdentifier), _
                                      pendingRequest.ActivationCode)

            oMail.Body = oMail.Body & vbCrLf & vbCrLf & vbCrLf & oResourceConfig.getValue("systemMailFirma")

            oMail.IndirizziTO.Add(New MailAddress(pendingRequest.Mail))
            'oMail.Body = Replace(oMail.Body, "&nbsp;", "")
            'oMail.Body = Replace(oMail.Body, "<br>", vbCrLf)
            oMail.InviaMailHTML()

        Catch ex As Exception

        End Try
    End Sub

  
#End Region
  
    Private Sub BTNcloseMailWindowFromCode_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailWindowFromCode.Click, BTNcloseMailWindowFromEditor.Click, BTNcloseMailWindowFromErrors.Click
        RaiseEvent CloseWindow()
    End Sub
End Class