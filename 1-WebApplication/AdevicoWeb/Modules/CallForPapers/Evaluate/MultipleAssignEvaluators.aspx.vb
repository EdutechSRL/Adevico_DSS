Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq

Public Class MultipleAssignEvaluators
    Inherits PageBase
    Implements IViewCommitteesSubmissionAssignments

#Region "Context"
    Private _Presenter As CommitteesEvaluationAssignmentsPresenter
    Private ReadOnly Property CurrentPresenter() As CommitteesEvaluationAssignmentsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommitteesEvaluationAssignmentsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewCommitteesSubmissionAssignments.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewCommitteesSubmissionAssignments.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewCommitteesSubmissionAssignments.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewCommitteesSubmissionAssignments.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property

    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewCommitteesSubmissionAssignments.PreloadPageIndex
        Get
            If IsNumeric(Me.Request.QueryString("PageIndex")) Then
                Return CInt(Me.Request.QueryString("PageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewCommitteesSubmissionAssignments.PreloadPageSize
        Get
            If IsNumeric(Me.Request.QueryString("PageSize")) Then
                Return CInt(Me.Request.QueryString("PageSize"))
            Else
                Return 50
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadOrderBy As lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder Implements IViewCommitteesSubmissionAssignments.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder.ByType)
        End Get
    End Property
    'Private ReadOnly Property PreloadFilterBy As lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus Implements IViewCommitteesSubmissionAssignments.PreloadFilterBy
    '    Get
    '        Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus).GetByString(Request.QueryString("Filter"), lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus.OnlySubmitted)
    '    End Get
    'End Property
    'Private ReadOnly Property PreloadSearchForName As String Implements IViewCommitteesSubmissionAssignments.PreloadSearchForName
    '    Get
    '        Return Request.QueryString("SearchForName")
    '    End Get
    'End Property
    Private ReadOnly Property PreloadFilters As lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionFilters Implements IViewCommitteesSubmissionAssignments.PreloadFilters
        Get
            Dim dto As New lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionFilters
            With dto
                .Ascending = PreloadAscending
                .CallType = lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids
                .OrderBy = PreloadOrderBy
                .SearchForName = "" 'PreloadSearchForName
                .Status = lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus.Accepted 'PreloadFilterBy

                .TranslationsRevision = New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.RevisionStatus, String)

                Dim types As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionStatus, String)
                'For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionStatus))
                '    types.Add([Enum].Parse(GetType(SubmissionStatus), name), Me.Resource.getValue("SubmissionStatus." & name))
                'Next
                .TranslationsSubmission = types
            End With
            Return dto
        End Get
    End Property

    Private ReadOnly Property PreloadAscending As Boolean Implements IViewCommitteesSubmissionAssignments.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return False
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewCommitteesSubmissionAssignments.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridTop.Pager = value
            Me.PGgridBottom.Pager = value
            Me.DVpagerTop.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property
    Private Property CurrentFilters As lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionFilters Implements IViewCommitteesSubmissionAssignments.CurrentFilters
        Get
            Dim filter As lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionFilters = ViewStateOrDefault("CurrentFilters", PreloadFilters)
            filter.SearchForName = "" ' Me.TXBusername.Text
            filter.Status = lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus.Accepted
            'If Me.RBLstatus.SelectedIndex = -1 Then
            '    filter.Status = SubmissionFilterStatus.OnlySubmitted
            'Else
            '    filter.Status = Me.RBLstatus.SelectedValue
            'End If
            Return filter
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.dtoSubmissionFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
    Private Property CurrentFilterBy As lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus Implements IViewCommitteesSubmissionAssignments.CurrentFilterBy
        Get
            Return ViewStateOrDefault("CurrentFilterBy", lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus.Accepted)
        End Get
        Set(ByVal value As lm.Comol.Modules.CallForPapers.Domain.SubmissionFilterStatus)
            Me.ViewState("CurrentFilterBy") = value
        End Set
    End Property
    Private Property CurrentOrderBy As lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder Implements IViewCommitteesSubmissionAssignments.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder.ByUser)
        End Get
        Set(ByVal value As lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewCommitteesSubmissionAssignments.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property PageSize As Integer Implements IViewCommitteesSubmissionAssignments.PageSize
        Get
            Return ViewStateOrDefault("PageSize", 50)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("PageSize") = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewCommitteesSubmissionAssignments.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            Me.BTNsaveMultipleCommitteeAssignmentsBottom.Visible = value
            Me.BTNsaveMultipleCommitteeAssignmentsTop.Visible = value
        End Set
    End Property
    Private Property AllowAssignEvaluatorsToAll As Boolean Implements IViewCommitteesSubmissionAssignments.AllowAssignEvaluatorsToAll
        Get
            Return ViewStateOrDefault("AllowAssignEvaluatorsToAll", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowAssignEvaluatorsToAll") = value
            Me.BTNassignEvaluatorsToAllSubmissionBottom.Visible = value
            Me.BTNassignEvaluatorsToAllSubmissionTop.Visible = value
        End Set
    End Property
    Private Property AllowChangeAssignModeToEvalutors As Boolean Implements IViewCommitteesSubmissionAssignments.AllowChangeAssignModeToEvalutors
        Get
            Return ViewStateOrDefault("AllowChangeAssignModeToEvalutors", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowChangeAssignModeToEvalutors") = value
            RBLmultipleAssignmentStartup.Enabled = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewCommitteesSubmissionAssignments.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewCommitteesSubmissionAssignments.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewCommitteesSubmissionAssignments.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property AvailableEvaluators As Dictionary(Of Long, Dictionary(Of Long, String)) Implements IViewCommitteesSubmissionAssignments.AvailableEvaluators
        Get
            Return ViewStateOrDefault("AvailableEvaluators", New Dictionary(Of Long, Dictionary(Of Long, String)))
        End Get
        Set(value As Dictionary(Of Long, Dictionary(Of Long, String)))
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

#Region "Internal"
    Public ReadOnly Property GetCommitteeCssClass(display As lm.Comol.Modules.CallForPapers.Domain.displayAs) As String
        Get
            Return IIf(display <> lm.Comol.Modules.CallForPapers.Domain.displayAs.item, display.ToString, "")
        End Get
    End Property
#End Region

    Private Sub EditEvaluationCommittees_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
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
            .setButton(BTNsaveMultipleCommitteeAssignmentsBottom, True, , , True)
            .setButton(BTNsaveMultipleCommitteeAssignmentsTop, True, , , True)
            .setButton(BTNassignEvaluatorsToAllSubmissionBottom, True, , , True)
            .setButton(BTNassignEvaluatorsToAllSubmissionTop, True, , , True)
            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setRadioButtonList(RBLmultipleAssignmentStartup, "True")
            .setRadioButtonList(RBLmultipleAssignmentStartup, "False")
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewCommitteesSubmissionAssignments.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewCommitteesSubmissionAssignments.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewCommitteesSubmissionAssignments.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(PreloadIdCall, idCommunity, WizardEvaluationStep.MultipleAssignSubmission, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewCommitteesSubmissionAssignments.SetActionUrl
        Me.HYPbackBottom.Visible = True
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackTop.Visible = True
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub SetContainerName(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.MultipleAssignSubmission.ToString())
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewCommitteesSubmissionAssignments.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.MultipleAssignSubmission.ToString())
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

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewCommitteesSubmissionAssignments.LoadUnknowCall
        MLVsettings.SetActiveView(VIWempty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("LBnoCalls." & type.ToString), Helpers.MessageType.error)
    End Sub
    Public Sub HideErrorMessages() Implements IViewCommitteesSubmissionAssignments.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewCommitteesSubmissionAssignments.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewCommitteesSubmissionAssignments.DisplayWarning
        DisplayError(err, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewCommitteesSubmissionAssignments.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayNoAvailableSubmission(count As Long, rejected As Long) Implements IViewCommitteesSubmissionAssignments.DisplayNoAvailableSubmission
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
    Private Sub DisplayStartup(count As Long, approved As Long, rejected As Long) Implements IViewCommitteesSubmissionAssignments.DisplayStartup
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
        RBLmultipleAssignmentStartup.SelectedIndex = 0
    End Sub
    Private Sub LoadSubmissions(assignments As List(Of dtoSubmissionMultipleAssignment), evaluators As Dictionary(Of Long, Dictionary(Of Long, String))) Implements IViewCommitteesSubmissionAssignments.LoadSubmissions
        Me.MLVsettings.SetActiveView(VIWsettings)
        MLVassignments.SetActiveView(VIWassignments)
        Me.AvailableEvaluators = evaluators
        RPTsubmissions.DataSource = assignments
        RPTsubmissions.DataBind()
    End Sub
    Private Function GetAssignments() As List(Of dtoSubmissionMultipleAssignment) Implements IViewCommitteesSubmissionAssignments.GetAssignments
        Dim items As New List(Of dtoSubmissionMultipleAssignment)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsubmissions.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim item As New dtoSubmissionMultipleAssignment
            Dim oLiteral As Literal = row.FindControl("LTidSubmission")
            item.IdSubmission = CLng(oLiteral.Text)
            oLiteral = row.FindControl("LTidSubmitterType")
            item.IdSubmitterType = CInt(oLiteral.Text)

            Dim oRepeater As Repeater = row.FindControl("RPTcommittees")
            For Each rowCommittee As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                Dim committee As New dtoCommitteeAssignment

                oLiteral = rowCommittee.FindControl("LTidCommittee")
                committee.IdCommittee = CLng(oLiteral.Text)
                committee.IdSubmission = item.IdSubmission
                Dim oSelect As HtmlSelect = rowCommittee.FindControl("SLBevaluators")
                committee.Evaluators = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
                item.Committees.Add(committee)
            Next
            items.Add(item)
        Next
        Return items
    End Function

    Private Sub ReloadEditor(url As String) Implements IViewCommitteesSubmissionAssignments.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
    'Private Sub InitializeAddFieldControl(idCall As Long) Implements IViewCallEditor.InitializeAddFieldControl
    '    Me.CTRLaddField.InitializeControl(idCall)
    'End Sub
#End Region

    Private Sub RPTsubmissions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubmissions.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTsubmittername_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmitterType_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTevaluators_t")
            Me.Resource.setLiteral(oLiteral)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oLiteral As Literal = e.Item.FindControl("LTcommittee_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTevaluators_t")
            Me.Resource.setLiteral(oLiteral)
        End If
    End Sub
    Protected Sub RPTcommittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCommitteeAssignment = DirectCast(e.Item.DataItem, dtoCommitteeAssignment)
            Dim allowEdit As Boolean = Not AllowSave AndAlso Not AllowChangeAssignModeToEvalutors

            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBevaluators")
            oSelect.DataSource = AvailableEvaluators(item.IdCommittee)
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
        End If
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Me.CurrentPresenter.LoadAssignments(IdCall, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        Me.CurrentPresenter.LoadAssignments(IdCall, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub BTNsaveMultipleCommitteeAssignmentsTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveMultipleCommitteeAssignmentsBottom.Click, BTNsaveMultipleCommitteeAssignmentsTop.Click
        If Me.MLVassignments.GetActiveView Is VIWstartup Then
            Me.CurrentPresenter.CreateAssignments(CBool(Me.RBLmultipleAssignmentStartup.SelectedValue), CurrentFilters)
        Else
            Me.CurrentPresenter.SaveSettings(GetAssignments(), CurrentFilters, Pager.PageIndex, Pager.PageSize)
        End If
    End Sub
    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub


    Private Sub BTNassignEvaluatorsToAllSubmissionBottom_Click(sender As Object, e As System.EventArgs) Handles BTNassignEvaluatorsToAllSubmissionBottom.Click, BTNassignEvaluatorsToAllSubmissionTop.Click
        Me.CurrentPresenter.SetEvaluatorsToAllSubmissions(CurrentFilters, Pager.PageIndex, Pager.PageSize)
    End Sub
End Class