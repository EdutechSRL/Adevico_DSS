Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class ActivityManagement
    Inherits EPpageBaseEduPath

    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                Dim unitID_local As Long

                If currentActivity.Id = 0 OrElse currentActivity.ParentUnit.Id = 0 Then
                    unitID_local = UnitID
                Else
                    unitID_local = currentActivity.ParentUnit.Id
                End If
                _PathType = ServiceEP.GetEpType(unitID_local, ItemType.Unit)
            End If
            Return _PathType
        End Get
    End Property
#Region "InitStandard"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Private _Service As Services_EduPath
    Private ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(Me.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property

    Private _serviceAssignment As ServiceAssignment
    Private ReadOnly Property ServiceAssignment As ServiceAssignment
        Get
            If IsNothing(_serviceAssignment) Then
                _serviceAssignment = ServiceEP.ServiceAssignments
            End If
            Return _serviceAssignment
        End Get
    End Property

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region
#Region "Property"

    Private _pathId As Int64 = -1
    Public ReadOnly Property PathId As Int64
        Get
            If _pathId = -1 Then
                _pathId = ServiceEP.GetPathId_ByActivityId(CurrentActivityID)
            End If
            Return _pathId
        End Get
    End Property

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

    Private ReadOnly Property UnitID() As Long
        Get
            If IsNumeric(Request.QueryString("UId")) Then
                Return Request.QueryString("UId")
            Else
                Return -1
            End If
        End Get
    End Property

    Private ReadOnly Property CurrentStep() As Integer
        Get
            If IsNumeric(Request.QueryString("Step")) Then
                Return Request.QueryString("Step")
            Else
                Return StepActivityManagement.ErrorStep
            End If
        End Get
    End Property

    Private ReadOnly Property SessionUniqueKey() As String
        Get
            If Me.ViewState("SessionUniqueKey") Is Nothing Then
                Me.ViewState("SessionUniqueKey") = Me.CurrentActivityID & "_" & Me.UtenteCorrente.ID
            End If
            Return Me.ViewState("SessionUniqueKey")
        End Get
    End Property

    Private Property CurrentActivityID() As Long
        Get
            If IsNumeric(Me.ViewState("CurrentActivityID")) And Me.ViewState("CurrentActivityID") > 0 Then
                Return Me.ViewState("CurrentActivityID")
            End If
            Dim qs_activityId As String = Me.Request.QueryString("AId")
            If IsNumeric(qs_activityId) Then
                Me.ViewState("CurrentActivityID") = qs_activityId
                Return qs_activityId
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentActivityID") = value
        End Set
    End Property

    Private ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return 0
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
    Private _currentActivity As Activity

    Private Property currentActivity As Activity
        Get
            If _currentActivity Is Nothing Then
                _currentActivity = New Activity
            End If
            If _currentActivity.Id = 0 AndAlso CurrentActivityID > 0 Then
                _currentActivity = Me.ServiceEP.GetActivity(Me.CurrentActivityID)
            End If
            Return _currentActivity
        End Get
        Set(ByVal value As Activity)
            If IsNothing(_currentActivity) Then
                _currentActivity = New Activity
            End If
            _currentActivity = value
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
        If Page.IsPostBack AndAlso Me.CurrentStep = StepActivityManagement.SelectPermission Then
            Me.InitSelectPermission()

        End If
    End Sub
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides Sub BindDati()
        Select Case Me.CurrentStep
            Case StepActivityManagement.NewActivity

                If VerifyUrl() Then
                    SetHelpImg()
                    If CurrentActivityID > 0 Then 'verifico se l'attivita' e' gia' stata salvata in precedenza o meno.
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Unit, Me.CurrentActivityID), InteractionType.UserWithLearningObject)
                        Me.InitUpdate()
                    Else
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Unit, "0"), InteractionType.UserWithLearningObject)
                        Me.InitNewActivity()
                        CurrentActivityID = currentActivity.Id
                    End If
                Else
                    ShowError(EpError.Url)
                End If
            Case StepActivityManagement.Update
                If VerifyUrl() Then
                    SetHelpImg()
                    Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Unit, Me.CurrentActivityID), InteractionType.UserWithLearningObject)
                    Me.InitUpdate()
                Else
                    ShowError(EpError.Url)
                End If
            Case StepActivityManagement.Detail
                If VerifyUrl() Then
                    SetHelpImg()
                    Me.InitDetail()
                Else
                    ShowError(EpError.Url)
                End If
            Case StepActivityManagement.SelectPermission
                If VerifyUrl() Then
                    Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Assignment, "0"), InteractionType.UserWithLearningObject)
                    SetHelpImg()
                    Me.InitSelectPermission()
                Else
                    ShowError(EpError.Url)
                End If
            Case StepActivityManagement.SelectPerson
                If VerifyUrl() Then
                    Me.InitSelectPerson()
                Else
                    ShowError(EpError.Url)
                End If
            Case StepActivityManagement.AddQuiz
                If IsNumeric(Request.QueryString("idQ")) Then
                    SetHelpImg()
                    ServiceEP.SaveOrUpdateSubQuiz(CurrentActivityID, Request.QueryString("idQ"), New COL_Questionario.Questionario(Request.QueryString("idQ")), PageUtility.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex), PageUtility.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex), ComunitaCorrenteID, UtenteCorrente.ID, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress)
                    If IsNumeric(Request.QueryString("owId")) Then
                        Dim owId As Int64 = Request.QueryString("owId")
                        If currentActivity.isQuiz Then
                            If Me.ServiceEP.CheckStatus(currentActivity.Status, Status.Draft) Then
                                currentActivity.Status = currentActivity.Status - Status.Draft
                            End If
                            RedirectToUrl(RootObject.ActivityManagement(CurrentActivityID, currentActivity.ParentUnit.Id, ComunitaCorrenteID, StepActivityManagement.Detail, IsMoocPath, PreloadIsFromReadOnly))
                        Else
                            RedirectToUrl(RootObject.ViewActivity(CurrentActivityID, currentActivity.ParentUnit.Id, currentActivity.Path.Id, ComunitaCorrenteID, EpViewModeType.Manage, IsMoocPath, PreloadIsFromReadOnly))
                        End If
                    End If

                Else
                    BindNoPermessi()
                End If
            Case Else
                Me.ShowError(EpError.Generic)
        End Select
        setControls_ByEpType()
        Me.CTRLhelpRole.InitDialog(Not Me.isAutoEp)
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
            .setLabel(Me.LBcroleSummary)
            .setLabel(Me.LBpersonSummary)
            .setLabel_To_Value(Me.LBdetailTitle, "LBdetailTitle.Activity")
            .setLabel(Me.LBnameTitle)
            .setLabel(Me.LBdescriptionTitle)
            .setLabel(Me.LBtypeTitle)
            .setLabel(Me.LBminCompletionTitle)
            .setLabel(LBminMark)
            .setLabel_To_Value(LBhours, "LBhours.Activity")
            .setCheckBox(Me.CKBvisibilityAct)
            .setCheckBox(Me.CKBmandatoryAct)
            .setCheckBox(Me.CKBstartDate)
            .setCheckBox(Me.CKBendDate)
            .setLabel(LBweight)
            .setCompareValidator(COVminCompletion)
            .setCompareValidator(COVweight, COVminCompletion)
            .setCompareValidator(COVminMark, COVminCompletion)
            .setRangeValidator(RNVminMark)
            .setRangeValidator(RNVminCompletion, RNVminMark)
            .setRequiredFieldValidator(RFVname, True, False)
            .setRequiredFieldValidator(Me.RFVminCompl, True, False)
            .setRequiredFieldValidator(Me.RFVminMark, True, False)
            .setRequiredFieldValidator(Me.RFVweight, True, False)
            .setLabel_To_Value(LBpoints, "Points")
            .setLabel(Me.LbMincompletionHelp)
            .setLabel_To_Value(Me.LbWeightManHelp, "LbWeightManHelp.Activity")
            Me.Master.ServiceTitle = .getValue("ManAct")
            .setLabel(LBadvanced)
            .setLabel(Me.LBselectPermHelpAct)
            .setLabel(Me.LBpermissionHelpAct)
            .setLabel(Me.LBmarkHelpAct)
            .setLabel(Me.LBweightHelpAct)
            .setLabel_To_Value(Me.LbMincompletionHelp, "LbMincompletionHelp.Manual.Activity")
            .setLabel(LBerrorEndDate)
            .setLabel(LBerroStartDate)
        End With

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

