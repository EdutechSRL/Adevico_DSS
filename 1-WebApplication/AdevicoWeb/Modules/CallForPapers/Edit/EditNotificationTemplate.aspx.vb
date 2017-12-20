Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.Mail

Public Class EditNotificationTemplate
    Inherits PageBase
    Implements IViewEditNotificationMail

#Region "Context"
    Private _Presenter As EditNotificationMailPresenter
    Private ReadOnly Property CurrentPresenter() As EditNotificationMailPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditNotificationMailPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewBaseEditCall.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewBaseEditCall.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseEditCall.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadType As CallForPaperType Implements IViewBaseEditCall.PreloadType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallForPaperType).GetByString(Request.QueryString("type"), CallForPaperType.CallForBids)
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewBaseEditCall.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewBaseEditCall.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewBaseEditCall.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
            Resource.setHyperLink(HYPpreviewCallBottom, value.ToString(), True, True)
            Resource.setHyperLink(HYPpreviewCallTop, value.ToString(), True, True)
            Resource.setHyperLink(HYPbackTop, value.ToString, True, True)
            Resource.setHyperLink(HYPbackBottom, value.ToString, True, True)
        End Set
    End Property
    Private Property IdCall As Long Implements IViewBaseEditCall.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewBaseEditCall.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewBaseEditCall.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdTemplate As Long Implements IViewEditNotificationMail.IdTemplate
        Get
            Return ViewStateOrDefault("IdTemplate", CLng(0))
        End Get
        Set(value As Long)
            Me.ViewState("IdTemplate") = value
        End Set
    End Property
    Private ReadOnly Property GetDefaultTemplate As dtoManagerTemplateMail Implements IViewEditNotificationMail.GetDefaultTemplate
        Get
            Dim template As New dtoManagerTemplateMail
            template.Id = 0
            template.IdLanguage = 1
            template.IdCallForPaper = IdCall
            template.Name = "Manager"
            template.Body = Resource.getValue(CallType.ToString & ".ManagerTemplate.Standard")
            template.Subject = Resource.getValue(CallType.ToString & ".ManagerTemplate.Subject")
            template.MailSettings = New lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings With {.CopyToSender = False, .IsBodyHtml = True, .NotifyToSender = False, .SenderType = lm.Comol.Core.MailCommons.Domain.SenderUserType.System, .PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration}
            template.NotifyFields = NotifyFields.Submitter
            Return template
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

    Public WriteOnly Property AllowUpdateTags As Boolean Implements IViewBaseEditCall.AllowUpdateTags
        Set(value As Boolean)

        End Set
    End Property
