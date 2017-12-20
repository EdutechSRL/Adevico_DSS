Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditTemplateSettings
    Inherits PageBase
    Implements IViewEditSettings

#Region "Context"
    Private _Presenter As EditSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As EditSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewEditSettings.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadBackUrl As String Implements IViewEditSettings.PreloadBackUrl
       Get
            If PreloadCurrentSessionId = Guid.Empty Then
                Return Request.QueryString("BackUrl")
            Else
                Return Me.Session(lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.SessionName(PreloadCurrentSessionId))
            End If

        End Get
    End Property
    Private ReadOnly Property PreloadPreview As Boolean Implements IViewEditSettings.PreloadPreview
        Get
            Return (Request.QueryString("preview") = "true")
        End Get
    End Property
    Private ReadOnly Property IsTemplateAdded As Boolean Implements IViewEditSettings.IsTemplateAdded
        Get
            Return (Request.QueryString("add") = "true")
        End Get
    End Property
    Private ReadOnly Property PreloadIdTemplate As Long Implements IViewEditSettings.PreloadIdTemplate
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdTemplate")) AndAlso IsNumeric(Request.QueryString("IdTemplate")) Then
                Return CLng(Request.QueryString("IdTemplate"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdVersion As Long Implements IViewEditSettings.PreloadIdVersion
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdVersion")) AndAlso IsNumeric(Request.QueryString("IdVersion")) Then
                Return CLng(Request.QueryString("IdVersion"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadOwnership As dtoBaseTemplateOwner Implements IViewBase.PreloadOwnership
        Get
            Dim item As New dtoBaseTemplateOwner()
            item.Type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OwnerType).GetByString(Request.QueryString("ownType"), OwnerType.None)
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunity")) AndAlso IsNumeric(Request.QueryString("idCommunity")) Then
                item.IdCommunity = CInt(Request.QueryString("idCommunity"))
            Else
                item.IdCommunity = -1
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("idPerson")) AndAlso IsNumeric(Request.QueryString("idPerson")) Then
                item.IdPerson = CInt(Request.QueryString("idPerson"))
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("idModule")) AndAlso IsNumeric(Request.QueryString("idModule")) Then
                item.IdModule = CInt(Request.QueryString("idModule"))
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("moduleCode")) Then
                item.ModuleCode = Request.QueryString("moduleCode")
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("idModuleP")) AndAlso IsNumeric(Request.QueryString("idModuleP")) Then
                item.ModulePermission = CLng(Request.QueryString("idModuleP"))
            End If

            Select Case item.Type
                Case OwnerType.Object
                    If Not String.IsNullOrEmpty(Request.QueryString("idObj")) AndAlso IsNumeric(Request.QueryString("idObj")) Then
                        item.IdObject = CLng(Request.QueryString("idObj"))
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("idObjType")) AndAlso IsNumeric(Request.QueryString("idObjType")) Then
                        item.IdObjectType = CInt(Request.QueryString("idObjType"))
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("idObjModule")) AndAlso IsNumeric(Request.QueryString("idObjModule")) Then
                        item.IdObjectModule = CInt(Request.QueryString("idObjModule"))
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("idObjCommunity")) AndAlso IsNumeric(Request.QueryString("idObjCommunity")) Then
                        item.IdObjectCommunity = CInt(Request.QueryString("idObjCommunity"))
                    End If
            End Select
            Return item
        End Get
    End Property
    Private ReadOnly Property PreloadTemplateType As TemplateType Implements IViewBase.PreloadTemplateType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateType).GetByString(Request.QueryString("type"), TemplateType.None)
        End Get
    End Property
    Private ReadOnly Property PreloadFromIdCommunity As Integer Implements IViewBaseEditSetting.PreloadFromIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunityCnt")) AndAlso IsNumeric(Request.QueryString("idCommunityCnt")) Then
                Return CLng(Request.QueryString("idCommunityCnt"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromModulePermissions As Long Implements IViewBaseEditSetting.PreloadFromModulePermissions
        Get
            If String.IsNullOrEmpty(Request.QueryString("mPrmCnt")) OrElse Not IsNumeric(Request.QueryString("mPrmCnt")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("mPrmCnt"))
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromModuleCode As String Implements IViewBaseEditSetting.PreloadFromModuleCode
        Get
            Return Request.QueryString("mCodeCnt")
        End Get
    End Property
    Private ReadOnly Property PreloadCurrentSessionId As Guid Implements IViewBaseEditSetting.PreloadCurrentSessionId
        Get
            If String.IsNullOrEmpty(Request.QueryString("sId")) Then
                Return Guid.Empty
            Else
                Try
                    Return New Guid(Request.QueryString("sId"))
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End If
        End Get
    End Property
    Private Property InputReadOnly As Boolean Implements IViewEditSettings.InputReadOnly
        Get
            Return ViewStateOrDefault("InputReadOnly", False)
        End Get
        Set(value As Boolean)
            ViewState("InputReadOnly") = value
            If value Then
                Me.BTNeditSettingsTop.Visible = Not value
                Me.BTNeditSettingsBottom.Visible = Not value
                Me.BTNeditDraftSettingsBottom.Visible = Not value
                Me.BTNeditDraftSettingsTop.Visible = Not value
                TXBtemplateName.ReadOnly = value
            End If
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewEditSettings.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNeditSettingsTop.Visible = value
            Me.BTNeditSettingsBottom.Visible = value
        End Set
    End Property
    Private Property AllowSaveDraft As Boolean Implements IViewEditSettings.AllowSaveDraft
        Get
            Return ViewStateOrDefault("AllowSaveDraft", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSaveDraft") = value
            Me.BTNeditDraftSettingsBottom.Visible = value
            Me.BTNeditDraftSettingsTop.Visible = value
            Me.BTNapplyModules.Visible = value
        End Set
    End Property
    Private Property AllowEditModuleContent As Boolean Implements IViewEditSettings.AllowEditModuleContent
        Get
            Return ViewStateOrDefault("AllowEditModuleContent", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEditModuleContent") = value
            Me.BTNapplyModules.Visible = AllowSaveDraft AndAlso value
            If Not value Then
                SLBmodules.Disabled = True
            End If

        End Set
    End Property
    Private Property AllowSenderEdit As Boolean Implements IViewEditSettings.AllowSenderEdit
        Get
            Return ViewStateOrDefault("AllowSenderEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSenderEdit") = value
        End Set
    End Property
    Private Property AllowSubjectEdit As Boolean Implements IViewEditSettings.AllowSubjectEdit
        Get
            Return ViewStateOrDefault("AllowSubjectEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSubjectEdit") = value
        End Set
    End Property
    Private Property AllowSignatureEdit As Boolean Implements IViewEditSettings.AllowSignatureEdit
        Get
            Return ViewStateOrDefault("AllowSignatureEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSignatureEdit") = value
        End Set
    End Property
    Private Property IdTemplateCommunity As Integer Implements IViewEditSettings.IdTemplateCommunity
        Get
            Return ViewStateOrDefault("IdTemplateCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateCommunity") = value
        End Set
    End Property
    Private Property IdTemplateModule As Integer Implements IViewEditSettings.IdTemplateModule
        Get
            Return ViewStateOrDefault("IdTemplateModule", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateModule") = value
        End Set
    End Property
    Private Property IdTemplate As Long Implements IViewEditSettings.IdTemplate
        Get
            Return ViewStateOrDefault("IdTemplate", 0)
        End Get
        Set(value As Long)
            ViewState("IdTemplate") = value
        End Set
    End Property
    Private Property IdVersion As Long Implements IViewEditSettings.IdVersion
        Get
            Return ViewStateOrDefault("IdVersion", 0)
        End Get
        Set(value As Long)
            ViewState("IdVersion") = value
        End Set
    End Property
    Private Property CurrentType As TemplateType Implements IViewEditSettings.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", TemplateType.None)
        End Get
        Set(value As TemplateType)
            ViewState("CurrentType") = value
        End Set
    End Property
    Private Property SavingStatus As TemplateStatus Implements IViewEditSettings.SavingStatus
        Get
            Return ViewStateOrDefault("SavingStatus", TemplateStatus.Draft)
        End Get
        Set(value As TemplateStatus)
            ViewState("SavingStatus") = value
        End Set
    End Property
    Private Property Ownership As dtoBaseTemplateOwner Implements IViewBase.Ownership
        Get
            Return ViewStateOrDefault("Ownership", New dtoBaseTemplateOwner() With {.Type = OwnerType.Person, .IdCommunity = -1})
        End Get
        Set(value As dtoBaseTemplateOwner)
            ViewState("Ownership") = value
        End Set
    End Property
    Private Property ChannelSettings As List(Of dtoChannelConfigurator) Implements IViewEditSettings.ChannelSettings
        Get
            Return ViewStateOrDefault("ChannelSettings", New List(Of dtoChannelConfigurator))
        End Get
        Set(value As List(Of dtoChannelConfigurator))
            ViewState("ChannelSettings") = value
        End Set
    End Property
    Private ReadOnly Property SelectedContentModules As List(Of String) Implements IViewEditSettings.SelectedContentModules
        Get
            Return (From l As ListItem In SLBmodules.Items Where l.Selected Select l.Value).ToList()
        End Get
    End Property
    Private Property InUsePlaceHolders As Dictionary(Of String, List(Of String)) Implements IViewEditSettings.InUsePlaceHolders
        Get
            Return ViewStateOrDefault("InUsePlaceHolders", New Dictionary(Of String, List(Of String)))
        End Get
        Set(value As Dictionary(Of String, List(Of String)))
            ViewState("InUsePlaceHolders") = value
        End Set
    End Property
    Private Property SavingSettings As Boolean Implements IViewEditSettings.SavingSettings
        Get
            Return ViewStateOrDefault("SavingSettings", False)
        End Get
        Set(value As Boolean)
            ViewState("SavingSettings") = value
        End Set
    End Property
    'Private Property InUseNotificationAction As Dictionary(Of String, List(Of Long)) Implements IViewBaseEditSetting.InUseNotificationAction
    '    Get
    '        Return ViewStateOrDefault("InUseNotificationAction", New Dictionary(Of String, List(Of Long)))
    '    End Get
    '    Set(value As Dictionary(Of String, List(Of Long)))
    '        ViewState("InUseNotificationAction") = value
    '    End Set
    'End Property
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
#End Region

#Region "Internal"
    Protected ReadOnly Property DialogTitleTranslation As String
        Get
            Return Resource.getValue("DialogTitleTranslation.ConfirmContentModules")
        End Get
    End Property
    Private Property TranslatedModules As List(Of PlainService)
        Get
            Return ViewStateOrDefault("TranslatedModules", ManagerService.ListSystemTranslated(PageUtility.LinguaID))
        End Get
        Set(value As List(Of PlainService))
            ViewState("TranslatedModules") = value
        End Set
    End Property
    Private Property TranslatedModuleSettings As Dictionary(Of String, List(Of TranslatedItem(Of Long)))
        Get
            Return ViewStateOrDefault("TranslatedModuleSettings", New Dictionary(Of String, List(Of TranslatedItem(Of Long))))
        End Get
        Set(value As Dictionary(Of String, List(Of TranslatedItem(Of Long))))
            ViewState("TranslatedModuleSettings") = value
        End Set
    End Property
    Private _Translations As List(Of TranslatedItem(Of String))
    Private ReadOnly Property TranslateNotificationModules As List(Of TranslatedItem(Of String))
        Get
            If _Translations Is Nothing Then
                Dim codes As List(Of String) = TranslatedModuleSettings.Select(Function(m) m.Key).ToList()
                _Translations = TranslatedModules.Where(Function(m) codes.Contains(m.Code)).Select(Function(m) New TranslatedItem(Of String) With {.Id = m.Code, .Translation = m.Name}).ToList()
                For Each code As String In codes.Where(Function(c) Not _Translations.Where(Function(t) t.Id = c).Any()).ToList()
                    _Translations.Add(New TranslatedItem(Of String) With {.Id = code, .Translation = Resource.getValue("Module." & code)})
                Next
            End If
            Return _Translations
        End Get
    End Property
    Private ReadOnly Property InEditing As Boolean
        Get
            Return Not InputReadOnly AndAlso (AllowSave OrElse AllowSaveDraft)
        End Get
    End Property


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Templates", "Modules", "Templates")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Master.ServiceTitle = .getValue("serviceTitle.EditStandard")
            Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(LBnoTemplate)
            .setHyperLink(HYPbackTop, False, True)
            .setHyperLink(HYPbackBottom, False, True)
            .setButton(BTNapplyModules, True)
            .setButton(BTNeditDraftSettingsBottom, True)
            .setButton(BTNeditDraftSettingsTop, True)
            .setButton(BTNeditSettingsBottom, True)
            .setButton(BTNeditSettingsTop, True)
            .setButton(BTNnoActionOnPlaceHolders, True)
            .setButton(BTNremovePlaceHoldersFromContent, True)
            .setButton(BTNundoApplyModuleContentEdit, True)
            .setLabel(LBeditTemplateDescription)
            .setLabel(LBtemplateName_t)
            .setLabel(LBtemplateLanguage_t)
            .setLabel(LBmoduleName_t)
            .setLabel(LBnotificationSettings_t)
            .setLabel(LBnotificationType_t)
            .setLabel(LBconfirmModulesContentForApply)
            .setLabel(LBtemplateVersion)
            .setLabel(LBconfirmModulesContent)
            .setLabel(LBnoNotifications)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewBase.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, IdTemplate), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadFromIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub UnableToReadUrlSettings() Implements IViewEditSettings.UnableToReadUrlSettings
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnoTemplate.Text = Resource.getValue("UnableToReadUrlSettings")
    End Sub
    Private Sub DisplayTemplateAdded() Implements IViewEditSettings.DisplayTemplateAdded
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateAdded"), Helpers.MessageType.success)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub DisplayTemplateSettingsErrors() Implements IViewEditSettings.DisplayTemplateSettingsErrors
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateSettingsErrors"), Helpers.MessageType.error)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub DisplayTemplateSettingsSaved() Implements IViewEditSettings.DisplayTemplateSettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateSettingsSaved"), Helpers.MessageType.success)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub DisplayUnknownTemplate() Implements IViewEditSettings.DisplayUnknownTemplate
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayUnknownTemplate"), Helpers.MessageType.alert)
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.MLVcontent.SetActiveView(VIWunknown)
    End Sub
    Public Sub DisplayTemplateUnableToAddNotificationChannel() Implements IViewEditSettings.DisplayTemplateUnableToAddNotificationChannel
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateUnableToAddNotificationType"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayTemplateSettingDeleted() Implements IViewEditSettings.DisplayTemplateSettingDeleted
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateSettingDeleted"), Helpers.MessageType.success)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub DisplayTemplateSettingErrorDeleting() Implements IViewEditSettings.DisplayTemplateSettingErrorDeleting
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateSettingErrorDeleting"), Helpers.MessageType.error)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub DisplayContentModulesErrorSaving() Implements IViewEditSettings.DisplayContentModulesErrorSaving
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayContentModulesErrorSaving"), Helpers.MessageType.error)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub DisplayContentModulesSaved() Implements IViewEditSettings.DisplayContentModulesSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayContentModulesSaved"), Helpers.MessageType.success)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub HideUserMessage() Implements IViewEditSettings.HideUserMessage
        Me.CTRLmessages.Visible = False
    End Sub
