Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class PathManagement
    Inherits PageBaseEduPath

#Region "Property"

    Public Const ShowFloatingDeadlines As Boolean = False

    Private _NotIsAutoTimePath As String = "none"
    Public ReadOnly Property NotIsAutoTimePath As String
        Get
            If _NotIsAutoTimePath = "none" Then
                If ServiceEP.CheckEpType(PathType, EpType.TimeAuto) Then
                    _NotIsAutoTimePath = " style='display:none;'"
                Else
                    _NotIsAutoTimePath = "    "
                End If
            End If
            Return _NotIsAutoTimePath
        End Get
    End Property


    Public ReadOnly Property RoleEpCount As String
        Get
            If isAutoEp Then
                Return "3" '+EP[Rob]
                ' Return "2"
            Else
                Return "4" '+EP[Rob]
                'Return "3"
            End If
        End Get
    End Property


    Protected Overrides ReadOnly Property PathType As EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(CurrentPathID, ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property
    Dim _isManageable As Integer = -10
    Protected ReadOnly Property isManageable As Boolean
        Get
            If _isManageable = -10 Then
                _isManageable = (ServiceEP.CheckStatus(currentPath.Status, Status.Locked) OrElse ServiceEP.CheckEpType(currentPath.EPType, EpType.Manual)) AndAlso (Not ServiceStat.UsersStartedPath(CurrentPathID, UtenteCorrente.ID))
            End If
            Return _isManageable
        End Get
    End Property
    Private ReadOnly Property CurrentStep() As Integer
        Get
            If IsNumeric(Request.QueryString("Step")) Then
                Return Request.QueryString("Step")
            Else
                Return StepPathManagement.ErrorStep
            End If
        End Get
    End Property



    Private Property SessionUniqueKey() As String
        Get
            Return Me.ViewState("SessionUniqueKey")
        End Get
        Set(ByVal value As String)
            Me.ViewState("SessionUniqueKey") = value
        End Set
    End Property

    Private Property CurrentPathID() As Long
        Get
            If IsNumeric(Me.ViewState("CurrentPathID")) And Me.ViewState("CurrentPathID") > 0 Then
                Return Me.ViewState("CurrentPathID")
            End If
            Dim qs_pathId As String = Me.Request.QueryString("PId")
            If IsNumeric(qs_pathId) Then
                Return qs_pathId
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentPathID") = value
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
    Private Function VerifyUrl() As Boolean
        If Me.CurrentStep <> StepPathManagement.NewPath And Me.CurrentPathID = -1 Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        If Me.CurrentCommunityID = 0 And Me.CurrentStep <> StepPathManagement.Update Then
            Me.ShowError(EpError.Url)
            Return False
        End If
        Return True
    End Function
    Private Property ListOfAssignmentByPerson As List(Of dtoGenericAssignmentWithDelete)
        Get

            If IsNothing(Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString)) Then
                Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString) = New List(Of dtoGenericAssignmentWithDelete)
            End If
            Return Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As List(Of dtoGenericAssignmentWithDelete))
            Me.Session("ListOfAssignmentByPerson_" & SessionUniqueKey.ToString) = value
        End Set
    End Property
    Private Property ListOfAssignmentByCommRole As List(Of dtoGenericAssignment)
        Get
            If IsNothing(Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString)) Then
                Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString) = New List(Of dtoGenericAssignment)
            End If
            Return Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString)
        End Get
        Set(ByVal value As List(Of dtoGenericAssignment))
            Me.Session("ListOfAssignmentByCommRole_" & SessionUniqueKey.ToString) = value
        End Set
    End Property

    Private _currentPath As Path

    Private Property currentPath As Path
        Get
            If IsNothing(_currentPath) Then
                _currentPath = New Path
            End If
            If _currentPath.Id = 0 AndAlso CurrentPathID > 0 Then
                _currentPath = Me.ServiceEP.GetPath(Me.CurrentPathID)
            End If
            Return _currentPath
        End Get
        Set(ByVal value As Path)
            _currentPath = value
        End Set
    End Property
