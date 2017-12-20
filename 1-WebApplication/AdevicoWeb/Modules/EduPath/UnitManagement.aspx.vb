Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class UnitManagement
    Inherits PageBaseEduPath

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(PathID, ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property

#Region "Property"

    Public ReadOnly Property RoleEpCount As String
        Get
            If isAutoEp Then
                Return "2"
            Else
                Return "3"
            End If
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

    Private ReadOnly Property PathID() As Long
        Get
            If IsNumeric(Request.QueryString("PId")) Then
                Return Request.QueryString("PId")
            Else
                Return -1
            End If
        End Get
    End Property

    Private ReadOnly Property CurrentStep() As Integer
        Get
            If IsNumeric(Request.QueryString("Step")) AndAlso Request.QueryString("Step") >= -2 AndAlso Request.QueryString("Step") <= 2 Then
                Return Request.QueryString("Step")
            Else
                Return StepUnitManagement.ErrorStep
            End If
        End Get
    End Property

    Private ReadOnly Property SessionUniqueKey() As String
        Get
            If Me.ViewState("SessionUniqueKey") Is Nothing Then
                Me.ViewState("SessionUniqueKey") = Me.CurrentUnitID & "_" & Me.UtenteCorrente.ID
            End If
            Return Me.ViewState("SessionUniqueKey")
        End Get

    End Property

    Private Property CurrentUnitID() As Long
        Get
            If IsNumeric(Me.ViewState("CurrentUnitID")) And Me.ViewState("CurrentUnitID") > 0 Then
                Return Me.ViewState("CurrentUnitID")
            End If
            Dim qs_unitId As String = Me.Request.QueryString("UId")
            If IsNumeric(qs_unitId) Then
                Me.ViewState("CurrentUnitID") = qs_unitId
                Return qs_unitId
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentUnitID") = value
        End Set
    End Property

    Private ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return Me.CurrentContext.UserContext.CurrentCommunityID
            End If
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

    Private _currentUnit As Unit
    Private Property currentUnit As Unit
        Get
            If IsNothing(_currentUnit) Then
                _currentUnit = New Unit
            End If
            If _currentUnit.Id = 0 AndAlso CurrentUnitID > 0 Then
                _currentUnit = Me.ServiceEP.GetUnit(Me.CurrentUnitID)
            End If
            Return _currentUnit
        End Get
        Set(ByVal value As Unit)
            If IsNothing(_currentUnit) Then
                _currentUnit = New Unit
            End If
            _currentUnit = value
        End Set
    End Property
#End Region

    Private Sub ClearSession()
        If Not IsNothing(SessionUniqueKey) Then
            Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString) = Nothing
            Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString) = Nothing
            Me.Session("CurrentModeType_" & SessionUniqueKey.ToString) = Nothing
        End If
    End Sub

