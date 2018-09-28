Imports lm.Comol.Modules.EduPath.Domain
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.BusinessLogic

Public Class TextItem
    Inherits EPpageBaseEduPath

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

#Region "Internal"

    Private _currentStatus As Status = Status.None
    Private Property CurrentStatus As Status
        Get
            If _currentStatus = Status.None Then
                _currentStatus = ServiceEP.GetStatus(RealItemId, ItemType)
            End If
            Return _currentStatus
        End Get
        Set(ByVal value As Status)
            _currentStatus = value
        End Set
    End Property

    Private _text As String
    Private Property Text As String
        Get
            If IsNothing(_text) Then
                _text = ServiceEP.GetTextItem(RealItemId, ItemType)
            End If
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property




    Private ReadOnly Property SessionUniqueKey() As String
        Get
            If Me.ViewState("SessionUniqueKey") Is Nothing Then
                Me.ViewState("SessionUniqueKey") = Me.RealItemId & "_" & Me.CurrentUserId
            End If
            Return Me.ViewState("SessionUniqueKey")
        End Get
    End Property

    Private Property ListOfAssignmentByPerson As List(Of dtoGenericAssWithOldRoleEpAndDelete)
        Get

            If IsNothing(Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString)) Then
                Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString) = New List(Of dtoGenericAssWithOldRoleEpAndDelete)
            End If
            Return Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As List(Of dtoGenericAssWithOldRoleEpAndDelete))
            Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString) = value
        End Set
    End Property
    Private Property ListOfAssignmentByCommRole As List(Of dtoGenericAssignmentWithOldRoleEP)
        Get
            If IsNothing(Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString)) Then
                Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString) = New List(Of dtoGenericAssignmentWithOldRoleEP)
            End If
            Return Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As List(Of dtoGenericAssignmentWithOldRoleEP))
            Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString) = value
        End Set
    End Property

    Private _pathId As Long = 0
    Private ReadOnly Property PathId
        Get
            If _pathId = 0 Then
                _pathId = ServiceEP.GetPathId_ByItemId(ParentId, ItemType - 1)
            End If
            Return _pathId
        End Get
    End Property

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(PathId, lm.Comol.Modules.EduPath.Domain.ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property

    Private _NotIsAutoTimePath As String = "none"
    Public ReadOnly Property NotIsAutoTimePath As String
        Get
            If _NotIsAutoTimePath = "none" Then
                If ServiceEP.CheckEpType(PathType, EPType.TimeAuto) Then
                    _NotIsAutoTimePath = " style='display:none;'"
                Else
                    _NotIsAutoTimePath = "    "
                End If
            End If
            Return _NotIsAutoTimePath
        End Get
    End Property

    Private _itemType As ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.None
    Private ReadOnly Property ItemType As ItemType
        Get
            Dim qs_itemType As String = Request.QueryString("It")
            If _itemType = lm.Comol.Modules.EduPath.Domain.ItemType.None AndAlso IsNumeric(qs_itemType) Then

                Select Case qs_itemType

                    Case lm.Comol.Modules.EduPath.Domain.ItemType.Unit
                        _itemType = lm.Comol.Modules.EduPath.Domain.ItemType.Unit

                    Case lm.Comol.Modules.EduPath.Domain.ItemType.Activity
                        _itemType = lm.Comol.Modules.EduPath.Domain.ItemType.Activity

                    Case Else
                        _itemType = lm.Comol.Modules.EduPath.Domain.ItemType.None
                End Select

            End If
            Return _itemType
        End Get
    End Property

    Private ReadOnly Property QueryItemId() As Long
        Get
            If IsNumeric(Request.QueryString("ItemId")) Then
                Return Request.QueryString("ItemId")
            Else
                Return 0
            End If
        End Get
    End Property


    Private Property RealItemId As Long
        Get
            Return Me.ViewState("RealItemId")
        End Get
        Set(ByVal value As Long)
            Me.ViewState("RealItemId") = value
        End Set
    End Property

    Private ReadOnly Property ParentId As Long
        Get
            If IsNumeric(Request.QueryString("ParentId")) Then
                Return Request.QueryString("ParentId")
            Else
                Return -1
            End If
        End Get
    End Property

    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return PageUtility.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property

    Private ReadOnly Property IdCommunityRole As Integer
        Get
            Dim key As String = "CurrentCommRoleID_" & PathId.ToString() & "_" & CurrentUserId.ToString
            Dim idRole As Integer = ViewStateOrDefault(key, -1)
            If idRole = -1 Then
                idRole = ServiceEP.GetIdCommunityRole(CurrentUserId, ServiceEP.GetPathIdCommunity(PathId))
                ViewState(key) = idRole
            End If
            Return idRole
        End Get
    End Property

    Private ReadOnly Property CurrentStep() As Integer
        Get
            If IsNumeric(Request.QueryString("Step")) AndAlso Request.QueryString("Step") >= -1 AndAlso Request.QueryString("Step") <= 3 Then
                Return Request.QueryString("Step")
            Else
                Return StepTextItem.ErrorStep
            End If
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region " Base"


    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
        Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode)
    End Sub



