Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.ActionDataContract
Imports System.Linq


Public Class cpAdvEvalDetail
    Inherits PageBase
    Implements IViewEvaluationSummary

#Region "Context"
    Private _Presenter As ViewEvaluationSummaryPresenter
    Private ReadOnly Property CurrentPresenter() As ViewEvaluationSummaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewEvaluationSummaryPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Private minValue As Integer = 0
#End Region

#Region "Implements"

    Public ReadOnly Property AdvCommissionId As Long Implements IViewEvaluationSummary.AdvCommissionId
        Get
            Return Request.QueryString("acId")
        End Get
    End Property


#Region "Settings Preload"
    Private ReadOnly Property Portalname As String Implements IViewEvaluationSummary.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdSubmission As Long Implements IViewEvaluationSummary.PreloadIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewEvaluationSummary.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewEvaluationSummary.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region "Settings"
    Private Property IdCall As Long Implements IViewEvaluationSummary.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewEvaluationSummary.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewEvaluationSummary.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdSubmission As Long Implements IViewEvaluationSummary.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdRevision As Long Implements IViewEvaluationSummary.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdRevision") = value
        End Set
    End Property
#End Region

    Private Property IdCurrentCommittee As Long Implements IViewEvaluationSummary.IdCurrentCommittee
        Get
            'If Me.DDLcommittees.SelectedIndex < 0 Then
            '    Return 0
            'Else
            '    Return CLng(Me.DDLcommittees.SelectedValue)
            'End If
            Return Me.AdvCommissionId
        End Get
        Set(ByVal value As Long)
            'If Not IsNothing(Me.DDLcommittees.Items.FindByValue(value)) Then
            '    Me.DDLcommittees.SelectedValue = value
            'End If
        End Set
    End Property
    Private Property CommitteesCount As Integer Implements IViewEvaluationSummary.CommitteesCount
        Get
            Return ViewStateOrDefault("CommitteesCount", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CommitteesCount") = value
        End Set
    End Property
    Private Property AllowHideComments As Boolean Implements IViewEvaluationSummary.AllowHideComments
        Get
            Return ViewStateOrDefault("AllowHideComments", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowHideComments") = value
            HYPcloseComments.Visible = value
        End Set
    End Property
    Private Property AllowExportAll As Boolean Implements IViewEvaluationSummary.AllowExportAll
        Get
            Return ViewStateOrDefault("AllowExportAll", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportAll") = value
            LNBexportCommitteesEvaluationsToCsv.Visible = value
            LNBexportCommitteesEvaluationsOneColumnToCsv.Visible = value
        End Set
    End Property
    Private Property AllowExportCurrent As Boolean Implements IViewEvaluationSummary.AllowExportCurrent
        Get
            Return ViewStateOrDefault("AllowExportCurrent", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportCurrent") = value
            LNBexportCommitteeEvaluationsToCsv.Visible = value
            LNBexportCommitteeEvaluationsOneColumnToCsv.Visible = value
        End Set
    End Property
    Private ReadOnly Property AnonymousDisplayname As String Implements IViewEvaluationSummary.AnonymousDisplayName
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewEvaluationSummary.UnknownDisplayname
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property
    Private Property CurrentEvaluationType As EvaluationType Implements IViewEvaluationSummary.CurrentEvaluationType
        Get
            Return ViewStateOrDefault("CurrentEvaluationType", EvaluationType.Sum)
        End Get
        Set(ByVal value As EvaluationType)
            Me.ViewState("CurrentEvaluationType") = value
        End Set
    End Property
    Private Property CallUseFuzzy As Boolean Implements IViewEvaluationSummary.CallUseFuzzy
        Get
            Return ViewStateOrDefault("CallUseFuzzy", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CallUseFuzzy") = value
        End Set
    End Property
    Private Property CommitteeIsFuzzy As Dictionary(Of Long, Boolean) Implements IViewEvaluationSummary.CommitteeIsFuzzy
        Get
            Return ViewStateOrDefault("CommitteeIsFuzzy", New Dictionary(Of Long, Boolean))
        End Get
        Set(ByVal value As Dictionary(Of Long, Boolean))
            Me.ViewState("CommitteeIsFuzzy") = value
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

#Region "internal"
    Protected ReadOnly Property CssCriterion(ByVal type As displayAs) As String
        Get
            Select Case type
                Case displayAs.item
                    Return ""
                Case displayAs.first
                    Return "first"
                Case displayAs.last
                    Return "last"
                Case Else
                    Return "first last"
            End Select
        End Get
    End Property
    Protected ReadOnly Property CssTableEvaluatorsCount(ByVal item As dtoSubmissionCommitteeItem) As String
        Get
            Return IIf(item.Evaluators.Count > 5, "", "noslide eval" & item.Evaluators.Count.ToString)
        End Get
    End Property
    Protected ReadOnly Property CookieName As String
        Get
            Return "EvaluationSummary_" & IdSubmission
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayEvaluationSummaryToken.Message")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayEvaluationSummaryToken.Title")
        End Get
    End Property


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsView,
                Me.IdCurrentCommittee,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                String.Format("SubmissionId:{0}", Me.IdSubmission))
        End If

        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsView,
                Me.IdCurrentCommittee,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                String.Format("NoPermission;SubmissionId:{0}", Me.IdSubmission))
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Master.ServiceTitle = .getValue("serviceTitle.EvaluationSummary")
            Master.ServiceNopermission = .getValue("nopermission")

            .setLiteral(LTsubmitterEvaluations_t)
            .setLabel_To_Value(LBcallName_t, "LBcallName_t." & CallForPaperType.CallForBids.ToString)
            .setLabel(LBowner_t)
            .setLabel(LBsubmittedOn_t)
            .setLabel(LBsubmittedBy_t)
            .setLiteral(LTdisplayEvaluationsStatus_t)
            .setLabel(LBcommitteeEvaluation)
            '.setLabel(LBcommitteesList_t)

            .setHyperLink(HYPcloseComments, False, True)
            .setHyperLink(HYPviewExtendedEvaluation, False, True)
            .setHyperLink(HYPviewSubmission, False, True)
            .setLinkButton(LNBexportCommitteeEvaluationsToCsv, False, True)
            .setLinkButton(LNBexportCommitteesEvaluationsToCsv, False, True)
            .setLinkButton(LNBexportCommitteeEvaluationsOneColumnToCsv, False, True)
            .setLinkButton(LNBexportCommitteesEvaluationsOneColumnToCsv, False, True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayDssWarning(lastUpdate As Date?, isCompleted As Boolean) Implements IViewEvaluationSummary.DisplayDssWarning
        CTRLdssMessage.Visible = True
        If lastUpdate = Nothing Then
            CTRLdssMessage.InitializeControl(Resource.getValue("DisplayDssWarning.NoDate"), Helpers.MessageType.alert)
        Else
            CTRLdssMessage.InitializeControl(String.Format(Resource.getValue("DisplayDssWarning.Date.isCompleted." & isCompleted.ToString), lastUpdate.Value.ToString()), IIf(isCompleted, Helpers.MessageType.success, Helpers.MessageType.alert))
        End If
    End Sub
    Private Sub HideDssWarning() Implements IViewEvaluationSummary.HideDssWarning
        CTRLdssMessage.Visible = False
    End Sub
    Private Sub DisplayNoEvaluationsFound() Implements IViewEvaluationSummary.DisplayNoEvaluationsFound
        Me.DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayNoEvaluationsFound")
    End Sub
    Public Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewEvaluationSummary.DisplayUnknownCall
        Me.DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayUnknownCall." & type.ToString)
        Select Case type
            Case CallForPaperType.CallForBids
                SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewEvaluationsSummary)
            Case CallForPaperType.RequestForMembership
                SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewEvaluationsSummary)
        End Select
    End Sub
    Private Sub DisplayEvaluationUnavailable1() Implements IViewEvaluationSummary.DisplayEvaluationUnavailable
        Me.DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("EvaluationSummary.DisplayEvaluationUnavailable")
    End Sub
    Private Sub DisplayNoPermissionToView() Implements IViewEvaluationSummary.DisplayNoPermissionToView
        Me.MLVstatistics.SetActiveView(VIWempty)
        LBnoSubmission.Text = Resource.getValue("DisplayNoEvaluationsToView")
    End Sub
    Private Sub DisplayUnknownSubmission(idCommunity As Integer, idModule As Integer, idSubmission As Long, type As CallForPaperType) Implements IViewEvaluationSummary.DisplayUnknownSubmission
        Me.MLVstatistics.SetActiveView(VIWempty)
        LBnoSubmission.Text = Resource.getValue("DisplayUnknownSubmission")

        Select Case type
            Case CallForPaperType.CallForBids
                SendUserAction(idCommunity, idModule, IdCall, idSubmission, ModuleCallForPaper.ActionType.ViewUnknowSubmission)
            Case CallForPaperType.RequestForMembership
                SendUserAction(idCommunity, idModule, IdCall, idSubmission, ModuleCallForPaper.ActionType.ViewUnknowSubmission)
        End Select

        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.ViewUnknowSubmission, , InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewEvaluationSummary.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.ViewSubmissionTableEvaluations(PreloadIdSubmission, PreloadIdCall, PreloadIdCommunity))
    End Sub

    Private Sub DisplaySessionTimeout(ByVal url As String) Implements IViewEvaluationSummary.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow
        dto.DestinationUrl = url
        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
                lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsView,
                Me.IdCurrentCommittee,
                lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                String.Format("SessionTimeOut;SubmissionId:{0}", Me.IdSubmission))

        webPost.Redirect(dto)
    End Sub

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewEvaluationSummary.DisplayNoPermission
        DVexport.Visible = False
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

