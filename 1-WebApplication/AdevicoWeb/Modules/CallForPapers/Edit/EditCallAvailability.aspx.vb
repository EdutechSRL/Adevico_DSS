Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditCallAvailability
    Inherits PageBase
    Implements IViewEditCallAvailability


#Region "Context"
    Private _Presenter As EditCallAvailabilityPresenter
    Private ReadOnly Property CurrentPresenter() As EditCallAvailabilityPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditCallAvailabilityPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property UnknownUserTranslation As String Implements IViewEditCallAvailability.UnknownUserTranslation
        Get
            Return Me.Resource.getValue("UnknownUser")
        End Get
    End Property

    Private ReadOnly Property UnknownCommunityTranslation As String Implements IViewEditCallAvailability.UnknownCommunityTranslation
        Get
            Return Me.Resource.getValue("UnknownCommunity")
        End Get
    End Property

    Private ReadOnly Property Portalname As String Implements IViewEditCall.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadAction As CallStandardAction Implements IViewEditCall.PreloadAction
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStandardAction).GetByString(Request.QueryString("action"), CallStandardAction.List)
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewEditCall.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewEditCall.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadType As CallForPaperType Implements IViewEditCall.PreloadType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallForPaperType).GetByString(Request.QueryString("type"), CallForPaperType.CallForBids)
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewEditCall.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewEditCall.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewEditCall.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
            Resource.setLabel_To_Value(LBisPublicInfo, "LBisPublicInfo." & value.ToString & ".text")
            Resource.setHyperLink(HYPpreviewCallBottom, value.ToString(), True, True)
            Resource.setHyperLink(HYPpreviewCallTop, value.ToString(), True, True)
            Resource.setHyperLink(HYPbackTop, value.ToString, True, True)
            Resource.setHyperLink(HYPbackBottom, value.ToString, True, True)

            Resource.setLiteral(LTtoolDescription, value.ToString)
            Resource.setLabel_To_Value(LBpersonTypesDescription, "LBpersonTypesDescription." & value.ToString)
            CTRLaddCommunity.Description = Resource.getValue("LBselectCommunityDescription." & value.ToString)
            Resource.setLabel_To_Value(LBselectRolesDescription, "LBselectRolesDescription." & value.ToString)
        End Set
    End Property
    Private Property CurrentAction As CallStandardAction Implements IViewEditCall.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", IIf(IdCall = 0, CallStandardAction.Add, CallStandardAction.Edit))
        End Get
        Set(value As CallStandardAction)
            Me.ViewState("CurrentAction") = value
        End Set
    End Property
    Private Property CurrentStatus As CallForPaperStatus Implements IViewEditCall.CurrentStatus
        Get
            Return ViewStateOrDefault("CurrentStatus", CallForPaperStatus.Draft)
        End Get
        Set(ByVal value As CallForPaperStatus)
            Me.ViewState("CurrentStatus") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewEditCall.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewEditCall.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property


    Private Property ForPortal As Boolean Implements IViewEditCallAvailability.ForPortal
        Get
            Return ViewStateOrDefault("ForPortal", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ForPortal") = value
        End Set
    End Property
    'Private Property ForAllMembers As Boolean Implements lm.Comol.Modules.CallForPapers.Presentation.IViewEditCallAvailability.ForAllMembers
    '    Get
    '        Return Me.RBLallowTo.SelectedValue
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.RBLallowTo.SelectedValue = value
    '        Me.DVprofileType.Visible = Not value AndAlso ForPortal
    '        Me.DVroles.Visible = Not value AndAlso Not ForPortal
    '        Me.DVusers.Visible = Not value
    '        'Me.MLVpermission.SetActiveView(IIf(value, VIWnone, VIWdefinePermission))
    '        ''Me.UDPpermission.Update()
    '        'Me.UPdpermissionContainer.Update()
    '    End Set
    'End Property
    Private Property IsPublic As Boolean Implements IViewEditCallAvailability.IsPublic
        Get
            Return Me.CBXisPublic.Checked
        End Get
        Set(value As Boolean)
            Me.CBXisPublic.Checked = value
        End Set
    End Property
    Private Property IdEditingCommunity As Integer Implements IViewEditCallAvailability.IdEditingCommunity
        Get
            Return ViewStateOrDefault("IdEditingCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdEditingCommunity") = value
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
    Private ReadOnly Property isSkinSelectorVisible As Boolean Implements IViewEditCallAvailability.isSkinSelectorVisible
        Get
            Return Me.DVmoduleSkin.Visible
        End Get
    End Property
    Private Property SelectedIdUsers As List(Of Integer) Implements IViewEditCallAvailability.SelectedIdUsers
        Get
            Return ViewStateOrDefault("SelectedIdUsers", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            Me.ViewState("SelectedIdUsers") = value
        End Set
    End Property
    Private ReadOnly Property MaxDisplayUsers As Integer Implements IViewEditCallAvailability.MaxDisplayUsers
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
            .setButton(BTNsaveAvailabilityBottom, True, , , True)
            .setButton(BTNsaveAvailabilityTop, True, , , True)

            .setLabel(LBisPublic_t)

            .setLabel(LBpublicUrlInfo_t)
            .setLiteral(LTpublicListUrlMessage)
            .setLiteral(LTpublicCallDirectUrlMessage)

            .setLabel(LBselectAllpersonTypes_t)
            .setButton(BTNclosePersonTypesAssignments, True)
            .setButton(BTNsavePersonTypesAssignments, True)


            .setLabel(LBcommunityName_t)
            .setLabel(LBselectAllroles_t)
            .setButton(BTNcloseRoleAssignments, True)
            .setButton(BTNsaveRoleAssignments, True)


            .setLabel(LBselectAllroles_t)

            CTRLcommunitySelectorHeader.ModalTitle = DialogTitleTranslation(0)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"

#Region "Actions"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditCall.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditCall.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.NoPermission, ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.EditCallAvailability(CallType, PreloadIdCall, idCommunity, PreloadView)

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
    Private Sub DisplayNoAvailability() Implements IViewEditCallAvailability.DisplayNoAvailability
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoAvailability"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplaySaveErrors(display As Boolean) Implements IViewEditCallAvailability.DisplaySaveErrors
        CTRLmessages.Visible = display
        If display Then
            CTRLmessages.InitializeControl(Resource.getValue("DisplayAvailabilitySaveErrors"), Helpers.MessageType.error)
        End If
    End Sub
    Private Sub DisplayNoAssignments() Implements IViewEditCallAvailability.DisplayNoAssignments
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoAssignments"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayAvailabilitySaved() Implements IViewEditCallAvailability.DisplayAvailabilitySaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayAvailabilitySaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAssignmentAdded(name As String) Implements IViewEditCallAvailability.DisplayCommunityAssignmentAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayCommunityAssignmentAdded"), name), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAssignmentsAdded() Implements IViewEditCallAvailability.DisplayCommunityAssignmentsAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayCommunityAssignmentsAdded"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAssignmentDeleted(name As String) Implements IViewEditCallAvailability.DisplayCommunityAssignmentDeleted
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayCommunityAssignmentDeleted"), name), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayUserAssignmentsAdded() Implements IViewEditCallAvailability.DisplayUserAssignmentsAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayUserAssignmentsAdded"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayCommunityAssignmentSaved(name As String) Implements IViewEditCallAvailability.DisplayCommunityAssignmentSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("DisplayCommunityAssignmentSaved"), name), Helpers.MessageType.success)
    End Sub


    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewEditCall.SetActionUrl
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
    Private Sub SetContainerName(action As CallStandardAction, name As String, itemName As String) Implements IViewEditCall.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & CurrentAction.ToString() & "." & CallType.ToString())
        Master.ServiceTitle = String.Format(title, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        If String.IsNullOrEmpty(name) Then
            Master.ServiceTitleToolTip = String.Format(title, itemName)
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & action.Edit.ToString() & "." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub
    Private Sub LoadWizardSteps(idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of WizardCallStep))) Implements IViewEditCall.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, type, idCommunity, PreloadView, steps)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewEditCall.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Private Sub GoToUrl(action As lm.Comol.Modules.CallForPapers.Domain.CallStandardAction, url As String) Implements IViewEditCall.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub

