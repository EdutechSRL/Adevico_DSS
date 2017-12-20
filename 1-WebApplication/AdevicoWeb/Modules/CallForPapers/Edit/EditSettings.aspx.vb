Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract

Public Class EditCallSettings
    Inherits PageBase
    Implements IViewEditCallSettings

#Region "Context"
    Private _Presenter As EditCallSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As EditCallSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditCallSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
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
    Private Property AllowStatusEdit As Boolean Implements IViewEditCallSettings.AllowStatusEdit
        Get
            Return ViewStateOrDefault("AllowStatusEdit", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowStatusEdit") = value
            DDLstatus.Enabled = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewEditCall.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            Me.CTRLprintSet.IsReadOnly = Not value
            CBXsignMandatory.Enabled = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewEditCall.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
            Resource.setHyperLink(HYPpreviewCallBottom, value.ToString(), True, True)
            Resource.setHyperLink(HYPpreviewCallTop, value.ToString(), True, True)
            Resource.setHyperLink(HYPbackTop, value.ToString, True, True)
            Resource.setHyperLink(HYPbackBottom, value.ToString, True, True)

            If value = CallForPaperType.RequestForMembership Then
                Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode)
                Me.CTRLeditorSummary.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode)
            Else
                Me.CTRLeditorDescription.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode)
                Me.CTRLeditorSummary.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode)
            End If

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
            If Me.DDLstatus.Items.Count = 0 Then
                Return CallForPaperStatus.Draft
            Else
                Return Me.DDLstatus.SelectedValue
            End If
        End Get
        Set(ByVal value As CallForPaperStatus)
            If IsNothing(Me.DDLstatus.Items.FindByValue(value)) Then
                Me.DDLstatus.Items.Add(New ListItem(Resource.getValue("CallForPaperStatus." & value.ToString), value))
            End If
            Me.DDLstatus.SelectedValue = value
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
    Private Property ForPortal As Boolean Implements IViewEditCallSettings.ForPortal
        Get
            Return ViewStateOrDefault("ForPortal", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("ForPortal") = value
        End Set
    End Property
    Private ReadOnly Property CurrentCall As dtoBaseForPaper Implements IViewEditCallSettings.CurrentCallForPaper
        Get
            Dim dto As dtoBaseForPaper
            If CallType = CallForPaperType.CallForBids Then
                dto = New dtoCall
            Else
                dto = New dtoRequest
            End If
            With dto
                .Id = IdCall
                .Status = CurrentStatus
                .AllowSubmissionExtension = CBXallowExtensionDate.Checked
                .RevisionSettings = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RevisionMode).GetByString(Me.RBLrevisionType.SelectedValue, RevisionMode.None)
                .AcceptRefusePolicy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of NotifyAcceptRefusePolicy).GetByString(Me.RBLacceptRefusePolicy.SelectedValue, NotifyAcceptRefusePolicy.None)

                .Summary = Me.CTRLeditorSummary.HTML
                .Description = Me.CTRLeditorDescription.HTML

                .Edition = Me.TXBedition.Text
                .StartDate = Me.RDPfromDay.SelectedDate
                'If RDPtoDay.SelectedDate.HasValue AndAlso .StartDate > RDPtoDay.SelectedDate.Value Then
                '    .StartDate = RDPtoDay.SelectedDate
                '    .EndDate = Me.RDPfromDay.SelectedDate
                '    RDPtoDay.SelectedDate = .EndDate
                '    Me.RDPtoDay.SelectedDate = .StartDate
                'Else
                If RDPtoDay.SelectedDate.HasValue Then
                    .EndDate = RDPtoDay.SelectedDate.Value
                End If
                'End If
                .IsPublic = False
                .Name = Me.TXBtitle.Text
                .OverrideHours = Me.DDLhours.SelectedValue
                .OverrideMinutes = Me.DDLminutes.SelectedValue
                .SubmissionClosed = CBXsubmissionClosed.Checked
                .AttachSign = CBXsignMandatory.Checked

            End With
            If TypeOf (dto) Is dtoCall Then
                Dim dtoC As dtoCall = DirectCast(dto, dtoCall)
                With dtoC
                    .AdvacedEvaluation = (Me.RBLcommissionType.SelectedValue = "1")
                    .AwardDate = TXBawardDate.Text
                    .DisplayWinner = CBXdisplayWinner.Checked
                    If RDPendEvaluationOn.SelectedDate.HasValue Then
                        .EndEvaluationOn = RDPendEvaluationOn.SelectedDate.Value
                        '    If .EndDate.HasValue AndAlso .EndDate.Value >= .EndEvaluationOn.Value Then
                        '        .EndEvaluationOn = .EndDate.Value.AddDays(1)
                        '        RDPendEvaluationOn.SelectedDate = .EndEvaluationOn
                        '    ElseIf Not .EndDate.HasValue And .StartDate >= .EndEvaluationOn.Value Then
                        '        .EndEvaluationOn = .StartDate.AddDays(7)
                        '        RDPendEvaluationOn.SelectedDate = .EndEvaluationOn
                        '    End If
                    End If
                    If Me.DDLevaluationType.SelectedIndex <> -1 Then
                        .EvaluationType = Me.DDLevaluationType.SelectedValue
                    End If
                End With
                Return dtoC
            Else
                Return dto
            End If
        End Get
    End Property
    Private Property IdCallModule As Integer Implements IViewBaseEditCall.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property InvalidStatusFound As Boolean Implements IViewEditCallSettings.InvalidStatusFound
        Get
            Return ViewStateOrDefault("InvalidStatusFound", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("InvalidStatusFound") = value
            If Not value Then
                CloseDialog()
            End If
        End Set
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
        Me.CurrentPresenter.InitView(SystemSettings.Presenter.AllowDssUse)


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
            CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & CallType.ToString), Helpers.MessageType.error)
            .setHyperLink(HYPpreviewCallBottom, CallType.ToString(), True, True)
            .setHyperLink(HYPpreviewCallTop, CallType.ToString(), True, True)
            If IdCall > 0 Then
                .setHyperLink(HYPbackTop, CallType.ToString, True, True)
                .setHyperLink(HYPbackBottom, CallType.ToString, True, True)
            Else
                .setHyperLink(HYPbackTop, PreloadType.ToString, True, True)
                .setHyperLink(HYPbackBottom, PreloadType.ToString, True, True)
            End If
         
            .setButton(BTNsaveSettingsTop, True, , , True)
            .setButton(BTNsaveSettingsBottom, True, , , True)

            .setLabel(LBtitle_t)
            .setLabel(LBedition_t)
            .setLabel(LBdescription_t)
            .setLabel(LBsummary_t)
            .setLabel(LBtoDate_t)
            .setLabel(LBfromDate_t)
            .setLabel(LBoverrideData)
            .setLabel(LBhours)
            .setLabel(LBminutes)
            .setLabel(LBallowExtensionDate)
            .setLabel(LBallowExtensionDate)
            .setLabel(LBAwardDate)
            .setLabel(LBsubmissionClosed)
            .setLabel(LBstatus)
            .setButton(BTNconfirmStatus, True, , , True)
            .setButton(BTNundoConfirmStatus, True, , , True)
            .setLabel(LBconfirmEndDate)
            .setDropDownList(Me.DDLevaluationType, CInt(EvaluationType.Average))
            .setDropDownList(Me.DDLevaluationType, CInt(EvaluationType.Sum))
            .setDropDownList(Me.DDLevaluationType, CInt(EvaluationType.Dss))
            .setLabel(LBallowRevision_t)
            .setRadioButtonList(RBLrevisionType, RevisionMode.None.ToString())
            .setRadioButtonList(RBLrevisionType, RevisionMode.OnlyManager.ToString())
            .setRadioButtonList(RBLrevisionType, RevisionMode.ManagerSubmitter.ToString())

            .setLabel(LBAcceptRefusePolicy_t)
            .setRadioButtonList(RBLacceptRefusePolicy, NotifyAcceptRefusePolicy.None.ToString())
            .setRadioButtonList(RBLacceptRefusePolicy, NotifyAcceptRefusePolicy.Accept.ToString())
            .setRadioButtonList(RBLacceptRefusePolicy, NotifyAcceptRefusePolicy.Refuse.ToString())
            .setRadioButtonList(RBLacceptRefusePolicy, NotifyAcceptRefusePolicy.All.ToString())
            'AGGIUNTO DA MD in fase di correzione traduzioni
            .setLabel(LBendEvaluationOn)
            .setLabel(LBevaluationResults_t)
            .setLabel(LBdisplayWinner)
            .setLabel(LBAwardDate)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditCall.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditCall.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
        Me.CloseDialog()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        If CurrentAction = CallStandardAction.Add Then
            dto.DestinationUrl = RootObject.AddCall(CallType, idCommunity, PreloadView)
        Else
            dto.DestinationUrl = RootObject.EditCallSettings(CallType, PreloadIdCall, idCommunity, PreloadView)
        End If

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

    Private Sub DisplayDateError(startDate As DateTime, endDate As DateTime) Implements IViewEditCallSettings.DisplayDateError
        CTRLmessages.Visible = True
        Dim message As String = Resource.getValue("DisplayDateError")
        If Not String.IsNullOrEmpty(message) Then
            message = String.Format(message, FormatDateTime(startDate, DateFormat.ShortDate) & " " & FormatDateTime(startDate, DateFormat.ShortTime), FormatDateTime(endDate, DateFormat.ShortDate) & " " & FormatDateTime(endDate, DateFormat.ShortTime))
        End If
        CTRLmessages.InitializeControl(message, Helpers.MessageType.alert)
        Me.CloseDialog()
    End Sub
    Private Sub DisplayDateError(type As CallForPaperType) Implements IViewEditCallSettings.DisplayDateError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayDateError." & type.ToString), Helpers.MessageType.alert)
        Me.CloseDialog()
    End Sub
    Private Sub DisplayEvaluationDateError(endDate As DateTime, evaluationDate As DateTime) Implements IViewEditCallSettings.DisplayEvaluationDateError
        CTRLmessages.Visible = True
        Dim message As String = Resource.getValue("DisplayEvaluationDateError")
        If Not String.IsNullOrEmpty(message) Then
            message = String.Format(message, FormatDateTime(endDate, DateFormat.ShortDate) & " " & FormatDateTime(endDate, DateFormat.ShortTime), FormatDateTime(endDate, DateFormat.ShortDate) & " " & FormatDateTime(endDate, DateFormat.ShortTime))
        End If
        CTRLmessages.InitializeControl(message, Helpers.MessageType.alert)
        Me.CloseDialog()
    End Sub
    Private Sub DisplaySettingsError() Implements IViewEditCallSettings.DisplaySettingsError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("EditSettings.DisplaySettingsError"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewEditCallSettings.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("EditSettings.DisplaySettingsSaved"), Helpers.MessageType.success)
        Me.CloseDialog()
    End Sub
    Private Sub DisplaySkippedRequiredSteps(steps As List(Of WizardCallStep)) Implements IViewEditCallSettings.DisplaySkippedRequiredSteps
        CTRLmessages.Visible = True
        Dim message As String = Resource.getValue("DisplaySkippedRequiredSteps")
        If Not String.IsNullOrEmpty(message) Then
            Dim stepTranslation As String = ""
            For Each item As WizardCallStep In steps
                stepTranslation &= ", " & Me.Resource.getValue("WizardCallStep." & item.ToString)
            Next
            If Not String.IsNullOrEmpty(stepTranslation) Then
                stepTranslation = Right(stepTranslation, stepTranslation.Length - 2)
            End If
            message = String.Format(message, stepTranslation)
        End If
        CTRLmessages.InitializeControl(message, Helpers.MessageType.alert)
        Me.CloseDialog()
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
        Dim toolTip As String = title
        If CurrentAction = CallStandardAction.Edit Then
            title = String.Format(title, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
            title = String.Format(title, itemName)
        End If
        Master.ServiceTitle = title
        If String.IsNullOrEmpty(name) Then
            Master.ServiceTitleToolTip = toolTip
        Else
            toolTip = Me.Resource.getValue("serviceTitle.Community." & action.Edit.ToString() & "." & CallType.ToString())
            If Not String.IsNullOrEmpty(toolTip) Then
                Master.ServiceTitleToolTip = String.Format(toolTip, name)
            End If
        End If
    End Sub
    Private Sub LoadStatus(items As List(Of CallForPaperStatus)) Implements IViewEditCallSettings.LoadStatus
        Me.DDLstatus.Items.Clear()
        Me.DDLstatus.Items.AddRange((From s As CallForPaperStatus In items Select New ListItem(Resource.getValue("CallForPaperStatus." & s.ToString), CInt(s))).OrderBy(Function(l) l.Text).ToArray)
    End Sub
    Private Sub LoadStatus(items As List(Of CallForPaperStatus), selected As CallForPaperStatus) Implements IViewEditCallSettings.LoadStatus
        Me.DDLstatus.Items.Clear()
        Me.DDLstatus.Items.AddRange((From s As CallForPaperStatus In items Select New ListItem(Resource.getValue("CallForPaperStatus." & s.ToString), CInt(s))).OrderBy(Function(l) l.Text).ToArray)
        Me.CurrentStatus = selected
    End Sub
    Private Sub LoadEmptyCall() Implements IViewEditCallSettings.LoadEmptyCall
        Me.MLVsettings.SetActiveView(VIWsettings)
        TXBedition.Text = ""
        TXBtitle.Text = "--"
        CTRLeditorSummary.HTML = ""
        CTRLeditorDescription.HTML = ""
        RDPfromDay.SelectedDate = Date.Now
        RDPtoDay.SelectedDate = Date.Now.AddMonths(2)
        DDLhours.SelectedValue = 0
        DDLminutes.SelectedValue = 0
        CBXallowExtensionDate.Checked = True
        Me.RBLrevisionType.SelectedValue = RevisionMode.None.ToString()
        Me.RBLacceptRefusePolicy.SelectedValue = NotifyAcceptRefusePolicy.None.ToString()
        CBXdisplayWinner.Checked = False
        CBXsubmissionClosed.Checked = False
        Me.TXBawardDate.Text = ""
        Me.CurrentStatus = CallForPaperStatus.Draft
        Me.RDPendEvaluationOn.SelectedDate = Nothing
        FLDevaluation.Visible = (CallType = CallForPaperType.CallForBids)
        DVacceptRefusePolicy.Visible = (CallType = CallForPaperType.CallForBids)
        DVrevision.Visible = (CallType = CallForPaperType.CallForBids)

        CTRLprintSet.InitControl(0)
    End Sub
    Private Sub LoadCall(item As dtoBaseForPaper, canChangeCommission As Boolean) Implements IViewEditCallSettings.LoadCall
        Me.MLVsettings.SetActiveView(VIWsettings)
        With item
            TXBedition.Text = .Edition
            TXBtitle.Text = .Name
            CTRLeditorSummary.HTML = .Summary
            CTRLeditorDescription.HTML = .Description
            RDPfromDay.SelectedDate = .StartDate
            If .EndDate.HasValue Then
                RDPtoDay.SelectedDate = .EndDate.Value
            End If
            DDLhours.SelectedValue = .OverrideHours
            DDLminutes.SelectedValue = .OverrideMinutes
            CBXallowExtensionDate.Checked = .AllowSubmissionExtension
            RBLrevisionType.SelectedValue = item.RevisionSettings.ToString()
            RBLacceptRefusePolicy.SelectedValue = item.AcceptRefusePolicy.ToString()
            FLDevaluation.Visible = False
            DVrevision.Visible = (item.Type = CallForPaperType.CallForBids)

            DVadvancedCommission.Visible = (item.Type = CallForPaperType.CallForBids) 'ToDo: mettere " AND Application("Se c'è lovedi")
            If item.AdvacedEvaluation Then
                Me.RBLcommissionType.SelectedValue = "1"
            End If

            Me.RBLcommissionType.Enabled = canChangeCommission

            CBXsignMandatory.Checked = .AttachSign
            FLDSsignMandatory.Visible = (item.Type = CallForPaperType.CallForBids)

            CTRLprintSet.InitControl(.Id)
        End With
    End Sub
    Private Sub LoadEvaluationSettings(settings As dtoEvaluationSettings) Implements IViewEditCallSettings.LoadEvaluationSettings
        FLDevaluation.Visible = True
        If Not IsNothing(settings) Then
            With settings
                CBXdisplayWinner.Checked = .DisplayWinner
                Me.TXBawardDate.Text = .AwardDate
                If .EndEvaluationOn.HasValue Then
                    Me.RDPendEvaluationOn.SelectedDate = .EndEvaluationOn
                End If
                If Me.DDLevaluationType.Items.Count > 0 AndAlso Not IsNothing(Me.DDLevaluationType.Items.FindByValue(settings.EvaluationType)) Then
                    Me.DDLevaluationType.SelectedValue = settings.EvaluationType
                    If settings.EvaluationType <> EvaluationType.Dss AndAlso Not SystemSettings.Presenter.AllowDssUse Then
                        DDLevaluationType.Items.RemoveAt(2)
                    End If
                    For Each t As EvaluationType In (From e As EvaluationType In [Enum].GetValues(GetType(EvaluationType)) Select e)
                        Dim oListItem As ListItem = DDLevaluationType.Items.FindByValue(CInt(t))
                        If Not IsNothing(oListItem) Then
                            oListItem.Enabled = settings.AllowedChangeTo.Contains(t)
                        End If
                    Next
                    If SystemSettings.Presenter.AllowDssUse AndAlso settings.AllowedTypes.Contains(EvaluationType.Dss) Then
                        DVevaluationMessage.Visible = True
                        CTRLevaluationMessage.InitializeControl(Resource.getValue("IViewEditCallSettings.UseOfDssEvaluation"), Helpers.MessageType.alert)
                    End If
                End If
            End With
        Else
            CBXdisplayWinner.Checked = False
            Me.TXBawardDate.Text = ""
        End If
    End Sub
    Public Sub LoadWizardSteps(idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of WizardCallStep))) Implements IViewEditCall.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, type, idCommunity, PreloadView, steps)
    End Sub

    Private Sub LoadInvalidStatus(status As CallForPaperStatus, endDate As Date?) Implements IViewEditCallSettings.LoadInvalidStatus
        Me.DVendDate.Visible = False
        Select Case status
            Case CallForPaperStatus.Published
            Case CallForPaperStatus.SubmissionClosed

            Case CallForPaperStatus.SubmissionOpened
                Me.DVendDate.Visible = endDate.HasValue
                If endDate.HasValue Then
                    Me.RDPconfirmEndDay.MinDate = Now.Date
                    '  Me.CMVconfirmEndDay.ErrorMessage = String.Format(Resource.getValue("CMVconfirmEndDay.text"), endDate.Value.ToShortDateString & " " & endDate.Value.ToShortTimeString)
                End If

        End Select

        'NOTA: Il seguente codice è stato COMMENTATO e ricreato!
        'Nella presente funzione ha la sola funzione di creare un loop che mostra un pop-up con un testo fuorviante!
        'Per questo motivo è stato COMMENTATO per:
        '- Nascondere il tasto CONFERMA (che di fatto genera il loop ed è totalmente inutile)
        '- Assegnare un testo compatibile con quanto sta accadendo!

        If status = CallForPaperStatus.Published OrElse status = CallForPaperStatus.Draft Then
            Me.LBinfoConfirmStatus.Text = Resource.getValue("LBinfoConfirmStatus." & CallType.ToString & "." & status.ToString)
        Else
            Me.LBinfoConfirmStatus.Text = Resource.getValue("LBinfoConfirmStatus." & status.ToString)
        End If

        'Me.BTNconfirmStatus.Visible = False
        'Me.LBinfoConfirmStatus.Text = Resource.getValue("InvalidStatus." & & status.ToString)


        Me.OpenDialog()
    End Sub

    Public Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewEditCall.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Public Sub GoToUrl(action As lm.Comol.Modules.CallForPapers.Domain.CallStandardAction, url As String) Implements IViewEditCall.GoToUrl
        'Select Case action
        '    Case CallStandardAction.List
        PageUtility.RedirectToUrl(url)
        '   End Select
    End Sub
