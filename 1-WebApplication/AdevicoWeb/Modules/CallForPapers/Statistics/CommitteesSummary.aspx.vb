Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.ActionDataContract
Imports System.Linq

Public Class CommitteesSummary
    Inherits PageBase
    Implements IViewCommitteesSummary


#Region "Context"
    Private _Presenter As CommitteesSummaryPresenter
    Private ReadOnly Property CurrentPresenter() As CommitteesSummaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommitteesSummaryPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property IdCallAdvCommission As Long Implements IViewBaseSummary.IdCallAdvCommission
        Get
            Return Request.QueryString("acId")
        End Get
    End Property
    Private ReadOnly Property Portalname As String Implements IViewCommitteesSummary.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommittee As Long Implements IViewCommitteesSummary.PreloadIdCommittee
        Get
            If IsNumeric(Me.Request.QueryString("idCommittee")) Then
                Return CLng(Me.Request.QueryString("idCommittee"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewCommitteesSummary.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewCommitteesSummary.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewCommitteesSummary.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewCommitteesSummary.PreloadPageIndex
        Get
            If IsNumeric(Me.Request.QueryString("PageIndex")) Then
                Return CInt(Me.Request.QueryString("PageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewCommitteesSummary.PreloadPageSize
        Get
            If IsNumeric(Me.Request.QueryString("PageSize")) Then
                Return CInt(Me.Request.QueryString("PageSize"))
            Else
                Return 50
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdSubmitterType As Long Implements IViewBaseSummary.PreloadIdSubmitterType
        Get
            If IsNumeric(Me.Request.QueryString("idType")) Then
                Return CLng(Me.Request.QueryString("idType"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadOrderBy As SubmissionsOrder Implements IViewCommitteesSummary.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), SubmissionsOrder.ByEvaluationIndex)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterBy As EvaluationFilterStatus Implements IViewCommitteesSummary.PreloadFilterBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of EvaluationFilterStatus).GetByString(Request.QueryString("Filter"), EvaluationFilterStatus.All)
        End Get
    End Property
    Private ReadOnly Property PreloadSearchForName As String Implements IViewCommitteesSummary.PreloadSearchForName
        Get
            Return Request.QueryString("SearchForName")
        End Get
    End Property
    Private ReadOnly Property PreloadFilters As dtoEvaluationsFilters Implements IViewCommitteesSummary.PreloadFilters
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
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewCommitteesSummary.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return True
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private Property IdCall As Long Implements IViewCommitteesSummary.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewCommitteesSummary.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewCommitteesSummary.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewCommitteesSummary.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property CurrentFilters As dtoEvaluationsFilters Implements IViewCommitteesSummary.CurrentFilters
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
    Private Property CurrentFilterBy As EvaluationFilterStatus Implements IViewCommitteesSummary.CurrentFilterBy
        Get
            Return ViewStateOrDefault("CurrentFilterBy", EvaluationFilterStatus.All)
        End Get
        Set(ByVal value As EvaluationFilterStatus)
            Me.ViewState("CurrentFilterBy") = value
        End Set
    End Property
    Private Property CurrentOrderBy As SubmissionsOrder Implements IViewCommitteesSummary.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", SubmissionsOrder.ByEvaluationIndex)
        End Get
        Set(ByVal value As SubmissionsOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewCommitteesSummary.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property PageSize As Integer Implements IViewCommitteesSummary.PageSize
        Get
            Return ViewStateOrDefault("PageSize", 50)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("PageSize") = value
        End Set
    End Property
    Private Property CurrentPageIndex As Integer Implements IViewCommitteesSummary.CurrentPageIndex
        Get
            Return ViewStateOrDefault("CurrentPageIndex", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentPageIndex") = value
        End Set
    End Property
    Private Property CommitteesCount As Integer Implements IViewCommitteesSummary.CommitteesCount
        Get
            Return ViewStateOrDefault("CommitteesCount", 1)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CommitteesCount") = value
        End Set
    End Property
    Private Property CurrentCommittees As List(Of dtoCommittee) Implements IViewCommitteesSummary.CurrentCommittees
        Get
            Return ViewStateOrDefault("CurrentCommittees", New List(Of dtoCommittee))
        End Get
        Set(ByVal value As List(Of dtoCommittee))
            Me.ViewState("CurrentCommittees") = value
        End Set
    End Property
    Private Property AllowExportAll As Boolean Implements IViewCommitteesSummary.AllowExportAll
        Get
            Return ViewStateOrDefault("AllowExportAll", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportAll") = value
            'LNBexportAllCommitteesSummaryToXLS.Visible = value
            'LNBexportAllEvaluationsSummaryData.Visible = value
            LNBexportAllCommitteesSummaryToCsv.Visible = value
            LNBexportAllEvaluationsSummaryDataToCsv.Visible = value
            'LNBexportFullEvaluationsSummaryDataToXml.Visible = value
            LNBexportFullEvaluationsSummaryDataToCsv.Visible = value
        End Set
    End Property
    Private Property AllowExportCurrent As Boolean Implements IViewCommitteesSummary.AllowExportCurrent
        Get
            Return ViewStateOrDefault("AllowExportCurrent", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportCurrent") = value
            'LNBexportFilteredCommitteesSummaryToXLS.Visible = value
            LNBexportFilteredCommitteesSummaryToCsv.Visible = value
        End Set
    End Property
    Private Property AllowBackToSummary As Boolean Implements IViewCommitteesSummary.AllowBackToSummary
        Get
            Return ViewStateOrDefault("AllowBackToSummary", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowBackToSummary") = value
            Me.HYPbackToSummaryIndex.Visible = value
            'Dim filters As dtoEvaluationsFilters = CurrentFilters
            'Me.HYPbackToSummaryIndex.NavigateUrl = BaseUrl & RootObject.EvaluationsSummary(IdCall, IdCallCommunity, PreloadView, 0, filters.IdSubmitterType, filters.Status, filters.OrderBy, filters.Ascending, CurrentPageIndex, PageSize, Server.UrlEncode(filters.SearchForName))
        End Set
    End Property
    Private ReadOnly Property AnonymousDisplayname As String Implements IViewCommitteesSummary.AnonymousDisplayname
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewCommitteesSummary.UnknownDisplayname
        Get
            Return Resource.getValue("UnknownDisplayname")
        End Get
    End Property
    Private Property AvailableOrderBy As Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder, Boolean) Implements IViewBaseSummary.AvailableOrderBy
        Get
            Return ViewStateOrDefault("AvailableOrderBy", New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder, Boolean))
        End Get
        Set(value As Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionsOrder, Boolean))
            ViewState("AvailableOrderBy") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewBaseSummary.Pager
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
    Private Property CurrentEvaluationType As EvaluationType Implements IViewBaseSummary.CurrentEvaluationType
        Get
            Return ViewStateOrDefault("CurrentEvaluationType", EvaluationType.Sum)
        End Get
        Set(ByVal value As EvaluationType)
            Me.ViewState("CurrentEvaluationType") = value
        End Set
    End Property
    Private Property CallUseFuzzy As Boolean Implements IViewBaseSummary.CallUseFuzzy
        Get
            Return ViewStateOrDefault("CallUseFuzzy", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CallUseFuzzy") = value
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
    Protected ReadOnly Property CssTableCommitteeCount As String
        Get
            Return IIf(CommitteesCount > 5, "", "noslide")
        End Get
    End Property
    Protected ReadOnly Property CookieName As String
        Get
            Return "CommitteesSummary"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayCommitteesSummaryToken.Message")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayCommitteesSummaryToken.Title")
        End Get
    End Property
#End Region

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
            .setLiteral(LTcommitteesSliderTop)
            .setLiteral(LTcommitteesSliderBottom)
            .setLiteral(LTsubmitterName_t)
            .setLiteral(LTsubmitterType_t)
            .setLiteral(LTsubmissionPoints_t)
            .setLiteral(LTevaluationStatus_t)

            .setHyperLink(HYPmanage, CallType.ToString(), False, True)
            .setHyperLink(HYPlist, CallType.ToString(), False, True)
            .setHyperLink(HYPbackToSummaryIndex, False, True)

            .setLinkButton(LNBexportAllCommitteesSummaryToXLS, True, True)
            .setLinkButton(LNBexportFilteredCommitteesSummaryToXLS, True, True)
            .setLinkButton(LNBexportAllEvaluationsSummaryData, True, True)

            .setLinkButton(LNBexportAllCommitteesSummaryToCsv, False, True)
            .setLinkButton(LNBexportFilteredCommitteesSummaryToCsv, False, True)
            .setLinkButton(LNBexportAllEvaluationsSummaryDataToCsv, False, True)
            .setLinkButton(LNBexportFullEvaluationsSummaryDataToXml, False, True)
            .setLinkButton(LNBexportFullEvaluationsSummaryDataToCsv, False, True)

            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setLabel(LBsearchEvaluationsFor_t)
            .setLabel(LBevaluationStatusFilter_t)
            .setButton(BTNfindEvaluations, True)
            .setLabel(LBsubmitterType_t)

            .setHyperLink(HYPorderByEvaluationIndexUp, False, True)
            .setHyperLink(HYPorderByEvaluationIndexDown, False, True)
            .setHyperLink(HYPorderByTypeUp, False, True)
            .setHyperLink(HYPorderByTypeDown, False, True)
            .setHyperLink(HYPorderByEvaluationPointsUp, False, True)
            .setHyperLink(HYPorderByEvaluationPointsDown, False, True)
            .setHyperLink(HYPorderByEvaluationIndexUp, False, True)
            .setHyperLink(HYPorderByEvaluationIndexDown, False, True)
            .setHyperLink(HYPorderByEvaluationStatusUp, False, True)
            .setHyperLink(HYPorderByEvaluationStatusDown, False, True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
#Region "Display"
    Private Sub DisplayDssWarning(lastUpdate As Date?, isCompleted As Boolean) Implements IViewBaseSummary.DisplayDssWarning
        CTRLdssMessage.Visible = True
        If lastUpdate = Nothing Then
            CTRLdssMessage.InitializeControl(Resource.getValue("DisplayDssWarning.NoDate"), Helpers.MessageType.alert)
        Else
            CTRLdssMessage.InitializeControl(String.Format(Resource.getValue("DisplayDssWarning.Date.isCompleted." & isCompleted.ToString), lastUpdate.Value.ToString()), IIf(isCompleted, Helpers.MessageType.success, Helpers.MessageType.alert))
        End If
    End Sub
    Private Sub HideDssWarning() Implements IViewBaseSummary.HideDssWarning
        CTRLdssMessage.Visible = False
    End Sub
    Private Sub DisplayNoEvaluationsFound() Implements IViewCommitteesSummary.DisplayNoEvaluationsFound
        Me.DVexport.Visible = AllowExportAll
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayNoEvaluationsFound")
    End Sub
    Private Sub SetContainerName(callName As String, endEvaluationOn As Date?) Implements IViewCommitteesSummary.SetContainerName
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.CommitteesSummary"), IIf(Len(callName) > 70, Left(callName, 70) & "...", callName))
        If endEvaluationOn.HasValue Then
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.CommitteesSummary.ToolTip.Deadline"), endEvaluationOn.Value.ToShortDateString & " " & endEvaluationOn.Value.ToString("HH:mm"), callName)
        Else
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.CommitteesSummary.ToolTip"), callName)
        End If
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long) Implements IViewBaseSummary.DisplayUnknownCall
        Me.DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayUnknownCall." & CallType.ToString)

        SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewEvaluationsSummary)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewCommitteesSummary.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.CommitteesSummary(PreloadIdCall, PreloadIdCommunity, PreloadView, PreloadIdSubmitterType, PreloadFilterBy, PreloadOrderBy, PreloadAscending, PreloadPageIndex, PreloadPageSize, EncodeSearchBy(PreloadSearchForName)))
    End Sub
    Private Sub DisplaySessionTimeout(ByVal url As String) Implements IViewBaseSummary.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub DisplayEvaluationUnavailable() Implements IViewBaseSummary.DisplayEvaluationUnavailable
        Me.DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("EvaluationSummary.DisplayEvaluationUnavailable")
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewCommitteesSummary.DisplayNoPermission
        DVexport.Visible = False
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayEvaluationInfo(endEvaluationOn As Date?) Implements IViewCommitteesSummary.DisplayEvaluationInfo
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleCallForPaper.ActionType) Implements IViewCommitteesSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRequestForMembership.ActionType) Implements IViewCommitteesSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewCommitteesSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType) Implements IViewCommitteesSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Private Sub LoadAvailableStatus(items As List(Of EvaluationFilterStatus), selected As EvaluationFilterStatus) Implements IViewBaseSummary.LoadAvailableStatus
        RBLevaluationStatus.Items.Clear()
        RBLevaluationStatus.Items.AddRange((From s As EvaluationFilterStatus In items Where s <> EvaluationFilterStatus.All Select New ListItem(Resource.getValue("EvaluationFilterStatus." & s.ToString), CInt(s))).OrderBy(Function(l) l.Text).ToArray)

        If Not IsNothing(items) AndAlso items.Contains(EvaluationFilterStatus.All) Then
            RBLevaluationStatus.Items.Insert(0, New ListItem(Resource.getValue("EvaluationFilterStatus." & EvaluationFilterStatus.All.ToString), CInt(EvaluationFilterStatus.All)))
        End If

        If Me.RBLevaluationStatus.Items.Count = 0 Then
            RBLevaluationStatus.Items.Add(New ListItem(Resource.getValue("EvaluationFilterStatus." & selected.ToString), CInt(selected)))
        ElseIf Not IsNothing(RBLevaluationStatus.Items.FindByValue(selected)) Then
            RBLevaluationStatus.SelectedValue = CInt(selected)
        Else
            RBLevaluationStatus.SelectedIndex = 0
        End If
        DVstatusfilter.Visible = (items.Count > 1)
    End Sub
    Private Sub LoadAvailableSubmitterTypes(items As List(Of dtoSubmitterType), selected As Long) Implements IViewBaseSummary.LoadAvailableSubmitterTypes
        DDLsubmitterTypes.Items.Clear()
        DDLsubmitterTypes.DataSource = items
        DDLsubmitterTypes.DataTextField = "Name"
        DDLsubmitterTypes.DataValueField = "Id"
        DDLsubmitterTypes.DataBind()
        If Not IsNothing(items) AndAlso items.Count > 0 Then
            DDLsubmitterTypes.Items.Insert(0, New ListItem(Resource.getValue("SubmitterType.All"), -1))
        End If

        If DDLsubmitterTypes.Items.Count = 0 Then
            DDLsubmitterTypes.Items.Insert(0, New ListItem(Resource.getValue("SubmitterType.All"), -1))
        ElseIf Not IsNothing(DDLsubmitterTypes.Items.FindByValue(selected)) Then
            DDLsubmitterTypes.SelectedValue = selected
        Else
            DDLsubmitterTypes.SelectedIndex = 0
        End If
        DVsubmitterType.Visible = (items.Count > 1)
    End Sub