#End Region

#Region "Assignments"
    Private Sub LoadAvailableAssignments(items As List(Of CallAssignmentType)) Implements IViewEditCallAvailability.LoadAvailableAssignments
        LIaddToolbar.Visible = items.Any
        If items.Any Then
            If items.Contains(CallAssignmentType.Person) Then
                Me.LIaddUserFromCommunity.Visible = True
                Me.BTNaddUserFromCommunity.Visible = True
                Resource.setButtonByValue(BTNaddUserFromCommunity, CallType.ToString, True)
                Me.LTaddUserFromCommunity.Text = BTNaddUserFromCommunity.Text
                Me.LTaddUserFromCommunityDescription.Text = Resource.getValue("LTaddUserFromCommunityDescription." & CallType.ToString)
            End If

            If items.Contains(CallAssignmentType.Community) Then
                Me.LIaddCommunity.Visible = True
                Me.BTNaddCommunity.Visible = True
                Resource.setButtonByValue(BTNaddCommunity, CallType.ToString, True)
                Me.LTaddCommunity.Text = BTNaddCommunity.Text
                Me.LTaddCommunityDescription.Text = Resource.getValue("LTaddCommunityDescription." & CallType.ToString)
            End If

            If items.Contains(CallAssignmentType.SubmitterOfBaseForPaper) Then
                Me.LIaddUserFromCall.Visible = True
                Me.BTNaddUserFromCall.Visible = True
                Resource.setButtonByValue(BTNaddUserFromCall, CallType.ToString, True)
                Me.LTaddUserFromCall.Text = BTNaddUserFromCall.Text
                Me.LTaddUserFromCallDescription.Text = Resource.getValue("LTaddUserFromCallDescription." & CallType.ToString)
            End If

            If items.Contains(CallAssignmentType.PersonType) Then
                Me.LIaddProfile.Visible = True
                Me.BTNaddProfile.Visible = True
                Resource.setButtonByValue(BTNaddProfile, CallType.ToString, True)
                Me.LTaddProfile.Text = BTNaddProfile.Text
                Me.LTaddProfileDescription.Text = Resource.getValue("LTaddProfileDescription." & CallType.ToString)
            End If
        End If
    End Sub
    Private Function GetTranslatedProfileTypes() As Dictionary(Of Integer, String) Implements IViewEditCallAvailability.GetTranslatedProfileTypes
        Return (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Order By o.Descrizione Select o).ToDictionary(Function(t) t.ID, Function(t) t.Descrizione)
    End Function
    Private Function GetTranslatedRoles() As Dictionary(Of Integer, String) Implements IViewEditCallAvailability.GetTranslatedRoles
        Return COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToDictionary(Function(r) r.ID, Function(r) r.Name)
    End Function