#End Region

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ServiceTitle = Me.Resource.getValue("TitleTextItem")
        IsMoocPath = ServiceEP.IsMooc(PathId)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.PathView(PathId, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathId), IsMoocPath, PreloadIsFromReadOnly)
        Me.InitWizardButton()
        Select Case CurrentStep
            Case StepTextItem.NewNote
                Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(ItemType, PathId), InteractionType.UserWithLearningObject)
                RealItemId = ServiceEP.SaveActOrUnitNoteDraft(ItemType, ParentId, Me.CurrentUserId, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress, CurrentCommunityID)
                InitSummaryAssignment()
            Case StepTextItem.Update
                Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(ItemType, PathId), InteractionType.UserWithLearningObject)
                RealItemId = QueryItemId
                InitSummaryAssignment()
                Me.CTRLeditorDescription.HTML = Text
            Case StepTextItem.SelectPermission
                Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Assignment, "0"), InteractionType.UserWithLearningObject)
                RealItemId = QueryItemId
                Me.InitSelectPermission()
            Case StepTextItem.SelectPerson
                RealItemId = QueryItemId
                Me.InitSelectPerson()
            Case Else
                Me.ShowError(EpError.Url)
        End Select
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        If ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Unit Then
            If QueryItemId > 0 Then
                Return ServiceEP.CheckCommunityId(Of Unit)(QueryItemId, CurrentCommunityID) AndAlso ServiceEP.AdminCanUpdate(QueryItemId, ItemType, CurrentUserId, IdCommunityRole)
            Else
                Return ServiceEP.HasPermessi_ByItem(Of Path)(ParentId, CurrentCommunityID) AndAlso ServiceEP.AdminCanUpdate(ParentId, lm.Comol.Modules.EduPath.Domain.ItemType.Path, CurrentUserId, IdCommunityRole)
            End If
        ElseIf ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Activity Then
            If QueryItemId > 0 Then
                Return ServiceEP.CheckCommunityId(Of Activity)(Me.QueryItemId, Me.CurrentCommunityID) AndAlso ServiceEP.AdminCanUpdate(QueryItemId, ItemType, CurrentUserId, IdCommunityRole)
            Else
                Return ServiceEP.HasPermessi_ByItem(Of Unit)(ParentId, CurrentCommunityID) AndAlso ServiceEP.AdminCanUpdate(ParentId, lm.Comol.Modules.EduPath.Domain.ItemType.Unit, CurrentUserId, IdCommunityRole)
            End If
        Else
            Return False
        End If
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_SubActText", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(Me.HYPerror, False, True)
            .setLabel(Me.LBdetailTitle)
            .setLabel(Me.LBdescriptionTitle)
            .setLabel(Me.LBpersonSummary)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        Me.Master.ServiceTitle = Me.Resource.getValue("TitleTextItem")
        MLVtextItem.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub


