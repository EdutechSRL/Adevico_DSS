Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel.Languages

Public Class NotificationSettings
    Inherits PageBase
    Implements IViewNotificationSettings

#Region "Context"
    Private _Presenter As NotificationSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As NotificationSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New NotificationSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Preload"
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewNotificationSettings.PreloadIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunity")) AndAlso IsNumeric(Request.QueryString("idCommunity")) Then
                Return CInt(Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdOrganization As Integer Implements IViewNotificationSettings.PreloadIdOrganization
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idOrg")) AndAlso IsNumeric(Request.QueryString("idOrg")) Then
                Return CInt(Request.QueryString("idOrg"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadForPortal As Boolean Implements IViewNotificationSettings.PreloadForPortal
        Get
            Return (Request.QueryString("portal") = "true")
        End Get
    End Property
    Private ReadOnly Property PreloadModuleObject As ModuleObject Implements IViewNotificationSettings.PreloadModuleObject
        Get
            Dim item As New ModuleObject()

            If Not String.IsNullOrEmpty(Request.QueryString("oId")) AndAlso IsNumeric(Request.QueryString("oId")) Then
                item.ObjectLongID = CLng(Request.QueryString("oId"))
            End If
            If item.ObjectLongID > 0 Then
                If Not String.IsNullOrEmpty(Request.QueryString("oType")) AndAlso IsNumeric(Request.QueryString("oType")) Then
                    item.ObjectTypeID = CInt(Request.QueryString("oType"))
                End If
                If Not String.IsNullOrEmpty(Request.QueryString("oIdModule")) AndAlso IsNumeric(Request.QueryString("oIdModule")) Then
                    item.ServiceID = CInt(Request.QueryString("oIdModule"))
                End If
                If Not String.IsNullOrEmpty(Request.QueryString("oCommunity")) AndAlso IsNumeric(Request.QueryString("oCommunity")) Then
                    item.CommunityID = CInt(Request.QueryString("oCommunity"))
                End If
                item.ServiceCode = Request.QueryString("oMcode")
                Return item
            Else
                Return Nothing
            End If

        End Get
    End Property
#End Region

#Region "Current"
    Private Property SettingsIdCommunity As Integer Implements IViewNotificationSettings.SettingsIdCommunity
        Get
            Return ViewStateOrDefault("SettingsIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("SettingsIdCommunity") = value
        End Set
    End Property
    Private Property SettingsIdOrganization As Integer Implements IViewNotificationSettings.SettingsIdOrganization
        Get
            Return ViewStateOrDefault("SettingsIdOrganization", 0)
        End Get
        Set(value As Integer)
            ViewState("SettingsIdOrganization") = value
        End Set
    End Property
    Private Property SettingsForObject As Boolean Implements IViewNotificationSettings.SettingsForObject
        Get
            Return ViewStateOrDefault("SettingsForObject", False)
        End Get
        Set(value As Boolean)
            ViewState("SettingsForObject") = value
        End Set
    End Property
    Private Property SettingsObj As ModuleObject Implements IViewNotificationSettings.SettingsObj
        Get
            If Not IsNothing(ViewState("SettingsObj")) Then
                Try
                    Return DirectCast(ViewState("SettingsObj"), ModuleObject)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(value As ModuleObject)
            ViewState("SettingsObj") = value
        End Set
    End Property
    Private Property SettingsForPortal As Boolean Implements IViewNotificationSettings.SettingsForPortal
        Get
            Return ViewStateOrDefault("SettingsForPortal", False)
        End Get
        Set(value As Boolean)
            ViewState("SettingsForPortal") = value
        End Set
    End Property
    Private Property SettingsLevel As TemplateLevel Implements IViewNotificationSettings.SettingsLevel
        Get
            Return ViewStateOrDefault("SettingsLevel", TemplateLevel.None)
        End Get
        Set(value As TemplateLevel)
            ViewState("SettingsLevel") = value
        End Set
    End Property
    Private Property CurrentPermissions As Dictionary(Of String, lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages) Implements IViewNotificationSettings.CurrentPermissions
        Get
            Return ViewStateOrDefault("CurrentPermissions", New Dictionary(Of String, lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages))
        End Get
        Set(value As Dictionary(Of String, lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages))
            ViewState("CurrentPermissions") = value
        End Set
    End Property
#End Region

    Private ReadOnly Property SelectedItems As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents) Implements IViewNotificationSettings.SelectedItems
        Get
            Dim items As New List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents)

            For Each row As RepeaterItem In RPTmodules.Items
                Dim item As New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents
                item.ModuleCode = DirectCast(row.FindControl("LTmoduleCode"), Literal).Text
                For Each rowEvent As RepeaterItem In DirectCast(row.FindControl("RPTmoduleEvents"), Repeater).Items
                    Dim eItem As New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent
                    eItem.IdEvent = DirectCast(rowEvent.FindControl("LTidEvent"), Literal).Text
                    eItem.ModuleCode = item.ModuleCode
                    Dim oControl As UC_TemplateSelector = rowEvent.FindControl("CTRLselector")
                    Dim s As dtoVersionItem = oControl.SelectedItem
                    If Not IsNothing(s) Then
                        eItem.IdTemplate = s.IdTemplate
                        eItem.IdVersion = s.Id
                        eItem.ItemLevel = s.Level
                    End If
                    eItem.IsMandatory = CBool(DirectCast(rowEvent.FindControl("LTisMandatory"), Literal).Text)
                    item.Events.Add(eItem)
                Next
                items.Add(item)
            Next
            Return items
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
#End Region

#Region "Page/Control"
    Public ReadOnly Property PersistStoreName() As String
        Get
            Return "NotificationSettings_" & PageUtility.CurrentUser.ID
        End Get
    End Property
    Public ReadOnly Property DialogTitle() As String
        Get
            Return Resource.getValue("DialogTitle.SettingsError")
        End Get
    End Property

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(PreloadForPortal, PreloadIdCommunity, PreloadIdOrganization, PreloadModuleObject)
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
            Master.ServiceTitle = .getValue("serviceTitle.Settings")
            Master.ServiceNopermission = .getValue("nopermission")
            '.setHyperLink(HYPbackUrl, False, True)
            .setLinkButton(LNBsaveSettingsUp, False, True)
            .setLiteral(LTmoduleNameHeader_t)
            .setLiteral(LTtemplateNameHeader_t)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewNotificationSettings.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, "0"), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewNotificationSettings.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    'Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer, moduleCode As String) Implements IViewBaseModuleMessage.DisplayNoPermission
    '    Select Case moduleCode
    '        Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
    '            Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
    '        Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
    '            Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
    '        Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
    '            Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
    '    End Select

    '    Me.BindNoPermessi()
    'End Sub
    Private Sub DisplaySessionTimeout() Implements IViewNotificationSettings.DisplaySessionTimeout
        DisplaySessionTimeout(lm.Comol.Core.TemplateMessages.RootObject.Settings(PreloadForPortal, PreloadIdCommunity, PreloadIdOrganization, PreloadModuleObject))
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewNotificationSettings.DisplaySessionTimeout
        Dim idCommunity As Integer = SettingsIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub DisplaySavedSettings(savedItems As Integer, unselectedItems As Integer, unsavedItems As Integer, inheritedItems As Integer, mandatoryMissing As Integer) Implements IViewNotificationSettings.DisplaySavedSettings
        Dim mType As Helpers.MessageType = Helpers.MessageType.alert
        Dim tKey As String = "SavedSettings."
        Dim translation As String = ""
        If savedItems = 0 AndAlso (unselectedItems > 0 OrElse unsavedItems > 0 OrElse mandatoryMissing) Then
            mType = IIf(unsavedItems > 0 OrElse mandatoryMissing > 0, Helpers.MessageType.error, Helpers.MessageType.info)
            tKey &= "notSaved."
            tKey &= IIf(unselectedItems <= 1, unselectedItems, "n") & "." & IIf(unsavedItems > 1, "n", unsavedItems.ToString) & "." & IIf(mandatoryMissing > 1, "n", mandatoryMissing.ToString)

            Select Case unselectedItems
                Case 0, 1
                    Select Case unsavedItems
                        Case 0, 1
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = Resource.getValue(tKey)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), mandatoryMissing)
                            End Select
                        Case Else
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = String.Format(Resource.getValue(tKey), unsavedItems)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), unsavedItems, mandatoryMissing)
                            End Select

                    End Select
                Case Else
                    Select Case unsavedItems
                        Case 0, 1
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = String.Format(Resource.getValue(tKey), unselectedItems)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), unselectedItems, mandatoryMissing)
                            End Select
                        Case Else
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = String.Format(Resource.getValue(tKey), unselectedItems, unsavedItems)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), unselectedItems, unsavedItems, mandatoryMissing)
                            End Select
                    End Select
            End Select

        ElseIf savedItems > 0 Then
            mType = IIf(unsavedItems > 0 OrElse mandatoryMissing > 0, Helpers.MessageType.alert, Helpers.MessageType.success)
            tKey &= "savedItems."
            tKey &= IIf(savedItems = 1, "1", "n") & "." & IIf(unsavedItems > 1, "n", unsavedItems.ToString) & "." & IIf(mandatoryMissing > 1, "n", mandatoryMissing.ToString)
            Select Case unsavedItems
                Case 0, 1
                    Select Case savedItems
                        Case 1
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = Resource.getValue(tKey)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), mandatoryMissing)
                            End Select

                        Case Else
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = String.Format(Resource.getValue(tKey), savedItems)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), savedItems, mandatoryMissing)
                            End Select
                    End Select
                Case Else
                    Select Case savedItems
                        Case 1
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = String.Format(Resource.getValue(tKey), unsavedItems)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), unsavedItems, mandatoryMissing)
                            End Select
                        Case Else
                            Select Case mandatoryMissing
                                Case 0, 1
                                    translation = String.Format(Resource.getValue(tKey), savedItems, unsavedItems)
                                Case Else
                                    translation = String.Format(Resource.getValue(tKey), savedItems, unsavedItems, mandatoryMissing)
                            End Select
                    End Select
            End Select
        End If
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(translation, mType)
    End Sub
    Private Sub DisplayMandatoryMissing(items As Integer) Implements IViewNotificationSettings.DisplayMandatoryMissing
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(IIf(items > 1, String.Format(Resource.getValue("DisplayMandatoryMissing.n"), items), Resource.getValue("DisplayMandatoryMissing")), Helpers.MessageType.error)
    End Sub

    Private Sub DisplaySavingErrors() Implements IViewNotificationSettings.DisplaySavingErrors
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplaySavingErrors"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayPortalInfo() Implements IViewNotificationSettings.DisplayPortalInfo
        Master.ServiceTitle = Resource.getValue("serviceTitle.Settings.Portal")
        Master.ServiceTitleToolTip = Resource.getValue("serviceTitle.Settings")
    End Sub
    Private Sub DisplayCommunityInfo(name As String) Implements IViewNotificationSettings.DisplayCommunityInfo
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.Settings.Community"), name)
        Master.ServiceTitleToolTip = Resource.getValue("serviceTitle.Settings")
    End Sub
    Private Sub DisplayOrganizationInfo(name As String) Implements IViewNotificationSettings.DisplayOrganizationInfo
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.Settings.Organization"), name)
        Master.ServiceTitleToolTip = Resource.getValue("serviceTitle.Settings")
    End Sub
    Private Sub DisplayObjectInfo(moduleCode As String, idObjectType As Integer) Implements IViewNotificationSettings.DisplayObjectInfo
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.Settings.Object"), Resource.getValue("SaveAsObjectName." & moduleCode))
        Master.ServiceTitleToolTip = Resource.getValue("serviceTitle.Settings")
    End Sub