#Region "Community/Portal"
    Private Sub LoadAssignments(assignments As List(Of dtoCallAssignment)) Implements IViewEditCallAvailability.LoadAssignments
        If assignments.Where(Function(a) a.Type = CallAssignmentType.Community OrElse a.Type = CallAssignmentType.Portal).Any() Then
            Me.RPTcommunityAssignments.Visible = True
            Me.RPTcommunityAssignments.DataSource = assignments.Where(Function(a) a.Type = CallAssignmentType.Community OrElse a.Type = CallAssignmentType.Portal).ToList()
            Me.RPTcommunityAssignments.DataBind()
        Else
            Me.RPTcommunityAssignments.Visible = False
        End If
        If assignments.Where(Function(a) a.Type = CallAssignmentType.Person).Any() Then
            Me.RPTusersAssignments.Visible = True
            Me.RPTusersAssignments.DataSource = assignments.Where(Function(a) a.Type = CallAssignmentType.Person).OrderBy(Function(a) a.DisplayName).ToList()
            Me.RPTusersAssignments.DataBind()
        Else
            Me.RPTusersAssignments.Visible = False
        End If

    End Sub

    Private Sub DisplayCommunityToAdd(idProfile As Integer, forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability) Implements IViewEditCallAvailability.DisplayCommunityToAdd
        Me.CTRLmessages.Visible = False
        CTRLaddCommunity.Visible = True
        Master.SetOpenDialogOnPostbackByCssClass(CTRLaddCommunity.ModalIdentifier)
        If forAdministration Then
            CTRLaddCommunity.InitializeAdministrationControl(idProfile, unloadIdCommunities, availability, New List(Of Integer))
        Else
            CTRLaddCommunity.InitializeControlByModules(idProfile, requiredPermissions, unloadIdCommunities, availability)
        End If
    End Sub
    Private Sub CTRLaddCommunity_LoadDefaultFiltersToHeader(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLaddCommunity.LoadDefaultFiltersToHeader
        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
    Private Sub LoadCommunityAssignment(assignment As dtoCallCommunityAssignment, communityName As String, idCommunity As Integer, availableRoles As List(Of Integer)) Implements IViewEditCallAvailability.LoadCommunityAssignment
        Me.CTRLmessages.Visible = False

        Dim items As Dictionary(Of Integer, String) = GetTranslatedRoles().Where(Function(t) availableRoles.Contains(t.Key) AndAlso t.Key > 0).OrderBy(Function(t) t.Value).ToDictionary(Function(t) t.Key, Function(t) t.Value)
        Me.DVselectRoles.Visible = True

        Me.CBLroles.DataSource = items
        Me.CBLroles.DataValueField = "Key"
        Me.CBLroles.DataTextField = "Value"
        Me.CBLroles.DataBind()
        If Not IsNothing(assignment) Then
            Me.CBXselectAllRoles.Checked = assignment.ForAllUsers
            If Not assignment.ForAllUsers Then
                For Each i As ListItem In Me.CBLroles.Items
                    i.Selected = assignment.Roles.Where(Function(a) a.IdRole = CInt(i.Value)).Any()
                Next
            End If
            Me.CBLroles.Enabled = Not assignment.ForAllUsers
        Else
            Me.CBLroles.Enabled = False
            Me.CBXselectAllRoles.Checked = True
        End If
        Me.LBcommunityName.Text = communityName
        IdEditingCommunity = idCommunity
    End Sub
    Private Function SelectedRoles() As List(Of Integer) Implements IViewEditCallAvailability.SelectedRoles
        Dim roles As New List(Of Int32)
        If (CBLroles.Enabled) Then
            roles = (From item As ListItem In CBLroles.Items Where item.Selected Select CInt(item.Value)).ToList
        End If
        Return roles
    End Function
#End Region

#Region "Person"
    Private Sub DisplayAddUsersFromCall(t As CallForPaperType, fromPortal As Boolean, fromCommunities As List(Of Integer), removeUsers As List(Of Integer)) Implements IViewEditCallAvailability.DisplayAddUsersFromCall
        Me.CTRLmessages.Visible = False
        Me.DVselectFromCall.Visible = True
        Me.CTRLselectUsersFromCall.InitializeControl(t, fromPortal, fromCommunities, removeUsers)
    End Sub

    Private Sub DisplayAddUsersFromCommunity(idCommunity As Integer, removeUsers As List(Of Integer)) Implements IViewEditCallAvailability.DisplayAddUsersFromCommunity
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunity, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription." & CallType.ToString & ".text"))
    End Sub

    Private Sub DisplayAddUsersFromCommunity(idCommunities As List(Of Integer), removeUsers As List(Of Integer)) Implements IViewEditCallAvailability.DisplayAddUsersFromCommunity
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, idCommunities, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription." & CallType.ToString & ".text"))
    End Sub

    Private Sub DisplayAddUsersFromPortal(removeUsers As List(Of Integer)) Implements IViewEditCallAvailability.DisplayAddUsersFromPortal
        Me.CTRLmessages.Visible = False
        Me.DVselectUsers.Visible = True
        Me.CLTRselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, True, False, removeUsers, Nothing, Resource.getValue("LBselectUsersDescription." & CallType.ToString & ".text"))
    End Sub
    Private Function GetCurrentSelectedUsers() As List(Of dtoSelectItem(Of Integer)) Implements IViewEditCallAvailability.GetCurrentSelectedUsers
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