#End Region

    Private Sub InitSelectPerson()
        Me.WZRnoteCreate.ActiveStepIndex = StepTextItem.SelectPerson
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBuserTitle)

        Dim ListCommunityID As New List(Of Integer)
        ListCommunityID.Add(Me.CurrentCommunityID)
        Dim PersonToHide As List(Of Integer) = Me.ServiceAssignment.GetIdFromDtoAssignementGenericWithOldRoleEP(Me.ListOfAssignmentByPerson)
        Me.CTRLselectUser.CurrentPresenter.Init(ListCommunityID, ListSelectionMode.Multiple, PersonToHide)
    End Sub

    Private Sub ClearSession()
        If Not IsNothing(SessionUniqueKey) Then
            Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString) = Nothing
            Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString) = Nothing
        End If
    End Sub

    Private Sub InitSelectPermission()
        Me.WZRnoteCreate.ActiveStepIndex = StepTextItem.SelectPermission
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBpersonPermission)
        Me.Resource.setLabel(Me.LBcrolePermission)
        Me.Resource.setButton(Me.BTNselectPerson)
        Me.Resource.setLabel(LBpermissionTitle)

        If Me.ListOfAssignmentByCommRole.Count = 0 Then
            Me.Resource.setLabel(Me.LBcrolePermissionNoAss)
            Me.LBcrolePermissionNoAss.Visible = True
        Else
            Me.RPcrolePermission.DataSource = Me.ListOfAssignmentByCommRole
            Me.RPcrolePermission.DataBind()
        End If

        If Me.ListOfAssignmentByPerson.Count = 0 Then
            Me.Resource.setLabel(Me.LBpersonPermissionNoAss)
            Me.LBpersonPermissionNoAss.Visible = True
        Else
            Me.RPuserPermission.DataSource = Me.ListOfAssignmentByPerson
            Me.RPuserPermission.DataBind()
        End If
    End Sub

    Private Sub InitSummaryAssignment()
        InitListOfAssignment()
        Dim ListActiveCRoleAssignment As List(Of dtoGenericAssignment) = Me.ServiceAssignment.GetActiveAssignment(Me.ListOfAssignmentByCommRole)

        Dim ListActivePersonAssignment As List(Of dtoGenericAssignment) = Me.ServiceAssignment.GetActiveAssignment(Me.ListOfAssignmentByPerson)
        If ListActivePersonAssignment.Count > 0 Then
            RPpersonSummary.DataSource = ListActivePersonAssignment
            RPpersonSummary.DataBind()
        Else
            LBpersonSummary.Visible = False
        End If
    End Sub

    Private Sub InitListOfAssignment()
        If ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Activity Then

            If Me.ListOfAssignmentByPerson.Count = 0 Then
                If ServiceEP.CheckStatus(CurrentStatus, Status.Draft) Then
                    Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonActivityAssignment(0, Me.ParentId)
                Else
                    Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonActivityAssignment(Me.RealItemId, Me.ParentId)
                End If
            End If
            If Me.ListOfAssignmentByCommRole.Count = 0 Then
                If ServiceEP.CheckStatus(CurrentStatus, Status.Draft) Then
                    Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleActivityAssignment(0, Me.ParentId, Me.CurrentCommunityID, PageUtility.CurrentContext.UserContext.CurrentUserID, PageUtility.CurrentContext.UserContext.Language.Id)
                Else
                    Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleActivityAssignment(Me.RealItemId, Me.ParentId, Me.CurrentCommunityID, PageUtility.CurrentContext.UserContext.CurrentUserID, PageUtility.CurrentContext.UserContext.Language.Id)
                End If
            End If

        Else

            'is Unit
            If Me.ListOfAssignmentByPerson.Count = 0 Then
                If ServiceEP.CheckStatus(CurrentStatus, Status.Draft) Then
                    Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonUnitAssignment(0, Me.PathId)
                Else
                    Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonUnitAssignment(Me.RealItemId, Me.PathId)
                End If
            End If
            If Me.ListOfAssignmentByCommRole.Count = 0 Then
                If ServiceEP.CheckStatus(CurrentStatus, Status.Draft) Then
                    Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleUnitAssignment(0, Me.PathId, Me.CurrentCommunityID, PageUtility.CurrentContext.UserContext.CurrentUserID, PageUtility.CurrentContext.UserContext.Language.Id)
                Else
                    Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleUnitAssignment(Me.RealItemId, Me.PathId, Me.CurrentCommunityID, PageUtility.CurrentContext.UserContext.CurrentUserID, PageUtility.CurrentContext.UserContext.Language.Id)
                End If
            End If
        End If

    End Sub


    Private Sub SubGetSelectedPermissionByCRole()
        Me.ListOfAssignmentByCommRole.Clear()
        Dim oLabel As Label
        Dim oCkb As CheckBox
        Dim oDtoAssign As dtoGenericAssignmentWithOldRoleEP
        For Each item As RepeaterItem In Me.RPcrolePermission.Items
            If (item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.Item) Then
                oDtoAssign = New dtoGenericAssignmentWithOldRoleEP
                oLabel = item.FindControl("LBcommRole")
                If Not IsNothing(oLabel) Then
                    oDtoAssign.ItemID = oLabel.Attributes.Item("CRoleID")
                    oDtoAssign.DB_ID = oLabel.Attributes.Item("AssignmentID")
                    oDtoAssign.OldRoleEP = oLabel.Attributes.Item("OldRoleEP")
                    oDtoAssign.ItemName = oLabel.Text
                End If
                oCkb = item.FindControl("CKBpartecipant")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.Participant
                    End If
                End If
                oCkb = item.FindControl("CKBevaluator")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.Evaluator Or oDtoAssign.RoleEP
                    End If
                End If
                oCkb = item.FindControl("CKBmanager")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.Manager Or oDtoAssign.RoleEP
                    End If
                End If


            End If
            Me.ListOfAssignmentByCommRole.Add(oDtoAssign)
        Next
    End Sub

    Private Sub SubGetSelectedPermissionByPerson()
        Me.ListOfAssignmentByPerson.Clear()
        Dim oLabel As Label
        Dim oCkb As CheckBox
        Dim oDtoAssign As dtoGenericAssWithOldRoleEpAndDelete
        For Each item As RepeaterItem In Me.RPuserPermission.Items
            If (item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.Item) Then

                oDtoAssign = New dtoGenericAssWithOldRoleEpAndDelete
                oLabel = item.FindControl("LBperson")
                oDtoAssign.ItemID = oLabel.Attributes.Item("PersonID")
                oDtoAssign.ItemName = oLabel.Text
                oDtoAssign.DB_ID = oLabel.Attributes.Item("AssignmentID")
                oDtoAssign.OldRoleEP = oLabel.Attributes.Item("OldRoleEP")

                oCkb = item.FindControl("CKBpartecipant")
                If oCkb.Checked Then
                    oDtoAssign.RoleEP = RoleEP.Participant
                End If

                oCkb = item.FindControl("CKBevaluator")
                If oCkb.Checked Then
                    oDtoAssign.RoleEP = RoleEP.Evaluator Or oDtoAssign.RoleEP
                End If

                oCkb = item.FindControl("CKBmanager")
                If oCkb.Checked Then
                    oDtoAssign.RoleEP = RoleEP.Manager Or oDtoAssign.RoleEP
                End If

                oCkb = item.FindControl("CKBactivePerson")
                oDtoAssign.isDeleted = Not oCkb.Checked
                Me.ListOfAssignmentByPerson.Add(oDtoAssign)
            End If

        Next
    End Sub



