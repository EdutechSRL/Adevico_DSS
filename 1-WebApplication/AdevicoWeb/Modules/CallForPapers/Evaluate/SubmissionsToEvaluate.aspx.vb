Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq

Public Class SubmissionsToEvaluateList
    Inherits PageBase
    Implements IViewSubmissionsToEvaluate

#Region "Context"
    Private _Presenter As lm.Comol.Modules.CallForPapers.Presentation.Evaluation.SubmissionsToEvaluatePresenter
    Private ReadOnly Property CurrentPresenter() As lm.Comol.Modules.CallForPapers.Presentation.Evaluation.SubmissionsToEvaluatePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.CallForPapers.Presentation.Evaluation.SubmissionsToEvaluatePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewSubmissionsToEvaluate.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewSubmissionsToEvaluate.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewSubmissionsToEvaluate.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewSubmissionsToEvaluate.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommittee As Long Implements IViewSubmissionsToEvaluate.PreloadIdCommittee
        Get
            If IsNumeric(Me.Request.QueryString("idCommittee")) Then
                Return CLng(Me.Request.QueryString("idCommittee"))
            Else
                Return 0
            End If
        End Get
    End Property

    'Private ReadOnly Property PreloadPageIndex As Integer Implements IViewSubmissionsToEvaluate.PreloadPageIndex
    '    Get
    '        If IsNumeric(Me.Request.QueryString("PageIndex")) Then
    '            Return CInt(Me.Request.QueryString("PageIndex"))
    '        Else
    '            Return 0
    '        End If
    '    End Get
    'End Property
    'Private ReadOnly Property PreloadPageSize As Integer Implements IViewSubmissionsToEvaluate.PreloadPageSize
    '    Get
    '        If IsNumeric(Me.Request.QueryString("PageSize")) Then
    '            Return CInt(Me.Request.QueryString("PageSize"))
    '        Else
    '            Return 50
    '        End If
    '    End Get
    'End Property
    Private ReadOnly Property PreloadOrderBy As SubmissionsOrder Implements IViewSubmissionsToEvaluate.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), SubmissionsOrder.ByUser)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterBy As EvaluationFilterStatus Implements IViewSubmissionsToEvaluate.PreloadFilterBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of EvaluationFilterStatus).GetByString(Request.QueryString("Filter"), EvaluationFilterStatus.All)
        End Get
    End Property
    Private ReadOnly Property PreloadIdSubmitterType As Long Implements IViewSubmissionsToEvaluate.PreloadIdSubmitterType
        Get
            If IsNumeric(Me.Request.QueryString("idType")) Then
                Return CLng(Me.Request.QueryString("idType"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadSearchForName As String Implements IViewSubmissionsToEvaluate.PreloadSearchForName
        Get
            Return Request.QueryString("searchBy")
        End Get
    End Property
    Private ReadOnly Property PreloadFilters As dtoEvaluationsFilters Implements IViewSubmissionsToEvaluate.PreloadFilters
        Get
            Dim dto As New dtoEvaluationsFilters
            With dto
                .Ascending = PreloadAscending
                .OrderBy = PreloadOrderBy
                .SearchForName = PreloadSearchForName
                .Status = PreloadFilterBy
                .IdSubmitterType = PreloadIdSubmitterType
                Dim tStatus As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String)
                For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus))
                    tStatus.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus), name), Me.Resource.getValue("EvaluationTranslations.EvaluationStatus." & name))
                Next
                .TranslationsEvaluationStatus = tStatus
            End With
            Return dto
        End Get
    End Property
    'Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewSubmissionsList.Pager
    '    Get
    '        Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
    '    End Get
    '    Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
    '        Me.ViewState("Pager") = value
    '        Me.PGgridTop.Pager = value
    '        Me.PGgridBottom.Pager = value
    '        Me.DVpagerTop.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
    '        Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
    '    End Set
    'End Property
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewSubmissionsToEvaluate.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return True
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private Property CurrentFilters As dtoEvaluationsFilters Implements IViewSubmissionsToEvaluate.CurrentFilters
        Get
            Dim filter As dtoEvaluationsFilters = ViewStateOrDefault("CurrentFilters", PreloadFilters)
            filter.SearchForName = Me.TXBusername.Text
            If Me.RBLevaluationStatus.SelectedIndex = -1 Then
                filter.Status = EvaluationFilterStatus.All
            Else
                filter.Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of EvaluationFilterStatus).GetByString(Me.RBLevaluationStatus.SelectedValue, EvaluationFilterStatus.All)
            End If
            If Me.DDLsubmitterTypes.SelectedIndex = -1 Then
                filter.IdSubmitterType = -1
            Else
                filter.IdSubmitterType = CLng(DDLsubmitterTypes.SelectedValue)
            End If
            Return filter
        End Get
        Set(value As dtoEvaluationsFilters)
            ViewState("CurrentFilters") = value
            TXBusername.Text = value.SearchForName
        End Set
    End Property
    'Private Property CurrentFilterBy As SubmissionFilterStatus Implements IViewSubmissionsToEvaluate.CurrentFilterBy
    '    Get
    '        Return ViewStateOrDefault("CurrentFilterBy", SubmissionFilterStatus.OnlySubmitted)
    '    End Get
    '    Set(ByVal value As SubmissionFilterStatus)
    '        Me.ViewState("CurrentFilterBy") = value
    '    End Set
    'End Property
    Private Property CurrentOrderBy As SubmissionsOrder Implements IViewSubmissionsToEvaluate.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", SubmissionsOrder.ByUser)
        End Get
        Set(ByVal value As SubmissionsOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property CurrentEvaluationType As EvaluationType Implements IViewSubmissionsToEvaluate.CurrentEvaluationType
        Get
            Return ViewStateOrDefault("CurrentEvaluationType", EvaluationType.Sum)
        End Get
        Set(ByVal value As EvaluationType)
            Me.ViewState("CurrentEvaluationType") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewSubmissionsToEvaluate.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CurrentAscending") = value
        End Set
    End Property
    'Private Property PageSize As Integer Implements IViewSubmissionsToEvaluate.PageSize
    '    Get
    '        Return ViewStateOrDefault("PageSize", 50)
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me.ViewState("PageSize") = value
    '    End Set
    'End Property
    Private Property AllowEvaluate As Boolean Implements IViewSubmissionsToEvaluate.AllowEvaluate
        Get
            Return ViewStateOrDefault("AllowEvaluate", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowEvaluate") = value
        End Set
    End Property
    Private Property AllowExportAll As Boolean Implements IViewSubmissionsToEvaluate.AllowExportAll
        Get
            Return ViewStateOrDefault("AllowExportAll", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportData") = value
            LNBexportSelfCommitteesStatisticsToXLS.Visible = value
            LNBexportSelfCommitteesStatisticsToCsv.Visible = value
        End Set
    End Property
    Private Property AllowExportCurrent As Boolean Implements IViewSubmissionsToEvaluate.AllowExportCurrent
        Get
            Return ViewStateOrDefault("AllowExportCurrent", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportCurrent") = value
            LNBexportSelfCommitteeStatisticsToXLS.Visible = value
            LNBexportSelfCommitteeStatisticsToCsv.Visible = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewSubmissionsToEvaluate.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property CallType As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType Implements IViewSubmissionsToEvaluate.CallType
        Get
            Return ViewStateOrDefault("CallType", lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids)
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewSubmissionsToEvaluate.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewSubmissionsToEvaluate.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Public Property IdCurrentCommittee As Long Implements IViewSubmissionsToEvaluate.IdCurrentCommittee
        Get
            Return ViewStateOrDefault("IdCurrentCommittee", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCurrentCommittee") = value
        End Set
    End Property
    Private Property IdEvaluator As Long Implements IViewSubmissionsToEvaluate.IdEvaluator
        Get
            Return ViewStateOrDefault("IdEvaluator", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdEvaluator") = value
        End Set
    End Property
    Private Property CriteriaCount As Integer Implements IViewSubmissionsToEvaluate.CriteriaCount
        Get
            Return ViewStateOrDefault("CriteriaCount", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("CriteriaCount") = value
        End Set
    End Property
    Private ReadOnly Property AnonymousDisplayname As String Implements IViewSubmissionsToEvaluate.AnonymousDisplayname
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewSubmissionsToEvaluate.UnknownDisplayname
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property
    Private WriteOnly Property AvailableOrderBy As Dictionary(Of SubmissionsOrder, Boolean) Implements IViewSubmissionsToEvaluate.AvailableOrderBy
        Set(value As Dictionary(Of SubmissionsOrder, Boolean))
            HYPorderByUserUp.Visible = value(SubmissionsOrder.ByUser)
            HYPorderByUserDown.Visible = value(SubmissionsOrder.ByUser)
            HYPorderByTypeUp.Visible = value(SubmissionsOrder.ByType)
            HYPorderByTypeDown.Visible = value(SubmissionsOrder.ByType)
            HYPorderByEvaluationPointsUp.Visible = value(SubmissionsOrder.ByEvaluationPoints)
            HYPorderByEvaluationPointsDown.Visible = value(SubmissionsOrder.ByEvaluationPoints)
            HYPorderByEvaluationIndexUp.Visible = value(SubmissionsOrder.ByEvaluationIndex) AndAlso (CurrentOrderBy <> SubmissionsOrder.ByEvaluationIndex OrElse Not CurrentAscending)
            HYPorderByEvaluationIndexDown.Visible = value(SubmissionsOrder.ByEvaluationIndex) AndAlso (CurrentOrderBy <> SubmissionsOrder.ByEvaluationIndex OrElse CurrentAscending)
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
    Private _AllEvaluated As Boolean
    Private _IsFuzzyCommittees As Boolean
    Protected ReadOnly Property CssTableCriteriaCount As String
        Get
            Return IIf(CriteriaCount > 5, "", "noslide")
        End Get
    End Property
    Protected ReadOnly Property CookieName As String
        Get
            Return "SubmissionsToEvaluate"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplaySelfCommitteeStatisticsToken.Message")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplaySelfCommitteeStatisticsToken.Title")
        End Get
    End Property

    Public Property IdAdvCommittee As Long Implements IViewSubmissionsToEvaluate.IdAdvCommittee
        Get
            Dim idComm As Integer = 0
            Try
                idComm = ViewStateOrDefault("IdAdvCommission", 0)
            Catch ex As Exception

            End Try

            If (idComm = 0) Then
                Try
                    idComm = Request.QueryString("AdCId")
                Catch ex As Exception

                End Try

            End If

            Return idComm
        End Get
        Set(value As Long)
            ViewState("IdAdvCommission") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

        If Not Page.IsPostBack Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSubmissionList,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If

        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSubmissionList,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "NoPermission")

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

            .setLinkButton(LNBexportSelfCommitteesStatisticsToXLS, False, True)
            .setLinkButton(LNBexportSelfCommitteesStatisticsToCsv, False, True)
            .setLinkButton(LNBexportSelfCommitteeStatisticsToXLS, False, True)
            .setLinkButton(LNBexportSelfCommitteeStatisticsToCsv, False, True)
            .setLinkButton(LNBexportSelfCommitteeFilteredStatisticsToCsv, False, True)


            .setHyperLink(HYPmanage, CallType.ToString(), False, True)
            .setHyperLink(HYPadvCommission, CallType.ToString(), False, True)

            .setHyperLink(HYPlist, CallType.ToString(), False, True)
            .setLabel(LBevaluationsStatus_t)
            .setLabel(LBcommitteesInfo_t)
            .setLabel(LBiconLegendTop)
            .setLabel(LBlegendTop)
            .setLabel(LBiconLegendBottom)
            DVlegend.Attributes.Add("title", LBiconLegendBottom.ToolTip)
            .setLabel(LBlegendBottom)
            .setLiteral(LTsubmitterType_t)
            .setLiteral(LTsubmitterName_t)
            .setLiteral(LTsubmissionPoints_t)
            .setLiteral(LTactions_t)

            .setLiteral(LTshortCriterionName)
            .setLiteral(LTlongCriterionName)
            .setLabel(LBdashboardCommitteeName)

            .setHyperLink(HYPorderByUserUp, False, True)
            .setHyperLink(HYPorderByUserDown, False, True)
            .setHyperLink(HYPorderByTypeUp, False, True)
            .setHyperLink(HYPorderByTypeDown, False, True)
            .setHyperLink(HYPorderByEvaluationPointsUp, False, True)
            .setHyperLink(HYPorderByEvaluationPointsDown, False, True)
            .setHyperLink(HYPorderByEvaluationIndexUp, False, True)
            .setHyperLink(HYPorderByEvaluationIndexDown, False, True)

            .setLabel(LBsearchEvaluationsFor_t)
            .setLabel(LBevaluationStatusFilter_t)
            .setButton(BTNfindEvaluations, True)
            .setLabel(LBsubmitterType_t)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayEvaluationInfo(endEvaluationOn As Date?) Implements lm.Comol.Modules.CallForPapers.Presentation.Evaluation.IViewSubmissionsToEvaluate.DisplayEvaluationInfo
        LBcommitteeEvaluationEndOn.Visible = endEvaluationOn.HasValue
        If endEvaluationOn.HasValue Then
            Resource.setLabel(LBcommitteesInfo_t)
            'LBcommitteesInfo_t.Text &= " " & String.Format(Resource.getValue("Evaluation.Deadline"), endEvaluationOn.Value.ToShortDateString, endEvaluationOn.Value.ToString("HH:mm"))

            Resource.setLabel(LBcommitteeEvaluationEndOn)
            LBcommitteeEvaluationEndOn.Text = String.Format(LBcommitteeEvaluationEndOn.Text, endEvaluationOn.Value.ToShortDateString, endEvaluationOn.Value.ToString("HH:mm"))
        End If
    End Sub

    Private Sub DisplayEvaluationUnavailable() Implements IViewSubmissionsToEvaluate.DisplayEvaluationUnavailable
        DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("DisplayEvaluationUnavailable")
    End Sub

    Private Sub DisplayNotEvaluationPermission() Implements IViewSubmissionsToEvaluate.DisplayNotEvaluationPermission
        DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("DisplayNotEvaluationPermission")
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewSubmissionsToEvaluate.DisplaySessionTimeout
        DisplaySessionTimeout(lm.Comol.Modules.CallForPapers.Domain.RootObject.ViewSubmissionsToEvaluate(PreloadIdCommittee, CallType, PreloadIdCall, PreloadIdCommunity, PreloadView, PreloadOrderBy, PreloadAscending, PreloadIdSubmitterType, PreloadFilterBy, GetItemEncoded(PreloadSearchForName)))
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewSubmissionsToEvaluate.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.EvaluationSubmissionList,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewSubmissionsToEvaluate.DisplayUnknownCall
        DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewSubmissionsToEvaluate.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType) Implements IViewSubmissionsToEvaluate.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewSubmissionsToEvaluate.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType = lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

#End Region

    Private Sub LoadAvailableStatus(items As List(Of EvaluationFilterStatus), selected As EvaluationFilterStatus) Implements IViewSubmissionsToEvaluate.LoadAvailableStatus
        Me.RBLevaluationStatus.Items.Clear()
        Me.RBLevaluationStatus.Items.AddRange((From s As EvaluationFilterStatus In items Where s <> EvaluationFilterStatus.All Select New ListItem(Resource.getValue("EvaluationFilterStatus." & s.ToString), s.ToString)).OrderBy(Function(l) l.Text).ToArray)

        If Not IsNothing(items) AndAlso items.Contains(EvaluationFilterStatus.All) Then
            Me.RBLevaluationStatus.Items.Insert(0, New ListItem(Resource.getValue("EvaluationFilterStatus." & EvaluationFilterStatus.All.ToString), EvaluationFilterStatus.All.ToString))
        End If

        If Me.RBLevaluationStatus.Items.Count = 0 Then
            Me.RBLevaluationStatus.Items.Add(New ListItem(Resource.getValue("EvaluationFilterStatus." & selected.ToString), CInt(selected)))
        ElseIf Not IsNothing(RBLevaluationStatus.Items.FindByValue(selected.ToString)) Then
            Me.RBLevaluationStatus.SelectedValue = selected.ToString
        Else
            Me.RBLevaluationStatus.SelectedIndex = 0
        End If
        DVstatusfilter.Visible = (items.Count > 1)
    End Sub
    Private Sub LoadAvailableSubmitterTypes(items As List(Of dtoSubmitterType), selected As Long) Implements IViewSubmissionsToEvaluate.LoadAvailableSubmitterTypes
        Me.DDLsubmitterTypes.Items.Clear()
        Me.DDLsubmitterTypes.DataSource = items
        Me.DDLsubmitterTypes.DataTextField = "Name"
        Me.DDLsubmitterTypes.DataValueField = "Id"
        Me.DDLsubmitterTypes.DataBind()
        If Not IsNothing(items) AndAlso items.Count > 0 Then
            Me.DDLsubmitterTypes.Items.Insert(0, New ListItem(Resource.getValue("SubmitterType.All"), -1))
        End If

        If Me.DDLsubmitterTypes.Items.Count = 0 Then
            Me.DDLsubmitterTypes.Items.Insert(0, New ListItem(Resource.getValue("SubmitterType.All"), -1))
        ElseIf Not IsNothing(DDLsubmitterTypes.Items.FindByValue(selected)) Then
            Me.DDLsubmitterTypes.SelectedValue = selected
        Else
            Me.DDLsubmitterTypes.SelectedIndex = 0
        End If
        DVsubmitterType.Visible = (items.Count > 1)
    End Sub
    Private Sub LoadEvaluationData(globalStat As dtoBaseEvaluatorStatistics, committees As List(Of dtoCommitteeEvaluationsInfo), statistics As dtoEvaluatorCommitteeStatistic, idType As Long, status As EvaluationFilterStatus, name As String) Implements IViewSubmissionsToEvaluate.LoadEvaluationData
        Me.MLVstatistics.SetActiveView(VIWstatistics)
        Me.IdCurrentCommittee = statistics.IdCommittee
        Dim idCommittee As Long = IdCurrentCommittee
        If statistics.IdCommittee > 0 Then
            HYPorderByUserUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByUser, True, idType, status, name)
            HYPorderByUserDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByUser, False, idType, status, name)
            HYPorderByTypeUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByType, True, idType, status, name)
            HYPorderByTypeDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByType, False, idType, status, name)
            HYPorderByEvaluationPointsUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationPoints, True, idType, status, name)
            HYPorderByEvaluationPointsDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationPoints, False, idType, status, name)
            HYPorderByEvaluationIndexUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationIndex, True, idType, status, name)
            HYPorderByEvaluationIndexDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(statistics.IdCommittee, CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationIndex, False, idType, status, name)
        Else
            HYPorderByUserUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByUser, True, idType, status, name)
            HYPorderByUserDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByUser, False, idType, status, name)
            HYPorderByTypeUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByType, True, idType, status, name)
            HYPorderByTypeDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByType, False, idType, status, name)
            HYPorderByEvaluationPointsUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationPoints, True, idType, status, name)
            HYPorderByEvaluationPointsDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationPoints, False, idType, status, name)
            HYPorderByEvaluationIndexUp.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationIndex, True, idType, status, name)
            HYPorderByEvaluationIndexDown.NavigateUrl = BaseUrl & RootObject.ViewSubmissionsToEvaluate(CallForPaperType.CallForBids, IdCall, IdCallCommunity, PreLoadedContentView, SubmissionsOrder.ByEvaluationIndex, False, idType, status, name)
        End If
        




        Me.RPTcommittees.DataSource = committees
        Me.RPTcommittees.DataBind()
        InitStackedBar(globalStat, CTRLstackedBar)

        DVexport.Visible = AllowExportAll OrElse AllowExportCurrent
        DVcommittee.Visible = False
        If statistics.IdCommittee > 0 Then
            'Me.DVexport.Attributes.Add("class", IIf((AllowExportAll AndAlso AllowExportCurrent), "ddbuttonlist enabled", "ddbuttonlist"))
            DVcommittee.Visible = True
            Me.LBcommitteeName.Text = statistics.Name

            InitStackedBar(statistics, CTRLcommitteeStackedBar)
            _IsFuzzyCommittees = CurrentEvaluationType = EvaluationType.Dss AndAlso committees.Any(Function(c) c.isFuzzy AndAlso c.IdCommittee.Equals(statistics.IdCommittee))

            RPTcriteria.DataSource = statistics.Criteria
            RPTcriteria.DataBind()
            THcriterionPlaceHolder.Visible = (statistics.Criteria.Count Mod 5) > 0
            THcriterionPlaceHolder.ColSpan = IIf(statistics.Criteria.Count > 5, (statistics.Criteria.Count Mod 5), 5 - (statistics.Criteria.Count))

            _AllEvaluated = Not statistics.Evaluations.Any(Function(e) e.Status = EvaluationStatus.None) AndAlso (Not statistics.Evaluations.Any(Function(e) e.Criteria.Any(Function(c) c.IsValueEmpty OrElse Not c.IsValidForCriterionSaving OrElse Not c.IsValidForEvaluation)))
            RPTevaluations.DataSource = statistics.Evaluations
            RPTevaluations.DataBind()

            RPTlegend.DataSource = statistics.Criteria
            RPTlegend.DataBind()
        End If
    End Sub

    Private Sub SetContainerName(callName As String, endEvaluationOn As Date?) Implements IViewSubmissionsToEvaluate.SetContainerName
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.EvaluationBoard"), IIf(Len(callName) > 70, Left(callName, 70) & "...", callName))
        If endEvaluationOn.HasValue Then
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.EvaluationBoard.ToolTip.Deadline"), endEvaluationOn.Value.ToShortDateString & " " & endEvaluationOn.Value.ToString("HH:mm"), callName)
        Else
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.EvaluationBoard.ToolTip"), callName)
        End If
    End Sub
    Private Sub SetActionUrl(action As lm.Comol.Modules.CallForPapers.Domain.CallStandardAction, url As String, isAdvance As Boolean, advUrl As String) Implements IViewSubmissionsToEvaluate.SetActionUrl
        Select Case action
            Case lm.Comol.Modules.CallForPapers.Domain.CallStandardAction.Manage
                HYPmanage.Visible = True
                HYPmanage.NavigateUrl = BaseUrl & url

                If (isAdvance) Then
                    HYPadvCommission.Visible = isAdvance
                    HYPadvCommission.NavigateUrl = BaseUrl & advUrl
                End If


            Case Else
                HYPlist.Visible = True
                HYPlist.NavigateUrl = BaseUrl & url
        End Select
    End Sub
    Private Function GetItemEncoded(name As String) As String Implements IViewSubmissionsToEvaluate.GetItemEncoded
        Return EncodeSearchBy(name)
    End Function