#Region "Public mode"

    Private Sub HideSkinsInfo() Implements IViewEditCallAvailability.HideSkinsInfo
        Me.DVmoduleSkin.Visible = False
    End Sub

    Private Sub LoadSkinInfo(idCall As Long, fullyQualifiedName As String, itemType As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType, idModule As Integer, idCommunity As Integer, allowAdd As Boolean, allowEdit As Boolean, loadAll As Boolean) Implements IViewEditCallAvailability.LoadSkinInfo
        Me.DVmoduleSkin.Visible = True
        CTRLmoduleSkin.InitializeControl(idModule, idCommunity, idCall, itemType, fullyQualifiedName, allowAdd, allowEdit, IIf(loadAll, lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Module Or lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Object, lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Object))
    End Sub

    Private Sub LoadSkinInfo(idCall As Long, fullyQualifiedName As String, itemType As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType, idModule As Integer, idCommunity As Integer, allowAdd As Boolean, allowEdit As Boolean, loadAll As Boolean) Implements IViewEditCallAvailability.LoadSkinInfo
        Me.DVmoduleSkin.Visible = True
        CTRLmoduleSkin.InitializeControl(idModule, idCommunity, idCall, itemType, fullyQualifiedName, allowAdd, allowEdit, IIf(loadAll, lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Module Or lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Object, lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Object))
    End Sub

    Private Sub DisplayPublicUrl(type As CallForPaperType, callUrl As String) Implements IViewEditCallAvailability.DisplayPublicUrl
        Me.DVpublicAccess.Visible = True
        LTpublicCallDirectUrl.Text = PageUtility.ApplicationUrlBase & callUrl
        LIpublicListUrl.Visible = False
        LTpublicListUrlMessage.Text = Resource.getValue("LTpublicCallDirectUrlMessage." & type.ToString())
    End Sub

    Private Sub DisplayPublicUrls(type As CallForPaperType, callUrl As String, collectorUrl As String) Implements IViewEditCallAvailability.DisplayPublicUrls
        Me.DVpublicAccess.Visible = True
        LIpublicListUrl.Visible = True
        LTpublicCallDirectUrl.Text = PageUtility.ApplicationUrlBase & callUrl
        LTpublicListUrl.Text = PageUtility.ApplicationUrlBase & collectorUrl
        LTpublicListUrlMessage.Text = Resource.getValue("LTpublicListUrlMessage." & type.ToString())
        LTpublicCallDirectUrlMessage.Text = Resource.getValue("LTpublicCallDirectUrlMessage." & type.ToString())
    End Sub

    Private Sub HidePublicUrl() Implements IViewEditCallAvailability.HidePublicUrl
        Me.DVpublicAccess.Visible = False
    End Sub
    Private Function SaveSkinSelection() As Boolean
        If CTRLmoduleSkin.Visible AndAlso CTRLmoduleSkin.isInitialized Then
            Return CTRLmoduleSkin.SaveSkinAssociation()
        Else
            Return True
        End If
    End Function