#Region "Button"

    Private Sub InitWizardButton()
        Dim oButton As Button
        oButton = Me.WZRnoteCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNcancel")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
        End If
        oButton = Me.WZRnoteCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNprevious")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
            oButton.Visible = (Me.CurrentStep = StepTextItem.SelectPermission Or Me.CurrentStep = StepTextItem.SelectPerson)
        End If

        oButton = Me.WZRnoteCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNedit")
        If Not IsNothing(oButton) Then
            If CurrentStep = StepTextItem.NewNote Or CurrentStep = StepTextItem.Update Then
                Me.Resource.setButton(oButton)
                oButton.Visible = True
            Else
                oButton.Visible = False
            End If

        End If

        oButton = Me.WZRnoteCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNnext")
        If Not IsNothing(oButton) Then
            If Me.CurrentStep = StepTextItem.NewNote Or CurrentStep = StepTextItem.Update Then
                Me.Resource.setButtonByValue(oButton, "complete", True)
            Else
                Me.Resource.setButtonByValue(oButton, "next", True)
            End If
        End If
    End Sub

    Public Sub BTNnext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Select Case Me.CurrentStep
            Case StepTextItem.NewNote

                Text = Me.CTRLeditorDescription.HTML
                If ServiceEP.CheckStatus(CurrentStatus, Status.Draft) Then
                    CurrentStatus = CurrentStatus - Status.Draft
                End If
                If PersistData() Then
                    PageUtility.RedirectToUrl(RootObject.PathView(PathId, CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathId), IsMoocPath, PreloadIsFromReadOnly))
                Else
                    Me.ShowError(EpError.Generic)
                End If

                'Case StepTextItem.Detail
                '    _isDraft = False
                '    If PersistData() Then
                '        PageUtility.RedirectToUrl(RootObject.PathView(PathId, CurrentCommunityID, EpViewModeType.Manage))
                '    Else
                '        Me.ShowError(EpError.Generic)
                '    End If

            Case StepTextItem.SelectPermission
                Me.SubGetSelectedPermissionByCRole()
                Me.SubGetSelectedPermissionByPerson()
                PageUtility.RedirectToUrl(RootObject.TextItem(RealItemId, ParentId, ItemType, CurrentCommunityID, StepTextItem.Update, IsMoocPath, PreloadIsFromReadOnly))


            Case StepTextItem.SelectPerson
                Me.GetSelectedPerson()
                PageUtility.RedirectToUrl(RootObject.TextItem(RealItemId, ParentId, ItemType, CurrentCommunityID, StepTextItem.SelectPermission, IsMoocPath, PreloadIsFromReadOnly))

            Case StepTextItem.Update
                If ServiceEP.CheckStatus(CurrentStatus, Status.Draft) Then
                    CurrentStatus = CurrentStatus - Status.Draft
                End If
                Text = Me.CTRLeditorDescription.HTML
                If PersistData() Then
                    RedirectToUrl(RootObject.PathView(PathId, CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathId), IsMoocPath, PreloadIsFromReadOnly))
                Else
                    Me.ShowError(EpError.Generic)
                End If
        End Select
    End Sub

    Public Sub BTNedit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Text = Me.CTRLeditorDescription.HTML
        If Me.PersistData() Then
            PageUtility.RedirectToUrl(RootObject.TextItem(RealItemId, ParentId, ItemType, CurrentCommunityID, StepTextItem.SelectPermission, IsMoocPath, PreloadIsFromReadOnly))
        Else
            Me.ShowError(EpError.Generic)
        End If
    End Sub

    Public Sub BTNprevious_Click(ByVal sender As Object, ByVal e As EventArgs)
        Select Case Me.CurrentStep
            Case StepTextItem.SelectPermission
                Me.SubGetSelectedPermissionByCRole()
                Me.SubGetSelectedPermissionByPerson()
                Me.PageUtility.RedirectToUrl(RootObject.UpdateTextItem(RealItemId, ParentId, ItemType, CurrentCommunityID, IsMoocPath, PreloadIsFromReadOnly))

            Case StepTextItem.SelectPerson
                Me.GetSelectedPerson()
                Me.PageUtility.RedirectToUrl(RootObject.TextItem(RealItemId, ParentId, ItemType, CurrentCommunityID, StepTextItem.SelectPermission, IsMoocPath, PreloadIsFromReadOnly))
        End Select
    End Sub

    Public Sub BTNcancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        If ServiceEP.CheckStatus(CurrentStatus, Status.Draft) AndAlso ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Unit Then
            If Not ServiceEP.DeleteOnlyUnit(RealItemId) Then
                ShowError(EpError.Generic)
            End If
        ElseIf ServiceEP.CheckStatus(CurrentStatus, Status.Draft) AndAlso ItemType = lm.Comol.Modules.EduPath.Domain.ItemType.Activity Then
            If Not ServiceEP.DeleteOnlyActivity(RealItemId) Then
                ShowError(EpError.Generic)
            End If
        End If
        ClearSession()

        PageUtility.RedirectToUrl(RootObject.PathView(Me.PathId, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathId), IsMoocPath, PreloadIsFromReadOnly))

    End Sub

    Private Sub BTNselectPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNselectPerson.Click
        Me.SubGetSelectedPermissionByCRole()
        Me.SubGetSelectedPermissionByPerson()
        PageUtility.RedirectToUrl(RootObject.TextItem(RealItemId, ParentId, ItemType, CurrentCommunityID, StepTextItem.SelectPerson, IsMoocPath, PreloadIsFromReadOnly))
    End Sub