#End Region

    Private Sub EditCallAvailability_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If

        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()


    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "NoPermission")

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnocalls.Text = Resource.getValue("LBnoCalls." & CallType.ToString())


            .setHyperLink(HYPpreviewCallBottom, CallType.ToString(), True, True)
            .setHyperLink(HYPpreviewCallTop, CallType.ToString(), True, True)
            .setHyperLink(HYPbackTop, CallType.ToString, True, True)
            .setHyperLink(HYPbackBottom, CallType.ToString, True, True)
            .setButton(BTNsaveNotificationMailBottom, True, , , True)
            .setButton(BTNsaveNotificationMailTop, True, , , True)
            .setButton(BTNpreviewNotificationMailBottom, True)
            .setButton(BTNpreviewNotificationMailTop, True)
            .setButton(BTNcloseMailMessageWindow, True)
            .setLabel(LBnotificationTemplateName)
            .setLabel(LBnotificationTemplateName_t)
            .setLabel(LBnotifyTo)
            DVpreview.Attributes.Add("title", Resource.getValue("MailMessagePreview.Title"))
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditNotificationMail.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditNotificationMail.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.NoPermission, ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.EditNotificationTemplateMail(CallType, PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewBaseEditCall.SetActionUrl
        If action = CallStandardAction.PreviewCall Then
            Me.HYPpreviewCallBottom.Visible = True
            Me.HYPpreviewCallBottom.NavigateUrl = BaseUrl & url
            Me.HYPpreviewCallTop.Visible = True
            Me.HYPpreviewCallTop.NavigateUrl = BaseUrl & url
        Else
            'Select Case action
            '    Case CallStandardAction.List
            Me.HYPbackBottom.Visible = True
            Me.HYPbackBottom.NavigateUrl = BaseUrl & url
            Me.HYPbackTop.Visible = True
            Me.HYPbackTop.NavigateUrl = BaseUrl & url
            '    Case CallStandardAction.Manage
            '        Me.HYPmanage.Visible = True
            '        Me.HYPmanage.NavigateUrl = BaseUrl & url
            '    Case CallStandardAction.Add
            '        Me.HYPcreateCall.Visible = True
            '        Me.HYPcreateCall.NavigateUrl = BaseUrl & url
            'End Select
        End If
    End Sub

    Private Sub SetContainerName(action As CallStandardAction, name As String, itemName As String) Implements IViewBaseEditCall.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & action.ToString & "." & CallType.ToString())
        Master.ServiceTitle = String.Format(title, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        If String.IsNullOrEmpty(name) Then
            Master.ServiceTitleToolTip = String.Format(title, itemName)
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & action.ToString() & "." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub

    Private Sub LoadWizardSteps(idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of WizardCallStep))) Implements IViewBaseEditCall.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, type, idCommunity, PreloadView, steps)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewBaseEditCall.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Private Sub HideErrorMessages() Implements IViewEditNotificationMail.HideErrorMessages
        CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayErrorSaving() Implements IViewEditNotificationMail.DisplayErrorSaving
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNotificationTemplateErrorSaving"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayNoTemplates() Implements IViewEditNotificationMail.DisplayNoTemplate
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoNotificationTemplate"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewEditNotificationMail.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNotificationTemplateSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub LoadTemplate(template As dtoManagerTemplateMail) Implements IViewEditNotificationMail.LoadTemplate
        Dim dto As New dtoMailContent
        dto.Settings = template.MailSettings
        dto.Body = template.Body
        dto.Subject = template.Subject

        Dim attributes As New List(Of TranslatedItem(Of String))
        attributes = (From p In TemplatePlaceHolders.GetPlaceHoldersType(False) Where p <> PlaceHoldersType.None Select New TranslatedItem(Of String) With {.Id = p.ToString, .Translation = Me.Resource.getValue(CallType.ToString & ".PlaceHoldersType." & p.ToString)}).ToList()

        Me.CTRLtemplate.InitializeControl(dto, False, (CallType = CallForPaperType.RequestForMembership), attributes, False)
        Me.TXBnotifyTo.Text = template.NotifyTo
        Me.IdTemplate = template.Id
        LBtemplateName.Text = template.Name
    End Sub
    Private Sub DisplayMessagePreview(previewItem As lm.Comol.Core.Mail.dtoMailMessagePreview, recipient As String) Implements IViewEditNotificationMail.DisplayMessagePreview
        Me.DVpreview.Visible = True
        Me.CTRLmailpreview.InitializeControlForPreview(previewItem, recipient)
    End Sub
#End Region

    Private Sub BTNsaveTemplatesBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveNotificationMailBottom.Click, BTNsaveNotificationMailTop.Click
        Dim template As New dtoManagerTemplateMail With {.Id = IdTemplate, .IdLanguage = 1, .IdCallForPaper = IdCall, .NotifyFields = NotifyFields.Submitter}

        With template
            Dim dto As dtoMailContent = CTRLtemplate.Mail
            .MailSettings = dto.Settings
            .Body = dto.Body
            .Subject = dto.Subject
            .NotifyTo = Me.TXBnotifyTo.Text
            .Name = LBtemplateName.Text
        End With
        Me.CurrentPresenter.SaveSettings(template)

        CallTrapHelper.SendTrap(
                    lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
                    Me.IdCall,
                    lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
                    "ManagerNotifications")
    End Sub

    Private Sub BTNcloseMailMessageWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailMessageWindow.Click
        Me.DVpreview.Visible = False
    End Sub
  
    Private Sub BTNpreviewNotificationMailTop_Click(sender As Object, e As System.EventArgs) Handles BTNpreviewNotificationMailTop.Click, BTNpreviewNotificationMailBottom.Click
        Dim sTranslations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations))
            sTranslations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
        Next

        Dim template As New dtoManagerTemplateMail With {.Id = IdTemplate, .IdLanguage = 1, .IdCallForPaper = IdCall, .NotifyFields = NotifyFields.Submitter}

        With template
            Dim dto As dtoMailContent = CTRLtemplate.Mail
            .MailSettings = dto.Settings
            .Body = dto.Body
            .Subject = dto.Subject
            .NotifyTo = Me.TXBnotifyTo.Text
            .Name = LBtemplateName.Text
        End With
        Me.CurrentPresenter.PreviewMessage(template, Resource.getValue("fakeName"), Resource.getValue("fakeSurname"), Resource.getValue("fakeTaxCode"), Resource.getValue("fakeMail"), PageUtility.CurrentSmtpConfig, PageUtility.ApplicationUrlBase, sTranslations)
    End Sub

    Private Sub CTRLmailpreview_CloseWindow() Handles CTRLmailpreview.CloseWindow
        Me.DVpreview.Visible = False
    End Sub
End Class