Imports lm.Comol.Core.BaseModules.MailSender.Presentation
Imports lm.Comol.Core.DomainModel

Public Class UC_MailMessage
    Inherits BaseControl
    Implements IViewMailMessage

#Region "Context"
    Private _Presenter As MailMessagePresenter
    Private ReadOnly Property CurrentPresenter() As MailMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MailMessagePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewMailMessage.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property AllowSendMail As Boolean Implements IViewMailMessage.AllowSendMail
        Get
            Return ViewStateOrDefault("AllowSendMail", False)
        End Get
        Set(value As Boolean)
            Me.DVsendTo.Visible = value
            BTNsendPreviewMail.Visible = value
            ViewState("AllowSendMail") = value
        End Set
    End Property
    Private Property CurrentMode As lm.Comol.Core.BaseModules.MailSender.PreviewMode Implements IViewMailMessage.CurrentMode
        Get
            Return ViewStateOrDefault("CurrentMode", lm.Comol.Core.BaseModules.MailSender.PreviewMode.None)
        End Get
        Set(value As lm.Comol.Core.BaseModules.MailSender.PreviewMode)
            ViewState("CurrentMode") = value
        End Set
    End Property
    Private Property DisplayOptions As Boolean Implements IViewMailMessage.DisplayOptions
        Get
            Return ViewStateOrDefault("DisplayOptions", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayOptions") = value
            Me.DVoptions.Visible = value
        End Set
    End Property
    Public Property DisplayOtherRecipients As Boolean Implements IViewMailMessage.DisplayOtherRecipients
        Get
            Return ViewStateOrDefault("DisplayOtherRecipients", False)
        End Get
        Set(value As Boolean)
            Me.DVrecipients.Visible = value
            ViewState("DisplayOtherRecipients") = value
        End Set
    End Property
    Public Property EditAddressTo As Boolean Implements IViewMailMessage.EditAddressTo
        Get
            Return Not Me.TXBaddressTo.ReadOnly
        End Get
        Set(value As Boolean)
            Me.TXBaddressTo.ReadOnly = Not value
        End Set
    End Property
    Private Property MailSettings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Implements IViewMailMessage.MailSettings
        Get
            Return ViewStateOrDefault("MailSettings", New lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings())
        End Get
        Set(value As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings)
            ViewState("MailSettings") = value
        End Set
    End Property
    Private Property FromDisplayName As String Implements IViewMailMessage.FromDisplayName
        Get
            Return ViewStateOrDefault("FromDisplayName", "")
        End Get
        Set(value As String)
            ViewState("FromDisplayName") = value
        End Set
    End Property
    Private Property FromMailAddress As String Implements IViewMailMessage.FromMailAddress
        Get
            Return ViewStateOrDefault("FromMailAddress", "")
        End Get
        Set(value As String)
            ViewState("FromMailAddress") = value
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

#Region "Control/Page"
    Public Event CloseWindow()
    Public Property DisplayTopWindowCloseButton As Boolean
        Get
            Return ViewStateOrDefault("DisplayTopWindowCloseButton", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayTopWindowCloseButton") = value
            BTNclosePreviewMessageWindow.Visible = value
        End Set
    End Property
    Private ReadOnly Property GetTranslatedProfileTypes As List(Of TranslatedItem(Of Integer))
        Get
            Return (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID)
                    Select New TranslatedItem(Of Integer) With {.Id = o.ID, .Translation = o.Descrizione}).ToList
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_MailEditor", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLinkButton(LNBaddressesOn, True, True)
            .setLinkButton(LNBaddressesOff, True, True)
            .setLinkButton(LNBattachmentsOn, True, True)
            .setLinkButton(LNBattachmentsOff, True, True)
            .setLinkButton(LNBoptionsOn, True, True)
            .setLinkButton(LNBoptionsOff, True, True)
            .setLabel(LBsentBy_t)
            .setButton(BTNsendPreviewMail, True)
            .setLabel(LBrecipients_t)
            .setLabel(LBaddressTo_t)

            .setLabel(LBshowAllrecipients)
            .setLabel(LBhideAllrecipients)
            .setLabel(LBattachments_t)
            .setLabel(LBshowAllAttachments)
            .setLabel(LBhideAllAttachments)
            .setLabel(LBmessageSubject_t)
            .setLiteral(LTmailAddressInvalidErrorsInfo)
            .setButton(BTNclosePreviewMessageWindow, True)
            .setHyperLink(HYPhideErrors, False, True)
        End With
    End Sub
#End Region


#Region "Implements"
    Public Sub InitializeControlForPreview(dtoContent As lm.Comol.Core.Mail.dtoMailMessagePreview, Optional recipients As String = "", Optional attachments As List(Of lm.Comol.Core.DomainModel.Repository.dtoBaseGenericFile) = Nothing) Implements IViewMailMessage.InitializeControlForPreview
        Me.CurrentPresenter.InitView(dtoContent, recipients, attachments)
    End Sub
    Public Sub InitializeControlForPreview(languageCode As String, content As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, cSettings As List(Of lm.Comol.Core.TemplateMessages.Domain.ChannelSettings), idCommunity As Integer, Optional obj As ModuleObject = Nothing) Implements IViewMailMessage.InitializeControlForPreview
        Me.CurrentPresenter.InitView(languageCode, content, PageUtility.CurrentSmtpConfig, "", Nothing, cSettings, idCommunity, obj)
    End Sub
    Public Sub InitializeControlForPreview(languageCode As String, content As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, modules As List(Of String), cSettings As List(Of lm.Comol.Core.TemplateMessages.Domain.ChannelSettings), idCommunity As Integer, Optional obj As ModuleObject = Nothing) Implements IViewMailMessage.InitializeControlForPreview
        Me.CurrentPresenter.InitView(languageCode, content, PageUtility.CurrentSmtpConfig, "", modules, cSettings, idCommunity, obj)
    End Sub
    Public Sub InitializeControlForPreview(languageCode As String, content As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, modules As List(Of String), mSettings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings, idCommunity As Integer, Optional obj As lm.Comol.Core.DomainModel.ModuleObject = Nothing) Implements IViewMailMessage.InitializeControlForPreview
        Me.CurrentPresenter.InitInternalView(languageCode, content, PageUtility.CurrentSmtpConfig, "", modules, mSettings, idCommunity, obj)
    End Sub
    Private Sub HideContent() Implements IViewMailMessage.HideContent
        Me.MLVmailMessage.SetActiveView(VIWempty)
    End Sub
    Private Sub LoadDisplayItems(items As List(Of lm.Comol.Core.BaseModules.MailSender.DisplayItem), switchOptionsOn As Boolean, onItems As List(Of lm.Comol.Core.BaseModules.MailSender.DisplayItem)) Implements IViewMailMessage.LoadDisplayItems
        DVswitchGroup.Visible = switchOptionsOn
        If switchOptionsOn Then
            LNBaddressesOn.Enabled = items.Contains(lm.Comol.Core.BaseModules.MailSender.DisplayItem.Recipients)
            LNBaddressesOff.Enabled = LNBaddressesOn.Enabled
            If Not LNBaddressesOn.Enabled Then
                LNBaddressesOn.Attributes("class") = LNBaddressesOn.Attributes("class") & " disabled"
                LNBaddressesOff.Attributes("class") = LNBaddressesOff.Attributes("class") & " disabled"
            End If
            If Not onItems.Contains(lm.Comol.Core.BaseModules.MailSender.DisplayItem.Recipients) Then
                LNBaddressesOn.Attributes("class") = Replace(LNBaddressesOn.Attributes("class"), "active", "")
                LNBaddressesOff.Attributes("class") = LNBaddressesOff.Attributes("class") & " active"
            End If

            LNBattachmentsOn.Enabled = items.Contains(lm.Comol.Core.BaseModules.MailSender.DisplayItem.Attachments)
            LNBattachmentsOff.Enabled = LNBattachmentsOn.Enabled
            If Not LNBattachmentsOn.Enabled Then
                LNBattachmentsOn.Attributes("class") = LNBattachmentsOn.Attributes("class") & " disabled"
                LNBattachmentsOff.Attributes("class") = LNBattachmentsOff.Attributes("class") & " disabled"
            End If
            If Not onItems.Contains(lm.Comol.Core.BaseModules.MailSender.DisplayItem.Attachments) Then
                LNBattachmentsOn.Attributes("class") = Replace(LNBattachmentsOn.Attributes("class"), "active", "")
                LNBattachmentsOff.Attributes("class") = LNBaddressesOff.Attributes("class") & " active"
            End If

            LNBoptionsOn.Enabled = items.Contains(lm.Comol.Core.BaseModules.MailSender.DisplayItem.Options)
            LNBoptionsOff.Enabled = LNBoptionsOff.Enabled
            If Not LNBoptionsOn.Enabled Then
                LNBoptionsOn.Attributes("class") = LNBoptionsOn.Attributes("class") & " disabled"
                LNBoptionsOff.Attributes("class") = LNBoptionsOff.Attributes("class") & " disabled"
            End If
            If Not onItems.Contains(lm.Comol.Core.BaseModules.MailSender.DisplayItem.Options) Then
                LNBoptionsOn.Attributes("class") = Replace(LNBoptionsOn.Attributes("class"), "active", "")
                LNBoptionsOff.Attributes("class") = LNBoptionsOff.Attributes("class") & " active"
            End If
        End If
    End Sub

    Private Sub LoadPreviewMessage(pMode As lm.Comol.Core.BaseModules.MailSender.PreviewMode, item As lm.Comol.Core.Mail.dtoMailMessagePreview, recipients As String, Optional attachments As List(Of lm.Comol.Core.DomainModel.Repository.dtoBaseGenericFile) = Nothing) Implements lm.Comol.Core.BaseModules.MailSender.Presentation.IViewMailMessage.LoadPreviewMessage
        Me.CurrentMode = pMode
        Me.TXBaddressTo.Text = recipients
        Me.LBmessageSubject.Text = item.Subject
        Me.LTmessageContent.Text = item.Body
        If item.SystemForUser Then
            LBsentBy.Text = String.Format(Resource.getValue("SystemForUser"), item.Sender.DisplayName, item.Sender.Address, item.From.DisplayName, item.From.Address)
        Else
            Me.LBsentBy.Text = item.From.DisplayName & " ( " & item.From.Address & " )"
        End If
        DVsentBy.Visible = True
        Me.MailSettings = item.Settings
        Me.FromDisplayName = item.From.DisplayName
        Me.FromMailAddress = item.From.Address
        Me.MLVmailMessage.SetActiveView(VIWmail)
    End Sub

    Private Sub LoadPreviewTemplateMessage(pMode As lm.Comol.Core.BaseModules.MailSender.PreviewMode, content As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation) Implements IViewMailMessage.LoadPreviewTemplateMessage
        Me.CurrentMode = pMode
        Me.TXBaddressTo.Text = ""
        Me.LBmessageSubject.Text = content.Subject
        Me.LTmessageContent.Text = content.Body
        'If item.SystemForUser Then
        '    LBsentBy.Text = String.Format(Resource.getValue("SystemForUser"), item.Sender.DisplayName, item.Sender.Address, item.From.DisplayName, item.From.Address)
        'Else
        '    Me.LBsentBy.Text = item.From.DisplayName & " ( " & item.From.Address & " )"
        'End If
        DVsentBy.Visible = False
        ' Me.MailSettings = item.Settings
        ' Me.FromDisplayName = item.From.DisplayName
        '  Me.FromMailAddress = item.From.Address
        Me.MLVmailMessage.SetActiveView(VIWmail)
    End Sub

    Private Sub LoadPreviewTemplateMessage(pMode As lm.Comol.Core.BaseModules.MailSender.PreviewMode, item As lm.Comol.Core.Mail.dtoMailMessagePreview, recipients As String) Implements IViewMailMessage.LoadPreviewTemplateMessage
        Me.CurrentMode = pMode
        Me.TXBaddressTo.Text = recipients
        Me.LBmessageSubject.Text = item.Subject
        Me.LTmessageContent.Text = item.Body
        If item.SystemForUser Then
            LBsentBy.Text = String.Format(Resource.getValue("SystemForUser"), item.Sender.DisplayName, item.Sender.Address, item.From.DisplayName, item.From.Address)
            DVsentBy.Visible = True
        ElseIf Not IsNothing(item.From) Then
            DVsentBy.Visible = True
            Me.LBsentBy.Text = item.From.DisplayName & " ( " & item.From.Address & " )"
        Else
            DVsentBy.Visible = False
        End If

        Me.MailSettings = item.Settings
        If Not IsNothing(item.From) Then
            Me.FromDisplayName = item.From.DisplayName
            Me.FromMailAddress = item.From.Address
        End If
        Me.MLVmailMessage.SetActiveView(VIWmail)
    End Sub

    Private Function GetPortalName(idLanguage As Integer) As String Implements IViewMailMessage.GetPortalName
        Return SystemSettings.Presenter.PortalDisplay.LocalizeName(idLanguage)
    End Function
    Private Function ParseContent(idLanguage As Integer, languageCode As String, content As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, modules As List(Of String), idcommunity As Integer, communityName As String, p As Person, organizationName As String, Optional obj As ModuleObject = Nothing) As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation Implements IViewMailMessage.ParseContent
        Dim fakeCommunity As New Community With {.Id = idcommunity, .Name = communityName}
        If idcommunity = -1 OrElse String.IsNullOrEmpty(communityName) Then
            fakeCommunity.Name = Resource.getValue("fakeCommunityName")
        End If
        Return ParseContent(idLanguage, languageCode, content, modules, fakeCommunity, p, organizationName, obj)
    End Function

    Private Function ParseContent(idLanguage As Integer, languageCode As String, content As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, modules As System.Collections.Generic.List(Of String), community As lm.Comol.Core.DomainModel.Community, p As Person, organizationName As String, Optional obj As lm.Comol.Core.DomainModel.ModuleObject = Nothing) As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation Implements IViewMailMessage.ParseContent
        Dim parseSubject As Boolean = Not String.IsNullOrEmpty(content.Subject) AndAlso content.Subject.Contains("[")
        Dim parseBody As Boolean = Not String.IsNullOrEmpty(content.Body) AndAlso content.Body.Contains("[")
        Dim parseShort As Boolean = Not String.IsNullOrEmpty(content.ShortText) AndAlso content.ShortText.Contains("[")
        Dim roleName, profileName As String

        Dim r As New ResourceManager
        r.UserLanguages = languageCode
        r.ResourcesName = "pg_UC_MailEditor"
        r.Folder_Level1 = "Modules"
        r.Folder_Level2 = "Common"
        r.setCulture()


        roleName = r.getValue("fakeRoleName")
        profileName = GetTranslatedProfileTypes.Where(Function(pt) pt.Id = UserTypeStandard.Student).FirstOrDefault().Translation
        Dim fakeUser As New Person
        fakeUser.Id = 0
        fakeUser.Name = r.getValue("fakeName")
        fakeUser.Surname = r.getValue("fakeSurname")
        fakeUser.TaxCode = r.getValue("fakeTaxCode")
        fakeUser.Name = r.getValue("fakeMail")
        fakeUser.TypeID = UserTypeStandard.Student

        Dim fakeLiteUser As New litePerson
        fakeLiteUser.Id = 0
        fakeLiteUser.Name = r.getValue("fakeName")
        fakeLiteUser.Surname = r.getValue("fakeSurname")
        fakeLiteUser.TaxCode = r.getValue("fakeTaxCode")
        fakeLiteUser.Name = r.getValue("fakeMail")
        fakeLiteUser.TypeID = UserTypeStandard.Student

        If parseSubject Then
            content.Subject = lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.Translate(content.Subject, fakeUser, community, DateTime.Now.AddDays(-5), organizationName, roleName, profileName, PageUtility.SystemSettings.Presenter.PortalDisplay.LocalizeIstanceName(idLanguage))
        End If
        If parseBody Then
            content.Body = lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.Translate(content.Body, fakeUser, community, DateTime.Now.AddDays(-5), organizationName, roleName, profileName, PageUtility.SystemSettings.Presenter.PortalDisplay.LocalizeIstanceName(idLanguage))
        End If
        If parseShort Then
            content.ShortText = lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.Translate(content.ShortText, fakeUser, community, DateTime.Now.AddDays(-5), organizationName, roleName, profileName, PageUtility.SystemSettings.Presenter.PortalDisplay.LocalizeIstanceName(idLanguage))
        End If
        For Each moduleCode As String In modules
            Select Case moduleCode
                Case lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID


                Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode

                    Try


                    Catch ex As Exception

                    End Try
                Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                    Try
                        Dim TKService As New lm.Comol.Core.BaseModules.Tickets.TicketService(PageUtility.CurrentContext)
                        content = TKService.NotificationGetTemplatePreview(True, content, GetTicketSettings(r, languageCode), GetTicketNotificationData(r))
                    Catch ex As Exception

                    End Try
                Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode

                Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                    Dim callTranslations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations, String)

                    For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations))
                        callTranslations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations), name), r.getValue("SubmissionTranslations." & name))
                    Next

                    Select Case moduleCode
                        Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                            Dim serviceCall As New lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(PageUtility.CurrentContext)
                            If IsNothing(obj) Then
                                Dim fakeCall As lm.Comol.Modules.CallForPapers.Domain.CallForPaper = GetFakeCallForPaper(fakeLiteUser, New liteCommunity() With {.Id = community.Id, .IdFather = community.IdFather, .IdOrganization = community.IdOrganization, .IdType = community.IdTypeOfCommunity, .Name = community.Name})
                                content = serviceCall.GetTemplateContentPreview("[", "]", True, PageUtility.ApplicationUrlBase, fakeCall, GetFakeSubmissionFields(fakeCall, fakeLiteUser), content, fakeLiteUser, callTranslations)
                            Else
                                content = serviceCall.GetTemplateContentPreview("[", "]", True, PageUtility.ApplicationUrlBase, obj.ObjectLongID, content, fakeLiteUser, callTranslations)
                            End If
                        Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                            Dim serviceRequest As New lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(PageUtility.CurrentContext)

                            If IsNothing(obj) Then
                                Dim rRequest As lm.Comol.Modules.CallForPapers.Domain.RequestForMembership = GetFakeRequest(fakeLiteUser, New liteCommunity() With {.Id = community.Id, .IdFather = community.IdFather, .IdOrganization = community.IdOrganization, .IdType = community.IdTypeOfCommunity, .Name = community.Name})
                                content = serviceRequest.GetTemplateContentPreview("[", "]", True, PageUtility.ApplicationUrlBase, rRequest, GetFakeSubmissionFields(rRequest, fakeLiteUser), content, fakeLiteUser, callTranslations)
                            Else
                                content = serviceRequest.GetTemplateContentPreview("[", "]", True, PageUtility.ApplicationUrlBase, obj.ObjectLongID, content, fakeLiteUser, callTranslations)
                            End If
                    End Select
            End Select
        Next
        Return content
    End Function