#End Region
    Private Function GetAvailableModules(forPortal As Boolean) As List(Of String) Implements IViewNotificationSettings.GetAvailableModules
        Dim modules As New List(Of String)
        If forPortal Then
            modules.Add(lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID)
        End If
        modules.Add(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode)
        modules.Add(lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode)
        modules.Add(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode)
        Return modules
    End Function
    Private Function GetTranslatedModuleActions(code As String) As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleAction) Implements IViewNotificationSettings.GetTranslatedModuleActions
        Dim oList As New List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleAction)

        Select Case code
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Return (From p In [Enum].GetValues(GetType(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.MailSenderActionType)).Cast(Of lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.MailSenderActionType).ToList() Select New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleAction() With {.Name = Me.Resource.getValue("ModuleWebConferencing.NotificationAction." & p.ToString), .ModuleCode = code, .IdEvent = CLng(p), .IsMandatory = (p <> lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.MailSenderActionType.GenericInvitation)}).ToList
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                Return (From p In [Enum].GetValues(GetType(lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.MailSenderActionType)).Cast(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.MailSenderActionType).ToList() Select New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleAction() With {.Name = Me.Resource.getValue("ModuleProjectManagement.NotificationAction." & p.ToString), .ModuleCode = code, .IdEvent = CLng(p), .IsMandatory = False}).ToList
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                Return (From p In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType)).Cast(Of lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType).ToList() Where p <> lm.Comol.Core.BaseModules.Tickets.ModuleTicket.MailSenderActionType.none Select New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleAction() With {.Name = Me.Resource.getValue("ModuleTicket.NotificationAction." & p.ToString), .ModuleCode = code, .IdEvent = CLng(p), .IsMandatory = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.GetMandatoryActions().Contains(p)}).ToList
            Case lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID
                Return (From p In lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.GetNotificationActions() Select New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleAction() With {.Name = Me.Resource.getValue("ModuleProfileManagement.NotificationAction." & p.ToString), .IdEvent = CLng(p), .ModuleCode = code, .IsMandatory = False}).ToList()
        End Select
        Return oList
    End Function
    Private Function GetTranslatedModules(codes As List(Of String)) As Dictionary(Of String, String) Implements IViewNotificationSettings.GetTranslatedModules
        Dim translations As Dictionary(Of String, String) = ManagerService.ListSystemTranslated(PageUtility.LinguaID).Where(Function(m) codes.Contains(m.Code)).ToDictionary(Function(m) m.Code, Function(m) m.Name)

        For Each code As String In codes.Where(Function(c) Not translations.ContainsKey(c)).ToList()
            translations.Add(code, Resource.getValue("Module." & code))
        Next
        Return translations
    End Function
    Private Sub LoadEmptyItems() Implements IViewNotificationSettings.LoadEmptyItems
        LoadItems(New List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents))
    End Sub
    Private Sub LoadItems(events As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents)) Implements IViewNotificationSettings.LoadItems
        Me.RPTmodules.DataSource = events
        Me.RPTmodules.DataBind()
        Me.MLVcontent.SetActiveView(VIWcontent)
    End Sub
    Private Function GetModulePermissions(moduleCode As String, permissions As Long, idCommunity As Integer, profileType As Integer) As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Implements IViewNotificationSettings.GetModulePermissions
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
            Case lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID
                Dim pModule As New lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement
                pModule = lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.CreatePortalmodule(profileType)

                p = pModule.ToTemplateModule()
            Case Else
                p = New lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(moduleCode)

        End Select

        'End If

        Return p
    End Function