#End Region

    Private Sub GetSelectedPerson()
        Me.ListOfAssignmentByPerson.AddRange(ServiceAssignment.GetDtoAssignemtGenericWithOldRoleEPAndDelFromMembers(Me.CTRLselectUser.CurrentPresenter.GetConfirmedUsers))
    End Sub

#Region "RP ItemDataBound"

    Public Sub RPsummary_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim oLabel As Label
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oImg As System.Web.UI.WebControls.Image
            Dim oDto As dtoGenericAssignment
            Dim Text As String
            Try
                oDto = DirectCast(e.Item.DataItem, dtoGenericAssignment)
                oLabel = e.Item.FindControl("LBname")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = oDto.ItemName
                End If
                oImg = e.Item.FindControl("IMGpartecipant")
                If (oDto.RoleEP And RoleEP.Participant) = RoleEP.Participant Then
                    Text = Me.Resource.getValue(RoleEP.Participant.ToString & ".True")
                    oImg.ImageUrl = Me.BaseUrl & "Modules/EduPath/img/verde.gif"
                    oImg.ToolTip = Text
                    oImg.AlternateText = Text
                Else
                    e.Item.Controls.Remove(oImg)
                    '    Text = Me.Resource.getValue(RoleEP.Participant.ToString & ".False")
                    '    oImg.ImageUrl = Me.BaseUrl & "Modules/EduPath/img/rosso.gif"
                    '    oImg.ToolTip = Text
                    '    oImg.AlternateText = Text
                End If

                oImg = e.Item.FindControl("IMGevaluator")
                If Not IsNothing(oImg) Then
                    If (oDto.RoleEP And RoleEP.Evaluator) = RoleEP.Evaluator Then
                        Text = Me.Resource.getValue(RoleEP.Evaluator.ToString & ".True")
                        oImg.ImageUrl = RootObject.ImgGreen(Me.BaseUrl)
                        oImg.ToolTip = Text
                        oImg.AlternateText = Text
                    Else
                        e.Item.Controls.Remove(oImg)
                        'Text = Me.Resource.getValue(RoleEP.Evaluator.ToString & ".False")
                        'oImg.ImageUrl = Me.BaseUrl & "Modules/EduPath/img/rosso.gif"
                        'oImg.ToolTip = Text
                        'oImg.AlternateText = Text
                    End If
                End If
                oImg = e.Item.FindControl("IMGmanager")
                If Not IsNothing(oImg) Then
                    If (oDto.RoleEP And RoleEP.Manager) = RoleEP.Manager Then
                        Text = Me.Resource.getValue(RoleEP.Manager.ToString & ".True")
                        oImg.ImageUrl = RootObject.ImgGreen(Me.BaseUrl)
                        oImg.ToolTip = Text
                        oImg.AlternateText = Text
                    Else
                        e.Item.Controls.Remove(oImg)
                        'Text = Me.Resource.getValue(RoleEP.Manager.ToString & ".False")
                        'oImg.ImageUrl = Me.BaseUrl & "Modules/EduPath/img/rosso.gif"
                        'oImg.ToolTip = Text
                        'oImg.AlternateText = Text
                    End If
                End If

            Catch ex As Exception

            End Try
        ElseIf e.Item.ItemType = ListItemType.Header Then
            oLabel = e.Item.FindControl("LBnameCroleTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RPtitle.CRole")
            End If
            oLabel = e.Item.FindControl("LBnamePersonTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RPtitle.Person")
            End If
            oLabel = e.Item.FindControl("LBpartecipantTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Participant.ToString)
            End If
            oLabel = e.Item.FindControl("LBevaluatorTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Evaluator.ToString)
            End If
            oLabel = e.Item.FindControl("LBmanagerTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Manager.ToString)
            End If
        End If
    End Sub

    Private Sub RPcrolePermission_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPcrolePermission.ItemDataBound
        Dim oLabel As Label
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dtoCRoleAss As dtoGenericAssignmentWithOldRoleEP
            Dim oCkb As CheckBox
            Try
                dtoCRoleAss = DirectCast(e.Item.DataItem, dtoGenericAssignmentWithOldRoleEP)
                oLabel = e.Item.FindControl("LBcommRole")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = dtoCRoleAss.ItemName
                    oLabel.Attributes.Add("CRoleID", dtoCRoleAss.ItemID.ToString)
                    oLabel.Attributes.Add("AssignmentID", dtoCRoleAss.DB_ID.ToString)
                    oLabel.Attributes.Add("OldRoleEP", dtoCRoleAss.OldRoleEP)
                End If
                oCkb = e.Item.FindControl("CKBpartecipant")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Participant.ToString)
                    oCkb.Checked = (dtoCRoleAss.RoleEP And RoleEP.Participant) = RoleEP.Participant
                End If
                oCkb = e.Item.FindControl("CKBevaluator")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Evaluator.ToString)
                    oCkb.Checked = (dtoCRoleAss.RoleEP And RoleEP.Evaluator) = RoleEP.Evaluator
                End If
                oCkb = e.Item.FindControl("CKBmanager")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Manager.ToString)
                    oCkb.Checked = (dtoCRoleAss.RoleEP And RoleEP.Manager) = RoleEP.Manager

                End If
            Catch ex As Exception
            End Try
        ElseIf e.Item.ItemType = ListItemType.Header Then
            oLabel = e.Item.FindControl("LBcommRoleTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RPtitle.CRole")
            End If
            oLabel = e.Item.FindControl("LBpartecipantTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Participant.ToString)
            End If
            oLabel = e.Item.FindControl("LBevaluatorTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Evaluator.ToString)
            End If
            oLabel = e.Item.FindControl("LBmanagerTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Manager.ToString)
            End If
        End If
    End Sub

    Private Sub RPuserPermission_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPuserPermission.ItemDataBound
        Dim oLabel As Label
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dtoPersonAss As dtoGenericAssWithOldRoleEpAndDelete
            Dim oCkb As CheckBox

            dtoPersonAss = e.Item.DataItem
            oLabel = e.Item.FindControl("LBperson")

            oLabel.Text = dtoPersonAss.ItemName
            oLabel.Attributes.Add("PersonID", dtoPersonAss.ItemID.ToString)
            oLabel.Attributes.Add("AssignmentID", dtoPersonAss.DB_ID.ToString)
            oLabel.Attributes.Add("OldRoleEP", dtoPersonAss.OldRoleEP)

            oCkb = e.Item.FindControl("CKBpartecipant")

            oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Participant.ToString)
            oCkb.Checked = (dtoPersonAss.RoleEP And RoleEP.Participant) = RoleEP.Participant

            oCkb = e.Item.FindControl("CKBevaluator")

            oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Evaluator.ToString)
            oCkb.Checked = (dtoPersonAss.RoleEP And RoleEP.Evaluator) = RoleEP.Evaluator

            oCkb = e.Item.FindControl("CKBmanager")

            oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Manager.ToString)
            oCkb.Checked = (dtoPersonAss.RoleEP And RoleEP.Manager) = RoleEP.Manager

            oCkb = e.Item.FindControl("CKBactivePerson")

            oCkb.Text = Me.Resource.getValue("Select")

            If (dtoPersonAss.RoleEP And RoleEP.Participant) = RoleEP.Participant Then
                oCkb.Enabled = False
                oCkb.Checked = True
            Else
                oCkb.Checked = Not dtoPersonAss.isDeleted
            End If


        ElseIf e.Item.ItemType = ListItemType.Header Then

            oLabel = e.Item.FindControl("LBpersonTitle")

            oLabel.Text = Me.Resource.getValue("RPtitle.Person")

            oLabel = e.Item.FindControl("LBpartecipantTitle")

            oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Participant.ToString)

            oLabel = e.Item.FindControl("LBevaluatorTitle")

            oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Evaluator.ToString)

            oLabel = e.Item.FindControl("LBmanagerTitle")

            oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.Manager.ToString)
        End If
    End Sub

#End Region

    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setHyperLink(Me.HYPerror, False, True)
        Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage, IsMoocPath)
        Select Case ErrorType
            Case EpError.Generic
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & ErrorType.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & ErrorType.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & ErrorType.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.PathNotFind
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & ErrorType.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)

        End Select
        Me.MLVtextItem.ActiveViewIndex = 1
    End Sub



    Private Function PersistData()
        Dim transactionOk As Boolean

        transactionOk = ServiceEP.SaveOrUpdateTextItem(RealItemId, Text, CurrentStatus, ItemType, ParentId, ListOfAssignmentByCommRole, ListOfAssignmentByPerson, CurrentUserId, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress, Me.CurrentCommunityID, LinguaID)
        Return transactionOk
    End Function


End Class

Public Enum StepTextItem
    ErrorStep = -1
    NewNote = 0
    SelectPermission = 1
    SelectPerson = 2
    Update = 3
End Enum