#End Region

#End Region

#Region "Page"

#Region "Common"
    Protected Function DialogTitleTranslation(t As CallAssignmentType) As String
        If t = CallAssignmentType.SubmitterOfBaseForPaper Then
            Return Resource.getValue("DialogTitleTranslation." & t.ToString & "." & CallType.ToString)
        Else
            Return Resource.getValue("DialogTitleTranslation." & t.ToString)
        End If

    End Function
#End Region

#Region "Page - Manage Portal/Community Assignments"
    Protected Function CssAssignmentItem(ByVal isDefault As Boolean) As String
        Return IIf(isDefault, "currentcommunity", "")
    End Function
    Private Sub RPTcommunityAssignments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommunityAssignments.ItemDataBound
        Dim item As dtoCallAssignment = DirectCast(e.Item.DataItem, dtoCallAssignment)
        Dim isDefault As Boolean = True

        Dim oRepeater As Repeater = e.Item.FindControl("RPTselectedItems")
        Dim oButton As Button = e.Item.FindControl("BTNeditCallCommunityAssignment")

        Me.Resource.setButton(oButton, True)

        Dim oLabel As Label = e.Item.FindControl("LBdisplayName_t")
        Dim oLabelShow As Label = e.Item.FindControl("LBshowAllSelectedItems")
        Dim oLabelHide As Label = e.Item.FindControl("LBhideAllSelectedItems")
        Dim oControl As HtmlControl = e.Item.FindControl("ULitems")

        oLabelShow.Text = Resource.getValue("showAllSelectedItems.Text")
        oLabelHide.Text = Resource.getValue("hideAllSelectedItems.Text")
        Select Case item.Type
            Case CallAssignmentType.Portal
                Dim pItem As dtoCallPortalAssignment = DirectCast(e.Item.DataItem, dtoCallPortalAssignment)
                isDefault = pItem.IsDefault
                Me.Resource.setLabel_To_Value(oLabel, "DisplayName.Portal")

                oLabel = e.Item.FindControl("LBselectedItems_t")
                Me.Resource.setLabel_To_Value(oLabel, "SelectedItems.PersonTypes")

                oLabel = e.Item.FindControl("LBallSelectedItemsInfo")
                oLabel.Visible = pItem.ForAllUsers
                oRepeater.Visible = Not pItem.ForAllUsers
                If pItem.ForAllUsers Then
                    Me.Resource.setLabel_To_Value(oLabel, "LBallSelectedItemsInfo.PersonTypes")
                    oControl.Visible = False
                Else
                    oRepeater.DataSource = pItem.ProfileTypes
                    oRepeater.DataBind()

                    oLabelShow.ToolTip = String.Format(Resource.getValue("showAllSelectedItems.PersonTypes"), pItem.ProfileTypes.Count)
                    oLabelHide.ToolTip = Resource.getValue("hideAllSelectedItems.PersonTypes")
                End If
                oLabel = e.Item.FindControl("LBdisplayName")
                oLabel.Visible = False

                oButton.CommandArgument = pItem.Id.ToString & ",0"

                oButton = e.Item.FindControl("BTNdeleteCallCommunityAssignment")
                oButton.CommandArgument = pItem.Id.ToString & ",0"
                oButton.Visible = Not isDefault
                Me.Resource.setButton(oButton, True)
            Case CallAssignmentType.Community
                Dim cItem As dtoCallCommunityAssignment = DirectCast(e.Item.DataItem, dtoCallCommunityAssignment)
                isDefault = cItem.IsDefault
                Me.Resource.setLabel_To_Value(oLabel, "DisplayName.Community")

                oLabel = e.Item.FindControl("LBselectedItems_t")
                Me.Resource.setLabel_To_Value(oLabel, "SelectedItems.Roles")


                oLabel = e.Item.FindControl("LBallSelectedItemsInfo")
                oLabel.Visible = cItem.ForAllUsers OrElse cItem.IsEmpty
                oRepeater.Visible = Not cItem.ForAllUsers
                If cItem.ForAllUsers Then
                    Me.Resource.setLabel_To_Value(oLabel, "LBallSelectedItemsInfo.Roles")
                    oControl.Visible = False
                ElseIf cItem.IsEmpty Then
                    Me.Resource.setLabel_To_Value(oLabel, "LBallSelectedItemsInfo.Roles.None")
                    oControl.Visible = False
                Else
                    oRepeater.DataSource = cItem.Roles
                    oRepeater.DataBind()

                    oLabelShow.ToolTip = String.Format(Resource.getValue("showAllSelectedItems.Roles"), cItem.Roles.Count)
                    oLabelHide.ToolTip = Resource.getValue("hideAllSelectedItems.Roles")
                End If
                oButton.CommandArgument = cItem.Id.ToString & "," & cItem.IdCommunity

                oButton = e.Item.FindControl("BTNdeleteCallCommunityAssignment")
                oButton.CommandArgument = cItem.Id.ToString & "," & cItem.IdCommunity
                oButton.Visible = Not isDefault
                Me.Resource.setButton(oButton, True)
        End Select

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
                    Dim assignment As dtoCallPortalAssignment = Me.CurrentPresenter.GetCallPortalAssignments(items)

                    Me.CBLprofileTypes.DataSource = items
                    Me.CBLprofileTypes.DataValueField = "Key"
                    Me.CBLprofileTypes.DataTextField = "Value"
                    Me.CBLprofileTypes.DataBind()
                    If Not IsNothing(assignment) Then
                        Me.CBXselectAllpersonTypes.Checked = assignment.ForAllUsers
                        If Not assignment.ForAllUsers Then
                            For Each i As ListItem In Me.CBLprofileTypes.Items
                                i.Selected = assignment.ProfileTypes.Where(Function(a) a.IdPersonType = CInt(i.Value)).Any()
                            Next
                        End If
                        Me.CBLprofileTypes.Enabled = Not assignment.ForAllUsers
                    Else
                        Me.CBXselectAllpersonTypes.Checked = True
                        Me.CBLprofileTypes.Enabled = False
                    End If
                Else
                    Me.CurrentPresenter.LoadCommunityAssignments(idCommunity, idAssignment, GetTranslatedRoles)
                End If
            Case "deleteitem"
                Me.CurrentPresenter.DeleteCommunityAssignment(idCommunity, idAssignment)
        End Select
    End Sub

