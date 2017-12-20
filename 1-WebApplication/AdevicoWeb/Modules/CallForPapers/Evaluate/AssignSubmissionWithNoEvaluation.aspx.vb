Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq

Public Class AssignSubmissionWithNoEvaluation
    Inherits PageBase
    Implements IViewAssignSubmissionWithNoEvaluation

#Region "Context"
    Private _Presenter As AssignSubmissionWithNoEvaluationPresenter
    Private ReadOnly Property CurrentPresenter() As AssignSubmissionWithNoEvaluationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AssignSubmissionWithNoEvaluationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewAssignSubmissionWithNoEvaluation.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewAssignSubmissionWithNoEvaluation.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewAssignSubmissionWithNoEvaluation.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewAssignSubmissionWithNoEvaluation.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property

#Region "common"
    Private Property IdCall As Long Implements IViewAssignSubmissionWithNoEvaluation.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewAssignSubmissionWithNoEvaluation.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewAssignSubmissionWithNoEvaluation.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewAssignSubmissionWithNoEvaluation.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            Me.BTNassignToSubmissionsWithNoEvaluationBottom.Visible = value
            Me.BTNassignToSubmissionsWithNoEvaluationTop.Visible = value
        End Set
    End Property
    Private Property MultiComittees As Boolean Implements IViewAssignSubmissionWithNoEvaluation.MultiComittees
        Get
            Return ViewStateOrDefault("MultiComittees", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("MultiComittees") = value
        End Set
    End Property
#End Region

#Region "Single"
    Private Property IdCommittee As Long Implements IViewAssignSubmissionWithNoEvaluation.IdCommittee
        Get
            Return ViewStateOrDefault("IdCommittee", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCommittee") = value
        End Set
    End Property
#End Region

#Region "Multiple"
    Private Property AvailableEvaluators As Dictionary(Of Long, Dictionary(Of Long, String)) Implements IViewAssignSubmissionWithNoEvaluation.AvailableEvaluators
        Get
            Return ViewStateOrDefault("AvailableEvaluators", New Dictionary(Of Long, Dictionary(Of Long, String)))
        End Get
        Set(value As Dictionary(Of Long, Dictionary(Of Long, String)))
            Me.ViewState("AvailableEvaluators") = value
        End Set
    End Property
    Private Property CurrentOrderBy As lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder Implements IViewAssignSubmissionWithNoEvaluation.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder.ByUser)
        End Get
        Set(ByVal value As lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewAssignSubmissionWithNoEvaluation.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CurrentAscending") = value
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

#Region "Internal"

    Public ReadOnly Property GetCommitteeCssClass(display As lm.Comol.Modules.CallForPapers.Domain.displayAs) As String
        Get
            Return IIf(display <> lm.Comol.Modules.CallForPapers.Domain.displayAs.item, display.ToString, "")
        End Get
    End Property
    Public ReadOnly Property DialogTitleTranslation As String
        Get
            Return Resource.getValue("DialogTitleTranslation.ConfirmSubmissionAssignments")
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
            LBnocalls.Text = Resource.getValue("LBnoCalls")

            .setHyperLink(HYPbackTop, True, True)
            .setHyperLink(HYPbackBottom, True, True)
            .setButton(BTNassignToSubmissionsWithNoEvaluationTop, True, , , True)
            .setButton(BTNassignToSubmissionsWithNoEvaluationBottom, True, , , True)

            .setButton(BTNcloseConfirmInEvaluationSettings, True, , , True)
            .setButton(BTNconfirmInEvaluationSettings, True, , , True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Common"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewAssignSubmissionWithNoEvaluation.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewAssignSubmissionWithNoEvaluation.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewAssignSubmissionWithNoEvaluation.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(PreloadIdCall, idCommunity, WizardEvaluationStep.AssignSubmissionWithNoEvaluation, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewAssignSubmissionWithNoEvaluation.SetActionUrl
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
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewAssignSubmissionWithNoEvaluation.SetContainerName
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

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewAssignSubmissionWithNoEvaluation.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Public Sub HideErrorMessages() Implements IViewAssignSubmissionWithNoEvaluation.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewAssignSubmissionWithNoEvaluation.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewAssignSubmissionWithNoEvaluation.DisplayWarning
        DisplayError(err, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewAssignSubmissionWithNoEvaluation.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub ReloadEditor(url As String) Implements IViewAssignSubmissionWithNoEvaluation.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Sub DisplayStartup(count As Long, approved As Long, rejected As Long) Implements IViewAssignSubmissionWithNoEvaluation.DisplayStartup
        If count = 0 Then
            LBdisplayStartupInfo.Text = Resource.getValue("DisplayStartup.InEvaluations.0") & "."
        Else
            If count = 1 Then
                LBdisplayStartupInfo.Text = Resource.getValue("DisplayStartup.InEvaluations.1")
            Else
                LBdisplayStartupInfo.Text = String.Format(Resource.getValue("DisplayStartup.InEvaluations"), count)
            End If
            If approved > 0 OrElse rejected > 0 Then
                LBdisplayStartupInfo.Text &= ": "
            End If
            Select Case approved
                Case 1
                    LBdisplayStartupInfo.Text &= Resource.getValue("DisplayStartup.InEvaluations.Approved.1")
                Case 0
                    Exit Select
                Case Else
                    LBdisplayStartupInfo.Text &= String.Format(Resource.getValue("DisplayStartup.InEvaluations.Approved.n"), approved)
            End Select
            If approved > 0 AndAlso rejected > 0 Then
                LBdisplayStartupInfo.Text &= ", "
            End If
            Select Case rejected
                Case 1
                    LBdisplayStartupInfo.Text &= Resource.getValue("DisplayStartup.InEvaluations.Rejected.1")
                Case 0
                    Exit Select
                Case Else
                    LBdisplayStartupInfo.Text &= String.Format(Resource.getValue("DisplayStartup.InEvaluations.Rejected.n"), rejected)
            End Select
            LBdisplayStartupInfo.Text &= "."
        End If
    End Sub
    Private Sub ConfirmSettings(items As List(Of dtoCommitteePartialAssignment)) Implements IViewAssignSubmissionWithNoEvaluation.ConfirmSettings
        LBdefaultConfirmDescription.Text = Resource.getValue("ConfirmSettings.MultipleAssignments")

        Me.RPTconfirmItems.DataSource = items
        Me.RPTconfirmItems.DataBind()

        Me.RPTconfirmItems.Visible = items.Count > 0
        Me.DVconfirmAction.Visible = True
    End Sub
#End Region

    Private Sub DisplayNoAvailableSubmission(count As Long, rejected As Long) Implements IViewAssignSubmissionWithNoEvaluation.DisplayNoAvailableSubmission
        'Me.MLVsettings.SetActiveView(VIWsettings)
        'MLVassignments.SetActiveView(VIWdisplayInfo)

        'If count > 1 AndAlso rejected > 1 Then
        '    LBdisplayInfo.Text = String.Format(Resource.getValue("NoAvailableSubmission"), count, rejected)
        'ElseIf (count = 1 AndAlso rejected = 0) OrElse (count = 0 AndAlso rejected = 1) OrElse (count = 1 AndAlso rejected = 1) Then
        '    LBdisplayInfo.Text = Resource.getValue("NoAvailableSubmission." & count.ToString & "." & rejected.ToString())
        'ElseIf count = 1 AndAlso rejected > 1 Then
        '    LBdisplayInfo.Text = String.Format(Resource.getValue("NoAvailableSubmission.1.n"), rejected)
        'ElseIf count > 1 AndAlso rejected = 1 Then
        '    LBdisplayInfo.Text = String.Format(Resource.getValue("NoAvailableSubmission.n.1"), count)
        'ElseIf count = 0 AndAlso rejected = 0 Then
        '    LBdisplayInfo.Text = Resource.getValue("NoAvailableSubmission.0.0")
        'End If
    End Sub

#Region "Single Assignments"
    Private Sub LoadSubmissions(assignments As List(Of dtoSubmissionAssignment), evaluators As Dictionary(Of Long, String)) Implements IViewAssignSubmissionWithNoEvaluation.LoadSubmissions
        Me.MLVsettings.SetActiveView(VIWsettings)
        MLVassignments.SetActiveView(VIWsingleCommitteeAssignments)
        Dim aEvaluators As Dictionary(Of Long, Dictionary(Of Long, String)) = Me.AvailableEvaluators
        If aEvaluators.ContainsKey(IdCommittee) Then
            Me.AvailableEvaluators(IdCommittee) = evaluators
        Else
            Me.AvailableEvaluators.Add(IdCommittee, evaluators)
        End If
        Me.RPTassignments.DataSource = assignments
        Me.RPTassignments.DataBind()
    End Sub
    Private Function GetAssignments() As List(Of dtoSubmissionAssignment) Implements IViewAssignSubmissionWithNoEvaluation.GetAssignments
        Dim items As New List(Of dtoSubmissionAssignment)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTassignments.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim item As New dtoSubmissionAssignment
            Dim oLiteral As Literal = row.FindControl("LTidSubmission")
            Dim oLabel As Label = row.FindControl("LBsubmitterName")
            item.IdSubmission = CLng(oLiteral.Text)
            oLiteral = row.FindControl("LTidSubmitterType")
            item.IdSubmitterType = CInt(oLiteral.Text)
            oLiteral = row.FindControl("LTsubmitterType")
            item.SubmitterType = oLiteral.Text
            item.DisplayName = oLabel.Text
           
            Dim oSelect As HtmlSelect = row.FindControl("SLBevaluators")
            item.Evaluators = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
            items.Add(item)
        Next
        Return items
    End Function
#End Region

#Region "Multiple Assignments"
    Private Sub LoadSubmissions(assignments As List(Of dtoSubmissionMultipleAssignment), evaluators As Dictionary(Of Long, Dictionary(Of Long, String))) Implements IViewAssignSubmissionWithNoEvaluation.LoadSubmissions
        Me.MLVsettings.SetActiveView(VIWsettings)
        MLVassignments.SetActiveView(VIWmultipleCommitteeAssignments)
        Me.AvailableEvaluators = evaluators
        Me.RPTsubmissions.DataSource = assignments
        Me.RPTsubmissions.DataBind()
    End Sub
    Private Function GetMultipleAssignment() As List(Of dtoSubmissionMultipleAssignment) Implements IViewAssignSubmissionWithNoEvaluation.GetMultipleAssignment
        Dim items As New List(Of dtoSubmissionMultipleAssignment)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsubmissions.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim item As New dtoSubmissionMultipleAssignment
            Dim oLiteral As Literal = row.FindControl("LTidSubmission")
            item.IdSubmission = CLng(oLiteral.Text)
            oLiteral = row.FindControl("LTidSubmitterType")
            item.IdSubmitterType = CInt(oLiteral.Text)
            oLiteral = row.FindControl("LTsubmitterType")
            item.SubmitterType = oLiteral.Text

            Dim oLabel As Label = row.FindControl("LBsubmitterName")
            item.DisplayName = oLabel.Text

            Dim oRepeater As Repeater = row.FindControl("RPTcommittees")
            For Each rowCommittee As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                Dim committee As New dtoCommitteeAssignment

                oLiteral = rowCommittee.FindControl("LTidCommittee")
                committee.IdCommittee = CLng(oLiteral.Text)
                committee.IdSubmission = item.IdSubmission
                oLiteral = rowCommittee.FindControl("LTcommitteeName")
                committee.Name = oLiteral.Text

                Dim oSelect As HtmlSelect = rowCommittee.FindControl("SLBevaluators")
                committee.Evaluators = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
                item.Committees.Add(committee)
            Next
            items.Add(item)
        Next
        Return items
    End Function
#End Region

#End Region

#Region "page"

#Region "Common"
    Private Sub BTNassignToSubmissionsWithNoEvaluationTop_Click(sender As Object, e As System.EventArgs) Handles BTNassignToSubmissionsWithNoEvaluationTop.Click, BTNassignToSubmissionsWithNoEvaluationBottom.Click
        If Me.MLVassignments.GetActiveView Is VIWmultipleCommitteeAssignments Then
            Me.CurrentPresenter.SaveSettings(GetMultipleAssignment())
        Else
            Me.CurrentPresenter.SaveSettings(GetAssignments())
        End If
    End Sub
    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Sub RPTconfirmItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTconfirmItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCommitteePartialAssignment = DirectCast(e.Item.DataItem, dtoCommitteePartialAssignment)
            Dim oLiteral As Literal = e.Item.FindControl("LTevaluators")
            Dim oCell As HtmlTableCell = e.Item.FindControl("TDcommitteeName")
            Me.Resource.setLiteral(oLiteral)
            oCell.Visible = MultiComittees
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTsubmittername_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmitterType_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTcommitteeNameHeader_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTevaluatorsHeader_t")
            Me.Resource.setLiteral(oLiteral)

            Dim oCell As HtmlTableCell = e.Item.FindControl("THcommittee")
            oCell.Visible = MultiComittees
        End If
    End Sub
    Private Sub BTNcloseConfirmInEvaluationSettings_Click(sender As Object, e As System.EventArgs) Handles BTNcloseConfirmInEvaluationSettings.Click
        Me.DVconfirmAction.Visible = False
    End Sub
    Private Sub BTNconfirmInEvaluationSettings_Click(sender As Object, e As System.EventArgs) Handles BTNconfirmInEvaluationSettings.Click
        Me.DVconfirmAction.Visible = False
        If MultiComittees Then
            Me.CurrentPresenter.ConfirmSettings(GetMultipleAssignment())
        Else
            Me.CurrentPresenter.ConfirmSettings(GetAssignments())
        End If
    End Sub
#End Region

#Region "Multiple"
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


            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBevaluators")
            oSelect.DataSource = AvailableEvaluators(item.IdCommittee)
            oSelect.DataTextField = "Value"
            oSelect.DataValueField = "Key"
            oSelect.DataBind()
            For Each idEvaluator As Long In item.Evaluators
                Dim oListItem As ListItem = oSelect.Items.FindByValue(idEvaluator)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
            oSelect.Attributes.Add("data-placeholder", Resource.getValue("Evaluators.data-placeholder"))
            oSelect.Disabled = Not AllowSave
            Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNsubmissionEvaluatorsSelectAll")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmissionEvaluatorsSelectAll.ToolTip"))
            oHtmlControl.Visible = AllowSave
            oHtmlControl = e.Item.FindControl("SPNsubmissionEvaluatorsSelectNone")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmissionEvaluatorsSelectNone.ToolTip"))
            oHtmlControl.Visible = AllowSave
        End If
    End Sub
#End Region

#Region "Single"
    Private Sub RPTassignments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTassignments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoSubmissionAssignment = DirectCast(e.Item.DataItem, dtoSubmissionAssignment)


            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBevaluators")
            oSelect.DataSource = AvailableEvaluators(IdCommittee)
            oSelect.DataTextField = "Value"
            oSelect.DataValueField = "Key"
            oSelect.DataBind()
            For Each idCommittee As Long In item.Evaluators
                Dim oListItem As ListItem = oSelect.Items.FindByValue(idCommittee)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
            oSelect.Attributes.Add("data-placeholder", Resource.getValue("Evaluators.data-placeholder"))
            oSelect.Disabled = Not AllowSave
            Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNsubmissionEvaluatorsSelectAll")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmissionEvaluatorsSelectAll.ToolTip"))
            oHtmlControl.Visible = AllowSave
            oHtmlControl = e.Item.FindControl("SPNsubmissionEvaluatorsSelectNone")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmissionEvaluatorsSelectNone.ToolTip"))
            oHtmlControl.Visible = AllowSave


        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTsubmittername_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTsubmitterType_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTevaluators_t")
            Me.Resource.setLiteral(oLiteral)
        End If
    End Sub
#End Region

#End Region

End Class