#Region " Base"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub BindDati()
     
        If Not IsPostBack Then
            Select Case Me.CurrentStep
                Case StepUnitManagement.NewUnit
                    If VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Unit, "0"), InteractionType.UserWithLearningObject)
                        'SetHelpImg()
                        Me.InitNewUnit()
                        CurrentUnitID = currentUnit.Id
                    Else
                        Me.ShowError(EpError.Url)
                    End If

                Case StepUnitManagement.Update
                    Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Unit, Me.CurrentUnitID), InteractionType.UserWithLearningObject)
                    If VerifyUrl() Then
                        'SetHelpImg()
                        Me.InitUpdate()
                    Else
                        Me.ShowError(EpError.Url)
                    End If

                Case StepUnitManagement.Detail
                    If VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Unit, Me.CurrentUnitID), InteractionType.UserWithLearningObject)
                        'SetHelpImg()
                        Me.InitDetail()
                    Else
                        Me.ShowError(EpError.Url)
                    End If
                Case StepUnitManagement.SelectPermission
                    If VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Assignment, "0"), InteractionType.UserWithLearningObject)
                        'SetHelpImg()
                        Me.InitSelectPermission()
                    Else
                        Me.ShowError(EpError.Url)
                    End If
                Case StepUnitManagement.SelectPerson
                    If VerifyUrl() Then
                        Me.InitSelectPerson()
                    Else
                        Me.ShowError(EpError.Url)
                    End If
                Case Else
                    Me.ShowError(EpError.Generic)
            End Select
            setControls_ByEpType()
        ElseIf Me.CurrentStep = StepUnitManagement.SelectPermission Then
            Me.InitSelectPermission()
        End If
        CTRLhelpRole.Visible = True
                CTRLhelpVisibility.Visible = True
                Me.CTRLhelpRole.InitDialog(Not isTimeEp)
                Me.CTRLhelpVisibility.InitDialog()

    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.ShowError(EpError.NotPermission)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        'Dim value As Boolean
        'With Me.CurrentService
        '    value = .Admin
        'End With
        'Return value
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Create", "EduPath") 'da modificare
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBcroleSummary)
            .setLabel(LBpersonSummary)
            .setLabel_To_Value(Me.LBdetailTitle, "LBdetailTitle.Unit")
            .setLabel(Me.LBnameTitle)
            .setLabel(Me.LBdescriptionTitle)
            .setCheckBox(Me.CKBvisibilityUnit)
            .setCheckBox(Me.CKBmandatoryUnit)
            .setLabel(Me.LBminCompletionTitle)
            .setLabel(LBweight)
            .setLabel(LBminMark)
            .setCompareValidator(COVweight)
            .setCompareValidator(COVminCompletion, COVweight)
            .setCompareValidator(COVminMark, COVweight)
            .setRangeValidator(RNVminMark)
            .setRangeValidator(RNVminCompletion, RNVminMark)
            .setLabel_To_Value(LBpoints, "Points")
            .setLabel(Me.LbMincompletionHelp)
            .setRequiredFieldValidator(Me.RFVminCompl, True, False)
            .setRequiredFieldValidator(Me.RFVminMark, True, False)
            .setRequiredFieldValidator(Me.RFVweight, True, False)
            Me.Master.ServiceTitle = .getValue("ManUnit")
            .setLabel(LBadvanced)
            .setRequiredFieldValidator(RFVname, True, False)
            .setLabel(Me.LBselectPermHelpUnit)
            .setLabel(Me.LBpermissionHelpUnit)
            .setLabel(Me.LBmarkHelpUnit)
            .setLabel(Me.LBweightHelpUnit)
            .setLabel_To_Value(Me.LbMincompletionHelp, "LbMincompletionHelp.Manual.Unit")
        End With

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                If CurrentUnitID > 0 Then
                    RedirectOnSessionTimeOut(RootObject.UpdateUnit(Me.CurrentCommunityID, CurrentUnitID, PathID), CurrentCommunityID)
                Else
                    RedirectOnSessionTimeOut(RootObject.NewUnit(Me.CurrentCommunityID, PathID), CurrentCommunityID)
                End If
            End If
            Return False
        End Get
    End Property

