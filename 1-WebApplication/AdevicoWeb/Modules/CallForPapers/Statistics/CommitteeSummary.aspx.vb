Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.ActionDataContract
Imports System.Linq

Public Class CommitteeSummary
    Inherits PageBase
    Implements IViewCommitteeSummary

#Region "Context"
    Private _Presenter As CommitteeSummaryPresenter
    Private ReadOnly Property CurrentPresenter() As CommitteeSummaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommitteeSummaryPresenter(Me.PageUtility.CurrentContext, Me)
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
    Private ReadOnly Property Portalname As String Implements IViewCommitteeSummary.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommittee As Long Implements IViewCommitteeSummary.PreloadIdCommittee
        Get
            If IsNumeric(Me.Request.QueryString("idCommittee")) Then
                Return CLng(Me.Request.QueryString("idCommittee"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewCommitteeSummary.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewCommitteeSummary.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
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
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewCommitteeSummary.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewCommitteeSummary.PreloadPageIndex
        Get
            If IsNumeric(Me.Request.QueryString("PageIndex")) Then
                Return CInt(Me.Request.QueryString("PageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewCommitteeSummary.PreloadPageSize
        Get
            If IsNumeric(Me.Request.QueryString("PageSize")) Then
                Return CInt(Me.Request.QueryString("PageSize"))
            Else
                Return 50
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadOrderBy As SubmissionsOrder Implements IViewCommitteeSummary.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), SubmissionsOrder.ByEvaluationIndex)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterBy As EvaluationFilterStatus Implements IViewCommitteeSummary.PreloadFilterBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of EvaluationFilterStatus).GetByString(Request.QueryString("Filter"), EvaluationFilterStatus.All)
        End Get
    End Property
    Private ReadOnly Property PreloadSearchForName As String Implements IViewCommitteeSummary.PreloadSearchForName
        Get
            Return Request.QueryString("SearchForName")
        End Get
    End Property
    Private ReadOnly Property PreloadFilters As dtoEvaluationsFilters Implements IViewCommitteeSummary.PreloadFilters
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
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewCommitteeSummary.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return True
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private Property IdCall As Long Implements IViewCommitteeSummary.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewCommitteeSummary.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewCommitteeSummary.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewCommitteeSummary.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property CurrentIdCommittee As Long Implements IViewCommitteeSummary.CurrentIdCommittee
        Get
            Return ViewStateOrDefault("CurrentIdCommittee", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentIdCommittee") = value
        End Set
    End Property
    Private Property CurrentFilters As dtoEvaluationsFilters Implements IViewCommitteeSummary.CurrentFilters
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
    Private Property CurrentFilterBy As EvaluationFilterStatus Implements IViewCommitteeSummary.CurrentFilterBy
        Get
            Return ViewStateOrDefault("CurrentFilterBy", EvaluationFilterStatus.All)
        End Get
        Set(ByVal value As EvaluationFilterStatus)
            Me.ViewState("CurrentFilterBy") = value
        End Set
    End Property
    Private Property CurrentOrderBy As SubmissionsOrder Implements IViewCommitteeSummary.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", SubmissionsOrder.ByEvaluationIndex)
        End Get
        Set(ByVal value As SubmissionsOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewCommitteeSummary.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property PageSize As Integer Implements IViewCommitteeSummary.PageSize
        Get
            Return ViewStateOrDefault("PageSize", 50)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("PageSize") = value
        End Set
    End Property
    Private Property CurrentPageIndex As Integer Implements IViewCommitteeSummary.CurrentPageIndex
        Get
            Return ViewStateOrDefault("CurrentPageIndex", 0)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentPageIndex") = value
        End Set
    End Property
    Private Property EvaluatorsCount As Integer Implements IViewCommitteeSummary.EvaluatorsCount
        Get
            Return ViewStateOrDefault("EvaluatorsCount", 1)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("EvaluatorsCount") = value
        End Set
    End Property

    Private Property AllowHideComments As Boolean Implements IViewCommitteeSummary.AllowHideComments
        Get
            Return ViewStateOrDefault("AllowHideComments", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowHideComments") = value
            HYPcloseComments.Visible = value
        End Set
    End Property
    Private Property AllowExportAll As Boolean Implements IViewCommitteeSummary.AllowExportAll
        Get
            Return ViewStateOrDefault("AllowExportAll", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportAll") = value
            'LNBexportAllEvaluationsSummaryData.Visible = value
            LNBexportAllEvaluationsSummaryDataToCsv.Visible = value
            'LNBexportFullEvaluationsSummaryDataToXml.Visible = value
            LNBexportFullEvaluationsSummaryDataToCsv.Visible = value
        End Set
    End Property
    Private Property AllowExportCurrent As Boolean Implements IViewCommitteeSummary.AllowExportCurrent
        Get
            Return ViewStateOrDefault("AllowExportCurrent", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExportCurrent") = value
            'LNBexportAllCommitteeSummaryToXLS.Visible = value
            'LNBexportFilteredCommitteeSummaryToXLS.Visible = value
            LNBexportAllCommitteeSummaryToCsv.Visible = value
            LNBexportFilteredCommitteeSummaryToCsv.Visible = value
        End Set
    End Property
    Private Property AllowBackToSummary As Boolean Implements IViewCommitteeSummary.AllowBackToSummary
        Get
            Return ViewStateOrDefault("AllowBackToSummary", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowBackToSummary") = value
            Me.HYPbackToSummaryIndex.Visible = value
            Dim filters As dtoEvaluationsFilters = CurrentFilters
            'Me.HYPbackToSummaryIndex.NavigateUrl = BaseUrl & RootObject.EvaluationsSummary(IdCall, IdCallCommunity, PreLoadedContentView, 0, filters.IdSubmitterType, filters.Status, filters.OrderBy, filters.Ascending, CurrentPageIndex, PageSize, Server.UrlEncode(filters.SearchForName))
        End Set
    End Property
    Private ReadOnly Property AnonymousDisplayname As String Implements IViewCommitteeSummary.AnonymousDisplayname
        Get
            Return Resource.getValue("AnonymousOwnerName")
        End Get
    End Property
    Private ReadOnly Property UnknownDisplayname As String Implements IViewCommitteeSummary.UnknownDisplayname
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
    Private _IsFuzzyCommittee As Boolean
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
    Protected ReadOnly Property CssTableEvaluatorsCount As String
        Get
            Return IIf(EvaluatorsCount > 5, "", "noslide")
        End Get
    End Property
    Protected ReadOnly Property CookieName As String
        Get
            Return "CommitteeSummary"
        End Get
    End Property
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayCommitteeSummaryToken.Message")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayCommitteeSummaryToken.Title")
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
            .setLiteral(LTevaluatorsSliderTop)
            .setLiteral(LTevaluatorsSliderBottom)
            .setLiteral(LTsubmitterName_t)
            .setLiteral(LTsubmitterType_t)
            .setLiteral(LTsubmissionPoints_t)

            LTsubmissionPointsSingle_t.Text = LTsubmissionPoints_t.Text
            LTsubmitterNameSingle_t.Text = LTsubmitterName_t.Text
            LTsubmitterTypeSingle_t.Text = LTsubmitterType_t.Text


            .setHyperLink(HYPmanage, CallType.ToString(), False, True)
            .setHyperLink(HYPlist, CallType.ToString(), False, True)
            .setHyperLink(HYPbackToSummaryIndex, False, True)


            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setLabel(LBsearchEvaluationsFor_t)
            .setLabel(LBevaluationStatusFilter_t)
            .setButton(BTNfindEvaluations, True)
            .setLabel(LBsubmitterType_t)

            .setHyperLink(HYPcloseComments, False, True)
            .setLinkButton(LNBexportAllCommitteeSummaryToCsv, False, True)
            .setLinkButton(LNBexportAllCommitteeSummaryToXLS, False, True)
            .setLinkButton(LNBexportFilteredCommitteeSummaryToXLS, False, True)
            .setLinkButton(LNBexportFilteredCommitteeSummaryToCsv, False, True)
            .setLinkButton(LNBexportAllEvaluationsSummaryData, False, True)

            .setLinkButton(LNBexportAllCommitteeSummaryToCsv, False, True)
            .setLinkButton(LNBexportFilteredCommitteeSummaryToCsv, False, True)
            .setLinkButton(LNBexportAllEvaluationsSummaryDataToCsv, False, True)
            .setLinkButton(LNBexportFullEvaluationsSummaryDataToCsv, False, True)
            .setLinkButton(LNBexportFullEvaluationsSummaryDataToXml, False, True)
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
    Private Sub DisplayNoEvaluationsFound() Implements IViewCommitteeSummary.DisplayNoEvaluationsFound
        Me.DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayNoEvaluationsFound")
    End Sub
    Private Sub SetContainerName(callName As String, endEvaluationOn As Date?) Implements IViewCommitteeSummary.SetContainerName
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.CommitteeSummary"), IIf(Len(callName) > 70, Left(callName, 70) & "...", callName))
        If endEvaluationOn.HasValue Then
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.CommitteeSummary.ToolTip.Deadline"), endEvaluationOn.Value.ToShortDateString & " " & endEvaluationOn.Value.ToString("HH:mm"), callName)
        Else
            Master.ServiceTitleToolTip = String.Format(Resource.getValue("serviceTitle.CommitteeSummary.ToolTip"), callName)
        End If
    End Sub
    Private Sub SetCommitteeName(name As String) Implements IViewCommitteeSummary.SetCommitteeName
        Master.ServiceTitle = String.Format(Resource.getValue("serviceTitle.CommitteeSummary.CommitteeName"), name)
    End Sub
    Private Sub DisplayUnknownCall(idCommunity As Integer, idModule As Integer, idCall As Long) Implements IViewBaseSummary.DisplayUnknownCall
        Me.DVexport.Visible = False
        Me.MLVstatistics.SetActiveView(VIWnoItems)
        Me.LBnoEvaluations.Text = Resource.getValue("DisplayUnknownCall." & CallType.ToString)

        SendUserAction(idCommunity, idModule, idCall, ModuleCallForPaper.ActionType.ViewEvaluationsSummary)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewCommitteeSummary.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.CommitteeSummary(PreloadIdCommittee, PreloadIdCall, PreloadIdCommunity, PreloadView, PreloadIdSubmitterType, PreloadFilterBy, PreloadOrderBy, PreloadAscending, PreloadPageIndex, PreloadPageSize, EncodeSearchBy(PreloadSearchForName)))
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
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewCommitteeSummary.DisplayNoPermission
        DVexport.Visible = False
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayEvaluationInfo(endEvaluationOn As Date?) Implements IViewCommitteeSummary.DisplayEvaluationInfo
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleCallForPaper.ActionType) Implements IViewCommitteeSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRequestForMembership.ActionType) Implements IViewCommitteeSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewCommitteeSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType) Implements IViewCommitteeSummary.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub LoadAvailableStatus(items As List(Of EvaluationFilterStatus), selected As EvaluationFilterStatus) Implements IViewBaseSummary.LoadAvailableStatus
        Me.RBLevaluationStatus.Items.Clear()
        Me.RBLevaluationStatus.Items.AddRange((From s As EvaluationFilterStatus In items Where s <> EvaluationFilterStatus.All Select New ListItem(Resource.getValue("EvaluationFilterStatus." & s.ToString), CInt(s))).OrderBy(Function(l) l.Text).ToArray)

        If Not IsNothing(items) AndAlso items.Contains(EvaluationFilterStatus.All) Then
            Me.RBLevaluationStatus.Items.Insert(0, New ListItem(Resource.getValue("EvaluationFilterStatus." & EvaluationFilterStatus.All.ToString), CInt(EvaluationFilterStatus.All)))
        End If

        If Me.RBLevaluationStatus.Items.Count = 0 Then
            Me.RBLevaluationStatus.Items.Add(New ListItem(Resource.getValue("EvaluationFilterStatus." & selected.ToString), CInt(selected)))
        ElseIf Not IsNothing(RBLevaluationStatus.Items.FindByValue(selected)) Then
            Me.RBLevaluationStatus.SelectedValue = CInt(selected)
        Else
            Me.RBLevaluationStatus.SelectedIndex = 0
        End If
        DVstatusfilter.Visible = (items.Count > 1)
    End Sub
    Private Sub LoadAvailableSubmitterTypes(items As List(Of dtoSubmitterType), selected As Long) Implements IViewBaseSummary.LoadAvailableSubmitterTypes
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
#End Region
    Private Sub DisplayCommitteesSummary(idCall As Long, idCommunity As Integer, filters As lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluationsFilters) Implements IViewCommitteeSummary.DisplayCommitteesSummary
        PageUtility.RedirectToUrl(RootObject.CommitteesSummary(idCall, idCommunity, PreLoadedContentView, PreloadIdSubmitterType, filters.Status, filters.OrderBy, filters.Ascending, PreloadPageIndex, PreloadPageSize, EncodeSearchBy(filters.SearchForName)))
    End Sub
    Private Sub LoadEvaluations(idCommittee As Long, committees As List(Of dtoCommittee), evaluators As List(Of dtoBaseCommitteeMember), statistics As List(Of dtoCommitteeSummaryItem)) Implements IViewCommitteeSummary.LoadEvaluations
        If IsNothing(statistics) OrElse statistics.Count = 0 OrElse evaluators.Count = 0 Then
            Me.DisplayNoEvaluationsFound()
            Me.DVexport.Visible = False
        Else
            Me.DVexport.Visible = AllowExportAll OrElse AllowExportCurrent
            Me.MLVstatistics.SetActiveView(VIWstatistics)
            Me.DDLcommittees.Items.Clear()

            Dim name As String = Resource.getValue("CommitteeNumber")
            Dim itemIndex As Integer = 1
            For Each c As dtoCommittee In committees
                Me.DDLcommittees.Items.Add(New ListItem(String.Format(name, itemIndex), c.Id))
            Next

            Me.DDLcommittees.DataSource = committees
            Me.DDLcommittees.DataTextField = "Name"
            Me.DDLcommittees.DataValueField = "Id"
            Me.DDLcommittees.DataBind()
            Dim multiple As Boolean = (committees.Count > 1)

            TRmultipleCommittees.Visible = multiple
            Me.THpoints.Visible = Not multiple
            Me.THsubmittername.Visible = Not multiple
            Me.THsubmitternumber.Visible = Not multiple
            Me.THsubmittertype.Visible = Not multiple
            If committees.Count > 1 Then
                Me.DDLcommittees.Items.Insert(0, New ListItem(Resource.getValue("AllCommittees"), -1))
            ElseIf committees.Count = 1 Then
                Me.DDLcommittees.Enabled = False
            End If
            Me.DDLcommittees.SelectedValue = idCommittee


            Me.RPTevaluators.DataSource = evaluators
            Me.RPTevaluators.DataBind()

            THevaluatorPlaceHolder.Visible = evaluators.Count < 5
            If evaluators.Count < 5 Then
                THevaluatorPlaceHolder.ColSpan = 5 - evaluators.Count
            End If


            Dim isVisible As Boolean = (statistics.Count > 1)
            HYPorderByEvaluationIndexUp.Visible = isVisible
            HYPorderByEvaluationIndexDown.Visible = isVisible
            HYPorderByTypeUp.Visible = isVisible
            HYPorderByTypeDown.Visible = isVisible
            HYPorderByEvaluationPointsUp.Visible = isVisible
            HYPorderByEvaluationPointsDown.Visible = isVisible
            HYPorderByEvaluationIndexUp.Visible = isVisible
            HYPorderByEvaluationIndexDown.Visible = isVisible
            Dim idCallItem As Long = IdCall
            Dim idCommunity As Integer = IdCallCommunity
            Dim filter As dtoEvaluationsFilters = CurrentFilters
            Dim v As CallStatusForSubmitters = PreloadView
            Dim searchBy As String = EncodeSearchBy(filter.SearchForName)

            HYPorderByEvaluationIndexUp.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByUser, True, 0, PageSize, searchBy)
            HYPorderByEvaluationIndexDown.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByUser, False, 0, PageSize, searchBy)
            HYPorderByTypeUp.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByType, True, 0, PageSize, searchBy)
            HYPorderByTypeDown.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByType, False, 0, PageSize, searchBy)
            HYPorderByEvaluationPointsUp.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationPoints, True, 0, PageSize, searchBy)
            HYPorderByEvaluationPointsDown.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationPoints, False, 0, PageSize, searchBy)
            HYPorderByEvaluationIndexUp.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationIndex, True, 0, PageSize, searchBy)
            HYPorderByEvaluationIndexDown.NavigateUrl = BaseUrl & RootObject.CommitteeSummary(idCommittee, idCallItem, idCommunity, v, filter.IdSubmitterType, filter.Status, SubmissionsOrder.ByEvaluationIndex, False, 0, PageSize, searchBy)

            If CurrentEvaluationType = EvaluationType.Dss Then
                _AllEvaluated = Not statistics.Any(Function(i) IsNothing(i.DssEvaluation) OrElse (Not i.DssEvaluation.IsCompleted))
                _IsFuzzyCommittee = CallUseFuzzy OrElse committees.Any(Function(c) c.Id = idCommittee AndAlso c.UseDss AndAlso c.MethodSettings.IsFuzzyMethod)
            End If
            Me.RPTevaluations.DataSource = statistics
            Me.RPTevaluations.DataBind()
        End If
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewCommitteeSummary.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.Visible = True
                Me.HYPlist.NavigateUrl = BaseUrl & url
            Case CallStandardAction.Manage
                Me.HYPmanage.Visible = True
                Me.HYPmanage.NavigateUrl = BaseUrl & url
        End Select
    End Sub
    Private Function GetItemEncoded(name As String) As String Implements IViewCommitteeSummary.GetItemEncoded
        Return EncodeSearchBy(name)
    End Function
    Private Sub SetBackToSummary(url As String) Implements IViewCommitteeSummary.SetBackToSummary
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
        Me.CurrentPresenter.LoadEvaluations(CurrentIdCommittee, CurrentEvaluationType, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        Me.CurrentPresenter.LoadEvaluations(CurrentIdCommittee, CurrentEvaluationType, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
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
        Me.CurrentPresenter.LoadEvaluations(CurrentIdCommittee, CurrentEvaluationType, filters, 0, PreloadPageSize)
    End Sub
    Private Sub RPTevaluators_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluators.ItemDataBound
        Dim item As dtoBaseCommitteeMember = DirectCast(e.Item.DataItem, dtoBaseCommitteeMember)

        Dim oLabel As Label = e.Item.FindControl("LBevaluatorName")
        oLabel.ToolTip = item.Surname
        oLabel.Text = item.Name(0).ToString.ToUpper() & ". " & item.Surname
    End Sub
    Private Sub RPTevaluations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTevaluations.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCommitteeSummaryItem = DirectCast(e.Item.DataItem, dtoCommitteeSummaryItem)
            Dim oLiteral As Literal = e.Item.FindControl("LTCommitteeDetailsStatus")
            Resource.setLiteral(oLiteral)

            Dim oLabel As Label = e.Item.FindControl("LBvote")
          
            Select Case CurrentEvaluationType
                Case EvaluationType.Average
                    oLabel.Text = dto.AverageRatingToString()
                Case EvaluationType.Dss
                    If dto.Status = EvaluationStatus.None Then
                        oLabel.Text = "--"
                    Else
                        Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")
                        SetDssValue(_AllEvaluated, dto.DssEvaluation, CallUseFuzzy, oLabel, e.Item.FindControl("SPNfuzzy"), e.Item.FindControl("CTRLfuzzyNumber"))
                    End If

                Case EvaluationType.Sum
                    oLabel.Text = dto.SumRatingToString()
            End Select


            Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDevaluatorPlaceHolder")
            Dim count As Integer = EvaluatorsCount
            oTableCell.Visible = count < 5
            If count < 5 Then
                oTableCell.ColSpan = 5 - count
            End If
            oTableCell = e.Item.FindControl("TDcriteriaHeader")
            oTableCell.ColSpan = 4 + count + IIf(count < 5, 5 - count, 0)

            oLabel = e.Item.FindControl("LBcriteriaHeader")
            Resource.setLabel(oLabel)
        End If
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
                End Select

                If dto.Status = EvaluationStatus.EvaluatorReplacement OrElse dto.Status = EvaluationStatus.Invalidated Then
                    If dto.Values.Where(Function(v) Not v.IsValueEmpty).Any() Then
                        oLabel.Visible = False
                        oHyperLink.Visible = True
                        oHyperLink.Text = "//"
                        oHyperLink.ToolTip = Resource.getValue("ViewEvaluation")
                        ' oHyperLink.NavigateUrl = BaseUrl & RootObject.ViewSingleEvaluation(dto.IdEvaluation, dto.IdSubmission, IdCall, IdCallCommunity)
                        oHyperLink.NavigateUrl = BaseUrl & RootObject.ViewUserEvaluations(dto.IdEvaluator, dto.IdSubmission, IdCall, IdCallCommunity, Me.IdCallAdvCommission)
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
                    Select Case CurrentEvaluationType
                        Case EvaluationType.Average
                            oHyperLink.Text = dto.AverageRatingToString
                        Case EvaluationType.Dss
                            If IsNothing(dto.DssEvaluation) Then
                                oLabel.Text = "--"
                                oLabel.Visible = True
                                oHyperLink.Visible = False
                            Else
                                SetDssValue(_AllEvaluated, dto.DssEvaluation, CallUseFuzzy, oHyperLink, e.Item.FindControl("SPNfuzzy"), e.Item.FindControl("CTRLfuzzyNumber"))
                            End If
                            'If _AllEvaluated Then
                            '    oHyperLink.Text = dto.DssRankingToString(4)
                            'Else
                            '    oHyperLink.Text = dto.DssRankingToString(4) & "(*)"
                            'End If
                        Case Else
                            oHyperLink.Text = dto.SumRatingToString
                    End Select


                  
                    oHyperLink.ToolTip = Resource.getValue("ViewEvaluation")
                    ' oHyperLink.NavigateUrl = BaseUrl & RootObject.ViewSingleEvaluation(dto.IdEvaluation, dto.IdSubmission, IdCall, IdCallCommunity)
                    oHyperLink.NavigateUrl = BaseUrl & RootObject.ViewUserEvaluations(dto.IdEvaluator, dto.IdSubmission, IdCall, IdCallCommunity, Me.IdCallAdvCommission)
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
            End If

        End If
    End Sub
    Protected Sub RPTcriteria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCriterionSummaryItem = DirectCast(e.Item.DataItem, dtoCriterionSummaryItem)

            Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDcriteriaPlaceHolder")
            Dim count As Integer = EvaluatorsCount
            oTableCell.Visible = count < 5
            If count < 5 Then
                oTableCell.ColSpan = 5 - count
            End If
            Dim oLabel As Label = e.Item.FindControl("LBcriterionEvaluated")
            Select Case CurrentEvaluationType
                Case EvaluationType.Dss
                Case EvaluationType.Average
                    oLabel.Text = dto.AverageRatingToString()
                Case Else
                    oLabel.Text = dto.SumRatingToString()
            End Select

            Dim oLiteral As Literal = e.Item.FindControl("LTcriterionName")
            oLiteral.Text = String.Format(Resource.getValue("CriterionName"), dto.Name)

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
            If dto.Criterion.Status = EvaluationStatus.EvaluatorReplacement OrElse dto.Criterion.Status = EvaluationStatus.Invalidated Then
                oLabel.Text = "//"
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " gray")
            ElseIf dto.Criterion.Status = EvaluationStatus.None OrElse dto.Criterion.IsValueEmpty Then
                oLabel.Text = "--"
                oGeneric.Attributes.Add("class", oGeneric.Attributes("class") & " gray")
            Else
                Dim name As String = "", shortName As String = ""
                Dim oSpan As HtmlGenericControl = e.Item.FindControl("SPNfuzzyNumber")
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
                        If _IsFuzzyCommittee Then
                            SetDssValue(dto.Criterion.DecimalValue, oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                        Else
                            oLabel.Text = dto.Criterion.DecimalValueToString
                        End If

                    Case CriterionType.RatingScale, CriterionType.RatingScaleFuzzy
                        SetDssValue(dto.Criterion, _IsFuzzyCommittee, oLabel, oSpan, e.Item.FindControl("CTRLfuzzyNumber"))
                    Case CriterionType.Textual
                        oLabel.Text = dto.Criterion.StringValue
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
    Private Sub SetDssValue(value As Decimal, oLabel As Label, oSpan As HtmlGenericControl, oControl As UC_FuzzyNumber)
        oSpan.Visible = True
        oLabel.Visible = False
        oControl.InitializeControl(True, value)
    End Sub
    Private Function GetEvaluatedValue(sum As Double, Optional decimals As Integer = 2) As String
        Dim fractional As Double = sum - Math.Floor(sum)
        If (fractional = 0) Then
            Return String.Format("{0:N0}", sum)
        Else
            Return String.Format("{0:N" & decimals.ToString() & "}", sum)
        End If
    End Function
    Private Sub DDLcommittees_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLcommittees.SelectedIndexChanged
        Dim idCommittee As Long = CLng(DDLcommittees.SelectedValue)
        If idCommittee < 0 Then
            DisplayCommitteesSummary(IdCall, IdCallCommunity, CurrentFilters)
        Else
            Dim filters As dtoEvaluationsFilters = CurrentFilters
            PageUtility.RedirectToUrl(RootObject.CommitteeSummary(idCommittee, IdCall, IdCallCommunity, PreLoadedContentView, filters.IdSubmitterType, filters.Status, filters.OrderBy, filters.Ascending, PreloadPageIndex, PreloadPageSize, EncodeSearchBy(filters.SearchForName)))
        End If
    End Sub
#End Region

#Region "Export Data"

#Region "Button"
    Private Sub LNBexportAllEvaluationsSummaryData_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryData.Click
        ExportEvaluations(SummaryType.Evaluations, ItemsToExport.All, ExportData.Fulldata, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportAllCommitteeSummaryToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllCommitteeSummaryToXLS.Click
        ExportEvaluations(SummaryType.Committee, ItemsToExport.All, ExportData.DisplayData, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportFilteredCommitteeSummaryToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportFilteredCommitteeSummaryToXLS.Click
        ExportEvaluations(SummaryType.Committee, ItemsToExport.Filtered, ExportData.DisplayData, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportFullEvaluationsSummaryDataToXml_Click(sender As Object, e As System.EventArgs) Handles LNBexportFullEvaluationsSummaryDataToXml.Click
        ExportEvaluations(SummaryType.Evaluations, ItemsToExport.All, ExportData.FulldataToAnalyze, Helpers.Export.ExportFileType.xml)
    End Sub

    Private Sub LNBexportAllEvaluationsSummaryDataToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllEvaluationsSummaryDataToCsv.Click
        ExportEvaluations(SummaryType.Evaluations, ItemsToExport.All, ExportData.Fulldata, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportAllCommitteeSummaryToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportAllCommitteeSummaryToCsv.Click
        ExportEvaluations(SummaryType.Committee, ItemsToExport.All, ExportData.DisplayData, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportFilteredCommitteeSummaryToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportFilteredCommitteeSummaryToCsv.Click
        ExportEvaluations(SummaryType.Committee, ItemsToExport.Filtered, ExportData.DisplayData, Helpers.Export.ExportFileType.csv)
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
            fileName = CurrentPresenter.GetFileName(fileName, type, items, data) & "." & fileType.ToString
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

    'Private Function CalculateColspan(count As Integer) As Integer
    '    'If count = 5 Then
    '    '    Return 0
    '    'ElseIf count < 5 Then
    '    '    Return 5 - count
    '    'Else
    '    Dim d As Integer = count / 5
    '    Dim m As Integer = count Mod 5
    '    If d = 0 Then
    '        Return IIf(m = 0, 0, 5 - m)
    '    Else
    '        Return (d * 5) - count - 1
    '    End If
    '    ' End If

    '    'Dim d As Integer = count / 5
    '    'Dim m As Integer = count Mod 5
    '    'If d = 0 Then
    '    '    Return IIf(m = 0, 0, 5 - m)
    '    'Else
    '    '    Return ((d + 1) * 5) - m - count
    '    'End If
    'End Function
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

End Class