#End Region

    Private Sub ClearSession()
        If Me.CurrentStep <> StepPathManagement.SelectCommunity Then
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

    Private _Status As Dictionary(Of Integer, lm.Comol.Core.DomainModel.ModuleStatus)
    Public Overrides Sub BindDati()

        DIVfloatingDeadlines.Visible = ShowFloatingDeadlines

        'Dim x = ServiceEP.PathsWeightAutoCorrect()

        If Not IsPostBack Then
            Select Case Me.CurrentStep
                Case StepPathManagement.NewPath
                    If Me.CurrentCommunityID <= 0 Then
                        Me.PageUtility.RedirectToUrl(RootObject.PathManagementSelectCommunity())
                    ElseIf VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, "0"), InteractionType.UserWithLearningObject)

                        Me.InitNewPath()
                        SetHelpImg()
                        Me.CurrentPathID = Me.currentPath.Id
                    Else
                        ShowError(EpError.Url)
                    End If
                Case StepPathManagement.Update
                    If VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.CurrentPathID), InteractionType.UserWithLearningObject)
                        Me.SessionUniqueKey = Me.CurrentPathID & Me.CurrentCommunityID
                        SetHelpImg()
                        Me.InitUpdate()
                    Else
                        ShowError(EpError.Url)
                    End If
                Case StepPathManagement.Detail
                    If VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Edit, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.CurrentPathID), InteractionType.UserWithLearningObject)
                        Me.SessionUniqueKey = Me.CurrentPathID & Me.CurrentCommunityID
                        SetHelpImg()
                        Me.InitDetail()
                    Else
                        ShowError(EpError.Url)
                    End If
                Case StepPathManagement.SelectPermission
                    If VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Create, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Assignment, "0"), InteractionType.UserWithLearningObject)
                        Me.SessionUniqueKey = Me.CurrentPathID & Me.CurrentCommunityID
                        Me.InitSelectPermission()
                    Else
                        ShowError(EpError.Url)
                    End If
                Case StepPathManagement.SummaryPermission
                    If VerifyUrl() Then
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Review, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.EduPath, Me.CurrentPathID), InteractionType.UserWithLearningObject)
                        Me.PageUtility.AddAction(Me.CurrentCommunityID, Services_EduPath.ActionType.Review, Me.PageUtility.CreateObjectsList(Services_EduPath.ObjectType.Assignment, "0"), InteractionType.UserWithLearningObject)
                        Me.SessionUniqueKey = Me.CurrentPathID & Me.CurrentCommunityID
                        Me.InitSummary()
                    Else
                        ShowError(EpError.Url)
                    End If

                Case StepPathManagement.SelectPerson
                    If VerifyUrl() Then
                        Me.SessionUniqueKey = Me.CurrentPathID & Me.CurrentCommunityID
                        Me.InitSelectPerson()
                    Else
                        ShowError(EpError.Url)
                    End If
                Case StepPathManagement.SelectCommunity
                    Me.InitCommunitySelection()
                Case Else
                    Me.ShowError(EpError.Generic)
            End Select
            Me.CTRLhelpRole.InitDialog(Not isTimeEp)
            setControls_ByEpType()
        ElseIf Me.CurrentStep = StepPathManagement.SelectPermission Then
            Me.InitSelectPermission()
        End If
    End Sub

    Private Sub setControls_ByEpType()
        If ServiceEP.CheckEpType(PathType, lm.Comol.Modules.EduPath.Domain.EPType.Time) Then
            hideControl(DIVmark)
            hideControl(LBminMark)
            hideControl(LBminMarkRes)
            hideControl(LBminMarkTitleRes)
        Else
            hideControl(LBhours)
            TXBhours.Visible = False
            TXBmins.Visible = False
            hideControl(LBweightAuto)
            'hideControl(IMGweightHelp)
            hideControl(DIVweightAuto)
            hideControl(DIVtime)
        End If

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
        MyBase.SetCulture("pg_Create", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        With Me.Resource
            .setLabel(LBerrorEndDate)
            .setCheckBox(CKBplayMode)
            .setCheckBox(CKBsingleAction)
            .setCheckBox(CKBfloatingDeadlines)
            .setLabel_To_Value(Me.LBdetailTitle, "LBdetailTitle.Path")
            .setLabel(Me.LBnameTitle)
            .setLabel(Me.LBdescriptionTitle)
            .setLabel(Me.LBminCompletionTitle)
            .setLabel(LBtype_t)
            .setLabel_To_Value(LBhours, "LBhours.Path")
            .setRequiredFieldValidator(RFVname, True, False)
            .setLabel(LBminMark)
            .setCompareValidator(COVminCompletion)
            .setCompareValidator(COVminMark, COVminCompletion)
            .setRequiredFieldValidator(Me.RFVminCompl, True, False)
            .setRequiredFieldValidator(Me.RFVminMark, True, False)
            .setRangeValidator(RNVminMark)
            .setRangeValidator(RNVminCompletion, RNVminMark)
            .setLabel(LBweightAuto)
            If ServiceEP.CheckEpType(EpType, EpType.Auto) Then
                .setLabel_To_Value(Me.LbMincompletionHelp, "LbMincompletionHelp." & EpType.Auto.ToString)
            Else
                .setLabel_To_Value(Me.LbMincompletionHelp, "LbMincompletionHelp." & EpType.Manual.ToString & ".Path")
            End If

            .setLabel(Me.LbWeightAutoHelp)
            .setLabel_To_Value(Me.LbWeightManHelp, "LbWeightManHelp.Path")
            .setLabel(Me.LBpermissionHelp)
            .setLabel(Me.LBselectPermHelp)
            LBweightAuto.Text = LBweightAuto.Text & ": " & ServiceEP.GetTime(currentPath.WeightAuto)
            .setLabel(Me.LBmarkHelpPath)
            Me.Master.ServiceTitle = .getValue("ManPath")

            .setLabel_To_Value(LBendDateTitle, "LBendDate.text")
            .setLabel(LBplayModeTitle)

            .setLabel(LBsingleActionTitle)
            .setLabel(LBfloatingDeadlinesTitle)

            .setLabel(LBendDateOverflow)
            .setLabel_To_Value(LBendDateOverflowTitle, "LBendDateOverflow.text")
            .setLabel(LBcontinueExecutionTitle)

            .setLabel(LBendDate)
            .setLabel(LBendDateOverflowHelp)
            CKBcontinueExecution.Text = .getValue("LBcontinueExecutionTitle.text")
            .setLabel(LBsetEndDate)
            .setLabel(LBcompletionPolicy_t)
            .setLabel(LBdisplayPolicy_t)
            .setLabel(LBscormSettingsPolicy_t)

            .setLiteral(LTadvancedSettingsTitle)
            .setLabel(LBspanCollapseList)
            .setLabel(LBspanExpandList)

            .setLabel(LBcompletionPolicyTitle)
            .setLabel(LBdisplayPolicyTitle)
            .setLabel(LBscormSettingsPolicyTitle)

        End With
        Aclearit.InnerText = Me.Resource.getValue("Aclearit.text")
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                If CurrentPathID > 0 Then
                    RedirectOnSessionTimeOut(RootObject.PathManagement(CurrentCommunityID, CurrentPathID, CInt(StepPathManagement.Detail).ToString, PathType), CurrentCommunityID)
                Else
                    RedirectOnSessionTimeOut(RootObject.AddPath(CurrentCommunityID, EpType.TimeAuto), CurrentCommunityID)
                End If

            End If
            Return False
        End Get
    End Property

#End Region

    Public Sub InitCommunitySelection()
        Me.Resource.setLabel(Me.LBselectCommunityTitle)
        Me.InitWizardButton()
        'Dim oService As Services_EduPath = Services_EduPath.Create
        'oService. = True
        Dim oService As Services_PostIt = Services_PostIt.Create 'temporaneo
        oService.CreatePostIt = True

        Dim oServiceBase As New ServiceBase(0, oService.Codex, oService.PermessiAssociati)
        Dim oClause As New GenericClause(Of ServiceClause)
        oClause.OperatorForNextClause = OperatorType.OrCondition
        oClause.Clause = New ServiceClause(oServiceBase, OperatorType.OrCondition)
        Me.CTRLcommunity.ServiceClauses = oClause
        Me.CTRLcommunity.SelectionMode = ListSelectionMode.Single
        Me.CTRLcommunity.BindDati()
        Me.WZRpathCreate.ActiveStepIndex = StepPathManagement.SelectCommunity
    End Sub


    Private Sub ShowError(ByVal ErrorType As EpError)
        Me.Resource.setButtonByValue(Me.BTNerror, "Path", True)
        Select Case ErrorType
            Case EpError.Generic
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.NotPermission
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
            Case EpError.Url
                Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
                Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        End Select
        Me.MLVpathCreate.SetActiveView(VIWerror)
    End Sub

    Public Sub SetHelpImg()

        'SetHelpImg(Me.IMGminComplHelp)
        'SetHelpImg(Me.IMGweightHelp)
        'If isMarkEp Then
        '    SetHelpImg(Me.IMGminMarklHelp)
        'End If
        'If EpType = EPType.TimeAuto OrElse (isTimeEp AndAlso isAutoEp) Then

        '    SetHelpImg(Me.ImgWeightManHelp)
        'Else
        '    Me.ImgWeightManHelp.Visible = False
        'End If
    End Sub

    Public Sub SetHelpImg(ByRef oImg As System.Web.UI.WebControls.Image)
        'oImg.ImageUrl = RootObject.ImgHelp(Me.BaseUrl)
        'oImg.ToolTip = Me.Resource.getValue("ImgHelp")
    End Sub

    Private Sub InitUpdate()
        currentPath = Me.ServiceEP.GetPath(Me.CurrentPathID)
        If IsNothing(currentPath) Then
            Me.ShowError(EpError.Generic)
        Else
            Me.InitDetail()
        End If

    End Sub
    Private ReadOnly Property EpType As EPType
        Get
            Dim qs As String = Request.QueryString("Type")
            If IsNumeric(qs) Then

                If ServiceEP.CheckEpType(qs, EpType.TimeAuto) Then
                    Return EpType.TimeAuto

                ElseIf ServiceEP.CheckEpType(qs, EpType.MarkManual) Then
                    Return EpType.MarkManual

                ElseIf ServiceEP.CheckEpType(qs, EpType.MarkAuto) Then
                    Return EpType.MarkAuto

                ElseIf ServiceEP.CheckEpType(qs, EpType.TimeManual) Then
                    Return EpType.TimeManual
                Else
                    ShowError(EpError.Url)
                End If

            Else
                ShowError(EpError.Url)
            End If


            'Dim pathType As String = Request.QueryString("Type")
            'If IsNumeric(pathType) AndAlso (pathType = EPType.TimeAuto OrElse pathType = EPType.TimeManual OrElse pathType = EPType.VoteManual OrElse pathType = EPType.VoteAuto) Then
            '    Return pathType
            'Else
            '    Return EPType.None
            'End If

        End Get
    End Property
    Private Sub InitNewPath()
        currentPath = New Path
        currentPath.Status = currentPath.Status Or Status.Draft
        currentPath.EPType = EpType
        _PathType = currentPath.EPType 'senza questo PathType cerca di leggerlo dal DB
        Me.ServiceEP.SaveEP(currentPath, Me.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ProxyIPadress, OLDpageUtility.ClientIPadress)
        If IsNothing(currentPath) Then
            Me.ShowError(EpError.Generic)
        Else
            Me.SessionUniqueKey = currentPath.Id.ToString & Me.CurrentCommunityID.ToString
            currentPath.EPType = currentPath.EPType Or EpType.PlayMode
            Me.InitDetail()
        End If
    End Sub

    Private Sub InitDetail()
        Me.InitWizardButton()

        Me.TXBname.Text = Me.currentPath.Name

        Try
            Me.RBLtype.SelectedValue = Me.currentPath.Type
        Catch ex As Exception
            Me.RBLtype.SelectedValue = 0
        End Try

        Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode)
        Me.CTRLeditorDescription.HTML = Me.currentPath.Description

        Me.TXBminCompletion.Text = currentPath.MinCompletion
        TXBminCompletion.Enabled = isManageable
        TXBminMark.Text = currentPath.MinMark
        TXBminMark.Enabled = isManageable

        TXBhours.Text = Math.Floor(currentPath.Weight / 60)
        TXBmins.Text = currentPath.Weight Mod 60
        TXBhours.NumberFormat.DecimalDigits = 0
        TXBmins.NumberFormat.DecimalDigits = 0

        Me.CKBplayMode.Checked = ServiceEP.CheckEpType(currentPath.EPType, EpType.PlayMode)
        Me.CKBsingleAction.Checked = currentPath.SingleAction
        Me.CKBfloatingDeadlines.Checked = currentPath.FloatingDeadlines


        If Not IsNothing(currentPath.EndDate) Then
            Dim endDate As DateTime = CType(currentPath.EndDate, DateTime)

            RDPendDate.SelectedDate = endDate
            TXBhEnd.Text = endDate.Hour
            TXBmEnd.Text = endDate.Minute

            If (currentPath.EndDate.HasValue) Then
                DIVsliding.Attributes("class") = "slidingDiv open"
                LBsetEndDate.Visible = False
            Else
                DIVsliding.Attributes("class") = "slidingDiv close"
                LBsetEndDate.Visible = True
            End If

            If Not IsNothing(currentPath.EndDateOverflow) Then
                Dim endDateOverFlow As DateTime = CType(currentPath.EndDateOverflow, DateTime)
                TXBhEndOver.Text = ((endDateOverFlow - endDate).Days * 24) + (endDateOverFlow - endDate).Hours
                TXBmEndOver.Text = (endDateOverFlow - endDate).Minutes

            End If
        End If



        TXBhEndOver.NumberFormat.DecimalDigits = 0
        TXBmEndOver.NumberFormat.DecimalDigits = 0
        TXBhEnd.NumberFormat.DecimalDigits = 0
        TXBmEnd.NumberFormat.DecimalDigits = 0

        CKBcontinueExecution.Checked = ServiceEP.CheckEpType(currentPath.EPType, lm.Comol.Modules.EduPath.Domain.EPType.AlwaysStat)

        Me.WZRpathCreate.ActiveStepIndex = StepPathManagement.Detail

        UpdateRadioButtonList(RBLcompletionPolicy, GetCompletionItems(currentPath.Policy.Statistics))
        UpdateRadioButtonList(RBLdisplayPolicy, GetDisplayItems(currentPath.Policy.DisplaySubActivity))
        UpdateRadioButtonList(RBLscormSettings, GetScormItems(currentPath.Policy.Scorm))
    End Sub

    Private Function GetCompletionItems(dItem As CompletionPolicy) As List(Of dtoSettingsItem)
        Dim items As List(Of CompletionPolicy) = (From e In [Enum].GetValues(GetType(CompletionPolicy)).Cast(Of CompletionPolicy)() Where e <> CompletionPolicy.UpdateOnlyIfWorst AndAlso e <> CompletionPolicy.UpdateOnlyIfBetter Select e).ToList()
        If Not items.Contains(dItem) Then
            dItem = items.FirstOrDefault()
        End If
        Return items.Select(Function(e) New dtoSettingsItem With {.Id = CInt(e), .IsSelected = (e = dItem), .Name = Resource.getValue("Selector.Name.CompletionPolicy." & e.ToString), .Description = Resource.getValue("Selector.Description.CompletionPolicy." & e.ToString)}).ToList()
    End Function
    Private Function GetDisplayItems(dItem As DisplayPolicy) As List(Of dtoSettingsItem)
        Dim items As List(Of DisplayPolicy) = (From e In [Enum].GetValues(GetType(DisplayPolicy)).Cast(Of DisplayPolicy)() Where e <> DisplayPolicy.InheritedByPath AndAlso e <> DisplayPolicy.InheritedByUnit AndAlso e <> DisplayPolicy.InheritedByActivity Select e).ToList()
        If Not items.Contains(dItem) Then
            dItem = items.FirstOrDefault()
        End If
        Return items.Select(Function(e) New dtoSettingsItem With {.Id = CInt(e), .IsSelected = (e = dItem), .Name = Resource.getValue("Selector.Name.DisplayPolicy." & e.ToString), .Description = Resource.getValue("Selector.Description.DisplayPolicy." & e.ToString)}).ToList()
    End Function
    Private Function GetScormItems(dItem As ScormSettingsPolicy) As List(Of dtoSettingsItem)
        Dim items As List(Of ScormSettingsPolicy) = (From e In [Enum].GetValues(GetType(ScormSettingsPolicy)).Cast(Of ScormSettingsPolicy)() Select e).ToList()
        If Not items.Contains(dItem) Then
            dItem = items.FirstOrDefault()
        End If
        Return items.Select(Function(e) New dtoSettingsItem With {.Id = CInt(e), .IsSelected = (e = dItem), .Name = Resource.getValue("Selector.Name.ScormSettingsPolicy." & e.ToString), .Description = Resource.getValue("Selector.Description.ScormSettingsPolicy." & e.ToString)}).ToList()
    End Function
    Private Sub UpdateRadioButtonList(obj As RadioButtonList, items As List(Of dtoSettingsItem))
        obj.Items.Clear()
        For Each item As dtoSettingsItem In items
            Dim oListItem As New ListItem() With {.Value = item.Id}
            oListItem.Attributes("class") = LToptionCssClass.Text
            oListItem.Selected = item.IsSelected
            oListItem.Text = Replace(LToptionTemplate.Text, "#label#", item.Name).Replace("#description#", item.Description)

            obj.Items.Add(oListItem)
        Next
    End Sub
    Private Function UpdateDetail()
        currentPath.Name = Me.TXBname.Text
        currentPath.Type = Me.RBLtype.SelectedValue

        currentPath.Description = removeBRfromStringEnd(Me.CTRLeditorDescription.HTML)

        currentPath.setLocked()

        If Not CKBplayMode.Checked AndAlso ServiceEP.CheckEpType(currentPath.EPType, EpType.PlayMode) Then
            currentPath.EPType = currentPath.EPType - EpType.PlayMode

        ElseIf CKBplayMode.Checked AndAlso Not ServiceEP.CheckEpType(currentPath.EPType, EpType.PlayMode) Then
            currentPath.EPType = currentPath.EPType Or EpType.PlayMode

        End If

        currentPath.SingleAction = CKBsingleAction.Checked
        currentPath.FloatingDeadlines = CKBfloatingDeadlines.Checked

        'If CKBmandatory.Checked Then
        '    oPath.Status = Status.Mandatory Or oPath.Status
        'ElseIf Not CKBmandatory.Checked AndAlso ((oPath.Status And Status.Mandatory) = Status.Mandatory) Then
        '    oPath.Status = oPath.Status - Status.Mandatory
        'End If
        currentPath.Weight = IIf(isAutoEp AndAlso isTimeEp, ServiceEP.ConvertTime(IIf(IsNumeric(TXBhours.Text), TXBhours.Text, 0), IIf(IsNumeric(TXBmins.Text), TXBmins.Text, 0)), 0)
        currentPath.MinCompletion = IIf(IsNumeric(TXBminCompletion.Text), TXBminCompletion.Text, 0)
        currentPath.MinMark = IIf(IsNumeric(TXBminMark.Text), TXBminMark.Text, 0)

        currentPath.EndDateOverflow = Nothing
        currentPath.EndDate = Nothing

        currentPath.Policy.Scorm = DirectCast(CInt(RBLscormSettings.SelectedValue), ScormSettingsPolicy)
        currentPath.Policy.Statistics = DirectCast(CInt(RBLcompletionPolicy.SelectedValue), CompletionPolicy)
        currentPath.Policy.DisplaySubActivity = DirectCast(CInt(RBLdisplayPolicy.SelectedValue), DisplayPolicy)

        If Not IsNothing(RDPendDate.SelectedDate()) Then
            Dim endDate As DateTime = RDPendDate.SelectedDate
            If Not IsNothing(TXBhEnd.Value) Then
                endDate = endDate.AddHours(TXBhEnd.Value)
            End If
            If Not IsNothing(TXBhEnd.Value) Then
                endDate = endDate.AddMinutes(TXBmEnd.Value)
            End If
            currentPath.EndDate = endDate

            Dim overDate As DateTime = endDate

            If Not IsNothing(TXBhEndOver.Value) Then
                overDate = overDate.AddHours(TXBhEndOver.Value)
            End If

            If Not IsNothing(TXBmEndOver.Value) Then
                overDate = overDate.AddMinutes(TXBmEndOver.Value)
            End If

            currentPath.EndDateOverflow = overDate

            If CKBcontinueExecution.Checked AndAlso Not ServiceEP.CheckEpType(currentPath.EPType, lm.Comol.Modules.EduPath.Domain.EPType.AlwaysStat) Then
                currentPath.EPType = lm.Comol.Modules.EduPath.Domain.EPType.AlwaysStat Or currentPath.EPType

            ElseIf Not CKBcontinueExecution.Checked AndAlso ServiceEP.CheckEpType(currentPath.EPType, lm.Comol.Modules.EduPath.Domain.EPType.AlwaysStat) Then
                currentPath.EPType = currentPath.EPType - lm.Comol.Modules.EduPath.Domain.EPType.AlwaysStat
            End If

            If Not IsNothing(RDPendDate.SelectedDate) AndAlso RDPendDate.SelectedDate < DateTime.Now Then
                LBerrorEndDate.Visible = True
                Return False
            Else
                LBerrorEndDate.Visible = False
                Return True
            End If
        End If
        Return True
    End Function

    Private Sub InitSelectPerson()
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBuserTitle)

        Dim ListCommunityID As New List(Of Integer)
        ListCommunityID.Add(Me.CurrentCommunityID)
        Dim PersonToHide As List(Of Integer) = Me.ServiceAssignment.GetIdFromDtoAssignementGenericWithDelete(Me.ListOfAssignmentByPerson)
        Me.CTRLselectUser.CurrentPresenter.Init(ListCommunityID, ListSelectionMode.Multiple, PersonToHide)
        Me.WZRpathCreate.ActiveStepIndex = StepPathManagement.SelectPerson
    End Sub
    Private Sub GetSelectedPerson()
        Me.ListOfAssignmentByPerson.AddRange(ServiceAssignment.GetDtoAssignemtGenericWithDeleteFromMembers(Me.CTRLselectUser.CurrentPresenter.GetConfirmedUsers))
    End Sub

    Private Sub InitSelectPermission()
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBpersonPermission)
        Me.Resource.setLabel(Me.LBcrolePermission)
        Me.Resource.setButton(Me.BTNselectPerson)
        Me.Resource.setLabel_To_Value(Me.LBpermissionTitle, "LBpermissionTitle.Path")
        'Me.SetHelpImg(Me.IMGhelp)
        'Me.SetHelpImg(Me.IMGhelpSelectPerm)
        Me.SubInitSelectPermissionByCRole()
        Me.SubInitSelectPermissionByPerson()
        Me.WZRpathCreate.ActiveStepIndex = StepPathManagement.SelectPermission
    End Sub

    Private Sub GetSelectedPermission()
        Me.SubGetSelectedPermissionByCRole()
        Me.SubGetSelectedPermissionByPerson()
    End Sub

    Private Sub SubInitSelectPermissionByCRole()
        If Me.ListOfAssignmentByCommRole.Count = 0 Then
            If (currentPath.Status And Status.Draft) = Status.Draft Then
                Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRolePathAssignment(0, Me.CurrentCommunityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentContext.UserContext.Language.Id)
            Else
                Me.ListOfAssignmentByCommRole = Me.ServiceAssignment.GetListCRolePathAssignment(currentPath.Id, Me.CurrentCommunityID, Me.CurrentContext.UserContext.CurrentUserID, Me.CurrentContext.UserContext.Language.Id)
            End If
        End If
        Me.RPcrolePermission.DataSource = Me.ListOfAssignmentByCommRole
        Me.RPcrolePermission.DataBind()
    End Sub

    Private Sub SubGetSelectedPermissionByCRole()
        Me.ListOfAssignmentByCommRole.Clear()
        Dim oLabel As Label
        Dim oCkb As CheckBox
        Dim oDtoAssign As dtoGenericAssignment
        For Each item As RepeaterItem In Me.RPcrolePermission.Items
            If (item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.Item) Then
                oDtoAssign = New dtoGenericAssignment
                oLabel = item.FindControl("LBcommRole")
                If Not IsNothing(oLabel) Then
                    oDtoAssign.ItemID = oLabel.Attributes.Item("CRoleID")
                    oDtoAssign.DB_ID = oLabel.Attributes.Item("AssignmentID")
                    oDtoAssign.ItemName = oLabel.Text
                End If
                oCkb = item.FindControl("CKBpartecipant")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.Participant
                    End If
                End If

                oCkb = item.FindControl("CKBstatviewer")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.StatViewer Or oDtoAssign.RoleEP
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
        If ListOfAssignmentByPerson.Count = 0 And ((currentPath.Status And Status.Draft) <> Status.Draft) Then
            Me.ListOfAssignmentByPerson = ServiceAssignment.GetPathPersonAssignments(currentPath.Id)
        End If
        If Me.ListOfAssignmentByPerson.Count > 0 Then
            Me.RPuserPermission.DataSource = Me.ListOfAssignmentByPerson
            Me.RPuserPermission.DataBind()
        Else
            Me.LBpersonPermission.Visible = False
        End If
    End Sub
    Private Sub SubGetSelectedPermissionByPerson()
        Me.ListOfAssignmentByPerson.Clear()
        Dim oLabel As Label
        Dim oCkb As CheckBox
        Dim oDtoAssign As dtoGenericAssignmentWithDelete
        For Each item As RepeaterItem In Me.RPuserPermission.Items
            If (item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.Item) Then
                oDtoAssign = New dtoGenericAssignmentWithDelete
                oLabel = item.FindControl("LBperson")
                If Not IsNothing(oLabel) Then
                    oDtoAssign.ItemID = oLabel.Attributes.Item("PersonID")
                    oDtoAssign.ItemName = oLabel.Text
                    oDtoAssign.DB_ID = oLabel.Attributes.Item("AssignmentID")
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

                oCkb = item.FindControl("CKBstatviewer")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.StatViewer Or oDtoAssign.RoleEP
                    End If
                End If

                oCkb = item.FindControl("CKBmanager")
                If Not IsNothing(oCkb) Then
                    If oCkb.Checked Then
                        oDtoAssign.RoleEP = RoleEP.Manager Or oDtoAssign.RoleEP
                    End If
                End If
                oCkb = item.FindControl("CKBactivePerson")
                If Not IsNothing(oCkb) Then
                    oDtoAssign.isDeleted = Not oCkb.Checked
                End If

            End If
            Me.ListOfAssignmentByPerson.Add(oDtoAssign)
        Next
    End Sub

    Private Sub InitSummary()
        Me.InitWizardButton()
        Me.Resource.setLabel(Me.LBcroleSummary)
        Me.Resource.setLabel(Me.LBpersonSummary)
        Me.Resource.setLabel_To_Value(Me.LBsummaryTitle, "LBsummaryTitle.Path")
        Me.LBnameTitleRes.Text = Me.Resource.getValue("LBnameTitle.text")
        Me.LBdescriptionTitleRes.Text = Me.Resource.getValue("LBdescriptionTitle.text")

        Me.LBminCompletionTitleRes.Text = Me.Resource.getValue("LBminCompletionTitle.text")
        Me.LBminMarkTitleRes.Text = Me.Resource.getValue("LBminMark.text")

        Me.LBstatusTitleRes.Text = Me.Resource.getValue("LBstatusTitle.text")
        If Not ((currentPath.Status And Status.Locked) = Status.Locked) Then
            Me.LBstatusRes.Text = Me.Resource.getValue("Status.Visible")
        Else
            Me.LBstatusRes.Text = Me.Resource.getValue("Status." & Status.Locked.ToString)
        End If
        Me.LBnameRes.Text = System.Web.HttpUtility.HtmlEncode(currentPath.Name)
        LBdescriptionRes.Text = currentPath.Description 'System.Web.HttpUtility.HtmlEncode(currentPath.Description)
        Me.LBminCompletionRes.Text = currentPath.MinCompletion
        If isAutoEp Then
            hideControl(DIVminMark)
        Else
            LBminMarkRes.Text = currentPath.MinMark
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

        Me.LBplayMode.Text = Me.Resource.getValue(ServiceEP.CheckEpType(currentPath.EPType, lm.Comol.Modules.EduPath.Domain.EPType.PlayMode).ToString)

        Me.LBsingleAction.Text = Me.Resource.getValue(currentPath.SingleAction.ToString)

        Me.LBfloatingDeadlines.Text = Me.Resource.getValue(currentPath.FloatingDeadlines.ToString)

        If IsNothing(currentPath.EndDate) Then
            hideControl(DIVendTimeRes)
        Else
            Me.LBendDateRes.Text = ServiceEP.GetDate(currentPath.EndDate)

            If IsNothing(currentPath.EndDateOverflow) Then
                hideControl(DIVendDateOver)
            Else
                Dim endDate As DateTime = CType(currentPath.EndDate, DateTime)
                Dim endDateOverflow As DateTime = CType(currentPath.EndDateOverflow, DateTime)
                Dim time As Int16 = ((endDateOverflow - endDate).Days * 24 + (endDateOverflow - endDate).Hours) * 60 + (endDateOverflow - endDate).Minutes

                LBendDateOverflowRes.Text = ServiceEP.GetTime(time)
            End If

            Me.LBcontinueExecution.Text = Me.Resource.getValue(ServiceEP.CheckEpType(currentPath.EPType, lm.Comol.Modules.EduPath.Domain.EPType.AlwaysStat).ToString)
        End If
        LBcompletionPolicy.Text = Resource.getValue("Selector.Name.CompletionPolicy." & currentPath.Policy.Statistics.ToString)
        LBdisplayPolicy.Text = Resource.getValue("Selector.Name.DisplayPolicy." & currentPath.Policy.DisplaySubActivity.ToString)
        LBscormSettingsPolicy.Text = Resource.getValue("Selector.Name.ScormSettingsPolicy." & currentPath.Policy.Scorm.ToString)
        Me.WZRpathCreate.ActiveStepIndex = StepPathManagement.SummaryPermission
    End Sub

    Private Function PersistData() As Boolean
        Dim IsTransactionExecute As Boolean = Me.ServiceEP.SaveOrUpdateEPandAssignment(currentPath, ListOfAssignmentByCommRole, ListOfAssignmentByPerson, CurrentCommunityID, Me.CurrentContext.UserContext.CurrentUserID, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
        If IsTransactionExecute Then
            ClearSession()
        Else
            Me.ShowError(EpError.Generic)
        End If
        Return IsTransactionExecute
    End Function

    Public ReadOnly Property Sliding As String
        Get
            If currentPath.EndDate.HasValue Then
                Return "open"
            Else
                Return "closed"
            End If
        End Get

    End Property



#Region "Button"

    Private Sub InitWizardButton()
        Dim oButton As Button
        oButton = Me.WZRpathCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNcancel")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
        End If
        oButton = Me.WZRpathCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNprevious")
        If Not IsNothing(oButton) Then
            Me.Resource.setButton(oButton, True)
            oButton.Visible = (Me.CurrentStep = StepPathManagement.SummaryPermission Or Me.CurrentStep = StepPathManagement.SelectPermission)
        End If
        oButton = Me.WZRpathCreate.FindControl("StepNavigationTemplateContainerID").FindControl("BTNnext")
        If Not IsNothing(oButton) Then
            If Me.CurrentStep = StepPathManagement.SummaryPermission Then
                Me.Resource.setButtonByValue(oButton, "complete", True)
            Else
                Me.Resource.setButtonByValue(oButton, "next", True)
            End If

        End If
    End Sub


    Public Sub BTNnext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Select Case Me.CurrentStep
            Case StepPathManagement.NewPath
                Dim riseAlert As Boolean = isAutoEp AndAlso ServiceEP.CheckStatus(currentPath.Status, Status.NotLocked) AndAlso Not currentPath.WeightAuto = (TXBhours.Text * 60 + TXBmins.Text)
                If riseAlert Then
                    LITalert.Text = Resource.getValue("MSGalert")
                End If
                If Me.UpdateDetail() Then
                    If Not riseAlert Then
                        If PersistData() Then
                            RedirectToUrl((RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SelectPermission, EpType)))
                        Else
                            Me.ShowError(EpError.Generic)
                        End If
                    End If
                Else
                    BindDati()
                End If

            Case StepPathManagement.Detail
                Dim riseAlert As Boolean = isAutoEp AndAlso ServiceEP.CheckStatus(currentPath.Status, Status.NotLocked) AndAlso Not currentPath.WeightAuto = (TXBhours.Text * 60 + TXBmins.Text)
                If riseAlert Then
                    LITalert.Text = Resource.getValue("MSGalert")

                End If
                If Me.UpdateDetail() Then
                    If Not riseAlert Then
                        If PersistData() Then
                            RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SelectPermission, EpType))
                        Else
                            ShowError(EpError.Generic)
                        End If
                    End If
                Else
                    BindDati()
                End If


            Case StepPathManagement.SelectPermission
                Me.GetSelectedPermission()
                RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SummaryPermission, EpType))
            Case StepPathManagement.SelectPerson
                Me.GetSelectedPerson()
                RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SelectPermission, EpType))
            Case StepPathManagement.SummaryPermission
                If Me.ServiceEP.CheckStatus(currentPath.Status, Status.Draft) Then
                    currentPath.Status = currentPath.Status - Status.Draft
                End If
                If PersistData() Then
                    RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage) & "#" & CurrentPathID)
                End If

            Case StepPathManagement.Update
                Dim riseAlert As Boolean = isAutoEp AndAlso ServiceEP.CheckStatus(currentPath.Status, Status.NotLocked) AndAlso Not currentPath.WeightAuto = (TXBhours.Text * 60 + TXBmins.Text)
                If riseAlert Then
                    LITalert.Text = Resource.getValue("MSGalert")

                End If
                If Not riseAlert Then
                    Me.UpdateDetail()
                    If PersistData() Then
                        RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SelectPermission, EpType))
                    Else
                        ShowError(EpError.Generic)
                    End If
                End If

            Case StepPathManagement.SelectCommunity
                Dim SelectedCommId = GetSelectedCommunityID()
                Me.PageUtility.RedirectToUrl("Modules/EduPath/PathManagement.aspx?ComId=" & SelectedCommId & "&Step=" & StepPathManagement.NewPath)
        End Select
    End Sub
    Private Function GetSelectedCommunityID() As Integer
        Dim SelectedCommunities As List(Of Integer) = Me.CTRLcommunity.SelectedCommunitiesID
        If SelectedCommunities.Count = 1 Then
            Return SelectedCommunities.First
        Else
            Return -1
        End If
    End Function
    Public Sub BTNprevious_Click(ByVal sender As Object, ByVal e As EventArgs)
        Select Case Me.CurrentStep
            '   Case StepPathCreate.Detail

            Case StepPathManagement.SelectPermission
                Me.GetSelectedPermission()
                Me.PageUtility.RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.Detail, EpType))
            Case StepPathManagement.SelectPerson
                Me.GetSelectedPerson()
                Me.PageUtility.RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SelectPermission, EpType))
            Case StepPathManagement.SummaryPermission
                Me.PageUtility.RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SelectPermission, EpType))
        End Select
    End Sub
    Public Sub BTNcancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        If (currentPath.Status And Status.Draft) = Status.Draft Then
            If Not ServiceEP.DeleteOnlyEP(currentPath.Id) Then
                Me.ShowError(EpError.Generic)
            End If
        End If
        ClearSession()
        PageUtility.RedirectToUrl(RootObject.EduPathList(Me.CurrentCommunityID, EpViewModeType.Manage))
    End Sub
    Private Sub BTNselectPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNselectPerson.Click
        Me.GetSelectedPermission()
        Me.PageUtility.RedirectToUrl(RootObject.PathManagement(CurrentCommunityID, currentPath.Id, StepPathManagement.SelectPerson, EpType))
    End Sub