#End Region

#Region "Dialog"
    Private Sub OpenDialog()
        LTscriptOpen.Visible = True
        Me.DVconfirmStatus.Visible = True
        'Dim script As String = String.Format("showDialog('{0}')", dialogId)
        'ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub CloseDialog()
        LTscriptOpen.Visible = False
        Me.DVconfirmStatus.Visible = False
        'Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        'ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
#End Region

    Private Sub BTNconfirmStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconfirmStatus.Click
        Dim oDto As dtoBaseForPaper = Me.CurrentCall
        If Me.DVendDate.Visible Then
            If RDPconfirmEndDay.SelectedDate.HasValue AndAlso oDto.StartDate > RDPconfirmEndDay.SelectedDate.Value Then
                oDto.EndDate = oDto.StartDate.AddDays(1)
            Else
                oDto.EndDate = RDPconfirmEndDay.SelectedDate.Value
            End If
        End If
        Me.CloseDialog()
        Me.CurrentPresenter.SaveSettings(oDto, Resource.getValue("DefaultSubmitterName"), SystemSettings.Presenter.AllowDssUse, False)
        Me.CTRLprintSet.SaveSettings()
    End Sub
    Private Sub BTNundoConfirmStatus_Click(sender As Object, e As System.EventArgs) Handles BTNundoConfirmStatus.Click
        Me.CloseDialog()
        Me.DisplaySettingsError()
    End Sub
    Private Sub BTNsaveSettingsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveSettingsBottom.Click, BTNsaveSettingsTop.Click
        Me.CurrentPresenter.SaveSettings(Me.CurrentCall, Resource.getValue("DefaultSubmitterName"), SystemSettings.Presenter.AllowDssUse, True)

        Me.CTRLprintSet.SaveSettings()

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "Settings")

        Me.BindDati()
    End Sub
    Private Sub EditCallSettings_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

End Class