#Region "Person Types"
    Private Sub CBXselectAllpersonTypes_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXselectAllpersonTypes.CheckedChanged
        Me.CBLprofileTypes.Enabled = Not CBXselectAllpersonTypes.Checked
    End Sub
    Private Sub BTNclosePersonTypesAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNclosePersonTypesAssignments.Click
        Me.DVselectPersonTypes.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub BTNsavePersonTypesAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNsavePersonTypesAssignments.Click
        Me.DVselectPersonTypes.Visible = False
        Me.CurrentPresenter.SaveSettings(Me.CBXselectAllpersonTypes.Checked, SelectedProfileTypes())

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "Availability")
    End Sub
    Private Function SelectedProfileTypes() As List(Of Integer) Implements IViewEditCallAvailability.SelectedProfileTypes
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
    'Private Sub BTNselectCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNselectCommunity.Click
    '    Me.DVselectCommunity.Visible = False
    '    Me.CurrentPresenter.AddCommunity(Me.CTRLcommunity.GetIdSelectedItems)
    'End Sub
    'Private Sub BTNcloseSelectCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNcloseSelectCommunity.Click
    '    Me.DVselectCommunity.Visible = False
    '    Me.CTRLmessages.Visible = False
    'End Sub
#End Region
#Region "Select roles"
    Private Sub CBXselectAllRoles_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXselectAllRoles.CheckedChanged
        Me.CBLroles.Enabled = Not CBXselectAllRoles.Checked
    End Sub
    Private Sub BTNcloseRoleAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNcloseRoleAssignments.Click
        Me.DVselectRoles.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub BTNsaveRoleAssignments_Click(sender As Object, e As System.EventArgs) Handles BTNsaveRoleAssignments.Click
        Me.DVselectRoles.Visible = False
        Me.CurrentPresenter.SaveSettings(IdEditingCommunity, Me.CBXselectAllRoles.Checked, SelectedRoles())

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "Availability")
    End Sub