#End Region
    Private Sub setControls_ByEpType()
        If isAutoEp Then
            hideControl(DIVcompletion)
        Else
            hideControl(DIVtime)
        End If
    End Sub

    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setButton(Me.BTNerror)
        Select Case ErrorType
            Case EpError.Generic
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & ErrorType.ToString), Helpers.MessageType.error)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                CTRLerrorMessage.InitializeControl(Resource.getValue("Error." & ErrorType.ToString), Helpers.MessageType.alert)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        End Select
        Me.MLVactivityCreate.ActiveViewIndex = 1
    End Sub

    Private Sub InitUpdate()
        currentActivity = Me.ServiceEP.GetActivity(Me.CurrentActivityID)
        If IsNothing(currentActivity) Then
            Me.ShowError(EpError.Generic)
        Else
            Me.InitDetail()
        End If
    End Sub
    Private Function GetItemListStatus(ByVal oStatus As Status) As ListItem
        Dim oItem As New ListItem
        oItem.Text = Me.Resource.getValue("Status." & oStatus.ToString)
        oItem.Value = oStatus
        If Me.CurrentStep = StepActivityManagement.Detail Or Me.CurrentStep = StepActivityManagement.Update Then
            oItem.Selected = (Me.currentActivity.Status And oStatus) = oStatus
        End If
        Return oItem
    End Function

    Public Sub SetHelpImg()
        'SetHelpImg(IMGhelpSelectPerm)
        'SetHelpImg(IMGhelpSelectPerm)
        'SetHelpImg(IMGhelp)
        'SetHelpImg(IMGminComplHelp)
        'SetHelpImg(IMGvisibilityHelp)
        'SetHelpImg(ImgWeightManHelp)
        'SetHelpImg(IMGhelpWeight)
        'SetHelpImg(IMGminMarklHelp)
    End Sub

    Private Sub InitDDLType()
        Dim oItem As New ListItem
        oItem.Text = Me.Resource.getValue("ActivityType." & ActivityManagementType.General.ToString)
        oItem.Value = ActivityManagementType.General
        Me.DDLtype.Items.Add(oItem)
        DIVtype.Visible = False
        If CurrentActivityID > 0 Then
            If currentActivity.isQuiz Then
                oItem = New ListItem
                oItem.Text = Me.Resource.getValue("ActivityType." & ActivityManagementType.Quiz.ToString)
                oItem.Value = ActivityManagementType.Quiz
                Me.DDLtype.Items.Add(oItem)
                DIVtype.Visible = True
                DDLtype.Enabled = False
            End If
            'If ServiceEP.IsServiceActive(COL_Questionario.ModuleQuestionnaire.UniqueID, CurrentCommunityID) Then
            '    oItem = New ListItem
            '    oItem.Text = Me.Resource.getValue("ActivityType." & ActivityManagementType.Quiz.ToString)
            '    oItem.Value = ActivityManagementType.Quiz
            '    Me.DDLtype.Items.Add(oItem)
            'End If
        End If

    End Sub
    Private Sub InitNewActivity()
        currentActivity = New Activity
        If isAutoEp Then
            currentActivity.Weight = 0
            currentActivity.MinCompletion = 100
        Else
            currentActivity.Weight = 1
        End If
        currentActivity.Status = currentActivity.Status Or Status.Draft
        Me.SubInitSelectPermissionByCRole()
        Me.SubInitSelectPermissionByPerson()
        Me.ServiceEP.SaveActivity(currentActivity, UnitID, Me.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress, CurrentCommunityID)
        If IsNothing(currentActivity) Then
            ShowError(EpError.Generic)
        Else
            Me.InitDetail()
        End If
    End Sub
    Private Sub InitDetail()
        Me.WZRactivityCreate.ActiveStepIndex = StepActivityManagement.Detail
        Me.InitWizardButton()

        If DDLtype.Items.Count = 0 Then
            InitDDLType()
        End If
        Me.TXBname.Text = Me.currentActivity.Name
        Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode)
        Me.CTRLeditorDescription.HTML = Me.currentActivity.Description
        Me.CKBvisibilityAct.Checked = Not ((currentActivity.Status And Status.Locked) = Status.Locked)
        Me.TXBminCompletion.Text = currentActivity.MinCompletion
        TXBminMark.Text = currentActivity.MinMark
        TXBweight.Text = currentActivity.Weight
        TXBhours.Text = Math.Floor(currentActivity.Weight / 60)
        TXBmins.Text = currentActivity.Weight Mod 60
        TXBhours.NumberFormat.DecimalDigits = 0
        TXBmins.NumberFormat.DecimalDigits = 0
        Me.CKBmandatoryAct.Checked = ((Me.currentActivity.Status And Status.Mandatory) = Status.Mandatory)
        If IsNothing(currentActivity.StartDate) Then
            Me.RDPstartDate.SelectedDate = DateTime.Now
        Else
            Me.CKBstartDate.Checked = True
            Me.RDPstartDate.SelectedDate = currentActivity.StartDate
        End If
        If IsNothing(currentActivity.EndDate) Then
            Me.RDPendDate.SelectedDate = DateTime.Now
        Else
            Me.CKBendDate.Checked = True
            Me.RDPendDate.SelectedDate = currentActivity.EndDate
        End If
        If currentActivity.isQuiz Then
            Me.DDLtype.SelectedValue = ActivityManagementType.Quiz
            Dim ctrlBTNQuiz As Control = Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNquiz")
            ctrlBTNQuiz.Visible = True
            If currentActivity.SubActivityList.Count = 0 Then
                Me.Resource.setButtonByValue(ctrlBTNQuiz, "create", True) 'Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNquiz"), "create", True)
            Else
                Me.Resource.setButtonByValue(ctrlBTNQuiz, "update", True) 'Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNquiz"), "update", True)
            End If
        Else
            Me.DDLtype.SelectedValue = ActivityManagementType.General
            Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNquiz").Visible = False
        End If

        If (ListOfAssignmentByCommRole.Count = 0) Then
            Me.SubInitSelectPermissionByCRole()
        End If
        If (ListOfAssignmentByPerson.Count = 0) Then
            Me.SubInitSelectPermissionByPerson()
        End If

        Dim ListActiveCRoleAssignment As List(Of dtoGenericAssignment) = Me.ServiceAssignment.GetActiveAssignment(Me.ListOfAssignmentByCommRole)
        If ListActiveCRoleAssignment.Count > 0 Then
            RPcroleSummary.DataSource = ListActiveCRoleAssignment
            RPcroleSummary.DataBind()
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

    Private Function GetDraftActivityDetail() As Activity
        Dim oDraftActivity As New Activity
        oDraftActivity.Name = Me.TXBname.Text
        oDraftActivity.Description = removeBRfromStringEnd(Me.CTRLeditorDescription.HTML)
        oDraftActivity.Status = Status.Draft Or oDraftActivity.Status
        If Not Me.CKBvisibilityAct.Checked Then
            oDraftActivity.Status = oDraftActivity.Status Or Status.Locked
        End If
        If CKBmandatoryAct.Checked Then
            oDraftActivity.Status = Status.Mandatory Or oDraftActivity.Status
        End If
        If CKBstartDate.Checked Then
            oDraftActivity.StartDate = RDPstartDate.SelectedDate
        End If
        If CKBendDate.Checked Then
            oDraftActivity.EndDate = RDPendDate.SelectedDate
        End If
        oDraftActivity.isQuiz = Me.DDLtype.SelectedValue
        oDraftActivity.MinCompletion = IIf(IsNumeric(Me.TXBminCompletion.Text), Me.TXBminCompletion.Text, 0)
        oDraftActivity.MinMark = IIf(IsNumeric(TXBminMark.Text), TXBminMark.Text, 0)
        oDraftActivity.Weight = GetWeight()
        Return oDraftActivity
    End Function
    Private Function GetWeight() As Short
        Dim newWeight As Short
        newWeight = IIf(isTimeEp, ConvertTime, IIf(IsNumeric(TXBweight.Text), TXBweight.Text, 0))
        If isAutoEp Then
            ServiceEP.UpdateWeight(currentActivity, newWeight)
        End If
        Return newWeight
    End Function

    Private Function ConvertTime() As Short
        Dim h As Short = 0
        Dim min As Short = 0

        If IsNumeric(TXBhours.Text) Then
            h = TXBhours.Text
        End If

        If IsNumeric(TXBmins.Text) Then
            min = TXBmins.Text
        End If
        Return ServiceEP.ConvertTime(h, min)
    End Function

    Private Function ValidateDateConstraint() As Boolean
        If CKBstartDate.Checked Then
            If IsNothing(RDPstartDate.SelectedDate) Then
                LBerroStartDate.Visible = True
                Return False
            Else
                LBerroStartDate.Visible = True
                currentActivity.StartDate = RDPstartDate.SelectedDate
            End If
        Else
            currentActivity.StartDate = Nothing
            LBerroStartDate.Visible = True
        End If

        If CKBendDate.Checked Then
            If IsNothing(RDPendDate.SelectedDate) Then
                LBerrorEndDate.Visible = False
                Return False
            Else
                Dim a As Date = RDPendDate.SelectedDate
                currentActivity.EndDate = a.AddHours(23).AddMinutes(59)
                If CKBstartDate.Checked AndAlso currentActivity.StartDate > currentActivity.EndDate Then
                    LBerrorEndDate.Visible = True
                    Return False
                Else
                    LBerrorEndDate.Visible = False
                End If
            End If
        Else
            currentActivity.EndDate = Nothing
            LBerrorEndDate.Visible = False
        End If

        Return True
    End Function


    Private Function UpdateDetail() As Boolean
        currentActivity.Name = Me.TXBname.Text
        currentActivity.Description = removeBRfromStringEnd(Me.CTRLeditorDescription.HTML)
        If Me.CKBvisibilityAct.Checked Then
            If (currentActivity.Status And Status.Locked) = Status.Locked Then
                currentActivity.setNotLocked()
            End If
        Else
            currentActivity.setLocked()
        End If
        If CKBmandatoryAct.Checked Then
            currentActivity.setMandatory()
        ElseIf Not CKBmandatoryAct.Checked AndAlso ((currentActivity.Status And Status.Mandatory) = Status.Mandatory) Then
            currentActivity.setNotMandatory()
        End If

        currentActivity.MinCompletion = IIf(IsNumeric(Me.TXBminCompletion.Text), Me.TXBminCompletion.Text, 0)
        currentActivity.MinMark = IIf(IsNumeric(TXBminMark.Text), TXBminMark.Text, 0)
        currentActivity.Weight = GetWeight()
        currentActivity.isQuiz = Me.DDLtype.SelectedValue
        Return ValidateDateConstraint()

    End Function

    Private Sub InitSelectPerson()
        Me.WZRactivityCreate.ActiveStepIndex = StepActivityManagement.SelectPerson
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBuserTitle)

        Dim ListCommunityID As New List(Of Integer)
        ListCommunityID.Add(Me.CurrentContext.UserContext.CurrentCommunityID)
        Dim PersonToHide As List(Of Integer) = Me.ServiceAssignment.GetIdFromDtoAssignementGenericWithOldRoleEP(Me.ListOfAssignmentByPerson)
        Me.CTRLselectUser.CurrentPresenter.Init(ListCommunityID, ListSelectionMode.Multiple, PersonToHide)

    End Sub
    Private Sub GetSelectedPerson()
        Me.ListOfAssignmentByPerson.AddRange(ServiceAssignment.GetDtoAssignemtGenericWithOldRoleEPAndDelFromMembers(Me.CTRLselectUser.CurrentPresenter.GetConfirmedUsers))
    End Sub

    Private Sub InitSelectPermission()
        Me.WZRactivityCreate.ActiveStepIndex = StepActivityManagement.SelectPermission
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBpersonPermission)
        Me.Resource.setLabel(Me.LBcrolePermission)
        Me.Resource.setButton(Me.BTNselectPerson)
        Me.Resource.setLabel_To_Value(Me.LBpermissionTitle, "LBpermissionTitle.Activity")
        Me.SubInitSelectPermissionByCRole()
        Me.SubInitSelectPermissionByPerson()

    End Sub

    Private Sub GetSelectedPermission()
        Me.SubGetSelectedPermissionByCRole()
        Me.SubGetSelectedPermissionByPerson()
    End Sub

    Private Function VerifyUrl() As Boolean
        If Me.CurrentStep <> StepActivityManagement.NewActivity And Me.CurrentActivityID = -1 Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        If Me.UnitID = -1 Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        If Me.CurrentCommunityID = 0 And Me.CurrentStep <> StepActivityManagement.Update Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        Return True
    End Function

    Private Sub SubInitSelectPermissionByCRole()
        If Me.ListOfAssignmentByCommRole.Count = 0 Then
            If (currentActivity.Status And Status.Draft) = Status.Draft Then
                Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleActivityAssignment(0, Me.UnitID, Me.CurrentCommunityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentContext.UserContext.Language.Id)
            Else
                Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRoleActivityAssignment(Me.CurrentActivityID, Me.UnitID, Me.CurrentCommunityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentContext.UserContext.Language.Id)
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
                oCkb = item.FindControl("CKBevaluator")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.Evaluator
                    End If
                End If
                oCkb = item.FindControl("CKBmanager")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.Manager Or oDtoAssign.RoleEP
                    End If
                End If
                Me.ListOfAssignmentByCommRole.Add(oDtoAssign)
            End If
        Next
    End Sub

    Private Sub SubInitSelectPermissionByPerson()
        If Me.ListOfAssignmentByPerson.Count = 0 Then
            If (currentActivity.Status And Status.Draft) = Status.Draft Then
                Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonActivityAssignment(0, Me.UnitID)
            Else
                Me.ListOfAssignmentByPerson = Me.ServiceAssignment.GetListPersonActivityAssignment(Me.CurrentActivityID, Me.UnitID)
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

    Private Function PersistData() As Boolean
        Dim IsTransactionExecute As Boolean = Me.ServiceEP.SaveOrUpdateActivityandAssignment(currentActivity, Me.UnitID, ListOfAssignmentByCommRole, ListOfAssignmentByPerson, Me.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress, Me.CurrentCommunityID, LinguaID)
        'ClearSession()
        If Not IsTransactionExecute Then
            ShowError(EpError.Generic)
        End If
        Return IsTransactionExecute
    End Function