#End Region

    Private Sub setControls_ByEpType()
        If isAutoEp Then
            hideControl(DIVcompletion)
        End If
    End Sub
    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setButton(Me.BTNerror)
        Select Case ErrorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        End Select
        Me.MLVunitCreate.ActiveViewIndex = 1
    End Sub

    Private Function GetItemListStatus(ByVal oStatus As Status) As ListItem
        Dim oItem As New ListItem
        oItem.Text = Me.Resource.getValue("Status." & oStatus.ToString)
        oItem.Value = oStatus
        If Me.CurrentStep = StepUnitManagement.Detail Or Me.CurrentStep = StepUnitManagement.Update Then
            oItem.Selected = (Me.currentUnit.Status And oStatus) = oStatus
        End If
        Return oItem
    End Function

    Private Sub InitUpdate()
        currentUnit = Me.ServiceEP.GetUnit(Me.CurrentUnitID)
        If IsNothing(currentUnit) Then
            Me.ShowError(EpError.Generic)
        Else
            Me.InitDetail()
        End If

    End Sub

    Private Sub InitNewUnit()
        'Me.Resource.setLabel(LBcroleSummary)
        currentUnit = New Unit
        currentUnit.Weight = IIf(isAutoEp, 0, currentUnit.Weight)
        currentUnit.Status = currentUnit.Status Or Status.Draft
        Me.SubInitSelectPermissionByCRole()
        Me.SubInitSelectPermissionByPerson()
        Me.ServiceEP.SaveUnit(currentUnit, PathID, Me.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress, CurrentCommunityID)
        If IsNothing(currentUnit) Then
            ShowError(EpError.Generic)
        Else
            Me.InitDetail()
        End If
    End Sub


    Private Sub InitDetail()

        Me.WZRunitCreate.ActiveStepIndex = StepUnitManagement.Detail
        Me.InitWizardButton()

        If isAutoEp Then
            Me.DIVmandatory.Visible = False
            hideControl(Me.DIVmandatory)
        End If

        Me.TXBname.Text = Me.currentUnit.Name

        Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode)
        Me.CTRLeditorDescription.HTML = Me.currentUnit.Description
        Me.CKBvisibilityUnit.Checked = Not ((currentUnit.Status And Status.Locked) = Status.Locked)
        Me.TXBminCompletion.Text = currentUnit.MinCompletion
        Me.TXBminMark.Text = currentUnit.MinMark
        Me.TXBweight.Text = currentUnit.Weight
        Me.CKBmandatoryUnit.Checked = (Me.currentUnit.Status And Status.Mandatory) = Status.Mandatory

        Dim ListActiveCRoleAssignment As List(Of dtoGenericAssignment) = Me.ServiceAssignment.GetActiveAssignment(Me.ListOfAssignmentByCommRole)
        If ListActiveCRoleAssignment.Count > 0 Then
            RPcRoleSummary.DataSource = ListActiveCRoleAssignment
            RPcRoleSummary.DataBind()
        Else
            LBcroleSummary.Visible = False
        End If
        Dim ListActivePersonAssignment As List(Of dtoGenericAssignment) = Me.ServiceAssignment.GetActiveAssignment(Me.ListOfAssignmentByPerson)
        If ListActivePersonAssignment.Count > 0 Then
            RPpersonSummary.DataSource = ListActivePersonAssignment
            RPpersonSummary.DataBind()
        Else
            LBpersonSummary.Visible = False
        End If

    End Sub

    Private Sub UpdateDetail()
        currentUnit.Name = Me.TXBname.Text
        currentUnit.Description = removeBRfromStringEnd(Me.CTRLeditorDescription.HTML)
        If Me.CKBvisibilityUnit.Checked Then
            If (currentUnit.Status And Status.Locked) = Status.Locked Then
                currentUnit.setNotLocked()
            End If
        Else
            currentUnit.setLocked()
        End If
        If CKBmandatoryUnit.Checked Then
            currentUnit.setMandatory()
        ElseIf Not CKBmandatoryUnit.Checked AndAlso ((currentUnit.Status And Status.Mandatory) = Status.Mandatory) Then
            currentUnit.setNotMandatory()
        End If
        currentUnit.MinCompletion = IIf(IsNumeric(Me.TXBminCompletion.Text), Me.TXBminCompletion.Text, 0)
        currentUnit.MinMark = IIf(IsNumeric(TXBminMark.Text), TXBminMark.Text, 0)
        currentUnit.Weight = IIf(IsNumeric(TXBweight.Text), TXBweight.Text, 0)
    End Sub

    Private Sub InitSelectPerson()
        Me.WZRunitCreate.ActiveStepIndex = StepUnitManagement.SelectPerson
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBuserTitle)

        Dim ListCommunityID As New List(Of Integer)
        ListCommunityID.Add(Me.CurrentCommunityID)
        Dim PersonToHide As List(Of Integer) = Me.ServiceAssignment.GetIdFromDtoAssignementGenericWithOldRoleEP(Me.ListOfAssignmentByPerson)
        Me.CTRLselectUser.CurrentPresenter.Init(ListCommunityID, ListSelectionMode.Multiple, PersonToHide)
    End Sub

    Private Sub GetSelectedPerson()
        Me.ListOfAssignmentByPerson.AddRange(ServiceAssignment.GetDtoAssignemtGenericWithOldRoleEPAndDelFromMembers(Me.CTRLselectUser.CurrentPresenter.GetConfirmedUsers))
    End Sub

    Private Sub InitSelectPermission()
        Me.WZRunitCreate.ActiveStepIndex = StepUnitManagement.SelectPermission
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBpersonPermission)
        Me.Resource.setLabel(Me.LBcrolePermission)
        Me.Resource.setButton(Me.BTNselectPerson)
        Me.Resource.setLabel_To_Value(Me.LBpermissionTitle, "LBpermissionTitle.Unit")
        Me.SubInitSelectPermissionByCRole()
        Me.SubInitSelectPermissionByPerson()
    End Sub

    Private Sub GetSelectedPermission()
        Me.SubGetSelectedPermissionByCRole()
        Me.SubGetSelectedPermissionByPerson()
    End Sub

    Private Function VerifyUrl() As Boolean
        If Me.CurrentStep <> StepUnitManagement.NewUnit And Me.CurrentUnitID = -1 Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        If Me.PathID = -1 Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        If Me.CurrentCommunityID = 0 And (Me.CurrentStep <> StepUnitManagement.Update And Me.CurrentStep <> StepUnitManagement.NewUnit) Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        Return True
    End Function

    'Public Sub SetHelpImg()
    'SetHelpImg(IMGhelpSelectPerm)
    'SetHelpImg(IMGhelp)
    'SetHelpImg(IMGminComplHelp)
    'SetHelpImg(IMGvisibilityHelp)
    'SetHelpImg(IMGhelpWeight)
    'SetHelpImg(IMGminMarklHelp)
    'End Sub


    Private Sub SubInitSelectPermissionByCRole() 'attualmente carico solo i ruoli selezionti come manager o valutatori nel path
        If Me.ListOfAssignmentByCommRole.Count = 0 Then
            If (currentUnit.Status And Status.Draft) = Status.Draft Then
                Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleUnitAssignment(0, Me.PathID, Me.CurrentCommunityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentContext.UserContext.Language.Id)
            Else
                Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleUnitAssignment(Me.CurrentUnitID, Me.PathID, Me.CurrentCommunityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentContext.UserContext.Language.Id)
            End If
        End If
        If Me.ListOfAssignmentByCommRole.Count = 0 Then
            Me.Resource.setLabel(Me.LBcrolePermissionNoAss)
            Me.LBcrolePermissionNoAss.Visible = True
        Else
            Me.RPcrolePermission.DataSource = Me.ListOfAssignmentByCommRole
            Me.RPcrolePermission.DataBind()
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

    Private Sub SubInitSelectPermissionByPerson()
        If Me.ListOfAssignmentByPerson.Count = 0 Then
            If (currentUnit.Status And Status.Draft) = Status.Draft Then
                Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonUnitAssignment(0, Me.PathID)
            Else
                Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonUnitAssignment(Me.CurrentUnitID, Me.PathID)
            End If
        End If
        If Me.ListOfAssignmentByPerson.Count = 0 Then
            Me.Resource.setLabel(Me.LBpersonPermissionNoAss)
            Me.LBpersonPermissionNoAss.Visible = True
        Else
            Me.RPuserPermission.DataSource = Me.ListOfAssignmentByPerson
            Me.RPuserPermission.DataBind()
        End If
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


    Private Function PersistData()
        Dim IsTransactionExecute As Boolean = Me.ServiceEP.SaveOrUpdateUnitandAssignment(currentUnit, Me.PathID, ListOfAssignmentByCommRole, ListOfAssignmentByPerson, Me.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, LinguaID)
        If Not IsTransactionExecute Then
            ShowError(EpError.Generic)
        End If
        Return IsTransactionExecute
    End Function