#End Region

    Private Sub DisplayInput(name As String, versionNumber As Integer, status As TemplateStatus, availableModules As List(Of String), selectedModules As List(Of String), channels As List(Of lm.Comol.Core.Notification.Domain.NotificationChannel)) Implements IViewEditSettings.DisplayInput
        Me.MLVsettings.SetActiveView(VIWsettings)
        If status = TemplateStatus.Draft Then
            LBeditTemplateDescription.Text = Resource.getValue("LBeditTemplateDescription.Draft.text")
        End If
        Me.TXBtemplateName.Text = name
        LBtemplateVersion.Text = String.Format(LBtemplateVersion.Text, versionNumber)
        LBtemplateVersion.Visible = True
        LoadChannels(channels)

        DVmoduleSelector.Visible = (availableModules.Count > 0)
        If availableModules.Count > 0 Then

            Dim modules As List(Of PlainService) = TranslatedModules.Where(Function(m) availableModules.Contains(m.Code)).ToList
            If availableModules.Contains(lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID) AndAlso Not modules.Where(Function(m) m.Code = lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID).Any() Then
                modules.Add(New PlainService() With {.ID = -1, .Code = lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID, .Name = Resource.getValue("Module." & lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID)})
            End If

            SLBmodules.DataSource = modules.OrderBy(Function(m) m.Name).ToList()
            SLBmodules.DataTextField = "Name"
            SLBmodules.DataValueField = "Code"
            SLBmodules.DataBind()
            For Each moduleCode As String In selectedModules
                Dim oListItem As ListItem = SLBmodules.Items.FindByValue(moduleCode)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
            Dim editing As Boolean = Not InputReadOnly AndAlso (AllowSave OrElse AllowSaveDraft) AndAlso AllowEditModuleContent
            SLBmodules.Attributes.Add("data-placeholder", Resource.getValue("Modules.data-placeholder"))
            SLBmodules.Disabled = Not editing
            SPNmoduleSelectAll.Attributes.Add("title", Resource.getValue("SPNmoduleSelectAll.ToolTip"))
            SPNmoduleSelectAll.Visible = editing
            SPNmoduleSelectNone.Attributes.Add("title", Resource.getValue("SPNmoduleSelectNone.ToolTip"))
            SPNmoduleSelectNone.Visible = editing
        End If
    End Sub
    Private Sub DisplayInput(name As String, versionNumber As Integer, status As TemplateStatus) Implements IViewEditSettings.DisplayInput
        Me.MLVsettings.SetActiveView(VIWsettings)
        If status = TemplateStatus.Draft Then
            LBeditTemplateDescription.Text = Resource.getValue("LBeditTemplateDescription.Draft.text")
        End If
        Me.TXBtemplateName.Text = name
        LBtemplateVersion.Text = String.Format(LBtemplateVersion.Text, versionNumber)
        LBtemplateVersion.Visible = True
    End Sub
    Private Sub LoadContentModules(selectedModules As List(Of String)) Implements IViewEditSettings.LoadContentModules
        For Each moduleCode As String In selectedModules
            Dim oListItem As ListItem = SLBmodules.Items.FindByValue(moduleCode)
            If Not IsNothing(oListItem) Then
                oListItem.Selected = True
            End If
        Next
    End Sub
    Private Sub LoadChannels(types As List(Of lm.Comol.Core.Notification.Domain.NotificationChannel)) Implements IViewEditSettings.LoadChannels
        Me.DVnotificationSelector.Visible = True ' (types.Count > 0)
        Me.DVnotificationType.Visible = (types.Count > 0)
        Me.LBnotificationType_t.Visible = (types.Count > 0)
        If types.Count > 0 Then
            If types.Count = 1 Then
                DVnotificationType.Attributes("class") = DVnotificationType.Attributes("class").Replace("enabled", "")
            End If
            Me.RPTnotificationType.DataSource = types
            Me.RPTnotificationType.DataBind()
        End If
    End Sub

    Private Sub SetBackUrl(url As String) Implements IViewEditSettings.SetBackUrl
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
    End Sub
    Private Function GetAvailableModules(Optional removeCodes As List(Of String) = Nothing) As List(Of String) Implements IViewEditSettings.GetAvailableModules
        Dim items As New List(Of String)
        Dim aCodes As New List(Of String)
        aCodes.Add(lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode)
        aCodes.Add(lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode)
        aCodes.Add(lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode)
        aCodes.Add(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode)
        aCodes.Add(lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode)
        aCodes.Add(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode)
        items.AddRange(TranslatedModules.Where(Function(m) aCodes.Contains(m.Code)).Select(Function(m) m.Code).ToList())
        items.Add(lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID)
        Return items
    End Function
    Private Sub LoadWizardSteps(idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep))) Implements IViewEditSettings.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(IdTemplate, IdVersion, IdTemplateCommunity, PreloadFromIdCommunity, PreloadFromModuleCode, PreloadFromModulePermissions, CurrentType, Ownership, PreloadCurrentSessionId, GetEncodedBackUrl, steps, PreloadPreview)
    End Sub

    Private Function HasPermissionForObject(source As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewEditSettings.HasPermissionForObject
        Return False
    End Function
    Private Function GetModulePermissions(moduleCode As String, idModule As Integer, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Implements IViewBaseEditSetting.GetModulePermissions
        Dim p As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages
        'If Not IsNothing(obj) Then
        '    p = New lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(obj.ServiceCode)

        'Else
        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing(permissions)
                End If
                p = wModule.ToTemplateModule()
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement(permissions)
                End If
                p = wModule.ToTemplateModule()
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                Dim wModule As lm.Comol.Core.BaseModules.Tickets.ModuleTicket
                If idCommunity = 0 Then
                    wModule = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Core.BaseModules.Tickets.ModuleTicket(permissions)
                End If
                p = wModule.ToTemplateModule()
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Dim cModule As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper
                If idCommunity = 0 Then
                    cModule = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.CreatePortalmodule(profileType)
                Else
                    cModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper(permissions)
                End If
                p = cModule.ToTemplateModule()
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Dim rModule As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership
                If idCommunity = 0 Then
                    rModule = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.CreatePortalmodule(profileType)
                Else
                    rModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership(permissions)
                End If
                p = rModule.ToTemplateModule()
            Case Else
                p = New lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(moduleCode)

        End Select

        'End If

        Return p
    End Function
    'Private Function GetInUseNotificationTypes() As List(Of NotificationType) Implements IViewEditSettings.GetInUseNotificationTypes
    '    Dim settings As List(Of dtoNotificationConfigurator) = NotificationSettings
    '    If settings.Count > 0 Then
    '        Return settings.Select(Function(s) s.Type).ToList()
    '    Else
    '        Return New List(Of NotificationType)
    '    End If
    'End Function
    Private Function GetModuleCodesForNotification() As Dictionary(Of String, List(Of TranslatedItem(Of Long))) Implements IViewEditSettings.GetModuleCodesForNotification
        Dim results As Dictionary(Of String, List(Of TranslatedItem(Of Long))) = TranslatedModuleSettings

        If results.Keys.Any Then
            Return results
        Else
            Dim tActions As New List(Of TranslatedItem(Of Long))
            tActions = (From p In [Enum].GetValues(GetType(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.MailSenderActionType)).Cast(Of lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.MailSenderActionType).ToList() Select New TranslatedItem(Of Long) With {.Id = CLng(p), .Translation = Me.Resource.getValue("ModuleWebConferencing.NotificationAction." & p.ToString)}).ToList()
            results.Add(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode, tActions)

            tActions = (From p In lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.GetNotificationActions() Select New TranslatedItem(Of Long) With {.Id = CLng(p), .Translation = Me.Resource.getValue("ModuleProfileManagement.NotificationAction." & p.ToString)}).ToList()
            results.Add(lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID, tActions)
        End If


        'lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
        'lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
        'lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode

        Return results
    End Function
    Private Function GetChannelSettings() As List(Of dtoChannelConfigurator) Implements IViewEditSettings.GetChannelSettings
        Dim settings As List(Of dtoChannelConfigurator) = ChannelSettings
        For Each row As RepeaterItem In RPTnotificationSettings.Items
            Dim oLiteral As Literal = row.FindControl("LTtemporaryID")
            Dim config As dtoChannelConfigurator = settings.Where(Function(s) s.TemporaryId = New Guid(oLiteral.Text)).FirstOrDefault()
            If Not IsNothing(config) Then
                'Dim oCheck As CheckBox = row.FindControl("CBXnotificationMode")
                'Dim oDDLmodules As DropDownList = row.FindControl("DDLmodules")
                'Dim oDDLactions As DropDownList = row.FindControl("DDLactions")
                'If oCheck.Checked AndAlso oDDLactions.SelectedIndex > 0 AndAlso oDDLmodules.SelectedIndex > 0 Then
                '    config.Settings.Mode = NotificationMode.Automatic
                '    config.Settings.ModuleCode = oDDLmodules.SelectedValue
                '    config.Settings.IdModuleAction = CLng(oDDLactions.SelectedValue)
                'Else
                '    config.Settings.Mode = NotificationMode.Manual
                'End If
                Select Case config.Channel
                    Case lm.Comol.Core.Notification.Domain.NotificationChannel.Mail
                        Dim control As UC_MailSettings = row.FindControl("CTRLmailSettings")
                        config.Settings.MailSettings = control.Settings
                End Select
            End If
        Next
        Return settings
    End Function
    Private Sub LoadChannelSettings(items As List(Of dtoChannelConfigurator)) Implements IViewEditSettings.LoadChannelSettings
        Me.RPTnotificationSettings.Visible = (items.Count > 0)
        Me.RPTnotificationSettings.DataSource = items
        Me.RPTnotificationSettings.DataBind()
        DVnoNotifications.Visible = (items.Count = 0)
    End Sub
    Private Sub DisplayConfirmModules(modulesCodes As List(Of String), placeHolders As Dictionary(Of String, List(Of String))) Implements IViewEditSettings.DisplayConfirmModules
        Me.DVmodulesConfirm.Visible = True
        Me.RPTremovedPlaceHolders.DataSource = placeHolders
        Me.RPTremovedPlaceHolders.DataBind()
    End Sub

    Private Function GetOldContentPlaceHolders(modulesCodes As List(Of String)) As Dictionary(Of String, List(Of String)) Implements IViewEditSettings.GetOldContentPlaceHolders
        Dim tags As New Dictionary(Of String, List(Of String))
        For Each Code As String In modulesCodes
            Select Case Code
                Case lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID
                    tags.Add(Code, lm.Comol.Core.BaseModules.ProfileManagement.TemplatePlaceHolders.PlaceHolders.Values.ToList())
                Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                    tags.Add(Code, lm.Comol.Modules.Standard.WebConferencing.Domain.TemplatePlaceHolders.PlaceHolders.Values.ToList())
                Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                    tags.Add(Code, lm.Comol.Modules.Standard.ProjectManagement.Domain.TemplatePlaceHolders.PlaceHolders.Values.ToList())
                Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                    tags.Add(Code, lm.Comol.Core.BaseModules.Tickets.Domain.TemplatePlaceHolders.PlaceHolders.Values.ToList())
                Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                    tags.Add(Code, lm.Comol.Modules.EduPath.TemplateEduPathPlaceHolders.PlaceHolders.ToDictionary(Function(k) k.Key, Function(v) v.Value).Values.ToList())
                    '.Where(Function(v) v.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.QuizList AndAlso v.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.QuizTable)

                Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                    tags.Add(Code, lm.Comol.Modules.CallForPapers.Domain.TemplatePlaceHolders.PlaceHolders(True).Values.ToList())
            End Select
        Next
        Return tags
    End Function
    Private Sub GoToUrl(url As String) Implements IViewEditSettings.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Function GetEncodedBackUrl() As String Implements IViewBaseEditSetting.GetEncodedBackUrl
        If String.IsNullOrEmpty(PreloadBackUrl) Then
            Return ""
        Else
            Return Server.UrlEncode(PreloadBackUrl)
        End If
    End Function
#End Region

#Region "Notifications"
    Private Sub RPTnotificationType_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnotificationType.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBnotificationType")
            Resource.setLinkButtonToValue(oLinkbutton, e.Item.DataItem.ToString(), False, True)
            oLinkbutton.CommandArgument = e.Item.DataItem.ToString()
        End If
    End Sub
    Private Sub RPTnotificationType_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTnotificationType.ItemCommand
        Me.CurrentPresenter.AddChannelSetting(lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Notification.Domain.NotificationChannel).GetByString(e.CommandArgument, lm.Comol.Core.Notification.Domain.NotificationChannel.None), GetChannelSettings())
    End Sub
    Private Sub RPTnotificationSettings_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnotificationSettings.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoChannelConfigurator = DirectCast(e.Item.DataItem, dtoChannelConfigurator)

            Dim oLabel As Label = e.Item.FindControl("LBsettingsTitle")
            oLabel.Text = Resource.getValue(dto.Channel.ToString & ".Title")

            Dim oButton As Button = e.Item.FindControl("BTNdeleteSettings")
            Resource.setButton(oButton, True)
            oButton.Visible = InEditing AndAlso dto.AllowDelete

            'oLabel = e.Item.FindControl("LBitemDescription")
            'oLabel.Text = Resource.getValue(dto.Channel.ToString & ".Description")

            'oLabel = e.Item.FindControl("LBnotificationMode_t")
            'Resource.setLabel(oLabel)

            'Dim oCheck As CheckBox = e.Item.FindControl("CBXnotificationMode")
            'Resource.setCheckBox(oCheck)

            'Dim modules As Dictionary(Of String, List(Of TranslatedItem(Of Long))) = GetModuleCodesForNotification()
            'Dim oDropdownList As DropDownList = e.Item.FindControl("DDLmodules")
            'Dim translations As List(Of TranslatedItem(Of String)) = TranslateNotificationModules

            'oLabel = e.Item.FindControl("LBmodules_t")
            'Resource.setLabel(oLabel)
            'oLabel = e.Item.FindControl("LBmodulesAction_t")
            'Resource.setLabel(oLabel)

            'oDropdownList.DataSource = translations
            'oDropdownList.DataValueField = "Id"
            'oDropdownList.DataTextField = "Translation"
            'oDropdownList.DataBind()
            'oDropdownList.Items.Insert(0, New ListItem(Resource.getValue("Module.Select"), ""))

            'Dim currentModule As String = ""
            'If dto.Mode = NotificationMode.Automatic Then
            '    If modules.ContainsKey(dto.Settings.ModuleCode) Then
            '        oDropdownList.SelectedValue = dto.Settings.ModuleCode
            '        currentModule = dto.Settings.ModuleCode
            '    End If
            'End If

            'Dim oDLLactions As DropDownList = e.Item.FindControl("DDLactions")
            'If currentModule = "" Then
            '    oDLLactions.Items.Insert(0, New ListItem(Resource.getValue("Module.SelectAction"), ""))
            'Else
            '    Try
            '        Dim actions As List(Of TranslatedItem(Of Long)) = modules(currentModule)
            '        If actions.Any() AndAlso InUseNotificationAction.ContainsKey(currentModule) Then
            '            actions = actions.Where(Function(a) (dto.Settings.IdModuleAction = a.Id) OrElse (dto.Settings.IdModuleAction <> a.Id AndAlso Not InUseNotificationAction(currentModule).Contains(a.Id))).ToList()
            '        End If


            '        oDLLactions.DataSource = actions
            '        oDLLactions.DataValueField = "Id"
            '        oDLLactions.DataTextField = "Translation"
            '        oDLLactions.DataBind()
            '        oDLLactions.Items.Insert(0, New ListItem(Resource.getValue("Module.SelectAction"), ""))

            '        If actions.Where(Function(a) a.Id = dto.Settings.IdModuleAction).Any AndAlso oDropdownList.SelectedValue = currentModule Then
            '            oDLLactions.SelectedValue = dto.Settings.IdModuleAction
            '        End If
            '    Catch ex As Exception
            '        oDLLactions.Items.Clear()
            '        oDLLactions.Items.Insert(0, New ListItem(Resource.getValue("Module.SelectAction"), ""))
            '    End Try
            'End If

            'Select Case dto.Mode
            '    Case NotificationMode.Automatic
            '        'If modules.Count > 0 AndAlso oDLLactions.Items.Count > 1 Then
            '        '    oCheck.Checked = (oDLLactions.SelectedIndex <> 0)
            '        'End If
            '        oCheck.Checked = True
            '    Case Else
            '        oCheck.Checked = False
            '        oCheck.Enabled = dto.AvailableModes.Contains(NotificationMode.Automatic)
            'End Select
            'Dim oGenericControl As HtmlGenericControl = e.Item.FindControl("DVservice")
            'If Not oCheck.Checked Then
            '    oGenericControl.Attributes("class") &= " disabled"
            '    oGenericControl = e.Item.FindControl("DVevents")
            '    oGenericControl.Attributes("class") &= " disabled"
            'End If
            Select Case dto.Channel
                Case lm.Comol.Core.Notification.Domain.NotificationChannel.Mail
                    Dim oControl As UC_MailSettings = e.Item.FindControl("CTRLmailSettings")
                    oControl.Visible = True
                    oControl.InitializeControl(dto.Settings.MailSettings, AllowSenderEdit, AllowSubjectEdit, AllowSignatureEdit, InEditing)
            End Select
            'If Not InEditing Then
            '    oCheck.Enabled = False
            '    oDropdownList.Enabled = False
            '    oDLLactions.Enabled = False
            'End If
        End If
    End Sub
    Private Sub RPTnotificationSettings_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTnotificationSettings.ItemCommand
        Select Case e.CommandName
            Case "virtualdelete"
                Dim oLiteral As Literal = e.Item.FindControl("LTtemporaryID")
                CurrentPresenter.VirtualDeleteChannelSetting(GetChannelSettings(), New Guid(oLiteral.Text))
        End Select
    End Sub
    Protected Sub DDLmodules_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Dim modules As Dictionary(Of String, List(Of TranslatedItem(Of Long))) = GetModuleCodesForNotification()
        Dim actions As List(Of TranslatedItem(Of Long)) = modules(sender.selectedvalue)
        Dim oDLLactions As DropDownList = sender.BindingContainer.FindControl("DDLactions")
        oDLLactions.DataSource = actions
        oDLLactions.DataValueField = "Id"
        oDLLactions.DataTextField = "Translation"
        oDLLactions.DataBind()
        oDLLactions.Items.Insert(0, New ListItem(Resource.getValue("Module.SelectAction"), ""))

        Dim oGenericControl As HtmlGenericControl = sender.BindingContainer.FindControl("DVservice")
        oGenericControl.Attributes("class") = oGenericControl.Attributes("class").Replace("disabled", "enabled")
        oGenericControl = sender.BindingContainer.FindControl("DVevents")
        oGenericControl.Attributes("class") = oGenericControl.Attributes("class").Replace("disabled", "enabled")
    End Sub
#End Region

    Private Sub AddTemplate_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub


    Private Sub BTNeditDraftSettingsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNeditDraftSettingsBottom.Click, BTNeditDraftSettingsTop.Click
        Me.CurrentPresenter.SaveSettings(TXBtemplateName.Text, SelectedContentModules, GetChannelSettings, TemplateStatus.Draft)
    End Sub
    Private Sub BTNeditSettingsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNeditSettingsBottom.Click, BTNeditSettingsTop.Click
        Me.CurrentPresenter.SaveSettings(TXBtemplateName.Text, SelectedContentModules, GetChannelSettings, TemplateStatus.Active)
    End Sub
    Private Sub BTNapplyModules_Click(sender As Object, e As System.EventArgs) Handles BTNapplyModules.Click
        Me.CurrentPresenter.SaveModuleContentSettings(SelectedContentModules())
    End Sub

    Private Sub BTNnoActionOnPlaceHolders_Click(sender As Object, e As System.EventArgs) Handles BTNnoActionOnPlaceHolders.Click
        Me.DVmodulesConfirm.Visible = False
        If SavingSettings Then
            Me.CurrentPresenter.LeavePlaceHolders(TXBtemplateName.Text, SelectedContentModules, GetChannelSettings, SavingStatus)
        Else
            Me.CurrentPresenter.LeavePlaceHolders(SelectedContentModules)
        End If
    End Sub

    Private Sub BTNremovePlaceHoldersFromContent_Click(sender As Object, e As System.EventArgs) Handles BTNremovePlaceHoldersFromContent.Click
        Me.DVmodulesConfirm.Visible = False
        If SavingSettings Then
            Me.CurrentPresenter.RemovePlaceHolders(TXBtemplateName.Text, SelectedContentModules, GetChannelSettings, SavingStatus)
        Else
            Me.CurrentPresenter.RemovePlaceHolders(SelectedContentModules)
        End If
    End Sub

    Private Sub BTNundoApplyModuleContentEdit_Click(sender As Object, e As System.EventArgs) Handles BTNundoApplyModuleContentEdit.Click
        Me.DVmodulesConfirm.Visible = False
        Me.CurrentPresenter.ReloadModules(SelectedContentModules)
    End Sub


   
    Private Sub RPTremovedPlaceHolders_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTremovedPlaceHolders.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then

            Dim oLiteral As Literal = e.Item.FindControl("LTModuleName")
            oLiteral.Text = TranslatedModules.Where(Function(t) t.Code = e.Item.DataItem.Key).Select(Function(t) t.Name).FirstOrDefault()
            If String.IsNullOrEmpty(oLiteral.Text) Then
                oLiteral.Text = Resource.getValue("Module." & e.Item.DataItem.Key)
            End If

            Dim oRepeater As Repeater = e.Item.FindControl("RPTplaceHolders")
            If oRepeater.Items.Count = 1 Then
                DirectCast(oRepeater.Items(0).FindControl("LIplaceHolder"), HtmlGenericControl).Attributes.Add("class", "first last")
            ElseIf oRepeater.Items.Count > 1 Then
                DirectCast(oRepeater.Items(0).FindControl("LIplaceHolder"), HtmlGenericControl).Attributes.Add("class", "first")
                DirectCast(oRepeater.Items(oRepeater.Items.Count - 1).FindControl("LIplaceHolder"), HtmlGenericControl).Attributes.Add("class", "last")
            End If
        End If
    End Sub
End Class