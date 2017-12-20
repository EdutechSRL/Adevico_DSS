Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Public Class SingleAssignEvaluators
    Inherits PageBase
    Implements IViewCommitteeSubmissionAssignments

#Region "Context"
    Private _Presenter As CommitteeSubmissionAssignmentsPresenter
    Private ReadOnly Property CurrentPresenter() As CommitteeSubmissionAssignmentsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommitteeSubmissionAssignmentsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewCommitteeSubmissionAssignments.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewCommitteeSubmissionAssignments.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewCommitteeSubmissionAssignments.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewCommitteeSubmissionAssignments.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewCommitteeSubmissionAssignments.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            Me.BTNsaveSingleAssignmentsBottom.Visible = value
            Me.BTNsaveSingleAssignmentsTop.Visible = value
        End Set
    End Property
    Private Property AllowAssignEvaluatorsToAll As Boolean Implements IViewCommitteeSubmissionAssignments.AllowAssignEvaluatorsToAll
        Get
            Return ViewStateOrDefault("AllowAssignEvaluatorsToAll", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowAssignEvaluatorsToAll") = value
            Me.BTNassignEvaluatorsToAllSubmissionBottom.Visible = value
            Me.BTNassignEvaluatorsToAllSubmissionTop.Visible = value
        End Set
    End Property
    Private Property AllowChangeAssignModeToEvalutors As Boolean Implements IViewCommitteeSubmissionAssignments.AllowChangeAssignModeToEvalutors
        Get
            Return ViewStateOrDefault("AllowChangeAssignModeToEvalutors", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowChangeAssignModeToEvalutors") = value
            RBLsingleAssignmentStartup.Enabled = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewCommitteeSubmissionAssignments.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewCommitteeSubmissionAssignments.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewCommitteeSubmissionAssignments.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdCommittee As Long Implements IViewCommitteeSubmissionAssignments.IdCommittee
        Get
            Return ViewStateOrDefault("IdCommittee", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCommittee") = value
        End Set
    End Property
    Private Property AvailableEvaluators As Dictionary(Of Long, String) Implements IViewCommitteeSubmissionAssignments.AvailableEvaluators
        Get
            Return ViewStateOrDefault("AvailableEvaluators", New Dictionary(Of Long, String))
        End Get
        Set(value As Dictionary(Of Long, String))
            Me.ViewState("AvailableEvaluators") = value
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
#End Region

    Private Sub EditEvaluationCommittees_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
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
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls"), Helpers.MessageType.error)

            .setHyperLink(HYPbackTop, True, True)
            .setHyperLink(HYPbackBottom, True, True)
            .setButton(BTNsaveSingleAssignmentsBottom, True, , , True)
            .setButton(BTNsaveSingleAssignmentsTop, True, , , True)
            .setButton(BTNassignEvaluatorsToAllSubmissionBottom, True, , , True)
            .setButton(BTNassignEvaluatorsToAllSubmissionTop, True, , , True)

            .setRadioButtonList(RBLsingleAssignmentStartup, "True")
            .setRadioButtonList(RBLsingleAssignmentStartup, "False")
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewCommitteeSubmissionAssignments.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewCommitteeSubmissionAssignments.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewCommitteeSubmissionAssignments.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(PreloadIdCall, idCommunity, WizardEvaluationStep.AssignSubmission, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewCommitteeSubmissionAssignments.SetActionUrl
        Me.HYPbackBottom.Visible = True
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackTop.Visible = True
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub SetContainerName(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.AssignSubmission.ToString())
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewCommitteeSubmissionAssignments.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.AssignSubmission.ToString())
        Master.ServiceTitle = title
        If String.IsNullOrEmpty(communityName) Then
            Master.ServiceTitleToolTip = title & " - " & callName
        Else
            Master.ServiceTitleToolTip = title & " - " & callName
        End If
    End Sub

    Private Sub LoadWizardSteps(idCall As Long, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep))) Implements IViewBaseEditEvaluationSettings.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, idCommunity, PreloadView, steps)
    End Sub
    Private Sub LoadWizardSteps(idCall As Long, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep)), err As EvaluationEditorErrors) Implements IViewBaseEditEvaluationSettings.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, idCommunity, PreloadView, steps, err)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewCommitteeSubmissionAssignments.LoadUnknowCall
        MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Public Sub HideErrorMessages() Implements IViewCommitteeSubmissionAssignments.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewCommitteeSubmissionAssignments.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewCommitteeSubmissionAssignments.DisplayWarning
        DisplayError(err, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewCommitteeSubmissionAssignments.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayNoAvailableSubmission(count As Long, rejected As Long) Implements IViewCommitteeSubmissionAssignments.DisplayNoAvailableSubmission
        Me.MLVsettings.SetActiveView(VIWsettings)
        MLVassignments.SetActiveView(VIWdisplayInfo)

        If count > 1 AndAlso rejected > 1 Then
            LBdisplayInfo.Text = String.Format(Resource.getValue("NoAvailableSubmission"), count, rejected)
        ElseIf (count = 1 AndAlso rejected = 0) OrElse (count = 0 AndAlso rejected = 1) OrElse (count = 1 AndAlso rejected = 1) Then
            LBdisplayInfo.Text = Resource.getValue("NoAvailableSubmission." & count.ToString & "." & rejected.ToString())
        ElseIf count = 1 AndAlso rejected > 1 Then
            LBdisplayInfo.Text = String.Format(Resource.getValue("NoAvailableSubmission.1.n"), rejected)
        ElseIf count > 1 AndAlso rejected = 1 Then
            LBdisplayInfo.Text = String.Format(Resource.getValue("NoAvailableSubmission.n.1"), count)
        ElseIf count = 0 AndAlso rejected = 0 Then
            LBdisplayInfo.Text = Resource.getValue("NoAvailableSubmission.0.0")
        End If
    End Sub
    Private Sub DisplayStartup(count As Long, approved As Long, rejected As Long) Implements IViewCommitteeSubmissionAssignments.DisplayStartup
        Me.MLVsettings.SetActiveView(VIWsettings)
        MLVassignments.SetActiveView(VIWstartup)

        If count = 0 Then
            LBdisplayStartupInfo.Text = Resource.getValue("DisplayStartup.0") & "."
        Else
            If count = 1 Then
                LBdisplayStartupInfo.Text = Resource.getValue("DisplayStartup.1")
            Else
                LBdisplayStartupInfo.Text = String.Format(Resource.getValue("DisplayStartup"), count)
            End If
            If approved > 0 OrElse rejected > 0 Then
                LBdisplayStartupInfo.Text &= ": "
            End If
            Select Case approved
                Case 1
                    LBdisplayStartupInfo.Text &= Resource.getValue("DisplayStartup.Approved.1")
                Case 0
                    Exit Select
                Case Else
                    LBdisplayStartupInfo.Text &= String.Format(Resource.getValue("DisplayStartup.Approved.n"), approved)
            End Select
            If approved > 0 AndAlso rejected > 0 Then
                LBdisplayStartupInfo.Text &= ", "
            End If
            Select Case rejected
                Case 1
                    LBdisplayStartupInfo.Text &= Resource.getValue("DisplayStartup.Rejected.1")
                Case 0
                    Exit Select
                Case Else
                    LBdisplayStartupInfo.Text &= String.Format(Resource.getValue("DisplayStartup.Rejected.n"), rejected)
            End Select
            LBdisplayStartupInfo.Text &= "."
            LBdisplayInfoAction.Text = Resource.getValue("DisplayStartup.End")
        End If
        Me.RBLsingleAssignmentStartup.SelectedIndex = 0
    End Sub
    Private Sub LoadSubmissions(assignments As List(Of dtoSubmissionAssignment), evaluators As Dictionary(Of Long, String)) Implements IViewCommitteeSubmissionAssignments.LoadSubmissions
        Me.MLVsettings.SetActiveView(VIWsettings)
        MLVassignments.SetActiveView(VIWassignments)
        Me.AvailableEvaluators = evaluators
        Me.RPTassignments.DataSource = assignments
        Me.RPTassignments.DataBind()
    End Sub
    Private Function GetAssignments() As List(Of dtoSubmissionAssignment) Implements IViewCommitteeSubmissionAssignments.GetAssignments
        Dim items As New List(Of dtoSubmissionAssignment)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTassignments.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim item As New dtoSubmissionAssignment
            Dim oLiteral As Literal = row.FindControl("LTidSubmission")
            item.IdSubmission = CLng(oLiteral.Text)
            oLiteral = row.FindControl("LTidSubmitterType")
            item.IdSubmitterType = CInt(oLiteral.Text)

            Dim oSelect As HtmlSelect = row.FindControl("SLBevaluators")
            item.Evaluators = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
            items.Add(item)
        Next
        Return items
    End Function

    Private Sub ReloadEditor(url As String) Implements IViewCommitteeSubmissionAssignments.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
    'Private Sub InitializeAddFieldControl(idCall As Long) Implements IViewCallEditor.InitializeAddFieldControl
    '    Me.CTRLaddField.InitializeControl(idCall)
    'End Sub
#End Region

    Private Sub RPTassignments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTassignments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoSubmissionAssignment = DirectCast(e.Item.DataItem, dtoSubmissionAssignment)
            Dim allowEdit As Boolean = Not AllowSave AndAlso Not AllowChangeAssignModeToEvalutors

            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBevaluators")
            oSelect.DataSource = AvailableEvaluators
            oSelect.DataTextField = "Value"
            oSelect.DataValueField = "Key"
            oSelect.DataBind()

            If AllowChangeAssignModeToEvalutors Then
                For Each idEvaluator As Long In item.Evaluators
                    Dim oListItem As ListItem = oSelect.Items.FindByValue(idEvaluator)
                    If Not IsNothing(oListItem) Then
                        oListItem.Selected = True
                    End If
                Next
            Else
                For Each oListItem As ListItem In oSelect.Items
                    oListItem.Selected = True
                Next
            End If

            oSelect.Attributes.Add("data-placeholder", Resource.getValue("Evaluators.data-placeholder"))
            oSelect.Disabled = Not allowEdit
            Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNsubmissionEvaluatorsSelectAll")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmissionEvaluatorsSelectAll.ToolTip"))
            oHtmlControl.Visible = allowEdit
            oHtmlControl = e.Item.FindControl("SPNsubmissionEvaluatorsSelectNone")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmissionEvaluatorsSelectNone.ToolTip"))
            oHtmlControl.Visible = allowEdit


        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTsubmittername_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmitterType_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTevaluators_t")
            Me.Resource.setLiteral(oLiteral)
        End If
    End Sub
    Private Sub BTNsaveSingleAssignmentsTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveSingleAssignmentsBottom.Click, BTNsaveSingleAssignmentsTop.Click
        If Me.MLVassignments.GetActiveView Is VIWstartup Then
            Me.CurrentPresenter.CreateAssignments(CBool(Me.RBLsingleAssignmentStartup.SelectedValue))
        Else
            Me.CurrentPresenter.SaveSettings(GetAssignments())
        End If
    End Sub
    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub


   
    Private Sub BTNassignEvaluatorsToAllSubmissionBottom_Click(sender As Object, e As System.EventArgs) Handles BTNassignEvaluatorsToAllSubmissionBottom.Click, BTNassignEvaluatorsToAllSubmissionTop.Click
        Me.CurrentPresenter.SetEvaluatorsToAllSubmissions()
    End Sub
End Class