#End Region

#Region "Actions"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, idSubmission As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewEvaluationSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.UserSubmission, idSubmission.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, idSubmission As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType) Implements IViewEvaluationSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType.UserSubmission, idSubmission.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleCallForPaper.ActionType) Implements IViewEvaluationSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRequestForMembership.ActionType) Implements IViewEvaluationSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewEvaluationSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType) Implements IViewEvaluationSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub

#End Region

    Private Sub SetViewEvaluationsUrl(url As String) Implements IViewEvaluationSummary.SetViewEvaluationsUrl
        'HYPviewExtendedEvaluation.Visible = Not String.IsNullOrEmpty(url)
        'HYPviewExtendedEvaluation.NavigateUrl = ApplicationUrlBase & url
    End Sub

    Private Sub SetViewSubmissionUrl(url As String) Implements IViewEvaluationSummary.SetViewSubmissionUrl
        HYPviewSubmission.Visible = Not String.IsNullOrEmpty(url)
        HYPviewSubmission.NavigateUrl = ApplicationUrlBase & url
    End Sub

    Private _canConfirmEvaluation As Boolean = True
    Private _isPresident As Boolean = True

    Private Sub LoadSubmissionInfo(callName As String, submitterName As String, submittedOn As Date?, submittedBy As String, committees As List(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCommitteeEvaluationInfo), idCommittee As Long,
                                   isPresident As Boolean, minValue As Integer) Implements IViewEvaluationSummary.LoadSubmissionInfo

        _isPresident = isPresident

        MLVstatistics.SetActiveView(VIWstatistics)
        Me.LBowner.Text = submitterName
        Me.LBcallName.Text = callName

        Me.minValue = minValue

        If submittedOn.HasValue Then
            Me.LBsubmittedOn.Text = submittedOn.Value.ToShortDateString & " " & submittedOn.Value.ToShortTimeString
            LBsubmittedBy_t.Visible = Not String.IsNullOrEmpty(submitterName)
            LBsubmittedBy.Visible = Not String.IsNullOrEmpty(submitterName)
            Me.LBsubmittedBy.Text = submitterName
        Else
            Me.LBsubmittedOn.Text = "//"
        End If

        Me.SPNsingleCommittee.Visible = (committees.Count = 1)
        'Me.RPTevaluationStatus.Visible = True
        If (committees.Count > 1) Then
            Resource.setLiteral(LTdisplayEvaluationsStatus_t, "multiple")
        Else
            Resource.setLiteral(LTdisplayEvaluationsStatus_t, "single")
        End If
        'If committees.Count <= 1 Then
        If (committees.Any) Then

            Resource.setLabel_To_Value(LBcommitteeEvaluation, "SingleEvaluation." & committees(0).Status.ToString)

            Select Case committees(0).Status
                Case EvaluationStatus.None
                    LBcommitteeEvaluation.Text = "Non iniziata"
                Case EvaluationStatus.Confirmed
                    LBcommitteeEvaluation.Text = "Confermata"
                    _canConfirmEvaluation = False
                Case Else
                    LBcommitteeEvaluation.Text = "In corso"
            End Select

            If committees(0).Status = EvaluationStatus.Confirmed Then

                LBcommitteeEvaluation.Text = "Confermata"
            End If

            SetEvaluationStatus(committees(0).Status, LBcommitteeEvaluation)

            Me.LBcommittee.Text = committees.FirstOrDefault().Name
        End If
        'Else
        '    Me.RPTevaluationStatus.DataSource = committees
        '    Me.RPTevaluationStatus.DataBind()
        'End If


        'Me.DDLcommittees.DataSource = committees
        'Me.DDLcommittees.DataTextField = "Name"
        'Me.DDLcommittees.DataValueField = "IdCommittee"
        'Me.DDLcommittees.DataBind()
        'Select Case committees.Count
        '    Case 0
        '        Me.DDLcommittees.Items.Insert(0, New ListItem(Resource.getValue("NoCommittees"), 0))
        '    Case 1

        '    Case Else
        '        Me.DDLcommittees.Items.Insert(0, New ListItem(Resource.getValue("AllCommittees"), 0))
        'End Select
    End Sub
    Private Sub LoadEvaluations(evaluations As List(Of dtoSubmissionCommitteeItem)) Implements IViewEvaluationSummary.LoadEvaluations
        If IsNothing(evaluations) OrElse evaluations.Count = 0 OrElse evaluations.Count = 0 Then
            Me.DisplayNoEvaluationsFound()
            Me.DVexport.Visible = False
        Else
            Me.DVexport.Visible = AllowExportAll OrElse AllowExportCurrent
            ' Me.DVexport.Attributes.Add("class", IIf((AllowExportAll AndAlso AllowExportCurrent), "ddbuttonlist enabled", "ddbuttonlist"))
            Me.MLVstatistics.SetActiveView(VIWstatistics)

            Me.RPTcommittees.DataSource = evaluations
            Me.RPTcommittees.DataBind()
        End If
    End Sub

