Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel.Languages

Public Class MessageRecipients
    Inherits PageBase
    Implements IViewMessageRecipients

#Region "Context"
    Private _Presenter As MessageRecipientsPresenter
    Private ReadOnly Property CurrentPresenter() As MessageRecipientsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MessageRecipientsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Preload"
    Private ReadOnly Property PreloadModuleObject As ModuleObject Implements IViewMessageRecipients.PreloadModuleObject
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
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewMessageRecipients.PreloadIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunity")) AndAlso IsNumeric(Request.QueryString("idCommunity")) Then
                Return CLng(Request.QueryString("idCommunity"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadModuleCode As String Implements IViewMessageRecipients.PreloadModuleCode
        Get
            Return Request.QueryString("code")
        End Get
    End Property
    Private ReadOnly Property PreloadIdModule As Integer Implements IViewMessageRecipients.PreloadIdModule
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idModule")) AndAlso IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdMessage As Long Implements IViewMessageRecipients.PreloadIdMessage
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idMessage")) AndAlso IsNumeric(Request.QueryString("idMessage")) Then
                Return CLng(Request.QueryString("idMessage"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromModulePermissions As Long Implements IViewMessageRecipients.PreloadFromModulePermissions
        Get
            If String.IsNullOrEmpty(Request.QueryString("mPrmCnt")) OrElse Not IsNumeric(Request.QueryString("mPrmCnt")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("mPrmCnt"))
            End If
        End Get
    End Property
    Private ReadOnly Property AnonymousUserTranslation As String Implements IViewMessageRecipients.AnonymousUserTranslation
        Get
            Return Resource.getValue("UnknownUserName")
        End Get
    End Property
    Private ReadOnly Property UnknownUserTranslation As String Implements IViewMessageRecipients.UnknownUserTranslation
        Get
            Return Resource.getValue("RemovedUserName")
        End Get
    End Property
#End Region

#Region "Current"
    Private Property CurrentModuleObject As ModuleObject Implements IViewMessageRecipients.CurrentModuleObject
        Get
            If Not IsNothing(ViewState("CurrentModuleObject")) Then
                Try
                    Return DirectCast(ViewState("CurrentModuleObject"), ModuleObject)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(value As ModuleObject)
            ViewState("CurrentModuleObject") = value
        End Set
    End Property
    Private Property CurrentIdCommunity As Integer Implements IViewMessageRecipients.CurrentIdCommunity
        Get
            Return ViewStateOrDefault("CurrentIdCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdCommunity") = value
        End Set
    End Property
    Private Property CurrentModuleCode As String Implements IViewMessageRecipients.CurrentModuleCode
        Get
            Return ViewStateOrDefault("CurrentModuleCode", "")
        End Get
        Set(value As String)
            ViewState("CurrentModuleCode") = value
        End Set
    End Property
    Private Property CurrentIdModule As Integer Implements IViewMessageRecipients.CurrentIdModule
        Get
            Return ViewStateOrDefault("CurrentIdModule", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdModule") = value
        End Set
    End Property
    Private Property CurrentIdMessage As Long Implements IViewMessageRecipients.CurrentIdMessage
        Get
            Return ViewStateOrDefault("CurrentIdMessage", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdMessage") = value
        End Set
    End Property
    Private Property CurrentSessionId As Guid Implements IViewMessageRecipients.CurrentSessionId
        Get
            Return ViewStateOrDefault("CurrentSessionId", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("CurrentSessionId") = value
        End Set
    End Property
    Private Property ContainerContext As lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext Implements IViewMessageRecipients.ContainerContext
        Get
            Return ViewStateOrDefault("ContainerContext", New lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext())
        End Get
        Set(value As lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext)
            ViewState("ContainerContext") = value
        End Set
    End Property
#End Region

#Region "Current - Filters - Users"
    Private ReadOnly Property GetTranslatedRoles As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) Implements IViewMessageRecipients.GetTranslatedRoles
        Get
            Return COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).Select(Function(r) New TranslatedItem(Of Integer) With {.Id = r.ID, .Translation = r.Name}).ToList()
        End Get
    End Property
    Private ReadOnly Property GetTranslatedProfileTypes As List(Of lm.Comol.Core.DomainModel.TranslatedItem(Of Integer)) Implements IViewMessageRecipients.GetTranslatedProfileTypes
        Get
            Return (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList.Select(Function(r) New TranslatedItem(Of Integer) With {.Id = r.ID, .Translation = r.Descrizione}).OrderBy(Function(t) t.Translation).ToList()
        End Get
    End Property
    Protected Friend Property IsAgencyColumnVisible As Boolean Implements IViewMessageRecipients.IsAgencyColumnVisible
        Get
            Return Me.ViewStateOrDefault("IsAgencyColumnVisible", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("IsAgencyColumnVisible") = value
        End Set
    End Property
    Private Property UserCurrentOrderBy As lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder Implements IViewMessageRecipients.UserCurrentOrderBy
        Get
            Return ViewStateOrDefault("UserCurrentOrderBy", lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder.ByUser)
        End Get
        Set(value As lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder)
            Me.ViewState("UserCurrentOrderBy") = value
        End Set
    End Property
    Private Property SelectedSearchBy As lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy Implements IViewMessageRecipients.SelectedSearchBy
        Get
            If DDLsearchUserBy.SelectedIndex = -1 Then
                Return lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy).GetByString(Me.DDLsearchUserBy.SelectedValue, lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy.All)
            End If
        End Get
        Set(value As lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy)
            If DDLsearchUserBy.SelectedIndex > -1 AndAlso Me.DDLsearchUserBy.SelectedValue <> value.ToString AndAlso Not IsNothing(Me.DDLsearchUserBy.Items.FindByValue(value.ToString)) Then
                Me.DDLsearchUserBy.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentIdProfileType As Integer Implements IViewMessageRecipients.CurrentIdProfileType
        Get
            If DDLprofileTypes.SelectedIndex = -1 Then
                Return -1
            Else
                Return CInt(DDLprofileTypes.SelectedValue)
            End If
        End Get
        Set(value As Integer)
            If DDLprofileTypes.SelectedIndex > -1 AndAlso Me.DDLprofileTypes.SelectedValue <> value.ToString AndAlso Not IsNothing(Me.DDLprofileTypes.Items.FindByValue(value.ToString)) Then
                Me.DDLprofileTypes.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentIdRole As Integer Implements IViewMessageRecipients.CurrentIdRole
        Get
            If DDLroles.SelectedIndex = -1 Then
                Return -1
            Else
                Return CInt(DDLroles.SelectedValue)
            End If
        End Get
        Set(value As Integer)
            If DDLroles.SelectedIndex > -1 AndAlso Me.DDLroles.SelectedValue <> value.ToString AndAlso Not IsNothing(Me.DDLroles.Items.FindByValue(value.ToString)) Then
                Me.DDLroles.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property SelectedIdAgency As Long Implements IViewMessageRecipients.SelectedIdAgency
        Get
            If DDLcommunityAgencies.SelectedIndex = -1 Then
                Return -1
            Else
                Return CLng(DDLcommunityAgencies.SelectedValue)
            End If
        End Get
        Set(value As Long)
            If DDLcommunityAgencies.Items.Count > 0 AndAlso Not IsNothing(DDLcommunityAgencies.Items.FindByValue(value)) Then
                Me.DDLcommunityAgencies.SelectedValue = value
            End If
        End Set
    End Property
    Private Property CurrentFilter As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters Implements IViewMessageRecipients.CurrentFilter
        Get
            Dim filter As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters = ViewStateOrDefault("CurrentFilter", SelectedFilter)
            If (filter.ProfyleTypeTranslations.Count = 0) Then
                filter.ProfyleTypeTranslations = GetTranslatedProfileTypes.ToDictionary(Function(t) t.Id, Function(k) k.Translation)
            End If
            If (filter.RoleTranslations.Count = 0) Then
                filter.RoleTranslations = GetTranslatedRoles.ToDictionary(Function(t) t.Id, Function(k) k.Translation)
            End If
            Return filter
        End Get
        Set(value As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters)
            Me.TXBsearchBy.Text = value.Value
            SelectedIdAgency = value.IdAgency
            SelectedSearchBy = value.SearchBy
            CurrentIdProfileType = value.IdProfileType
            CurrentIdRole = value.IdRole

            Me.ViewState("CurrentFilter") = value
        End Set
    End Property
    Private ReadOnly Property SelectedFilter As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters Implements IViewMessageRecipients.SelectedFilter
        Get
            Dim oFilter As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters = ViewStateOrDefault("CurrentFilter", New lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters() With {.Ascending = True, .OrderBy = lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder.ByUser})
            If (oFilter.ProfyleTypeTranslations.Count = 0) Then
                oFilter.ProfyleTypeTranslations = GetTranslatedProfileTypes.ToDictionary(Function(t) t.Id, Function(k) k.Translation)
            End If
            If (oFilter.RoleTranslations.Count = 0) Then
                oFilter.RoleTranslations = GetTranslatedRoles.ToDictionary(Function(t) t.Id, Function(k) k.Translation)
            End If
            oFilter.SearchBy = SelectedSearchBy
            oFilter.IdAgency = SelectedIdAgency
            oFilter.Ascending = Ascending
            oFilter.OrderBy = UserCurrentOrderBy
            If Not String.IsNullOrEmpty(CurrentSearchValue) Then
                oFilter.Value = CurrentSearchValue.Trim
            End If
            oFilter.StartWith = CurrentStartWith
            oFilter.IdProfileType = CurrentIdProfileType
            oFilter.IdRole = CurrentIdRole
            Return oFilter
        End Get
    End Property
    Private Property CurrentSearchValue As String Implements IViewMessageRecipients.CurrentSearchValue
        Get
            Return Me.TXBsearchBy.Text
        End Get
        Set(value As String)
            Me.TXBsearchBy.Text = value
        End Set
    End Property
#End Region

#Region "Current - Filters"
    Private Property LoadedNoUsers As Boolean Implements IViewMessageRecipients.LoadedNoUsers
        Get
            Return ViewStateOrDefault("LoadedNoUsers", False)
        End Get
        Set(value As Boolean)
            ViewState("LoadedNoUsers") = value
        End Set
    End Property
    Private Property Ascending As Boolean Implements IViewMessageRecipients.Ascending
        Get
            Return ViewStateOrDefault("Ascending", True)
        End Get
        Set(value As Boolean)
            ViewState("Ascending") = value
        End Set
    End Property
    Private Property CurrentStartWith As String Implements IViewMessageRecipients.CurrentStartWith
        Get
            Return CTRLalphabetSelector.SelectedItem
        End Get
        Set(value As String)
            CTRLalphabetSelector.SelectedItem = value
        End Set
    End Property
    Private Property AvailableColumns As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.ColumnMessageGrid) Implements IViewMessageRecipients.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.ColumnMessageGrid))
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.ColumnMessageGrid))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewMessageRecipients.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = PageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            'Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Private Property PageSize As Integer Implements IViewMessageRecipients.PageSize
        Get
            Return ViewStateOrDefault("PageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("PageSize") = value
        End Set
    End Property
#End Region

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
    Public ReadOnly Property isColumnVisible(ByVal column As lm.Comol.Core.BaseModules.TemplateMessages.Domain.ColumnMessageGrid) As Boolean
        Get
            Return AvailableColumns.Contains(column)
        End Get
    End Property
    Public ReadOnly Property GetOpenCssClass(identifier As System.Guid) As String
        Get
            Return IIf(identifier = OpenIdentifier, "expanded", "")
        End Get
    End Property
    Private Property OpenIdentifier As System.Guid
        Get
            Return ViewStateOrDefault("OpenIdentifier", System.Guid.Empty)
        End Get
        Set(value As System.Guid)
            ViewState("OpenIdentifier") = value
        End Set
    End Property
    Protected Function MessageCssClass(ByVal item As lm.Comol.Core.Mail.Messages.dtoDisplayMessage) As String
        Return ""
    End Function
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Pager
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
            Master.ServiceTitle = .getValue("serviceTitle.MessageRecipients." & PreloadModuleCode)
            ''." & PreloadModuleCode)
            Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(LBcommunityAgencyFilter_t)
            .setLabel(LBcommunityRole_t)
            .setLabel(LBprofileType_t)
            .setLabel(LBsearchUserByFilter_t)
            .setButton(BTNapplyFilters, True)
            .setButton(BTNcloseMailMessageWindow, True)
            DVpreview.Attributes.Add("title", .getValue("DVpreview.title"))
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    'Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewBase.SendUserAction
    '    PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, "0"), InteractionType.UserWithLearningObject)
    'End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewMessageRecipients.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer, moduleCode As String) Implements IViewMessageRecipients.DisplayNoPermission
        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        End Select

        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewMessageRecipients.DisplaySessionTimeout
        Dim idCommunity As Integer = CurrentIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub DisplayMessageInfo(message As lm.Comol.Core.Mail.Messages.MailMessage) Implements IViewMessageRecipients.DisplayMessageInfo
        Dim translated As String = Resource.getValue("InfoSent")
        If translated.Contains("{0}") Then
            If IsNothing(message.CreatedBy) OrElse (Not IsNothing(message.CreatedBy) AndAlso message.CreatedBy.TypeID = UserTypeStandard.Guest) Then
                translated = String.Format(translated, UnknownUserTranslation)
            Else
                translated = String.Format(translated, message.CreatedBy.SurnameAndName)
            End If
            translated &= " " & String.Format(Resource.getValue("InfoSentOn"), message.CreatedOn.ToString(Resource.CultureInfo.DateTimeFormat.ShortDatePattern))
            translated &= " " & String.Format(Resource.getValue("InfoSentAt"), message.CreatedOn.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern))
            Master.ServiceTitle = translated
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("InfoSentAt"), message.CreatedOn.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern))
        End If
    End Sub