#End Region

#End Region

#Region "Page - User Assignments"
    Private Sub RPTusersAssignments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTusersAssignments.ItemDataBound
        Dim oLabel As Label = Nothing
        Dim oLiteral As Literal = Nothing

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCallPersonAssignment = DirectCast(e.Item.DataItem, dtoCallPersonAssignment)
            oLiteral = e.Item.FindControl("LTuserName")
            oLiteral.Text = item.DisplayName

            oLiteral = e.Item.FindControl("LTuserMail")
            If item.IsUnknown Then
                oLiteral.Text = "-"
            Else
                oLiteral.Text = item.AssignedTo.Mail
            End If
            Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBXuser")
            oCheck.Checked = SelectedIdUsers.Contains(item.IdPerson)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            oLabel = e.Item.FindControl("LBuserListTitle")
            Resource.setLabel(oLabel)

            oLiteral = e.Item.FindControl("LTusername_t")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTusermail_t")
            Resource.setLiteral(oLiteral)

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

#Region "Select Call"
    Private Sub CTRLselectUsersFromCall_CloseWindow() Handles CTRLselectUsersFromCall.CloseWindow
        Me.DVselectFromCall.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub CTRLselectUsersFromCall_SelectedItems(idUsers As List(Of Integer)) Handles CTRLselectUsersFromCall.SelectedIdUsers
        Me.DVselectFromCall.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.SaveSettings(idUsers)
    End Sub
#End Region

#Region "Select Users"
    Private Sub CLTRselectUsers_CloseWindow() Handles CLTRselectUsers.CloseWindow
        Me.DVselectUsers.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub CLTRselectUsers_UsersSelected(idUsers As List(Of Integer)) Handles CLTRselectUsers.UsersSelected
        Me.DVselectUsers.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.SaveSettings(idUsers)
    End Sub
#End Region

#End Region

#Region "Add buttons"
    Private Sub BTNaddCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNaddCommunity.Click
        Me.CurrentPresenter.SelectCommunityToAdd()
    End Sub
    Private Sub BTNaddProfile_Click(sender As Object, e As System.EventArgs) Handles BTNaddProfile.Click
        Me.CurrentPresenter.AddUserFromPortal()
    End Sub
    Private Sub BTNaddUserFromCall_Click(sender As Object, e As System.EventArgs) Handles BTNaddUserFromCall.Click
        Me.CurrentPresenter.AddUserFromCall()
    End Sub
    Private Sub BTNaddUserFromCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNaddUserFromCommunity.Click
        Me.CurrentPresenter.AddUserFromCommunity()
    End Sub
#End Region

#End Region

    Private Sub BTNsaveAvailabilityBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveAvailabilityBottom.Click, BTNsaveAvailabilityTop.Click
        Me.CurrentPresenter.SaveSettings()
        SaveSkinSelection()

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "Availability")
    End Sub


End Class