#Region "Button"

    Private Sub InitWizardButton()
        Dim oButton As Button
        oButton = Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNcancel")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
        End If
        oButton = Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNprevious")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
            oButton.Visible = (Me.CurrentStep = StepActivityManagement.SelectPermission Or Me.CurrentStep = StepActivityManagement.SelectPerson)
        End If
        oButton = Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNedit")
        If Not IsNothing(oButton) Then
            If CurrentStep = StepActivityManagement.Detail Or CurrentStep = StepActivityManagement.NewActivity Or CurrentStep = StepActivityManagement.Update Then
                Me.Resource.setButton(oButton)
                oButton.Visible = True
            Else
                oButton.Visible = False
            End If
        End If
        oButton = Me.WZRactivityCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNnext")
        If Not IsNothing(oButton) Then
            If Me.CurrentStep = StepActivityManagement.Detail Or Me.CurrentStep = StepActivityManagement.NewActivity Or CurrentStep = StepActivityManagement.Update Then
                Me.Resource.setButtonByValue(oButton, "complete", True)
            Else
                Me.Resource.setButtonByValue(oButton, "next", True)
            End If
        End If
    End Sub
    Public Sub BTNquiz_Click(ByVal sender As Object, ByVal e As EventArgs)
        UpdateDetail()
        PersistData()
        If currentActivity.SubActivityList.Count = 0 Then
            'redirect alla subactivity
            'RedirectToUrl(RootObject.CreateQuiz(ServiceEP.SaveOrUpdateSubQuiz(CurrentActivityID, Request.QueryString("idQ"), New COL_Questionario.Questionario(0), PageUtility.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex), PageUtility.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex), ComunitaCorrenteID, UtenteCorrente.ID, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress).Id, OwnerType_enum.EduPathSubActivity))
            'redirect all'activity padre, senza pre-creazione sub
            RedirectToUrl(RootObject.CreateQuiz(CurrentActivityID, OwnerType_enum.EduPathActivity))
        Else
            RedirectToUrl(RootObject.UpdateQuiz(currentActivity.SubActivityList(0).Id, currentActivity.SubActivityList(0).IdObjectLong))
        End If
    End Sub

    Public Sub BTNnext_Click(ByVal sender As Object, ByVal e As EventArgs)

        Select Case Me.CurrentStep
            Case StepActivityManagement.NewActivity
                If Me.UpdateDetail() Then


                    If Me.ServiceEP.CheckStatus(currentActivity.Status, Status.Draft) Then
                        currentActivity.Status = currentActivity.Status - Status.Draft
                    End If
                    If PersistData() Then 'cancella currentActivity
                        RedirectToUrl(RootObject.PathView(PathId, CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, PreloadIsFromReadOnly))
                    Else
                        Me.ShowError(EpError.Generic)
                    End If

                Else
                    BindDati()
                End If

            Case StepActivityManagement.Detail
                If Me.UpdateDetail() Then
                    If Me.ServiceEP.CheckStatus(currentActivity.Status, Status.Draft) Then
                        currentActivity.Status = currentActivity.Status - Status.Draft
                    End If
                    If PersistData() Then 'cancella currentActivity
                        RedirectToUrl(RootObject.PathView(PathId, CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, PreloadIsFromReadOnly))
                    Else
                        Me.ShowError(EpError.Generic)
                    End If
                Else
                    BindDati()
                End If

            Case StepActivityManagement.SelectPermission
                Me.GetSelectedPermission()
                Me.PageUtility.RedirectToUrl(RootObject.ActivityManagement(CurrentActivityID, Me.UnitID, Me.CurrentCommunityID, StepActivityManagement.Detail, IsMoocPath, PreloadIsFromReadOnly))

            Case StepActivityManagement.SelectPerson
                Me.GetSelectedPerson()
                Me.PageUtility.RedirectToUrl(RootObject.ActivityManagement(CurrentActivityID, Me.UnitID, Me.CurrentCommunityID, StepActivityManagement.SelectPermission, IsMoocPath, PreloadIsFromReadOnly))
            Case StepActivityManagement.Update
                If Me.UpdateDetail() Then
                    If Me.ServiceEP.CheckStatus(currentActivity.Status, Status.Draft) Then
                        currentActivity.Status = currentActivity.Status - Status.Draft
                    End If
                    If PersistData() Then 'cancella currentActivity
                        RedirectToUrl(RootObject.PathView(PathId, CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, PreloadIsFromReadOnly))
                    Else
                        Me.ShowError(EpError.Generic)
                    End If
                Else
                    BindDati()
                End If
        End Select
    End Sub
    Public Sub BTNedit_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.UpdateDetail() Then
            If Me.PersistData() Then
                Me.PageUtility.RedirectToUrl(RootObject.ActivityManagement(CurrentActivityID, Me.UnitID, Me.CurrentCommunityID, StepActivityManagement.SelectPermission, IsMoocPath, PreloadIsFromReadOnly))
            Else
                Me.ShowError(EpError.Generic)
            End If
        Else
            BindDati()
        End If

    End Sub
    Public Sub BTNprevious_Click(ByVal sender As Object, ByVal e As EventArgs)
        Select Case Me.CurrentStep
            Case StepActivityManagement.SelectPermission
                Me.GetSelectedPermission()
                Me.PageUtility.RedirectToUrl(RootObject.ActivityManagement(currentActivity.Id, Me.UnitID, Me.CurrentCommunityID, StepActivityManagement.Detail, IsMoocPath, PreloadIsFromReadOnly))
            Case StepActivityManagement.SelectPerson
                Me.GetSelectedPerson()
                Me.PageUtility.RedirectToUrl(RootObject.ActivityManagement(currentActivity.Id, Me.UnitID, Me.CurrentCommunityID, StepActivityManagement.SelectPermission, IsMoocPath, PreloadIsFromReadOnly))
        End Select
    End Sub
    Public Sub BTNcancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim PathId As Long = Me.PathId
        If (currentActivity.Status And Status.Draft) = Status.Draft Then
            If Not (ServiceEP.DeleteOnlyActivity(currentActivity.Id)) Then
                Me.ShowError(EpError.Generic)
            End If
        End If
        ClearSession()
        PageUtility.RedirectToUrl(RootObject.PathView(PathId, Me.CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, PreloadIsFromReadOnly))
    End Sub
    Private Sub BTNselectPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNselectPerson.Click
        Me.GetSelectedPermission()
        Me.PageUtility.RedirectToUrl(RootObject.ActivityManagement(currentActivity.Id, Me.UnitID, Me.CurrentCommunityID, StepActivityManagement.SelectPerson, IsMoocPath, PreloadIsFromReadOnly))
    End Sub