#Region "Helpers"
    Private Sub InitStackedBar(item As dtoCommitteesSummaryItem, control As UC_StackedBar)
        Dim count As Long = item.Committees.Select(Function(c) c.Evaluations.Count()).Sum()
        Dim nEvaluated As Long = item.GetEvaluationsCount(EvaluationStatus.Evaluated)
        Dim nEvaluating As Long = item.GetEvaluationsCount(EvaluationStatus.Evaluating)
        Dim nNotstarted As Long = item.GetEvaluationsCount(EvaluationStatus.None)
        Dim nReplace As Long = item.GetEvaluationsCount(EvaluationStatus.EvaluatorReplacement)
        Dim nInvalid As Long = item.GetEvaluationsCount(EvaluationStatus.Invalidated)
        InitStackedBar(count, nEvaluated, nEvaluating, nNotstarted, nReplace, nInvalid, control)

    End Sub
    Private Sub InitStackedBar(item As dtoCommitteeDisplayItem, control As UC_StackedBar)
        Dim count As Long = item.Evaluations.Count
        Dim nEvaluated As Long = item.GetEvaluationsCount(EvaluationStatus.Evaluated)
        Dim nEvaluating As Long = item.GetEvaluationsCount(EvaluationStatus.Evaluating)
        Dim nNotstarted As Long = item.GetEvaluationsCount(EvaluationStatus.None)
        Dim nReplace As Long = item.GetEvaluationsCount(EvaluationStatus.EvaluatorReplacement)
        Dim nInvalid As Long = item.GetEvaluationsCount(EvaluationStatus.Invalidated)

        InitStackedBar(count, nEvaluated, nEvaluating, nNotstarted, nReplace, nInvalid, control)
    End Sub

    Private Sub InitStackedBar(count As Long, nEvaluated As Long, nEvaluating As Long, nNotstarted As Long, nReplace As Long, nInvalid As Long, control As UC_StackedBar)
        Dim evaluated As StackedBarItem = New StackedBarItem()
        evaluated.CssClass = "completed"
        evaluated.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Evaluated.ToString())
        evaluated.Value = Percentual(nEvaluated, count)

        Dim evaluating As StackedBarItem = New StackedBarItem()
        evaluating.CssClass = "started"
        evaluating.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Evaluating.ToString())
        evaluating.Value = Percentual(nEvaluating, count)

        Dim notstarted As StackedBarItem = New StackedBarItem()
        notstarted.CssClass = "notstarted"
        notstarted.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.None.ToString())
        notstarted.Value = Percentual(nNotstarted, count)
        If nReplace = 0 AndAlso nInvalid = 0 Then
            control.InitializeControl({evaluated, evaluating, notstarted})
        Else
            Dim otherValues As StackedBarItem = New StackedBarItem()
            otherValues.CssClass = IIf(nInvalid > 0, EvaluationStatus.Invalidated.ToString.ToLower(), "") & IIf(nReplace > 0, " " & EvaluationStatus.EvaluatorReplacement.ToString.ToLower(), "")
            If nInvalid > 0 AndAlso nReplace > 0 Then
                otherValues.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus.Other")
            ElseIf nInvalid > 0 Then
                otherValues.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.Invalidated.ToString())
            ElseIf nReplace > 0 Then
                otherValues.Title = Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.EvaluatorReplacement.ToString())
            End If

            otherValues.Value = Percentual(nInvalid + nReplace, count)
            If otherValues.Value > 0 Then
                otherValues.Value = 100 - evaluated.Value - evaluating.Value - notstarted.Value
            ElseIf notstarted.Value > 0 Then
                notstarted.Value = 100 - evaluated.Value - evaluating.Value
            ElseIf evaluating.Value > 0 Then
                evaluating.Value = 100 - evaluated.Value
            End If
            control.InitializeControl({evaluated, evaluating, otherValues, notstarted})
        End If

    End Sub
    Private Function Percentual(value As Long, tot As Long) As Int32
        If tot > 0 Then
            Dim d As Double = value / tot * 100
            Return Math.Floor(d)
        Else
            Return 0
        End If
    End Function