#Region "CallForPaper/Request For Membership"
    Private Function GetFakeCallForPaper(p As litePerson, ByVal c As liteCommunity) As lm.Comol.Modules.CallForPapers.Domain.CallForPaper
        Dim fakeCall As New lm.Comol.Modules.CallForPapers.Domain.CallForPaper

        With fakeCall
            .AcceptRefusePolicy = lm.Comol.Modules.CallForPapers.Domain.NotifyAcceptRefusePolicy.None
            .UseStartCompilationDate = False
            .Community = c
            .Description = Resource.getValue("CallForPaper.Description")
            .Edition = ""
            .EndDate = DateTime.Now.AddDays(10)
            .EndEvaluationOn = DateTime.Now.AddDays(20)
            .EvaluationType = lm.Comol.Modules.CallForPapers.Domain.EvaluationType.Sum
            .ForSubscribedUsers = True
            .IsPortal = IsNothing(c)
            .IsPublic = False
            .ModifiedBy = p
            .CreatedBy = p
            .CreatedOn = DateTime.Now.AddDays(-1)
            .ModifiedOn = .CreatedOn
            .Name = Resource.getValue("CallForPaper.Name")
            .OneCommitteeMembership = True
            .RevisionSettings = lm.Comol.Modules.CallForPapers.Domain.RevisionMode.None
            .StartDate = DateTime.Now.AddDays(-10)
            .Status = lm.Comol.Modules.CallForPapers.Domain.CallForPaperStatus.SubmissionOpened
            .SubmissionClosed = False
            .SubmittersType.Add(New lm.Comol.Modules.CallForPapers.Domain.SubmitterType() With {.Id = -100, .Call = fakeCall, .Name = Resource.getValue("CallForPaper.SubmittersType.Name"), .Description = Resource.getValue("CallForPaper.SubmittersType.Description")})
            .Sections.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldsSection() With {.Id = -101, .Call = fakeCall, .Name = Resource.getValue("CallForPaper.FieldsSection.1.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.1.Description"), .DisplayOrder = 1})
            .Sections(0).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -201, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Name, .Mandatory = True, .DisplayOrder = 1, .Call = fakeCall, .Section = fakeCall.Sections(0), .Name = Resource.getValue("CallForPaper.FieldDefinition.1.1.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.1.1.Description")})
            .Sections(0).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -202, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Surname, .Mandatory = True, .DisplayOrder = 2, .Call = fakeCall, .Section = fakeCall.Sections(0), .Name = Resource.getValue("CallForPaper.FieldDefinition.1.2.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.1.2.Description")})
            .Sections(0).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -203, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.TaxCode, .DisplayOrder = 3, .Call = fakeCall, .Section = fakeCall.Sections(0), .Name = Resource.getValue("CallForPaper.FieldDefinition.1.3.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.1.3.Description")})

            .Sections.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldsSection() With {.Id = -102, .Call = fakeCall, .Name = Resource.getValue("CallForPaper.FieldsSection.2.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.2.Description"), .DisplayOrder = 2})
            .Sections(1).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -301, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Mail, .Mandatory = True, .DisplayOrder = 1, .Call = fakeCall, .Section = fakeCall.Sections(1), .Name = Resource.getValue("CallForPaper.FieldDefinition.2.1.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.2.1.Description")})
            .Sections(1).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -302, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.TelephoneNumber, .DisplayOrder = 2, .Call = fakeCall, .Section = fakeCall.Sections(1), .Name = Resource.getValue("CallForPaper.FieldDefinition.2.2.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.2.2.Description")})
            .Sections(1).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -303, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine, .DisplayOrder = 3, .Call = fakeCall, .Section = fakeCall.Sections(1), .Name = Resource.getValue("CallForPaper.FieldDefinition.2.3.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.2.3.Description")})

            .Summary = Resource.getValue("CallForPaper.Summary")
            .Type = lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids
        End With

        Return fakeCall
    End Function
    Private Function GetFakeRequest(p As litePerson, ByVal c As liteCommunity) As lm.Comol.Modules.CallForPapers.Domain.RequestForMembership
        Dim fakeRequest As New lm.Comol.Modules.CallForPapers.Domain.RequestForMembership
        With fakeRequest
            .AcceptRefusePolicy = lm.Comol.Modules.CallForPapers.Domain.NotifyAcceptRefusePolicy.None
            .UseStartCompilationDate = False
            .Community = c
            .Description = Resource.getValue("RequestForMembership.Description")
            .Edition = ""
            .EndDate = DateTime.Now.AddDays(10)
            .ForSubscribedUsers = True
            .IsPortal = IsNothing(c)
            .IsPublic = False
            .ModifiedBy = p
            .CreatedBy = p
            .CreatedOn = DateTime.Now.AddDays(-1)
            .ModifiedOn = .CreatedOn
            .Name = Resource.getValue("RequestForMembership.Name")
            .RevisionSettings = lm.Comol.Modules.CallForPapers.Domain.RevisionMode.None
            .StartDate = DateTime.Now.AddDays(-10)
            .Status = lm.Comol.Modules.CallForPapers.Domain.CallForPaperStatus.SubmissionOpened
            .SubmissionClosed = False
            .SubmittersType.Add(New lm.Comol.Modules.CallForPapers.Domain.SubmitterType() With {.Id = -100, .Call = fakeRequest, .Name = Resource.getValue("RequestForMembership.SubmittersType.Name"), .Description = Resource.getValue("RequestForMembership.SubmittersType.Description")})
            .Sections.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldsSection() With {.Id = -101, .Call = fakeRequest, .Name = Resource.getValue("CallForPaper.FieldsSection.1.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.1.Description"), .DisplayOrder = 1})
            .Sections(0).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -201, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Name, .Mandatory = True, .DisplayOrder = 1, .Call = fakeRequest, .Section = fakeRequest.Sections(0), .Name = Resource.getValue("RequestForMembership.FieldDefinition.1.1.Name"), .Description = Resource.getValue("RequestForMembership.FieldsSection.1.1.Description")})
            .Sections(0).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -202, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Surname, .Mandatory = True, .DisplayOrder = 2, .Call = fakeRequest, .Section = fakeRequest.Sections(0), .Name = Resource.getValue("RequestForMembership.FieldDefinition.1.2.Name"), .Description = Resource.getValue("RequestForMembership.FieldsSection.1.2.Description")})
            .Sections(0).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -203, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine, .DisplayOrder = 3, .Call = fakeRequest, .Section = fakeRequest.Sections(0), .Name = Resource.getValue("RequestForMembership.FieldDefinition.1.3.Name"), .Description = Resource.getValue("RequestForMembership.FieldsSection.1.3.Description")})

            .Sections.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldsSection() With {.Id = -102, .Call = fakeRequest, .Name = Resource.getValue("CallForPaper.FieldsSection.2.Name"), .Description = Resource.getValue("CallForPaper.FieldsSection.2.Description"), .DisplayOrder = 2})
            .Sections(1).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -301, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Mail, .Mandatory = True, .DisplayOrder = 1, .Call = fakeRequest, .Section = fakeRequest.Sections(1), .Name = Resource.getValue("RequestForMembership.FieldDefinition.2.1.Name"), .Description = Resource.getValue("RequestForMembership.FieldsSection.2.1.Description")})
            .Sections(1).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -302, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine, .DisplayOrder = 2, .Call = fakeRequest, .Section = fakeRequest.Sections(1), .Name = Resource.getValue("RequestForMembership.FieldDefinition.2.2.Name"), .Description = Resource.getValue("RequestForMembership.FieldsSection.2.2.Description")})
            .Sections(1).Fields.Add(New lm.Comol.Modules.CallForPapers.Domain.FieldDefinition() With {.Id = -303, .Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine, .DisplayOrder = 3, .Call = fakeRequest, .Section = fakeRequest.Sections(1), .Name = Resource.getValue("RequestForMembership.FieldDefinition.2.3.Name"), .Description = Resource.getValue("RequestForMembership.FieldsSection.2.3.Description")})

            .Summary = Resource.getValue("RequestForMembership.Summary")
            .Type = lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.RequestForMembership
        End With
        Return fakeRequest
    End Function
    Private Function GetFakeSubmissionFields(ByVal cl As lm.Comol.Modules.CallForPapers.Domain.CallForPaper, fakeUser As litePerson) As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField)
        Dim fields As New List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField)

        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 1, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(cl.Sections(0).Fields(0)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.Name)})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 2, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(cl.Sections(0).Fields(1)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.Surname)})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 3, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(cl.Sections(0).Fields(2)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.TaxCode)})

        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 4, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(cl.Sections(1).Fields(0)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.Mail)})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 5, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(cl.Sections(1).Fields(1)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField("0101-01010101")})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 6, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(cl.Sections(1).Fields(2)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField("")})
        Return fields
    End Function
    Private Function GetFakeSubmissionFields(ByVal rq As lm.Comol.Modules.CallForPapers.Domain.RequestForMembership, fakeUser As litePerson) As List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField)
        Dim fields As New List(Of lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField)

        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 1, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(rq.Sections(0).Fields(0)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.Name)})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 2, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(rq.Sections(0).Fields(1)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.Surname)})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 3, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(rq.Sections(0).Fields(2)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField("")})

        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 4, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(rq.Sections(1).Fields(0)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.Mail)})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 5, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(rq.Sections(1).Fields(1)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField(fakeUser.Name & "." & fakeUser.Surname)})
        fields.Add(New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionValueField With {.Id = 6, .Field = New lm.Comol.Modules.CallForPapers.Domain.dtoCallField(rq.Sections(1).Fields(2)), .Value = New lm.Comol.Modules.CallForPapers.Domain.dtoValueField("")})
        Return fields
    End Function