#End Region

    'Public Sub SetHelpImg(ByRef oImg As System.Web.UI.WebControls.Image)
    '    'oImg.ImageUrl = RootObject.ImgHelp(Me.BaseUrl)
    '    oImg.ToolTip = Me.Resource.getValue("ImgHelp")

    'End Sub

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

            dtoCRoleAss = e.Item.DataItem
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

            ' Me.SetHelpImg(e.Item.FindControl("IMGhelp"))


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
        If Not _IsMoocPath.HasValue Then
            Dim idPath As Long = PathId
            If (idPath > 0) Then
                IsMoocPath = ServiceEP.IsMooc(PathId)
                If PreloadIsMooc AndAlso Not IsMoocPath Then
                    IsMoocPath = PreloadIsMooc
                End If
            Else
                IsMoocPath = PreloadIsMooc
            End If
        End If
    End Sub

    Private Sub BTNerror_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNerror.Click
        Me.ClearSession()
        RedirectToUrl(RootObject.PathView(ServiceEP.GetPathId_ByUnitId(UnitID), Me.CurrentCommunityID, EpViewModeType.Manage, False, IsMoocPath, PreloadIsFromReadOnly))
    End Sub
    Private Sub DDLtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtype.SelectedIndexChanged
        If Me.UpdateDetail() Then
            PersistData()
        End If

        BindDati()
    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As ModuleStatus)
        MLVactivityCreate.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class
Public Enum StepActivityManagement
    ErrorStep = -1000
    Update = -2 'Step only for querystring
    NewActivity = -1 'Step only for querystring
    Detail = 0
    SelectPermission = 1
    SelectPerson = 2
    'SummaryPermission = 3
    AddQuiz = 4
End Enum
Public Enum ActivityManagementType
    General = 0
    Quiz = 1
End Enum