#End Region

#End Region

#Region "Grid"
    'Private Sub RPTevaluationStatus_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluationStatus.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim item As dtoCommitteeEvaluationInfo = DirectCast(e.Item.DataItem, dtoCommitteeEvaluationInfo)
    '        Dim oLiteral As Literal = e.Item.FindControl("LTinCommittee")
    '        oLiteral.Visible = True
    '        Resource.setLiteral(oLiteral)

    '        Dim oLabel As Label = e.Item.FindControl("LBcommitteeName")
    '        oLabel.Text = item.Name

    '        oLabel = e.Item.FindControl("LBcommitteeEvaluation")
    '        Resource.setLabel_To_Value(oLabel, "LBcommitteeEvaluation." & item.Status.ToString)

    '        SetEvaluationStatus(item.Status, oLabel)
    '    End If
    'End Sub
    Private Sub SetEvaluationStatus(status As EvaluationStatus, oLabel As Label)
        Dim css As String = ""
        Select Case status
            Case EvaluationStatus.Evaluating
                css = " gray"
            Case EvaluationStatus.Evaluated
                css = " yellow"
            Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                css = " red"
            Case EvaluationStatus.Confirmed
                css = " blue"
            Case Else
                css &= " gray"
        End Select

        oLabel.CssClass &= css
    End Sub
    Private _tempIdCommittee As Long
    Private Sub RPTcommittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommittees.ItemDataBound
        Dim item As dtoSubmissionCommitteeItem = DirectCast(e.Item.DataItem, dtoSubmissionCommitteeItem)

        Dim oLiteral As Literal = e.Item.FindControl("LTcommitteeName_t")
        Resource.setLiteral(oLiteral)

        oLiteral = e.Item.FindControl("LTsubmissionPointsSingle_t")
        Resource.setLiteral(oLiteral)

        oLiteral = e.Item.FindControl("LTevaluatorsSliderTop")
        Resource.setLiteral(oLiteral)


        oLiteral = e.Item.FindControl("LTevaluatorsSliderBottom")
        Resource.setLiteral(oLiteral)


        Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDevaluatorPlaceHolder")
        oTableCell.Visible = item.Evaluators.Count < 5
        If item.Evaluators.Count < 5 Then
            oTableCell.ColSpan = 5 - item.Evaluators.Count
        End If

        oTableCell = e.Item.FindControl("THevaluatorPlaceHolder")
        oTableCell.Visible = item.Evaluators.Count < 5
        If item.Evaluators.Count < 5 Then
            oTableCell.ColSpan = 5 - item.Evaluators.Count
        End If
        Dim oLabel As Label = e.Item.FindControl("LBcommitteeRating")
        Select Case CurrentEvaluationType
            Case EvaluationType.Average
                oLabel.Text = item.AverageRatingToString
                'Case EvaluationType.Dss
                '    If Not IsNothing(item.DssEvaluation) AndAlso item.DssEvaluation.IsValid Then
                '        SetDssValue(Not item.Evaluators.Any(Function(ev) ev.Status <> EvaluationStatus.Evaluated AndAlso ev.Status <> EvaluationStatus.EvaluatorReplacement AndAlso ev.Status <> EvaluationStatus.Invalidated), item.DssEvaluation, IsFuzzyValue(item.DssEvaluation.IsFuzzy, item.IdCommittee), oLabel, e.Item.FindControl("SPNfuzzy"), e.Item.FindControl("CTRLfuzzyNumber"))
                '    Else
                '        oLabel.Text = "--"
                '    End If
            Case Else
                oLabel.Text = item.SumRatingToString
        End Select
        _tempIdCommittee = item.IdCommittee
    End Sub
    Protected Sub RPTevaluators_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim item As dtoEvaluatorDisplayItem = DirectCast(e.Item.DataItem, dtoEvaluatorDisplayItem)

        Dim oLabel As Label = e.Item.FindControl("LBevaluatorName")
        oLabel.ToolTip = item.Surname
        oLabel.Text = item.Name(0).ToString.ToUpper() & ". " & item.Surname


        Dim LTisconfirmed As Literal = e.Item.FindControl("LTEvalStatus")

        Dim statusString As String = item.Status.ToString() 'ToDo: internazionalizzare
        LTisconfirmed.Text = String.Format(LTisconfirmed.Text, item.Status.ToString(), statusString)

        Dim LkbConfirm As LinkButton = e.Item.FindControl("LkbConfirm")
        LkbConfirm.CommandName = "Confirm"
        LkbConfirm.CommandArgument = item.IdEvaluation
        LkbConfirm.Visible = _isPresident AndAlso _canConfirmEvaluation

        'If item.Status = EvaluationStatus.Evaluated Then
        '    LkbConfirm.Visible = True
        'Else
        '    LkbConfirm.Visible = True
        'End If

        Dim LkbSetDraft As LinkButton = e.Item.FindControl("LkbSetDraft")
        LkbSetDraft.CommandName = "SetDraft"
        LkbSetDraft.CommandArgument = item.IdEvaluation
        LkbSetDraft.Visible = _isPresident AndAlso _canConfirmEvaluation

    End Sub
    Protected Sub RPTevaluatorEvaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoEvaluatorDisplayItem = DirectCast(e.Item.DataItem, dtoEvaluatorDisplayItem)
            Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDevaluator")
            Dim oGeneric As HtmlGenericControl = e.Item.FindControl("SPNvoteContainer")
            Dim oLabel As Label = e.Item.FindControl("LBvote")
            Dim oHyperLink As HyperLink = e.Item.FindControl("HYPvote")
            If dto.IgnoreEvaluation Then
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " ignoreevaluation")
                oLabel.Text = "//"
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " gray")
            Else
                Select Case dto.Status
                    Case EvaluationStatus.Evaluated
                        oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " green")
                    Case EvaluationStatus.Evaluating
                        oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " yellow")
                    Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                        oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & dto.Status.ToString.ToLower())
                        ' " red")
                    Case EvaluationStatus.Confirmed
                        oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " blue")
                End Select

                If dto.Status = EvaluationStatus.EvaluatorReplacement OrElse dto.Status = EvaluationStatus.Invalidated Then
                    If dto.Values.Where(Function(v) Not v.IsValueEmpty).Any() Then
                        oLabel.Visible = False
                        oHyperLink.Visible = True
                        oHyperLink.Text = "//"
                        oHyperLink.ToolTip = Resource.getValue("ViewEvaluation")
                        'oHyperLink.NavigateUrl = BaseUrl & RootObject.ViewUserEvaluations(dto.IdEvaluator, dto.IdSubmission, IdCall, IdCallCommunity)
                        oHyperLink.Enabled = False
                    Else
                        oLabel.Text = "//"
                    End If
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " gray")
                ElseIf dto.Status = EvaluationStatus.None Then
                    oLabel.Text = "--"
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " gray")
                Else
                    oLabel.Visible = False
                    oHyperLink.Visible = True

                    If minValue > 0 Then

                        If CurrentEvaluationType = EvaluationType.Average Then
                            oHyperLink.Text = String.Format("{0}/{1}", dto.AverageRatingToString(), minValue)
                        Else
                            oHyperLink.Text = String.Format("{0}/{1}", dto.SumRatingToString(), minValue)
                        End If
                    Else
                            oHyperLink.Text = dto.SumRatingToString
                    End If

                    'Select Case CurrentEvaluationType
                    '    Case EvaluationType.Average
                    '        oHyperLink.Text = dto.AverageRatingToString
                    '    Case EvaluationType.Dss
                    '        If IsNothing(dto.DssEvaluation) Then
                    '            oLabel.Text = "--"
                    '            oLabel.Visible = True
                    '            oHyperLink.Visible = False
                    '        Else
                    '            SetDssValue((dto.Status = EvaluationStatus.Evaluated OrElse dto.DssEvaluation.IsValid), dto.DssEvaluation, IsFuzzyValue(dto.DssEvaluation.IsFuzzy, dto.IdCommittee), oHyperLink, e.Item.FindControl("SPNfuzzy"), e.Item.FindControl("CTRLfuzzyNumber"))
                    '        End If
                    '        'If _AllEvaluated Then
                    '        '    oHyperLink.Text = dto.DssRankingToString(4)
                    '        'Else
                    '        '    oHyperLink.Text = dto.DssRankingToString(4) & "(*)"
                    '        'End If
                    '    Case Else
                    '        oHyperLink.Text = dto.SumRatingToString
                    'End Select

                    oHyperLink.ToolTip = Resource.getValue("ViewEvaluation")
                    oHyperLink.NavigateUrl = BaseUrl & RootObject.ViewUserEvaluations(dto.IdEvaluator, IdSubmission, IdCall, IdCallCommunity, AdvCommissionId)
                End If

                oLabel = e.Item.FindControl("LBiconComment")
                Resource.setLabel(oLabel)
                oLabel.ToolTip = Resource.getValue("GlobalComment.ToolTip")
                Dim cComment As UC_DialogComment = e.Item.FindControl("CTRLcomment")

                If Not String.IsNullOrEmpty(dto.Comment) Then
                    oLabel.Visible = True
                    cComment.Visible = True
                    cComment.InitializeControl(Resource.getValue("GlobalComment"), dto.SubmitterName, dto.EvaluatorName, dto.Comment)
                End If


                Dim LTpassed As Literal = e.Item.FindControl("LTpassed")
                If dto.IsPassed Then
                    LTpassed.Text = String.Format(LTpassed.Text, "passed", "Approvato")
                Else
                    LTpassed.Text = String.Format(LTpassed.Text, "failed", "Respinto")
                End If



            End If

        End If
    End Sub

    Protected Sub RPTevaluators_ItemCommand(source As Object, e As RepeaterCommandEventArgs)

        Dim EvalId As Long = 0
        Try
            EvalId = System.Convert.ToInt64(e.CommandArgument)
        Catch ex As Exception
        End Try

        If EvalId > 0 Then
            Select Case e.CommandName
                Case "Confirm"

                    CallTrapHelper.SendTrap(
                        lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsConfirm,
                        Me.IdCurrentCommittee,
                        lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                        String.Format("SubmissionId:{0}; EvaluationId:{1}", Me.IdSubmission, EvalId))

                    Me.CurrentPresenter.EvalSetConfirmed(EvalId)
                Case "SetDraft"
                    CallTrapHelper.SendTrap(
                        lm.Comol.Modules.CallForPapers.Trap.CallTrapId.AdvEvaluationsSetDraft,
                        Me.IdCurrentCommittee,
                        lm.Comol.Modules.CallForPapers.Trap.CallObjectId.AdvCommission,
                        String.Format("SubmissionId:{0}; EvaluationId:{1}", Me.IdSubmission, EvalId))

                    Me.CurrentPresenter.EvalSetDraf(EvalId)
            End Select
        End If
    End Sub

    Protected Sub RPTcriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCriterionSummaryItem = DirectCast(e.Item.DataItem, dtoCriterionSummaryItem)

            Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDcriteriaPlaceHolder")
            Dim count As Integer = dto.EvaluatorsCount
            oTableCell.Visible = count < 5
            If count < 5 Then
                oTableCell.ColSpan = 5 - count
            End If


            Dim oLiteral As Literal = e.Item.FindControl("LTcriterionName")
            oLiteral.Text = String.Format(Resource.getValue("CriterionName"), dto.Name)
            Dim oLabel As Label = e.Item.FindControl("LBcriteriaRating")

            If dto.Type = CriterionType.Boolean Then
                oLabel.Text = "--"
            Else
                Select Case CurrentEvaluationType
                    Case EvaluationType.Average
                        oLabel.Text = dto.AverageRatingToString
                    Case EvaluationType.Dss

                        'If _AllEvaluated Then
                        oLabel.Text = ""
                        'Else
                        '    oHyperLink.Text = dto.DssRatingToString(4) & "(*)"
                        'End If
                    Case Else
                        oLabel.Text = dto.SumRatingToString
                End Select
            End If

        End If
    End Sub

    Protected Sub RPTcriteriaEvaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCriterionValueSummaryItem = DirectCast(e.Item.DataItem, dtoCriterionValueSummaryItem)

            Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDcriterion")
            Dim oGeneric As HtmlGenericControl = e.Item.FindControl("SPNvoteContainer")

            Select Case dto.Criterion.Status
                Case EvaluationStatus.Evaluated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " green")
                Case EvaluationStatus.Evaluating
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " yellow")
                Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " red")
            End Select
            Dim oLabel As Label = e.Item.FindControl("LBvote")

            Dim name As String = "", shortName As String = ""
            Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")

            If dto.Criterion.Status = EvaluationStatus.EvaluatorReplacement OrElse dto.Criterion.Status = EvaluationStatus.Invalidated Then
                oLabel.Text = "//"
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " gray")
            ElseIf dto.Criterion.Status = EvaluationStatus.None OrElse dto.Criterion.IsValueEmpty Then
                oLabel.Text = "--"
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " gray")
            Else
                Select Case dto.Criterion.Criterion.Type
                    Case CriterionType.StringRange
                        oLabel.CssClass = "text"
                        If dto.Criterion.IdOption > 0 AndAlso dto.Criterion.Criterion.Options.Where(Function(o) o.Id = dto.Criterion.IdOption).Any() Then
                            oLabel.Text = dto.Criterion.Criterion.Options.Where(Function(o) o.Id = dto.Criterion.IdOption).FirstOrDefault().Name
                            oLabel.ToolTip = "( " & dto.Criterion.Criterion.Options.Where(Function(o) o.Id = dto.Criterion.IdOption).FirstOrDefault().ValueToString & " )"
                        Else
                            oLabel.Text = "--"
                        End If
                    Case CriterionType.DecimalRange, CriterionType.IntegerRange
                        If IsFuzzyValue(dto.Criterion.DssValue, _tempIdCommittee) Then
                            SetDssValue(dto.Criterion.DecimalValue, oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                        Else
                            oLabel.Text = dto.Criterion.DecimalValueToString
                        End If
                    Case CriterionType.RatingScale, CriterionType.RatingScaleFuzzy
                        If dto.Criterion.DssValue.Error = lm.Comol.Core.Dss.Domain.DssError.None Then
                            SetDssValue(dto.Criterion, IsFuzzyValue(dto.Criterion.DssValue, _tempIdCommittee), oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                            'Select Case dto.Criterion.DssValue.RatingType
                            '    Case lm.Comol.Core.Dss.Domain.RatingType.simple
                            '        oLabel.Text = GetEvaluatedValue(dto.Criterion.DssValue.Value)
                            '    Case Else
                            '        Dim optStart As dtoCriterionOption = dto.Criterion.Criterion.Options.Where(Function(o) o.IdRatingValue = dto.Criterion.DssValue.IdRatingValue).FirstOrDefault()
                            '        Dim optEnd As dtoCriterionOption = dto.Criterion.Criterion.Options.Where(Function(o) o.IdRatingValue = dto.Criterion.DssValue.IdRatingValueEnd).FirstOrDefault()
                            '        Dim oControl As UC_FuzzyNumber = e.Item.FindControl("CTRLfuzzyNumber")
                            '        oSpan.Visible = True
                            '        oLabel.Visible = False

                            '        Select Case dto.Criterion.DssValue.RatingType
                            '            Case lm.Comol.Core.Dss.Domain.RatingType.extended
                            '            Case lm.Comol.Core.Dss.Domain.RatingType.intermediateValues

                            '        End Select
                            '        oControl.InitializeControl(False, dto.Criterion.DssValue.Value, dto.Criterion.DssValue.ValueFuzzy, name, shortName)
                            'End Select
                        Else
                            oLabel.Text = "--"
                        End If
                        'Case 
                        '    If dto.Criterion.DssValue.Error = lm.Comol.Core.Dss.Domain.DssError.None Then
                        '        Dim optStart As dtoCriterionOption = dto.Criterion.Criterion.Options.Where(Function(o) o.IdRatingValue = dto.Criterion.DssValue.IdRatingValue).FirstOrDefault()
                        '        Dim optEnd As dtoCriterionOption = dto.Criterion.Criterion.Options.Where(Function(o) o.IdRatingValue = dto.Criterion.DssValue.IdRatingValueEnd).FirstOrDefault()
                        '        Dim endName As String = "", endShortName As String = ""
                        '        If Not IsNothing(optStart) Then
                        '            name = optStart.Name
                        '            shortName = optStart.ShortName
                        '        End If
                        '        If Not IsNothing(optEnd) Then
                        '            endName = optEnd.Name
                        '            endShortName = optEnd.ShortName
                        '        End If
                        '        Dim oControl As UC_FuzzyNumber = e.Item.FindControl("CTRLfuzzyNumber")
                        '        oControl.Visible = True
                        '        oLabel.Visible = False
                        '        oControl.InitializeControl(False, dto.Criterion.DssValue.Value, dto.Criterion.DssValue.ValueFuzzy, dto.Criterion.DssValue.RatingType, name, shortName, endName, endShortName)
                        '    Else
                        '        oLabel.Text = "--"
                        '    End If
                    Case CriterionType.Textual
                        oLabel.Text = dto.Criterion.StringValue
                    Case CriterionType.Boolean
                        If (dto.Criterion.DecimalValue > 0) Then
                            oLabel.Text = "Approvato"
                        Else
                            oLabel.Text = "Non approvato"
                        End If

                End Select

            End If
            oLabel = e.Item.FindControl("LBiconComment")
            Resource.setLabel(oLabel)
            oLabel.ToolTip = Resource.getValue("CriterionComment.ToolTip")
            oLabel.Visible = Not String.IsNullOrEmpty(dto.Criterion.Comment)
            Dim cComment As UC_DialogComment = e.Item.FindControl("CTRLcomment")

            Select Case dto.Criterion.Criterion.CommentType
                Case CommentType.Allowed, CommentType.Mandatory
                    If Not String.IsNullOrEmpty(dto.Criterion.Comment) Then
                        cComment.Visible = True
                        cComment.InitializeControl(Resource.getValue("CriterionComment"), dto.Evaluator.SubmitterName, dto.Evaluator.EvaluatorName, dto.Criterion.Comment)
                    End If
            End Select
        End If
    End Sub
    Private Function GetEvaluatedValue(sum As Double, Optional decimals As Integer = 2) As String
        Dim fractional As Double = sum - Math.Floor(sum)
        If (fractional = 0) Then
            Return String.Format("{0:N0}", sum)
        Else
            Return String.Format("{0:N" & decimals.ToString() & "}", sum)
        End If
    End Function
    'Private Sub DDLcommittees_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLcommittees.SelectedIndexChanged
    '    Me.CurrentPresenter.LoadEvaluations(IdSubmission, IdCall, CurrentEvaluationType, DDLcommittees.SelectedValue, CommitteesCount)
    'End Sub
    Private Sub SetDssValue(isCompleted As Boolean, rating As DssCallEvaluation, displayAsFuzzy As Boolean, oHyperlink As HyperLink, oGenericControl As HtmlGenericControl, oControl As UC_FuzzyNumber)
        Dim name As String = "", shortName As String = ""
        If Not IsNothing(rating) AndAlso (rating.IsValid OrElse rating.IsCompleted OrElse isCompleted) Then
            If Not rating.IsFuzzy AndAlso Not displayAsFuzzy Then
                oHyperlink.Text = GetEvaluatedValue(rating.Ranking, 4)
                oGenericControl.Visible = False
            Else
                oGenericControl.Visible = True
                oControl.InitializeControl(True, rating.Ranking, rating.Value, rating.ValueFuzzy, GetEvaluatedValue(rating.Ranking, 4), "")
            End If
            If Not rating.IsValid OrElse Not rating.IsCompleted Then
                oHyperlink.ToolTip = Resource.getValue("DssNotcompleted.Evaluator")
            ElseIf Not isCompleted Then
                oHyperlink.ToolTip = Resource.getValue("DssNotcompleted")
            End If
        Else
            oHyperlink.Text = "--"
            oHyperlink.ToolTip = Resource.getValue("DssNotcompleted.Evaluator")
        End If
    End Sub
    Private Sub SetDssValue(isCompleted As Boolean, rating As DssCallEvaluation, displayAsFuzzy As Boolean, oLabel As Label, oGenericControl As HtmlGenericControl, oControl As UC_FuzzyNumber)
        Dim name As String = "", shortName As String = ""
        If Not IsNothing(rating) AndAlso rating.IsValid AndAlso (rating.IsCompleted OrElse isCompleted) Then
            If Not rating.IsFuzzy AndAlso Not displayAsFuzzy Then
                oLabel.Text = GetEvaluatedValue(rating.Value, 4)
                oGenericControl.Visible = False
            Else
                oGenericControl.Visible = True
                oLabel.Visible = False
                oControl.InitializeControl(True, rating.Value, rating.ValueFuzzy, GetEvaluatedValue(rating.Ranking, 4), "")
            End If
        Else
            oLabel.Text = "--"
            oLabel.ToolTip = Resource.getValue("DssNotcompleted")
        End If
    End Sub
    Private Sub SetDssValue(item As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated, displayAsFuzzy As Boolean, oLabel As Label, oSpan As HtmlGenericControl, oControl As UC_FuzzyNumber)
        Dim name As String = "", shortName As String = ""
        If Not IsNothing(item.DssValue) AndAlso item.DssValue.Error = lm.Comol.Core.Dss.Domain.DssError.None Then
            Dim optStart As dtoCriterionOption = item.Criterion.Options.Where(Function(o) o.IdRatingValue = item.DssValue.IdRatingValue).FirstOrDefault()
            Dim optEnd As dtoCriterionOption = item.Criterion.Options.Where(Function(o) o.IdRatingValue = item.DssValue.IdRatingValueEnd).FirstOrDefault()

            Dim endName As String = "", endShortName As String = ""
            If Not IsNothing(optStart) Then
                name = optStart.Name
                shortName = optStart.ShortName
            End If
            If Not IsNothing(optEnd) Then
                endName = optEnd.Name
                endShortName = optEnd.ShortName
            End If

            Select Case item.DssValue.RatingType
                Case lm.Comol.Core.Dss.Domain.RatingType.simple
                    If Not item.DssValue.IsFuzzy AndAlso Not displayAsFuzzy Then
                        oLabel.Text = GetEvaluatedValue(item.DssValue.Value)
                    Else
                        oSpan.Visible = True
                        oLabel.Visible = False
                        oControl.InitializeControl(item.DssValue.IsFuzzy OrElse displayAsFuzzy, item.DssValue.Value, item.DssValue.ValueFuzzy, name, shortName)
                    End If
                Case Else
                    oSpan.Visible = True
                    oLabel.Visible = False
                    oControl.InitializeControl(item.DssValue.IsFuzzy OrElse displayAsFuzzy, item.DssValue.Value, item.DssValue.ValueFuzzy, name, shortName)
            End Select
        Else
            oLabel.Text = "--"
        End If
    End Sub
    Private Sub SetDssValue(value As Decimal, oLabel As Label, oSpan As HtmlGenericControl, oControl As UC_FuzzyNumber)
        oSpan.Visible = True
        oLabel.Visible = False
        oControl.InitializeControl(True, value)
    End Sub
    Private Function IsFuzzyValue(ByVal itemRating As lm.Comol.Core.Dss.Domain.Templates.dtoItemRating, idComittee As Long)
        Return IsFuzzyValue(Not IsNothing(itemRating) AndAlso itemRating.IsFuzzy, idComittee)
    End Function
    Private Function IsFuzzyValue(ByVal itemValue As Boolean, idComittee As Long)
        Dim result As Boolean = itemValue OrElse CallUseFuzzy
        If Not result AndAlso CommitteeIsFuzzy.ContainsKey(idComittee) Then
            Return CommitteeIsFuzzy(idComittee)
        End If
        Return result
    End Function
#End Region

#Region "Export Data"

#Region "Button"
    Private Sub LNBexportAllEvaluationsSummaryData_Click(sender As Object, e As System.EventArgs) Handles LNBexportCommitteeEvaluationsToCsv.Click
        ExportEvaluations(SummaryType.EvaluationCommittees, IdSubmission, ExportData.Fulldata, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportAllCommitteeSummaryToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportCommitteesEvaluationsToCsv.Click
        ExportEvaluations(SummaryType.EvaluationCommittee, IdSubmission, ExportData.Fulldata, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportCommitteeEvaluationsOneColumnToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportCommitteeEvaluationsOneColumnToCsv.Click
        ExportEvaluations(SummaryType.EvaluationCommittees, IdSubmission, ExportData.FulldataToAnalyze, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportCommitteesEvaluationsOneColumnToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportCommitteesEvaluationsOneColumnToCsv.Click
        ExportEvaluations(SummaryType.EvaluationCommittee, IdSubmission, ExportData.FulldataToAnalyze, Helpers.Export.ExportFileType.csv)
    End Sub
#End Region

    Private Sub ExportEvaluations(type As SummaryType, idSubmission As Long, data As ExportData, fileType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Response.Clear()
        Response.AppendCookie(New HttpCookie(Me.CookieName, HDNdownloadTokenValue.Value)) '; //downloadTokenValue will have been provided in the form submit via the hidden input field
        Dim fileName As String = Resource.getValue(type.ToString() & ".All")
        If String.IsNullOrEmpty(fileName) Then
            fileName = GetFileName(type, fileType)
        Else
            fileName = CurrentPresenter.GetFileName(fileName, type, IdCall, idSubmission, IdCurrentCommittee) & "." & fileType.ToString
        End If
        Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)

        Try
            Dim translations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations))
                translations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations), name), Me.Resource.getValue("EvaluationTranslations." & name))
            Next
            Dim tStatus As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus))
                tStatus.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus), name), Me.Resource.getValue("EvaluationTranslations.EvaluationStatus." & name))
            Next

            Select Case fileType
                Case Helpers.Export.ExportFileType.csv
                    Response.Charset = "utf-8"
                    Response.ContentType = "text/csv"
                Case Else
                    Response.Charset = "utf-8"
                    Response.ContentType = "text/csv"
            End Select
            Response.BinaryWrite(Response.ContentEncoding.GetPreamble())
            Response.Write(CurrentPresenter.ExportTo(type, IdCall, idSubmission, IdCurrentCommittee, data, fileType, translations, tStatus))
        Catch de As Exception

        End Try
        Response.End()
    End Sub
    Private Function GetFileName(type As SummaryType, ByVal fileType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String
        Dim filename As String = Resource.getValue("ExportGenericEvaluations." & type.ToString)
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            Select Case type
                Case SummaryType.EvaluationCommittee
                    filename = "{0}_{1}_{2}_EvaluationCommittee"
                Case SummaryType.EvaluationCommittees
                    filename = "{0}_{1}_{2}_EvaluationCommittees"
            End Select
        End If
        Return String.Format(filename, oDate.Year, oDate.Month, oDate.Day) & "." & fileType.ToString
    End Function
#End Region

    Private Sub ListCalls_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

End Class