#End Region
    Private Sub DisplaySingleCommittee(idCommittee As Long, idCall As Long, idCommunity As Integer, filters As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluationsFilters) Implements IViewCommitteesSummary.DisplaySingleCommittee
        PageUtility.RedirectToUrl(RootObject.CommitteeSummary(idCommittee, idCall, idCommunity, PreloadView, filters.IdSubmitterType, filters.Status, filters.OrderBy, filters.Ascending, PreloadPageIndex, PreloadPageSize, EncodeSearchBy(filters.SearchForName)))
    End Sub
    Private Sub LoadEvaluations(committees As List(Of dtoCommittee), statistics As List(Of dtoCommitteesSummaryItem)) Implements IViewCommitteesSummary.LoadEvaluations
        If IsNothing(statistics) OrElse statistics.Count = 0 Then
            DisplayNoEvaluationsFound()
            DVexport.Visible = AllowExportAll
        Else
            DVexport.Visible = AllowExportAll OrElse AllowExportCurrent
            MLVstatistics.SetActiveView(VIWstatistics)
            DDLcommittees.Items.Clear()

            Dim name As String = Resource.getValue("CommitteeNumber")
            Dim itemIndex As Integer = 1
            For Each c As dtoCommittee In committees
                Me.DDLcommittees.Items.Add(New ListItem(String.Format(name, itemIndex), c.Id))
            Next

            DDLcommittees.DataSource = committees
            DDLcommittees.DataTextField = "Name"
            DDLcommittees.DataValueField = "Id"
            DDLcommittees.DataBind()
            If committees.Count > 1 Then
                Me.DDLcommittees.Items.Insert(0, New ListItem(Resource.getValue("AllCommittees"), -1))
            ElseIf committees.Count = 1 Then
                Me.DDLcommittees.Enabled = False
            End If

            Me.RPTcommittees.DataSource = committees
            Me.RPTcommittees.DataBind()

            THcommitteePlaceHolder.Visible = committees.Count < 5
            If committees.Count < 5 Then
                THcommitteePlaceHolder.ColSpan = 5 - committees.Count
            End If

            'THcommitteePlaceHolder.Visible = (committees.Count Mod 5) < 0
            'THcommitteePlaceHolder.ColSpan = IIf(committees.Count < 5, (committees.Count Mod 5), 5 - (committees.Count))

            Dim isVisible As Boolean = (statistics.Count > 1)
            HYPorderByEvaluationIndexUp.Visible = isVisible
            HYPorderByEvaluationIndexDown.Visible = isVisible
            HYPorderByTypeUp.Visible = isVisible
            HYPorderByTypeDown.Visible = isVisible
            HYPorderByEvaluationPointsUp.Visible = isVisible
            HYPorderByEvaluationPointsDown.Visible = isVisible
            HYPorderByEvaluationIndexUp.Visible = isVisible
            HYPorderByEvaluationIndexDown.Visible = isVisible
            HYPorderByEvaluationStatusUp.Visible = isVisible
            HYPorderByEvaluationStatusDown.Visible = isVisible

            Dim idCallItem As Long = IdCall
            Dim idCommunity As Integer = IdCallCommunity
            Dim filter As dtoEvaluationsFilters = CurrentFilters
            Dim v As CallStatusForSubmitters = PreloadView
            Dim searchBy As String = EncodeSearchBy(filter.SearchForName)

            HYPorderByEvaluationIndexUp.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByUser, True, 0, PageSize, searchBy)
            HYPorderByEvaluationIndexDown.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByUser, False, 0, PageSize, searchBy)
            HYPorderByTypeUp.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByType, True, 0, PageSize, searchBy)
            HYPorderByTypeDown.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByType, False, 0, PageSize, searchBy)
            HYPorderByEvaluationPointsUp.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationPoints, True, 0, PageSize, searchBy)
            HYPorderByEvaluationPointsDown.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationPoints, False, 0, PageSize, searchBy)
            HYPorderByEvaluationIndexUp.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationIndex, True, 0, PageSize, searchBy)
            HYPorderByEvaluationIndexDown.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationIndex, False, 0, PageSize, searchBy)
            HYPorderByEvaluationStatusUp.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationStatus, True, 0, PageSize, searchBy)
            HYPorderByEvaluationStatusDown.NavigateUrl = BaseUrl & RootObject.CommitteesSummary(idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationStatus, False, 0, PageSize, searchBy)

            If CurrentEvaluationType = EvaluationType.Dss Then
                _AllEvaluated = Not statistics.Any(Function(i) IsNothing(i.DssEvaluation) OrElse Not i.DssEvaluation.IsCompleted)
            End If

            RPTevaluations.DataSource = statistics
            RPTevaluations.DataBind()
        End If
    End Sub

    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewCommitteesSummary.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.Visible = True
                Me.HYPlist.NavigateUrl = BaseUrl & url
            Case CallStandardAction.Manage
                Me.HYPmanage.Visible = True
                Me.HYPmanage.NavigateUrl = BaseUrl & url
        End Select
    End Sub
    Private Function GetItemEncoded(name As String) As String Implements IViewCommitteesSummary.GetItemEncoded
        Return EncodeSearchBy(name)
    End Function
    Private Sub SetBackToSummary(url As String) Implements IViewCommitteesSummary.SetBackToSummary
        If String.IsNullOrEmpty(url) Then
            HYPbackToSummaryIndex.Visible = False
        Else
            HYPbackToSummaryIndex.NavigateUrl = BaseUrl & url
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

            If notstarted.Value > 0 Then
                notstarted.Value = 100 - evaluated.Value - evaluating.Value
            ElseIf evaluating.Value > 0 Then
                evaluating.Value = 100 - evaluated.Value
            End If

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
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Me.CurrentPresenter.LoadEvaluations(IdCall, CurrentEvaluationType, IdCallCommunity, CurrentCommittees, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        Me.CurrentPresenter.LoadEvaluations(IdCall, CurrentEvaluationType, IdCallCommunity, CurrentCommittees, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub BTNfindEvaluations_Click(sender As Object, e As System.EventArgs) Handles BTNfindEvaluations.Click
        LoadEvaluations()
    End Sub
    Private Sub RBLevaluationStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLevaluationStatus.SelectedIndexChanged
        LoadEvaluations()
    End Sub
    Private Sub DDLsubmitterTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLsubmitterTypes.SelectedIndexChanged
        LoadEvaluations()
    End Sub
    Private Sub LoadEvaluations()
        Dim filters As dtoEvaluationsFilters = Me.CurrentFilters
        filters.SearchForName = Me.TXBusername.Text
        filters.Ascending = True

        filters.Status = Me.CurrentFilterBy
        'Select Case filters.Status
        '    Case EvaluationFilterStatus.All, EvaluationFilterStatus.AllValid, EvaluationFilterStatus.Evaluated, EvaluationFilterStatus.Evaluating
        '        filters.OrderBy = SubmissionsOrder.ByEvaluationIndex
        '    Case Else
        '        filters.OrderBy = SubmissionsOrder.ByUser
        '        filters.Ascending = True
        'End Select
        Me.CurrentPresenter.LoadEvaluations(IdCall, CurrentEvaluationType, IdCallCommunity, CurrentCommittees, filters, 0, PreloadPageSize)
    End Sub
    Private Sub RPTcommittees_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcommittees.ItemDataBound
        Dim item As dtoCommittee = DirectCast(e.Item.DataItem, dtoCommittee)

        Dim oLabel As Label = e.Item.FindControl("LBcommitteeName")
        oLabel.ToolTip = item.Name
        oLabel.Text = String.Format(Resource.getValue("CommitteeNumber"), (e.Item.ItemIndex + 1).ToString())
    End Sub
    Private Sub RPTevaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluations.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCommitteesSummaryItem = DirectCast(e.Item.DataItem, dtoCommitteesSummaryItem)

            Dim oLabel As Label = e.Item.FindControl("LBvote")
            Select Case CurrentEvaluationType
                Case EvaluationType.Average
                    oLabel.Text = dto.AverageRatingToString()
                Case EvaluationType.Dss
                    SetDssValue(_AllEvaluated, dto.DssEvaluation, CallUseFuzzy, oLabel, e.Item.FindControl("SPNfuzzy"), e.Item.FindControl("CTRLfuzzyNumber"))
                Case EvaluationType.Sum
                    oLabel.Text = dto.SumRatingToString()
            End Select

            Dim oControl As UC_StackedBar = e.Item.FindControl("CTRLevaluationStackedBar")
            Me.InitStackedBar(dto, oControl)

            Dim oLiteral As Literal = e.Item.FindControl("LTCommitteeDetailsStatus")
            Resource.setLiteral(oLiteral)

            Dim oCell As HtmlTableCell = e.Item.FindControl("TDcommitteePlaceHolder")
            oCell.Visible = CommitteesCount < 5
            If CommitteesCount < 5 Then
                oCell.ColSpan = 5 - CommitteesCount
            End If

            oCell = e.Item.FindControl("TDcommissionstatsPlaceHolder")
            oCell.Visible = CommitteesCount < 5
            If CommitteesCount < 5 Then
                oCell.ColSpan = 5 - CommitteesCount
            End If

        End If
    End Sub
    Private Sub SetDssValue(isCompleted As Boolean, rating As DssCallEvaluation, displayAsFuzzy As Boolean, oLabel As Label, oGenericControl As HtmlGenericControl, oControl As UC_FuzzyNumber)
        Dim name As String = "", shortName As String = ""
        If Not IsNothing(rating) AndAlso rating.IsValid AndAlso isCompleted Then
            If Not rating.IsFuzzy AndAlso Not displayAsFuzzy Then
                oLabel.Text = GetEvaluatedValue(rating.Value, 4)
                oGenericControl.Visible = False
            Else
                oGenericControl.Visible = True
                oLabel.Visible = False
                oControl.InitializeControl(True, rating.Ranking, rating.Value, rating.ValueFuzzy, GetEvaluatedValue(rating.Ranking, 4), "")
            End If
        Else
            oLabel.Text = "--"
            oLabel.ToolTip = Resource.getValue("DssWaitingEvaluations")
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
    Protected Sub RPTcommitteeEvaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCommitteeDisplayItem = DirectCast(e.Item.DataItem, dtoCommitteeDisplayItem)
            Dim oCell As HtmlTableCell = e.Item.FindControl("TDcommitteeEvaluation")

            Dim oLabel As Label = e.Item.FindControl("LBcommitteeEvaluation")
            If dto.Evaluations.Count = 0 Then
                oLabel.Text = "-"
            Else
                Select Case CurrentEvaluationType
                    Case EvaluationType.Average
                        If dto.isEmpty Then
                            oLabel.Text = "-"
                        Else
                            oLabel.Text = dto.AverageRatingToString()
                        End If

                    Case EvaluationType.Dss
                        If dto.isEmpty Then
                            oLabel.Text = "-"
                        Else
                            SetDssValue(_AllEvaluated OrElse (Not IsNothing(dto.DssEvaluation) AndAlso dto.DssEvaluation.IsCompleted AndAlso dto.DssEvaluation.IsValid), dto.DssEvaluation, CallUseFuzzy, oLabel, e.Item.FindControl("SPNfuzzy"), e.Item.FindControl("CTRLfuzzyNumber"))
                        End If
                        'If _AllEvaluated Then
                        '    oLabel.Text = dto.DssRatingToString()
                        '    oLabel.ToolTip = oLabel.Text
                        'Else
                        '    oLabel.Text = dto.DssRatingToString(4) & "(*)"
                        '    oLabel.ToolTip = Resource.getValue("DssWaitingEvaluations")
                        'End If
                    Case EvaluationType.Sum
                        If dto.isEmpty Then
                            oLabel.Text = "-"
                        Else
                            oLabel.Text = dto.SumRatingToString()
                        End If
                End Select
            End If
           
        End If
    End Sub
    Protected Sub RPTcommitteesStatus_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCommitteeDisplayItem = DirectCast(e.Item.DataItem, dtoCommitteeDisplayItem)
            Dim oControl As UC_StackedBar = e.Item.FindControl("CTRLevaluationStackedBar")
            Me.InitStackedBar(dto, oControl)

        End If
    End Sub

#End Region

#Region "Export Data"

#Region "Button"
    Private Sub LNBexportAllEvaluationsSummaryData_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryData.Click
        ExportEvaluations(SummaryType.Evaluations, ItemsToExport.All, ExportData.Fulldata, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportAllCommitteesSummaryToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllCommitteesSummaryToXLS.Click
        ExportEvaluations(SummaryType.Committees, ItemsToExport.All, ExportData.DisplayData, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportFilteredCommitteesSummaryToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportFilteredCommitteesSummaryToXLS.Click
        ExportEvaluations(SummaryType.Committees, ItemsToExport.Filtered, ExportData.DisplayData, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportFullEvaluationsSummaryDataToXml_Click(sender As Object, e As System.EventArgs) Handles LNBexportFullEvaluationsSummaryDataToXml.Click
        ExportEvaluations(SummaryType.Evaluations, ItemsToExport.All, ExportData.FulldataToAnalyze, Helpers.Export.ExportFileType.xml)
    End Sub

    Private Sub LNBexportAllEvaluationsSummaryDataToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryDataToCsv.Click
        ExportEvaluations(SummaryType.Evaluations, ItemsToExport.All, ExportData.Fulldata, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportAllCommitteesSummaryToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllCommitteesSummaryToCsv.Click
        ExportEvaluations(SummaryType.Committees, ItemsToExport.All, ExportData.DisplayData, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportFilteredCommitteesSummaryToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportFilteredCommitteesSummaryToCsv.Click
        ExportEvaluations(SummaryType.Committees, ItemsToExport.Filtered, ExportData.DisplayData, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportFullEvaluationsSummaryDataToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportFullEvaluationsSummaryDataToCsv.Click
        ExportEvaluations(SummaryType.Evaluations, ItemsToExport.All, ExportData.FulldataToAnalyze, Helpers.Export.ExportFileType.csv)
    End Sub
#End Region

    Private Sub ExportEvaluations(type As SummaryType, items As ItemsToExport, data As ExportData, fileType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Response.Clear()
        Response.AppendCookie(New HttpCookie(Me.CookieName, HDNdownloadTokenValue.Value)) '; //downloadTokenValue will have been provided in the form submit via the hidden input field
        Dim fileName As String = Resource.getValue(type.ToString() & "." & items.ToString & "." & data.ToString)
        If String.IsNullOrEmpty(fileName) Then
            fileName = GetFileName(fileType)
        Else
            fileName = CurrentPresenter.GetFileName(fileName, items, data) & "." & fileType.ToString
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
                Case Helpers.Export.ExportFileType.xls
                    Response.ContentType = "application/ms-excel"
                Case Helpers.Export.ExportFileType.xml
                    Response.ContentType = "application/ms-excel"
                Case Helpers.Export.ExportFileType.csv
                    Response.Charset = "utf-8"
                    Response.ContentType = "text/csv"
                Case Else
                    Response.Charset = "utf-8"
                    Response.ContentType = "text/csv"
            End Select
            Response.BinaryWrite(Response.ContentEncoding.GetPreamble())
            Response.Write(CurrentPresenter.ExportTo(CurrentFilters, type, items, data, fileType, translations, tStatus))
        Catch de As Exception

        End Try
        Response.End()
    End Sub
    Private Function GetFileName(ByVal type As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType) As String
        Dim filename As String = Resource.getValue("ExportGenericEvaluations")
        Dim oDate As DateTime = DateTime.Now
        If String.IsNullOrEmpty(filename) Then
            filename = "{0}_{1}_{2}_CallEvaluations"
        End If
        Return String.Format(filename, oDate.Year, oDate.Month, oDate.Day) & "." & type.ToString
    End Function
#End Region

    Private Sub ListCalls_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

    Private Function EncodeSearchBy(search As String) As String
        If String.IsNullOrEmpty(search) OrElse String.IsNullOrEmpty(search.Trim) Then
            Return ""
        Else
            Return Server.UrlEncode(search)
        End If
    End Function

    Private Sub DDLcommittees_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLcommittees.SelectedIndexChanged
        DisplaySingleCommittee(Me.DDLcommittees.SelectedValue, IdCall, IdCallCommunity, CurrentFilters)
    End Sub

    Public Sub RedirectToAdvance(CallId As Long) Implements IViewCommitteesSummary.RedirectToAdvance
        Response.Redirect(BaseUrl & RootObject.AdvStepsEdit(CallId))
    End Sub
End Class