Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class AddTemplate
    Inherits PageBase
    Implements IViewAdd

#Region "Context"
    Private _Presenter As AddPresenter
    Private ReadOnly Property CurrentPresenter() As AddPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewAdd.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadBackUrl As String Implements IViewAdd.PreloadBackUrl
        Get
            If PreloadCurrentSessionId = Guid.Empty Then
                Return Request.QueryString("BackUrl")
            Else
                Return Me.Session(lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.SessionName(PreloadCurrentSessionId))
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
    Private ReadOnly Property PreloadFromModuleCode As String Implements IViewBaseEditSetting.PreloadFromModuleCode
        Get
            Return Request.QueryString("mCodeCnt")
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
    Private Property AllowAdd As Boolean Implements IViewAdd.AllowAdd
        Get
            Return ViewStateOrDefault("AllowAdd", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAdd") = value
            Me.BTNcreateSettingsBottom.Visible = value
            Me.BTNcreateSettingsTop.Visible = value
        End Set
    End Property
    Private Property AllowSenderEdit As Boolean Implements IViewAdd.AllowSenderEdit
        Get
            Return ViewStateOrDefault("AllowSenderEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSenderEdit") = value
        End Set
    End Property
    Private Property AllowSubjectEdit As Boolean Implements IViewAdd.AllowSubjectEdit
        Get
            Return ViewStateOrDefault("AllowSubjectEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSubjectEdit") = value
        End Set
    End Property
    Private Property AllowSignatureEdit As Boolean Implements IViewAdd.AllowSignatureEdit
        Get
            Return ViewStateOrDefault("AllowSignatureEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSignatureEdit") = value
        End Set
    End Property

    Private Property IdTemplateCommunity As Integer Implements IViewAdd.IdTemplateCommunity
        Get
            Return ViewStateOrDefault("IdTemplateCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateCommunity") = value
        End Set
    End Property
    Private Property IdTemplateModule As Integer Implements IViewAdd.IdTemplateModule
        Get
            Return ViewStateOrDefault("IdTemplateModule", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateModule") = value
        End Set
    End Property
    Private Property CurrentType As TemplateType Implements IViewAdd.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", TemplateType.None)
        End Get
        Set(value As TemplateType)
            ViewState("CurrentType") = value
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
    Private Property ChannelSettings As List(Of dtoChannelConfigurator) Implements IViewAdd.ChannelSettings
        Get
            Return ViewStateOrDefault("ChannelSettings", New List(Of dtoChannelConfigurator))
        End Get
        Set(value As List(Of dtoChannelConfigurator))
            ViewState("ChannelSettings") = value
        End Set
    End Property
    Private ReadOnly Property SelectedContentModules As List(Of String) Implements IViewAdd.SelectedContentModules
        Get
            Return (From l As ListItem In SLBmodules.Items Where l.Selected Select l.Value).ToList()
        End Get
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
            Master.ServiceTitle = .getValue("serviceTitle.Add")
            Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(LBnoTemplate)
            .setHyperLink(HYPbackTop, False, True)
            .setHyperLink(HYPbackBottom, False, True)
            .setButton(BTNcreateSettingsBottom, True)
            .setButton(BTNcreateSettingsTop, True)
            .setLabel(LBaddTemplateDescription)
            .setLabel(LBtemplateName_t)
            .setLabel(LBtemplateLanguage_t)
            .setLabel(LBmoduleName_t)
            .setLabel(LBnotificationSettings_t)
            .setLabel(LBnotificationType_t)
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
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, "0"), InteractionType.UserWithLearningObject)
    End Sub
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

    Private Sub UnableToReadUrlSettings() Implements IViewAdd.UnableToReadUrlSettings
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnoTemplate.Text = Resource.getValue("UnableToReadUrlSettings")
    End Sub
    Private Sub DisplayAddUnavailable() Implements IViewAdd.DisplayAddUnavailable
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayAddUnavailable"), Helpers.MessageType.alert)
        Me.MLVsettings.SetActiveView(VIWsettings)
    End Sub
    Private Sub DisplayChannelSettingsError() Implements IViewAdd.DisplayChannelSettingsError
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayNotificationSettingsError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayTemplateAddError() Implements IViewAdd.DisplayTemplateAddError
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateAddError"), Helpers.MessageType.error)
    End Sub
    Public Sub DisplayTemplateUnableToAddNotificationChannel() Implements IViewAdd.DisplayTemplateUnableToAddNotificationChannel
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateUnableToAddNotificationType"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayInput(lng As lm.Comol.Core.DomainModel.Language, ByVal tNumber As Long, channels As List(Of lm.Comol.Core.Notification.Domain.NotificationChannel)) Implements IViewAdd.DisplayInput
        Me.CTRLmessages.Visible = False
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.TXBtemplateName.Text = String.Format(Resource.getValue("templatename"), tNumber)
        LoadChannels(channels)
    End Sub
    Private Sub LoadChannels(channels As List(Of lm.Comol.Core.Notification.Domain.NotificationChannel)) Implements IViewAdd.LoadChannels
        Me.DVnotificationSelector.Visible = True ' (types.Count > 0)
        Me.DVnotificationType.Visible = (channels.Count > 0)
        Me.LBnotificationType_t.Visible = (channels.Count > 0)
        If channels.Count > 0 Then
            If channels.Count = 1 Then
                DVnotificationType.Attributes("class") = DVnotificationType.Attributes("class").Replace("enabled", "")
            End If
            Me.RPTnotificationType.DataSource = channels
            Me.RPTnotificationType.DataBind()
        End If
    End Sub
    Private Sub DisplayInput(lng As lm.Comol.Core.DomainModel.Language, ByVal tNumber As Long, availableModules As List(Of String), channels As List(Of lm.Comol.Core.Notification.Domain.NotificationChannel)) Implements IViewAdd.DisplayInput
        DisplayInput(lng, tNumber, channels)
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
            'For Each idSubmitter As Long In field.Submitters
            '    Dim oListItem As ListItem = oSelect.Items.FindByValue(idSubmitter)
            '    If Not IsNothing(oListItem) Then
            '        oListItem.Selected = True
            '    End If
            'Next
            SLBmodules.Attributes.Add("data-placeholder", Resource.getValue("Modules.data-placeholder"))
            SLBmodules.Disabled = Not AllowAdd
            SPNmoduleSelectAll.Attributes.Add("title", Resource.getValue("SPNmoduleSelectAll.ToolTip"))
            SPNmoduleSelectAll.Visible = AllowAdd
            SPNmoduleSelectNone.Attributes.Add("title", Resource.getValue("SPNmoduleSelectNone.ToolTip"))
            SPNmoduleSelectNone.Visible = AllowAdd
        End If
    End Sub
    Private Sub SetBackUrl(url As String) Implements IViewAdd.SetBackUrl
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
    End Sub
    Private Function GetAvailableModules(Optional removeCodes As List(Of String) = Nothing) As List(Of String) Implements IViewAdd.GetAvailableModules
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
    Private Sub LoadWizardSteps(idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep))) Implements IViewAdd.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(0, 0, IdTemplateCommunity, PreloadFromIdCommunity, PreloadFromModuleCode, PreloadFromModulePermissions, CurrentType, Ownership, PreloadCurrentSessionId, GetEncodedBackUrl, steps)
    End Sub

    Private Function HasPermissionForObject(source As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewAdd.HasPermissionForObject
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

    'Private Function GetInUseNotificationTypes() As List(Of NotificationType) Implements IViewAdd.GetInUseNotificationTypes
    '    Dim settings As List(Of dtoNotificationConfigurator) = NotificationSettings
    '    If settings.Count > 0 Then
    '        Return settings.Select(Function(s) s.Type).ToList()
    '    Else
    '        Return New List(Of NotificationType)
    '    End If
    'End Function
    Private Function GetModuleCodesForNotification() As Dictionary(Of String, List(Of TranslatedItem(Of Long))) Implements IViewAdd.GetModuleCodesForNotification
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
    Private Function GetChannelSettings() As List(Of dtoChannelConfigurator) Implements IViewAdd.GetChannelSettings
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
    Private Sub LoadChannelSettings(items As List(Of dtoChannelConfigurator)) Implements IViewAdd.LoadChannelSettings
        Me.RPTnotificationSettings.Visible = (items.Count > 0)
        Me.RPTnotificationSettings.DataSource = items
        Me.RPTnotificationSettings.DataBind()
        DVnoNotifications.Visible = (items.Count = 0)
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewAdd.GoToUrl
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
        Me.CurrentPresenter.AddNotificationSetting(lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Notification.Domain.NotificationChannel).GetByString(e.CommandArgument, lm.Comol.Core.Notification.Domain.NotificationChannel.None), GetChannelSettings())
    End Sub
    Private Sub RPTnotificationSettings_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTnotificationSettings.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoChannelConfigurator = DirectCast(e.Item.DataItem, dtoChannelConfigurator)

            Dim oLabel As Label = e.Item.FindControl("LBsettingsTitle")
            oLabel.Text = Resource.getValue(dto.Channel.ToString & ".Title")

            Dim oButton As Button = e.Item.FindControl("BTNdeleteSettings")
            Resource.setButton(oButton, True)

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
                    oControl.InitializeControl(dto.Settings.MailSettings, AllowSenderEdit, AllowSubjectEdit, AllowSignatureEdit)
            End Select
        End If
    End Sub
    Private Sub RPTnotificationSettings_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTnotificationSettings.ItemCommand
        Select Case e.CommandName
            Case "virtualdelete"
                Dim oLiteral As Literal = e.Item.FindControl("LTtemporaryID")
                CurrentPresenter.VirtualDeleteNotificationSetting(GetChannelSettings(), New Guid(oLiteral.Text))
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

    Private Sub BTNcreateSettingsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcreateSettingsBottom.Click, BTNcreateSettingsTop.Click
        Me.CurrentPresenter.AddTemplate(TXBtemplateName.Text, SelectedContentModules(), GetChannelSettings())
    End Sub

End Class