#Region "Button"

    Private Sub InitWizardButton()
        Dim oButton As Button
        oButton = Me.WZRunitCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNcancel")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
        End If
        oButton = Me.WZRunitCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNprevious")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
            oButton.Visible = (Me.CurrentStep = StepUnitManagement.SelectPermission Or Me.CurrentStep = StepUnitManagement.SelectPerson)
        End If

        oButton = Me.WZRunitCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNedit")
        If Not IsNothing(oButton) Then
            If CurrentStep = StepUnitManagement.Detail Or CurrentStep = StepUnitManagement.NewUnit Or CurrentStep = StepUnitManagement.Update Then
                Me.Resource.setButton(oButton)
                oButton.Visible = True
            Else
                oButton.Visible = False
            End If

        End If

        oButton = Me.WZRunitCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNnext")
        If Not IsNothing(oButton) Then
            If Me.CurrentStep = StepUnitManagement.Detail Or Me.CurrentStep = StepUnitManagement.NewUnit Or CurrentStep = StepUnitManagement.Update Then
                Me.Resource.setButtonByValue(oButton, "complete", True)
            Else
                Me.Resource.setButtonByValue(oButton, "next", True)
            End If
        End If
    End Sub

    Public Sub BTNnext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Select Case Me.CurrentStep
            Case StepUnitManagement.NewUnit
                Me.UpdateDetail()
                If Me.ServiceEP.CheckStatus(currentUnit.Status, Status.Draft) Then
                    currentUnit.Status = currentUnit.Status - Status.Draft
                End If
                If PersistData() Then
                    RedirectToUrl(RootObject.PathView(PathID, CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathID)))
                Else
                    Me.ShowError(EpError.Generic)
                End If
            Case StepUnitManagement.Detail
                Me.UpdateDetail()
                If Me.ServiceEP.CheckStatus(currentUnit.Status, Status.Draft) Then
                    currentUnit.Status = currentUnit.Status - Status.Draft
                End If
                If PersistData() Then
                    RedirectToUrl(RootObject.PathView(PathID, CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathID)))
                Else
                    Me.ShowError(EpError.Generic)
                End If
                'Me.PageUtility.RedirectToUrl(RootObject.UnitManagement(currentUnit.Id, Me.PathID, Me.CurrentCommunityID, StepUnitManagement.SelectPermission))
            Case StepUnitManagement.SelectPermission
                Me.GetSelectedPermission()
                Me.PageUtility.RedirectToUrl(RootObject.UnitManagement(currentUnit.Id, Me.PathID, Me.CurrentCommunityID, StepUnitManagement.Detail))
            Case StepUnitManagement.SelectPerson
                Me.GetSelectedPerson()
                Me.PageUtility.RedirectToUrl(RootObject.UnitManagement(currentUnit.Id, Me.PathID, Me.CurrentCommunityID, StepUnitManagement.SelectPermission))
            Case StepUnitManagement.Update
                Me.UpdateDetail()
                If PersistData() Then
                    RedirectToUrl(RootObject.PathView(PathID, CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathID)))
                Else
                    Me.ShowError(EpError.Generic)
                End If
        End Select
    End Sub

    Public Sub BTNedit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.UpdateDetail()
        If Me.PersistData() Then
            Me.PageUtility.RedirectToUrl(RootObject.UnitManagement(currentUnit.Id, Me.PathID, Me.CurrentCommunityID, StepUnitManagement.SelectPermission))
        Else
            Me.ShowError(EpError.Generic)
        End If
    End Sub

    Public Sub BTNprevious_Click(ByVal sender As Object, ByVal e As EventArgs)
        Select Case Me.CurrentStep
            Case StepUnitManagement.SelectPermission
                Me.GetSelectedPermission()
                Me.PageUtility.RedirectToUrl(RootObject.UnitManagement(currentUnit.Id, Me.PathID, Me.CurrentCommunityID, StepUnitManagement.Detail))
            Case StepUnitManagement.SelectPerson
                Me.GetSelectedPerson()
                Me.PageUtility.RedirectToUrl(RootObject.UnitManagement(currentUnit.Id, Me.PathID, Me.CurrentCommunityID, StepUnitManagement.SelectPermission))
        End Select
    End Sub

    Public Sub BTNcancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        If (currentUnit.Status And Status.Draft) = Status.Draft Then
            If Not ServiceEP.DeleteOnlyUnit(currentUnit.Id) Then
                ShowError(EpError.Generic)
            End If
        End If
        ClearSession()

        If (From units In currentUnit.ParentPath.UnitList Where (Not ServiceEP.CheckStatus(units.Status, Status.Draft) And units.Deleted = 0) Select units).Count = 0 Then
            PageUtility.RedirectToUrl(RootObject.EduPathList(CurrentCommunityID, EpViewModeType.Manage))
        Else
            PageUtility.RedirectToUrl(RootObject.PathView(Me.PathID, Me.CurrentCommunityID, EpViewModeType.Manage, ServiceEP.isPlayModePath(PathID)))
        End If
    End Sub
    Private Sub BTNselectPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNselectPerson.Click
        Me.GetSelectedPermission()
        PageUtility.RedirectToUrl(RootObject.UnitManagement(Me.currentUnit.Id, Me.PathID, Me.CurrentCommunityID, StepUnitManagement.SelectPerson))
    End Sub