#Region "Helpers"

    Private Function EncodeSearchBy(search As String) As String
        If String.IsNullOrEmpty(search) OrElse String.IsNullOrEmpty(search.Trim) Then
            Return ""
        Else
            Return Server.UrlEncode(search)
        End If
    End Function
    Private Sub InitStackedBar(item As dtoBaseEvaluatorStatistics, control As UC_StackedBar)

        InitStackedBar(item.GetEvaluationsCount(), item.GetEvaluationsCount(EvaluationStatus.Evaluated), item.GetEvaluationsCount(EvaluationStatus.Evaluating), item.GetEvaluationsCount(EvaluationStatus.None), item.GetEvaluationsCount(EvaluationStatus.Confirmed), control)
    End Sub
    Private Sub InitStackedBar(item As dtoEvaluatorCommitteeStatistic, control As UC_StackedBar)
        InitStackedBar(item.Evaluations.Count, item.GetEvaluationsCount(EvaluationStatus.Evaluated), item.GetEvaluationsCount(EvaluationStatus.Evaluating), item.GetEvaluationsCount(EvaluationStatus.None), item.GetEvaluationsCount(EvaluationStatus.Confirmed), control)
    End Sub
    Private Sub InitStackedBar(count As Long, nEvaluated As Long, nEvaluating As Long, nNotstarted As Long, nConfirmed As Long, control As UC_StackedBar)
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

        Dim confirmed As StackedBarItem = New StackedBarItem()
        confirmed.CssClass = "confirmed"
        confirmed.Title = "Confermate: {0}%" 'Me.Resource.getValue("ProgressTooltip.EvaluationStatus." & EvaluationStatus.None.ToString())
        confirmed.Value = Percentual(nConfirmed, count)

        If notstarted.Value > 0 Then
            notstarted.Value = 100 - evaluated.Value - evaluating.Value - confirmed.Value
        ElseIf evaluating.Value > 0 Then
            evaluating.Value = 100 - evaluated.Value - notstarted.Value - confirmed.Value
        ElseIf evaluated.Value > 0 Then
            evaluated.Value = 100 - notstarted.Value - confirmed.Value
        End If

        control.InitializeControl({evaluated, evaluating, notstarted, confirmed})
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