#End Region

    Private Function GetFakeWebConference(p As Person, ByVal c As Community) As lm.Comol.Modules.Standard.WebConferencing.Domain.DTO.DTO_RoomTagData
        Dim oRoom As New lm.Comol.Modules.Standard.WebConferencing.Domain.DTO.DTO_RoomTagData()
        oRoom.DestUser = New lm.Comol.Modules.Standard.WebConferencing.Domain.DTO.DTO_UserTagData()

        With oRoom
            .Name = Resource.getValue("Webconferencing.Room.Name")
            .Code = "#cdcd#"
            If (IsNothing(c) OrElse c.Id <= 0 OrElse String.IsNullOrEmpty(c.Name)) Then
                .CommunityName = SystemSettings.Presenter.PortalDisplay.LocalizeName(p.LanguageID)
            Else
                .CommunityName = c.Name
            End If


            Try
                .CreatedBy = Me.CurrentPresenter.UserContext.CurrentUser
            Catch ex As Exception
                .CreatedBy = New Person()
                .CreatedBy.SurnameAndName = p.SurnameAndName
            End Try

            .CreatedOn = Now.AddDays(-5)
            .Description = Resource.getValue("Webconferencing.Room.Description")
            '.DestUser.
            .FinishOn = Now.AddDays(5)
            .StartOn = Now.AddDays(-5)
            '.Summary = Resource.getValue("RequestForMembership.Summary")
        End With

        With oRoom.DestUser
            .AccessKey = "#akak#"
            .Name = Resource.getValue("Webconferencing.User.Name")
            .SName = Resource.getValue("Webconferencing.User.SName")
            .LanguageCode = Resource.getValue("Webconferencing.User.LangCode")
            .Mail = Resource.getValue("Webconferencing.User.Mail")
        End With
        Return oRoom
    End Function