#End Region

    Public Sub SetHelpImg(ByRef oImg As System.Web.UI.WebControls.Image)
        'oImg.ToolTip = Me.Resource.getValue("ImgHelp")
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

            'Me.SetHelpImg(e.Item.FindControl("IMGhelp"))
            'Me.SetHelpImg(e.Item.FindControl("IMGhelp1"))
            'Me.SetHelpImg(e.Item.FindControl("IMGhelp2"))

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
            'Me.SetHelpImg(e.Item.FindControl("IMGhelp"))

            oLabel = e.Item.FindControl("LBcommRoleTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RPtitle.CRole")
            End If
            oLabel = e.Item.FindControl("LBroleEp")
            If Not IsNothing(oLabel) Then
                Me.Resource.setLabel(oLabel)
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

            'Me.SetHelpImg(e.Item.FindControl("IMGhelp"))


            oLabel = e.Item.FindControl("LBpersonTitle")

            oLabel.Text = Me.Resource.getValue("RPtitle.Person")

            oLabel = e.Item.FindControl("LBroleEp")
            If Not IsNothing(oLabel) Then
                Me.Resource.setLabel(oLabel)
            End If
        End If
    End Sub

#End Region

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_EduPath.Codex)
    End Sub


    Private Sub BTNerror_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNerror.Click
        Me.ClearSession()
        Me.PageUtility.RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLhelpRole.Visible = False
        CTRLhelpVisibility.Visible = False
        MLVunitCreate.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class

Public Enum StepUnitManagement
    ErrorStep = -1000
    Update = -2 'Step only for querystring
    NewUnit = -1 'Step only for querystring
    Detail = 0
    SelectPermission = 1
    SelectPerson = 2
    'SummaryPermission = 3
End Enum