#Region "Filters"
    Private Sub BTNfindEvaluations_Click(sender As Object, e As System.EventArgs) Handles BTNfindEvaluations.Click
        CurrentPresenter.LoadCommitteData(IdCurrentCommittee, CurrentFilters)
    End Sub
    Private Sub RBLevaluationStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLevaluationStatus.SelectedIndexChanged
        CurrentPresenter.LoadCommitteData(IdCurrentCommittee, CurrentFilters)
    End Sub
    Private Sub DDLsubmitterTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLsubmitterTypes.SelectedIndexChanged
        CurrentPresenter.LoadCommitteData(IdCurrentCommittee, CurrentFilters)
    End Sub
#End Region
#Region "Controls"
    Private Sub RPTcommittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommittees.ItemDataBound
        Dim item As dtoCommitteeEvaluationsInfo = DirectCast(e.Item.DataItem, dtoCommitteeEvaluationsInfo)
        Dim oHyperlink As HyperLink = e.Item.FindControl("HYPcommitteeDashboard")
        Dim oLiteral As Literal = e.Item.FindControl("LTcommitteeName")
        Dim oControl As HtmlGenericControl = e.Item.FindControl("LIcommission")

        If item.IdCommittee = IdCurrentCommittee Then
            oControl.Attributes.Add("class", oControl.Attributes("class") & " active")
            oLiteral.Visible = True
            oLiteral.Text = item.Name
        Else
            oHyperlink.Visible = True
            oHyperlink.NavigateUrl = BaseUrl & item.NavigationUrl
            oHyperlink.Text = item.Name
        End If

        Dim oLabel As Label = e.Item.FindControl("LBcompleted")
        Dim oLabelCount As Label = e.Item.FindControl("LBcompletedCount")
        oLabel.Text = Resource.getValue("EvaluationStatus." & EvaluationStatus.Evaluated.ToString())
        oLabelCount.ToolTip = oLabel.Text
        oLabelCount.Text = item.GetEvaluationsCount(EvaluationStatus.Evaluated)

        oLabel = e.Item.FindControl("LBstarted")
        oLabelCount = e.Item.FindControl("LBstartedCount")
        oLabel.Text = Resource.getValue("EvaluationStatus." & EvaluationStatus.Evaluating.ToString())
        oLabelCount.ToolTip = oLabel.Text
        oLabelCount.Text = item.GetEvaluationsCount(EvaluationStatus.Evaluating)

        oLabel = e.Item.FindControl("LBnotstarted")
        oLabelCount = e.Item.FindControl("LBnotstartedCount")
        oLabel.Text = Resource.getValue("EvaluationStatus." & EvaluationStatus.None.ToString())
        oLabelCount.ToolTip = oLabel.Text
        oLabelCount.Text = item.GetEvaluationsCount(EvaluationStatus.None)


        oLabelCount = e.Item.FindControl("LBconfirmedCount")
        oLabelCount.ToolTip = "Confermati"
        oLabelCount.Text = item.GetEvaluationsCount(EvaluationStatus.Confirmed)

    End Sub
    Private Sub RPTcriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcriteria.ItemDataBound, RPTlegend.ItemDataBound
        Dim item As dtoCriterion = DirectCast(e.Item.DataItem, dtoCriterion)

        Dim oLabel As Label = e.Item.FindControl("LBcriterionName")
        oLabel.ToolTip = item.Name
        oLabel.Text = String.Format(Resource.getValue("CriterionNumber"), (e.Item.ItemIndex + 1).ToString())
    End Sub

    Private Sub RPTevaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluations.ItemDataBound
        Dim item As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation = DirectCast(e.Item.DataItem, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation)

        '<td class="submitternumber"></td>
        Dim oLiteral As Literal = e.Item.FindControl("LTdisplayName")
        If item.Anonymous Then
            oLiteral.Text = Resource.getValue("AnonymousOwnerName")
        Else
            oLiteral.Text = item.DisplayName
        End If

        Dim oGeneric As HtmlGenericControl = e.Item.FindControl("SPNvoteContainer")
        Select Case item.Status
            Case EvaluationStatus.Evaluated
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " green")
            Case EvaluationStatus.Evaluating
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " yellow")
            Case EvaluationStatus.EvaluatorReplacement, EvaluationStatus.Invalidated
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " red")
            Case EvaluationStatus.Confirmed
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " blue")
        End Select


        Dim oLabel As Label = e.Item.FindControl("LBvote")
        Select Case CurrentEvaluationType
            Case EvaluationType.Sum
                oLabel.Text = GetEvaluatedValue(item.SumRating)
            Case EvaluationType.Average
                oLabel.Text = GetEvaluatedValue(item.AverageRating)
            Case EvaluationType.Dss
                If Not IsNothing(item.DssRating) AndAlso item.DssRating.IsCompleted AndAlso item.Status <> EvaluationStatus.None AndAlso Not item.Criteria.Any(Function(c) Not c.IsValidForEvaluation) Then
                    SetDssValue(item.DssRating, _IsFuzzyCommittees, oLabel, e.Item.FindControl("SPNfuzzy"), e.Item.FindControl("CTRLfuzzyNumber"))
                    'If _AllEvaluated Then
                    '    oLabel.Text = GetEvaluatedValue(item.DssRating, 4)
                    '    oLabel.ToolTip = item.DssRating
                    'Else
                    '    oLabel.Text = GetEvaluatedValue(item.DssRating, 4) & "(*)"
                    '    oLabel.ToolTip = Resource.getValue("DssWaitingEvaluations")
                    'End If
                Else
                    oLabel.Text = "//"
                    oLabel.ToolTip = Resource.getValue("DssWaitingEvaluations")
                End If
        End Select

        Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDcriterionPlaceHolder")
        oTableCell.Visible = (item.Criteria.Count Mod 5) > 0
        oTableCell.ColSpan = IIf(item.Criteria.Count > 5, (item.Criteria.Count Mod 5), 5 - (item.Criteria.Count))

        Dim oHyperlink As HyperLink = e.Item.FindControl("HYPevaluateSubmission")
        oHyperlink.NavigateUrl = PageUtility.BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.EvaluateSubmission(IdCall, IdCallCommunity, item.Id)
        oHyperlink.Visible = AllowEvaluate
        Resource.setHyperLink(oHyperlink, True, True)

        oHyperlink = e.Item.FindControl("HYPviewEvaluation")
        oHyperlink.NavigateUrl = PageUtility.BaseUrl & lm.Comol.Modules.CallForPapers.Domain.RootObject.ViewSingleEvaluation(item.Id, item.IdSubmission, IdCall, IdCallCommunity, Me.IdAdvCommittee)
        oHyperlink.Visible = Not AllowEvaluate
        Resource.setHyperLink(oHyperlink, True, True)
    End Sub
    Protected Sub RPTcriteriaEvaluated_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim item As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated = DirectCast(e.Item.DataItem, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated)

        Dim oLabel As Label = e.Item.FindControl("LBcriterionEvaluated")
        Dim oLit As Literal = e.Item.FindControl("LTbool")

        oLit.Visible = False

        If item.IsValueEmpty Then
            oLabel.Text = "&nbsp;"
        Else

            Select Case item.Criterion.Type
                Case CriterionType.DecimalRange
                    If _IsFuzzyCommittees Then
                        SetDssValue(item.DecimalValue, oLabel, e.Item.FindControl("CTRLfuzzyNumber"))
                    Else
                        oLabel.Text = GetEvaluatedValue(item.DecimalValue)
                    End If

                Case CriterionType.IntegerRange
                    If _IsFuzzyCommittees Then
                        SetDssValue(Math.Floor(item.DecimalValue), oLabel, e.Item.FindControl("CTRLfuzzyNumber"))
                    Else
                        oLabel.Text = Math.Floor(item.DecimalValue)
                    End If
                Case CriterionType.StringRange
                    If item.IdOption > 0 AndAlso item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).Any() Then
                        oLabel.Text = item.Criterion.Options.Where(Function(o) o.Id = item.IdOption).FirstOrDefault().Name
                    Else
                        oLabel.Text = "--"
                    End If
                Case CriterionType.RatingScale
                    SetDssValue(item, _IsFuzzyCommittees, oLabel, e.Item.FindControl("CTRLfuzzyNumber"))
                Case CriterionType.RatingScaleFuzzy
                    SetDssValue(item, _IsFuzzyCommittees, oLabel, e.Item.FindControl("CTRLfuzzyNumber"))
                Case CriterionType.Textual
                    oLabel.Text = item.StringValue
                Case CriterionType.Boolean
                    oLabel.Visible = False
                    oLit.Visible = True

                    Dim isChecked As Boolean = item.DecimalValue > 0
                    Dim val As Integer = 0

                    Dim CheckedText As String = ""
                    If (isChecked) Then
                        val = 1
                        CheckedText = Resource.getValue("CriterionType.Boolean.True")
                    Else
                        CheckedText = Resource.getValue("CriterionType.Boolean.False")
                    End If

                    If String.IsNullOrEmpty(CheckedText) Then
                        If (isChecked) Then
                            CheckedText = "Superato"
                        Else
                            CheckedText = "Non superato"
                        End If
                    End If

                    oLit.Text = String.Format(oLit.Text, val, CheckedText)

            End Select
        End If
    End Sub
    Private Sub SetDssValue(item As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated, displayAsFuzzy As Boolean, oLabel As Label, oControl As UC_FuzzyNumber)
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
                        oControl.Visible = True
                        oLabel.Visible = False
                        oControl.InitializeControl(item.DssValue.IsFuzzy OrElse displayAsFuzzy, item.DssValue.Value, item.DssValue.ValueFuzzy, name, shortName)
                    End If
                Case Else
                    oControl.Visible = True
                    oLabel.Visible = False
                    oControl.InitializeControl(item.DssValue.IsFuzzy OrElse displayAsFuzzy, item.DssValue.Value, item.DssValue.ValueFuzzy, item.DssValue.RatingType, name, shortName, endName, endShortName)
            End Select
        Else
            oLabel.Text = "--"
        End If
    End Sub
    Private Sub SetDssValue(rating As dtoDssRating, displayAsFuzzy As Boolean, oLabel As Label, oGenericControl As HtmlGenericControl, oControl As UC_FuzzyNumber)
        Dim name As String = "", shortName As String = ""
        If Not IsNothing(rating) AndAlso rating.IsValid Then
            ' If Not IsNothing(rating) AndAlso rating.IsValid AndAlso isCompleted Then
            If Not rating.IsFuzzy AndAlso Not displayAsFuzzy Then
                oLabel.Text = GetEvaluatedValue(rating.Value)
                oGenericControl.Visible = False
            Else
                oGenericControl.Visible = True
                oLabel.Visible = False
                oControl.InitializeControl(True, rating.Ranking, rating.Value, rating.ValueFuzzy, GetEvaluatedValue(rating.Ranking, 4), "")
                'oControl.InitializeControl(True, rating.Value, rating.ValueFuzzy, rating.Value, "")
            End If
        Else
            oLabel.Text = "--"
        End If
    End Sub
    Private Sub SetDssValue(value As Decimal, oLabel As Label, oControl As UC_FuzzyNumber)
        oControl.Visible = True
        oLabel.Visible = False
        oControl.InitializeControl(True, value)
    End Sub