#End Region

#Region "RP ItemDataBound"

    Public Sub RPSummary_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
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
                If Not IsNothing(oImg) Then
                    If (oDto.RoleEP And RoleEP.Participant) = RoleEP.Participant Then
                        Text = Me.Resource.getValue(RoleEP.Participant.ToString & ".True")
                        oImg.ImageUrl = RootObject.ImgGreen(Me.BaseUrl)
                        oImg.ToolTip = Text
                        oImg.AlternateText = Text
                    Else
                        oImg.Visible = False
                        'Text = Me.Resource.getValue(RoleEP.Participant.ToString & ".False")
                        'oImg.ImageUrl = Me.BaseUrl & "Modules/EduPath/img/rosso.gif"
                        'oImg.ToolTip = Text
                        'oImg.AlternateText = Text
                    End If
                End If

                oImg = e.Item.FindControl("IMGstatviewer")
                If Not IsNothing(oImg) Then
                    If (oDto.RoleEP And RoleEP.StatViewer) = RoleEP.StatViewer Then
                        Text = Me.Resource.getValue(RoleEP.StatViewer.ToString & ".True")
                        oImg.ImageUrl = RootObject.ImgGreen(Me.BaseUrl)
                        oImg.ToolTip = Text
                        oImg.AlternateText = Text
                    Else
                        oImg.Visible = False
                        'Text = Me.Resource.getValue(RoleEP.Participant.ToString & ".False")
                        'oImg.ImageUrl = Me.BaseUrl & "Modules/EduPath/img/rosso.gif"
                        'oImg.ToolTip = Text
                        'oImg.AlternateText = Text
                    End If
                End If

                oImg = e.Item.FindControl("IMGevaluator")
                If Not IsNothing(oImg) Then
                    If (oDto.RoleEP And RoleEP.Evaluator) = RoleEP.Evaluator Then
                        Text = Me.Resource.getValue(RoleEP.Evaluator.ToString & ".True")
                        oImg.ImageUrl = RootObject.ImgGreen(Me.BaseUrl)
                        oImg.ToolTip = Text
                        oImg.AlternateText = Text
                    Else
                        oImg.Visible = False
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

            oLabel = e.Item.FindControl("LBstatviewerTitle")

            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RoleEP." & RoleEP.StatViewer.ToString)
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
            Dim dtoCRoleAss As dtoGenericAssignment
            Dim oCkb As CheckBox
            Try


                dtoCRoleAss = DirectCast(e.Item.DataItem, dtoGenericAssignment)
                oLabel = e.Item.FindControl("LBcommRole")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = dtoCRoleAss.ItemName
                    oLabel.Attributes.Add("CRoleID", dtoCRoleAss.ItemID.ToString)
                    oLabel.Attributes.Add("AssignmentID", dtoCRoleAss.DB_ID.ToString)
                End If
                oCkb = e.Item.FindControl("CKBpartecipant")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Participant.ToString)
                    oCkb.Checked = (dtoCRoleAss.RoleEP And RoleEP.Participant) = RoleEP.Participant
                End If

                oCkb = e.Item.FindControl("CKBstatviewer")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.StatViewer.ToString)
                    oCkb.Checked = (dtoCRoleAss.RoleEP And RoleEP.StatViewer) = RoleEP.StatViewer
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
            Dim dtoPersonAss As dtoGenericAssignmentWithDelete
            Dim oCkb As CheckBox
            Try

                dtoPersonAss = DirectCast(e.Item.DataItem, dtoGenericAssignmentWithDelete)
                oLabel = e.Item.FindControl("LBperson")
                If Not IsNothing(oLabel) Then
                    oLabel.Text = dtoPersonAss.ItemName
                    oLabel.Attributes.Add("PersonID", dtoPersonAss.ItemID.ToString)
                    oLabel.Attributes.Add("AssignmentID", dtoPersonAss.DB_ID.ToString)
                End If
                oCkb = e.Item.FindControl("CKBactivePerson")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("Select")
                    oCkb.Checked = Not dtoPersonAss.isDeleted
                End If
                oCkb = e.Item.FindControl("CKBpartecipant")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Participant.ToString)
                    oCkb.Checked = (dtoPersonAss.RoleEP And RoleEP.Participant) = RoleEP.Participant
                End If

                oCkb = e.Item.FindControl("CKBstatviewer")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.StatViewer.ToString)
                    oCkb.Checked = (dtoPersonAss.RoleEP And RoleEP.StatViewer) = RoleEP.StatViewer
                End If

                oCkb = e.Item.FindControl("CKBevaluator")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Evaluator.ToString)
                    oCkb.Checked = (dtoPersonAss.RoleEP And RoleEP.Evaluator) = RoleEP.Evaluator
                End If
                oCkb = e.Item.FindControl("CKBmanager")
                If Not IsNothing(oCkb) Then
                    oCkb.Text = Me.Resource.getValue("RoleEP." & RoleEP.Manager.ToString)
                    oCkb.Checked = (dtoPersonAss.RoleEP And RoleEP.Manager) = RoleEP.Manager
                End If
            Catch ex As Exception
            End Try
        ElseIf e.Item.ItemType = ListItemType.Header Then

            'Me.SetHelpImg(e.Item.FindControl("IMGhelp"))

            oLabel = e.Item.FindControl("LBpersonTitle")
            If Not IsNothing(oLabel) Then
                oLabel.Text = Me.Resource.getValue("RPtitle.Person")
            End If

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
        MLVpathCreate.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Class dtoSettingsItem
        Public Id As Integer
        Public Name As String
        Public Description As String
        Public IsSelected As Boolean
    End Class
End Class

Enum StepPathManagement
    ErrorStep = -1000
    Update = -2 'Step only for querystring
    NewPath = -1 'Step only for querystring
    Detail = 0
    SelectPermission = 1
    SelectPerson = 2
    SummaryPermission = 3
    SelectCommunity = 4
End Enum