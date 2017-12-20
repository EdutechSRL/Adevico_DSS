
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.ActionDataContract
Imports System.Linq

Public Class ViewEvaluation
    Inherits PageBase
    Implements IViewViewEvaluation

#Region "Context"
    Private _Presenter As ViewEvaluationPresenter
    Private ReadOnly Property CurrentPresenter() As ViewEvaluationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewEvaluationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewViewEvaluation.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewViewEvaluation.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdEvaluation As Long Implements IViewViewEvaluation.PreloadIdEvaluation
        Get
            If IsNumeric(Me.Request.QueryString("idEvaluation")) Then
                Return CLng(Me.Request.QueryString("idEvaluation"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdEvaluator As Long Implements IViewViewEvaluation.PreloadIdEvaluator
        Get
            If IsNumeric(Me.Request.QueryString("idEvaluator")) Then
                Return CLng(Me.Request.QueryString("idEvaluator"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdSubmission As Long Implements IViewViewEvaluation.PreloadIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewViewEvaluation.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property

    Private Property IdCall As Long Implements IViewViewEvaluation.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdEvaluation As Long Implements IViewViewEvaluation.IdEvaluation
        Get
            Return ViewStateOrDefault("IdEvaluation", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdEvaluation") = value
        End Set
    End Property
    Private Property IdEvaluator As Long Implements IViewViewEvaluation.IdEvaluator
        Get
            Return ViewStateOrDefault("IdEvaluator", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdEvaluator") = value
        End Set
    End Property
    Private Property IdSubmission As Long Implements IViewViewEvaluation.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewViewEvaluation.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewViewEvaluation.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewViewEvaluation.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property CurrentDisplay As DisplayEvaluations Implements IViewViewEvaluation.CurrentDisplay
        Get
            Return ViewStateOrDefault("CurrentDisplay", DisplayEvaluations.ForSubmission)
        End Get
        Set(value As DisplayEvaluations)
            Me.ViewState("CurrentDisplay") = value
        End Set
    End Property

    Private ReadOnly Property AnonymousDisplayname As String Implements IViewViewEvaluation.AnonymousDisplayName
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknonwUserName As String Implements IViewViewEvaluation.UnknonwUserName
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property
    Private Property AllowPrint As Boolean Implements IViewViewEvaluation.AllowPrint
        Get
            Return HYPprintEvaluation.Visible
        End Get
        Set(value As Boolean)
            HYPprintEvaluation.Visible = value
        End Set
    End Property
    Private Property CurrentEvaluationType As EvaluationType Implements IViewViewEvaluation.CurrentEvaluationType
        Get
            Return ViewStateOrDefault("CurrentEvaluationType", EvaluationType.Sum)
        End Get
        Set(ByVal value As EvaluationType)
            Me.ViewState("CurrentEvaluationType") = value
        End Set
    End Property
    Private Property CallUseFuzzy As Boolean Implements IViewViewEvaluation.CallUseFuzzy
        Get
            Return ViewStateOrDefault("CallUseFuzzy", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CallUseFuzzy") = value
        End Set
    End Property
    Private Property CommitteeIsFuzzy As Dictionary(Of Long, Boolean) Implements IViewViewEvaluation.CommitteeIsFuzzy
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

#Region "Internal"
    Private _IsFuzzyCommittee As Boolean
    Protected ReadOnly Property CssRowItem(ByVal type As displayAs) As String
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

    Public ReadOnly Property AdvCommId As Long Implements IViewViewEvaluation.AdvCommId
        Get
            '&acId={2}
            Dim advcId As Int64 = 0

            Try
                advcId = System.Convert.ToInt64(Request.QueryString("acId"))
            Catch ex As Exception

            End Try

            Return advcId
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSubmissionView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "")
        End If

        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSubmissionView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "NoPermessi")

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
            Master.ServiceTitle = .getValue("serviceTitle.ViewEvaluation")
            Master.ServiceNopermission = .getValue("nopermission")
            .setLabel_To_Value(LBcallName_t, "LBcallName_t." & CallType.ToString)
            .setLabel(LBowner_t)
            .setLabel(LBsubmittedOn_t)
            .setLabel(LBsubmittedBy_t)
            .setLabel(LBevaluatedBy_t)
            .setLabel(LBcommitteesInfo)
            .setHyperLink(HYPviewSubmission, False, True)
            .setHyperLink(HYPprintEvaluation, False, True)
            .setHyperLink(HYPviewTableEvaluation, False, True)

            .setLiteral(LTnavigateToEvaluators)
            .setLiteral(LTnavigateToCriteria)
            .setHyperLink(HYPnavigateToComments, False, False)
            .setLiteral(LTcommitteeName_t)
            .setLiteral(LTcommitteeEvaluationStatus_t)
            .setLiteral(LTcommitteeVote_t)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayDssWarning(lastUpdate As Date?, isCompleted As Boolean) Implements IViewViewEvaluation.DisplayDssWarning
        CTRLdssMessage.Visible = True
        If lastUpdate = Nothing Then
            CTRLdssMessage.InitializeControl(Resource.getValue("DisplayDssWarning.NoDate"), Helpers.MessageType.alert)
        Else
            CTRLdssMessage.InitializeControl(String.Format(Resource.getValue("DisplayDssWarning.Date.isCompleted." & isCompleted.ToString), lastUpdate.Value.ToString()), IIf(isCompleted, Helpers.MessageType.success, Helpers.MessageType.alert))
        End If
    End Sub
    Private Sub HideDssWarning() Implements IViewViewEvaluation.HideDssWarning
        CTRLdssMessage.Visible = False
    End Sub
    'Private Sub DisplayNoEvaluationsFound() Implements IViewViewEvaluation.DisplayNoEvaluationsFound
    '    DVexport.Visible = AllowExportAll
    '    Me.MLVevaluations.SetActiveView(VIWnoItems)
    '    Me.LBnoEvaluations.Text = Resource.getValue("DisplayNoEvaluationsFound")
    'End Sub
    'Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long) Implements IViewViewEvaluation.DisplayUnknownCall
    '    DVexport.Visible = False
    '    Me.DVfilter.Visible = False
    '    Me.MLVevaluations.SetActiveView(VIWnoItems)
    '    Me.LBnoEvaluations.Text = Resource.getValue("DisplayUnknownCall." & CallType.ToString)

    '    SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewEvaluationsSummary)
    'End Sub
    Private Sub DisplaySessionTimeout() Implements lm.Comol.Modules.CallForPapers.Presentation.IViewBase.DisplaySessionTimeout
        If PreloadIdEvaluation > 0 Then
            DisplaySessionTimeout(RootObject.ViewSingleEvaluation(PreloadIdEvaluation, PreloadIdSubmission, PreloadIdCall, PreloadIdCommunity, Me.AdvCommId))
        ElseIf PreloadIdEvaluator > 0 Then
            DisplaySessionTimeout(RootObject.ViewUserEvaluations(PreloadIdEvaluator, PreloadIdSubmission, PreloadIdCall, PreloadIdCommunity, Me.AdvCommId))
        Else
            DisplaySessionTimeout(RootObject.ViewSubmissionEvaluations(PreloadIdSubmission, PreloadIdCall, PreloadIdCommunity, Me.AdvCommId))
        End If
    End Sub
    Private Sub DisplaySessionTimeout(ByVal url As String) Implements IViewViewEvaluation.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSubmissionView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub

    Private Sub DisplayEvaluationUnavailable() Implements IViewViewEvaluation.DisplayEvaluationUnavailable
        MLVdata.SetActiveView(VIWdataEmpty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("DisplayEvaluationViewUnavailable"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewViewEvaluation.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayNoEvaluationsToView() Implements IViewViewEvaluation.DisplayNoEvaluationsToView
        Me.MLVevaluation.SetActiveView(VIWempty)
        LBempyMessage.Text = Resource.getValue("DisplayNoEvaluationsToView")
    End Sub
    Private Sub DisplayNoPermissionToView() Implements IViewViewEvaluation.DisplayNoPermissionToView
        Me.PageUtility.AddActionToModule(IdCallCommunity, IdCallModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.MLVdata.SetActiveView(VIWdataEmpty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("DisplayNoPermissionToView"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewViewEvaluation.DisplayUnknownCall
        Me.MLVdata.SetActiveView(VIWdataEmpty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("DisplayUnknownCall." & type.ToString), Helpers.MessageType.error)
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.ViewUnknowCallForPaper, , InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayUnknownEvaluation(idCommunity As Integer, idModule As Integer, idEvaluation As Long) Implements IViewViewEvaluation.DisplayUnknownEvaluation
        Me.MLVdata.SetActiveView(VIWdataEmpty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("DisplayUnknownEvaluation"), Helpers.MessageType.error)
    End Sub
    Public Sub DisplayUnknownSubmission(idCommunity As Integer, idModule As Integer, idSubmission As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements lm.Comol.Modules.CallForPapers.Presentation.Evaluation.IViewViewEvaluation.DisplayUnknownSubmission
        Me.MLVdata.SetActiveView(VIWdataEmpty)
        CTRLemptyMessage.InitializeControl(Resource.getValue("DisplayUnknownSubmission"), Helpers.MessageType.error)
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.ViewUnknowSubmission, , InteractionType.UserWithLearningObject)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idEvaluator As Long, idEvaluation As Long, idSubmission As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements lm.Comol.Modules.CallForPapers.Presentation.Evaluation.IViewViewEvaluation.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, IdCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idEvaluator As Long, idEvaluation As Long, idSubmission As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType) Implements lm.Comol.Modules.CallForPapers.Presentation.Evaluation.IViewViewEvaluation.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType.RequestForMembership, IdCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

    Private Sub SetViewEvaluationUrl(url As String) Implements IViewViewEvaluation.SetViewEvaluationUrl
        If String.IsNullOrEmpty(url) Then
            HYPviewTableEvaluation.Visible = False
            HYPviewTableEvaluation.NavigateUrl = ""
        Else
            HYPviewTableEvaluation.Visible = True
            HYPviewTableEvaluation.NavigateUrl = ApplicationUrlBase & url
        End If

    End Sub

    Private Sub LoadSubmissionInfo(owner As String, callName As String, submittedOn As DateTime?, submitterName As String) Implements IViewViewEvaluation.LoadSubmissionInfo
        Me.LBowner.Text = owner
        Me.LBcallName.Text = callName
        If submittedOn.HasValue Then
            Me.LBsubmittedOn.Text = submittedOn.Value.ToShortDateString & " " & submittedOn.Value.ToShortTimeString
            LBsubmittedBy_t.Visible = Not String.IsNullOrEmpty(submitterName)
            LBsubmittedBy.Visible = Not String.IsNullOrEmpty(submitterName)
            Me.LBsubmittedBy.Text = submitterName
        Else
            Me.LBsubmittedOn.Text = "//"
        End If
    End Sub
    Private Sub LoadCommitteesStatus(committees As List(Of dtoCommitteeEvaluationInfo), display As DisplayEvaluations) Implements IViewViewEvaluation.LoadCommitteesStatus
        Me.SPNsingleCommittee.Visible = (display = DisplayEvaluations.Single)
        Me.RPTevaluationStatus.Visible = Not (display = DisplayEvaluations.Single)
        If display = DisplayEvaluations.Single Then
            Resource.setLiteral(LTdisplayEvaluationsStatus_t, "single")
            If (committees.Any AndAlso committees.Count = 1) Then
                Resource.setLabel_To_Value(LBcommitteeEvaluation, "SingleEvaluation." & committees(0).Status.ToString)
                SetEvaluationStatus(committees(0).Status, LBcommitteeEvaluation)
            End If
        Else

            If (committees.Count > 1) AndAlso display <> DisplayEvaluations.Single Then
                Resource.setLiteral(LTdisplayEvaluationsStatus_t, "multiple")
            Else
                Resource.setLiteral(LTdisplayEvaluationsStatus_t, "single")
            End If
            Me.RPTevaluationStatus.DataSource = committees
            Me.RPTevaluationStatus.DataBind()
        End If
    End Sub
    Private Sub LoadEvaluatorInfo(name As String, committeesCount As Integer) Implements IViewViewEvaluation.LoadEvaluatorInfo
        Me.DVevaluatorInfo.Visible = True
        Me.LTevaluatedBy.Text = name
        Me.LBcommitteesInfo.Visible = (committeesCount > 1)
        If committeesCount > 1 Then
            Resource.setLabel(LBcommitteesInfo)
            If LBcommitteesInfo.Text <> "" AndAlso LBcommitteesInfo.Text.Contains("{0}") Then
                Me.LBcommitteesInfo.Text = String.Format(LBcommitteesInfo.Text, committeesCount)
            End If
        End If
    End Sub
    Private Sub LoadEvaluation(evaluation As dtoCommitteeEvaluation) Implements IViewViewEvaluation.LoadEvaluation
        Dim items As New List(Of dtoCommitteeEvaluation)
        items.Add(evaluation)
        LoadEvaluations(items)
    End Sub
    Private Sub LoadEvaluations(evaluations As List(Of dtoCommitteeEvaluation)) Implements IViewViewEvaluation.LoadEvaluations
        MLVevaluation.SetActiveView(VIWevaluator)
        RPTevaluatorCommittee.DataSource = evaluations
        RPTevaluatorCommittee.DataBind()
    End Sub
    Private Sub LoadEvaluations(comittees As List(Of dtoCommitteeEvaluationsDisplayItem)) Implements IViewViewEvaluation.LoadEvaluations
        If comittees.Any() Then
            MLVevaluation.SetActiveView(VIWevaluators)
            RPTcomittees.DataSource = comittees
            RPTcomittees.DataBind()
            If comittees.Count > 1 Then
                Resource.setLiteral(LTnavigateToComittees, "single")
            Else
                Resource.setLiteral(LTnavigateToComittees, "multiple")
            End If
            Me.SPNnavigateToComments.Visible = comittees.Where(Function(c) c.HasComments).Any()
            Me.HYPnavigateToComments.Visible = comittees.Where(Function(c) c.HasComments).Any()
        Else
            DisplayNoEvaluationsToView()
        End If
     
    End Sub
#End Region

    Private Sub ListCalls_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

    Private Sub RPTevaluationStatus_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluationStatus.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCommitteeEvaluationInfo = DirectCast(e.Item.DataItem, dtoCommitteeEvaluationInfo)
            Dim oLiteral As Literal = e.Item.FindControl("LTinCommittee")
            oLiteral.Visible = (CurrentDisplay <> DisplayEvaluations.ForSubmission)
            Resource.setLiteral(oLiteral)

            Dim oLabel As Label = e.Item.FindControl("LBcommitteeName")
            oLabel.Text = item.Name

            oLabel = e.Item.FindControl("LBcommitteeEvaluation")
            Resource.setLabel_To_Value(oLabel, "LBcommitteeEvaluation." & item.Status.ToString)

            SetEvaluationStatus(item.Status, oLabel)
        End If

    End Sub

#Region "Evaluator"
    Private _tempIdCommittee As Long
    Private Sub RPTevaluatorCommittee_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluatorCommittee.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCommitteeEvaluation = DirectCast(e.Item.DataItem, dtoCommitteeEvaluation)
            Dim oLiteral As Literal = e.Item.FindControl("LTcommitteeName")
            oLiteral.Text = item.Name


            Dim oLabel As Label = e.Item.FindControl("LBevaluationVote_t")
            Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBevaluationVote")

            Dim scoreString As String = ""


            Dim HasCriteria As Boolean = Not IsNothing(item.Evaluation) _
                AndAlso Not IsNothing(item.Evaluation.Criteria)

            If HasCriteria _
                AndAlso item.Evaluation.Criteria.Any(Function(c As dtoCriterionEvaluated) Not (c.Criterion.Type = CriterionType.Boolean)) Then

                Select Case CurrentEvaluationType
                    Case EvaluationType.Average
                        scoreString = item.AverageRating
                    Case EvaluationType.Dss
                        Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")
                        Dim oControl As UC_FuzzyNumber = e.Item.FindControl("CTRLfuzzyNumber")
                        oSpan.Visible = True
                        oLabel.Visible = False
                        If IsNothing(item.DssRating) Then
                            scoreString = "--"
                        Else
                            Dim message As String = ""
                            If Not item.DssRating.IsValid OrElse Not item.DssRating.IsCompleted Then
                                message = "(*)"
                            End If
                            oControl.InitializeControl(IsFuzzyValue(item.DssRating.IsFuzzy, item.IdCommittee), item.DssRating.Ranking, item.DssRating.Value, item.DssRating.ValueFuzzy, GetEvaluatedValue(item.DssRating.Ranking, 4) & message, "")
                        End If
                    Case Else
                        scoreString = item.Evaluation.SumRating
                End Select
            End If



            Dim BoolCount As String = ""
            If Not IsNothing(item.Evaluation) _
                AndAlso Not IsNothing(item.Evaluation.Criteria) _
                AndAlso item.Evaluation.Criteria.Any(Function(c As dtoCriterionEvaluated) c.Criterion.Type = CriterionType.Boolean) Then

                Dim Total As Integer = item.Evaluation.Criteria.Count()
                Dim Passed As Integer = item.Evaluation.Criteria.Where(Function(c As dtoCriterionEvaluated) c.DecimalValue > 0).Count()

                BoolCount = String.Format("<span class=""boolean"">Binari: {0}/{1}</span>", Passed, Total)
            End If

            Dim FinalPass As String = ""
            If item.Evaluation.IsPassed Then
                FinalPass = "Approvato"
            Else
                FinalPass = "Respinto"
            End If


            If Not String.IsNullOrEmpty(scoreString) Then
                scoreString = String.Format("<span class=""score"">Punteggio: {0}", scoreString)
            End If


            oLabel.Text = String.Format("{0}{1}{2}", FinalPass, scoreString, BoolCount)

            oLabel = e.Item.FindControl("LBevaluationComment_t")
                Resource.setLabel(oLabel)

                Dim oGenericControl As HtmlControl
                If String.IsNullOrEmpty(item.Evaluation.Comment) Then
                    oGenericControl = e.Item.FindControl("DDgeneralComment")
                    oGenericControl.Visible = False
                    oGenericControl = e.Item.FindControl("DDgeneralEmptyComment")
                    oGenericControl.Visible = True
                Else
                    oLabel = e.Item.FindControl("LBevaluationComment")
                    oLabel.Text = item.Evaluation.Comment
                End If
                _tempIdCommittee = item.IdCommittee
            End If
    End Sub
    Protected Sub RPTevaluationCriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCriterionEvaluated = DirectCast(e.Item.DataItem, dtoCriterionEvaluated)

            Dim oLabel As Label = e.Item.FindControl("LBevaluationCriterion_t")
            Resource.setLabel(oLabel)
            oLabel.Text = item.Criterion.Name


            Dim name As String = "", shortName As String = ""
            Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")
            oLabel = e.Item.FindControl("LBevaluationCriterion")
            Select Case item.Criterion.Type
                Case CriterionType.StringRange
                    ' oLabel.CssClass = "text"
                    If item.IdOption > 0 AndAlso item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).Any() Then
                        oLabel.Text = item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).FirstOrDefault().Name
                        oLabel.ToolTip = "( " & item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).FirstOrDefault().ValueToString & " )"
                    Else
                        oLabel.Text = "--"
                    End If
                Case CriterionType.DecimalRange, CriterionType.IntegerRange
                    If item.Ignore OrElse item.IsValueEmpty Then
                        oLabel.Text = "--"
                    ElseIf IsFuzzyValue(item.DssValue, _tempIdCommittee) Then
                        SetDssValue(item.DecimalValue, oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                    Else
                        oLabel.Text = item.DecimalValueToString
                    End If
                Case CriterionType.RatingScale, CriterionType.RatingScaleFuzzy
                    SetDssValue(item, IsFuzzyValue(item.DssValue, _tempIdCommittee), oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                Case CriterionType.Textual
                    If item.Ignore OrElse item.IsValueEmpty Then
                        oLabel.Text = "--"
                    Else
                        oLabel.Text = item.StringValue
                    End If
                Case CriterionType.Boolean
                    If item.DecimalValue > 0 Then
                        oLabel.Text = "Approvato"
                    Else
                        oLabel.Text = "Respinto"
                    End If


            End Select

            Dim oGenericControl As HtmlControl
            If Not item.CommentMandatory OrElse String.IsNullOrEmpty(item.Comment) Then
                oGenericControl = e.Item.FindControl("DDcriterionComment")
                oGenericControl.Visible = False
                oGenericControl = e.Item.FindControl("DDcriterionEmptyComment")
                oGenericControl.Visible = True
            Else
                oLabel = e.Item.FindControl("LBevaluationCriterionComment_t")
                Resource.setLabel(oLabel)

                oLabel = e.Item.FindControl("LBevaluationCriterionComment")
                oLabel.Text = item.Comment
            End If
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
#End Region

#Region "Evaluators"
    Private Sub RPTcomittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcomittees.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCommitteeEvaluationsDisplayItem = DirectCast(e.Item.DataItem, dtoCommitteeEvaluationsDisplayItem)
            Dim oGeneric As HtmlControl = e.Item.FindControl("SPNstatus")
            Dim oLabel As Label = e.Item.FindControl("LBcommitteeEvaluation")
            Dim hideValues As Boolean = False
            Select Case item.Status
                Case EvaluationStatus.Evaluated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " green")
                Case EvaluationStatus.Evaluating
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " yellow")
                Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " red")
                Case EvaluationStatus.Confirmed
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " blue")
                Case Else
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " gray")
                    hideValues = True
            End Select
            Resource.setLabel_To_Value(oLabel, "EvaluationTranslations.EvaluationStatus." & item.Status.ToString())



            Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")
            oLabel = e.Item.FindControl("LBcommitteeVote")
            If hideValues Then
                oLabel.Text = "--"
            Else
                Select Case CurrentEvaluationType
                    Case EvaluationType.Average
                        oLabel.Text = item.AverageRatingToString()
                    Case EvaluationType.Dss
                        If IsNothing(item.DssEvaluation) OrElse (Not IsNothing(item.DssEvaluation) AndAlso (Not item.DssEvaluation.IsCompleted OrElse Not item.DssEvaluation.IsValid)) Then
                            oLabel.Text = "--"
                        Else
                            Dim oControl As UC_FuzzyNumber = e.Item.FindControl("CTRLfuzzyNumber")
                            oSpan.Visible = True
                            oLabel.Visible = False
                            oControl.InitializeControl(item.DssEvaluation.IsFuzzy, item.DssEvaluation.Ranking, item.DssEvaluation.Value, item.DssEvaluation.ValueFuzzy, GetEvaluatedValue(item.DssEvaluation.Ranking, 4), "")
                        End If
                    Case Else
                        oLabel.Text = item.SumRatingToString()
                End Select
            End If
        End If
    End Sub
    Protected Sub RPTevaluators_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCommitteeEvaluatorEvaluationDisplayItem = DirectCast(e.Item.DataItem, dtoCommitteeEvaluatorEvaluationDisplayItem)
            Dim oGeneric As HtmlControl = e.Item.FindControl("SPNstatus")
            Dim oLabel As Label = e.Item.FindControl("LBevaluatorEvaluation")
            Dim hideValues As Boolean = False
            Select Case item.Status
                Case EvaluationStatus.Evaluated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " green")
                Case EvaluationStatus.Evaluating
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " yellow")
                Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " red")
                Case Else
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " gray")
                    hideValues = True
            End Select
            Resource.setLabel_To_Value(oLabel, "EvaluationTranslations.EvaluationStatus." & item.Status.ToString())

            Dim multi As MultiView = e.Item.FindControl("MLVevaluationComment")
            If String.IsNullOrEmpty(item.Comment) Then
                multi.ActiveViewIndex = 0
            Else
                multi.ActiveViewIndex = 1
                Dim oLiteral = e.Item.FindControl("LTevaluationComment_t")
                Resource.setLiteral(oLiteral)
            End If

            Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")
            oLabel = e.Item.FindControl("LBevaluatorVote")
            oSpan.Visible = False
            oLabel.Visible = True
            If hideValues Then
                oLabel.Text = "--"
            Else
                Select Case CurrentEvaluationType
                    Case EvaluationType.Average
                        oLabel.Text = item.AverageRatingToString()
                    Case EvaluationType.Dss
                        oSpan.Visible = True
                        oLabel.Visible = False
                        Dim oControl As UC_FuzzyNumber = e.Item.FindControl("CTRLfuzzyNumber")
                        Dim message As String = ""

                        If (Not IsNothing(item) AndAlso Not IsNothing(item.DssEvaluation)) Then

                            If Not item.DssEvaluation.IsValid OrElse Not item.DssEvaluation.IsCompleted Then
                                message = "(*)"
                            End If
                            oControl.InitializeControl(item.DssEvaluation.IsFuzzy, item.DssEvaluation.Ranking, item.DssEvaluation.Value, item.DssEvaluation.ValueFuzzy, GetEvaluatedValue(item.DssEvaluation.Ranking, 4) & message, "")
                        End If
                        
                    Case Else
                        oLabel.Text = item.SumRatingToString()
                End Select
            End If
        End If
    End Sub
    Protected Sub RPTcriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoCriterionEvaluatedDisplayItem = DirectCast(e.Item.DataItem, dtoCriterionEvaluatedDisplayItem)
            Dim oLabel As Label = e.Item.FindControl("LBcriterionEvaluation")
            Dim hideValues As Boolean = False
            Dim oGeneric As HtmlControl = e.Item.FindControl("SPNstatus")
            Select Case item.Status
                Case EvaluationStatus.Evaluated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " green")
                Case EvaluationStatus.Evaluating
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " yellow")
                Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " red")
                Case Else
                    oGeneric.Attributes.Add("class", oGeneric.Attributes("class") + " gray")
                    hideValues = True
            End Select
            Resource.setLabel_To_Value(oLabel, "EvaluationTranslations.EvaluationStatus." & item.Status.ToString())

            Dim name As String = "", shortName As String = ""
            Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")
            oLabel = e.Item.FindControl("LBcriterionVote")
            Select Case item.Criterion.Type
                Case CriterionType.StringRange
                    ' oLabel.CssClass = "text"
                    If item.IdOption > 0 AndAlso item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).Any() Then
                        oLabel.Text = item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).FirstOrDefault().Name
                        oLabel.ToolTip = "( " & item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).FirstOrDefault().ValueToString & " )"
                    Else
                        oLabel.Text = "--"
                    End If
                Case CriterionType.DecimalRange, CriterionType.IntegerRange
                    If hideValues Then
                        oLabel.Text = "--"
                    ElseIf IsFuzzyValue(item.DssValue, item.IdCommittee) Then
                        SetDssValue(item.DecimalValue, oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                    Else
                        oLabel.Text = item.DecimalValueToString
                    End If
                Case CriterionType.RatingScale, CriterionType.RatingScaleFuzzy
                    If Not IsNothing(item.DssValue) AndAlso item.DssValue.Error = lm.Comol.Core.Dss.Domain.DssError.None Then
                        SetDssValue(item, IsFuzzyValue(item.DssValue, item.IdCommittee), oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                    Else
                        oLabel.Text = "--"
                    End If
                    'Case 
                    '    If Not IsNothing(item.DssValue) AndAlso item.DssValue.Error = lm.Comol.Core.Dss.Domain.DssError.None Then
                    '        Dim optStart As dtoCriterionOption = item.Criterion.Options.Where(Function(o) o.IdRatingValue = item.DssValue.IdRatingValue).FirstOrDefault()
                    '        Dim optEnd As dtoCriterionOption = item.Criterion.Options.Where(Function(o) o.IdRatingValue = item.DssValue.IdRatingValueEnd).FirstOrDefault()
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
                    '        oSpan.Visible = True
                    '        oLabel.Visible = False
                    '        oControl.InitializeControl(item.DssValue.IsFuzzy, item.DssValue.Value, item.DssValue.ValueFuzzy, item.DssValue.RatingType, name, shortName, endName, endShortName)
                    '    Else
                    '        oLabel.Text = "--"
                    '    End If
                Case CriterionType.Textual
                    If hideValues Then
                        oLabel.Text = "--"
                    Else
                        oLabel.Text = item.StringValue
                    End If
            End Select
            Dim multi As MultiView = e.Item.FindControl("MLVcriterionComment")
            If String.IsNullOrEmpty(item.Comment) Then
                multi.ActiveViewIndex = 0
            Else
                multi.ActiveViewIndex = 1
            End If
        End If
    End Sub
#End Region

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

    Private Sub SetEvaluationStatus(status As EvaluationStatus, oLabel As Label)
       
        Select Case status
            Case EvaluationStatus.Evaluated
                oLabel.CssClass &= " green"
            Case EvaluationStatus.Evaluating
                oLabel.CssClass &= " yellow"
            Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                oLabel.CssClass &= " red"
            Case EvaluationStatus.Confirmed
                oLabel.CssClass &= " blue"
            Case Else
                oLabel.CssClass &= " gray"
        End Select
    End Sub

End Class