#End Region

    Private Function GetEvaluatedValue(sum As Double, Optional decimals As Integer = 2) As String
        Dim fractional As Double = sum - Math.Floor(sum)
        If (fractional = 0) Then
            Return String.Format("{0:N0}", sum)
        Else
            Return String.Format("{0:N" & decimals.ToString() & "}", sum)
        End If
    End Function

#Region "Export"
    Private Sub LNBexportSelfCommitteesStatisticsToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportSelfCommitteesStatisticsToXLS.Click
        ExportEvaluationsData(0, IdEvaluator, False, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportSelfCommitteesStatisticsToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportSelfCommitteesStatisticsToCsv.Click
        ExportEvaluationsData(0, IdEvaluator, False, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportSelfCommitteeFilteredStatisticsToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportSelfCommitteeFilteredStatisticsToCsv.Click
        ExportEvaluationsData(IdCurrentCommittee, IdEvaluator, True, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportSelfCommitteeStatisticsToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportSelfCommitteeStatisticsToXLS.Click
        ExportEvaluationsData(IdCurrentCommittee, IdEvaluator, False, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportSelfCommitteeStatisticsToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportSelfCommitteeStatisticsToCsv.Click
        ExportEvaluationsData(IdCurrentCommittee, IdEvaluator, False, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub ExportEvaluationsData(ByVal idCommittee As Long, ByVal idEvaluator As Long, applyFilters As Boolean, exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Response.Clear()
        Response.AppendCookie(New HttpCookie(CookieName, HDNdownloadTokenValue.Value))

        Dim clientFileName As String = ""

        Dim data As DateTime = DateTime.Now
        If idCommittee = 0 Then
            clientFileName = Resource.getValue("SelfCommitteesStatisticsFileName")
            If String.IsNullOrEmpty(clientFileName) Then
                clientFileName = "{1}_{2}_{3}_EvaluationsOfCall_{3}"
            End If
        Else
            clientFileName = Resource.getValue("SelfCommitteeStatisticsFileName")
            If String.IsNullOrEmpty(clientFileName) Then
                clientFileName = "{1}_{2}_{3}_EvaluationsOfCommittee_{3}"
            End If
        End If

        Try

            Response.AddHeader("Content-Disposition", "attachment; filename=" & CurrentPresenter.GetFileName(clientFileName, idCommittee) & "." & exportType.ToString)
            Response.Charset = ""
            Response.ContentEncoding = System.Text.Encoding.Default

            Dim translations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations))
                translations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations), name), Me.Resource.getValue("EvaluationTranslations." & name))
            Next
            Dim tStatus As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String)
            For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus))
                tStatus.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus), name), Me.Resource.getValue("EvaluationTranslations.EvaluationStatus." & name))
            Next
            Select Case exportType
                Case Helpers.Export.ExportFileType.xml
                    Response.ContentType = "application/ms-excel"
                Case Else
                    Response.Charset = "utf-8"
                    Response.ContentType = "text/csv"
            End Select
            Response.BinaryWrite(Response.ContentEncoding.GetPreamble())
            Response.Write(CurrentPresenter.ExportTo(Me.CurrentFilters, idCommittee, idEvaluator, applyFilters, exportType, translations, tStatus))
        Catch de As Exception

        End Try
        Response.End()
    End Sub
#End Region

    Private Sub SubmissionsToEvaluateList_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

    Private Sub LKBupdate_Click(sender As Object, e As EventArgs) Handles LKBupdate.Click
        BindDati()
    End Sub
End Class