#End Region

    Private Function GetModulePermissions(moduleCode As String, idModule As Integer, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Implements IViewMessageRecipients.GetModulePermissions
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

#Region "Filters"
    Private Sub InitializeWordSelector(availableWords As List(Of String)) Implements IViewMessageRecipients.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(availableWords)
    End Sub
    Private Sub InitializeWordSelector(availableWords As List(Of String), activeWord As String) Implements IViewMessageRecipients.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(availableWords, activeWord)
    End Sub
    Private Sub LoadAgencies(items As Dictionary(Of Long, String), idDefaultAgency As Long) Implements IViewMessageRecipients.LoadAgencies
        Me.DDLcommunityAgencies.Items.Clear()
        Me.DDLcommunityAgencies.DataSource = items
        Me.DDLcommunityAgencies.DataValueField = "Key"
        Me.DDLcommunityAgencies.DataTextField = "Value"
        Me.DDLcommunityAgencies.DataBind()

        If items.Count = 0 Then
            Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.ignorefilter"), -1))
        Else
            If items.Count > 1 Then
                Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.all"), -2))
            End If
            Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.noemployeee"), -3))
            Me.DDLcommunityAgencies.Items.Insert(0, New ListItem(Me.Resource.getValue("agencies.ignorefilter"), -1))
        End If
        Me.LBcommunityAgencyFilter_t.Visible = True
        Me.DDLcommunityAgencies.Visible = True
    End Sub
    Private Sub LoadSearchProfilesBy(list As List(Of lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy), defaultSearch As lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy) Implements IViewMessageRecipients.LoadSearchProfilesBy
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SearchProfilesBy." & s.ToString)}).ToList

        Me.DDLsearchUserBy.DataSource = translations
        Me.DDLsearchUserBy.DataValueField = "Id"
        Me.DDLsearchUserBy.DataTextField = "Translation"
        Me.DDLsearchUserBy.DataBind()

        Me.SelectedSearchBy = defaultSearch
    End Sub
    Private Sub UnLoadAgencies() Implements IViewMessageRecipients.UnLoadAgencies
        Me.DDLcommunityAgencies.Visible = False
        Me.LBcommunityAgencyFilter_t.Visible = False
    End Sub
    Private Sub LoadAvailableProfileType(items As List(Of Integer), selected As Integer) Implements IViewMessageRecipients.LoadAvailableProfileType
        Dim types As List(Of TranslatedItem(Of Integer)) = GetTranslatedProfileTypes.Where(Function(r) items.Contains(r.Id)).ToList()
        Me.DDLprofileTypes.DataSource = types
        Me.DDLprofileTypes.DataValueField = "ID"
        Me.DDLprofileTypes.DataTextField = "Descrizione"
        Me.DDLprofileTypes.DataBind()

        Me.DDLprofileTypes.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLprofileTypes." & -1), -1))

        Me.LBprofileType_t.Visible = True
        Me.DDLprofileTypes.Visible = True
        Me.CurrentIdProfileType = selected
    End Sub
    Private Sub LoadAvailableRoles(items As List(Of Integer), selected As Integer) Implements IViewMessageRecipients.LoadAvailableRoles
        Dim roles As List(Of TranslatedItem(Of Integer)) = GetTranslatedRoles.Where(Function(r) items.Contains(r.Id)).ToList()

        Me.DDLroles.DataSource = roles
        Me.DDLroles.DataValueField = "ID"
        Me.DDLroles.DataTextField = "Translation"
        Me.DDLroles.DataBind()

        Me.DDLroles.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLroles." & -1), -1))

        Me.LBcommunityRole_t.Visible = True
        Me.DDLroles.Visible = True
        Me.CurrentIdRole = selected
    End Sub
#End Region

    Private Function HasModulePermissions(moduleCode As String, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements IViewMessageRecipients.HasModulePermissions
        Dim result As Boolean = False
        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing(permissions)
                End If
                result = wModule.ManageRoom
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Dim cModule As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper
                If idCommunity = 0 Then
                    cModule = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.CreatePortalmodule(profileType)
                Else
                    cModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper(permissions)
                End If
                result = cModule.Administration OrElse cModule.ManageCallForPapers
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Dim rModule As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership
                If idCommunity = 0 Then
                    rModule = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.CreatePortalmodule(profileType)
                Else
                    rModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership(permissions)
                End If
                result = rModule.Administration OrElse rModule.ManageBaseForPapers
            Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                Dim epModule As lm.Comol.Modules.EduPath.Domain.ModuleEduPath
                If idCommunity = 0 Then
                    epModule = lm.Comol.Modules.EduPath.Domain.ModuleEduPath.CreatePortalmodule(profileType)
                Else
                    epModule = New lm.Comol.Modules.EduPath.Domain.ModuleEduPath(permissions)
                End If
                result = epModule.Administration
                If Not IsNothing(obj) AndAlso Not result Then

                End If
            Case COL_Questionario.ModuleQuestionnaire.UniqueID
                Dim qModule As COL_Questionario.ModuleQuestionnaire
                If idCommunity = 0 Then
                    qModule = COL_Questionario.ModuleQuestionnaire.CreatePortalmodule(profileType)
                Else
                    qModule = New COL_Questionario.ModuleQuestionnaire(permissions)
                End If
                result = qModule.Administration
                If Not IsNothing(obj) AndAlso Not result Then

                End If
        End Select
        Return result
    End Function

    Private Sub DisplayUnknownMessage() Implements IViewMessageRecipients.DisplayUnknownMessage
        Me.DVpreview.Visible = False
        Me.MLVcontent.SetActiveView(VIWdefault)
        LTdefaultMessage.Text = Resource.getValue("DisplayUnknownMessage")
    End Sub

    Private Sub DisplayNoUsersFound() Implements IViewMessageRecipients.DisplayNoUsersFound
        LoadRecipients(New List(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessage))
    End Sub
    Private Sub LoadRecipients(recipients As List(Of lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessage)) Implements IViewMessageRecipients.LoadRecipients
        Me.DVpreview.Visible = False
        RPTrecipients.DataSource = recipients
        RPTrecipients.DataBind()
    End Sub
    Private Sub DisplayMessagePreview(allowSendMail As Boolean, languageCode As String, tContent As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, modules As List(Of String), mSettings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings, idCommunity As Integer, Optional obj As lm.Comol.Core.DomainModel.ModuleObject = Nothing) Implements IViewMessageRecipients.DisplayMessagePreview
        Me.DVpreview.Visible = True
        Me.CTRLmailpreview.InitializeControlForPreview(languageCode, tContent, modules, mSettings, idCommunity, obj)
    End Sub
#End Region

    Private Sub AddTemplate_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

#Region "Control"
    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem
        Dim dto As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters = Me.SelectedFilter
        dto.StartWith = letter
        Me.CurrentStartWith = letter
        Me.CurrentFilter = dto
        Me.CurrentPresenter.LoadRecipients(CurrentIdMessage, dto, 0, PageSize, False)
    End Sub
    Private Sub BTNapplyFilters_Click(sender As Object, e As System.EventArgs) Handles BTNapplyFilters.Click
        Dim dto As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters = Me.SelectedFilter
        Me.CurrentFilter = dto
        Me.CurrentPresenter.LoadRecipients(CurrentIdMessage, dto, 0, PageSize, False)
    End Sub
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.CurrentPresenter.LoadRecipients(CurrentIdMessage, CurrentFilter, Me.Pager.PageIndex, PageSize, False)
    End Sub

    Private Sub RPTrecipients_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTrecipients.ItemCommand
        Select Case e.CommandName
            Case "orderby"
                Dim filter As lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters = CurrentFilter
                Ascending = e.CommandArgument.contains("True")
                filter.Ascending = e.CommandArgument.contains("True")
                filter.OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder).GetByString(e.CommandArgument.replace("." & filter.Ascending.ToString, ""), lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder.ByUser)
                Me.CurrentPresenter.LoadRecipients(CurrentIdMessage, filter, Me.Pager.PageIndex, Me.Pager.PageSize, False)
            Case "display"
                Dim idRecipientMessage As Long = CLng(e.CommandArgument)
                Me.CurrentPresenter.SelectMessage(idRecipientMessage)
        End Select
    End Sub
    Private Sub RPTrecipients_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrecipients.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessage = DirectCast(e.Item.DataItem, lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessage)

            Dim oLiteral As Literal = e.Item.FindControl("LTdisplayName")
            If String.IsNullOrEmpty(dto.DisplayName) Then
                oLiteral.Text = dto.MailAddress
            Else
                oLiteral.Text = dto.DisplayName
            End If
            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBdisplayUserMessage")
            oLinkbutton.CommandArgument = dto.IdRecipient
            Resource.setLinkButton(oLinkbutton, False, True)
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (RPTrecipients.Items.Count = 0)
            If (RPTrecipients.Items.Count = 0) Then
                Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                oTableCell.ColSpan = 2 + AvailableColumns.Where(Function(c) c = lm.Comol.Core.BaseModules.TemplateMessages.Domain.ColumnMessageGrid.AgencyName OrElse c = lm.Comol.Core.BaseModules.TemplateMessages.Domain.ColumnMessageGrid.ProfileType OrElse c = lm.Comol.Core.BaseModules.TemplateMessages.Domain.ColumnMessageGrid.Role).Count
                Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                If LoadedNoUsers Then
                    oLabel.Text = Resource.getValue("NoUsersForSelection")
                Else
                    Resource.setLabel(oLabel)
                End If
            End If
            If RPTrecipients.Items.Count < 2 Then
                Dim oLinkButton As LinkButton
                For Each name As String In [Enum].GetNames(GetType(lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder))
                    oLinkButton = RPTrecipients.Controls(0).FindControl("LNBorder" & name & "Up")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = False
                    End If
                    oLinkButton = RPTrecipients.Controls(0).FindControl("LNBorder" & name & "Down")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = False
                    End If
                Next
            ElseIf RPTrecipients.Items.Count > 2 AndAlso RPTrecipients.Controls(0).FindControl("LNBorder" & lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder.ByUser.ToString & "Up").Visible = False Then
                Dim oLinkButton As LinkButton
                For Each name As String In [Enum].GetNames(GetType(lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder))
                    oLinkButton = RPTrecipients.Controls(0).FindControl("LNBorder" & name & "Up")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = True
                    End If
                    oLinkButton = RPTrecipients.Controls(0).FindControl("LNBorder" & name & "Down")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.Visible = True
                    End If
                Next
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then

            Dim oLiteral As Literal = e.Item.FindControl("LTrecipientHeaderName_t")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTmessageRecipientActionsHeader_t")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTuserAgencyNameHeader_t")
            Resource.setLiteral(oLiteral)
            'oLabel = e.Item.FindControl("LBmailHeader_t")
            'Resource.setLabel(oLabel)
            'Dim oLiteral As Literal = e.Item.FindControl("LTuserPartecipantType_t")
            'Resource.setLiteral(oLiteral)
            Dim oLinkButton As LinkButton = Nothing
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Core.BaseModules.MailSender.UserByMessagesOrder))
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Up")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("UserByMessagesOrder.Ascending") & Resource.getValue("UserByMessagesOrder." & name)
                End If
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Down")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("UserByMessagesOrder.Descending") & Resource.getValue("UserByMessagesOrder." & name)
                End If
            Next
        End If
    End Sub

    Private Sub BTNcloseMailMessageWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailMessageWindow.Click
        Me.DVpreview.Visible = False
    End Sub
#End Region

    
End Class