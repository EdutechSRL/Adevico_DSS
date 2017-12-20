Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Domain
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditTemplatePermission
    Inherits PageBase
    Implements IViewEditPermissions

#Region "Context"
    Private _Presenter As EditPermissionsPresenter
    Private ReadOnly Property CurrentPresenter() As EditPermissionsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditPermissionsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property UnknownUserTranslation As String Implements IViewEditPermissions.UnknownUserTranslation
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property
    Private ReadOnly Property UnknownCommunityTranslation As String Implements IViewEditPermissions.UnknownCommunityTranslation
        Get
            Return Me.Resource.getValue("UnknownCommunity")
        End Get
    End Property
    Private ReadOnly Property Portalname As String Implements IViewEditPermissions.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadBackUrl As String Implements IViewBaseEdit.PreloadBackUrl
        Get
            If PreloadCurrentSessionId = Guid.Empty Then
                Return Request.QueryString("BackUrl")
            Else
                Return Me.Session(lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.SessionName(PreloadCurrentSessionId))
            End If

        End Get
    End Property
    Private ReadOnly Property PreloadPreview As Boolean Implements IViewBaseEdit.PreloadPreview
        Get
            Return (Request.QueryString("preview") = "true")
        End Get
    End Property
    Private ReadOnly Property PreloadIdTemplate As Long Implements IViewBaseEdit.PreloadIdTemplate
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdTemplate")) AndAlso IsNumeric(Request.QueryString("IdTemplate")) Then
                Return CLng(Request.QueryString("IdTemplate"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdVersion As Long Implements IViewBaseEdit.PreloadIdVersion
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
    Private ReadOnly Property PreloadFromIdCommunity As Integer Implements IViewBaseEdit.PreloadFromIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunityCnt")) AndAlso IsNumeric(Request.QueryString("idCommunityCnt")) Then
                Return CLng(Request.QueryString("idCommunityCnt"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromModuleCode As String Implements IViewBaseEdit.PreloadFromModuleCode
        Get
            Return Request.QueryString("mCodeCnt")
        End Get
    End Property
    Private ReadOnly Property PreloadFromModulePermissions As Long Implements IViewBaseEdit.PreloadFromModulePermissions
        Get
            If String.IsNullOrEmpty(Request.QueryString("mPrmCnt")) OrElse Not IsNumeric(Request.QueryString("mPrmCnt")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("mPrmCnt"))
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadCurrentSessionId As Guid Implements IViewBaseEdit.PreloadCurrentSessionId
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
    Private Property AllowSave As Boolean Implements IViewEditPermissions.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property ForPortal As Boolean Implements IViewEditPermissions.ForPortal
        Get
            Return ViewStateOrDefault("ForPortal", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ForPortal") = value
        End Set
    End Property
    Private Property IdTemplateCommunity As Integer Implements IViewBaseEdit.IdTemplateCommunity
        Get
            Return ViewStateOrDefault("IdTemplateCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateCommunity") = value
        End Set
    End Property
    Private Property IdTemplateModule As Integer Implements IViewBaseEdit.IdTemplateModule
        Get
            Return ViewStateOrDefault("IdTemplateModule", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateModule") = value
        End Set
    End Property
    Private Property IdTemplate As Long Implements IViewBaseEdit.IdTemplate
        Get
            Return ViewStateOrDefault("IdTemplate", 0)
        End Get
        Set(value As Long)
            ViewState("IdTemplate") = value
        End Set
    End Property
    Private Property IdVersion As Long Implements IViewBaseEdit.IdVersion
        Get
            Return ViewStateOrDefault("IdVersion", 0)
        End Get
        Set(value As Long)
            ViewState("IdVersion") = value
        End Set
    End Property
    Protected Property InputReadOnly As Boolean Implements IViewBaseEdit.InputReadOnly
        Get
            Return ViewStateOrDefault("InputReadOnly", False)
        End Get
        Set(value As Boolean)
            ViewState("InputReadOnly") = value
            If value Then
                Me.BTNsavePermissionsBottom.Visible = Not value
                Me.BTNsavePermissionsTop.Visible = Not value
                LIaddToolbar.Visible = Not value
            End If
        End Set
    End Property
    Private Property CurrentType As TemplateType Implements IViewBaseEdit.CurrentType
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
    Private Property IdEditingCommunity As Integer Implements IViewEditPermissions.IdEditingCommunity
        Get
            Return ViewStateOrDefault("IdEditingCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdEditingCommunity") = value
        End Set
    End Property
    Private Property SelectedIdUsers As List(Of Integer) Implements IViewEditPermissions.SelectedIdUsers
        Get
            Return ViewStateOrDefault("SelectedIdUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("SelectedIdUsers") = value
        End Set
    End Property
    Private ReadOnly Property MaxDisplayUsers As Integer Implements IViewEditPermissions.MaxDisplayUsers
        Get
            Dim maxItems As Integer = 5

            If Not String.IsNullOrEmpty(LTdataMax.Text) AndAlso IsNumeric(LTdataMax.Text) Then
                Integer.TryParse(LTdataMax.Text, maxItems)
            End If
            Return maxItems
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

#Region "Internal"
    Private Property TranslatedModules As List(Of PlainService)
        Get
            Return ViewStateOrDefault("TranslatedModules", ManagerService.ListSystemTranslated(PageUtility.LinguaID))
        End Get
        Set(value As List(Of PlainService))
            ViewState("TranslatedModules") = value
        End Set
    End Property
    Private _Translations As List(Of TranslatedItem(Of String))
    Private ReadOnly Property TranslateNotificationModules As List(Of TranslatedItem(Of String))
        Get
            If _Translations Is Nothing Then
                _Translations = TranslatedModules.Select(Function(m) New TranslatedItem(Of String) With {.Id = m.Code, .Translation = m.Name}).ToList()
                'For Each code As String In codes.Where(Function(c) Not _Translations.Where(Function(t) t.Id = c).Any()).ToList()
                '    _Translations.Add(New TranslatedItem(Of String) With {.Id = code, .Translation = Resource.getValue("Module." & code)})
                'Next
            End If
            Return _Translations
        End Get
    End Property
#End Region
    Private Sub EditCallAvailability_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
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
            Master.ServiceTitle = .getValue("serviceTitle.EditPermissions")
            Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(LBnoTemplate)

            .setHyperLink(HYPbackTop, False, True)
            .setHyperLink(HYPbackBottom, False, True)
            .setButton(BTNsavePermissionsBottom, True, , , True)
            .setButton(BTNsavePermissionsTop, True, , , True)


            .setButton(BTNclosePersonTypesAssignments, True)
            .setButton(BTNsavePersonTypesAssignments, True)

            .setLabel(LBcommunityName_t)
            .setButton(BTNcloseRoleAssignments, True)
            .setButton(BTNsaveRoleAssignments, True)

            .setLiteral(LTtoolDescription)
            .setLabel(LBpersonTypesDescription)
            .setLabel(LBselectRolesDescription)

            CTRLcommunitySelectorHeader.ModalTitle = DialogTitleTranslation(2)
            CTRLaddCommunity.Description = Resource.getValue("LBselectCommunityDescription.Text")

            If Not Page.IsPostBack AndAlso Not BTNaddCommunity.CssClass.Contains(CTRLcommunitySelectorHeader.ClientSideOpenDialogCssClass) Then
                BTNaddCommunity.CssClass &= " " & CTRLcommunitySelectorHeader.ClientSideOpenDialogCssClass
            End If
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"

#Region "Actions"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewBase.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, IdTemplate), InteractionType.UserWithLearningObject)
    End Sub
#End Region

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
    Private Sub DisplayUnknownTemplate() Implements IViewEditPermissions.DisplayUnknownTemplate
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayUnknownTemplate"), Helpers.MessageType.alert)
        Me.MLVpermissions.SetActiveView(VIWempty)
    End Sub
    Private Sub UnableToReadUrlSettings() Implements IViewEditPermissions.UnableToReadUrlSettings
        Me.MLVpermissions.SetActiveView(VIWempty)
        Me.LBnoTemplate.Text = Resource.getValue("UnableToReadUrlSettings")
    End Sub
    Private Sub HideUserMessage() Implements IViewEditPermissions.HideUserMessage
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayNoAssignments() Implements IViewEditPermissions.DisplayNoAssignments
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoAssignments"), Helpers.MessageType.info)
    End Sub
    Private Sub DisplayCommunityAssignmentAdded(name As String) Implements IViewEditPermissions.DisplayCommunityAssignmentAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayCommunityAssignmentAdded"), name), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAssignmentsAdded() Implements IViewEditPermissions.DisplayCommunityAssignmentsAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayCommunityAssignmentsAdded"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAssignmentDeleted(name As String) Implements IViewEditPermissions.DisplayCommunityAssignmentDeleted
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayCommunityAssignmentDeleted"), name), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAddingError() Implements IViewEditPermissions.DisplayCommunityAddingError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayCommunityAddingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayCommunityAssignmentErrorSaving() Implements IViewEditPermissions.DisplayCommunityAssignmentErrorSaving
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayCommunityAssignmentErrorSaving"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayCommunityDeletingError() Implements IViewEditPermissions.DisplayCommunityDeletingError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayCommunityDeletingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayPortalPermissionsSaved() Implements IViewEditPermissions.DisplayPortalPermissionsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayPortalPermissionsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayPortalPermissionsErrorSaving() Implements IViewEditPermissions.DisplayPortalPermissionsErrorSaving
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayPortalPermissionsErrorSaving"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUserAssignmentsAdded() Implements IViewEditPermissions.DisplayUserAssignmentsAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUserAssignmentsAdded"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayUserAddingError() Implements IViewEditPermissions.DisplayUserAddingError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUserAddingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUserAssigmentsSavingError() Implements IViewEditPermissions.DisplayUserAssigmentsSavingError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUserAssigmentsSavingError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUserAssignmentsSaved() Implements IViewEditPermissions.DisplayUserAssignmentsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUserAssignmentsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAssignmentSaved(name As String) Implements IViewEditPermissions.DisplayCommunityAssignmentSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayCommunityAssignmentSaved"), name), Helpers.MessageType.success)
    End Sub


    Private Sub SetBackUrl(url As String) Implements IViewEditPermissions.SetBackUrl
        Me.HYPbackBottom.Visible = True
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackTop.Visible = True
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
    End Sub

    Private Sub LoadWizardSteps(idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep))) Implements IViewEditPermissions.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(IdTemplate, IdVersion, IdTemplateCommunity, PreloadFromIdCommunity, PreloadFromModuleCode, PreloadFromModulePermissions, CurrentType, Ownership, PreloadCurrentSessionId, GetEncodedBackUrl(), steps, PreloadPreview)
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewEditPermissions.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
#End Region
    Private Function HasPermissionForObject(source As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements IViewEditPermissions.HasPermissionForObject
        Return False
    End Function
    Private Function GetModulePermissions(moduleCode As String, idModule As Integer, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Implements IViewBaseEdit.GetModulePermissions
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
#Region "Assignments"
    Private Sub LoadAvailableAssignments(items As List(Of PermissionType)) Implements IViewEditPermissions.LoadAvailableAssignments
        LIaddToolbar.Visible = items.Any
        If items.Any Then
            If items.Contains(PermissionType.ProfileType) Then
                Me.LIaddProfile.Visible = True
                Me.BTNaddProfile.Visible = True
                Resource.setButton(BTNaddProfile, True)
                Me.LTaddProfile.Text = BTNaddProfile.Text
                Resource.setLiteral(LTaddProfileDescription)
            ElseIf items.Contains(PermissionType.Person) Then
                Me.LIaddUserFromCommunity.Visible = True
                Me.BTNaddUserFromCommunity.Visible = True
                Resource.setButton(BTNaddUserFromCommunity, True)
                Me.LTaddUserFromCommunity.Text = BTNaddUserFromCommunity.Text
                Resource.setLiteral(LTaddUserFromCommunityDescription)
            End If

            If items.Contains(PermissionType.Community) Then
                Me.LIaddCommunity.Visible = True
                Me.BTNaddCommunity.Visible = True
                Resource.setButton(BTNaddCommunity, True)
                Me.LTaddCommunity.Text = BTNaddCommunity.Text
                Resource.setLiteral(LTaddCommunityDescription)

            End If
        End If
    End Sub
    Private Function GetTranslatedModules() As Dictionary(Of String, String) Implements IViewEditPermissions.GetTranslatedModules
        Return TranslateNotificationModules.ToDictionary(Function(t) t.Id, Function(t) t.Translation)
    End Function
    Private Function GetTranslatedProfileTypes() As Dictionary(Of Integer, String) Implements IViewEditPermissions.GetTranslatedProfileTypes
        Return (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Order By o.Descrizione Select o).ToDictionary(Function(t) t.ID, Function(t) t.Descrizione)
    End Function
    Private Function GetTranslatedRoles() As Dictionary(Of Integer, String) Implements IViewEditPermissions.GetTranslatedRoles
        Return COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToDictionary(Function(r) r.ID, Function(r) r.Name)
    End Function

    Private Function GetPermissions() As List(Of dtoTemplateAssignment) Implements IViewEditPermissions.GetPermissions
        Dim items As New List(Of dtoTemplateAssignment)
        items.AddRange(GetPortalPermissions)
        items.AddRange(GetRolePermissions)
        items.AddRange(GetPersonPermissions)
        Return items
    End Function

#Region "Community/Portal"
    Private Function GetPortalPermissions() As List(Of dtoProfileTypeAssignment) Implements IViewEditPermissions.GetPortalPermissions
        Dim items As New List(Of dtoProfileTypeAssignment)
        For Each r As RepeaterItem In RPTcommunityAssignments.Items
            Dim oLiteral As Literal = r.FindControl("LTidAssignment")

            If oLiteral.Text = 0 Then
                For Each row As RepeaterItem In DirectCast(r.FindControl("RPTselectedItems"), Repeater).Items
                    oLiteral = row.FindControl("LTidPersonType")

                    If Not String.IsNullOrEmpty(oLiteral.Text) AndAlso IsNumeric(oLiteral.Text) AndAlso CInt(oLiteral.Text) > 0 Then
                        Dim item As New dtoProfileTypeAssignment()
                        item.IdPersonType = CInt(oLiteral.Text)
                        oLiteral = row.FindControl("LTidAssignment")
                        item.Id = CInt(oLiteral.Text)
                        Dim oCheckbox As CheckBox = row.FindControl("CBXuse")
                        item.Use = oCheckbox.Checked
                        oCheckbox = row.FindControl("CBXedit")
                        item.Edit = oCheckbox.Checked
                        oCheckbox = row.FindControl("CBXclone")
                        item.Clone = oCheckbox.Checked
                        oCheckbox = row.FindControl("CBXpermissions")
                        item.ChangePermission = oCheckbox.Checked
                        items.Add(item)
                    End If
                Next
                Exit For
            End If
        Next
        Return items
    End Function
    Private Function GetRolePermissions() As List(Of dtoRoleAssignment) Implements IViewEditPermissions.GetRolePermissions
        Dim items As New List(Of dtoRoleAssignment)
        For Each r As RepeaterItem In RPTcommunityAssignments.Items
            Dim oLiteral As Literal = r.FindControl("LTidAssignment")

            If oLiteral.Text = -1 Then
                For Each row As RepeaterItem In DirectCast(r.FindControl("RPTselectedItems"), Repeater).Items
                    oLiteral = row.FindControl("LTidRole")

                    If Not String.IsNullOrEmpty(oLiteral.Text) AndAlso IsNumeric(oLiteral.Text) AndAlso CInt(oLiteral.Text) > 0 Then
                        Dim item As New dtoRoleAssignment()
                        item.IdRole = CInt(oLiteral.Text)
                        oLiteral = row.FindControl("LTidCommunity")
                        item.IdCommunity = CInt(oLiteral.Text)
                        oLiteral = row.FindControl("LTidAssignment")
                        item.Id = CInt(oLiteral.Text)
                        Dim oCheckbox As CheckBox = row.FindControl("CBXuse")
                        item.Use = oCheckbox.Checked
                        oCheckbox = row.FindControl("CBXedit")
                        item.Edit = oCheckbox.Checked
                        oCheckbox = row.FindControl("CBXclone")
                        item.Clone = oCheckbox.Checked
                        oCheckbox = row.FindControl("CBXpermissions")
                        item.ChangePermission = oCheckbox.Checked
                        items.Add(item)
                    End If
                Next
            End If
        Next
        Return items
    End Function
    Private Sub LoadAssignments(assignments As List(Of dtoTemplateAssignment)) Implements IViewEditPermissions.LoadAssignments
        If assignments.Where(Function(a) a.Type = PermissionType.Community OrElse a.Type = PermissionType.Portal).Any() Then
            Me.RPTcommunityAssignments.Visible = True
            Me.RPTcommunityAssignments.DataSource = assignments.Where(Function(a) a.Type = PermissionType.Community OrElse a.Type = PermissionType.Portal).ToList()
            Me.RPTcommunityAssignments.DataBind()
        Else
            Me.RPTcommunityAssignments.Visible = False
        End If
        If assignments.Where(Function(a) a.Type = PermissionType.Person).Any() Then
            Me.RPTusersAssignments.Visible = True
            Me.RPTusersAssignments.DataSource = assignments.Where(Function(a) a.Type = PermissionType.Person).OrderBy(Function(a) a.DisplayName).ToList()
            Me.RPTusersAssignments.DataBind()
        Else
            Me.RPTusersAssignments.Visible = False
        End If
    End Sub

    Private Sub InitializeCommunityToAdd(idProfile As Integer, forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability) Implements IViewEditPermissions.InitializeCommunityToAdd
        'Me.CTRLmessages.Visible = False
        CTRLaddCommunity.Visible = True
        'Master.SetOpenDialogOnPostbackByCssClass(CTRLaddCommunity.ModalIdentifier)
        If forAdministration Then
            CTRLaddCommunity.InitializeAdministrationControl(idProfile, unloadIdCommunities, availability, New List(Of Integer))
        Else
            CTRLaddCommunity.InitializeControlByModules(idProfile, requiredPermissions, unloadIdCommunities, availability)
        End If
    End Sub
    Private Sub CTRLaddCommunity_LoadDefaultFiltersToHeader(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLaddCommunity.LoadDefaultFiltersToHeader
        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
    Private Sub LoadCommunityAssignment(assignment As dtoCommunityAssignment, communityName As String, idCommunity As Integer, availableRoles As List(Of Integer)) Implements IViewEditPermissions.LoadCommunityAssignment
        Me.CTRLmessages.Visible = False

        Dim items As Dictionary(Of Integer, String) = GetTranslatedRoles().Where(Function(t) availableRoles.Contains(t.Key) AndAlso t.Key > 0).OrderBy(Function(t) t.Value).ToDictionary(Function(t) t.Key, Function(t) t.Value)
        Me.DVselectRoles.Visible = True

        Me.CBLroles.DataSource = items
        Me.CBLroles.DataValueField = "Key"
        Me.CBLroles.DataTextField = "Value"
        Me.CBLroles.DataBind()
        If Not IsNothing(assignment) Then
            For Each i As ListItem In Me.CBLroles.Items
                i.Selected = assignment.Roles.Where(Function(a) a.IdRole = CInt(i.Value)).Any()
            Next
            Me.CBLroles.Enabled = True
        End If
        Me.LBcommunityName.Text = communityName
        IdEditingCommunity = idCommunity
    End Sub
    Private Function SelectedRoles() As List(Of Integer) Implements IViewEditPermissions.SelectedRoles
        Dim roles As New List(Of Int32)
        If (CBLroles.Enabled) Then
            roles = (From item As ListItem In CBLroles.Items Where item.Selected Select CInt(item.Value)).ToList
        End If
        Return roles
    End Function
#End Region

#Region "Person"
    Private Function GetPersonPermissions() As List(Of dtoPersonAssignment) Implements IViewEditPermissions.GetPersonPermissions
        Dim items As New List(Of dtoPersonAssignment)
        For Each row As RepeaterItem In RPTusersAssignments.Items
            Dim item As New dtoPersonAssignment()
            Dim oLiteral As Literal = row.FindControl("LTidAssignment")
            item.Id = CInt(oLiteral.Text)

            oLiteral = row.FindControl("LTidPerson")
            item.IdPerson = CInt(oLiteral.Text)

            Dim oCheckbox As CheckBox = row.FindControl("CBXuse")
            item.Use = oCheckbox.Checked
            oCheckbox = row.FindControl("CBXedit")
            item.Edit = oCheckbox.Checked
            oCheckbox = row.FindControl("CBXclone")
            item.Clone = oCheckbox.Checked
            oCheckbox = row.FindControl("CBXpermissions")
            item.ChangePermission = oCheckbox.Checked
            items.Add(item)

        Next
        Return items
    End Function
    Private Sub DisplayAddUsersFromCommunity(idCommunity As Integer, removeUsers As List(Of Integer)) Implements IViewEditPermissions.DisplayAddUsersFromCommunity
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunity, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription.text"))
    End Sub

    Private Sub DisplayAddUsersFromCommunity(idCommunities As List(Of Integer), removeUsers As List(Of Integer)) Implements IViewEditPermissions.DisplayAddUsersFromCommunity
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunities, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription.text"))
    End Sub

    Private Sub DisplayAddUsersFromPortal(removeUsers As List(Of Integer)) Implements IViewEditPermissions.DisplayAddUsersFromPortal
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, True, False, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription.text"))
    End Sub
    Private Function GetCurrentSelectedUsers() As List(Of dtoSelectItem(Of Integer)) Implements IViewEditPermissions.GetCurrentSelectedUsers
        Dim items As New List(Of dtoSelectItem(Of Integer))

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTusersAssignments.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXuser")
            Dim oLiteral As Literal = row.FindControl("LTidUser")
            items.Add(New dtoSelectItem(Of Integer)() With {.Id = CInt(oLiteral.Text), .Selected = oCheck.Checked})
        Next
        Return items
    End Function
#End Region

#End Region

    Private Function GetEncodedBackUrl() As String Implements IViewBaseEdit.GetEncodedBackUrl
        If String.IsNullOrEmpty(PreloadBackUrl) Then
            Return ""
        Else
            Return Server.UrlEncode(PreloadBackUrl)
        End If
    End Function
#End Region

#Region "Page"

#Region "Common"
    Protected Function DialogTitleTranslation(t As PermissionType) As String
        Return Resource.getValue("DialogTitleTranslation." & t.ToString)
    End Function
#End Region

#Region "Page - Manage Portal/Community Assignments"
    Protected Function CssAssignmentItem(ByVal isDefault As Boolean) As String
        Return IIf(isDefault, "currentcommunity", "")
    End Function
    Private Sub RPTcommunityAssignments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunityAssignments.ItemDataBound
        Dim item As dtoTemplateAssignment = DirectCast(e.Item.DataItem, dtoTemplateAssignment)
        Dim isDefault As Boolean = True
        Dim allowEditing As Boolean = Not InputReadOnly

        Dim oRepeater As Repeater = e.Item.FindControl("RPTselectedItems")
        Dim oButton As Button = e.Item.FindControl("BTNeditCommunityAssignment")
        oButton.Visible = allowEditing

        Me.Resource.setButton(oButton, True)

        Dim oDiv As HtmlControl = e.Item.FindControl("DVmodulePermissions")
        Dim oLabel As Label = e.Item.FindControl("LBdisplayName_t")
        Select Case item.Type
            Case PermissionType.Portal
                Dim pItem As dtoPortalAssignment = DirectCast(e.Item.DataItem, dtoPortalAssignment)
                isDefault = pItem.IsDefault
                Me.Resource.setLabel_To_Value(oLabel, "DisplayName.Portal")

                oLabel = e.Item.FindControl("LBroleHeader_t")
                Me.Resource.setLabel_To_Value(oLabel, "LBroleHeader_t.PersonTypes")

                oLabel = e.Item.FindControl("LBdisplayName")
                oLabel.Visible = False

                oButton.CommandArgument = pItem.Id.ToString & ",0"

                oButton = e.Item.FindControl("BTNdeleteCommunityAssignment")
                oButton.CommandArgument = pItem.Id.ToString & ",0"
                oButton.Visible = Not isDefault AndAlso allowEditing

                oDiv.Visible = False

                'oDiv = e.Item.FindControl("DVmodulePermissionsTitle")
                'oDiv.Visible = (pItem.ProfileTypes.Count > 0)
                oDiv = e.Item.FindControl("DVtable")
                oDiv.Visible = (pItem.ProfileTypes.Count > 0)
                oRepeater.DataSource = pItem.ProfileTypes
                oRepeater.DataBind()
                Me.Resource.setButton(oButton, True)

            Case PermissionType.Community
                Dim cItem As dtoCommunityAssignment = DirectCast(e.Item.DataItem, dtoCommunityAssignment)
                isDefault = cItem.IsDefault
                Me.Resource.setLabel_To_Value(oLabel, "DisplayName.Community")

                oLabel = e.Item.FindControl("LBroleHeader_t")
                Me.Resource.setLabel_To_Value(oLabel, "LBroleHeader_t.Roles")


                oRepeater.DataSource = cItem.Roles
                oRepeater.DataBind()
                oDiv = e.Item.FindControl("DVtable")
                oDiv.Visible = (cItem.Roles.Count > 0)

                oButton.CommandArgument = cItem.Id.ToString & "," & cItem.IdCommunity

                oButton = e.Item.FindControl("BTNdeleteCommunityAssignment")
                oButton.CommandArgument = cItem.Id.ToString & "," & cItem.IdCommunity
                oButton.Visible = Not isDefault AndAlso allowEditing
                'Me.Resource.setButton(oButton, True)
                'Dim oLiteral As Literal = e.Item.FindControl("LTcommunityItemModulePermissionInfo")
                'Resource.setLiteral(oLiteral)

                'oLabel = e.Item.FindControl("LBpermissionUse")
                'Resource.setLabel(oLabel)

                'oLabel = e.Item.FindControl("LBpermissionEdit")
                'Resource.setLabel(oLabel)
                'oLabel = e.Item.FindControl("LBpermissionClone")
                'Resource.setLabel(oLabel)
                'oLabel = e.Item.FindControl("LBpermissionChange")
                'Resource.setLabel(oLabel)

                'Dim oCheckbox As CheckBox = e.Item.FindControl("CBXuse")
                'oCheckbox.Checked = cItem.Modules.Any() AndAlso cItem.Modules.FirstOrDefault.See
                'oCheckbox = e.Item.FindControl("CBXedit")
                'oCheckbox.Checked = cItem.Modules.Any() AndAlso cItem.Modules.FirstOrDefault.Edit
                'oCheckbox = e.Item.FindControl("CBXclone")
                'oCheckbox.Checked = cItem.Modules.Any() AndAlso cItem.Modules.FirstOrDefault.Clone
                'oCheckbox = e.Item.FindControl("CBXpermissions")
                'oCheckbox.Checked = cItem.Modules.Any() AndAlso cItem.Modules.FirstOrDefault.ChangePermission
        End Select

        oLabel = e.Item.FindControl("LBpermissionUseHeader_t")
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBuseSelectAll")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBuseSelectNone")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)

        oLabel = e.Item.FindControl("LBpermissionEditHeader_t")
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBeditSelectAll")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBeditSelectNone")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)

        oLabel = e.Item.FindControl("LBpermissionCloneHeader_t")
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBcloneSelectAll")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBcloneSelectNone")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)

        oLabel = e.Item.FindControl("LBpermissionChangeHeader_t")
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBpermissionChangeSelectAll")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)
        oLabel = e.Item.FindControl("LBpermissionChangeSelectNone")
        oLabel.Visible = allowEditing
        Resource.setLabel(oLabel)


        oLabel = e.Item.FindControl("LBcommunityItemModulePermissions_t")
        Resource.setLabel(oLabel)

    End Sub
    Private Sub RPTcommunityAssignments_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcommunityAssignments.ItemCommand
        Dim idAssignment As Long = CLng(e.CommandArgument.ToString.Split(",").First())
        Dim idCommunity As Long = CLng(e.CommandArgument.ToString.Split(",").Last())

        Select Case e.CommandName
            Case "edititem"
                Me.CTRLmessages.Visible = False
                If idAssignment = 0 Then
                    Dim items As Dictionary(Of Integer, String) = GetTranslatedProfileTypes()
                    Me.DVselectPersonTypes.Visible = True
                    Dim assignment As dtoPortalAssignment = Me.CurrentPresenter.GetPortalAssignments(items)

                    Me.CBLprofileTypes.DataSource = items
                    Me.CBLprofileTypes.DataValueField = "Key"
                    Me.CBLprofileTypes.DataTextField = "Value"
                    Me.CBLprofileTypes.DataBind()
                    If Not IsNothing(assignment) Then
                        For Each i As ListItem In Me.CBLprofileTypes.Items
                            i.Selected = assignment.ProfileTypes.Where(Function(a) a.IdPersonType = CInt(i.Value)).Any()
                        Next
                        Me.CBLprofileTypes.Enabled = True
                    Else
                        Me.CBLprofileTypes.Enabled = False
                    End If
                Else
                    Me.CurrentPresenter.LoadCommunityAssignments(idCommunity, idAssignment, GetTranslatedModules, GetTranslatedRoles)
                End If
            Case "deleteitem"
                Me.CurrentPresenter.DeleteCommunityAssignment(idCommunity, idAssignment)
        End Select
    End Sub

    Protected Sub RPTselectedItemss_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim item As dtoTemplateAssignment = DirectCast(e.Item.DataItem, dtoTemplateAssignment)

        Dim oLabel As Label = e.Item.FindControl("LBroleSelectAll")
        Resource.setLabel(oLabel)

        oLabel = e.Item.FindControl("LBroleSelectNone")
        Resource.setLabel(oLabel)

        Dim oLiteral As Literal = e.Item.FindControl("LTidPersonType")
        Select Case item.Type
            Case PermissionType.ProfileType
                Dim pItem As dtoProfileTypeAssignment = DirectCast(e.Item.DataItem, dtoProfileTypeAssignment)
                oLiteral.Text = pItem.IdPersonType
            Case PermissionType.Role
                Dim cItem As dtoRoleAssignment = DirectCast(e.Item.DataItem, dtoRoleAssignment)
                oLiteral = e.Item.FindControl("LTidRole")
                oLiteral.Text = cItem.IdRole
                oLiteral = e.Item.FindControl("LTidCommunity")
                oLiteral.Text = cItem.IdCommunity
        End Select
    End Sub


#Region "Person Types"
    Private Sub BTNclosePersonTypesAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNclosePersonTypesAssignments.Click
        Me.DVselectPersonTypes.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub BTNsavePersonTypesAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNsavePersonTypesAssignments.Click
        Me.DVselectPersonTypes.Visible = False
        Me.CurrentPresenter.SaveSettings(GetPermissions(), SelectedProfileTypes())
    End Sub
    Private Function SelectedProfileTypes() As List(Of Integer) Implements IViewEditPermissions.SelectedProfileTypes
        Dim types As New List(Of Int32)
        If (CBLprofileTypes.Enabled) Then
            types = (From item As ListItem In CBLprofileTypes.Items Where item.Selected Select CInt(item.Value)).ToList
        End If
        Return types
    End Function
#End Region

#Region "Select community"
  Private Sub CTRLaddCommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLaddCommunity.SelectedCommunities
        Master.SetOpenDialogOnPostbackByCssClass("")
        CurrentPresenter.AddCommunity(idCommunities)
    End Sub
#End Region

#Region "Select roles"
    Private Sub BTNcloseRoleAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNcloseRoleAssignments.Click
        Me.DVselectRoles.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub BTNsaveRoleAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNsaveRoleAssignments.Click
        Me.DVselectRoles.Visible = False
        Me.CurrentPresenter.SaveSettings(IdEditingCommunity, GetPermissions(), SelectedRoles())
    End Sub
#End Region

#End Region

#Region "Page - User Assignments"

    Private Sub RPTusersAssignments_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTusersAssignments.ItemCommand
        Select Case e.CommandName
            Case "save"
                CurrentPresenter.SavePersonSettings(GetPermissions)
            Case "manage"

        End Select
    End Sub
    Private Sub RPTusersAssignments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTusersAssignments.ItemDataBound
        Dim oLabel As Label = Nothing
        Dim oLiteral As Literal = Nothing
        Dim inEditing As Boolean = Not InputReadOnly

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoPersonAssignment = DirectCast(e.Item.DataItem, dtoPersonAssignment)

            Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBXuser")
            oCheck.Checked = SelectedIdUsers.Contains(item.IdPerson)
            oCheck.Disabled = Not inEditing OrElse Not item.AllowEdit
        ElseIf e.Item.ItemType = ListItemType.Header Then
            oLabel = e.Item.FindControl("LBuserListTitle")
            Resource.setLabel(oLabel)

            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBmanageUsers")
            oLinkbutton.Visible = inEditing
            Resource.setLinkButton(oLinkbutton, False, True)

            oLinkbutton = e.Item.FindControl("LNBsaveUsers")
            oLinkbutton.Visible = inEditing
            Resource.setLinkButton(oLinkbutton, False, True)

            oLiteral = e.Item.FindControl("LTusername_t")
            Resource.setLiteral(oLiteral)

            oLabel = e.Item.FindControl("LBpermissionUseHeader_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuseSelectAll")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBuseSelectNone")
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBpermissionEditHeader_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBeditSelectAll")
            oLabel.Visible = inEditing
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBeditSelectNone")
            oLabel.Visible = inEditing
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBpermissionCloneHeader_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcloneSelectAll")
            oLabel.Visible = inEditing
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcloneSelectNone")
            oLabel.Visible = inEditing
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBpermissionChangeHeader_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBpermissionChangeSelectAll")
            oLabel.Visible = inEditing
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBpermissionChangeSelectNone")
            oLabel.Visible = inEditing
            Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim tableRow As HtmlTableRow = e.Item.FindControl("TRfooter")
            If RPTusersAssignments.Items.Count > MaxDisplayUsers Then
                tableRow.Visible = True
                oLabel = e.Item.FindControl("LBshowextraUserItems")
                Resource.setLabel(oLabel)

                oLabel = e.Item.FindControl("LBhideextraUserItems")
                Resource.setLabel(oLabel)
            Else
                tableRow.Visible = False
            End If
        End If
    End Sub

#Region "Select Users"
    Private Sub CLTRselectUsers_CloseWindow() Handles CLTRselectUsers.CloseWindow
        Me.DVselectUsers.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub CLTRselectUsers_UsersSelected(idUsers As List(Of Integer)) Handles CLTRselectUsers.UsersSelected
        Me.DVselectUsers.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.AddUsers(GetPermissions(), idUsers)
    End Sub
#End Region

#End Region

#Region "Add buttons"
    Private Sub BTNaddCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNaddCommunity.Click
        CTRLmessages.Visible = False
    End Sub
    Private Sub BTNaddProfile_Click(sender As Object, e As System.EventArgs) Handles BTNaddProfile.Click
        Me.CurrentPresenter.AddUserFromPortal()
    End Sub
    Private Sub BTNaddUserFromCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNaddUserFromCommunity.Click
        Me.CurrentPresenter.AddUserFromCommunity()
    End Sub
#End Region

#End Region

    Private Sub BTNsaveAvailabilityBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsavePermissionsBottom.Click, BTNsavePermissionsTop.Click
        Me.CurrentPresenter.SaveSettings()
    End Sub

End Class