#End Region

#Region "Page Controls"

    Private Sub RPTmodules_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTmodules.ItemCommand
        Me.CurrentPresenter.ReloadItems(SelectedItems.SelectMany(Function(i) i.Events))
    End Sub
    Private Sub RPTmodules_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmodules.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents = DirectCast(e.Item.DataItem, lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents)
            Dim oLabel As Label = e.Item.FindControl("LBeventsEnabled")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBeventsTotal")
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBmoduleErrors")
            oLabel.Visible = dto.Errors.Any()
            If dto.Errors.Any() Then
                If dto.Errors.Distinct.Count = 1 Then
                    Dim er As lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError = dto.Errors.FirstOrDefault()
                    Select Case er
                        Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.TemplateRemoved, lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.VersionRemoved, lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.TemplateUnselected
                            oLabel.CssClass = LTiconsBaseCssClass.Text & IIf(dto.Events.Where(Function(ev) ev.IsMandatory AndAlso ev.Error = er).Any(), lm.Comol.Core.DomainModel.Helpers.MessageType.error, lm.Comol.Core.DomainModel.Helpers.MessageType.alert).ToString
                        Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.NoActionSelected
                            oLabel.CssClass = LTiconsBaseCssClass.Text & IIf(dto.Events.Where(Function(ev) ev.IsMandatory AndAlso ev.Error = er).Any(), lm.Comol.Core.DomainModel.Helpers.MessageType.alert, lm.Comol.Core.DomainModel.Helpers.MessageType.info).ToString
                    End Select
                Else
                    If dto.Events.Where(Function(ev) ev.Error <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.NoActionSelected AndAlso ev.Error <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.None).Any() Then
                        oLabel.CssClass = LTiconsBaseCssClass.Text & IIf(dto.Events.Where(Function(ev) ev.IsMandatory AndAlso ev.Error <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.None AndAlso ev.Error <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.NoActionSelected).Any(), lm.Comol.Core.DomainModel.Helpers.MessageType.error, lm.Comol.Core.DomainModel.Helpers.MessageType.alert).ToString
                    Else
                        oLabel.CssClass = LTiconsBaseCssClass.Text & IIf(dto.Events.Where(Function(ev) ev.IsMandatory AndAlso ev.Error = lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.NoActionSelected).Any(), lm.Comol.Core.DomainModel.Helpers.MessageType.alert, lm.Comol.Core.DomainModel.Helpers.MessageType.info).ToString
                    End If
                End If
                If dto.Errors.Count = 1 Then
                    oLabel.ToolTip = Resource.getValue("EventErrors.Verify.1")
                Else
                    oLabel.ToolTip = String.Format(Resource.getValue("EventErrors.Verify.n"), dto.Errors.Count)
                End If
                Dim oLiteral As Literal = e.Item.FindControl("LTmoduleErrors")
                For Each ev As lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent In dto.Events.Where(Function(ee) ee.Error <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.None).ToList()
                    oLiteral.Text &= "<p>" & GetTranslatedModuleActions(dto.ModuleCode).Where(Function(a) a.IdEvent = ev.IdEvent).Select(Function(a) a.Name).FirstOrDefault()
                    oLiteral.Text &= " =" & Resource.getValue("EventError." & ev.Error.ToString & "." & ev.IsMandatory) & "</p>"
                Next
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (RPTmodules.Items.Count = 0)
            If (RPTmodules.Items.Count = 0) Then
                Dim oLabel As Label = e.Item.FindControl("LBemptyNotificationSettingItems")
                Resource.setLabel(oLabel)
            End If
        End If
    End Sub
    Protected Sub RPTmoduleEvents_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim dto As lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent = DirectCast(e.Item.DataItem, lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent)
        Dim oControl As UC_TemplateSelector = e.Item.FindControl("CTRLselector")

        oControl.AllowSelect = dto.EditEnabled
        oControl.InitializeControl(dto.Permissions, lm.Comol.Core.Notification.Domain.NotificationChannel.Mail, dto.Context, dto.IdTemplate, dto.IdVersion, dto.Templates)
        Dim oLabel As Label = e.Item.FindControl("LBmoduleEventError")
        Dim oLiteral As Literal = e.Item.FindControl("LTmoduleEventError")
        oLabel.Visible = (dto.Error <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.None)

        Select Case dto.Error
            Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.TemplateRemoved, lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.VersionRemoved, lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.TemplateUnselected
                oLabel.CssClass = LTiconsBaseCssClass.Text & IIf(dto.IsMandatory, lm.Comol.Core.DomainModel.Helpers.MessageType.error, lm.Comol.Core.DomainModel.Helpers.MessageType.alert).ToString
            Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EventError.NoActionSelected
                oLabel.CssClass = LTiconsBaseCssClass.Text & IIf(dto.IsMandatory, lm.Comol.Core.DomainModel.Helpers.MessageType.alert, lm.Comol.Core.DomainModel.Helpers.MessageType.info).ToString
        End Select
        oLabel.ToolTip = Resource.getValue("EventError." & dto.Error.ToString & "." & dto.IsMandatory)
        oLiteral.Text = Resource.getValue("EventError." & dto.Error.ToString & "." & dto.IsMandatory)

        oLabel = e.Item.FindControl("LBmandatoryAction")
        oLabel.ToolTip = Resource.getValue("LBmandatoryAction.ToolTip")
    End Sub
    Protected Sub RPTmoduleEvents_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Me.CurrentPresenter.ReloadItems(SelectedItems.SelectMany(Function(i) i.Events))
    End Sub
    Protected Sub TemplateSelected(idTemplate As Long, idVersion As Long)
        CTRLmessages.Visible = False
        Dim items As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents) = SelectedItems
        If items.Any() Then
            Me.CurrentPresenter.ReloadItems(items.SelectMany(Function(i) i.Events).ToList())
        Else
            Me.CurrentPresenter.ReloadItems(New List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent))
        End If
    End Sub
#End Region


    Private Sub LNBsaveSettingsUp_Click(sender As Object, e As System.EventArgs) Handles LNBsaveSettingsUp.Click
        Dim items As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvents) = SelectedItems
        If items.Any() Then
            Me.CurrentPresenter.SaveSettings(items.SelectMany(Function(i) i.Events).ToList())
        Else
            Me.CurrentPresenter.SaveSettings(New List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent))
        End If
    End Sub

End Class