#Region "ModuleTicket"
    Private Function GetTicketSettings(resource As ResourceManager, ByVal lCode As String) As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_NotificationSettings
        Dim settings As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_NotificationSettings()
        With settings
            .AvailableCategoryTypes = [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryType)).Cast(Of lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryType).ToList.ToDictionary(Of lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryType, String)(Function(c) c, Function(c) resource.getValue("ModuleTicket.Category.CategoryType." & c.ToString))
            .AvailableTicketStatus = [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketStatus)).Cast(Of lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketStatus).ToList.ToDictionary(Of lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketStatus, String)(Function(c) c, Function(c) resource.getValue("ModuleTicket.Category.TicketStatus." & c.ToString))
            .BaseUrl = PageUtility.ApplicationUrlBase
            .CategoriesTemplate = ""
            .DateTimeFormat = resource.CultureInfo.DateTimeFormat.FullDateTimePattern
            .DateTimeFormats = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures).ToDictionary(Of String, String)(Function(c) c.Name, Function(c) c.DateTimeFormat.FullDateTimePattern)
            .LangCode = lCode
            .SmtpConfig = PageUtility.CurrentSmtpConfig
        End With
        Return settings
    End Function
    Private Function GetTicketNotificationData(resource As ResourceManager, Optional action As lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.none) As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_NotificationData
        Dim oData As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_NotificationData
        oData.Answer = GetTicketFakeAnswer(resource, lm.Comol.Core.BaseModules.Tickets.Domain.Enums.MessageType.FeedBack)
        oData.Action = GetTicketFakeAction(resource, action)
        Select Case action
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.externalPasswordChanged, lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.externalRecover
                oData.User = GetTicketFakeUser(resource, False)
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.externalRegistration
                oData.User = GetTicketFakeUser(resource, True)

                'Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.addAnswer, lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.assignmentChange, _
                '      lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.statusChange
                '    oData.User = GetTicketFakeUser(resource, False)
                '    oData.Ticket = GetTicketFakeTicket(resource)

                'Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.categoryChange, lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.categoryAssignedChange
                '    oData.Category = GetTicketFakeCategory(resource)
                '    oData.Ticket = GetTicketFakeTicket(resource)
            Case Else
                oData.User = GetTicketFakeUser(resource, False)
                oData.Category = GetTicketFakeCategory(resource)
                oData.Ticket = GetTicketFakeTicket(resource)
        End Select

        Return oData
    End Function
    Private Function GetTicketFakeAnswer(resource As ResourceManager, ByVal type As lm.Comol.Core.BaseModules.Tickets.Domain.Enums.MessageType) As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Answer
        Return New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Answer() With {.ShortText = resource.getValue("ModuleTicket.FakeAnswer.ShortText." & type.ToString), .SendDate = DateTime.Now, .FullText = resource.getValue("ModuleTicket.FakeAnswer.FullText." & type.ToString), .Type = type}
    End Function
    Private Function GetTicketFakeAction(resource As ResourceManager, ByVal action As lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType) As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Action
        Return New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Action() With {.UserDisplayName = resource.getValue("ModuleTicket.FakeAction.UserDisplayName." & action.ToString), .UserRole = resource.getValue("ModuleTicket.FakeAction.UserDisplayName." & action.ToString)}
    End Function
    Private Function GetTicketFakeCategory(resource As ResourceManager) As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Category
        Dim category As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Category
        With category
            .Description = resource.getValue("ModuleTicket.FakeCategory.Description")
            .Type = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryType.Public
            .NewAssignerDisplayName = resource.getValue("ModuleTicket.FakeCategory.NewAssignerDisplayName")
            .Name = resource.getValue("ModuleTicket.FakeCategory.Name")

            .Translations = GetTicketFakeCategoryTranslations(resource)
            .LanguagesCode = .Translations.Select(Function(t) t.LanguageCode).ToList()
        End With
        category.LanguagesCode.Insert(0, "multi")
        category.Translations.Insert(0, New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_CategoryLocalization() With {.Description = resource.getValue("ModuleTicket.FakeCategory.Description.multi"), .LanguageCode = "multi", .Language = "multi", .Name = resource.getValue("ModuleTicket.FakeCategory.Name.multi")})
        Return category
    End Function
    Private Function GetTicketFakeCategoryTranslations(resource As ResourceManager, Optional endtranslationkey As String = "") As List(Of lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_CategoryLocalization)
        Dim items As New List(Of lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_CategoryLocalization)

        Dim languages As List(Of Lingua) = ManagerLingua.GetLanguages()
        For Each language As Lingua In languages
            Dim t As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_CategoryLocalization
            Dim r As New ResourceManager
            r.UserLanguages = language.Codice
            r.ResourcesName = "pg_Templates"
            r.Folder_Level1 = "Modules"
            r.Folder_Level2 = "Templates"
            r.setCulture()
            t.Description = resource.getValue("ModuleTicket.FakeCategory.Description" & endtranslationkey)
            t.Language = language.Nome
            t.LanguageCode = language.Codice
            t.Name = resource.getValue("ModuleTicket.FakeCategory.Name" & endtranslationkey)

            items.Add(t)
        Next
        items.Insert(0, New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_CategoryLocalization() With {.Description = resource.getValue("ModuleTicket.FakeCategory.Description.multi" & endtranslationkey), .LanguageCode = "multi", .Language = "multi", .Name = resource.getValue("ModuleTicket.FakeCategory.Name.multi" & endtranslationkey)})
        Return items
    End Function
    Private Function GetTicketFakeUser(resource As ResourceManager, Optional ByVal withToken As Boolean = True) As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_User
        Dim user As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_User
        user.LanguageCode = resource.UserLanguages
        user.Mail = resource.getValue("ModuleTicket.FakeUser.Mail")
        user.Name = resource.getValue("ModuleTicket.FakeUser.Name")
        user.Surname = resource.getValue("ModuleTicket.FakeUser.Surname")
        user.Password = resource.getValue("ModuleTicket.FakeUser.Password")

        If withToken Then
            user.Token = New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_User.DTO_Token() With {.Code = Guid.NewGuid().ToString, .Creation = DateTime.Now}
        End If
        Return user
    End Function
    Private Function GetTicketFakeTicket(resource As ResourceManager) As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Ticket
        Dim ticket As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification.DTO_Ticket
        ticket.CategoryCurrent = GetTicketFakeCategoryTranslations(resource).ToDictionary(Function(t) t.LanguageCode, Function(t) t.Name)
        ticket.CategoryInitial = GetTicketFakeCategoryTranslations(resource, ".Initial").ToDictionary(Function(t) t.LanguageCode, Function(t) t.Name)
        ticket.Assigner = resource.getValue("ModuleTicket.FakeTicket.Assigner")
        ticket.CreatorDisplayName = resource.getValue("ModuleTicket.FakeTicket.CreatorDisplayName")
        ticket.Language = resource.CultureInfo.Name
        ticket.LanguageCode = resource.UserLanguages
        ticket.LongText = resource.getValue("ModuleTicket.FakeTicket.LongText")
        ticket.Preview = resource.getValue("ModuleTicket.FakeTicket.Preview")
        ticket.SendDate = DateTime.Now
        ticket.Status = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketStatus.open
        ticket.Title = resource.getValue("ModuleTicket.FakeTicket.Title")

        Return ticket
    End Function
#End Region

#End Region

    Private Sub BTNsendMail_Click(sender As Object, e As System.EventArgs) Handles BTNsendPreviewMail.Click
        Me.CurrentPresenter.SendMail(FromDisplayName, FromMailAddress, LBmessageSubject.Text, LTmessageContent.Text, Me.TXBaddressTo.Text, MailSettings, PageUtility.CurrentSmtpConfig)
    End Sub

    Private Sub BTNclosePreviewMessageWindow_Click(sender As Object, e As System.EventArgs) Handles BTNclosePreviewMessageWindow.Click
        RaiseEvent CloseWindow()
    End Sub
End Class